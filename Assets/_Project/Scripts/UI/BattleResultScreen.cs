using System;
using TugOfWar.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace TugOfWar.UI
{
    /// <summary>
    /// Екран результату: з'являється, коли бій закінчено, і передає намір «зіграти ще раз».
    /// </summary>
    public sealed class BattleResultScreen : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text title;
        [SerializeField] private Button restartButton;

        [Header("Тексти")]
        [SerializeField] private string victoryText = "Перемога!";
        [SerializeField] private string defeatText = "Поразка";

        private BattleOutcome _outcome;
        private Action _restart;

        public void Init(BattleOutcome outcome, Action restart)
        {
            _outcome = outcome;
            _restart = restart;
            _outcome.Ended += Show;
            restartButton.onClick.AddListener(OnRestart);
            panel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_outcome != null) _outcome.Ended -= Show;
        }

        private void Show(Side winner)
        {
            title.text = winner == Side.Player ? victoryText : defeatText;
            panel.SetActive(true);
        }

        private void OnRestart() => _restart?.Invoke();
    }
}
