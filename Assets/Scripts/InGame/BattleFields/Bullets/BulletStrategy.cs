using UnityEngine;

using Utils;
using Utils.Common;

using InGame.Cores;
using InGame.BattleFields.Enemies;
using InGame.Views;


namespace InGame.BattleFields.Bullets
{
    public enum MoveType
    {
        Linear,
        Follow,
        Parabola,
        CircleRound,
        Placement,
        Random,
    }
    
    public interface IMovable
    {
        void Move();
    }

    public abstract class MoveStrategy
    {
        protected Bullet m_bullet;
        protected Transform m_bulletTransform;
        protected int m_batchIdx;
        protected int m_bulletIdx;
        protected BulletManager m_bulletManager;

        public MoveStrategy(Bullet bullet)
        {
            this.m_bullet = bullet;
            this.m_bulletTransform = bullet.bulletView.transform;
            this.m_batchIdx = bullet.bulletIdx[0];
            this.m_bulletIdx = bullet.bulletIdx[1];
            this.m_bulletManager = GameManager.Instance.GetBulletManager();
            if(m_bulletIdx == 0) this.SetBatchInfo();
        }

        protected virtual void SetBatchInfo(){}
    }

    public class LinearMoveStrategy : MoveStrategy, IMovable
    {
        private Vector3 m_velocity;
    
        public LinearMoveStrategy(Bullet bullet) : base(bullet)
        {
            this.SetDirection();
        }

        protected override void SetBatchInfo()
        {
            Enemy closest = GameManager.Instance.GetEnemySpawnController().
                            GetClosestEnemy(m_bulletTransform.position);
            Vector3 target;
            if(closest != null) target = closest.GetView().transform.position;
            else target = Utilities.RandomPosition();

            if(m_bullet.origin.gameObject.TryGetComponent<EquipmentView>(out var equipmentView)) 
                equipmentView.SetTarget(target);

            Vector3 direction = target - m_bulletTransform.position;
            direction.z = Constants.BULLET_DEPTH;

            m_bulletManager.SetBatchInfo(direction, m_batchIdx);
        }

        private void SetDirection()
        {
            Vector3 direction = m_bulletManager.GetBatchInfo(m_batchIdx);

            int batchSize = m_bulletManager.GetBatchSize(m_batchIdx);
            float medium = (batchSize%2 == 0) ? (batchSize/2 - 0.5f) : batchSize/2;
            float theta = (m_bulletIdx - medium) * Constants.BULLET_BATCH_ROTATION_DEGREE * Mathf.Deg2Rad;

            Vector3 currDirection;
            currDirection.x = Mathf.Cos(theta) * direction.x - Mathf.Sin(theta) * direction.y;
            currDirection.y = Mathf.Sin(theta) * direction.x + Mathf.Cos(theta) * direction.y;
            currDirection.z = direction.z;

            Vector3 upwardDirection = currDirection;
            upwardDirection.z = 0;
            m_bulletTransform.rotation = Quaternion.LookRotation(Vector3.forward, upwardDirection);
            m_velocity = Constants.SPEED_MULTIPLIER * m_bullet.speed.value * 
                        Time.deltaTime * currDirection.normalized;
        }

        public void Move()
        {
            m_bulletTransform.Translate(m_velocity, Space.World);
        }
    }

    public class RandomMoveStrategy : MoveStrategy, IMovable
    {  
        private Vector3 m_velocity;

        public RandomMoveStrategy(Bullet bullet) : base(bullet)
        {
            this.SetDirection();
        }

        protected override void SetBatchInfo()
        {
            Enemy random = GameManager.Instance.GetEnemySpawnController().GetRandomEnemy();
            Vector3 target;
            if(random != null) target = random.GetView().transform.position;
            else target = Utilities.RandomPosition();

            if(m_bullet.origin.gameObject.TryGetComponent<EquipmentView>(out var equipmentView)) 
                equipmentView.SetTarget(target);
            
            target.z = Constants.BULLET_DEPTH;
            m_bulletManager.SetBatchInfo(target, m_batchIdx);
        }

        private void SetDirection()
        {
            Vector3 target = m_bulletManager.GetBatchInfo(m_batchIdx);
            Vector3 direction = (target - m_bulletTransform.position).normalized;
            m_velocity = Constants.SPEED_MULTIPLIER * m_bullet.speed.value * 
                        Time.deltaTime * direction;
        }

        public void Move()
        {
            m_bulletTransform.Translate(m_velocity, Space.World);
        }
    }

    public class FollowMoveStrategy : MoveStrategy, IMovable
    {
        private Transform m_target;

        public FollowMoveStrategy(Bullet bullet) : base(bullet)
        {
            this.SetTarget();
        }

        protected override void SetBatchInfo()
        {

        }

        private void SetTarget()
        {
            Enemy closest = GameManager.Instance.GetEnemySpawnController().
                            GetClosestEnemy(m_bulletTransform.position);
            if(closest == null){
                Debug.LogWarning("No Target to Follow!");
                return;
            }

            m_target = closest.GetView().transform;
            m_target.position = new Vector3(
                m_target.position.x,
                m_target.position.y,
                Constants.BULLET_DEPTH
            );
            
            if(m_bullet.origin.gameObject.TryGetComponent<EquipmentView>(out var equipmentView)) 
                equipmentView.SetTarget(m_target.position);
        }

        public void Move()
        {
            if(m_target == null) this.SetTarget();
            if(m_target == null) return;
            Vector3 direction = m_target.position - m_bulletTransform.position;
            direction.z = Constants.BULLET_DEPTH;
            Vector3 velocity = Constants.SPEED_MULTIPLIER * m_bullet.speed.value * 
                                Time.deltaTime * direction.normalized;
            m_bulletTransform.Translate(velocity, Space.World);
        }
    }

    public class ParabolaMoveStrategy : MoveStrategy, IMovable
    {
        private CountdownTimer m_timer;

        public ParabolaMoveStrategy(Bullet bullet) : base(bullet)
        {
            this.SetTarget();
            this.m_bullet.bulletView.Disable();
        }

        protected override void SetBatchInfo()
        {
            Enemy random = GameManager.Instance.GetEnemySpawnController().GetRandomEnemy();
            Vector3 target;
            if(random != null) target = random.GetView().transform.position;
            else target = Utilities.RandomPosition();  
            
            if(m_bullet.origin.gameObject.TryGetComponent<EquipmentView>(out var equipmentView)) 
                equipmentView.SetTarget(target);
            
            target.z = Constants.BULLET_DEPTH;
            m_bulletManager.SetBatchInfo(target, m_batchIdx);
        }

        private void SetTarget()
        {
            Vector3 target = m_bulletManager.GetBatchInfo(m_batchIdx);
            float distance = Vector3.Distance(this.m_bulletTransform.position, target);
            
            m_timer = new CountdownTimer(
                distance / m_bullet.speed.value / Constants.SPEED_MULTIPLIER
            );

            m_timer.OnTimerComplete.AddListener(() => {
                m_bullet.bulletView.Enable();
                m_bulletTransform.position = target;
            });

            m_timer.StartTimer();
        }

        public void Move()
        {
            m_timer.Update(Time.deltaTime);   
        }
    }

    public class CircleRoundMoveStrategy : MoveStrategy, IMovable
    {
        private Vector3 m_velocity;

        public CircleRoundMoveStrategy(Bullet bullet) : base(bullet)
        {
            this.SetDirection();    
        }

        protected override void SetBatchInfo()
        {
            Enemy closest = GameManager.Instance.GetEnemySpawnController().
                            GetClosestEnemy(m_bulletTransform.position);
            Vector3 target;
            if(closest != null) target = closest.GetView().transform.position;
            else target = Utilities.RandomPosition();
            
            if(m_bullet.origin.gameObject.TryGetComponent<EquipmentView>(out var equipmentView)) 
                equipmentView.SetTarget(target);
            
            Vector3 direction = target - m_bulletTransform.position;
            direction.z = Constants.BULLET_DEPTH;
            m_bulletManager.SetBatchInfo(direction, m_batchIdx);
        }
        
        private void SetDirection()
        {
            Vector3 direction = m_bulletManager.GetBatchInfo(m_batchIdx);
            int batchSize = m_bulletManager.GetBatchSize(m_batchIdx);

            float angleBetween = (batchSize > 0) ? 360f/batchSize : 0;
            angleBetween = m_bulletIdx * angleBetween * Mathf.Deg2Rad;
            
            Vector3 currDirection = new(
                Mathf.Cos(angleBetween) * direction.x - Mathf.Sin(angleBetween) * direction.y,
                Mathf.Sin(angleBetween) * direction.x + Mathf.Cos(angleBetween) * direction.y,
                direction.z
            );

            Vector3 upwardDirection = currDirection;
            upwardDirection.z = 0;
            m_bulletTransform.rotation = Quaternion.LookRotation(Vector3.forward, upwardDirection);
            m_velocity = Constants.SPEED_MULTIPLIER * m_bullet.speed.value * 
                        Time.deltaTime * currDirection.normalized;
        }

        public void Move()
        {
            m_bulletTransform.Translate(m_velocity, Space.World);
        }
    }

    public class PlacementMoveStrategy : MoveStrategy, IMovable
    {
        int m_maxIterations = 10;
        float m_separationDistance = 0.2f;

        public PlacementMoveStrategy(Bullet bullet) : base(bullet)
        {
            // Collider[] overlappingBullets = Physics.OverlapSphere(m_bulletTransform.position, 
                                                                // m_bullet.size.value, 
                                                                // Constants.BULLET_LAYER);
            // int iteration = 0;
            // while(overlappingBullets.Length > 0 && iteration < m_maxIterations)
            // {
                // foreach(var collider in overlappingBullets)
                // {
                    // if(collider.gameObject == m_bulletTransform.gameObject) continue;
                    // // Vector2 directionToMove = (m_bulletTransform.position - collider.transform.position).normalized;
                    // m_bulletTransform.position += (Vector3)(directionToMove * m_separationDistance);
                // }
                // overlappingBullets = Physics.OverlapSphere(m_bulletTransform.position, 
                                                        //    m_bullet.size.value, 
                                                        //    Constants.BULLET_LAYER);
                // iteration++;
            // }
        }

        public void Move()
        {

        }
    }
}