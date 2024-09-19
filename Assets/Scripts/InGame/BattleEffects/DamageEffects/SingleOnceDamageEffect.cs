using UnityEngine;
using Utils.Common;

namespace InGame.BattleEffects
{
    public class SingleOnceDamageEffect : Effect
    {
        [SerializeField]
        protected int m_damage;

        public SingleOnceDamageEffect(int damage) : base(-1)
        {
            this.m_damage = damage;
        }
        
        private SingleOnceDamageEffect(){}

        public override void Trigger(GameObject go)
        {
            if(!IsActive) return;
            IDamageable damageable = go.GetComponent<IDamageable>();
            damageable?.TakeDamage(m_damage);
        }

        protected override Effect OnCreateCopy()
        {
            return new SingleOnceDamageEffect()
            {
                m_damage = m_damage
            };
        }
    }
}