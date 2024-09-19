using UnityEngine;
using UnityEngine.UIElements;

using Utils.Common;

namespace UI
{
    public class ModuleInfoUI : IHidable
    {
        private VisualElement m_root;
        private VisualElement m_panel;
        private Label m_name;
        private Label m_category;
        private Label m_leftBox;
        private Label m_rightBox;
        private Label m_desc;
        private Label m_tag;
        private Label m_function;

        public ModuleInfoUI(VisualElement root)
        {
            m_root = root;
            m_panel = m_root.Q("panel");
            m_name = m_root.Q<Label>("name");
            m_category = m_root.Q<Label>("category");
            m_leftBox = m_root.Q<Label>("consumeNum");
            m_rightBox = m_root.Q<Label>("consumeType");
            m_desc = m_root.Q<Label>("description");
            m_tag = m_root.Q<Label>("tag");
            m_function = m_root.Q<Label>("functionality");
            ClearInfo();
        }

        public void SetInfo(string[] content)
        {
            m_name.text = content[0];
            m_category.text = content[1];
            m_leftBox.text = content[2];
            m_rightBox.text = content[3];
            m_desc.text = content[4];
            m_tag.text = content[5];
            m_function.text = content[6];
            Show();
        }

        public void ClearInfo()
        {
            m_name.text = "";
            m_category.text = "";
            m_leftBox.text = "";
            m_rightBox.text = "";
            m_desc.text = "";
            m_tag.text = "";
            m_function.text = "";
            Hide();
        }

        public void Hide()
        {
            m_root.style.visibility = Visibility.Hidden;
        }

        public void Show()
        {
            m_root.style.visibility = Visibility.Visible;
        }
    }
}