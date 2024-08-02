using InGame.Boards;
using InGame.Cores;
using UnityEngine;

namespace InGame.Effects.PlacingEffectRequirements
{
    public class BorderRequirement: PlacingEffectRequirement
    {
        public override RequirementType type => RequirementType.Border;
        public override bool CanTrigger(EffectBlackBoard bb)
        {
            var slot = bb.slot;
            Debug.Assert(slot != null);

            var slotPos = slot.pos;

            var board = GameManager.Instance.GetBoard();
            for (var deltaX = -1; deltaX <= 1; deltaX++)
            {
                for (var deltaY = -1; deltaY <= 1; deltaY++)
                {
                    if (Mathf.Abs(deltaX) + Mathf.Abs(deltaY) != 1) continue;
                    var adjPos = slotPos + new BoardPosition(deltaX, deltaY);

                    if (!board.GetSlotStatus(adjPos.x, adjPos.y, out var status) ||
                        status == SlotStatus.Hidden)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override PlacingEffectRequirement CreateCopy()
        {
            return this;
        }

        public static BorderRequirement CreateRequirement()
        {
            return new BorderRequirement();
        }
    }
}