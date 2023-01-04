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

            _diceSlot = Instantiate(monsterSo.DiceSlotSo.SlotPrefab).GetComponent<DiceSlot>(); //Spawn Die slots

            //Spawn Health Bar
            //Spawn Visuals
        }
        
        public void TryDealDamage()
        {
            var dieRolls = _diceSlot.GetDieResults();
            var damage = monsterSo.DamageFromCondition(dieRolls);
            //Take damage from health
        }
    }
}