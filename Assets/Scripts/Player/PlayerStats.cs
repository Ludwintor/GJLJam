using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GJLJam
{
    [System.Serializable]
    public class PlayerStats : Stats
    {
        public float InvulnerableTime { get; private set; }

        private Dictionary<Buttons, int> AvailableMoves { get; set; } = new Dictionary<Buttons, int>();
        private List<Buttons> movementPriorityReceive = new List<Buttons>(5);

        private MovementUI movementUI;

        public PlayerStats(PlayerDataObject data, MovementUI movementUI) : base(data.Health, data.MovementSpeed)
        {
            this.movementUI = movementUI;
            AvailableMoves.Add(Buttons.Space, 0); // SPACE
            AvailableMoves.Add(Buttons.Right, 0); // D
            AvailableMoves.Add(Buttons.Left, 0); // A
            AvailableMoves.Add(Buttons.Up, 0); // W
            AvailableMoves.Add(Buttons.Down, 0); // S

            AddForEachMove();

            InvulnerableTime = data.InvulnerableTime;
        }

        public void AddMove(Buttons button)
        {
            AvailableMoves[button] += 1;
            movementUI.UpdateMovementButton(button, AvailableMoves[button], false);
        }

        public void RemoveMove(Buttons button, bool currentMove)
        {
            AvailableMoves[button] -= 1;
            movementUI.UpdateMovementButton(button, AvailableMoves[button], currentMove);
        }

        public void RemoveMove(Vector2 move, bool currentMove) => RemoveMove(VectorToButton(move), currentMove);

        public void AddForEachMove()
        {
            var keys = AvailableMoves.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                AddMove(keys[i]);
            }
        }

        public int MovementCount(Vector2 move) => AvailableMoves[VectorToButton(move)];

        public void ReceiveMovement(Enemy _)
        {
            movementPriorityReceive.Clear();
            foreach (KeyValuePair<Buttons, int> kvp in AvailableMoves)
            {
                if (kvp.Value == 0)
                    movementPriorityReceive.Add(kvp.Key);
            }

            if (movementPriorityReceive.Count > 0)
            {
                int index = Random.Range(0, movementPriorityReceive.Count);
                Buttons move = movementPriorityReceive[index];
                AddMove(move);
            }
            else
            {
                Vector2 move = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));

                AddMove(VectorToButton(move));
            }
        }

        private Buttons VectorToButton(Vector2 move)
        {
            switch (move.x)
            {
                case 1:
                    return Buttons.Right;
                case 0:
                    break;
                case -1:
                    return Buttons.Left;
            }

            switch (move.y)
            {
                case 1:
                    return Buttons.Up;
                case 0:
                    break;
                case -1:
                    return Buttons.Down;
            }

            return Buttons.Space;
        }
    }
}