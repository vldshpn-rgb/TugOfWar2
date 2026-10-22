using System;
using TugOfWar.Battle;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Логічний загін (D-001): тип, сторона, здоров'я, позиція на лінії і стан.
    /// Звичайний C#-клас; кадри йому подає SquadSystem через Tick.
    /// </summary>
    public sealed class Squad
    {
        public event Action StateChanged;

        public SquadDefinition Definition { get; }
        public Side Side { get; }
        public SquadHealth Health { get; }

        /// <summary>Позиція на лінії (Y у світі).</summary>
        public float Y { get; private set; }

        /// <summary>+1, якщо загін іде вгору (гравець), −1, якщо вниз (ворог).</summary>
        public float Forward { get; }

        public SquadState State { get; private set; } = SquadState.Marching;

        private readonly Base _enemyBase;

        public Squad(SquadDefinition definition, Side side, Lane lane, Base enemyBase)
        {
            Definition = definition;
            Side = side;
            Health = new SquadHealth(definition.SoldierCount, definition.SoldierHp);
            Forward = lane.Forward(side);
            Y = lane.SpawnY(side);
            _enemyBase = enemyBase;
        }

        public void Tick(float deltaTime)
        {
            if (State == SquadState.Marching) March(deltaTime);
        }

        private void March(float deltaTime)
        {
            // Зупиняємось на дистанції атаки від переднього краю ворожої бази.
            float stopY = _enemyBase.FrontY - Forward * Definition.AttackRange;
            Y += Forward * Definition.MoveSpeed * deltaTime;

            if ((stopY - Y) * Forward <= 0f)
            {
                Y = stopY;
                SetState(SquadState.Fighting);
            }
        }

        private void SetState(SquadState state)
        {
            if (State == state) return;
            State = state;
            StateChanged?.Invoke();
        }
    }
}
