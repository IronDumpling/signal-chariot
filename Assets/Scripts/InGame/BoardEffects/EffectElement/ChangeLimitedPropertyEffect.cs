using InGame.BattleFields.Common;
using InGame.Boards.Modules.ModuleBuffs;
using InGame.Cores;

namespace InGame.Effects.EffectElement
{
    public class ChangeLimitedPropertyEffect: Effect
    {
        public override ModuleBuffType buffMask => ModuleBuffType.AndroidLimitProperty;
        public LimitedPropertyType propertyType;
        public int delta;
        public bool isCurrent;
        

        private AndroidLimitPropertyBuff m_buff = (AndroidLimitPropertyBuff) AndroidLimitPropertyBuff.CreateEmptyBuff();
        
        public override void OnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetAndroid().Increase(propertyType, delta, isCurrent);
        }

        public override void OnUnTrigger(EffectBlackBoard blackBoard)
        {
            GameManager.Instance.GetAndroid().Decrease(propertyType, delta, isCurrent);
        }

        public override Effect CreateCopy()
        {
            return new ChangeLimitedPropertyEffect
            {
                propertyType = this.propertyType,
                delta = this.delta,
                isCurrent = this.isCurrent
            };
            
        }

        public override void OnAddBuff(ModuleBuff buff)
        {
            var androidBuff = (AndroidLimitPropertyBuff)buff;

            

            m_buff.Add(androidBuff);
            
            if (!androidBuff.GetBlk(propertyType, out var blk)) return;

            GameManager.Instance.GetAndroid().Increase(propertyType,
                isCurrent ? blk.currentValueBuff : blk.maxValueBuff, isCurrent);
        }

        public override void OnRemoveBuff(ModuleBuff buff)
        {
            var androidBuff = (AndroidLimitPropertyBuff)buff;
            
            m_buff.Minus(androidBuff);
            
            if (!androidBuff.GetBlk(propertyType, out var blk)) return;

            GameManager.Instance.GetAndroid().Decrease(propertyType,
                isCurrent ? blk.currentValueBuff : blk.maxValueBuff, isCurrent);
        }

        public override void ClearBuffs()
        {
            if (!m_buff.GetBlk(propertyType, out var blk)) return;

            GameManager.Instance.GetAndroid().Decrease(propertyType,
                isCurrent ? blk.currentValueBuff : blk.maxValueBuff, isCurrent);
            
            m_buff.SetDefault();
        }

        public static ChangeLimitedPropertyEffect CreateEffect(LimitedPropertyType propertyType, int delta, bool isCurrent)
        {
            return new ChangeLimitedPropertyEffect
            {
                propertyType = propertyType,
                delta = delta,
                isCurrent = isCurrent
            };
        }
    }
}