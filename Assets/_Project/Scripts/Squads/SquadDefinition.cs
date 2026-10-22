using UnityEngine;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Тип загону (D-004): піхота, лучники тощо відрізняються лише значеннями цих полів.
    /// Під час гри лише читається (Architecture 3.5).
    /// </summary>
    [CreateAssetMenu(menuName = "TugOfWar/Squad Definition", fileName = "SquadDefinition")]
    public sealed class SquadDefinition : ScriptableObject
    {
        [Header("Виклик")]
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField, Min(0)] private int foodCost = 5;

        [Header("Солдати")]
        [Tooltip("«Всього» у формулі D-002.")]
        [SerializeField, Min(1)] private int soldierCount = 10;
        [SerializeField, Min(0.01f)] private float soldierHp = 10f;
        [Tooltip("Лише візуал солдата (D-001).")]
        [SerializeField] private GameObject soldierPrefab;

        [Header("Бій")]
        [SerializeField, Min(0f)] private float meleeDamage = 14f;
        [Tooltip("0 для ближніх загонів.")]
        [SerializeField, Min(0f)] private float rangedDamage;
        [Tooltip("D-002: частка шкоди, що не залежить від кількості живих.")]
        [SerializeField, Range(0f, 1f)] private float baseDamageShare = 0.5f;
        [Tooltip("D-008: частка шкоди, яку зрізає броня.")]
        [SerializeField, Range(0f, 0.8f)] private float armor;
        [Tooltip("Відстань між центрами загонів, з якої загін атакує (світові одиниці).")]
        [SerializeField, Min(0f)] private float attackRange = 1f;
        [SerializeField, Min(0.05f)] private float attackInterval = 1f;
        [Tooltip("GDD 3.2: фаза зіткнення перед боєм, секунди.")]
        [SerializeField, Min(0f)] private float clashDuration = 0.5f;

        [Header("Рух")]
        [Tooltip("Світових одиниць за секунду.")]
        [SerializeField, Min(0f)] private float moveSpeed = 1f;

        [Header("Нагорода")]
        [SerializeField, Range(0f, 1f)] private float coinDropChance;

        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public int FoodCost => foodCost;
        public int SoldierCount => soldierCount;
        public float SoldierHp => soldierHp;
        public GameObject SoldierPrefab => soldierPrefab;
        public float MeleeDamage => meleeDamage;
        public float RangedDamage => rangedDamage;
        public float BaseDamageShare => baseDamageShare;
        public float Armor => armor;
        public float AttackRange => attackRange;
        public float AttackInterval => attackInterval;
        public float ClashDuration => clashDuration;
        public float MoveSpeed => moveSpeed;
        public float CoinDropChance => coinDropChance;
        public bool IsRanged => rangedDamage > 0f;
    }
}
