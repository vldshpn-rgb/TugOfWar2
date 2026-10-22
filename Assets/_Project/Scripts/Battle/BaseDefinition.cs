using UnityEngine;

namespace TugOfWar.Battle
{
    /// <summary>
    /// База сторони (D-010). Параметри захисту бази додамо разом із самим захистом (Етап 3).
    /// </summary>
    [CreateAssetMenu(menuName = "TugOfWar/Base Definition", fileName = "BaseDefinition")]
    public sealed class BaseDefinition : ScriptableObject
    {
        [SerializeField, Min(1f)] private float maxHp = 500f;

        public float MaxHp => maxHp;
    }
}
