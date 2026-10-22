using TugOfWar.Battle;
using TugOfWar.Economy;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Виклик загону однієї сторони (прототип): списує їжу і створює загін.
    /// Без слотів, комбо і кулдауну (D-012); на Етапі 2 виросте в SummonQueue (D-007).
    /// Гравець (через UI) і ШІ викликають загони однаково, через свій SquadSummoner.
    /// </summary>
    public sealed class SquadSummoner
    {
        public Side Side { get; }
        public FoodBank Food { get; }

        private readonly ISquadFactory _factory;

        public SquadSummoner(Side side, FoodBank food, ISquadFactory factory)
        {
            Side = side;
            Food = food;
            _factory = factory;
        }

        public bool CanSummon(SquadDefinition definition) => Food.CanAfford(definition.FoodCost);

        public bool TrySummon(SquadDefinition definition)
        {
            if (!Food.TrySpend(definition.FoodCost)) return false;

            _factory.Spawn(definition, Side);
            return true;
        }
    }
}
