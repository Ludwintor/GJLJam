using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJLJam
{
    public class MovementUI : MonoBehaviour
    {
        private Dictionary<Buttons, MoveButtonUI> buttons = new Dictionary<Buttons, MoveButtonUI>();

        [SerializeField]
        private Color currentMoveColor;
        [SerializeField]
        private Color unavailableColor;

        private MoveButtonUI currentMove;
        private int currentMoveCount;

        private void Awake()
        {
            foreach (MoveButtonUI movementButton in GetComponentsInChildren<MoveButtonUI>())
            {
                buttons.Add(movementButton.MoveRepresentation, movementButton);
            }

            currentMove = null;
            currentMoveCount = 0;
        }

        public void UpdateMovementButton(Buttons move, int count, bool newMove)
        {
            MoveButtonUI currentButton = buttons[move];

            if (newMove)
            {
                if (currentMove != null)
                {
                    Color currentColor = currentMoveCount > 0 ? Color.white : unavailableColor;
                    currentMove?.UpdateUI(currentMoveCount, currentColor);
                }
                currentMove = currentButton;
            }

            if (currentButton == currentMove)
            {
                currentMoveCount = count;
                currentButton.UpdateUI(count, currentMoveColor);
            }
            else
            {
                Color color = count > 0 ? Color.white : unavailableColor;
                currentButton.UpdateUI(count, color);
            }
        }
    }

    public enum Buttons
    {
        Up,
        Down,
        Left,
        Right,
        Space
    }
}
