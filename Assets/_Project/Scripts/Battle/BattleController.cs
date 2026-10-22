using TugOfWar.Economy;
using TugOfWar.Squads;
using TugOfWar.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TugOfWar.Battle
{
    /// <summary>
    /// Composition root сцени бою: тримає посилання на системи (призначені в інспекторі),
    /// налаштовує їх з BattleConfig і зупиняє бій, коли він закінчився (Architecture 4).
    /// </summary>
    public sealed class BattleController : MonoBehaviour
    {
        [SerializeField] private BattleConfig config;
        [SerializeField] private Base playerBase;
        [SerializeField] private Base enemyBase;
        [SerializeField] private SquadSpawner squadSpawner;
        [SerializeField] private SquadSystem squadSystem;

        [Header("UI")]
        [SerializeField] private FoodCounter foodCounter;
        [SerializeField] private SummonPanel summonPanel;
        [SerializeField] private BattleResultScreen resultScreen;

        public Lane Lane { get; private set; }
        public SquadSummoner PlayerSummoner { get; private set; }
        public SquadSummoner EnemySummoner { get; private set; }
        public BattleOutcome Outcome { get; private set; }

        private EnemyCommander _enemyCommander;

        private void Awake()
        {
            Lane = new Lane(config.LaneLength, config.SpawnOffset);
            SetupBase(playerBase, config.PlayerBase, Side.Player);
            SetupBase(enemyBase, config.EnemyBase, Side.Enemy);
            squadSpawner.Init(Lane, playerBase, enemyBase);

            PlayerSummoner = CreateSummoner(Side.Player);
            EnemySummoner = CreateSummoner(Side.Enemy);
            _enemyCommander = new EnemyCommander(EnemySummoner, config.EnemySquads, new System.Random());

            Outcome = new BattleOutcome(playerBase, enemyBase);
            Outcome.Ended += OnBattleEnded;

            foodCounter.Init(PlayerSummoner.Food);
            summonPanel.Init(PlayerSummoner, config.PlayerSquads);
            resultScreen.Init(Outcome, Restart);
        }

        private void OnDestroy()
        {
            if (Outcome == null) return;
            Outcome.Ended -= OnBattleEnded;
            Outcome.Dispose();
        }

        private void Update()
        {
            if (Outcome.IsOver) return;

            float deltaTime = Time.deltaTime;
            PlayerSummoner.Food.Tick(deltaTime);
            EnemySummoner.Food.Tick(deltaTime);
            _enemyCommander.Tick();
        }

        private void OnBattleEnded(Side winner)
        {
            // Бій завмирає: загони зупиняються, їжа і ШІ теж (див. Update).
            squadSystem.enabled = false;
            summonPanel.gameObject.SetActive(false);
        }

        private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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
