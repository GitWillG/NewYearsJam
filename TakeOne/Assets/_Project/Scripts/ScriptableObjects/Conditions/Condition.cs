using System;
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
        EqualTo
    }
    
    [CreateAssetMenu(order = 0, fileName = "NewCondition", menuName = "Create New Condition")]
    public class Condition : ScriptableObject
    {
        [SerializeField] private string conditionDescription;
        [SerializeField] private bool allDieShouldMeetCondition;
        [SerializeField] private bool anyDieShouldMeetCondition;
        [SerializeField] private int amount;
        [SerializeField] private ConditionType conditionType;

        private delegate bool ConditionDelegate(int val);

        private ConditionDelegate _myDelegate;

        private readonly List<DieValeResult> _dieValeResults = new();

        private void Awake()
        {
            throw new NotImplementedException();
        }

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
                _ => _ => true
            };
            foreach (var val in results)
            {
                var result = new DieValeResult(val, _myDelegate(val));
                Debug.Log("For value : " + result.value + "Condition met? : "+ result.result);

                _dieValeResults.Add(result);
            }

            var resultString = results.Aggregate("", (current, val) => current + ("(" + val + ")"));
            Debug.Log("Condition check, Needed : " + conditionDescription + " Die result : " + resultString);
        }

        private bool IsEqualTo(int val) => val == amount;
        private bool IsGreaterThan(int val) => val > amount;
        private bool IsLessThan(int val) => val < amount;
        private static bool IsOdd(int val) => val % 2 == 1;
        private static bool IsEven(int val) => val % 2 == 0;
        
        public int GetDamage()
        {
            var anyTrue = _dieValeResults.Any(x => x.result);
            var anyFalse = _dieValeResults.Any(x => !x.result);
            
            if (allDieShouldMeetCondition)
            {
                if (anyFalse)
                {
                    Debug.Log("All die needed to meet condition, failed.");
                    return 0;
                }

                return DamageFromPassingDie();
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
            int totalDamage = 0;
            totalDamage = _dieValeResults.Sum(x => x.value);
            return totalDamage;
        }

        private int DamageFromPassingDie()
        {
            int totalDamage = 0;
            totalDamage = _dieValeResults.Sum(x => x.result ? x.value : 0);
            Debug.Log("All die met the condition, total damage to deal :" + totalDamage);
            return totalDamage;
        }
    }
}

public struct DieValeResult
{
    public int value;
    public bool result;

    public DieValeResult(int val, bool res)
    {
        value = val;
        result = res;
    }
}


