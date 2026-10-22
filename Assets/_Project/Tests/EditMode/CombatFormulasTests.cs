using NUnit.Framework;
using TugOfWar.Squads;

namespace TugOfWar.Tests
{
    public class CombatFormulasTests
    {
        private const float Tolerance = 1e-4f;

        [Test]
        public void Damage_FullSquad_DealsBaseDamage()
        {
            Assert.That(CombatFormulas.Damage(20f, 0.5f, 10, 10), Is.EqualTo(20f).Within(Tolerance));
        }

        [Test]
        public void Damage_GddExample_FourOfTenAlive_Deals14()
        {
            // GDD 3.4: 20 × (0.5 + 0.5 × 0.4) = 14
            Assert.That(CombatFormulas.Damage(20f, 0.5f, 4, 10), Is.EqualTo(14f).Within(Tolerance));
        }

        [Test]
        public void Damage_BaseShareOne_IgnoresLosses()
        {
            Assert.That(CombatFormulas.Damage(20f, 1f, 1, 10), Is.EqualTo(20f).Within(Tolerance));
        }

        [Test]
        public void Damage_BaseShareZero_ProportionalToAlive()
        {
            Assert.That(CombatFormulas.Damage(20f, 0f, 3, 10), Is.EqualTo(6f).Within(Tolerance));
        }

        [TestCase(0f, 10f)]
        [TestCase(0.5f, 5f)]
        [TestCase(0.8f, 2f)]
        public void AfterArmor_CutsPercent(float armor, float expected)
        {
            Assert.That(CombatFormulas.AfterArmor(10f, armor), Is.EqualTo(expected).Within(Tolerance));
        }
    }
}
