using System;

namespace TugOfWar.Battle
{
    /// <summary>
    /// Умова перемоги (D-010): хто першим зруйнував ворожу базу, той переміг.
    /// Повідомляє Ended один раз, навіть якщо обидві бази впадуть в одному кадрі.
    /// </summary>
    public sealed class BattleOutcome : IDisposable
    {
        /// <summary>Аргумент: сторона-переможець.</summary>
        public event Action<Side> Ended;

        public bool IsOver { get; private set; }
        public Side Winner { get; private set; }

        private readonly Base _playerBase;
        private readonly Base _enemyBase;

        public BattleOutcome(Base playerBase, Base enemyBase)
        {
            _playerBase = playerBase;
            _enemyBase = enemyBase;
            _playerBase.Destroyed += OnPlayerBaseDestroyed;
            _enemyBase.Destroyed += OnEnemyBaseDestroyed;
        }

        public void Dispose()
        {
            _playerBase.Destroyed -= OnPlayerBaseDestroyed;
            _enemyBase.Destroyed -= OnEnemyBaseDestroyed;
        }

        private void OnPlayerBaseDestroyed() => End(Side.Enemy);
        private void OnEnemyBaseDestroyed() => End(Side.Player);

        private void End(Side winner)
        {
            if (IsOver) return;
            IsOver = true;
            Winner = winner;
            Ended?.Invoke(winner);
        }
    }
}
