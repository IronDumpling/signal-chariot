using InGame.Cores;
using InGame.InGameStates;
using UnityEngine.SceneManagement;
using Utils;
using Utils.Common;

namespace InGame
{
    public class WorldState : GameState
    {
        private static WorldState _instance;

        public static WorldState instance => _instance;

        private StateMachine<InGameState> m_gameStateMachine = new();

        public InGameState currentState => m_gameStateMachine.current;

        public InGameState nextState
        {
            set => m_gameStateMachine.next = value;
        }

        public override void Enter(GameState last)
        {
            _instance = this;
            SceneManager.LoadScene(Constants.LEVEL0);
        }

        public override void Exit()
        {
            m_gameStateMachine.current?.Exit();
            GameManager.Instance.Clear();
            _instance = null;
        }

        public override void Update()
        {
            var state = m_gameStateMachine.current;

            if (state != null)
            {
                m_gameStateMachine.isLocked = true;
                state.Update();
                m_gameStateMachine.isLocked = false;
            }
        }

        public override void LateUpdate()
        {
            var state = m_gameStateMachine.current;
            if (state != null)
            {
                m_gameStateMachine.isLocked = true;
                state.LateUpdate();
                m_gameStateMachine.isLocked = false;
            }
        }
    }
}