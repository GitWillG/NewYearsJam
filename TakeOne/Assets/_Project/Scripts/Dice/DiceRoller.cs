using DiceGame.ScriptableObjects;
using DiceGame.ScriptableObjects.Dice;
using DiceGame.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    /// <summary>
    /// Rolls a given die, randomizes starting rotation and launches in random direction.
    /// </summary>
    public class DiceRoller : MonoBehaviour
    {
        public UnityEvent onLaunchAllDice;
        
        [SerializeField] private GameObject defaultDicePrefab;
        [SerializeField] private Vector2 diceForce;
        [SerializeField] private Vector2 diceTorque;
        [SerializeField] private float spawnRadius = 1f;
        [SerializeField] private DiceManager diceMan;
        
        public void RollDie(HeroSO diceOwner, DiceSO diceSo ,GameObject dieType = null)
        {
            if (dieType == null) dieType = defaultDicePrefab;
            onLaunchAllDice?.Invoke();

            DiceController dice = Instantiate(dieType, transform.position + Random.insideUnitSphere * spawnRadius, Quaternion.identity).GetComponent<DiceController>();
            dice.Initialize(diceOwner, diceSo);
            diceMan.RolledDice.Add(dice);
            dice.transform.RandomizeRotation();
            dice.GetComponent<DiceController>().LaunchDice(diceForce, diceTorque);
        }
    }
}