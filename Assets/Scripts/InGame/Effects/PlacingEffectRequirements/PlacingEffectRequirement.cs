using InGame.Boards;

namespace InGame.Effects.PlacingEffectRequirements
{
    [System.Serializable]
    public abstract class PlacingEffectRequirement
    {
        public enum RequirementType
        {
            Border
        }

        public abstract RequirementType type{ get;}
        public abstract bool CanTrigger(EffectBlackBoard bb);

        public abstract PlacingEffectRequirement CreateCopy();

        public static PlacingEffectRequirement CreateRequirement(RequirementType type)
        {
            return type switch
            {
                RequirementType.Border => BorderRequirement.CreateRequirement(),
                _ => null
            };
        }
    }
}