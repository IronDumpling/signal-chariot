namespace Utils
{
    public static class Constants 
    {
        #region Scene Name

        public const string MAIN_SCENE = "Main";
        public const string MAIN_MENU = "MainMenu";
        public const string LEVEL0 = "Level0";
        public const string LEVEL1 = "Level1";

        #endregion

        #region Game Object

        public const string MODEL = "Model";

        #endregion

        #region Resources Path

        public const string GO_BULLET_PATH = "Prefabs/BattleField/BulletView";
        public const string GO_CHARIOT_PATH = "Prefabs/BattleField/ChariotView";
        public const string GO_TOWER_PATH = "Prefabs/BattleField/TowerView";
        public const string GO_BOARD_PATH = "Prefabs/Board/BoardView";
        public const string GO_CAMERA_PATH = "Prefabs/3C/CameraManager";

        public const string UI_WAVE_WIN_PATH = "UI/Battle/WaveWin";
        public const string UI_BATTLE_WIN_PATH = "UI/Battle/BattleWin";
        public const string UI_FAIL_PATH = "UI/Battle/BattleFail";

        public const string SPRITE_SELECTABLE_SLOT_PATH = "Arts/Boards/SelectableSlot";
        public const string SPRITE_EMPTY_SLOT_PATH = "Arts/Boards/EmptySlot";

        #endregion

        #region Tag and Layer

        public const string CHARIOT_TAG = "Player";

        #endregion

        #region Positions
        // Positive is deeper, Negative is shallower
        // Board 
        public const float SLOT_DEPTH = 0;
        public const float MODULE_DEPTH = -0.5f;
        public const float PLACING_MODULE_DEPTH = -1f;
        public const float SIGNAL_DEPTH = -1.5f;
        public const float SIGNAL_MOVING_DURATION = 0.25f;

        // Battlefield
        public const float SCENE_DEPTH = 1f;
        public const float CHARIOT_DEPTH = 0;
        public const float ENEMY_DEPTH = 0;
        public const float BULLET_DEPTH = -0.5f;
        public const float TOWER_DEPTH = -1f;
        
        #endregion
        
        #region Interactions

        public const float COLLIDE_OFFSET = 0.1f;

        #endregion
    }
}
