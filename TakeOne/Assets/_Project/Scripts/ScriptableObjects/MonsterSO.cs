using System.Collections.Generic;
using System.Linq;
using DiceGame.ScriptableObjects.Conditions;
using UnityEngine;
using UnityEngine.Serialization;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CreateMonster", menuName = "Assets/Create/createMonster", order = 1)]
    public class MonsterSO : ScriptableObject
    {
        [SerializeField] private string monsterName;
        [SerializeField] private string monsterSkills;
        [SerializeField] private Vector2Int damageMinMax;
        [SerializeField] private int health;
        [SerializeField] private Condition damageCondition;
        [FormerlySerializedAs("visual")] [SerializeField] private Sprite monsterSprite;

        public string MonsterName => monsterName;
        public string MonsterSkills => monsterSkills;
        public int Health => health;
        public int DamageModifier { get; set; } // don't worry Rider
        public int Damage => Random.Range(damageMinMax.x, damageMinMax.y) + DamageModifier;

        public bool IsAlive { get; set; }

        public Sprite MonsterSprite
        {
            get => monsterSprite;
            set => monsterSprite = value;
        }

        public int DamageFromCondition(List<int> dieResults)
        {
            if (damageCondition == null)
            {
                return dieResults.Sum(x => x);
            }

            damageCondition.EvaluateConditions(dieResults);
            return damageCondition.GetDamage();
        }
    }
}
