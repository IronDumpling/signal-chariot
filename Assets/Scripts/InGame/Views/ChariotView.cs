using System;
using System.Collections.Generic;

using UnityEngine;

using Utils.Common;
using InGame.InGameStates;
using InGame.BattleFields.Chariots;
using InGame.Cores;

namespace InGame.Views
{
    public class ChariotView : MonoBehaviour, IDamageable
    {
        private Vector3 m_moveDirection = Vector3.zero;
        private Chariot m_chariot;

        #region Life Cycle
        public void Init(Chariot chariot)
        {
            m_chariot = chariot;
        }

        private void Update()
        {
            MoveChariot();
        }

        public void Die()
        {
            Destroy(gameObject);
            GameManager.Instance.ChangeToBattleResultState(BattleResultType.Fail);
        }
        #endregion

        #region Action
        private void MoveChariot()
        {
            if (m_moveDirection == Vector3.zero) return;
            this.transform.Translate(m_moveDirection, Space.World);
        }

        public void SetMoveDirection(Vector2 inputDirection)
        {
            float x = inputDirection.x;
            float y = inputDirection.y;
            m_moveDirection = new Vector3(
                x * Mathf.Sqrt(1 - y * y * 0.5f), 
                y * Mathf.Sqrt(1 - x * x * 0.5f), 
                0
            ) * Time.deltaTime * m_chariot.speed.value;
        }

        // TODO: add and remove towerview

        #endregion

        #region Interaction
        public void TakeDamage(float dmg)
        {
            m_chariot.health.DecreaseCurrent(dmg);
            if(m_chariot.health.current <= 0) Die();
        }
        #endregion
    }
}