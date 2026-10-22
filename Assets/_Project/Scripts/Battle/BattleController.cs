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

        public Lane Lane { get; private set; }

        private void Awake()
        {
            Lane = new Lane(config.LaneLength, config.SpawnOffset);
            SetupBase(playerBase, config.PlayerBase, Side.Player);
            SetupBase(enemyBase, config.EnemyBase, Side.Enemy);
        }

        private void SetupBase(Base target, BaseDefinition definition, Side side)
        {
            target.transform.position = new Vector3(0f, Lane.BaseY(side), 0f);
            target.Init(definition, side);
        }
    }
}
