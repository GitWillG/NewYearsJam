using System.Collections.Generic;
using DiceGame.Managers;
using UnityEngine;

namespace DiceGame.Relics
{
    public class RelicManager : MonoBehaviour
    {
        private List<RelicController> _allRelics;
        private PartyManager _partyManager;
        
        //Whenever Party Turn start
        public void PartyTurnStarted()
        {
            foreach (var relic in _allRelics)
            {
                relic.OnPartyTurnStart(_partyManager);
            }
        }
        
        
    }
}