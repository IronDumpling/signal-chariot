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

        public const string GO_UI_COMMON_PATH = "Prefabs/UI/";
        public const string GO_BULLET_PATH = "Prefabs/BattleFields/BulletView";
        public const string GO_ANDROID_PATH = "Prefabs/BattleFields/AndroidView";
        public const string GO_TOWER_PATH = "Prefabs/BattleFields/EquipmentView";
        public const string GO_MOD_PATH = "Prefabs/BattleFields/ModView";

        public const string GO_BOARD_PATH = "Prefabs/Boards/BoardView";
        public const string GO_CAMERA_PATH = "Prefabs/3C/CameraManager";

        public const string UI_WAVE_WIN_PATH = "UI/Battle/WaveWin";
        public const string UI_BATTLE_WIN_PATH = "UI/Battle/BattleWin";
        public const string UI_FAIL_PATH = "UI/Battle/BattleFail";

        public const string SPRITE_SELECTABLE_SLOT_PATH = "Arts/Boards/SelectableSlot";
        public const string SPRITE_EMPTY_SLOT_PATH = "Arts/Boards/EmptySlot";

        public const string VFX_EXPLOSION_PATH = "Prefabs/VFX/Explosions/Explosion_01";
        public const string VFX_DEATH_PATH = "Prefabs/VFX/Death";
        public const string VFX_HIT_PATH = "Prefabs/VFX/Hits/Hit_02";

        #endregion

        #region Audio Name
        public const string AUDIO_BOARD_EXPANSION = "Board Expansion";
        public const string AUDIO_WEAPON_FIRE = "Weapon Fire";
        public const string AUDIO_MODULE_SELECT = "Module Select";
        public const string AUDIO_MODULE_DROP = "Module Drop";
        public const string AUDIO_MODULE_ROTATE = "Module Rotate";
        public const string AUDIO_EXPLOSION = "Explosion";
        public const string AUDIO_ANDROID_DAMAGE = "Android Take Damage";
        public const string AUDIO_ENEMY_DAMAGE = "Enemy Take Damage";
        public const string AUDIO_MOD_PICKUP = "Mod Pick Up";

        #endregion

        #region Tag and Layer

        public const string ANDROID_TAG = "Player";
        public const int ANDROID_LAYER = 3;
        public const int UI_LAYER = 5;
        public const int BOARD_LAYER = 6;
        public const int BATTLEFIELD_LAYER = 7;
        public const int ENEMY_LAYER = 8;
        public const int BULLET_LAYER = 9;
        public const int OBSTACLE_LAYER = 10;
        public const int MOD_LAYER = 11;

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
        public const float MOD_DEPTH = 0.1f;
        public const float ENEMY_DEPTH = 0;
        public const float BULLET_DEPTH = -0.5f;
        public const float ANDROID_DEPTH = -1f;
        public const float EQUIPMENT_DEPTH = -1.1f;

        public const float EQUIPMENT_RADIUS = 1f;
        public const int MAX_BULLET_LEVEL = 3;
        
        #endregion
        
        #region Interactions

        public const float COLLIDE_OFFSET = 0.5f;
        public const float SEPERATION_DISTANCE = 1f;
        public const float SEPERATION_FORCE = 1f;
        public const float SELECT_THRESHOLD = 0.05f;
        public const int ADD_SLOT_COST_BASE = 20;
        public const float ADD_SLOT_COST_INCRE = 10;
        public const int MAX_ENEMY_POS_FIND_TIMES = 100;

        #endregion

        #region Multiplier

        public const float SPEED_MULTIPLIER = 0.1f;
        public const float BULLET_SIZE_MULTIPLIER = .1f;
        public const float BULLET_BATCH_ROTATION_DEGREE = 10f;

        #endregion
    }
}
