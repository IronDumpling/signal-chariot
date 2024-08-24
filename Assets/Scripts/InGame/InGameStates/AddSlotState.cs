﻿using System.Collections.Generic;
using InGame.Boards;
using InGame.Cores;
using InGame.Views;
using InGame.UI;
using UnityEngine;
using InGame.BattleFields.Common;
using Utils;

namespace InGame.InGameStates
{
    public class AddSlotState: InGameState
    {
        public override InGameStateType type => InGameStateType.AddSlotState;
        private BoardView m_boardView;
        private Board m_board;
        private int m_amount;
        private int m_currentSelectableAmount;
        private List<BoardPosition> m_selectableSlots;
            
        public override void Enter(InGameState last)
        {
            Debug.Log("Enter AddSlot");
            var cameraManager = GameManager.Instance.GetCameraManager();
            cameraManager.BoardCameraSetActive(true);
            cameraManager.MiniBoardCameraSetActive(false);
            cameraManager.BattleCameraSetActive(false);
            
            // change all the adjacent slot to selectable
            m_selectableSlots = m_board.GetAdjacentSlots();

            foreach (var pos in m_selectableSlots)
            {
                m_board.SetSlotStatus(pos, SlotStatus.Selectable);
            }
            
            m_currentSelectableAmount = m_selectableSlots.Count;
            if (m_amount == 0) GameManager.Instance.ChangeToNullState();
            
            // register the on click event for the board
            var boardCamera = GameManager.Instance.GetCameraManager().boardCamera;
            GameManager.Instance.GetInputManager().RegisterClickEvent(boardCamera, OnClicked);

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.BoardConsole
            );
            UIManager.Instance.SetDisplayUI(bitmask);
        }

        public override void Exit()
        {
            var boardCamera = GameManager.Instance.GetCameraManager().boardCamera;
            GameManager.Instance.GetInputManager().UnregisterClickEvent(boardCamera, OnClicked);
            
            foreach (var pos in m_selectableSlots)
            {
                if(m_board.GetSlotStatus(pos)== SlotStatus.Selectable)
                    m_board.SetSlotStatus(pos, SlotStatus.Hidden);
            }
            
            m_selectableSlots.Clear();
            Debug.Log("Exit AddSlot");

            int bitmask = UIManager.Instance.GetDisplayBit(
                UIElements.ModuleInfoCard,
                UIElements.BattleConsole,
                UIElements.BattleResult
            );
            UIManager.Instance.SetDisplayUI(bitmask);
        }

        private void OnClicked(Vector2 worldPosition)
        {
            if (!m_boardView.GetXY(worldPosition, out int x, out int y)) return;

            int currMod = (int) GameManager.Instance.GetAndroid().Get(UnlimitedPropertyType.Mod);
            if(currMod < Constants.ADD_SLOT_COST) return;
            
            if (m_board.GetSlotStatus(x, y) == SlotStatus.Selectable)
            {
                m_board.SetSlotStatus(x, y, SlotStatus.Empty);
                GameManager.Instance.GetAndroid().Decrease(UnlimitedPropertyType.Mod, Constants.ADD_SLOT_COST);    
                m_amount--;
                m_currentSelectableAmount--;
                
                var lists = m_board.GetAdjacentSlots(x, y);

                foreach (var pos in lists)
                {
                    m_board.SetSlotStatus(pos, SlotStatus.Selectable);
                    m_selectableSlots.Add(pos);
                    m_currentSelectableAmount++;
                }
                
                if (m_amount <= 0 || m_currentSelectableAmount <= 0)
                {
                    GameManager.Instance.ChangeToNullState();
                }
            }
        }

        public static AddSlotState CreateAddSlotState(BoardView boardView, Board board, int amount)
        {
            Debug.Assert(amount > 0, "amount must be greater than 0");
            var state = new AddSlotState
            {
                m_boardView = boardView,
                m_board = board,
                m_amount = amount
            };
            
            return state;
        }
    }
}