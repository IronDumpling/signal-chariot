using System.Collections.Generic;
using UnityEngine;

using InGame.Cores;
using InGame.UI;
using InGame.Views;
using InGame.BattleFields.Androids;
using InGame.BattleFields.Enemies;
using Utils.Common;

namespace InGame.InGameStates
{
    public class BattleState : InGameState
    {
        public override InGameStateType type => InGameStateType.BattleState;
        private AndroidView m_androidView;
        private EnemySpawnController m_enemySpawnController;

        public override void Enter(InGameState last)
        {
            Debug.Log("Enter battle");
            // Todo: put this in preparing big state
            GameManager.Instance.GetGeneralBoard().RemoveAllExtraBoardModules();
            
            
            #region Camera
            var boardView = GameManager.Instance.GetBoardView();
            boardView.GetActiveBoardCornerPos(out var minPos, out var maxPos);
            
            var center = (minPos + maxPos) / 2;
            var delta = maxPos - minPos;
            var length = Mathf.Max(delta.x, delta.y);
            
            var cameraManager = GameManager.Instance.GetCameraManager();
            cameraManager.SetBoardActive(false);
            
            cameraManager.SetBoardThumbnailActive(true);
            cameraManager.SetBoardThumbnailPosition(center);
            cameraManager.SetBoardThumbnailSize(length / 2);
            
            cameraManager.SetBattleActive(true);
            #endregion
            
            GameManager.Instance.GetInputManager().RegisterMoveEvent(OnMoveKeyPressed);

            var timeEffectManager = GameManager.Instance.GetTimeEffectManager();
            var signalController = GameManager.Instance.GetSignalController();
            timeEffectManager.Reset();
            signalController.Reset();
            
            timeEffectManager.Start();
            signalController.Start();
            
            m_enemySpawnController.GoNextWave();
            m_enemySpawnController.RegisterWaveFinishCallBack(OnWaveFinished);
            m_enemySpawnController.Start();

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BattleConsole
            );
            UIManager.Instance.SetDisplayUI(bitmask);
        }
        
        public override void Exit()
        {
            Debug.Log("Exit battle");
            GameManager.Instance.GetGeneralBoard().Reset();
            
            GameManager.Instance.GetInputManager().UnregisterMoveEvent(OnMoveKeyPressed);
            
            
            var timeEffectManager = GameManager.Instance.GetTimeEffectManager();
            var signalController = GameManager.Instance.GetSignalController();
            timeEffectManager.Stop();
            signalController.Stop();
            
            m_androidView.SetMoveDirection(Vector2.zero);
            m_enemySpawnController.UnregisterWaveFinishCallBack(OnWaveFinished);
            m_enemySpawnController.Stop();
            
            
            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BattleResult,
                UIElements.BoardConsole
            );
            UIManager.Instance.SetDisplayUI(bitmask);
        }
        
        private void OnMoveKeyPressed(Vector2 inputDirection)
        {
            m_androidView.SetMoveDirection(inputDirection);
        }

        private void OnWaveFinished(bool isLastWave, List<int> moduleRewards)
        {
            GameManager.Instance.ChangeToRewardState(isLastWave, moduleRewards);
            
        }
        

        public static BattleState CreateState(Android android, AndroidView androidView, EnemySpawnController enemySpawnController)
        {
            var state = new BattleState
            {
                m_androidView = androidView,
                m_enemySpawnController = enemySpawnController
            };
            return state;
        }
    }
}