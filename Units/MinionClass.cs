using System.ComponentModel;

namespace Units
{
    public enum MinionClass
    {
        [Description("Gladiator")] Gladiator = 0,
        [Description("Knight")] Templar = 1,
        [Description("Cleric")] Cleric = 2,
        [Description("Monk")] Chanter = 3,
        [Description("Sorcerer")] Sorcerer = 4,
        [Description("Warlock")] Spiritmaster = 5,
        [Description("Ranger")] Ranger = 6,
        [Description("Assassin")] Assassin = 7
    }

    public enum ClassParent
    {
        None = -1,
        Scout = 1, //Следопыт
        Warrior = 2, //Воин
        Mage = 3, //Маг
        Priest = 4, //Жрец
    }
}