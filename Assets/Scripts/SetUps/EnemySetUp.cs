using InGame.Views;

namespace SetUps
{
    [System.Serializable]
    public class EnemySetUp
    {
        public string name;
        
        public float maxHealth;

        public float defense;

        public float attack;

        public float attackDuration;

        public float attackRadius;

        public float speed;

        public EnemyView enemyPrefab;
    }
}