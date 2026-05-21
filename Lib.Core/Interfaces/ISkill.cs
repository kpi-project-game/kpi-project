namespace Lib.Core.Interfaces;

public interface ISkill
{
    public string Name {get;}
    public void Execute(IBattleUnit caster, IBattleUnit target);
}