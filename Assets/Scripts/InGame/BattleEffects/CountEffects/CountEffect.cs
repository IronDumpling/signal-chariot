using UnityEngine;

namespace InGame.BattleEffects
{
    public class CountEffect : Effect
    {
        public CountEffect(int count) : base(count){}
        
        private CountEffect(){}
        
        public override void Trigger(GameObject go)
        {
            if(!IsActive) return;
            m_count--;
        }

        protected override Effect OnCreateCopy()
        {
            return new CountEffect();
        }

        public bool AbleToDie()
        {
            return !IsActive;
        }
    }
}