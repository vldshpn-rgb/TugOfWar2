using TugOfWar.Battle;
using UnityEngine;

namespace TugOfWar.UI
{
    /// <summary>
    /// Смужка HP бази у світі гри. Лише читає Base і підписується на її подію.
    /// Fill це дочірній спрайт шириною 1 одиниця з центром у (0, 0) батька.
    /// </summary>
    public sealed class BaseHpBar : MonoBehaviour
    {
        [SerializeField] private Base target;
        [SerializeField] private Transform fill;

        private void OnEnable()
        {
            target.HpChanged += Refresh;
            Refresh();
        }

        private void OnDisable() => target.HpChanged -= Refresh;

        private void Refresh()
        {
            float fraction = target.HpFraction;
            fill.localScale = new Vector3(fraction, 1f, 1f);
            // Тримаємо смужку притиснутою до лівого краю.
            fill.localPosition = new Vector3((fraction - 1f) / 2f, 0f, 0f);
        }
    }
}
