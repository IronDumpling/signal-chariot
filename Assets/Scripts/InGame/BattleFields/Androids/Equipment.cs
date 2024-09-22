using System.Collections;

using UnityEngine;

using SetUps;
using InGame.Cores;
using InGame.Views;
using InGame.Boards.Modules;
using InGame.BattleFields.Bullets;
using InGame.BattleFields.Common;

using Utils;
using InGame.Boards.Modules.ModuleBuffs;
using Audios;

namespace InGame.BattleFields.Androids
{
    public class Equipment 
    {
        [Header("View")]
        private EquipmentView m_equipmentView;
        public EquipmentView equipmentView { get {return m_equipmentView;} }
        private Sprite m_sprite;
        public Sprite sprite { get { return m_sprite; } }

        [Header("Properties")]
        // private UnlimitedProperty m_damageMultiplier;
        // public UnlimitedProperty damageMultiplier { get { return m_damageMultiplier;}}
        
        [Header("Bullet")]
        private UnlimitedProperty m_bulletCount;
        private UnlimitedProperty m_shootCount;
        private UnlimitedProperty m_shootInterval;
        public UnlimitedProperty shootInterval { get { return m_shootInterval;}}
        private UnlimitedProperty m_seekInterval;
        public UnlimitedProperty seekInterval { get { return m_seekInterval;}}
        
        [Header("Module")]
        private Module m_module;
        public Module module { get { return m_module;}}

        #region Life Cycle
        public Equipment(EquipmentSetUp equipmentSetUp, Module module)
        {
            UnlimitedProperty bltCnt = new(equipmentSetUp.bulletCount, UnlimitedPropertyType.BulletCount);
            UnlimitedProperty shtCnt = new(equipmentSetUp.shootCount);
            UnlimitedProperty shtItv = new(equipmentSetUp.shootInterval, UnlimitedPropertyType.Interval);
            UnlimitedProperty skItv = new(equipmentSetUp.seekInterval, UnlimitedPropertyType.Speed);
            // UnlimitedProperty dmgMtp = new(equipmentSetUp.damageMultipler, UnlimitedPropertyType.Multiplier);

            m_bulletCount = bltCnt;
            m_shootCount = shtCnt;
            m_shootInterval = shtItv;
            m_seekInterval = skItv;
            // m_damageMultiplier = dmgMtp;

            m_module = module;              
            m_sprite = equipmentSetUp.sprite;

            CreateView();
        }

        public void Die()
        {
            m_equipmentView.Die();
        }
        
        private void CreateView()
        {
            GameObject equipmentPref = Resources.Load<GameObject>(Constants.GO_TOWER_PATH);
            GameObject equipmentGO = GameObject.Instantiate(equipmentPref);
            equipmentGO.transform.parent = GameManager.Instance.GetAndroid().androidView.transform;
            
            float x = equipmentGO.transform.parent.position.x + Constants.EQUIPMENT_RADIUS;
            float y = equipmentGO.transform.parent.position.y;
            equipmentGO.transform.position = new(x, y, Constants.EQUIPMENT_DEPTH); 

            m_equipmentView = equipmentGO.GetComponent<EquipmentView>();
            m_equipmentView.Init(this);
        }
        #endregion

        #region Effection

        public void Effect(WeaponBuff buff, BulletType type, int level)
        {
            m_equipmentView.Shoot(buff, type, level);
        }

        public IEnumerator ShootBullet(WeaponBuff buff, BulletType type, int level)
        {
            BulletSetUp bulletSetUp = GameManager.Instance.GetBulletManager().GenerateBulletSetUp(type, level, buff);
            if(bulletSetUp == null) yield break;
            
            float shootCount = m_shootCount.value + buff.numShotsFlatBuff;
            float bulletCount = m_bulletCount.value + buff.numBulletsPerShotFlatBuff;

            for(int i = 0; i < shootCount; i++)
            {
                AudioManager.Instance.PlaySound(Constants.AUDIO_WEAPON_FIRE);
                GameManager.Instance.GetBulletManager().AddBulletBatch(bulletCount, bulletSetUp, 
                                                                    this.equipmentView.transform);
                yield return new WaitForSeconds(m_shootInterval.value);
            }
        }
        #endregion
    }
}