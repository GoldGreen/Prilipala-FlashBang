public enum EffectType
{
    Reloadable,
    Single
}

public interface ILinkWithShower
{
    EffectShower EffectShower { get; set; }
}