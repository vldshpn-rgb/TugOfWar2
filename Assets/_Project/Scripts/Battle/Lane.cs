using System;

namespace TugOfWar.Battle
{
    /// <summary>
    /// Вертикальна лінія бою з центром у (0, 0): база гравця внизу, база ворога вгорі (D-009).
    /// Позиція на лінії це просто координата Y. Звичайний C#-клас, щоб його можна було перевіряти тестами.
    /// </summary>
    public sealed class Lane
    {
        public float Length { get; }

        /// <summary>Наскільки попереду своєї бази з'являються загони.</summary>
        public float SpawnOffset { get; }

        public Lane(float length, float spawnOffset)
        {
            if (length <= 0f) throw new ArgumentOutOfRangeException(nameof(length));
            if (spawnOffset < 0f || spawnOffset > length) throw new ArgumentOutOfRangeException(nameof(spawnOffset));

            Length = length;
            SpawnOffset = spawnOffset;
        }

        public float BaseY(Side side) => side == Side.Player ? -Length / 2f : Length / 2f;

        /// <summary>Напрямок руху загонів сторони по Y: гравець іде вгору, ворог униз.</summary>
        public float Forward(Side side) => side == Side.Player ? 1f : -1f;

        public float SpawnY(Side side) => BaseY(side) + Forward(side) * SpawnOffset;
    }
}
