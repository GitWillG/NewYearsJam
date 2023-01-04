using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] private MonsterSO monsterSo;
        [SerializeField] private List<int> dieResults;

        private DiceSlot _diceSlot;
        public void InitializeMonster(MonsterSO so, Transform spawnLocation)
        {
            monsterSo = so;
            transform.position = spawnLocation.position;

            //Spawn Die slots
            _diceSlot = Instantiate(monsterSo.DiceSlotSo.SlotPrefab).GetComponent<DiceSlot>(); 

            //Spawn Visuals
            var visuals = Instantiate(monsterSo.MonsterVisualPrefab, transform);
            visuals.transform.position = Vector3.zero;
            
            //Scale Health Bar based on monster stats
            
        }
        
        public void TryDealDamage()
        {
            var dieRolls = _diceSlot.GetDiceResults();
            var damage = monsterSo.DamageFromCondition(dieRolls);
            //Take damage from health
        }
    }
}