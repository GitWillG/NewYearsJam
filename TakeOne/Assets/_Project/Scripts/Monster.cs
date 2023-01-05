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

        public void InitializeMonster(MonsterSO so, Transform spawnLocation, Transform diceSlotLocation)
        {
            monsterSo = so;
            transform.parent = spawnLocation;
            //Spawn Visuals
            var visuals = Instantiate(monsterSo.MonsterVisualPrefab, transform);

            transform.localPosition = Vector3.zero;

            Debug.Log(monsterSo.DiceSlotSo.SlotPrefab);
            //Spawn Die slots
            _diceSlot = Instantiate(monsterSo.DiceSlotSo.SlotPrefab, transform).GetComponent<DiceSlot>();
            _diceSlot.transform.position = diceSlotLocation.position;


            
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