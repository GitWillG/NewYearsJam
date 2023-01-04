using UnityEngine;

namespace DiceGame
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private UIManager uIManager;
        public bool IsPlayerTurn { get; set; }
        public UIManager UIManager 
        { 
            get => uIManager; 
            set => uIManager = value; 
        }

        private void Start()
        {
            IsPlayerTurn = true;
        }

        public void EndTurn()
        {
            IsPlayerTurn = !IsPlayerTurn;
            if (IsPlayerTurn)
            {
                UIManager.EnableUIElement(UIManager.RollDice);
            }
        }
    }
}
