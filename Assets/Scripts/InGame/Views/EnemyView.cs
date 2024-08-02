using UnityEngine;

using Utils;
using Utils.Common;
using InGame.BattleFields.Enemies;

using DG.Tweening;
using Unity.VisualScripting;
using InGame.BattleFields.Common;

namespace InGame.Views
{
    public class EnemyView : MonoBehaviour, IDamager, IDamageable
    {
        private Enemy m_enemy;
        private Vector3 m_target = Vector3.zero;

        #region Life Cycle
        public void Init(Enemy enemy)
        {
            m_enemy = enemy;
        }

        private void Update()
        {
            Move();
        }

        public void Die()
        {
            Destroy(gameObject);
        }
        #endregion

        #region Action

        private void Move()
        {
            float distance = Vector3.Distance(this.transform.position, m_target);
            if(distance <= Constants.COLLIDE_OFFSET) return;
            transform.DOMove(m_target, distance / m_enemy.Get(UnlimitedPropertyType.Speed))
                    .SetEase(Ease.InOutQuad);
        }

        #endregion

        #region Interaction
        public void OnCollisionEnter(Collision other)
        {
            IDamageable target = other.gameObject.GetComponent<IDamageable>();
            if(target != null) DealDamage(target, m_enemy.Get(UnlimitedPropertyType.Damage));
        }

        public void TakeDamage(float dmg)
        {
            m_enemy.TakeDamage(dmg);
        }

        public void DealDamage(IDamageable target, float dmg)
        {
            target.TakeDamage(dmg);
        }

        #endregion
    }
}