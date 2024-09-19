using InGame.Effects;
using InGame.Effects.EffectElement;

namespace Editors.Effects
{
    public class ChangeShieldEffectEdit: EffectEdit
    {
        public int delta = 0;
        
        public override Effect CreateEffect()
        {
            return ChangeShieldSignalEffect.CreateEffect(delta);
        }
    }
}