using System;
using System.Collections.Generic;
using TugOfWar.Battle;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Логічний загін (D-001): тип, сторона, здоров'я, позиція на лінії і стан (GDD 3.2).
    /// Звичайний C#-клас; ціль і кадри йому подає SquadSimulation.
    /// </summary>
    public sealed class Squad
    {
        /// <summary>Запас на похибку float при порівнянні відстаней.</summary>
        private const float RangeEpsilon = 1e-4f;

        public event Action StateChanged;
        /// <summary>Скільки солдатів загинуло від одного удару.</summary>
        public event Action<int> SoldiersDied;
        public event Action Destroyed;

        public SquadDefinition Definition { get; }
        public Side Side { get; }
        public SquadHealth Health { get; }

        /// <summary>Позиція на лінії (Y у світі).</summary>
        public float Y { get; private set; }

        /// <summary>+1, якщо загін іде вгору (гравець), −1, якщо вниз (ворог).</summary>
        public float Forward { get; }

        public SquadState State { get; private set; } = SquadState.Marching;

        /// <summary>Ворожий загін, з яким б'ємось; null, якщо в радіусі нікого немає.</summary>
        public Squad Target { get; private set; }

        public bool IsAttackingBase => State == SquadState.Fighting && Target == null;

        public bool IsEnemyBaseInRange =>
            (_enemyBase.FrontY - Y) * Forward <= Definition.AttackRange + RangeEpsilon;

        private readonly Base _enemyBase;
        private float _clashTimer;
        private float _attackTimer;

        public Squad(SquadDefinition definition, Side side, Lane lane, Base enemyBase)
        {
            Definition = definition;
            Side = side;
            Health = new SquadHealth(definition.SoldierCount, definition.SoldierHp);
            Forward = lane.Forward(side);
            Y = lane.SpawnY(side);
            _enemyBase = enemyBase;
        }

        public bool IsInRange(Squad other) =>
            Math.Abs(other.Y - Y) <= Definition.AttackRange + RangeEpsilon;

        public void SetTarget(Squad target) => Target = target;

        /// <summary>Один крок стану. Удари не застосовує, а додає в hits (див. Hit).</summary>
        public void Advance(float deltaTime, List<Hit> hits)
        {
            switch (State)
            {
                case SquadState.Marching:
                    if (Target != null) StartClash();
                    else if (IsEnemyBaseInRange) StartFighting();
                    else March(deltaTime);
                    break;

                case SquadState.Clashing:
                    if (Target == null)
                    {
                        SetState(SquadState.Marching);
                        break;
                    }
                    _clashTimer += deltaTime;
                    if (_clashTimer >= Definition.ClashDuration) StartFighting();
                    break;

                case SquadState.Fighting:
                    // Перемога: ворогів у радіусі немає і база не поруч, тож марш далі.
                    if (Target == null && !IsEnemyBaseInRange)
                    {
                        SetState(SquadState.Marching);
                        break;
                    }
                    _attackTimer += deltaTime;
                    while (_attackTimer >= Definition.AttackInterval)
                    {
                        _attackTimer -= Definition.AttackInterval;
                        hits.Add(CreateHit());
                    }
                    break;
            }
        }

        /// <summary>Отримати шкоду (броню вже враховано тим, хто б'є).</summary>
        public void TakeHit(float damage)
        {
            if (State == SquadState.Destroyed) return;

            int killed = Health.ApplyDamage(damage);
            if (killed > 0) SoldiersDied?.Invoke(killed);

            if (Health.IsDead)
            {
                SetState(SquadState.Destroyed);
                Destroyed?.Invoke();
            }
        }

        private void March(float deltaTime)
        {
            // Не заходимо далі дистанції атаки від переднього краю ворожої бази.
            float stopY = _enemyBase.FrontY - Forward * Definition.AttackRange;
            Y += Forward * Definition.MoveSpeed * deltaTime;
            if ((stopY - Y) * Forward < 0f) Y = stopY;
        }

        private void StartClash()
        {
            _clashTimer = 0f;
            SetState(SquadState.Clashing);
        }

        private void StartFighting()
        {
            _attackTimer = 0f;
            SetState(SquadState.Fighting);
        }

        private Hit CreateHit()
        {
            // D-002: шкода залежить від живих солдатів. У прототипі лише ближня атака.
            float damage = CombatFormulas.Damage(
                Definition.MeleeDamage, Definition.BaseDamageShare, Health.Alive, Health.Total);

            if (Target == null) return new Hit(_enemyBase, damage); // у бази броні немає (D-011)
            return new Hit(Target, CombatFormulas.AfterArmor(damage, Target.Definition.Armor));
        }

        private void SetState(SquadState state)
        {
            if (State == state) return;
            State = state;
            StateChanged?.Invoke();
        }
    }
}
