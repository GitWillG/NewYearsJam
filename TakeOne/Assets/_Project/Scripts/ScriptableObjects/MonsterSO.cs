using System.Collections.Generic;
using DiceGame.ScriptableObjects.Conditions;
using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CreateMonster", menuName = "Assets/Create/createMonster", order = 1)]
    public class MonsterSO : ScriptableObject
    {
        [SerializeField] private string monsterName;
        [SerializeField] private string monsterSkills;
        [SerializeField] private int damage;
        [SerializeField] private int health;
        [SerializeField] private Condition damageCondition;
        [SerializeField] private Sprite visual;

        public string MonsterName => monsterName;
        public string MonsterSkills => monsterSkills;
        public int Health => health;

        public int AttackDamage
        {
            get => damage;
            set => damage = value;
        }
        
        public bool IsAlive { get; set; }
        public Sprite Visual 
        { 
            get => visual; 
            set => visual = value; 
        }

        public int DamageFromCondition(List<int> dieResults)
        {
           damageCondition.EvaluateConditions(dieResults);
           return damageCondition.GetDamage();
        }
    }
}
