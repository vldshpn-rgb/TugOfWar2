using System.Collections.Generic;
using TugOfWar.Squads;
using UnityEngine;

namespace TugOfWar.Battle
{
    /// <summary>
    /// Налаштування бою (Architecture 3.3). Поля для слотів і комбо (D-007) додамо на Етапі 2.
    /// </summary>
    [CreateAssetMenu(menuName = "TugOfWar/Battle Config", fileName = "BattleConfig")]
    public sealed class BattleConfig : ScriptableObject
    {
        [Header("Лінія")]
        [Tooltip("Відстань між базами, світові одиниці.")]
        [SerializeField, Min(1f)] private float laneLength = 16f;
        [Tooltip("Наскільки попереду своєї бази з'являються загони.")]
        [SerializeField, Min(0f)] private float spawnOffset = 1.5f;

        [Header("Їжа")]
        [SerializeField, Min(0f)] private float startFood = 10f;
        [SerializeField, Min(0f)] private float foodPerSecond = 1f;

        [Header("Бази")]
        [SerializeField] private BaseDefinition playerBase;
        [SerializeField] private BaseDefinition enemyBase;

        [Header("Загони")]
        [SerializeField] private List<SquadDefinition> playerSquads = new();
        [Tooltip("З цього списку ШІ випадково обирає наступний загін (D-012).")]
        [SerializeField] private List<SquadDefinition> enemySquads = new();

        public float LaneLength => laneLength;
        public float SpawnOffset => spawnOffset;
        public float StartFood => startFood;
        public float FoodPerSecond => foodPerSecond;
        public BaseDefinition PlayerBase => playerBase;
        public BaseDefinition EnemyBase => enemyBase;
        public IReadOnlyList<SquadDefinition> PlayerSquads => playerSquads;
        public IReadOnlyList<SquadDefinition> EnemySquads => enemySquads;
    }
}
