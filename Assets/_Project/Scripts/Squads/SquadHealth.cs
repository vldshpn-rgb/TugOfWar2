using System;

namespace TugOfWar.Squads
{
    /// <summary>
    /// Здоров'я загону за D-008: отримана шкода накопичується, і щоразу, коли накопичене
    /// досягає SoldierHp, гине один солдат, а залишок переходить на наступного.
    /// Звичайний C#-клас, щоб його можна було перевіряти тестами без сцени.
    /// </summary>
    public sealed class SquadHealth
    {
        public int Total { get; }
        public float SoldierHp { get; }
        public int Alive { get; private set; }

        /// <summary>Шкода, що вже прийшлася на поточного (ще живого) солдата.</summary>
        public float Accumulated { get; private set; }

        public bool IsDead => Alive == 0;

        /// <summary>GDD 3.1: HP загону = живі × HP солдата (мінус шкода по поточному солдату).</summary>
        public float CurrentHp => Alive * SoldierHp - Accumulated;

        public SquadHealth(int total, float soldierHp)
        {
            if (total <= 0) throw new ArgumentOutOfRangeException(nameof(total));
            if (soldierHp <= 0f) throw new ArgumentOutOfRangeException(nameof(soldierHp));

            Total = total;
            SoldierHp = soldierHp;
            Alive = total;
        }

        /// <summary>Застосовує шкоду (броню вже враховано). Повертає, скільки солдатів загинуло.</summary>
        public int ApplyDamage(float damage)
        {
            if (damage <= 0f || IsDead) return 0;

            Accumulated += damage;
            int killed = 0;
            while (Accumulated >= SoldierHp && Alive > 0)
            {
                Accumulated -= SoldierHp;
                Alive--;
                killed++;
            }

            if (IsDead) Accumulated = 0f;
            return killed;
        }
    }
}
