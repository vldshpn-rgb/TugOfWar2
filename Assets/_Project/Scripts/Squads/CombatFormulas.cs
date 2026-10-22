namespace TugOfWar.Squads
{
    /// <summary>
    /// Чисті формули бою (без MonoBehaviour), щоб їх можна було перевіряти EditMode-тестами.
    /// </summary>
    public static class CombatFormulas
    {
        /// <summary>
        /// D-002: шкода = базова × (частка_бази + (1 − частка_бази) × живі / всього).
        /// Діє і для ближньої, і для дальньої атаки (D-008).
        /// </summary>
        public static float Damage(float baseDamage, float baseShare, int alive, int total) =>
            baseDamage * (baseShare + (1f - baseShare) * alive / total);

        /// <summary>D-008: броня зрізає відсоток шкоди. Armor від 0 до 0.8.</summary>
        public static float AfterArmor(float damage, float armor) => damage * (1f - armor);
    }
}
