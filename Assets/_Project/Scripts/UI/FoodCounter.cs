using TugOfWar.Economy;
using UnityEngine;
using UnityEngine.UI;

namespace TugOfWar.UI
{
    /// <summary>Показує запас їжі гравця. Лише читає FoodBank.</summary>
    public sealed class FoodCounter : MonoBehaviour
    {
        [SerializeField] private Text label;

        private FoodBank _food;
        private int _shown = -1;

        public void Init(FoodBank food)
        {
            _food = food;
            _food.Changed += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (_food != null) _food.Changed -= Refresh;
        }

        private void Refresh()
        {
            // Текст оновлюємо лише коли змінилось ціле число, а не щокадру.
            int amount = Mathf.FloorToInt(_food.Amount);
            if (amount == _shown) return;
            _shown = amount;
            label.text = $"Їжа: {amount}";
        }
    }
}
