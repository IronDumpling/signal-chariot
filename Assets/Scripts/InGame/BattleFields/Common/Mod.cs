using UnityEngine;

using Utils;
using InGame.Views;
using InGame.Cores;

namespace InGame.BattleFields.Common
{
    public class Mod
    {
        private int m_quality;
        private Vector3 m_position;
        private ModView m_view;

        public ModView view => m_view;

        public Mod(int quality, Vector2 position)
        {
            m_quality = quality;
            m_position = position;
            CreateView();
        }

        private void CreateView()
        {
            GameObject androidPref = Resources.Load<GameObject>(Constants.GO_MOD_PATH);
            GameObject androidGO = GameObject.Instantiate(androidPref);
            androidGO.transform.position = new(m_position.x, m_position.y, Constants.MOD_DEPTH);
            m_view = androidGO.GetComponent<ModView>();
            m_view.Init(this);
        }

        private void Die()
        {
            m_view.Die();
        }

        public void Pickup()
        {
            GameManager.Instance.GetAndroid().Increase(UnlimitedPropertyType.Mod, m_quality);
            this.Die();
        }
    }
}
