using System;
using NUnit.Framework;
using TugOfWar.Battle;
using TugOfWar.Economy;
using TugOfWar.Squads;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TugOfWar.Tests
{
    public class FoodAndSummonTests
    {
        private const float Tolerance = 1e-4f;

        private SquadDefinition _infantry; // ціна 5 за замовчуванням

        [SetUp]
        public void SetUp() => _infantry = ScriptableObject.CreateInstance<SquadDefinition>();

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(_infantry);

        [Test]
        public void Food_Tick_AddsPerSecond_WithoutCap()
        {
            var food = new FoodBank(10f, 1f);

            food.Tick(2.5f);
            Assert.That(food.Amount, Is.EqualTo(12.5f).Within(Tolerance));

            food.Tick(1000f);
            Assert.That(food.Amount, Is.EqualTo(1012.5f).Within(Tolerance), "максимуму немає (D-013)");
        }

        [Test]
        public void Food_TrySpend_OnlyWhenEnough()
        {
            var food = new FoodBank(7f, 1f);

            Assert.That(food.TrySpend(5), Is.True);
            Assert.That(food.Amount, Is.EqualTo(2f).Within(Tolerance));
            Assert.That(food.TrySpend(5), Is.False);
            Assert.That(food.Amount, Is.EqualTo(2f).Within(Tolerance));
        }

        [Test]
        public void Food_Changed_RaisedOnTickAndSpend()
        {
            var food = new FoodBank(10f, 1f);
            int changed = 0;
            food.Changed += () => changed++;

            food.Tick(0.1f);
            food.TrySpend(5);
            food.TrySpend(100);

            Assert.That(changed, Is.EqualTo(2));
        }

        [Test]
        public void Food_NegativeArguments_Throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new FoodBank(-1f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new FoodBank(1f, -1f));
        }

        [Test]
        public void Summon_EnoughFood_SpendsAndSpawnsOnOwnSide()
        {
            var factory = new FakeSquadFactory();
            var summoner = new SquadSummoner(Side.Enemy, new FoodBank(10f, 1f), factory);

            Assert.That(summoner.TrySummon(_infantry), Is.True);

            Assert.That(summoner.Food.Amount, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(factory.Spawned, Has.Count.EqualTo(1));
            Assert.That(factory.Spawned[0].side, Is.EqualTo(Side.Enemy));
        }

        [Test]
        public void Summon_NotEnoughFood_DoesNothing()
        {
            var factory = new FakeSquadFactory();
            var summoner = new SquadSummoner(Side.Player, new FoodBank(4f, 1f), factory);

            Assert.That(summoner.CanSummon(_infantry), Is.False);
            Assert.That(summoner.TrySummon(_infantry), Is.False);

            Assert.That(summoner.Food.Amount, Is.EqualTo(4f).Within(Tolerance));
            Assert.That(factory.Spawned, Is.Empty);
        }
    }
}
