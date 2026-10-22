using System;
using NUnit.Framework;
using TugOfWar.Squads;

namespace TugOfWar.Tests
{
    public class SquadHealthTests
    {
        private const float Tolerance = 1e-4f;

        [Test]
        public void New_AllSoldiersAlive()
        {
            var health = new SquadHealth(10, 10f);

            Assert.That(health.Alive, Is.EqualTo(10));
            Assert.That(health.CurrentHp, Is.EqualTo(100f).Within(Tolerance));
            Assert.That(health.IsDead, Is.False);
        }

        [Test]
        public void Damage_BelowSoldierHp_KillsNobody()
        {
            var health = new SquadHealth(10, 10f);

            Assert.That(health.ApplyDamage(9f), Is.EqualTo(0));
            Assert.That(health.Alive, Is.EqualTo(10));
            Assert.That(health.CurrentHp, Is.EqualTo(91f).Within(Tolerance));
        }

        [Test]
        public void Damage_ExactlySoldierHp_KillsOne()
        {
            var health = new SquadHealth(10, 10f);

            Assert.That(health.ApplyDamage(10f), Is.EqualTo(1));
            Assert.That(health.Alive, Is.EqualTo(9));
            Assert.That(health.Accumulated, Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void Damage_Remainder_CarriesToNextSoldier()
        {
            var health = new SquadHealth(10, 10f);

            Assert.That(health.ApplyDamage(25f), Is.EqualTo(2));
            Assert.That(health.Accumulated, Is.EqualTo(5f).Within(Tolerance));

            Assert.That(health.ApplyDamage(5f), Is.EqualTo(1));
            Assert.That(health.Alive, Is.EqualTo(7));
        }

        [Test]
        public void Damage_Overkill_StopsAtZero()
        {
            var health = new SquadHealth(3, 10f);

            Assert.That(health.ApplyDamage(1000f), Is.EqualTo(3));
            Assert.That(health.IsDead, Is.True);
            Assert.That(health.CurrentHp, Is.EqualTo(0f).Within(Tolerance));
            Assert.That(health.ApplyDamage(10f), Is.EqualTo(0));
        }

        [Test]
        public void Damage_ZeroOrNegative_Ignored()
        {
            var health = new SquadHealth(10, 10f);

            Assert.That(health.ApplyDamage(0f), Is.EqualTo(0));
            Assert.That(health.ApplyDamage(-5f), Is.EqualTo(0));
            Assert.That(health.CurrentHp, Is.EqualTo(100f).Within(Tolerance));
        }

        [Test]
        public void Constructor_InvalidArguments_Throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SquadHealth(0, 10f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new SquadHealth(10, 0f));
        }
    }
}
