using UnityEngine;

using InGame.Cores;
using UI;

namespace InGame.InGameStates
{
    public enum BattleResultType
    {
        WaveWin,
        BattleWin,
        GameWin,
        Fail
    }

    public class BattleResultState : InGameState
    {
        public override InGameStateType type => InGameStateType.BattleResultState;

        private BattleResultType m_resultType;

        public override void Enter(InGameState last)
        {
            var cameraManager = GameManager.Instance.GetCameraManager();
            cameraManager.SetBoardActive(false);
            cameraManager.SetBoardThumbnailActive(false);
            cameraManager.SetBattleActive(true);
            
            Debug.Log("Enter battle result");

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BattleResult
            );
            UIManager.Instance.SetDisplayUI(bitmask);
            
            BattleResultUI battleResultUI = UIManager.Instance.GetUI(UIElements.BattleResult) as BattleResultUI;
            if(battleResultUI != null) 
                battleResultUI.Switch(m_resultType);
        }
        
        public override void Exit()
        {
            Debug.Log("Exit battle result");

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BattleConsole,
                UIElements.BoardConsole
            );
            UIManager.Instance.SetDisplayUI(bitmask);
        }

        public static BattleResultState CreateState(BattleResultType resultType)
        {
            var state = new BattleResultState
            {
                m_resultType = resultType,
            };
            return state;
        }
    }
}