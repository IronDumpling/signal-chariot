using InGame.BattleFields.Common;
using InGame.Effects;
using InGame.Effects.EffectElement;

namespace Editors.Effects
{
    public class ChangeUnlimitedPropertyEffectEdit: EffectEdit
    {
        public UnlimitedPropertyType propertyType;
        public int delta;


        public override Effect CreateEffect()
        {
            return ChangeUnlimitedPropertyEffect.CreateEffect(propertyType, delta);
        }
    }
}