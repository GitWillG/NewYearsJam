using UnityEngine;

namespace DiceGame.Managers
{
    public class TurnManager : MonoBehaviour
    {
        private UIManager _uIManager;
        private PartyManager _partyManager;
        private RelicManager _relicManager;
        
        public bool IsPlayerTurn { get; set; }


        private void Awake()
        {
            _partyManager = FindObjectOfType<PartyManager>();
            _relicManager = FindObjectOfType<RelicManager>();
        }

        private void Start()
        {
            _uIManager = FindObjectOfType<UIManager>();
            //IsPlayerTurn = true;
            NewEncounter(); //<- temp
        }

        public void EndTurn()
        {
            IsPlayerTurn = !IsPlayerTurn;
            if (IsPlayerTurn)
            {
                _uIManager.EnableUIElement(_uIManager.RollDice);
                _relicManager.OnPartyTurnStart(_partyManager);
                _partyManager.StartNewTurn();
            }
            else
            {
                _relicManager.OnPartyTurnEnd(_partyManager);
            }
        }
        public void NewEncounter()
        {
            IsPlayerTurn = true;
            _uIManager.EnableUIElement(_uIManager.RollDice);
        }
    }
}
