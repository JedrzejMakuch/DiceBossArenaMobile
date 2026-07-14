namespace DiceBossArena.Game
{
    public readonly struct FightStatModifier
    {
        public FightStatType StatType { get; }

        public FightStatModifierType ModifierType { get; }

        public int Value { get; }

        public FightStatModifier(
            FightStatType statType,
            FightStatModifierType modifierType,
            int value)
        {
            StatType = statType;
            ModifierType = modifierType;
            Value = value;
        }
    }
}