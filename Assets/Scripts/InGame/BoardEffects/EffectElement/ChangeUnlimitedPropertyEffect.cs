using InGame.BattleFields.Common;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Cores;

namespace InGame.Effects.EffectElement
{
    public class ChangeUnlimitedPropertyEffect: Effect
    {
        public override ModuleBuffType buffMask => ModuleBuffType.AndroidUnlimitProperty;
        public UnlimitedPropertyType propertyType;
        public int delta;
        

        private AndroidUnlimitPropertyBuff m_buff = (AndroidUnlimitPropertyBuff) AndroidUnlimitPropertyBuff.CreateEmptyBuff();
        
        public override void OnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetAndroid().Increase(propertyType, delta);
        }

        public override void OnUnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetAndroid().Decrease(propertyType, delta);
        }

        public override Effect CreateCopy()
        {
            return new ChangeUnlimitedPropertyEffect
            {
                propertyType = this.propertyType,
                delta = this.delta,
            };
            
        }

        public override void OnAddBuff(ModuleBuff buff)
        {
            var androidBuff = (AndroidUnlimitPropertyBuff)buff;

            

            m_buff.Add(androidBuff);
            
            if (!androidBuff.GetBlk(propertyType, out var blk)) return;

            GameManager.Instance.GetAndroid().Increase(propertyType, blk.valueBuff);
        }

        public override void OnRemoveBuff(ModuleBuff buff)
        {
            var androidBuff = (AndroidUnlimitPropertyBuff)buff;
            
            m_buff.Minus(androidBuff);
            
            if (!androidBuff.GetBlk(propertyType, out var blk)) return;

            GameManager.Instance.GetAndroid().Decrease(propertyType, blk.valueBuff);
        }

        public override void ClearBuffs()
        {
            if (!m_buff.GetBlk(propertyType, out var blk)) return;

            GameManager.Instance.GetAndroid().Decrease(propertyType, blk.valueBuff);
            
            m_buff.SetDefault();
        }

        public static ChangeUnlimitedPropertyEffect CreateEffect(UnlimitedPropertyType propertyType, int delta)
        {
            return new ChangeUnlimitedPropertyEffect
            {
                propertyType = propertyType,
                delta = delta,
            };
        }
    }
}