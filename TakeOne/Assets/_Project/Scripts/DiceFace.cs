using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class DiceFace : MonoBehaviour
    {
        // The possible face values of the dice
        [SerializeField] private int[] faceValues;

        // The current face value of the dice
        //This is the face not the face VALUE;
        public int Face { get; private set; }
        public int FaceValue { get; private set; }


        // The rotations of the dice's faces, in local space
        private Vector3[] _faceRotations = new Vector3[6];
    
        // A threshold for determining if the dice is rolling or not
        [SerializeField] private float rollingThreshold = 0.2f;

        // A timer for checking if the dice has stopped rolling
        private float _rollingTimer;

        private bool _isResultFound;

        public UnityEvent<int> onDiceRollResult;

        private Rigidbody _rigidbody;

        private Vector2 _cachedDiceForce, _cachedDiceTorque;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            InitDieFace();
        }

        private void InitDieFace()
        {
            var diceTransform = transform;

            _faceRotations = new[]
            {
                diceTransform.up,
                diceTransform.forward,
                diceTransform.right,
                -diceTransform.up,
                -diceTransform.forward,
                -diceTransform.right,
            };
        }

        void Update()
        {
            if(_isResultFound) return;
        
            // Check if the dice is rolling
            if (IsRolling())
            {
                // If the dice is rolling, reset the rolling timer
                _rollingTimer = 0f;
                return;
            }

            // If the dice is not rolling, increment the rolling timer
            _rollingTimer += Time.deltaTime;
            
            Face = CalculateFace();
            FaceValue = faceValues[Face];

            if (Face == -1)
            {
                _rigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
                LaunchDice(_cachedDiceForce, _cachedDiceTorque);
            }

            // If the rolling timer has reached the threshold, output the result
            if (_rollingTimer >= rollingThreshold)
            {
                Debug.Log("The result is: " + FaceValue);
                _isResultFound = true;  
                onDiceRollResult?.Invoke(FaceValue);
            }

        }

        //TODO: Bring dice in central area after all die finished roll
        private int CalculateFace()
        {
            // Calculate the dice's current upward facing vector
            Vector3 upward = transform.up;

            // Find the face rotation that is closest to the upward vector
            Vector3 closestRotation = GetClosestRotation(upward);
            
            return Array.IndexOf(_faceRotations, closestRotation);
        }

        private bool IsRolling()
        {
            // Get the dice's angular velocity
            Vector3 angularVelocity = GetComponent<Rigidbody>().angularVelocity;

            // Calculate the dice's rolling speed
            float rollingSpeed = Mathf.Abs(angularVelocity.x) + Mathf.Abs(angularVelocity.y) + Mathf.Abs(angularVelocity.z);

            // Check if the rolling speed is above the threshold
            return rollingSpeed > rollingThreshold;
        }
    
        // Find the face rotation that is closest to a given vector
        private Vector3 GetClosestRotation(Vector3 v)
        {
            Vector3 closestRotation = _faceRotations[0];
            float closestAngle = Vector3.Angle(v, closestRotation);
            foreach (Vector3 rotation in _faceRotations)
            {
                float angle = Vector3.Angle(v, rotation);
                if (angle < closestAngle)
                {
                    closestRotation = rotation;
                    closestAngle = angle;
                }
            }
            return closestRotation;
        }

        public void LaunchDice(Vector2 diceForce, Vector2 diceTorque)
        {
            _cachedDiceForce = diceForce;
            _cachedDiceTorque = diceTorque;
            
            // Apply a random force to the dice
            _rigidbody.AddForce(Random.insideUnitSphere * Random.Range(diceForce.x, diceForce.y), ForceMode.Impulse);

            // Apply a random torque to the dice
            _rigidbody.AddTorque(Random.Range(diceTorque.x, diceTorque.y), Random.Range(diceTorque.x, diceTorque.y), Random.Range(diceTorque.x, diceTorque.y), ForceMode.Impulse);
        }
    }
}