using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using InGame.Boards;
using InGame.Boards.Modules;
using InGame.Boards.Signals;
using InGame.BattleFields.Androids;
using InGame.BattleFields.Bullets;
using InGame.BattleFields.Common;
using InGame.BattleFields.Enemies;
using InGame.InGameStates;
using InGame.Views;

using SetUps;
using Utils.Common;
using Utils;
using MainMenu;

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

        [Header("Android")]
        private Android m_android;
        private AndroidView m_androidView;
        private ModManager m_modManager;
        private BulletLib m_bulletLib;
        private BulletManager m_bulletManager;

        [Header("Board")]
        private GeneralBoard m_generalBoard;
        private ModuleLib m_moduleLib;
        private GeneralSignalController m_generalSignalController;
        private ModuleInfoDisplayManager m_moduleInfoDisplayManager;

        [Header("Enemy")]
        private EnemySpawnLib m_enemySpawnLib;
        private EnemyLib m_enemyLib;
        private EnemySpawnController m_enemySpawnController;

        #region Life Cycle
        protected override void Init()
        {
            m_inputManager = new InputManager(m_playerInput);
            m_timeEffectManager = TimeEffectManager.CreateTimeEffectManager();

            InitAndroid();
            InitBoard();
            InitCamera();
            InitEnemy();
            InitMod();
        }

        private void Start()
        {
            ChangeToBoardWaitingState(); // initial state is board preparation
        }

        private void InitMod()
        {
            m_modManager = new ModManager();
        }
        
        private void InitEnemy()
        {
            m_enemyLib = new EnemyLib(m_setUp.enemyLibrary);
            m_enemySpawnLib = new EnemySpawnLib(m_setUp.enemySpawnSetUp);
            m_enemySpawnController = new EnemySpawnController(m_enemySpawnLib, m_enemyLib);
            m_enemySpawnController.Init(0);
        }

        private void InitAndroid()
        {
            m_android = new Android(m_setUp.androidSetUp);
            m_androidView = m_android.androidView;

            m_bulletLib = new();
            m_bulletLib.Init(m_setUp.bulletLibrary);

            m_bulletManager = new();
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

            m_generalSignalController = new GeneralSignalController(board, boardView);
            m_generalBoard = GeneralBoard.CreateGeneralBoard(board, extraBoard, boardView);

            m_moduleInfoDisplayManager = new ModuleInfoDisplayManager();
        }

        private void InitCamera()
        {
            var cameraPrefab = Resources.Load<CameraManager>(Constants.GO_CAMERA_PATH);
            m_cameraManager = Instantiate(cameraPrefab);
            m_cameraManager.transform.position = Vector3.zero;
            m_cameraManager.Init();
            
            m_cameraManager.SetBattleCameraFollow(m_androidView?.gameObject);
            m_cameraManager.SetBattleConfiner("Boundary");

            m_cameraManager.SetBoardThumbnailPosition(GetBoardView().transform.position);
            m_cameraManager.SetBoardPosition(GetBoardView().transform.position);
        }

        public void Update()
        {
            m_timeEffectManager?.Update(UnityEngine.Time.deltaTime, UnityEngine.Time.time);
            m_generalSignalController?.Update(UnityEngine.Time.deltaTime, UnityEngine.Time.time);
            m_enemySpawnController?.Update(UnityEngine.Time.deltaTime);
        }

        public void Clear()
        {
            // m_enemySpawnController.Clear();
            m_inputManager.Clear();
        }
        #endregion

        #region Getters
        public CameraManager GetCameraManager() => m_cameraManager;
        public InputManager GetInputManager() => m_inputManager;
        public ModuleLib GetModuleLib() =>  m_moduleLib;
        public BulletLib GetBulletLib() => m_bulletLib;
        public GeneralSignalController GetSignalController() => m_generalSignalController;
        public TimeEffectManager GetTimeEffectManager() => m_timeEffectManager;
        public Board GetBoard() => m_generalBoard.board;
        public Board GetExtraBoard() => m_generalBoard.extraBoard;
        public GeneralBoard GetGeneralBoard() => m_generalBoard;
        public BoardView GetBoardView() => m_generalBoard.boardView;
        public Android GetAndroid() => m_android;
        public EnemySpawnController GetEnemySpawnController() => m_enemySpawnController;
        public InGameStateType GetCurrentInGameState() => WorldState.instance.currentState.type;
        public ModManager GetModManager() => m_modManager;
        public BulletManager GetBulletManager() => m_bulletManager;
        public EnemyLib GetEnemyLib() => m_enemyLib;

        public ModuleInfoDisplayManager GetModuleInfoDisplayManager() =>
            m_moduleInfoDisplayManager;
        #endregion

        #region World State Machine
        public void ChangeToInitState()
        {
            Game.Instance.nextState = new InitState();
        }

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
            WorldState.instance.nextState = BoardTestState.CreateState(m_timeEffectManager, m_generalSignalController);
        }

        public void ChangeToBattleState()
        {
            WorldState.instance.nextState = BattleState.CreateState(m_android, m_androidView, m_enemySpawnController);
        }

        public void ChangeToBattleResultState(BattleResultType resultType)
        {
            WorldState.instance.nextState = BattleResultState.CreateState(resultType);
        }

        public void ChangeToRewardState(bool isLastWave, List<int> rewards)
        {
            WorldState.instance.nextState =
                RewardState.CreateState(isLastWave, rewards, GetGeneralBoard(), GetModuleLib());
        }
            
        public void ChangeToNullState()
        {
            WorldState.instance.nextState = null;
        }

        public bool CheckGameState(InGameStateType[] states)
        {
            bool isInState = false;
            foreach(InGameStateType state in states)
                isInState = isInState || (GetCurrentInGameState() == state);
            
            return isInState;
        }
        #endregion
    }
}
