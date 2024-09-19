using InGame.Boards.Modules.ModuleBuffs;

namespace InGame.Effects.EffectElement.ApplyModuleBuffEffects
{
    public class ApplyAndroidUnlimitedPropertyBuff: ApplyModuleBuff
    {
        public AndroidUnlimitPropertyBuff buff;
        public override ModuleBuff GetBuff()
        {
            return buff.CreateCopy();
        }

        public override ApplyModuleBuff OnCopy()
        {
            return new ApplyAndroidUnlimitedPropertyBuff
            {
                buff = (AndroidUnlimitPropertyBuff)buff.CreateCopy()
            };
        }
        
        public static ApplyAndroidUnlimitedPropertyBuff CreateEffect(BuffRange range, AndroidUnlimitPropertyBuff buff)
        {
            return new ApplyAndroidUnlimitedPropertyBuff
            {
                range = range,
                buff = buff
            };
        }
    }
}