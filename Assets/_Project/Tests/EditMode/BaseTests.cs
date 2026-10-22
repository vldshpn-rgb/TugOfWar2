using NUnit.Framework;
using TugOfWar.Battle;
using UnityEngine;

namespace TugOfWar.Tests
{
    public class BaseTests
    {
        private GameObject _go;
        private BaseDefinition _definition;
        private Base _base;

        [SetUp]
        public void SetUp()
        {
            _definition = ScriptableObject.CreateInstance<BaseDefinition>(); // MaxHp = 500 за замовчуванням
            _go = new GameObject("Base");
            _base = _go.AddComponent<Base>();
            _base.Init(_definition, Side.Player);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_go);
            Object.DestroyImmediate(_definition);
        }

        [Test]
        public void Init_FullHp()
        {
            Assert.That(_base.CurrentHp, Is.EqualTo(_definition.MaxHp));
            Assert.That(_base.HpFraction, Is.EqualTo(1f));
            Assert.That(_base.IsDestroyed, Is.False);
        }

        [Test]
        public void TakeDamage_ReducesHp_RaisesHpChanged()
        {
            int changed = 0;
            _base.HpChanged += () => changed++;

            _base.TakeDamage(100f);

            Assert.That(_base.CurrentHp, Is.EqualTo(_definition.MaxHp - 100f));
            Assert.That(changed, Is.EqualTo(1));
        }

        [Test]
        public void TakeDamage_Lethal_RaisesDestroyedOnce()
        {
            int destroyed = 0;
            _base.Destroyed += () => destroyed++;

            _base.TakeDamage(10000f);
            _base.TakeDamage(10f);

            Assert.That(_base.CurrentHp, Is.EqualTo(0f));
            Assert.That(_base.IsDestroyed, Is.True);
            Assert.That(destroyed, Is.EqualTo(1));
        }
    }
}
