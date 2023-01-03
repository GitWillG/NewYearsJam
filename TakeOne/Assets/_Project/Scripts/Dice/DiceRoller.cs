using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.Dice
{
    public class DiceRoller : MonoBehaviour
    {
        // The dice prefab that we will be instantiating
        [SerializeField] private GameObject defaultDicePrefab;
        // The force that will be applied to the dice when it is rolled
        [SerializeField] private Vector2 diceForce;
        // The torque that will be applied to the dice when it is rolled
        [SerializeField] private Vector2 diceTorque;
        [SerializeField] private float spawnRadius = 1f;
        [SerializeField] private DiceManager diceMan;

        public DiceManager DiceMan { get => diceMan; set => diceMan = value; }

        public void RollDie(GameObject dieType = null)
        {
            if (dieType == null) dieType = defaultDicePrefab;

            // Instantiate a new dice at the position of the DiceRoller game object
            GameObject dice = Instantiate(dieType, transform.position + Random.insideUnitSphere * spawnRadius, Quaternion.identity);
            DiceMan.RolledDice.Add(dice);
            RandomizeRotation(dice);
            dice.GetComponent<DiceFace>().LaunchDice(diceForce, diceTorque);
        }
        
        // Randomize the rotation of an object
        private void RandomizeRotation(GameObject obj)
        {
            // Generate random rotation values
            var x = Random.Range(0f, 360f);
            var y = Random.Range(0f, 360f);
            var z = Random.Range(0f, 360f);

            // Set the object's rotation to the random values
            obj.transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
}