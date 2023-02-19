using UnityEngine;

namespace DiceGame.Managers
{
    public class TurnManager : MonoBehaviour
    {
        private UIManager _uIManager;
        private PartyManager _partyManager;
        private GameEventPropagator _gameEventPropagator;
        
        public bool IsPlayerTurn { get; set; }


        private void Awake()
        {
            _partyManager = FindObjectOfType<PartyManager>();
            _gameEventPropagator = FindObjectOfType<GameEventPropagator>();
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
                _gameEventPropagator.OnPartyTurnStart(_partyManager);
                _partyManager.StartNewTurn();
            }
            else
            {
                _gameEventPropagator.OnPartyTurnEnd(_partyManager);
            }
        }
        public void NewEncounter()
        {
            IsPlayerTurn = true;
            _uIManager.EnableUIElement(_uIManager.RollDice);
        }
    }
}
