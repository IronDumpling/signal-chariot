using UnityEngine;

using InGame.BattleFields.Common;
using InGame.Views;
using SetUps;

namespace InGame.BattleFields.Enemies
{
    public class Enemy
    {
        
        private LimitedProperty m_health;
        private UnlimitedProperty m_speed;
        private UnlimitedProperty m_damage;
        private UnlimitedProperty m_range;
        private UnlimitedProperty m_attackInterval;

        public string name { get; private set; }
        public int typeID { get; private set; }

        private EnemyView m_viewPrefab;
        private EnemyView m_view = null;

        #region Life Cycle
        public EnemyView CreateView()
        {
            if (m_viewPrefab == null) return null;
            if (m_view != null) return m_view;

            m_view = GameObject.Instantiate(m_viewPrefab);
            m_view.Init(this);
            return m_view;
        }
        
        public static Enemy CreateEnemy(Enemy other)
        {
            return new Enemy
            {
                name = other.name,
                typeID = other.typeID,
                m_viewPrefab = other.m_viewPrefab,
                m_health = new LimitedProperty(
                    other.Get(LimitedPropertyType.Health, false), 
                    LimitedPropertyType.Health
                ),
                m_damage = new UnlimitedProperty(
                    other.Get(UnlimitedPropertyType.Damage), 
                    UnlimitedPropertyType.Damage
                ),
                m_range = new UnlimitedProperty(
                    other.Get(UnlimitedPropertyType.Range), 
                    UnlimitedPropertyType.Range
                ),
                m_attackInterval = new UnlimitedProperty(
                    other.Get(UnlimitedPropertyType.Interval), 
                    UnlimitedPropertyType.Interval
                ),
                m_speed = new UnlimitedProperty(
                    other.Get(UnlimitedPropertyType.Speed), 
                    UnlimitedPropertyType.Speed
                )
            };
        }
        
        public static Enemy CreateEnemy(EnemySetUp setUp, int typeID)
        {
            return new Enemy
            {
                name = setUp.name,
                typeID = typeID,
                m_viewPrefab = setUp.enemyPrefab,
                m_health = new LimitedProperty(setUp.maxHealth, LimitedPropertyType.Health),
                m_damage = new UnlimitedProperty(setUp.attack, UnlimitedPropertyType.Damage),
                m_range = new UnlimitedProperty(setUp.attackRadius, UnlimitedPropertyType.Range),
                m_attackInterval = new UnlimitedProperty(setUp.attackDuration, UnlimitedPropertyType.Interval),
                m_speed = new UnlimitedProperty(setUp.speed, UnlimitedPropertyType.Speed)
            };
        }

        public void Die()
        {
            m_view.Die();
        }

        #endregion

        #region Properties
        public float Get(LimitedPropertyType type, bool isCurrentValue)
        {
            LimitedProperty result = m_health;
            switch(type)
            {
                case LimitedPropertyType.Health:
                    result = m_health;
                    break;
            }

            if(isCurrentValue) return result.current;
            else return result.max;
        }
        
        public float Get(UnlimitedPropertyType type)
        {
            return type switch
            {
                UnlimitedPropertyType.Damage => m_damage.value,
                UnlimitedPropertyType.Speed => m_speed.value,
                UnlimitedPropertyType.Range => m_range.value,
                UnlimitedPropertyType.Interval => m_attackInterval.value,
            };
        }

        #endregion
        
        public void TakeDamage(float dmg)
        {
            this.m_health.current -= dmg;
            if(this.m_health.current <= 0) Die();
        }
    }
}