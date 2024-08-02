using System.Collections.Generic;

namespace InGame.BattleFields.Enemies
{
    using Enemies = KeyValuePair<Enemy, int>;
    using EnemyGroups = KeyValuePair<EnemyGroup, int>;

    public enum SpawnLogic
    {
        Random
    }

    public class EnemyGroup
    {
        private List<Enemies> m_enemyGroup;

        public EnemyGroup()
        {
            m_enemyGroup = new();
        }
    }

    public class EnemyWave
    {
        private List<EnemyGroups> m_enemyWave;

        public EnemyWave()
        {
            m_enemyWave = new();
        }
    }

    public class EnemyBattle
    {
        private List<EnemyWave> m_enemyBattle;

        // timeline: 0 -> end
        // time line parts: wave 0 ~ wave n
        // inside wave m (30s ~ 60s)
        // group 0 ~ group n
        // group 0 (0 ~ 10s): Enemies 1 (Which enemy + how many enemy), Enemies 2
        // group 1 (5s ~ 15s): 


        public EnemyBattle()
        {
            m_enemyBattle = new();
        }
    }
}