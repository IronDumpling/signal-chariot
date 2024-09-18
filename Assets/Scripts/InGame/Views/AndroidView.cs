using System.Collections.Generic;
using UnityEngine;

using Utils;
using Utils.Common;

using InGame.BattleFields.Androids;
using InGame.BattleFields.Common;
using Spine.Unity;
using UnityEngine.PlayerLoop;
using AnimationState = Spine.AnimationState;

namespace InGame.Views
{
    public class AndroidView : MonoBehaviour, IDamageable
    {
        private Vector3 m_moveDirection = Vector3.zero;
        private Vector3 m_obstacleDirection = Vector3.zero;
        private Android m_android;
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
        private AnimationState animationState => skeletonAnimation.AnimationState;
        private const string MoveAnimation = "Walk", IdleAnimation = "Stay", TakeDamageAnimation = "BeAttacked";
        private float GetAnimationDuration(string animationName) => 
            skeletonAnimation.Skeleton.Data.FindAnimation(animationName).Duration;
        
        private List<string> m_currentAnimations = new List<string>(2);

        #region Animation

        private void PlayAnimation(int trackIdx, string animationName, bool loop)
        {   
            while(m_currentAnimations.Count - 1 < trackIdx) m_currentAnimations.Add("");
            if (m_currentAnimations[trackIdx] == animationName) return;
            
            if (loop)
                m_currentAnimations[trackIdx] = animationName;
            animationState.SetAnimation(trackIdx, animationName, loop);
        }

        private void UpdateMoveAnimation()
        {
            var scale = transform.localScale;
            scale.x = m_moveDirection.x switch
            {
                < 0 => -Mathf.Abs(scale.x),
                > 0 => Mathf.Abs(scale.x),
                _ => scale.x
            };

            transform.localScale = scale;

            if (m_moveDirection == Vector3.zero) PlayAnimation(0, IdleAnimation, true);
            else PlayAnimation(0, MoveAnimation, true);
        }

        #endregion
        
        #region Life Cycle
        public void Init(Android android)
        {
            m_android = android;
            m_hits = new RaycastHit[4];
            
            if(!TryGetComponent<BoxCollider>(out var boxCollider))
            {
                Debug.LogError("Android has no collider!");
                return;
            }
            m_colliderSizes = new float[2];
            m_colliderSizes[0] = boxCollider.size.y/2;
            m_colliderSizes[1] = boxCollider.size.x/2;
        }

        private void Update()
        {
            GenerateRay();
            Move();
            UpdateMoveAnimation();
        }

        public void Die()
        {
            m_android = null;
            if(gameObject != null)
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
            
            if (m_hits[0].collider != null && m_moveDirection.y > 0)
                OnRayHit(m_hits[0].collider, false);
            if (m_hits[1].collider != null && m_moveDirection.y < 0)
                OnRayHit(m_hits[1].collider, false);
            if (m_hits[2].collider != null && m_moveDirection.x < 0)
                OnRayHit(m_hits[2].collider, true);
            if (m_hits[3].collider != null && m_moveDirection.x > 0)
                OnRayHit(m_hits[3].collider, true);
        }

        public void SetMoveDirection(Vector2 inputDirection)
        {
            float x = inputDirection.x;
            float y = inputDirection.y;
            m_moveDirection = new Vector3(x * Mathf.Sqrt(1 - y * y * 0.5f), 
                                        y * Mathf.Sqrt(1 - x * x * 0.5f), 0);
        }

        private void Move()
        {
            m_moveDirection = (m_moveDirection + m_obstacleDirection).normalized;
            Vector3 velocity = Constants.SPEED_MULTIPLIER * 
                                m_android.Get(UnlimitedPropertyType.Speed) * 
                                Time.deltaTime * m_moveDirection;

            if (velocity == Vector3.zero) return;
            this.transform.Translate(velocity, Space.World);
        }                                                                     
        #endregion

        #region Interaction
        public void TakeDamage(float dmg)
        {
            if (m_android == null) return;
            PlayAnimation(1, TakeDamageAnimation, false);
            m_android.TakeDamage(dmg);
        }

        private void OnRayHit(Collider other, bool isX)
        {
            int layer = other.gameObject.layer;
            switch (layer)
            {
                case Constants.OBSTACLE_LAYER:
                    if(isX) m_moveDirection.x = 0;
                    else m_moveDirection.y = 0;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            int layer = other.gameObject.layer;
            switch (layer)
            {
                case Constants.MOD_LAYER:
                    PickUp(other.gameObject);
                    break;
            }
        }

        private void PickUp(GameObject item)
        {   
            IPickable target = item.GetComponent<IPickable>();
            target?.PickUp();
        }
        #endregion

        public Vector2 GetPosition()
        {
            return new Vector2(transform.position.x, transform.position.y);
        }

        public void SetPosition(Vector2 newPos)
        {
            Vector3 pos = newPos;
            pos.z = transform.position.z;
            
            transform.position = pos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * m_colliderSizes[0]);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * m_colliderSizes[0]);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * m_colliderSizes[1]);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * m_colliderSizes[1]);
        }
    }
}