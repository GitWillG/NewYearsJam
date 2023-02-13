using System.Collections.Generic;
using System.Linq;
using DiceGame.ScriptableObjects.Conditions;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CreateMonster", menuName = "Assets/Create/createMonster", order = 1)]
    public class MonsterSO : ScriptableObject, IDiceOwner, IDamageDealer
    {
        [SerializeField] private string monsterName;
        [SerializeField] private string monsterSkills;
        [SerializeField] private Vector2Int damageMinMax;
        [SerializeField] private int maxHealth;
        [SerializeField] private Condition damageCondition;
        [SerializeField] private DiceSlotSO diceSlotSo;
        [SerializeField] private GameObject monsterVisualPrefab;
        [SerializeField] private GameObject attackEffectPrefab;

        public string MonsterName => monsterName;
        public string MonsterSkills => monsterSkills;
        public int MAXHealth => maxHealth;
        public int DamageModifier { get; set; } // don't worry Rider
        public int Damage => Random.Range(damageMinMax.x, damageMinMax.y) + DamageModifier;

        public bool IsAlive { get; set; }

        public GameObject MonsterVisualPrefab => monsterVisualPrefab;

        public DiceSlotSO DiceSlotSo => diceSlotSo;

        public Vector2Int DamageMinMax => damageMinMax;

        public Condition DamageCondition => damageCondition;

        public int DamageAmount => Damage;
        public GameObject AttackEffectPrefab => attackEffectPrefab;
        
    }
}
