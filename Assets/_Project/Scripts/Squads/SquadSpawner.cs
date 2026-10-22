using TugOfWar.Battle;
using UnityEngine;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Створює загін: логіку (Squad) реєструє в SquadSystem, візуал (SquadView) ставить у сцену.
    /// </summary>
    public sealed class SquadSpawner : MonoBehaviour
    {
        [SerializeField] private SquadSystem squadSystem;
        [SerializeField] private SquadView viewPrefab;

        [Tooltip("Тимчасово: загін для кнопок у контекстному меню компонента. Приберемо з появою кнопки виклику.")]
        [SerializeField] private SquadDefinition debugSquad;

        private Lane _lane;
        private Base _playerBase;
        private Base _enemyBase;

        public void Init(Lane lane, Base playerBase, Base enemyBase)
        {
            _lane = lane;
            _playerBase = playerBase;
            _enemyBase = enemyBase;
        }

        public Squad Spawn(SquadDefinition definition, Side side)
        {
            var enemyBase = side == Side.Player ? _enemyBase : _playerBase;
            var squad = new Squad(definition, side, _lane, enemyBase);
            squadSystem.Add(squad);

            var view = Instantiate(viewPrefab);
            view.name = $"{definition.name}_{side}";
            view.Bind(squad);
            return squad;
        }

        [ContextMenu("Debug: Spawn Player Squad")]
        private void DebugSpawnPlayer() => Spawn(debugSquad, Side.Player);

        [ContextMenu("Debug: Spawn Enemy Squad")]
        private void DebugSpawnEnemy() => Spawn(debugSquad, Side.Enemy);
    }
}
