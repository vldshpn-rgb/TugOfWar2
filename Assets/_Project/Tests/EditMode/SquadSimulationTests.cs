using System;
using System.Collections.Generic;
using NUnit.Framework;
using TugOfWar.Battle;
using TugOfWar.Squads;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TugOfWar.Tests
{
    public class SquadSimulationTests
    {
        private const float Tolerance = 1e-3f;
        private const float Dt = 0.02f;

        private readonly List<Object> _created = new();
        private Lane _lane;
        private SquadDefinition _infantry; // значення за замовчуванням = стартова піхота з GDD 3.6
        private Base _playerBase;
        private Base _enemyBase;
        private SquadSimulation _simulation;

        [SetUp]
        public void SetUp()
        {
            _lane = new Lane(16f, 1.5f);
            _infantry = MakeInfantry(10);
            var baseDefinition = Track(ScriptableObject.CreateInstance<BaseDefinition>()); // HP 500
            _playerBase = MakeBase(baseDefinition, Side.Player, -7.4f);
            _enemyBase = MakeBase(baseDefinition, Side.Enemy, 7.4f);
            _simulation = new SquadSimulation();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var obj in _created) Object.DestroyImmediate(obj);
            _created.Clear();
        }

        [Test]
        public void New_StartsAtOwnSpawn_Marching_FullHealth()
        {
            var squad = Spawn(_infantry, Side.Player);

            Assert.That(squad.Y, Is.EqualTo(-6.5f).Within(Tolerance));
            Assert.That(squad.State, Is.EqualTo(SquadState.Marching));
            Assert.That(squad.Health.Alive, Is.EqualTo(10));
        }

        [Test]
        public void March_PlayerUp_EnemyDown()
        {
            var player = Spawn(_infantry, Side.Player);
            var enemy = Spawn(_infantry, Side.Enemy);

            Run(2f);

            Assert.That(player.Y, Is.EqualTo(-4.5f).Within(0.05f));
            Assert.That(enemy.Y, Is.EqualTo(4.5f).Within(0.05f));
        }

        [Test]
        public void Alone_StopsAtAttackRangeFromBase_AndHitsIt()
        {
            var squad = Spawn(_infantry, Side.Player);

            RunUntil(() => squad.IsAttackingBase, 30f);
            Assert.That(squad.Y, Is.EqualTo(6.4f).Within(Tolerance)); // край бази 7.4, радіус 1

            Run(1.1f); // з запасом: сума кроків по 0.02 у float трохи менша за 1.0
            Assert.That(_enemyBase.CurrentHp, Is.EqualTo(500f - 14f).Within(Tolerance)); // один повний удар
        }

        [Test]
        public void Clash_NoDamageDuringClashDuration()
        {
            var player = Spawn(_infantry, Side.Player);
            var enemy = Spawn(_infantry, Side.Enemy);

            RunUntil(() => player.State == SquadState.Clashing, 30f);
            Run(0.4f); // зіткнення триває 0.5 с

            Assert.That(player.State, Is.EqualTo(SquadState.Clashing));
            Assert.That(player.Health.CurrentHp, Is.EqualTo(100f));
            Assert.That(enemy.Health.CurrentHp, Is.EqualTo(100f));
        }

        [Test]
        public void OneOnOne_EqualSquads_FightLastsAbout10Seconds_BothDestroyed()
        {
            var player = Spawn(_infantry, Side.Player);
            var enemy = Spawn(_infantry, Side.Enemy);

            RunUntil(() => player.State == SquadState.Fighting, 30f);
            float fight = RunUntil(() => player.State == SquadState.Destroyed, 30f);

            Assert.That(fight, Is.InRange(9.5f, 10.5f));
            Assert.That(enemy.State, Is.EqualTo(SquadState.Destroyed), "удари одночасні, тож гинуть обидва");
            Assert.That(_simulation.Squads, Is.Empty);
        }

        [Test]
        public void AttackingBase_EnemyAppearsInRange_SwitchesToEnemy()
        {
            var player = Spawn(_infantry, Side.Player);
            RunUntil(() => player.IsAttackingBase, 30f);

            var enemy = Spawn(_infantry, Side.Enemy); // спавн ворога 6.5, поруч з атакувальником
            _simulation.Step(Dt);
            float baseHp = _enemyBase.CurrentHp;

            Assert.That(player.Target, Is.SameAs(enemy));
            Assert.That(player.IsAttackingBase, Is.False);

            Run(3f);
            Assert.That(_enemyBase.CurrentHp, Is.EqualTo(baseHp), "поки поруч ворог, база не отримує шкоди");
        }

        [Test]
        public void Winner_ResumesMarching()
        {
            var player = Spawn(MakeInfantry(20), Side.Player);
            var enemy = Spawn(_infantry, Side.Enemy);

            RunUntil(() => enemy.State == SquadState.Destroyed, 30f);
            _simulation.Step(Dt);

            Assert.That(player.State, Is.EqualTo(SquadState.Marching));
            Assert.That(player.Health.Alive, Is.GreaterThan(0));
        }

        [Test]
        public void TwoSquads_JoinOneFight_KillEnemyFaster()
        {
            var first = Spawn(_infantry, Side.Player);
            var second = Spawn(_infantry, Side.Player);
            var enemy = Spawn(_infantry, Side.Enemy);

            RunUntil(() => first.State == SquadState.Fighting, 30f);
            Assert.That(first.Target, Is.SameAs(enemy));
            Assert.That(second.Target, Is.SameAs(enemy));

            float fight = RunUntil(() => enemy.State == SquadState.Destroyed, 30f);
            Assert.That(fight, Is.LessThan(7f));
            Assert.That(first.State, Is.Not.EqualTo(SquadState.Destroyed));
            Assert.That(second.State, Is.Not.EqualTo(SquadState.Destroyed));
        }

        // --- helpers ---

        private Squad Spawn(SquadDefinition definition, Side side)
        {
            var enemyBase = side == Side.Player ? _enemyBase : _playerBase;
            var squad = new Squad(definition, side, _lane, enemyBase);
            _simulation.Add(squad);
            return squad;
        }

        private void Run(float seconds)
        {
            int steps = Mathf.RoundToInt(seconds / Dt);
            for (int i = 0; i < steps; i++) _simulation.Step(Dt);
        }

        /// <summary>Крокує, доки умова не стане істинною. Повертає витрачений час.</summary>
        private float RunUntil(Func<bool> condition, float timeout)
        {
            float time = 0f;
            while (!condition())
            {
                if (time > timeout) Assert.Fail($"Умова не виконалась за {timeout} с");
                _simulation.Step(Dt);
                time += Dt;
            }
            return time;
        }

        private SquadDefinition MakeInfantry(int soldiers)
        {
            var definition = Track(ScriptableObject.CreateInstance<SquadDefinition>());
            var so = new SerializedObject(definition);
            so.FindProperty("soldierCount").intValue = soldiers;
            so.ApplyModifiedPropertiesWithoutUndo();
            return definition;
        }

        private Base MakeBase(BaseDefinition definition, Side side, float frontY)
        {
            var go = Track(new GameObject($"{side}Base"));
            var b = go.AddComponent<Base>();
            b.Init(definition, side, frontY);
            return b;
        }

        private T Track<T>(T obj) where T : Object
        {
            _created.Add(obj);
            return obj;
        }
    }
}
