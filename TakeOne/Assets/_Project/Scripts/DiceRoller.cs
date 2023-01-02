using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class DiceRoller : MonoBehaviour
    {
        // The dice prefab that we will be instantiating
        [FormerlySerializedAs("dicePrefab")] public GameObject defaultDicePrefab;

        // The force that will be applied to the dice when it is rolled
        [SerializeField] private Vector2 diceForce;

        // The torque that will be applied to the dice when it is rolled
        [SerializeField] private Vector2 diceTorque;

        [SerializeField] private float spawnRadius = 1f;
        
        public void RollDie(GameObject dieType = null)
        {
            if (dieType == null) dieType = defaultDicePrefab;

            // Instantiate a new dice at the position of the DiceRoller game object
            GameObject dice = Instantiate(dieType, transform.position + Random.insideUnitSphere * spawnRadius, Quaternion.identity);
            RandomizeRotation(dice);
            dice.GetComponent<DiceFace>().LaunchDice(diceForce, diceTorque);

            // // Apply a random force to the dice
            // dice.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * Random.Range(diceForce.x, diceForce.y), ForceMode.Impulse);
            //
            // // Apply a random torque to the dice
            // dice.GetComponent<Rigidbody>().AddTorque(Random.Range(diceTorque.x, diceTorque.y), Random.Range(diceTorque.x, diceTorque.y), Random.Range(diceTorque.x, diceTorque.y), ForceMode.Impulse);
        }
        
        // Randomize the rotation of an object
        void RandomizeRotation(GameObject obj)
        {
            // Generate random rotation values
            float x = Random.Range(0f, 360f);
            float y = Random.Range(0f, 360f);
            float z = Random.Range(0f, 360f);

            // Set the object's rotation to the random values
            obj.transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
}