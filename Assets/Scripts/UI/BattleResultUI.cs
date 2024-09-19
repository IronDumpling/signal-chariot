using UnityEngine;
using UnityEngine.UIElements;

using InGame.Cores;
using InGame.InGameStates;
using Utils.Common;
using Utils;

namespace UI
{
    public class BattleResultUI : MonoBehaviour, IHidable
    {
        [SerializeField] private UIDocument m_doc;

        private VisualElement m_waveWin;
        private VisualElement m_battleWin;
        private VisualElement m_fail;

        private void Awake()
        {
            m_waveWin = Resources.Load<VisualTreeAsset>(Constants.UI_WAVE_WIN_PATH).Instantiate().Q("panel");
            m_battleWin = Resources.Load<VisualTreeAsset>(Constants.UI_BATTLE_WIN_PATH).Instantiate().Q("panel");
            m_fail = Resources.Load<VisualTreeAsset>(Constants.UI_FAIL_PATH).Instantiate().Q("panel");

            Register();
            
            VisualElement root = m_doc.rootVisualElement;
            root.Add(m_waveWin);
            root.Add(m_battleWin);
            root.Add(m_fail);
        }

        private void Register()
        {
            Button waveWinContinue = m_waveWin.Q<Button>("continue");
            Button battleWinContinue = m_battleWin.Q<Button>("continue");
            Button failContinue = m_fail.Q<Button>("continue");

            waveWinContinue.clicked += () => 
            {
                GameManager.Instance.ChangeToBoardWaitingState();
            };

            battleWinContinue.clicked += () => 
            {
                GameManager.Instance.ChangeToInitState(); // TODO: Go to next level
            };

            failContinue.clicked += () => 
            {
                GameManager.Instance.ChangeToInitState();
            };
        }

        public void Hide()
        {
            m_doc.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            m_doc.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        public void Switch(BattleResultType type)
        {
            m_waveWin.style.display = DisplayStyle.None;
            m_battleWin.style.display = DisplayStyle.None;
            m_fail.style.display = DisplayStyle.None;

            switch (type)
            {
                case BattleResultType.WaveWin:
                    m_waveWin.style.display = DisplayStyle.Flex;
                    break;
                case BattleResultType.BattleWin:
                    m_battleWin.style.display = DisplayStyle.Flex;
                    break;
                case BattleResultType.GameWin:
                    break;
                case BattleResultType.Fail:
                    m_fail.style.display = DisplayStyle.Flex;
                    break;
            }
        }
    }
}