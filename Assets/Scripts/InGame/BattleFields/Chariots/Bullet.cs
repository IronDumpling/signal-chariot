using UnityEngine;

using InGame.BattleFields.Common;
using InGame.Views;
using SetUps;
using Utils;
using InGame.Cores;
using Unity.VisualScripting;
using Utils.Common;

namespace InGame.BattleFields.Chariots
{
    public class Bullet
    {   
        [Header("View")]
        private BulletView m_bulletView;

        [Header("Properties")]
        private Vector3 m_target;
        private Sprite m_sprite;
        private UnlimitedProperty m_speed;
        private UnlimitedProperty m_damage;
        private UnlimitedProperty m_range;

        public Vector3 target { get { return m_target;}}
        public Sprite sprite{ get { return m_sprite;}}
        public UnlimitedProperty speed { get { return m_speed;}}
        public UnlimitedProperty damage { get { return m_damage;}}
        public UnlimitedProperty range { get { return m_range;}}

        public Bullet(BulletSetUp bulletSetUp, Vector3 target, float damageMultiplier)
        {
            UnlimitedProperty dmg = new(bulletSetUp.damage * damageMultiplier, 
                                        UnlimitedPropertyType.Damage);
            UnlimitedProperty spd = new(bulletSetUp.speed, UnlimitedPropertyType.Speed);
            UnlimitedProperty rng = new(bulletSetUp.range, UnlimitedPropertyType.Range);

            m_target = target;            
            m_damage = dmg;
            m_speed = spd;
            m_range = rng;
            m_sprite = bulletSetUp.sprite;

            CreateView();
        }

        private void CreateView()
        {
            GameObject bulletPref = Resources.Load<GameObject>(Constants.GO_BULLET_PATH);
            GameObject bulletGO = GameObject.Instantiate(bulletPref);
            
            float x = GameManager.Instance.GetChariot().chariotView.transform.position.x;
            float y = GameManager.Instance.GetChariot().chariotView.transform.position.y;

            bulletGO.transform.position = new(x, y, Constants.BULLET_DEPTH);

            m_bulletView = bulletGO.GetComponent<BulletView>();
            m_bulletView.Init(this);
        }

        public void Die()
        {
            m_bulletView.Die();
        }

        public void DealDamage(IDamageable target, float dmg)
        {
            target.TakeDamage(dmg);
            this.Die();
        }
    }
}