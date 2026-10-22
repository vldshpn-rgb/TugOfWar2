using System;

namespace TugOfWar.Economy
{
    /// <summary>
    /// Їжа однієї сторони в бою (GDD 3.8): відновлюється з часом, витрачається на виклик.
    /// Максимуму немає (D-013).
    /// </summary>
    public sealed class FoodBank
    {
        public event Action Changed;

        public float Amount { get; private set; }
        public float PerSecond { get; }

        public FoodBank(float startAmount, float perSecond)
        {
            if (startAmount < 0f) throw new ArgumentOutOfRangeException(nameof(startAmount));
            if (perSecond < 0f) throw new ArgumentOutOfRangeException(nameof(perSecond));

            Amount = startAmount;
            PerSecond = perSecond;
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0f || PerSecond <= 0f) return;

            Amount += PerSecond * deltaTime;
            Changed?.Invoke();
        }

        public bool CanAfford(int cost) => Amount >= cost;

        public bool TrySpend(int cost)
        {
            if (!CanAfford(cost)) return false;

            Amount -= cost;
            Changed?.Invoke();
            return true;
        }
    }
}
