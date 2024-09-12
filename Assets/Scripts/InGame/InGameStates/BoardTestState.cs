﻿using InGame.BattleFields.Enemies;
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
        private GeneralSignalController m_signalController;
        private Vector2 m_androidPos;
        private Enemy m_firstEnemy, m_secondEnemy;
        
        public override void Enter(InGameState last)
        {
            Debug.Log("Enter BoardTest State");
            
            GameManager.Instance.GetGeneralBoard().Reset();
            
            var cameraManager = GameManager.Instance.GetCameraManager();
            cameraManager.BoardCameraSetActive(true);
            cameraManager.MiniBoardCameraSetActive(false);
            cameraManager.BattleCameraSetActive(false);
            
            m_timeEffectManager.Reset();
            m_signalController.Reset();
            
            // To Do: No Test Anymore
            m_timeEffectManager.Start();
            m_signalController.Start();

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BoardConsole
            );
            UIManager.Instance.SetDisplayUI(bitmask);
            
            // To do: Put this to a larger state
            var android = GameManager.Instance.GetAndroid();
            m_androidPos = android.GetPosition();
            android.SetPosition(new Vector2(30, 30));

            var enemySpawnController = GameManager.Instance.GetEnemySpawnController();
            
            var enemyLib = GameManager.Instance.GetEnemyLib();
            m_firstEnemy = enemySpawnController.GenerateEnemy(2);;
            m_secondEnemy = enemySpawnController.GenerateEnemy(2);;
            
            m_firstEnemy.SetPosition(new Vector2(32, 32));
            m_secondEnemy.SetPosition(new Vector2(28, 32));
            
        }

        public override void Exit()
        {
            Debug.Log("Exit BoardTest State");
            
            m_signalController.Stop();
            m_timeEffectManager.Stop();

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BattleConsole,
                UIElements.BattleResult,
                UIElements.ModuleInfoCard
            );
            UIManager.Instance.SetDisplayUI(bitmask);
            
            GameManager.Instance.GetAndroid().SetPosition(m_androidPos);
            
            m_firstEnemy.Die();
            m_secondEnemy.Die();
        }
        

        public static BoardTestState CreateState(TimeEffectManager timeEffectManager, GeneralSignalController signalController)
        {
            return new BoardTestState
            {
                m_timeEffectManager = timeEffectManager,
                m_signalController = signalController
            };

        }
    }
}