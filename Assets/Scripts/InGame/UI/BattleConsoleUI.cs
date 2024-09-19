using UnityEngine;
using UnityEngine.UIElements;

using InGame.Cores;
using Utils.Common;
using InGame.BattleFields.Enemies;

namespace InGame.UI
{
    public class BattleConsoleUI : MonoBehaviour, IHidable
    {
        [SerializeField] private UIDocument m_doc;
        private VisualElement m_root;
        private AndroidStatusUI m_status;
        private VisualElement m_progress;
        private ProgressBar[] m_timeline;
        private EnemySpawnController m_enemyController;

        private void Awake()
        {
            m_root = m_doc.rootVisualElement;   
        }

        private void Start()
        {
            m_status = new AndroidStatusUI(m_root.Q("status")); // stay in start
            DisplayProgressUI();
        }

        private void DisplayProgressUI()
        {
            m_enemyController = GameManager.Instance.GetEnemySpawnController();
        
            float[] timeLimits = m_enemyController.GetAllWaveDurations(); 
            
            timeLimits = new float[]{
                30, 30, 30, 45, 60, 45, 45, 45, 45, 60 // TODO: remove this
            };
            
            float totalLimits = 0;
            for (int i = 0; i < timeLimits.Length; i++)
                totalLimits += timeLimits[i];
            
            m_progress = m_root.Q("timeline");
            
            m_timeline = new ProgressBar[timeLimits.Length];
            for(int i = 0; i < timeLimits.Length; i++)
            {
                m_timeline[i] = new ProgressBar();
                m_timeline[i].style.width = new Length(timeLimits[i]/totalLimits*100, LengthUnit.Percent);
                m_timeline[i].style.marginRight = 0;
                m_timeline[i].style.marginLeft = 0;
                m_timeline[i].highValue = timeLimits[i];
                m_timeline[i].title = $"0/{m_timeline[i].highValue}";
                m_progress.Add(m_timeline[i]);
            }
        }

        private void Update()
        {
            SetProgress();
        }

        private void SetProgress()
        {
            int currWave = m_enemyController.GetCurrentWaveIdx();
            if(currWave < 0) return;
            
            m_timeline[currWave].highValue = m_enemyController.GetCurrentWaveTotalDuration(); // TODO: remove this
            m_timeline[currWave].value = m_enemyController.GetCurrentWaveTimer();
            m_timeline[currWave].title = $"{m_timeline[currWave].value}/{m_timeline[currWave].highValue}";
        }

        public void Hide()
        {
            m_doc.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            m_doc.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}