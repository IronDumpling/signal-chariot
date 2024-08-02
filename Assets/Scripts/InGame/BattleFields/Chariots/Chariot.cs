using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using InGame.Views;
using InGame.Cores;
using InGame.InGameStates;
using InGame.BattleFields.Common;
using SetUps;
using Utils;

namespace InGame.BattleFields.Chariots
{
    public class Chariot
    {
        [Header("View")]
        private ChariotView m_chariotView;

        [Header("Properties")]       
        private LimitedProperty m_health;
        private UnlimitedProperty m_armor;
        private UnlimitedProperty m_defence;
        private UnlimitedProperty m_speed;
        private UnlimitedProperty m_mod;

        [Header("Tower")]
        private TowerManager m_towerManager;
        
        public Chariot(ChariotSetUp setUp)
        {
            m_health = new LimitedProperty(
                setUp.maxHealth, 
                setUp.initialHealth, 
                LimitedPropertyType.Health
            );

            m_armor = new UnlimitedProperty(
                setUp.armor,
                UnlimitedPropertyType.Armor
            );

            m_defence = new UnlimitedProperty(
                setUp.defence,
                UnlimitedPropertyType.Defence
            );

            m_speed = new UnlimitedProperty(
                setUp.speed,
                UnlimitedPropertyType.Speed
            );

            m_mod = new UnlimitedProperty(
                setUp.mod,
                UnlimitedPropertyType.Mod
            );

            m_towerManager = new();

            CreateView();
        }

        private void Die()
        {
            m_chariotView.Die();
            GameManager.Instance.ChangeToBattleResultState(BattleResultType.Fail);
        }

        #region View
        public ChariotView chariotView { get { return m_chariotView;}}

        private void CreateView()
        {
            GameObject chariotPref = Resources.Load<GameObject>(Constants.GO_CHARIOT_PATH);
            GameObject chariotGO = GameObject.Instantiate(chariotPref);
            chariotGO.transform.position = new(0, 0, Constants.CHARIOT_DEPTH);

            m_chariotView = chariotGO.GetComponent<ChariotView>();
            m_chariotView.Init(this);
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
                UnlimitedPropertyType.Armor => m_armor.value,
                UnlimitedPropertyType.Mod => m_mod.value,
                UnlimitedPropertyType.Speed => m_speed.value,
                UnlimitedPropertyType.Defence => m_defence.value,
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

        private UnlimitedProperty GetUnlimitedProperty(UnlimitedPropertyType type)
        {
            return type switch
            {
                UnlimitedPropertyType.Armor => m_armor,
                UnlimitedPropertyType.Mod => m_mod,
                UnlimitedPropertyType.Speed => m_speed,
                UnlimitedPropertyType.Defence => m_defence,
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
            property.current += delta;
            if(isCurrentValue) property.current += delta;
            else property.max += delta;
        }
        
        public void Increase(UnlimitedPropertyType type, float delta)
        {
            UnlimitedProperty property = GetUnlimitedProperty(type);
            property.value += delta;
        }

        public void Decrease(LimitedPropertyType type, float delta, bool isCurrentValue)
        {
            LimitedProperty property = GetLimitedProperty(type);
            if(isCurrentValue) property.current -= delta;
            else property.max -= delta;
        }

        public void Decrease(UnlimitedPropertyType type, float delta)
        {
            UnlimitedProperty property = GetUnlimitedProperty(type);
            property.value -= delta;
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

        #region Tower

        public TowerManager GetTowerManager() => m_towerManager;

        #endregion

        #region Logic

        public void TakeDamage(float dmg)
        {
            if(m_armor.value >= dmg){
                m_armor.value -= dmg;
            }else if(m_armor.value > 0){
                dmg -= m_armor.value;
                m_armor.value = 0;
                m_health.current -= dmg;
            }else{
                m_health.current -= dmg;
            }

            if(m_health.current <= 0) Die();
        }

        #endregion 
    }
}
