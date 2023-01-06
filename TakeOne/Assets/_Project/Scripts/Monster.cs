using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame
{
    public class Monster : MonoBehaviour
    {
        private MonsterSO monsterSo;
        private List<int> dieResults;
        public int monsterLife;
        private MonsterManager _monsterManager;

        private DiceSlot _diceSlot;

        public MonsterSO MonsterSo => monsterSo;


        public void InitializeMonster(MonsterSO so, Transform spawnLocation, Transform diceSlotLocation, MonsterManager MManager)
        {
            monsterSo = so;
            transform.parent = spawnLocation;
            //Spawn Visuals
            var visuals = Instantiate(MonsterSo.MonsterVisualPrefab, transform);

            transform.localPosition = Vector3.zero;

            Debug.Log(MonsterSo.DiceSlotSo.SlotPrefab);
            //Spawn Die slots
            _diceSlot = Instantiate(MonsterSo.DiceSlotSo.SlotPrefab, transform).GetComponent<DiceSlot>();
            _diceSlot.transform.position = diceSlotLocation.localPosition;
            monsterLife = MonsterSo.Health;
            _monsterManager = MManager;


            
            //Scale Health Bar based on monster stats
            
        }
        
        public void TryDealDamage()
        {
            var dieRolls = _diceSlot.GetDiceResults();
            Debug.Log(dieRolls[0]);
            var damage = MonsterSo.DamageFromCondition(dieRolls);
            if (monsterLife - damage <= 0)
            {
                Debug.Log("dead");
                monsterLife = 0;
                Invoke(nameof(killSelf), 0.2f);
            }
            else
            {
                Debug.Log(damage);
                monsterLife -= damage;
            }

            //Take damage from health
        }
        public void killSelf()
        {
            _monsterManager.removeDead(this);
            GameObject.Destroy(this.gameObject);   
        }
    }
}