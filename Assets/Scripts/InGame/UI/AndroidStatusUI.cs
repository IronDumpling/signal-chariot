using InGame.BattleFields.Androids;
using InGame.BattleFields.Common;
using InGame.Cores;

using UnityEngine.UIElements;

using Utils.Common;

namespace InGame.UI
{
    public class AndroidStatusUI : IHidable
    {
        private VisualElement m_root;
        private VisualElement m_health;
        private VisualElement m_defense;
        private VisualElement m_armor;
        private VisualElement m_mod;
        private VisualElement m_crystal;
        
        public AndroidStatusUI(VisualElement root)
        {
            m_root = root;
            m_health = m_root.Q("health");
            m_defense = m_root.Q("defense");
            m_armor = m_root.Q("armor");
            m_mod = m_root.Q("mod");
            m_crystal = m_root.Q("crystal");
            Register();
        }

        private void Register()
        {
            Android android = GameManager.Instance.GetAndroid();
            android.RegisterPropertyEvent(LimitedPropertyType.Health, SetHealthUI);
            android.RegisterPropertyEvent(UnlimitedPropertyType.Defense, SetDefenseUI);
            android.RegisterPropertyEvent(LimitedPropertyType.Armor, SetArmorUI);
            android.RegisterPropertyEvent(LimitedPropertyType.Mod, SetModUI);
            android.RegisterPropertyEvent(LimitedPropertyType.Crystal, SetCrystalUI);
        }

        ~AndroidStatusUI()
        {
            Android android = GameManager.Instance.GetAndroid();
            android.UnregisterPropertyEvent(LimitedPropertyType.Health, SetHealthUI);
            android.UnregisterPropertyEvent(UnlimitedPropertyType.Defense, SetDefenseUI);
            android.UnregisterPropertyEvent(LimitedPropertyType.Armor, SetArmorUI);
            android.UnregisterPropertyEvent(LimitedPropertyType.Mod, SetModUI);
            android.UnregisterPropertyEvent(LimitedPropertyType.Crystal, SetCrystalUI);
        }        

        private void SetHealthUI(float current, float max)
        {
            ProgressBar bar = m_health.Q<ProgressBar>("bar");
            bar.highValue = max;
            bar.value = current;
            bar.title = $"{current}/{max}";
        }

        private void SetDefenseUI(float current)
        {
            Label content = m_defense.Q<Label>("content");
            content.text = $"防御: {current}";
        }

        private void SetArmorUI(float current, float max)
        {
            Label content = m_armor.Q<Label>("content");
            content.text = $"护甲: {current}/{max}";
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