using UnityEngine;

namespace DiceGame
{
    public class TurnManager : MonoBehaviour
    {
        private UIManager uIManager;
        public bool IsPlayerTurn { get; set; }

        private void Start()
        {
            uIManager = GameObject.FindObjectOfType<UIManager>();
            IsPlayerTurn = true;
        }

        public void EndTurn()
        {
            IsPlayerTurn = !IsPlayerTurn;
            if (IsPlayerTurn)
            {
                uIManager.EnableUIElement(uIManager.RollDice);
            }
        }
    }
}
