using System.Collections.Generic;
using DiceGame.Dice;
using DiceGame.Managers;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.Relics
{
    public class TestRelic : MonoBehaviour, ICombatEventListener, IDiceEventListener
    {
        private List<DiceController> _diceControllers = new();
        
        private int _dieValueModdedBy = 2;
        private bool _shouldTrigger;

        public async void OnPartyTurnStart(PartyManager partyManager)
        {
            if (_shouldTrigger)
            {
                // await Task.Delay(500);
                partyManager.Health += 5;
                Debug.Log("Test Relic Granted 5 health!");
            }
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
                _shouldTrigger = false;
                break;
            }
        }
    }
}