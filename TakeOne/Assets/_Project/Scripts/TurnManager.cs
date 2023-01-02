using UnityEngine;

namespace DiceGame
{
    public class TurnManager : MonoBehaviour
    {
        public bool IsPlayerTurn { get; set; }

        public void EndTurn()
        {
            IsPlayerTurn = !IsPlayerTurn;
        }
    }
}
