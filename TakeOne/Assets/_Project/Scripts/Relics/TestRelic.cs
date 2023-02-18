using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.Relics
{
    public class TestRelic : MonoBehaviour, ICombatEventListener, IDiceEventListener, IRelic
    {
        private List<DiceController> _diceControllers = new();
        
        private int _dieValueModdedBy = 2;
        private bool _allDieMeetCondition = true;
        
        private PartyManager _partyManager;
        
        public bool CanTrigger { get; set; }

        public void OnPartyTurnStart(PartyManager partyManager)
        {
            _partyManager = partyManager;
            TriggerPrimaryEffect();
        }
        public void OnDiceSelected(DiceController diceController)
        {
            _diceControllers.Add(diceController);
        }
        public void OnConfirmAllDie(List<DiceController> diceControllers)
        {
            foreach (var diceController in _diceControllers)
            {
                if (diceController.FaceValue % _dieValueModdedBy != 0) continue;
                _allDieMeetCondition = false;
                break;
            }
        }
        public async void TriggerPrimaryEffect()
        {
            if(!CanTrigger || !_allDieMeetCondition) return;
            
            // await Task.Delay(500);
            _partyManager.Health += 5;
            Debug.Log("Test Relic Granted 5 health!");
        }
    }
}