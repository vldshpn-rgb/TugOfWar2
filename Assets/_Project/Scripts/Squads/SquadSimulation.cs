using System.Collections.Generic;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Крок бою для всіх загонів (звичайний C#-клас, щоб бій можна було перевіряти тестами):
    /// 1) кожен загін обирає ціль: найближчий ворожий загін у радіусі (D-006);
    /// 2) кожен загін рухається або збирає свої удари;
    /// 3) усі удари застосовуються разом, тож сторони б'ють одночасно;
    /// 4) знищені загони прибираються.
    /// Ціль обирається щокроку заново, тому новий ворог у радіусі одразу відволікає загін від бази (D-011).
    /// </summary>
    public sealed class SquadSimulation
    {
        private readonly List<Squad> _squads = new();
        private readonly List<Hit> _hits = new();

        public IReadOnlyList<Squad> Squads => _squads;

        public void Add(Squad squad) => _squads.Add(squad);

        public void Step(float deltaTime)
        {
            foreach (var squad in _squads)
                squad.SetTarget(FindTarget(squad));

            _hits.Clear();
            foreach (var squad in _squads)
                squad.Advance(deltaTime, _hits);

            foreach (var hit in _hits)
                hit.Apply();

            _squads.RemoveAll(IsDestroyed);
        }

        private Squad FindTarget(Squad squad)
        {
            Squad nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (var other in _squads)
            {
                if (other.Side == squad.Side || other.State == SquadState.Destroyed) continue;
                if (!squad.IsInRange(other)) continue;

                float distance = System.Math.Abs(other.Y - squad.Y);
                if (distance < nearestDistance)
                {
                    nearest = other;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        private static bool IsDestroyed(Squad squad) => squad.State == SquadState.Destroyed;
    }
}
