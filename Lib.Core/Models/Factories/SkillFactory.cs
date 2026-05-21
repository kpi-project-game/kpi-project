using Lib.Core.Interfaces;
using Lib.Core.Models.Skills.DefaultSkills;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Factories;

public static class SkillFactory
{
    public static ISkill CreateByName(string skillName)
    {
        return skillName switch
        {
            "Hand Attack"    => new HandAttack(),
            "Defend"         => new Defend(),
            
            "Charming"       => new CharmSkill(),
            "Doom"           => new Doom(),
            "Fireball"       => new FireballSkill(10), 
            "Frenzy"         => new Frenzy(3),         
            "Frighten"       => new Frighten(),
            "Thunderstrike"  => new ThunderStrikeSkill(6),
            
            "Bite"           => new Bite(5),
            "HalebardStrike" => new HalebardStrike(8),
            
            "Einhorn"        => new Einhorn(3),       
            "Abschneiden"    => new Abschneiden(6, 3), 
            "Stechen"        => new Stechen(7),        
            "Zwerchhau"      => new Zwerchhau(4, 2),  
            
            _                => new HandAttack()
        };
    }
}