namespace TugOfWar.Battle
{
    /// <summary>Сторона бою. Гравець знизу екрана, ворог зверху (D-009).</summary>
    public enum Side
    {
        Player,
        Enemy
    }

    public static class SideExtensions
    {
        public static Side Opponent(this Side side) => side == Side.Player ? Side.Enemy : Side.Player;
    }
}
