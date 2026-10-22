using TugOfWar.Economy;
using TugOfWar.Squads;
using TugOfWar.UI;
using UnityEngine;

namespace TugOfWar.Battle
{
    /// <summary>
    /// Composition root сцени бою: тримає посилання на системи (призначені в інспекторі)
    /// і налаштовує їх з BattleConfig (Architecture 4).
    /// </summary>
    public sealed class BattleController : MonoBehaviour
    {
        [SerializeField] private BattleConfig config;
        [SerializeField] private Base playerBase;
        [SerializeField] private Base enemyBase;
        [SerializeField] private SquadSpawner squadSpawner;

        [Header("UI")]
        [SerializeField] private FoodCounter foodCounter;
        [SerializeField] private SummonPanel summonPanel;

        public Lane Lane { get; private set; }
        public SquadSummoner PlayerSummoner { get; private set; }
        public SquadSummoner EnemySummoner { get; private set; }

        private void Awake()
        {
            Lane = new Lane(config.LaneLength, config.SpawnOffset);
            SetupBase(playerBase, config.PlayerBase, Side.Player);
            SetupBase(enemyBase, config.EnemyBase, Side.Enemy);
            squadSpawner.Init(Lane, playerBase, enemyBase);

            PlayerSummoner = CreateSummoner(Side.Player);
            EnemySummoner = CreateSummoner(Side.Enemy);

            foodCounter.Init(PlayerSummoner.Food);
            summonPanel.Init(PlayerSummoner, config.PlayerSquads);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            PlayerSummoner.Food.Tick(deltaTime);
            EnemySummoner.Food.Tick(deltaTime);
        }

        private void SetupBase(Base target, BaseDefinition definition, Side side)
        {
            float y = Lane.BaseY(side);
            target.transform.position = new Vector3(0f, y, 0f);
            target.Init(definition, side, y + Lane.Forward(side) * definition.HalfDepth);
        }

        private SquadSummoner CreateSummoner(Side side) =>
            new(side, new FoodBank(config.StartFood, config.FoodPerSecond), squadSpawner);
    }
}
