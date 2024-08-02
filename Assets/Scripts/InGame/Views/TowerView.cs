using System.Collections;

using UnityEngine;

using Utils;
using InGame.BattleFields.Chariots;

using DG.Tweening;

namespace InGame.Views
{
    public class TowerView : MonoBehaviour
    {
        private Tower m_tower;
        private Vector3 m_target;

        #region Life Cycle
        public void Init(Tower tower)
        {
            m_tower = tower;
            SetSprite();
        }

        public void SetSprite()
        {
            Transform model = transform.Find(Constants.MODEL);
            if(model == null)
            {
                Debug.LogError("No model under this game object!");
                return;
            }

            SpriteRenderer spriteRenderer = model.GetComponent<SpriteRenderer>();
            if(spriteRenderer == null)
            {
                Debug.LogError("No sprite renderer under model!");
                return;
            }

            spriteRenderer.sprite = m_tower.sprite;
        }

        private void Update()
        {
            Aim();
        }

        public void Die()
        {
            Destroy(gameObject);
        }
        #endregion

        public void SetTarget(Vector3 value)
        {
            m_target = value;
        }

        #region Action

        public void Aim()
        {
            StartCoroutine(RotateTowards());
        }

        public void Shoot()
        {
            StartCoroutine(m_tower.ShootBullet());
        }

        public IEnumerator RotateTowards()
        {
            Vector3 direction = m_target - transform.position;
            direction.z = 0;
            
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Tween rotateTween = transform.DORotate(new Vector3(0, 0, targetAngle), m_tower.seekInterval.value);

            yield return rotateTween.WaitForCompletion();
        }

        #endregion
    }
}