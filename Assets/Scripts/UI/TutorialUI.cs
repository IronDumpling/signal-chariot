using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UIElements;

using Utils.Common;

namespace UI
{
    public class TutorialUI : MonoBehaviour, IHidable
    {
        [SerializeField] private UIDocument m_doc;
        private VisualElement m_root;
        private VisualElement m_tutorial;
        private Button m_close;
        private Button m_right;
        private Button m_left;

        [SerializeField] private string m_tutorialBasePath;
        private Texture2D[] m_textures;
        private int m_currentTutIndex = -1;

        private void Awake()
        {
            m_root = m_doc.rootVisualElement;
            m_tutorial = m_root.Q("tutorial");
            m_close = m_root.Q<Button>("close");
            m_right = m_root.Q<Button>("right");
            m_left = m_root.Q<Button>("left");

            InitTutorials();
            Register();
        }

        private void InitTutorials()
        {
            m_textures = Resources.LoadAll<Texture2D>(m_tutorialBasePath);

            if(m_textures.Length > 0)
            {
                m_currentTutIndex = 0;
                SetTutorial();
            }
        }

        private void Register()
        {
            m_close.clicked += () => {
                int bitmask = UIManager.Instance.GetDisplayBit(
                    UIElements.Tutorial
                );
                UIManager.Instance.RemoveDisplayUI(bitmask);
            };

            m_right.clicked += () => {
                if(m_textures.Length == 0) return;
                int newIdx = (m_currentTutIndex - 1) % m_textures.Length;
                m_currentTutIndex = (newIdx < 0) ? (m_textures.Length - 1) : newIdx;
                SetTutorial();
            };

            m_left.clicked += () => {
                if(m_textures.Length == 0) return;
                m_currentTutIndex = (m_currentTutIndex + 1) % m_textures.Length;
                SetTutorial();
            };
        }

        private void SetTutorial()
        {
            if(m_tutorial == null) return;
            m_tutorial.style.backgroundImage = new StyleBackground(m_textures[m_currentTutIndex]);
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