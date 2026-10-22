using System;
using UnityEngine;

namespace TugOfWar.Battle
{
    /// <summary>
    /// База сторони з HP (D-010). Коли HP доходить до нуля, повідомляє Destroyed.
    /// </summary>
    public sealed class Base : MonoBehaviour
    {
        public event Action HpChanged;
        public event Action Destroyed;

        public Side Side { get; private set; }
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public bool IsDestroyed => CurrentHp <= 0f;
        public float HpFraction => MaxHp > 0f ? CurrentHp / MaxHp : 0f;

        public void Init(BaseDefinition definition, Side side)
        {
            Side = side;
            MaxHp = definition.MaxHp;
            CurrentHp = MaxHp;
            HpChanged?.Invoke();
        }

        public void TakeDamage(float damage)
        {
            if (damage <= 0f || IsDestroyed) return;

            CurrentHp = Mathf.Max(0f, CurrentHp - damage);
            HpChanged?.Invoke();
            if (IsDestroyed) Destroyed?.Invoke();
        }
    }
}
