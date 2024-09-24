using UnityEngine;
using Utils;
using Utils.Common;

namespace InGame.BattleEffects
{
    public class SingleOnceDamageEffect : Effect
    {
        [SerializeField]
        protected int m_damage;
        private GameObject m_vfx;

        public SingleOnceDamageEffect(int damage) : base(-1)
        {
            this.m_damage = damage;
            this.m_vfx = Resources.Load<GameObject>(Constants.VFX_HIT_PATH);
        }
        
        private SingleOnceDamageEffect(){}

        public override void Trigger(GameObject go)
        {
            if(!IsActive) return;

            var vfxGO = GameObject.Instantiate(m_vfx);
            vfxGO.transform.position = go.transform.position;

            IDamageable damageable = go.GetComponent<IDamageable>();
            damageable?.TakeDamage(m_damage);
        }

        protected override Effect OnCreateCopy()
        {
            return new SingleOnceDamageEffect()
            {
                m_damage = m_damage,
                m_vfx = Resources.Load<GameObject>(Constants.VFX_HIT_PATH)
            };
        }
    }
}