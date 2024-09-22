using System.Linq;
using InGame.BattleFields.Androids;
using InGame.BattleFields.Common;
using InGame.Cores;
using UnityEngine;
using UnityEngine.UIElements;

using Utils.Common;

namespace UI
{
    public class AndroidStatusUI : IHidable
    {
        private VisualElement m_root;
        private VisualElement m_health;
        private ProgressBar m_healthBar;
        private VisualElement m_speed;
        private VisualElement m_armor;
        private ProgressBar m_armorBar;
        private VisualElement m_mod;
        private VisualElement m_crystal;
        
        public AndroidStatusUI(VisualElement root)
        {
            m_root = root;
            
            m_health = m_root.Q("health");
            m_healthBar = m_health.Q<ProgressBar>("bar");

            m_armor = m_root.Q("armor");
            m_armorBar = m_armor.Q<ProgressBar>("bar");

            m_speed = m_root.Q("speed");
            m_mod = m_root.Q("mod");
            m_crystal = m_root.Q("crystal");

            Register();
        }

        private void Register()
        {
            Android android = GameManager.Instance.GetAndroid();
            android.RegisterPropertyEvent(LimitedPropertyType.Health, SetHealthUI);
            android.RegisterPropertyEvent(LimitedPropertyType.Armor, SetArmorUI);
            android.RegisterPropertyEvent(UnlimitedPropertyType.Speed, SetSpeedUI);
            android.RegisterPropertyEvent(LimitedPropertyType.Mod, SetModUI);
            android.RegisterPropertyEvent(LimitedPropertyType.Crystal, SetCrystalUI);
        }

        ~AndroidStatusUI()
        {
            Android android = GameManager.Instance.GetAndroid();
            android.UnregisterPropertyEvent(LimitedPropertyType.Health, SetHealthUI);
            android.UnregisterPropertyEvent(LimitedPropertyType.Armor, SetArmorUI);
            android.UnregisterPropertyEvent(UnlimitedPropertyType.Speed, SetSpeedUI);
            android.UnregisterPropertyEvent(LimitedPropertyType.Mod, SetModUI);
            android.UnregisterPropertyEvent(LimitedPropertyType.Crystal, SetCrystalUI);
        }    

        private void SetHealthUI(float current, float max)
        {
            m_healthBar.highValue = max;
            m_healthBar.value = current;
            m_healthBar.title = $"{current}/{max}";
            
            var hp = m_healthBar.Q(className: ProgressBar.progressUssClassName);
            
            if(current/max < .2f)
            {
                hp.style.backgroundColor = Color.red;
                m_healthBar.style.color = Color.red;
            }
            else if(current/max < .6f)
            {
                hp.style.backgroundColor = Color.yellow;
                m_healthBar.style.color = Color.black;
            }
            else
            {
                hp.style.backgroundColor = Color.green;
                m_healthBar.style.color = Color.white;
            }
        }

        private void SetArmorUI(float current, float max)
        {
            m_armorBar.highValue = max;
            m_armorBar.value = current;
            m_armorBar.title = $"{current}/{max}";

            var armor = m_armorBar.Q(className: ProgressBar.progressUssClassName);
            armor.style.backgroundColor = Color.blue;
            m_armorBar.style.color = Color.white;
        }

        private void SetSpeedUI(float current)
        {
            Label content = m_speed.Q<Label>("content");
            content.text = $"速度: {current}";
        }

        private void SetModUI(float current, float max)
        {
            Label content = m_mod.Q<Label>("content");
            content.text = $"零件: {current}/{max}";
        }

        private void SetCrystalUI(float current, float max)
        {
            Label content = m_crystal.Q<Label>("content");
            content.text = $"晶体: {current}/{max}";
        }

        public void Hide()
        {
            m_root.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            m_root.style.display = DisplayStyle.Flex;
        }
    }
}