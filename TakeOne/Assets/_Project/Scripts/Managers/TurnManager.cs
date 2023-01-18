using UnityEngine;

namespace DiceGame.Managers
{
    public class TurnManager : MonoBehaviour
    {
        private UIManager _uIManager;
        public bool IsPlayerTurn { get; set; }

        private void Start()
        {
            _uIManager = FindObjectOfType<UIManager>();
            IsPlayerTurn = true;
        }

        public void EndTurn()
        {
            IsPlayerTurn = !IsPlayerTurn;
            if (IsPlayerTurn)
            {
                _uIManager.EnableUIElement(_uIManager.RollDice);
            }
        }
    }
}
