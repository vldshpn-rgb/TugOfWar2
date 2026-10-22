using NUnit.Framework;
using TugOfWar.Battle;
using UnityEngine;

namespace TugOfWar.Tests
{
    public class BattleOutcomeTests
    {
        private BaseDefinition _definition;
        private GameObject _playerGo;
        private GameObject _enemyGo;
        private Base _playerBase;
        private Base _enemyBase;
        private BattleOutcome _outcome;

        [SetUp]
        public void SetUp()
        {
            _definition = ScriptableObject.CreateInstance<BaseDefinition>(); // HP 500
            _playerGo = new GameObject("PlayerBase");
            _playerBase = _playerGo.AddComponent<Base>();
            _playerBase.Init(_definition, Side.Player, -7.4f);
            _enemyGo = new GameObject("EnemyBase");
            _enemyBase = _enemyGo.AddComponent<Base>();
            _enemyBase.Init(_definition, Side.Enemy, 7.4f);
            _outcome = new BattleOutcome(_playerBase, _enemyBase);
        }

        [TearDown]
        public void TearDown()
        {
            _outcome.Dispose();
            Object.DestroyImmediate(_playerGo);
            Object.DestroyImmediate(_enemyGo);
            Object.DestroyImmediate(_definition);
        }

        [Test]
        public void Start_NotOver()
        {
            Assert.That(_outcome.IsOver, Is.False);
        }

        [Test]
        public void EnemyBaseDestroyed_PlayerWins()
        {
            Side? winner = null;
            _outcome.Ended += side => winner = side;

            _enemyBase.TakeDamage(1000f);

            Assert.That(_outcome.IsOver, Is.True);
            Assert.That(winner, Is.EqualTo(Side.Player));
        }

        [Test]
        public void PlayerBaseDestroyed_EnemyWins()
        {
            _playerBase.TakeDamage(1000f);

            Assert.That(_outcome.Winner, Is.EqualTo(Side.Enemy));
        }

        [Test]
        public void BothBasesDestroyed_EndsOnce_FirstWins()
        {
            int ended = 0;
            _outcome.Ended += _ => ended++;

            _enemyBase.TakeDamage(1000f);
            _playerBase.TakeDamage(1000f);

            Assert.That(ended, Is.EqualTo(1));
            Assert.That(_outcome.Winner, Is.EqualTo(Side.Player));
        }

        [Test]
        public void DamageWithoutDestroying_NotOver()
        {
            _enemyBase.TakeDamage(100f);

            Assert.That(_outcome.IsOver, Is.False);
        }
    }
}
