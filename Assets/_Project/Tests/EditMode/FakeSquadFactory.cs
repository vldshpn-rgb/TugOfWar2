using System.Collections.Generic;
using TugOfWar.Battle;
using TugOfWar.Squads;

namespace TugOfWar.Tests
{
    /// <summary>Фабрика-заглушка для тестів: лише запам'ятовує, що її попросили створити.</summary>
    public sealed class FakeSquadFactory : ISquadFactory
    {
        public readonly List<(SquadDefinition definition, Side side)> Spawned = new();

        public Squad Spawn(SquadDefinition definition, Side side)
        {
            Spawned.Add((definition, side));
            return null;
        }
    }
}
