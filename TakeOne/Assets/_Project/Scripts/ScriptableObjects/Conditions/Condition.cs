using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame.ScriptableObjects.Conditions
{
    public enum ConditionType
    {
        Even,
        Odd,
        LessThan,
        GreaterThan,
        EqualTo, 
        LessThanEqualTo,
        GreaterThanEqualTo,
        NoCondition
    }
    
    /// <summary>
    /// Calculates the incoming damage based on conditions setup in the SO.
    /// </summary>
    [CreateAssetMenu(order = 0, fileName = "NewCondition", menuName = "Create New Condition")]
    public class Condition : ScriptableObject
    {
        [SerializeField] private string conditionDescription;
        [SerializeField] private bool allDieShouldMeetCondition;
        [SerializeField] private bool anyDieShouldMeetCondition;
        [SerializeField] private int amount;
        [SerializeField] private ConditionType conditionType;
        [SerializeField] private Color conditionColor;

        private delegate bool ConditionDelegate(int val);

        private ConditionDelegate _myDelegate;

        private readonly List<DieValeResult> _dieValeResults = new List<DieValeResult>();

        public ConditionType ConditionType => conditionType;

        public Color ConditionColor => conditionColor;

        public int Amount { get => amount; set => amount = value; }
        public string ConditionDescription { get => conditionDescription; set => conditionDescription = value; }

        public void EvaluateConditions(List<int> results)
        {
            _dieValeResults.Clear();
            _myDelegate = conditionType switch
            {
                ConditionType.Even => IsEven,
                ConditionType.Odd => IsOdd,
                ConditionType.LessThan => IsLessThan,
                ConditionType.GreaterThan => IsGreaterThan,
                ConditionType.EqualTo => IsEqualTo,
                ConditionType.LessThanEqualTo => IsLessThanEqualTo,
                ConditionType.GreaterThanEqualTo => IsGreaterThanEqualTo,
                ConditionType.NoCondition => NoCondition,
                _ => _ => true
            };
            
            foreach (var val in results)
            {
                var result = new DieValeResult(val, _myDelegate(val));
                _dieValeResults.Add(result);
            }
        }

        private bool IsEqualTo(int val) => val == amount;
        private bool IsGreaterThan(int val) => val > amount;
        private bool IsGreaterThanEqualTo(int val) => val >= amount;
        private bool IsLessThan(int val) => val < amount;
        private bool IsLessThanEqualTo(int val) => val <= amount;
        private static bool IsOdd(int val) => val % 2 == 1;
        private static bool IsEven(int val) => val % 2 == 0;
        private static bool NoCondition(int val) => true;
        
        public int GetDamage()
        {
            var anyTrue = _dieValeResults.Any(x => x.Result);
            var anyFalse = _dieValeResults.Any(x => !x.Result);
            
            if (allDieShouldMeetCondition)
            {
                return anyFalse ? 0 : DamageFromPassingDie();
            }

            if (anyDieShouldMeetCondition)
            {
                if (anyTrue)
                {
                    return DamageFromAllDie();
                }
            }

            return DamageFromPassingDie();
        }

        private int DamageFromAllDie()
        {
            var totalDamage = _dieValeResults.Sum(x => x.Value);
            return totalDamage;
        }

        private int DamageFromPassingDie()
        {
            var totalDamage = _dieValeResults.Sum(x => x.Result ? x.Value : 0);
            return totalDamage;
        }
    }
}

public readonly struct DieValeResult
{
    public readonly int Value;
    public readonly bool Result;

    public DieValeResult(int val, bool result)
    {
        Value = val;
        Result = result;
    }
}


