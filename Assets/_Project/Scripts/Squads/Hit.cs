using TugOfWar.Battle;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Удар, який загін вирішив завдати в цьому кроці симуляції. Застосовується пізніше,
    /// разом з усіма іншими ударами кроку, щоб обидві сторони били одночасно.
    /// Ціль або загін, або база.
    /// </summary>
    public readonly struct Hit
    {
        public readonly Squad TargetSquad;
        public readonly Base TargetBase;
        public readonly float Damage;

        public Hit(Squad targetSquad, float damage)
        {
            TargetSquad = targetSquad;
            TargetBase = null;
            Damage = damage;
        }

        public Hit(Base targetBase, float damage)
        {
            TargetSquad = null;
            TargetBase = targetBase;
            Damage = damage;
        }

        public void Apply()
        {
            if (TargetSquad != null) TargetSquad.TakeHit(Damage);
            else TargetBase.TakeDamage(Damage);
        }
    }
}
