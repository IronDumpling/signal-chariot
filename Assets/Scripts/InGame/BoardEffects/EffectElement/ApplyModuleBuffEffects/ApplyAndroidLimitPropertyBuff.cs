using InGame.Boards.Modules.ModuleBuffs;

namespace InGame.Effects.EffectElement.ApplyModuleBuffEffects
{
    public class ApplyAndroidLimitPropertyBuff: ApplyModuleBuff
    {
        public AndroidLimitPropertyBuff buff;
        public override ModuleBuff GetBuff()
        {
            return buff.CreateCopy();
        }

        public override ApplyModuleBuff OnCopy()
        {
            return new ApplyAndroidLimitPropertyBuff
            {
                buff = (AndroidLimitPropertyBuff)buff.CreateCopy()
            };
        }
        
        public static ApplyAndroidLimitPropertyBuff CreateEffect(BuffRange range, AndroidLimitPropertyBuff buff)
        {
            return new ApplyAndroidLimitPropertyBuff
            {
                range = range,
                buff = buff
            };
        }
    }
}