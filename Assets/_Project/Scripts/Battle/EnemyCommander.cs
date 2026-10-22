using System;
using System.Collections.Generic;
using TugOfWar.Squads;

namespace TugOfWar.Battle
{
    /// <summary>
    /// Ворожий ШІ прототипу (D-012): випадково обирає наступний тип загону, чекає, поки вистачить їжі,
    /// і викликає його через свій SquadSummoner, тобто за тими самими правилами, що й гравець.
    /// </summary>
    public sealed class EnemyCommander
    {
        private readonly SquadSummoner _summoner;
        private readonly IReadOnlyList<SquadDefinition> _squads;
        private readonly Random _random;

        /// <summary>Загін, на який ШІ зараз збирає їжу; null, якщо викликати нічого.</summary>
        public SquadDefinition Next { get; private set; }

        public EnemyCommander(SquadSummoner summoner, IReadOnlyList<SquadDefinition> squads, Random random)
        {
            _summoner = summoner;
            _squads = squads;
            _random = random;
            PickNext();
        }

        public void Tick()
        {
            if (Next == null) return;
            if (_summoner.TrySummon(Next)) PickNext();
        }

        private void PickNext() =>
            Next = _squads.Count > 0 ? _squads[_random.Next(_squads.Count)] : null;
    }
}
