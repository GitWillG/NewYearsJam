using System;
using DiceGame.ScriptableObjects.Dice;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    //The order of the sides is really important here. a lot of places use this exact order and are hard coded.
    public enum ObjectDirections
    {
        Up,
        Right,
        Backward,
        Forward,
        Left,
        Down,
    }
    
    /// <summary>
    /// Finds the current face of the dice that is pointing up after being rolled.
    /// </summary>
    public class DiceFace : MonoBehaviour
    {
        // A threshold for determining if the dice is rolling or not
        [SerializeField] private float rollingThreshold = 0.2f;

        private Rigidbody _rigidbody;
        private Vector3[] _faceRotations = new Vector3[6];// The rotations of the die's faces, in local space
        private int _sideRolled;
        private float _rollingTimer;// A timer for checking if the dice has stopped rolling
        private bool _isResultFound;
        private DiceController _diceController;
        private DiceSO _diceType;
        private int[] _faceValues = new int[6];// The possible face values of the dice
        
        public ObjectDirections ObjectDirectionsEnum { get; private set; }
        public int FaceValue { get; private set; }
        public bool IsResultFound => _isResultFound;
        public UnityEvent<int> onDiceRollResult;

        private void Awake()
        {
            AssignReferences();
        }

        private void AssignReferences()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _diceController = GetComponent<DiceController>();
        }

        //Gets the side of the dice based on data
        public void InitDieFace(DiceSO diceType)
        {
            _diceType = diceType;
            _faceValues = _diceType.DieSides.ToArray();
        }

        //Sets and stores the current up in local space
        private void UpdateFaceRotations()
        {
            var diceTransform = transform;
            var up = transform.InverseTransformDirection(diceTransform.up);
            var forward = transform.InverseTransformDirection(diceTransform.forward);
            var right = transform.InverseTransformDirection(diceTransform.right);

            _faceRotations = new[]
            {
                up,
                right,
                -forward,
                forward,
                -right,
                -up,
            };
        }

        private void Update()
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

            if (_sideRolled == -1)
            {
                _rigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
                _diceController.LaunchDice();
            }

            // If the rolling timer has reached the threshold, output the result
            if (_rollingTimer >= rollingThreshold)
            {
                FaceValue = GetDieFace();
                _isResultFound = true;  
                onDiceRollResult?.Invoke(FaceValue);
            }

        }
        
        private bool IsRolling()
        {
            Vector3 angularVelocity = _rigidbody.angularVelocity;

            float rollingSpeed = Mathf.Abs(angularVelocity.x) + Mathf.Abs(angularVelocity.y) + Mathf.Abs(angularVelocity.z);

            return rollingSpeed > rollingThreshold;
        }
        
        private int GetDieFace()
        {
            // Get the die's up direction
            Vector3 up = transform.InverseTransformDirection(Vector3.up);
            UpdateFaceRotations();
            
            // Find the die face that is closest to the up direction
            float closestDot = float.MinValue;
            int closestDieFace = _faceValues[0];
            
            foreach (var direction in _faceRotations)
            {
                float dot = Vector3.Dot(up, direction);
                if (dot > closestDot)
                {
                    closestDot = dot;
                    ObjectDirectionsEnum = (ObjectDirections)Array.IndexOf(_faceRotations, direction);
                    closestDieFace = _faceValues[Array.IndexOf(_faceRotations, direction)];
                }
            }
            return closestDieFace;
        }
    }
}