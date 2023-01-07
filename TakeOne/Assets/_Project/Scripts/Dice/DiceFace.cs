using System;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    public class DiceFace : MonoBehaviour
    {
        // The possible face values of the dice
        private int[] faceValues = new int[6];

        // A threshold for determining if the dice is rolling or not
        [SerializeField] private float rollingThreshold = 0.2f;
        [SerializeField] private DiceSO diceType;

        private Rigidbody _rigidbody;
        private Vector3[] _faceRotations = new Vector3[6];// The rotations of the dice's faces, in local space
        private int _sideRolled;
        private float _rollingTimer;// A timer for checking if the dice has stopped rolling
        private bool _isResultFound;
        private DiceController _diceController;
        
        private Vector3 _dieUp;
        
        public DiceSO DiceType 
        { 
            get => diceType; 
            set => diceType = value; 
        }
        public int FaceValue 
        { 
            get; 
            private set; 
        }
        public bool IsResultFound => _isResultFound;
        
        public UnityEvent<int> onDiceRollResult;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _diceController = GetComponent<DiceController>();
        }

        private void Start()
        {
            InitDieFace();
        }

        private void InitDieFace()
        {
            //Iterates through the dice faces applying 
            for (int i = 0; i <= 5; i++)
            {
                faceValues[i] = diceType.DieSides[i % diceType.DieSides.Count];
                //TODO: apply sprites to appropriate face
                //DiceFaceImage[i] = diceType.FaceSprites[faceValues[i]];
            }
        }

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
                Debug.Log("The result is: " + FaceValue, this);
                _isResultFound = true;  
                onDiceRollResult?.Invoke(FaceValue);
            }

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
        
        private int GetDieFace()
        {
            // Get the dice's up direction
            // Vector3 up = transform.up;
            Vector3 up = transform.InverseTransformDirection(Vector3.up);
            UpdateFaceRotations();
            // Find the die face that is closest to the up direction
            float closestDot = float.MinValue;
            int closestDieFace = faceValues[0];
            Vector3 closestDirection = Vector3.zero;
            foreach (var direction in _faceRotations)
            {
                float dot = Vector3.Dot(up, direction);
                if (dot > closestDot)
                {
                    closestDirection = direction;
                    closestDot = dot;
                    closestDieFace = faceValues[Array.IndexOf(_faceRotations, direction)];
                }
            }
            //TODO: Rotate object such that the closest direction is pointing up and Y = 0;
            _dieUp = closestDirection;
            return closestDieFace;
        }
    }
}