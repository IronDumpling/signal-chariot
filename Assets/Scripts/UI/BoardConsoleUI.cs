
using UnityEngine;
using UnityEngine.UIElements;

using InGame.Cores;
using Utils.Common;
using InGame.InGameStates;

namespace UI
{
    public class BoardConsoleUI : MonoBehaviour, IHidable
    {
        [SerializeField] private UIDocument m_doc;
        private VisualElement m_root;
        private Button m_testButton;
        private Button m_slotButton;
        private Button m_exitButton;
        private Button m_marchButton;
        private VisualElement m_log;
        private ModuleInfoUI m_moduleInfoUI;
        public ModuleInfoUI moduleInfoUI { get { return m_moduleInfoUI;}}

        private void Awake()
        {
            m_root = m_doc.rootVisualElement;
            Register();
        }

        private void Start()
        {
            m_log = m_root.Q("log");
            m_moduleInfoUI = new ModuleInfoUI(m_root.Q("module"));
            new AndroidStatusUI(m_root.Q("status")); // stay in start
        }

        private void Register()
        {
            m_testButton = m_root.Q<Button>("test");
            m_slotButton = m_root.Q<Button>("add");
            m_exitButton = m_root.Q<Button>("exit");
            m_marchButton = m_root.Q<Button>("march");

            m_testButton.clicked += () => {
                OnSignalClicked();
            };

            m_slotButton.clicked += () => {
                OnSlotClicked();
            };

            m_exitButton.clicked += () => {
                GameManager.Instance.ChangeToInitState();
            };

            m_marchButton.clicked += () => {
                GameManager.Instance.ChangeToBattleState();
            };
        }

        private void OnSignalClicked()
        {   
            if(GameManager.Instance.CheckGameState(new[]{InGameStateType.BoardTestState})){
                m_slotButton.text = "扩容";
                m_testButton.text = "测试";
                GameManager.Instance.ChangeToBoardWaitingState();
            }else{
                m_slotButton.text = "扩容";
                m_testButton.text = "退出测试";
                GameManager.Instance.ChangeToBoardTestState();
            }
        }

        private void OnSlotClicked()
        {
            if(GameManager.Instance.CheckGameState(new[]{InGameStateType.AddSlotState})){
                m_slotButton.text = "扩容";
                m_testButton.text = "测试";
                GameManager.Instance.ChangeToBoardWaitingState();
            }else{
                m_slotButton.text = "退出扩容";
                m_testButton.text = "测试";
                GameManager.Instance.ChangeToAddSlotState();
            }
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
