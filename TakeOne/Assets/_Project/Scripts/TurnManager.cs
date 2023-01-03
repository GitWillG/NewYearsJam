using UnityEngine;

namespace DiceGame
{
    public class TurnManager : MonoBehaviour
    {
        public bool IsPlayerTurn { get; set; }
        public UIManager _UIManager;
        private void Start()
        {
            IsPlayerTurn = true;
        }

        public void EndTurn()
        {
            IsPlayerTurn = !IsPlayerTurn;
            if (IsPlayerTurn)
            {
                _UIManager.enableUIElement(_UIManager.RollDice);
            }
        }
    }
}
