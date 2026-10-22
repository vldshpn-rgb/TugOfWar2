using System;
using NUnit.Framework;
using TugOfWar.Battle;

namespace TugOfWar.Tests
{
    public class LaneTests
    {
        private const float Tolerance = 1e-4f;

        [Test]
        public void Bases_AtLaneEnds_PlayerBottom()
        {
            var lane = new Lane(16f, 1.5f);

            Assert.That(lane.BaseY(Side.Player), Is.EqualTo(-8f).Within(Tolerance));
            Assert.That(lane.BaseY(Side.Enemy), Is.EqualTo(8f).Within(Tolerance));
        }

        [Test]
        public void Forward_PlayerUp_EnemyDown()
        {
            var lane = new Lane(16f, 1.5f);

            Assert.That(lane.Forward(Side.Player), Is.EqualTo(1f));
            Assert.That(lane.Forward(Side.Enemy), Is.EqualTo(-1f));
        }

        [Test]
        public void Spawn_InFrontOfOwnBase()
        {
            var lane = new Lane(16f, 1.5f);

            Assert.That(lane.SpawnY(Side.Player), Is.EqualTo(-6.5f).Within(Tolerance));
            Assert.That(lane.SpawnY(Side.Enemy), Is.EqualTo(6.5f).Within(Tolerance));
        }

        [Test]
        public void Opponent_IsOtherSide()
        {
            Assert.That(Side.Player.Opponent(), Is.EqualTo(Side.Enemy));
            Assert.That(Side.Enemy.Opponent(), Is.EqualTo(Side.Player));
        }

        [Test]
        public void Constructor_InvalidArguments_Throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Lane(0f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Lane(16f, -1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Lane(16f, 20f));
        }
    }
}
