using System.Threading.Tasks;
using DiceGame.Managers;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.Relics
{
    public class TestRelic : MonoBehaviour, ICombatEventListener
    {
        public async void OnPartyTurnStart(PartyManager partyManager)
        {
            await Task.Delay(500);
            partyManager.Health += 5;
            Debug.Log("Test Relic Granted 5 health!");
        }

        public void OnPartyTurnEnd(PartyManager partyManager)
        {
            
        }

        public void OnEnemyTurnStart(MonsterManager monsterManager)
        {
            
        }

        public void OnEnemyTurnEnd(MonsterManager monsterManager)
        {
           
        }

        public void OnDealDamage(IDamageable target, IDamageDealer owner)
        {
            
        }

        public void OnBlock(IDamageable target, IDamageDealer owner)
        {
            // return Task;
        }
    }
}