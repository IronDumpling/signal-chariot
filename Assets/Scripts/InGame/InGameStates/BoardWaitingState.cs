using InGame.Boards;
using InGame.Boards.Modules;
using InGame.Cores;
using UI;
using InGame.Views;
using UnityEngine;
using Utils;

namespace InGame.InGameStates
{
    public class BoardWaitingState: InGameState
    {
        public override InGameStateType type => InGameStateType.BoardWaitingState;
        private BoardView m_boardView;
        private Board m_board, m_extraBoard;
        private ModuleInfoDisplayManager m_displayManager;
        private Vector2 m_androidPos;

        
        private float m_notMovingAccumulatedTime = 0f;
        private const float SelectThreshold = Constants.SELECT_THRESHOLD;
        private Module m_currentLookingModule = null;
        private Module m_prevModule = null;
        private Vector2 m_currentWorldPosition;

        public override void Enter(InGameState last)
        {
            Debug.Log("Enter board waiting");
            var cameraManager = GameManager.Instance.GetCameraManager();

            cameraManager.SetBoardActive(true);
            cameraManager.SetBoardThumbnailActive(false);
            cameraManager.SetBattleActive(false);

            var boardCamera = cameraManager.boardCamera;

            GameManager.Instance.GetInputManager().RegisterClickEvent(boardCamera, OnClicked);
            GameManager.Instance.GetInputManager().RegisterMouseMoveEvent(boardCamera, OnMouseMove);
            
            m_displayManager.Start();

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BoardConsole
            );
            UIManager.Instance.SetDisplayUI(bitmask);
            
            // To do: Put this to a larger state
            var android = GameManager.Instance.GetAndroid();
            m_androidPos = android.GetPosition();
            android.SetPosition(new Vector2(30, 30));
        }

        public override void Exit()
        {
            Debug.Log("Exit board waiting");
            var boardCamera = GameManager.Instance.GetCameraManager().boardCamera;
            GameManager.Instance.GetInputManager().UnregisterClickEvent(boardCamera, OnClicked);
            GameManager.Instance.GetInputManager().UnregisterMouseMoveEvent(boardCamera, OnMouseMove);
            
            if (m_prevModule != null)
            {
                m_displayManager.UndisplayModule(m_prevModule);
                m_prevModule = null;
            }
            m_displayManager.Stop();

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BattleConsole,
                UIElements.BattleResult
            );
            UIManager.Instance.SetDisplayUI(bitmask);
            
            GameManager.Instance.GetAndroid().SetPosition(m_androidPos);
        }

        private void OnClicked(Vector2 worldPosition)
        {
            if (!GetBoardPosition(worldPosition, out int x, out int y, out Board board)) return;
            
            if (board.GetSlotStatus(x, y) == SlotStatus.Occupied)
            {
                var module = board.RemoveModule(x, y);

                GameManager.Instance.ChangeToModulePlacingState(module);
            }
        }

        private bool GetBoardPosition(Vector2 worldPosition, out int x, out int y, out Board board)
        {
            x = -1;
            y = -1;
            board = null;
            if (!m_boardView.GetXY(worldPosition, out x, out y, out bool isNormal)) return false;
            
            board = isNormal ? m_board : m_extraBoard;

            return true;
        }
        
        private void OnMouseMove(Vector2 worldPosition)
        {
            m_notMovingAccumulatedTime = 0f;
            m_currentWorldPosition = worldPosition;
        }

        private void FindModule()
        {
            if (m_currentLookingModule == null)
            {
                if (m_notMovingAccumulatedTime >= SelectThreshold)
                {
                    // not on any slot
                    if (!GetBoardPosition(m_currentWorldPosition, out int x, out int y, out Board board)) return;
                    
                    var module = board.GetModule(x, y);
                    
                    // not on any module
                    if (module == null) return;

                    m_currentLookingModule = module;

                }
                else
                {
                    m_notMovingAccumulatedTime += Time.deltaTime;
                }
            }
            else
            {
                if (!GetBoardPosition(m_currentWorldPosition, out int x, out int y, out Board board))
                {
                    m_currentLookingModule = null;
                }
                else
                {
                    m_currentLookingModule = board.GetModule(x, y);
                    
                }
            }
        }

        private void DisplayModule()
        {
            
            if (m_currentLookingModule != m_prevModule)
            {
                if (m_prevModule != null)
                {
                    m_displayManager.UndisplayModule(m_prevModule);
                    m_currentLookingModule = null;
                    m_prevModule = null;
                }else
                {
                    m_displayManager.DisplayModule(m_currentLookingModule);
                    m_prevModule = m_currentLookingModule;
                }
            }
        }
        public override void Update()
        {
            FindModule();
            DisplayModule();
        }

        public static BoardWaitingState CreateState(Board board, Board extraBoard, BoardView boardView)
        {
            var state = new BoardWaitingState
            {
                m_board = board,
                m_extraBoard = extraBoard,
                m_boardView = boardView,
                m_displayManager = GameManager.Instance.GetModuleInfoDisplayManager()
            };
            return state;
        }
    }
}