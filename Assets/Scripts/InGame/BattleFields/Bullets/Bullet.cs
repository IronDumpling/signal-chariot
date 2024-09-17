using System.Collections.Generic;

using UnityEngine;

using SetUps;

using InGame.Cores;
using InGame.Views;
using InGame.BattleFields.Androids;
using InGame.BattleFields.Common;
using InGame.BattleEffects;

using Utils;
using Utils.Common;

namespace InGame.BattleFields.Bullets
{
    public class Bullet
    {   
        [Header("View")]
        private BulletView m_bulletView;
        public BulletView bulletView {get { return m_bulletView;}}

        [Header("Generator")]
        private Equipment m_equipment;
        public Equipment equipment {get { return m_equipment;}}
        private int[] m_bulletIdx;
        public int[] bulletIdx {get { return m_bulletIdx;}}

        [Header("Properties")]
        private Sprite m_sprite;
        private UnlimitedProperty m_size;
        private UnlimitedProperty m_speed;
        private UnlimitedProperty m_damage;
        private UnlimitedProperty m_lifeTime;

        [Header("Logics")]
        private IMovable m_moveStrategy;
        private List<Effect> m_collisionEffects;
        private List<Effect> m_destructionEffects;

        public Sprite sprite{ get { return m_sprite;}}
        public UnlimitedProperty size{ get { return m_size;}}
        public UnlimitedProperty speed { get { return m_speed;}}
        public UnlimitedProperty damage { get { return m_damage;}}
        public UnlimitedProperty lifetime { get { return m_lifeTime;}}
        public IMovable moveStrategy { get { return m_moveStrategy;}}
        public List<Effect> collisionEffects { get{ return m_collisionEffects;}}
        public List<Effect> destructionEffects { get { return m_destructionEffects;}}

        public Bullet(BulletSetUp bulletSetUp, Equipment equipment, int[] bulletIdx)
        {
            m_equipment = equipment;
            m_bulletIdx = bulletIdx;
            
            UnlimitedProperty dmg = new(bulletSetUp.damage, UnlimitedPropertyType.Damage);
            UnlimitedProperty spd = new(bulletSetUp.speed, UnlimitedPropertyType.Speed);
            UnlimitedProperty lft = new(bulletSetUp.lifeTime);
            UnlimitedProperty siz = new(bulletSetUp.size * Constants.BULLET_SIZE_MULTIPLIER);

            m_damage = dmg;
            m_speed = spd;
            m_lifeTime = lft;
            m_size = siz;
            
            m_collisionEffects = bulletSetUp.collisionEffects;
            m_destructionEffects = bulletSetUp.destructionEffects;

            CreateView(bulletSetUp.sprite);
            CreateMoveStrategy(bulletSetUp.moveType);
        }

        private void CreateView(Sprite sprite)
        {
            m_sprite = sprite;

            GameObject bulletPref = Resources.Load<GameObject>(Constants.GO_BULLET_PATH);
            GameObject bulletGO = GameObject.Instantiate(bulletPref);
            
            float x = m_equipment.equipmentView.transform.position.x;
            float y = m_equipment.equipmentView.transform.position.y;

            bulletGO.transform.position = new(x, y, Constants.BULLET_DEPTH);

            m_bulletView = bulletGO.GetComponent<BulletView>();
            m_bulletView.Init(this);
        }

        private void CreateMoveStrategy(MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Linear:
                    m_moveStrategy = new LinearMoveStrategy(this);
                    break;
                case MoveType.Follow:
                    m_moveStrategy = new FollowMoveStrategy(this);
                    break;
                case MoveType.Parabola:
                    m_moveStrategy = new ParabolaMoveStrategy(this);
                    break;
                case MoveType.CircleRound:
                    m_moveStrategy = new CircleRoundMoveStrategy(this);
                    break;
                case MoveType.Placement:
                    m_moveStrategy = new PlacementMoveStrategy(this);
                    break;
                case MoveType.Random:
                    m_moveStrategy = new RandomMoveStrategy(this);
                    break;
                default:
                    Debug.LogWarning("No matching move type: " + moveType);
                    break;
            }
        }

        public void Die()
        {
            bool ableToDie = true;
            foreach(Effect effect in m_collisionEffects)
            {
                if (effect is CountEffect countEffect)
                    ableToDie = ableToDie && countEffect.AbleToDie();
            }
            if(ableToDie) m_bulletView.Die();
        }

        public void DealDamage(IDamageable target, float dmg)
        {
            target.TakeDamage(dmg);
        }
    }
}