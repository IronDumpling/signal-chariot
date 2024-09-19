using UnityEngine;
using UnityEngine.UIElements;

using InGame;
using UI;

using Utils.Common;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private UIDocument m_doc;
        private VisualElement m_root;
        private Button m_startButton;
        private Button m_quitButton;
        private Button m_tutButton;

        private void Start()
        {
            m_root = m_doc.rootVisualElement;
            
            Register();

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.Tutorial
            );
            UIManager.Instance.RemoveDisplayUI(bitmask);
        }

        private void Register()
        {
            m_startButton = m_root.Q<Button>("start");
            m_tutButton = m_root.Q<Button>("tutorial");
            m_quitButton = m_root.Q<Button>("quit");

            m_startButton.clicked += () => {
                OnStartPressed();
            };

            m_tutButton.clicked += () => {
                OnTutPressed();
            };

            m_quitButton.clicked += () => {
                OnQuitPressed();
            };
        }

        private void OnStartPressed()
        {
            Game.Instance.nextState = new WorldState();
        }

        private void OnTutPressed()
        {
            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.Tutorial
            );
            UIManager.Instance.AddDisplayUI(bitmask);
        }

        private void OnQuitPressed()
        {
            Application.Quit();
        }
    }
}