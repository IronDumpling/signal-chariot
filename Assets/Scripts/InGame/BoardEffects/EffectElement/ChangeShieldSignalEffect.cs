using InGame.BattleFields.Common;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Cores;

namespace InGame.Effects.EffectElement
{
    public class ChangeShieldSignalEffect: Effect
    {
        public override ModuleBuffType buffMask => ModuleBuffType.AndroidLimitProperty;
        public int delta;
        public int currentBuff = 0;
        
        public override void OnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetAndroid().Increase(LimitedPropertyType.Armor, delta + currentBuff, true);
        }

        public override void OnUnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetAndroid().Decrease(LimitedPropertyType.Armor, delta + currentBuff, true);
        }

        public override void OnAddBuff(ModuleBuff buff)
        {
            var limitBuff = (AndroidLimitPropertyBuff) buff;

            if (!limitBuff.GetBlk(LimitedPropertyType.Armor, out var blk)) return;

            currentBuff += (int) blk.currentValueBuff;

        }

        public override void OnRemoveBuff(ModuleBuff buff)
        {
            var limitBuff = (AndroidLimitPropertyBuff) buff;

            if (!limitBuff.GetBlk(LimitedPropertyType.Armor, out var blk)) return;
            currentBuff -= (int) blk.currentValueBuff;
        }

        public override void ClearBuffs()
        {
            currentBuff = 0;
        }

        public override Effect CreateCopy()
        {
            return new ChangeShieldSignalEffect { delta = this.delta };
            
        }

        public static ChangeShieldSignalEffect CreateEffect(int delta)
        {
            return new ChangeShieldSignalEffect
            {
                delta = delta
            };
        }
    }
}