using System.Collections.Generic;
using System.Linq;
using DiceGame.ScriptableObjects.Conditions;

namespace DiceGame.Utility
{
    public interface IDamageable
    {
        public Condition DamageCondition { get; }

        public bool TryTakeDamage(IDamageDealer damageDealer, out int damageTaken);
        public int CalculateDamageNegation();
        public void Init(int health, Condition damageCondition = null);
        
        public int DamageFromCondition(List<int> dieResults)
        {
            if (DamageCondition == null)
            {
                return dieResults.Sum(x => x);
            }

            DamageCondition.EvaluateConditions(dieResults);
            return DamageCondition.GetDamage();
        }
    }
    
    public interface IDamageDealer
    {
        public int DamageAmount { get; }
        
    }
}