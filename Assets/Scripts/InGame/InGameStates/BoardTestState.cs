using InGame.Boards.Signals;
using InGame.UI;
using InGame.Cores;
using UnityEngine;

namespace InGame.InGameStates
{
    public class BoardTestState: InGameState
    {
        public override InGameStateType type => InGameStateType.BoardTestState;

        private TimeEffectManager m_timeEffectManager;
        private SignalController m_signalController;
        
        public override void Enter(InGameState last)
        {
            Debug.Log("Enter BoardTest State");
            
            var cameraManager = GameManager.Instance.GetCameraManager();
            cameraManager.BoardCameraSetActive(true);
            cameraManager.MiniBoardCameraSetActive(false);
            cameraManager.BattleCameraSetActive(false);
            
            m_timeEffectManager.Reset();
            m_signalController.Reset();
            
            m_timeEffectManager.TestStart();
            m_signalController.TestStart();

            BattleProgressUI.Instance.Hide();
            BattleResultUI.Instance.Hide();
            ChariotStatusUI.Instance.Show();
            NavigationBarUI.Instance.Show();
            BoardBarUI.Instance.Show();
        }

        public override void Exit()
        {
            Debug.Log("Exit BoardTest State");
            m_signalController.TestStop();
            m_timeEffectManager.TestStop();

            BattleProgressUI.Instance.Show();
            BattleResultUI.Instance.Show();
            ChariotStatusUI.Instance.Hide();
            NavigationBarUI.Instance.Hide();
            BoardBarUI.Instance.Hide();
        }
        

        public static BoardTestState CreateState(TimeEffectManager timeEffectManager, SignalController signalController)
        {
            return new BoardTestState
            {
                m_timeEffectManager = timeEffectManager,
                m_signalController = signalController
            };

        }
    }
}