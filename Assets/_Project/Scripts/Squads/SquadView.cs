using System.Collections.Generic;
using TugOfWar.Battle;
using UnityEngine;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Візуал загону: розставляє солдатів строєм, рухається за позицією Squad,
    /// ховає загиблих солдатів. Логіку не змінює (D-001).
    /// </summary>
    public sealed class SquadView : MonoBehaviour
    {
        [Header("Стрій")]
        [SerializeField, Min(1)] private int columns = 5;
        [Tooltip("Відстань між солдатами у строю, світові одиниці.")]
        [SerializeField, Min(0.01f)] private float spacing = 0.3f;

        [Header("Кольори сторін")]
        [SerializeField] private Color playerColor = new(0.3f, 0.55f, 1f);
        [SerializeField] private Color enemyColor = new(1f, 0.35f, 0.3f);

        private readonly List<SoldierView> _soldiers = new();
        private Squad _squad;
        private int _nextToDie;

        public void Bind(Squad squad)
        {
            _squad = squad;
            _squad.SoldiersDied += OnSoldiersDied;
            _squad.Destroyed += OnDestroyed;
            SpawnSoldiers();
            SyncPosition();
        }

        private void OnDestroy()
        {
            if (_squad == null) return;
            _squad.SoldiersDied -= OnSoldiersDied;
            _squad.Destroyed -= OnDestroyed;
        }

        private void LateUpdate()
        {
            if (_squad != null) SyncPosition();
        }

        private void SyncPosition() => transform.position = new Vector3(0f, _squad.Y, 0f);

        private void OnSoldiersDied(int count)
        {
            // Першими гинуть передні ряди. Ефект смерті додамо разом з іншими ефектами (Етап 2).
            for (int i = 0; i < count && _nextToDie < _soldiers.Count; i++)
                _soldiers[_nextToDie++].gameObject.SetActive(false);
        }

        private void OnDestroyed() => Destroy(gameObject);

        private void SpawnSoldiers()
        {
            var definition = _squad.Definition;
            int total = definition.SoldierCount;
            int rows = Mathf.CeilToInt(total / (float)columns);
            Color color = _squad.Side == Side.Player ? playerColor : enemyColor;

            for (int i = 0; i < total; i++)
            {
                int row = i / columns;
                int column = i % columns;
                int inThisRow = Mathf.Min(columns, total - row * columns);

                // Ряд 0 стоїть першим у напрямку руху.
                float x = (column - (inThisRow - 1) / 2f) * spacing;
                float y = ((rows - 1) / 2f - row) * spacing * _squad.Forward;

                var soldier = Instantiate(definition.SoldierPrefab, transform).GetComponent<SoldierView>();
                soldier.transform.localPosition = new Vector3(x, y, 0f);
                soldier.SetColor(color);
                _soldiers.Add(soldier);
            }
        }
    }
}
