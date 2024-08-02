using InGame.BattleFields.Chariots;
using InGame.BattleFields.Common;
using InGame.Cores;
using UnityEngine;
using UnityEngine.UIElements;

using Utils.Common;

namespace InGame.UI
{
    public class ChariotStatusUI : MonoSingleton<ChariotStatusUI>, IHidable
    {
        [SerializeField] private UIDocument m_doc;
        private VisualElement m_root;
        private VisualElement m_health;
        private VisualElement m_armor;
        private VisualElement m_mod;
        
        private void Awake()
        {
            m_root = m_doc.rootVisualElement;
            m_health = m_root.Q("health");
            m_armor = m_root.Q("armor");
            m_mod = m_root.Q("mod");
        }

        private void Start()
        {
            Chariot chariot = GameManager.Instance.GetChariot();
            chariot.RegisterPropertyEvent(LimitedPropertyType.Health, SetHealthUI);
            chariot.RegisterPropertyEvent(UnlimitedPropertyType.Armor, SetArmorUI);
            chariot.RegisterPropertyEvent(UnlimitedPropertyType.Mod, SetModUI);
        }

        private void SetHealthUI(float current, float max)
        {
            ProgressBar bar = m_health.Q<ProgressBar>("bar");
            bar.highValue = max;
            bar.value = current;
            bar.title = $"{current}/{max}";
        }

        private void SetArmorUI(float current)
        {
            Label content = m_armor.Q<Label>("content");
            content.text = $"ARMOR: {current}";
        }

        private void SetModUI(float current)
        {
            Label content = m_mod.Q<Label>("content");
            content.text = $"MOD: {current}";
        }

        public void Hide()
        {
            m_doc.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            m_doc.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}