using System;
using UnityEngine;

using Utils;
using Utils.Common;
using InGame.UI;
using InGame.Cores;
using InGame.BattleFields.Enemies;
using InGame.BattleFields.Androids;
using InGame.BattleFields.Common;
using System.Collections;
using Spine.Unity;
using AnimationState = Spine.AnimationState;

namespace InGame.Views
{
    public class EnemyView : MonoBehaviour, IDamageable
    {
        private Enemy m_enemy;
        private Vector3 m_target = Vector3.zero;
        private Vector3 m_direction = Vector3.zero;
        private Vector3 m_obstacleDirection = Vector3.zero;
        private bool m_isOn = false;
        private IDamageable m_dmgTarget = null;
        private RaycastHit[] m_hits;
        private float[] m_colliderSizes;

        private SkeletonAnimation m_skeletonAnimation;

        private SkeletonAnimation skeletonAnimation
        {
            get
            {
                if (m_skeletonAnimation == null) m_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
                return m_skeletonAnimation;
            }
        }
        private AnimationState animationState
        {
            get
            {
                return skeletonAnimation.AnimationState;
            }
        }
        private const string MoveAnimation = "Move", AttackAnimation = "Attack";

        private float attackAnimationDuration => skeletonAnimation.Skeleton.Data.FindAnimation(AttackAnimation).Duration;

        #region Life Cycle
        

        public void Init(Enemy enemy)
        {
            m_enemy = enemy;
            m_hits = new RaycastHit[4];
            if(!TryGetComponent<BoxCollider>(out var boxCollider))
            {
                Debug.LogError("Android has no collider!");
                return;
            }
            m_colliderSizes = new float[2];
            m_colliderSizes[0] = boxCollider.size.y/2;
            m_colliderSizes[1] = boxCollider.size.x/2;
            new SlideBarUI(gameObject, m_enemy, LimitedPropertyType.Health);
        }

        private void Update()
        {
            if(!m_isOn) return;
            GenerateRay();
            Move();
        }

        public void Die()
        {
            m_enemy = null;
            Destroy(gameObject);
        }
        #endregion

        #region Action
        private void GenerateRay()
        {
            Physics.Raycast(transform.position, Vector2.up, out m_hits[0], m_colliderSizes[0]);
            Physics.Raycast(transform.position, Vector2.down, out m_hits[1], m_colliderSizes[0]);
            Physics.Raycast(transform.position, Vector2.left, out m_hits[2], m_colliderSizes[1]);
            Physics.Raycast(transform.position, Vector2.right, out m_hits[3], m_colliderSizes[1]);

            if (m_hits[0].collider != null && m_direction.y > 0)
                OnRayHit(m_hits[0].collider, false);
            if (m_hits[1].collider != null && m_direction.y < 0)
                OnRayHit(m_hits[1].collider, false);
            if (m_hits[2].collider != null && m_direction.x < 0)
                OnRayHit(m_hits[2].collider, true);
            if (m_hits[3].collider != null && m_direction.x > 0)
                OnRayHit(m_hits[3].collider, true);
        }

        private void Move()
        {
            Android android = GameManager.Instance.GetAndroid();
            if(android == null || android.androidView == null) return;
            
            m_target = android.androidView.transform.position;
            m_target.z = Constants.ENEMY_DEPTH;

            float distance = Vector3.Distance(this.transform.position, m_target);
            if(distance <= Constants.COLLIDE_OFFSET) return;

            m_direction = (m_target - this.transform.position + Seperation()).normalized;
            Vector3 velocity = Constants.SPEED_MULTIPLIER * m_enemy.Get(UnlimitedPropertyType.Speed) * 
                            Time.deltaTime * m_direction;
            
            this.transform.Translate(velocity, Space.World);
        }

        private Vector3 Seperation()
        {
            Vector3 seperation = Vector3.zero;
            foreach(Enemy otherEnemy in GameManager.Instance.GetEnemySpawnController().GetAllEnemies())
            {
                if(otherEnemy == null) continue;
                GameObject otherEnemyGO = otherEnemy.GetView().gameObject;
                if(otherEnemyGO == this.gameObject) continue;

                Vector3 directionToOther = otherEnemyGO.transform.position - transform.position;
                float distanceToOther = directionToOther.magnitude;
                if(distanceToOther < Constants.SEPERATION_DISTANCE)
                    seperation -= directionToOther / distanceToOther * 
                                (Constants.SEPERATION_DISTANCE - distanceToOther) * 
                                Constants.SEPERATION_FORCE;
            }
            return seperation;
        }

        #endregion
        
        #region Interaction
        private void OnRayHit(Collider other, bool isX)
        {
            int layer = other.gameObject.layer;
            switch(layer)
            {
                case Constants.OBSTACLE_LAYER:
                    if(isX) m_direction.x = 0;
                    else m_direction.y = 0;
                    break;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            int layer = other.gameObject.layer;
            switch(layer)
            {
                case Constants.ENEMY_LAYER:
                    break;
                default:
                    bool isRight = false;
                    if (other.gameObject.transform.position.x >= transform.position.x) isRight = true;
                    else isRight = false;
                    
                    m_dmgTarget = other.gameObject.GetComponent<IDamageable>();
                    if(m_dmgTarget != null) StartCoroutine(Attack(isRight));
                    break;
            }
        }

        public void TakeDamage(float dmg)
        {
            m_enemy.TakeDamage(dmg);
        }

        public IEnumerator Attack(bool isRight)
        {
            var target = m_dmgTarget;
            // TODO: enlarge collider size based on attack range
            var originalScale = transform.localScale;
            var modifiedScale = originalScale;
            
            if (!isRight) modifiedScale.x = -modifiedScale.x;
            

            transform.localScale = modifiedScale;
            animationState.SetAnimation(0, AttackAnimation, false);
            yield return new WaitForSeconds(attackAnimationDuration);
            
            // Die before attacking
            if (m_enemy == null) yield break;
            
            target.TakeDamage(m_enemy.Get(UnlimitedPropertyType.Damage));
            animationState.SetAnimation(0, MoveAnimation, true);
            
            transform.localScale = originalScale;
            yield return new WaitForSeconds(m_enemy.Get(UnlimitedPropertyType.Interval));
        }
        #endregion

        public void SetPosition(Vector2 pos)
        {
            Vector3 worldPos = pos;
            worldPos.z = Constants.ENEMY_DEPTH;
            transform.position = worldPos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * m_colliderSizes[0]);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * m_colliderSizes[0]);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * m_colliderSizes[1]);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * m_colliderSizes[1]);
        }
        
        public void TurnOn()
        { 
            m_isOn = true;
            animationState.SetAnimation(0, MoveAnimation, true);
        } 

        public void TurnOff()
        { 
            m_isOn = false;
            animationState.SetEmptyAnimation(0, 0f);
        }
    }
}