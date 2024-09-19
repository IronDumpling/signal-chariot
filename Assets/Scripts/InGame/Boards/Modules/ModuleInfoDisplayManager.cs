using UI;

namespace InGame.Boards.Modules
{
    public class ModuleInfoDisplayManager
    {
        private bool m_isOn;
        
        public ModuleInfoDisplayManager()
        {
            
        }

        public void Start()
        {
            m_isOn = true;
        }

        public void Stop()
        {
            m_isOn = false;
        }

        public void DisplayModule(Module module)
        {
            if (!m_isOn) return;
            BoardConsoleUI boardConsoleUI = UIManager.Instance.GetUI(UIElements.BoardConsole) as BoardConsoleUI;
            string[] content = new string[7]{
                module.name,
                module.category.ToString(),
                module.leftBox,
                module.rightBox,
                module.desc,
                module.tag,
                module.function
            };
            if (boardConsoleUI != null) boardConsoleUI.moduleInfoUI.SetInfo(content);
        }

        public void UndisplayModule(Module module)
        {
            if (!m_isOn) return;
            BoardConsoleUI boardConsoleUI = UIManager.Instance.GetUI(UIElements.
            BoardConsole) as BoardConsoleUI;
            if (boardConsoleUI != null) boardConsoleUI.moduleInfoUI.ClearInfo();
        }
    }
}