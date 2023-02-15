using System.Collections.Generic;
using System.Linq;
using DiceGame.Dice;
using DiceGame.ScriptableObjects.Conditions;
using UnityEngine;

namespace DiceGame.Utility
{
    public interface IDamageable
    {
        public Condition DamageCondition { get; }

        public bool TryTakeDamage(IDamageDealer damageDealer, out int damageTaken);
        public int CalculateDamageNegation();
        public void Init(int health, int maxHealth, Condition damageCondition = null, DiceSlotHolder damageSlot = null);
        
        public int DamageFromCondition(List<int> dieResults)
        {
            if (DamageCondition == null)
            {
                return dieResults.Sum(x => x);
            }

            DamageCondition.EvaluateConditions(dieResults);
            return DamageCondition.GetResult();
        }
    }
    
    public interface IDamageDealer
    {
        public int DamageAmount { get; }
        public GameObject AttackEffectPrefab { get; }
    }
}