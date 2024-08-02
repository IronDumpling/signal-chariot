using System.Threading.Tasks;

using UnityEngine;

using SetUps;
using InGame.Cores;
using InGame.Views;
using InGame.Boards.Modules;
using InGame.BattleFields.Common;
using Utils;
using System.Collections;

namespace InGame.BattleFields.Chariots
{
    public enum SeekMode
    {
        None,
        Nearest,
        Random,
    }

    public class Tower
    {
        [Header("View")]
        private TowerView m_towerView;
        private Sprite m_sprite;
        public Sprite sprite { get { return m_sprite; } }

        [Header("Properties")]
        private UnlimitedProperty m_damageMultiplier;
        
        [Header("Bullet")]
        private BulletManager m_bulletManager;
        private BulletSetUp m_bulletSetUp;
        private UnlimitedProperty m_bulletCount;
        private UnlimitedProperty m_shootInterval;
        public UnlimitedProperty shootInterval { get { return m_shootInterval;}}
        private SeekMode m_seekMode;
        private UnlimitedProperty m_seekInterval;
        public UnlimitedProperty seekInterval { get { return m_seekInterval;}}
        
        [Header("Module")]
        private Module m_module;
        public Module module { get { return m_module;}}

        #region Life Cycle
        public Tower(TowerSetUp towerSetUp, Module module)
        {
            UnlimitedProperty bulletCount = new(towerSetUp.bulletCount, UnlimitedPropertyType.BulletCount);
            UnlimitedProperty attakMultiplier  = new(towerSetUp.damageMultipler, UnlimitedPropertyType.Multiplier);
            UnlimitedProperty shootInterval = new(towerSetUp.shootInterval, UnlimitedPropertyType.Interval);
            UnlimitedProperty seekInterval = new(towerSetUp.seekInterval, UnlimitedPropertyType.Speed);
            
            m_damageMultiplier = attakMultiplier;
            m_bulletSetUp = towerSetUp.bulletSetUp;
            m_bulletCount = bulletCount;
            m_shootInterval = shootInterval;
            m_seekMode = towerSetUp.seekMode;
            m_seekInterval = seekInterval;
            m_module = module;              
            m_sprite = towerSetUp.sprite;

            m_bulletManager = new();
            
            CreateView();
        }

        public void Die()
        {
            m_towerView.Die();
        }
        
        private void CreateView()
        {
            GameObject towerPref = Resources.Load<GameObject>(Constants.GO_TOWER_PATH);
            GameObject towerGO = GameObject.Instantiate(towerPref);
            towerGO.transform.parent = GameManager.Instance.GetChariot().chariotView.transform;
            float x = towerGO.transform.parent.position.x;
            float y = towerGO.transform.parent.position.y;
            towerGO.transform.position = new(x, y, Constants.TOWER_DEPTH); 

            m_towerView = towerGO.GetComponent<TowerView>();
            m_towerView.Init(this);
        }
        #endregion

        public BulletManager GetBulletManager() => m_bulletManager;

        public void Effect()
        {
            m_towerView.Shoot();
        }

        public IEnumerator ShootBullet()
        {
            Vector3 target = FindTarget();
            m_towerView.SetTarget(target);
            
            for(int i = 0; i < m_bulletCount.value; i++)
            {
                m_bulletManager.AddBullet(m_bulletSetUp, target, m_damageMultiplier.value);
                yield return new WaitForSeconds(m_shootInterval.value);
            }
        }

        private Vector3 FindTarget()
        {
            Vector3 target = Vector3.zero;
            switch(m_seekMode)
            {
                case SeekMode.Nearest:
                    target = new(10, 10, 0);
                    break;
                case SeekMode.Random:
                    break;
                default:
                    break;
            }
            return target;
        }
    }


}