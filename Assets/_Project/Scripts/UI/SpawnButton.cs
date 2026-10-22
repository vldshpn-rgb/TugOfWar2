using TugOfWar.Squads;
using UnityEngine;
using UnityEngine.UI;

namespace TugOfWar.UI
{
    /// <summary>
    /// Кнопка виклику одного типу загону. Передає намір гравця в SquadSummoner
    /// і показує, чи вистачає їжі. Сама загони не створює.
    /// </summary>
    public sealed class SpawnButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Text label;

        private SquadSummoner _summoner;
        private SquadDefinition _definition;

        public void Init(SquadSummoner summoner, SquadDefinition definition)
        {
            _summoner = summoner;
            _definition = definition;
            label.text = $"{definition.DisplayName}\n{definition.FoodCost}";
            button.onClick.AddListener(OnClick);
            _summoner.Food.Changed += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (_summoner != null) _summoner.Food.Changed -= Refresh;
        }

        private void OnClick() => _summoner.TrySummon(_definition);

        private void Refresh() => button.interactable = _summoner.CanSummon(_definition);
    }
}
