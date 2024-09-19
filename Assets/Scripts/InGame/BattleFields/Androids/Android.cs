using System;
using UnityEngine;
using UnityEngine.Events;

using InGame.Views;
using InGame.Cores;
using InGame.InGameStates;
using InGame.BattleFields.Common;
using SetUps;
using Utils;
using Utils.Common;

namespace InGame.BattleFields.Androids
{
    public class Android : IPropertyRegisterable
    {
        [Header("View")]
        private AndroidView m_androidView;

        [Header("Properties")]       
        private LimitedProperty m_health;
        private LimitedProperty m_mod;
        private LimitedProperty m_crystal;
        private UnlimitedProperty m_defense;
        private LimitedProperty m_armor;
        private UnlimitedProperty m_speed;

        [Header("Equipment")]
        private EquipmentManager m_equipmentManager;
        
        public Android(AndroidSetUp setUp)
        {
            m_health = new LimitedProperty(
                setUp.maxHealth, 
                setUp.initialHealth, 
                LimitedPropertyType.Health
            );

            m_mod = new LimitedProperty(
                setUp.maxMod,
                setUp.initialMod,
                LimitedPropertyType.Mod
            );

            m_crystal = new LimitedProperty(
                setUp.maxCrystal,
                setUp.initialCrystal,
                LimitedPropertyType.Crystal
            );

            m_defense = new UnlimitedProperty(
                setUp.defense,
                UnlimitedPropertyType.Defense
            );

            m_armor = new LimitedProperty(
                setUp.maxArmor,
                setUp.armor,
                LimitedPropertyType.Armor
            );

            m_speed = new UnlimitedProperty(
                setUp.speed,
                UnlimitedPropertyType.Speed
            );

            m_equipmentManager = new();

            CreateView();
        }

        private void Die()
        {
            // m_equipmentManager.ClearEquipment();
            m_androidView.Die();
            GameManager.Instance.ChangeToBattleResultState(BattleResultType.Fail);
        }

        #region View
        public AndroidView androidView { get { return m_androidView;}}

        private void CreateView()
        {
            GameObject androidPref = Resources.Load<GameObject>(Constants.GO_ANDROID_PATH);
            GameObject androidGO = GameObject.Instantiate(androidPref);
            androidGO.transform.position = new(0, 0, Constants.ANDROID_DEPTH);

            m_androidView = androidGO.GetComponent<AndroidView>();
            m_androidView.Init(this);
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
                case LimitedPropertyType.Mod:
                    result = m_mod;
                    break;
                case LimitedPropertyType.Crystal:
                    result = m_crystal;
                    break;
                case LimitedPropertyType.Armor:
                    result = m_armor;
                    break;
            }
            
            if(isCurrentValue) return result.current;
            else return result.max;
        }

        public float Get(UnlimitedPropertyType type)
        {
            return type switch
            {
                UnlimitedPropertyType.Defense => m_defense.value,
                UnlimitedPropertyType.Speed => m_speed.value,
                _ => throw new NotImplementedException(),
            };
        }

        private LimitedProperty GetLimitedProperty(LimitedPropertyType type)
        {
            return type switch
            {
                LimitedPropertyType.Health => m_health,
                LimitedPropertyType.Mod => m_mod,
                LimitedPropertyType.Crystal => m_crystal,
                LimitedPropertyType.Armor => m_armor,
                _ => null
            };
        }

        private UnlimitedProperty GetUnlimitedProperty(UnlimitedPropertyType type)
        {
            return type switch
            {
                UnlimitedPropertyType.Defense => m_defense,
                UnlimitedPropertyType.Speed => m_speed,
                _ => null
            };
        }

        public void Set(LimitedPropertyType type, float val, bool isCurrentValue)
        {
            LimitedProperty property = GetLimitedProperty(type);
            if(isCurrentValue) property.current = val;
            else property.max = val;
        }

        public void Set(UnlimitedPropertyType type, float val)
        {
            UnlimitedProperty property = GetUnlimitedProperty(type);
            property.value = val;
        }

        public void Increase(LimitedPropertyType type, float delta, bool isCurrentValue)
        {
            LimitedProperty property = GetLimitedProperty(type);
            if(isCurrentValue) property.current += delta;
            else property.max += delta;
        }
        
        public void Increase(UnlimitedPropertyType type, float delta)
        {
            
            UnlimitedProperty property = GetUnlimitedProperty(type);
            property.value += delta;
            Debug.Log($"Increase {type}: {property.value}");
        }

        public void Decrease(LimitedPropertyType type, float delta, bool isCurrentValue)
        {
            LimitedProperty property = GetLimitedProperty(type);
            switch(type)
            {
                case LimitedPropertyType.Health:
                    if(isCurrentValue) DecreaseCurrHealth(delta);
                    else property.max -= delta;
                    break;
                default:
                    if(isCurrentValue) property.current -= delta;
                    else property.max -= delta;
                    break;
            }
        }

        public void Decrease(UnlimitedPropertyType type, float delta)
        {
            Debug.Log($"Decrease {type}: delta");
            UnlimitedProperty property = GetUnlimitedProperty(type);
            property.value -= delta;
            Debug.Log($"Increase {type}: {property.value}");
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
            UnlimitedProperty property = GetUnlimitedProperty(type);
            Debug.Assert(property != null, "UnlimitedProperty Type does not exist");
            property.onValueChanged.AddListener(call);
            property.onValueChanged.Invoke(property.value);
        }

        public void UnregisterPropertyEvent(UnlimitedPropertyType type, 
                                            UnityAction<float> call)
        {
            UnlimitedProperty property = GetUnlimitedProperty(type);
            Debug.Assert(property != null, "UnlimitedProperty Type does not exist");
            property.onValueChanged.RemoveListener(call);
        }
        #endregion

        #region Equipment

        public EquipmentManager GetEquipmentManager() => m_equipmentManager;

        #endregion

        #region Logic

        public void TakeDamage(float dmg)
        {
            dmg -= m_defense.value;
            dmg = Mathf.Max(dmg, 0);
            DecreaseCurrHealth(dmg);
            if(m_health.current <= 0) Die();
        }

        private void DecreaseCurrHealth(float delta)
        {
            float armor = m_armor.current;
            if(armor == 0)
                m_health.current -= delta;
            else if(armor >= delta)
                Decrease(LimitedPropertyType.Armor, delta, true);
            else if(armor < delta)
            {
                m_health.current -= delta - armor;
                m_armor.current = 0;
            }                        
        }

        #endregion

        public Vector2 GetPosition()
        {
            return m_androidView.GetPosition();
        }
        
        public void SetPosition(Vector2 newPos)
        {
            m_androidView.SetPosition(newPos);
        }
    }
}
