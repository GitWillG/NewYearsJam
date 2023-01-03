using System.Collections.Generic;
using DiceGame.ScriptableObjects;
using UnityEngine;

namespace DiceGame
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] private MonsterSO monsterSo;
        [SerializeField] private List<int> dieResults;
        public void InitializeMonster(MonsterSO so)
        {
            monsterSo = so;
            //Spawn Health Bar
            //Spawn Die slots
            //Spawn Visuals
        }

        public void TestMonsterDamage()
        {
            var something = monsterSo.DamageFromCondition(dieResults);

            Debug.Log("Damage dealt = "+ something);
        }

        public void TryDealDamage(List<int> dieRolls)
        {
            var damage = monsterSo.DamageFromCondition(dieRolls);
            //Take damage from health
        }
    }
}