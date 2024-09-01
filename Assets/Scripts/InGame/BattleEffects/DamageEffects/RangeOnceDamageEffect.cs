using UnityEngine;

using Utils.Common;

namespace InGame.BattleEffects
{
    public class RangeOnceDamageEffect : Effect
    {
        [SerializeField]
        private float m_radius;
        [SerializeField]
        private int m_damage;

        public RangeOnceDamageEffect(float radius, int damage) : base(1)
        {
            this.m_radius = radius;
            this.m_damage = damage;
        }
        
        private RangeOnceDamageEffect(){}

        public override void Trigger(GameObject go)
        {
            if (!IsActive) return;
            Debug.Log("random once damage effect active!");
            Vector3 center = go.transform.position;
            Collider[] colliders = Physics.OverlapSphere(center, m_radius);
            foreach (var collider in colliders)
            {
                Debug.Log("meet collider ");
                IDamageable damageable = collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(m_damage);
            }
            m_count--;
        }

        protected override Effect OnCreateCopy()
        {
            return new RangeOnceDamageEffect
            {
                m_radius = m_radius,
                m_damage = m_damage
            };
        }
    }
}