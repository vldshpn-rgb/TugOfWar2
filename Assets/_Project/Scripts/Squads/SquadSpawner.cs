using TugOfWar.Battle;
using UnityEngine;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Створює загін: логіку (Squad) реєструє в SquadSystem, візуал (SquadView) ставить у сцену.
    /// </summary>
    public sealed class SquadSpawner : MonoBehaviour, ISquadFactory
    {
        [SerializeField] private SquadSystem squadSystem;
        [SerializeField] private SquadView viewPrefab;

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
    }
}
