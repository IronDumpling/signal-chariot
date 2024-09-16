using System.Collections.Generic;

using UnityEngine;

using SetUps;
using Utils;

namespace InGame.BattleFields.Bullets
{
    public enum BulletType
    {
        Basic,
        Laser,
        Landmine,
        HighExplosive,
    }

    public class BulletLib
    {
        private Dictionary<BulletType, BulletSetUp[]> m_lib;

        public void Init(List<BulletSetUp> setUps)
        {
            m_lib = new();
            foreach (var setUp in setUps)
            {
                BulletType type = setUp.type;
                int level = setUp.level;
                if(level < 1 || level > Constants.MAX_BULLET_LEVEL) continue;
                if(m_lib.ContainsKey(type)) m_lib[type][level-1] = setUp;
                else
                {
                    m_lib.Add(type, new BulletSetUp[Constants.MAX_BULLET_LEVEL]);
                    m_lib[type][level-1] = setUp;
                }
            }
        }

        public BulletSetUp GetSetUp(BulletType type, int level)
        {
            return m_lib[type][level-1];
        }
    }
}