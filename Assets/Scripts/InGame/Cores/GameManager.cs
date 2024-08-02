using System;

using UnityEngine;
using UnityEngine.InputSystem;

using InGame.Boards;
using InGame.Boards.Modules;
using InGame.Boards.Signals;
using InGame.BattleFields.Chariots;
using InGame.BattleFields.Enemies;
using InGame.Cameras;
using InGame.InGameStates;
using InGame.Views;

using SetUps;
using Utils.Common;
using Utils;
using Time = InGame.Boards.Signals.Time;

namespace InGame.Cores
{
    public class GameManager: MonoSingleton<GameManager>
    {   
        [SerializeField] private PlayerInput m_playerInput;
        [SerializeField] private SetUp m_setUp;

        [Header("Managers")]
        private InputManager m_inputManager;
        private TimeEffectManager m_timeEffectManager;
        private CameraManager m_cameraManager;

        [Header("Chariot")]
        private Chariot m_chariot;
        private ChariotView m_chariotView;

        [Header("Board")]
        // private Board m_board;
        // private Board m_extraBoard;
        // private BoardView m_boardView;
        private GeneralBoard m_generalBoard;
        
        private ModuleLib m_moduleLib;
        private SignalController m_signalController;
        
        [Header("Enemy")]
        private EnemyManager m_enemyManager;

        private EnemyLib m_enemyLib;

        protected override void Init()
        {
            m_inputManager = new InputManager(m_playerInput);
            m_timeEffectManager = TimeEffectManager.CreateTimeEffectManager();

            InitChariot();
            InitBoard();
            InitCamera();
            InitEnemy();

            ChangeToBoardWaitingState(); // initial state is board preparation
        }

        private void InitEnemy()
        {
            m_enemyLib = new EnemyLib(m_setUp.enemyLibrary);
            m_enemyManager = new EnemyManager(m_setUp.enemySpawnLevels);
        }

        private void InitChariot()
        {
            m_chariot = new Chariot(m_setUp.chariotSetUp);
            m_chariotView = m_chariot.chariotView;
        }

        private void InitBoard()
        {
            m_moduleLib = new ModuleLib();
            m_moduleLib.Init(m_setUp.moduleLibrary);

            var board = new Board(m_setUp.boardSetUp);

            var extraBoard = new Board(m_setUp.extraBoardSetUp, SlotStatus.Empty, true);
            
            GameObject boardPref = Resources.Load<GameObject>(Constants.GO_BOARD_PATH);
            GameObject boardGO = Instantiate(boardPref);
            var boardView = boardGO.GetComponent<BoardView>();
            boardView.Init(board, extraBoard, m_setUp.boardSetUp, m_setUp.extraBoardSetUp);
            
            m_signalController = SignalController.CreateSignalController(board, boardView);
            m_generalBoard = GeneralBoard.CreateGeneralBoard(board, extraBoard, boardView);
        }

        private void InitCamera()
        {
            var cameraPrefab = Resources.Load<CameraManager>(Constants.GO_CAMERA_PATH);
            m_cameraManager = Instantiate(cameraPrefab);
            m_cameraManager.transform.position = Vector3.zero;

            m_cameraManager.SetBattleCameraFollow(m_chariotView?.gameObject);
            
            m_cameraManager.SetMiniBoardCameraPosition(GetBoardView().transform.position);
            m_cameraManager.SetBoardCameraPosition(GetBoardView().transform.position);
        }

        public void Update()
        {
            m_signalController?.Update(UnityEngine.Time.deltaTime, UnityEngine.Time.time);
            m_timeEffectManager?.Update(UnityEngine.Time.deltaTime, UnityEngine.Time.time);
        }

        #region Getters
        public CameraManager GetCameraManager() => m_cameraManager;
        public InputManager GetInputManager() => m_inputManager;
        public ModuleLib GetModuleLib() =>  m_moduleLib;
        public SignalController GetSignalController() => m_signalController;
        public TimeEffectManager GetTimeEffectManager() => m_timeEffectManager;
        public Board GetBoard() => m_generalBoard.board;
        public Board GetExtraBoard() => m_generalBoard.extraBoard;
        public GeneralBoard GetGeneralBoard() => m_generalBoard;
        public BoardView GetBoardView() => m_generalBoard.boardView;
        
        public Chariot GetChariot() => m_chariot;
        public InGameStateType GetCurrentInGameState() => WorldState.instance.currentState.type;
        #endregion

        #region World State Machine
        public void ChangeToBoardWaitingState()
        {
            WorldState.instance.nextState = BoardWaitingState.CreateState(GetBoard(), GetExtraBoard(), GetBoardView());
        }
        
        public void ChangeToAddSlotState()
        {
            WorldState.instance.nextState = AddSlotState.CreateAddSlotState(GetBoardView(), GetBoard(), Int32.MaxValue);
        }

        public void ChangeToModulePlacingState(Module module)
        {
            WorldState.instance.nextState = ModulePlacingState.CreateState(GetBoard(), GetExtraBoard(), GetBoardView(), module);
        }

        public void ChangeToBoardTestState()
        {
            WorldState.instance.nextState = BoardTestState.CreateState(m_timeEffectManager, m_signalController);
        }

        public void ChangeToBattleState()
        {
            WorldState.instance.nextState = BattleState.CreateState(m_chariot, m_chariotView);
        }
        
        public void ChangeToBattleResultState(BattleResultType resultType)
        {
            WorldState.instance.nextState = BattleResultState.CreateState(resultType);
        }

        public void ChangeToNullState()
        {
            WorldState.instance.nextState = null;
        }
        #endregion
    }
}