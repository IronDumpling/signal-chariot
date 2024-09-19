using UnityEngine;

using InGame.BattleFields.Common;
using InGame.Cores;
using InGame.Views;
using SetUps;
using UnityEngine.Events;
using Utils.Common;
using Audios;
using Utils;

namespace InGame.BattleFields.Enemies
{
    public class Enemy : IPropertyRegisterable
    {
        private LimitedProperty m_health;
        private UnlimitedProperty m_speed;
        private UnlimitedProperty m_damage;
        private UnlimitedProperty m_range;
        private UnlimitedProperty m_attackInterval;

        private UnityEvent m_dieCallBack = new UnityEvent();
        private int m_modQuantity;
        private int m_modQuality;
        
        public string name { get; private set; }
        public int typeID { get; private set; }

        private EnemyView m_viewPrefab;
        private EnemyView m_view = null;
        
        #region Life Cycle
        public EnemyView CreateView()
        {
            if (m_viewPrefab == null) return null;
            if (m_view != null) return m_view;
            
            // TODO: maybe also create the view in Enemy Lib?
            m_view = GameObject.Instantiate(m_viewPrefab);
            m_view.Init(this);
            return m_view;
        }
        
        public static Enemy CreateEnemy(Enemy other)
        {
            Enemy enemy = new Enemy
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
                ),
                m_modQuality = other.m_modQuality,
                m_modQuantity = other.m_modQuantity,
            };
            
            enemy.CreateView();
            enemy.RegisterDieCallBack(enemy.GenerateMod);
            return enemy;
        }
        
        public static Enemy CreateEnemy(EnemySetUp setUp, int typeID)
        {
            Enemy enemy = new Enemy
            {
                name = setUp.name,
                typeID = typeID,
                m_viewPrefab = setUp.enemyPrefab,
                m_health = new LimitedProperty(setUp.maxHealth, LimitedPropertyType.Health),
                m_damage = new UnlimitedProperty(setUp.attack, UnlimitedPropertyType.Damage),
                m_range = new UnlimitedProperty(setUp.attackRadius, UnlimitedPropertyType.Range),
                m_attackInterval = new UnlimitedProperty(setUp.attackDuration, UnlimitedPropertyType.Interval),
                m_speed = new UnlimitedProperty(setUp.speed, UnlimitedPropertyType.Speed),
                m_modQuality = setUp.modQuality,
                m_modQuantity = setUp.modQuantity,
            };
            
            enemy.RegisterDieCallBack(enemy.GenerateMod);
            return enemy;
        }

        public void Die()
        {
            m_dieCallBack.Invoke();
        }

        public void SelfDestroy()
        {
            m_view.Die();
            m_view = null;
            m_dieCallBack.RemoveAllListeners();
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
                _ => throw new System.NotImplementedException(),
            };
        }

        private LimitedProperty GetLimitedProperty(LimitedPropertyType type)
        {
            return type switch
            {
                LimitedPropertyType.Health => m_health,
                _ => null
            };
        }

        #endregion

        public void RegisterDieCallBack(UnityAction act)
        {
            if (act == null) return;
            m_dieCallBack.AddListener(act);
        }
        
        public void UnregisterDieCallBack(UnityAction act)
        {
            if (act == null) return;
            m_dieCallBack.RemoveListener(act);
        }

        public void RegisterPropertyEvent(LimitedPropertyType type, 
                                UnityAction<float, float> call)
        {
            LimitedProperty property = GetLimitedProperty(type);
            Debug.Assert(property != null, "LimitedProperty Type does not exist");
            property.onValueChanged.AddListener(call);
            property.onValueChanged.Invoke(property.current, property.max);
        }

        public void UnregisterPropertyEvent(LimitedPropertyType type, 
                                            UnityAction<float, float> call)
        {
            LimitedProperty property = GetLimitedProperty(type);
            Debug.Assert(property != null, "LimitedProperty Type does not exist");
            property.onValueChanged.RemoveListener(call);
        }

        public void RegisterPropertyEvent(UnlimitedPropertyType type, 
                                        UnityAction<float> call)
        {
            throw new System.NotImplementedException();
        }
        
        public void UnregisterPropertyEvent(UnlimitedPropertyType type, 
                                            UnityAction<float> call)
        {
            throw new System.NotImplementedException();
        }
        
        public EnemyView GetView() => m_view;

        public void SetPosition(Vector2 pos)
        {
            m_view.SetPosition(pos);
        }
        
        public void TurnOn()
        {
            m_view?.TurnOn();
        }

        public void TurnOff()
        {
            m_view?.TurnOff();
        }
        
        #region Action
        public void TakeDamage(float dmg)
        {
            AudioManager.Instance.PlaySound(Constants.AUDIO_ENEMY_DAMAGE);
            this.m_health.current -= dmg;
            if(this.m_health.current <= 0) Die();
        }

        private void GenerateMod()
        {
            Vector3 position = m_view.transform.position;
            for(int i = 0; i < m_modQuantity; i++)
            {
                float x = position.x + Random.Range(-1, 1);
                float y = position.y + Random.Range(-1, 1);
                GameManager.Instance.GetModManager().CreateMod(m_modQuality, new Vector2(x, y));
            }
        }
        #endregion

    }
}