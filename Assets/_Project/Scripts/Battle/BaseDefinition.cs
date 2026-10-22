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
        [Tooltip("Половина глибини бази вздовж лінії: від центру бази до її переднього краю.")]
        [SerializeField, Min(0f)] private float halfDepth = 0.6f;

        public float MaxHp => maxHp;
        public float HalfDepth => halfDepth;
    }
}
