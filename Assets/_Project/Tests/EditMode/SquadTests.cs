using NUnit.Framework;
using TugOfWar.Battle;
using TugOfWar.Squads;
using UnityEngine;

namespace TugOfWar.Tests
{
    public class SquadTests
    {
        private const float Tolerance = 1e-4f;

        private Lane _lane;
        private SquadDefinition _infantry; // значення за замовчуванням: швидкість 1, радіус атаки 1
        private BaseDefinition _baseDefinition;
        private GameObject _playerBaseGo;
        private GameObject _enemyBaseGo;
        private Base _playerBase;
        private Base _enemyBase;

        [SetUp]
        public void SetUp()
        {
            _lane = new Lane(16f, 1.5f);
            _infantry = ScriptableObject.CreateInstance<SquadDefinition>();
            _baseDefinition = ScriptableObject.CreateInstance<BaseDefinition>();

            _playerBaseGo = new GameObject("PlayerBase");
            _playerBase = _playerBaseGo.AddComponent<Base>();
            _playerBase.Init(_baseDefinition, Side.Player, -7.4f);

            _enemyBaseGo = new GameObject("EnemyBase");
            _enemyBase = _enemyBaseGo.AddComponent<Base>();
            _enemyBase.Init(_baseDefinition, Side.Enemy, 7.4f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_playerBaseGo);
            Object.DestroyImmediate(_enemyBaseGo);
            Object.DestroyImmediate(_infantry);
            Object.DestroyImmediate(_baseDefinition);
        }

        [Test]
        public void New_StartsAtOwnSpawn_Marching_FullHealth()
        {
            var squad = new Squad(_infantry, Side.Player, _lane, _enemyBase);

            Assert.That(squad.Y, Is.EqualTo(-6.5f).Within(Tolerance));
            Assert.That(squad.State, Is.EqualTo(SquadState.Marching));
            Assert.That(squad.Health.Alive, Is.EqualTo(_infantry.SoldierCount));
        }

        [Test]
        public void Tick_PlayerMarchesUp_EnemyMarchesDown()
        {
            var player = new Squad(_infantry, Side.Player, _lane, _enemyBase);
            var enemy = new Squad(_infantry, Side.Enemy, _lane, _playerBase);

            player.Tick(2f);
            enemy.Tick(2f);

            Assert.That(player.Y, Is.EqualTo(-4.5f).Within(Tolerance));
            Assert.That(enemy.Y, Is.EqualTo(4.5f).Within(Tolerance));
        }

        [Test]
        public void Tick_ReachesEnemyBase_StopsAtAttackRange_AndFights()
        {
            var squad = new Squad(_infantry, Side.Player, _lane, _enemyBase);
            int stateChanges = 0;
            squad.StateChanged += () => stateChanges++;

            squad.Tick(100f);

            // Передній край ворожої бази 7.4, радіус атаки 1.
            Assert.That(squad.Y, Is.EqualTo(6.4f).Within(Tolerance));
            Assert.That(squad.State, Is.EqualTo(SquadState.Fighting));
            Assert.That(stateChanges, Is.EqualTo(1));
        }
    }
}
