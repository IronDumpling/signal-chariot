using Audios;
using InGame.Boards;
using InGame.Boards.Modules;
using InGame.Cores;
using InGame.Views;
using UI;
using UnityEngine;
using Utils;
using Utils.Common;

namespace InGame.InGameStates
{
    public class ModulePlacingState: InGameState
    {
        public override InGameStateType type => InGameStateType.ModulePlacingState;

        private Module m_module;
        private ModuleView m_moduleView;
        private Board m_board, m_extraBoard;
        private BoardView m_boardView;
        private bool m_exiting;

        public override void Enter(InGameState last)
        {
            Debug.Log("Enter Module placing state");
            m_moduleView = m_module.moduleView;
            m_exiting = false;
            
            var cameraManager = GameManager.Instance.GetCameraManager();
            cameraManager.SetBoardActive(true);
            cameraManager.SetBoardThumbnailActive(false);
            cameraManager.SetBattleActive(false);
            
            var boardCamera = cameraManager.boardCamera;
            GameManager.Instance.GetInputManager().RegisterMouseMoveEvent(boardCamera, OnMouseMove);
            GameManager.Instance.GetInputManager().RegisterRotateEvent(OnRotatePressed);
            GameManager.Instance.GetInputManager().RegisterClickEvent(boardCamera, OnClick);

            int bitmask = UIManager.Instance.GetDisplayBit();
            UIManager.Instance.SetDisplayUI(bitmask);

            m_moduleView.DisplayRange();
        }

        public override void Exit()
        {
            Debug.Log("Exit Module placing state");
            var boardCamera = GameManager.Instance.GetCameraManager().boardCamera;
            GameManager.Instance.GetInputManager().UnregisterMouseMoveEvent(boardCamera, OnMouseMove);
            GameManager.Instance.GetInputManager().UnregisterRotateEvent(OnRotatePressed);
            GameManager.Instance.GetInputManager().UnregisterClickEvent(boardCamera, OnClick);

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BoardConsole,
                UIElements.BattleConsole,
                UIElements.BattleResult
            );
            UIManager.Instance.SetDisplayUI(bitmask);

            m_moduleView.HideRange();
        }

        private void OnMouseMove(Vector2 worldPos)
        {
            if (m_exiting) return;
            var pos = new Vector3(worldPos.x, worldPos.y, Constants.PLACING_MODULE_DEPTH);
            m_moduleView.SetGlobalWorldPos(pos);
        }

        private void OnClick(Vector2 worldPos)
        {
            if (!m_boardView.GetXY(worldPos, out int x, out int y, out bool isNormal)) return;

            BoardPosition pivotPos;
            pivotPos.x = x;
            pivotPos.y = y;

            var board = isNormal ? m_board : m_extraBoard;

            if (board.PlaceModule(m_module, pivotPos))
            {
                m_exiting = true;
                var pos = m_boardView.GetSlotCenterWorldPosition(pivotPos, isNormal);
                pos.z = Constants.MODULE_DEPTH;
                
                m_moduleView.SetWorldPos(pos);
                AudioManager.Instance.PlaySound(Constants.AUDIO_MODULE_DROP);
                GameManager.Instance.ChangeToBoardWaitingState();
            }
            else
            {
                Debug.Log("Can not place there");
            }
        }

        private void OnRotatePressed()
        {
            m_moduleView.Rotate();
        }

        public static ModulePlacingState CreateState(Board board, Board extraBoard, BoardView boardView, Module module)
        {
            return new ModulePlacingState
            {
                m_board = board,
                m_extraBoard = extraBoard,
                m_boardView = boardView,
                m_module = module
            };
        }
    }
}