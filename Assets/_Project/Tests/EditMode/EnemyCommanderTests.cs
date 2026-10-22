using System.Collections.Generic;
using NUnit.Framework;
using TugOfWar.Battle;
using TugOfWar.Economy;
using TugOfWar.Squads;
using UnityEditor;
using UnityEngine;

namespace TugOfWar.Tests
{
    public class EnemyCommanderTests
    {
        private SquadDefinition _cheap;     // ціна 5
        private SquadDefinition _expensive; // ціна 7

        [SetUp]
        public void SetUp()
        {
            _cheap = ScriptableObject.CreateInstance<SquadDefinition>();
            _expensive = ScriptableObject.CreateInstance<SquadDefinition>();
            var so = new SerializedObject(_expensive);
            so.FindProperty("foodCost").intValue = 7;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_cheap);
            Object.DestroyImmediate(_expensive);
        }

        [Test]
        public void NotEnoughFood_Waits()
        {
            var factory = new FakeSquadFactory();
            var commander = Create(factory, new FoodBank(4f, 0f), _cheap);

            commander.Tick();

            Assert.That(factory.Spawned, Is.Empty);
            Assert.That(commander.Next, Is.SameAs(_cheap));
        }

        [Test]
        public void EnoughFood_SummonsOnEnemySide_AndSpendsFood()
        {
            var factory = new FakeSquadFactory();
            var food = new FoodBank(5f, 0f);
            var commander = Create(factory, food, _cheap);

            commander.Tick();

            Assert.That(factory.Spawned, Has.Count.EqualTo(1));
            Assert.That(factory.Spawned[0].side, Is.EqualTo(Side.Enemy));
            Assert.That(food.Amount, Is.EqualTo(0f));
        }

        [Test]
        public void SummonsOncePerTick_EvenWithLotsOfFood()
        {
            var factory = new FakeSquadFactory();
            var commander = Create(factory, new FoodBank(100f, 0f), _cheap);

            commander.Tick();
            commander.Tick();

            Assert.That(factory.Spawned, Has.Count.EqualTo(2));
        }

        [Test]
        public void WaitsForChosenSquad_DoesNotSwitchToCheaperOne()
        {
            var factory = new FakeSquadFactory();
            var food = new FoodBank(6f, 0f); // на дешевий вистачає, на дорогий ні
            var commander = Create(factory, food, _expensive, _cheap);

            // Шукаємо зерно, з яким першим обрано дорогий загін.
            for (int seed = 0; commander.Next != _expensive; seed++)
                commander = Create(factory, food, seed, _expensive, _cheap);

            commander.Tick();

            Assert.That(factory.Spawned, Is.Empty, "ШІ чекає їжу на обраний загін (D-012)");
        }

        [Test]
        public void RandomChoice_UsesAllTypes()
        {
            var factory = new FakeSquadFactory();
            var commander = Create(factory, new FoodBank(10000f, 0f), _cheap, _expensive);

            for (int i = 0; i < 50; i++) commander.Tick();

            var spawned = new HashSet<SquadDefinition>();
            foreach (var entry in factory.Spawned) spawned.Add(entry.definition);
            Assert.That(spawned, Is.EquivalentTo(new[] { _cheap, _expensive }));
        }

        private static EnemyCommander Create(FakeSquadFactory factory, FoodBank food, params SquadDefinition[] squads) =>
            Create(factory, food, 42, squads);

        private static EnemyCommander Create(FakeSquadFactory factory, FoodBank food, int seed, params SquadDefinition[] squads) =>
            new(new SquadSummoner(Side.Enemy, food, factory), squads, new System.Random(seed));
    }
}
