namespace Inferno.GameFramework;

public struct PlayerStats
{
    /*
     * Strength
     * Dexterity
     * Constitution     - health/healthyness    :: Rezistance to status effects
     * Intelligent      - Book knolage - magic - math
     * Wisdom           - Streat smarts stuff and stabiltiy of mind (application of mind)
     * Rizz             - How well you can chat people up
     *
     */
    public int HitPoints;
    public int ManaPoints;
    public int Level;
    public int Experience;

    public int PhysicalArmorClass;
    public int MagicArmorClass;

    public int Dexterity;
    public int Strength;
    public int Arcane;
    public int Wisdom;
    public int Rizz;
}

