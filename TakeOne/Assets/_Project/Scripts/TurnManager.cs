using UnityEngine;

namespace DiceGame
{
    public class TurnManager : MonoBehaviour
    {
        private UIManager uIManager;
        public bool IsPlayerTurn { get; set; }
        public UIManager UIManager 
        { 
            get => uIManager; 
            set => uIManager = value; 
        }

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
                UIManager.EnableUIElement(UIManager.RollDice);
            }
        }
    }
}
