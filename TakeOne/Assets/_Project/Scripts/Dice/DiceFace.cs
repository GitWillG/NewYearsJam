using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace DiceGame.Dice
{
    public class DiceFace : MonoBehaviour
    {
        // The possible face values of the dice
        private int[] faceValues = new int[6];
        private Material _diceMat;

        // A threshold for determining if the dice is rolling or not
        [SerializeField] private float rollingThreshold = 0.2f;
        [SerializeField] private float lerpSpeed = 10;
        [SerializeField] private DiceSO diceType;

        private Rigidbody _rigidbody;
        private Vector2 _cachedDiceForce, _cachedDiceTorque;
        private Vector3[] _faceRotations = new Vector3[6];// The rotations of the dice's faces, in local space
        private int _sideRolled;
        private float _rollingTimer;// A timer for checking if the dice has stopped rolling
        private bool _isResultFound;
        private float _distanceCheckThreshold = 0.1f;
        private DiceSlot _currentSlot;
        
        public bool isInTray;
        public Transform _anchorTransform;

        private static readonly int Isflashing = Shader.PropertyToID("_IsFlashing");
        private static readonly int StartTime = Shader.PropertyToID("_StartTime");
        private static readonly int IsHovering = Shader.PropertyToID("_IsHover");
        
        private CancellationTokenSource _cancelSource = new CancellationTokenSource();


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
        public bool IsInSlot => _currentSlot != null;

        public DiceSlot CurrentSlot
        {
            get => _currentSlot;
            set => _currentSlot = value;
        }

        public UnityEvent<int> onDiceRollResult;

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
            _diceMat = gameObject.GetComponent<MeshRenderer>().material;
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
                LaunchDice(_cachedDiceForce, _cachedDiceTorque);
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
            return closestDieFace;
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

        public void DetachFromSlot()
        {
            if(!IsInSlot) return;
            
            _currentSlot.RemoveFromDiceSlot(this);
            _currentSlot = null;
        }

        public void SetAnchor(Transform anchorTransform, bool snapToAnchor = false)
        {
            _cancelSource.Cancel();
            _anchorTransform = anchorTransform;

            if (snapToAnchor)
            {
                transform.position = anchorTransform.position;
                return;
            }

            _cancelSource = new CancellationTokenSource();
            LerpAsync(anchorTransform, _cancelSource.Token);
        }
        
        async Task LerpAsync(Transform target , CancellationToken cancelToken)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            while (distance > _distanceCheckThreshold)
            {
                cancelToken.ThrowIfCancellationRequested();

                transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeed * Time.deltaTime);

                distance = Vector3.Distance(transform.position, target.position);

                await Task.Yield();
            }
        }
        
        async Task RotateAsync(Vector3 endRotation)
        {
            // Calculate the remaining angle to the end rotation
            float angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(endRotation));

            // Continue rotating as long as the angle to the end rotation is greater than the threshold
            while (angle > _distanceCheckThreshold)
            {
                // Rotate towards the end rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(endRotation), lerpSpeed * Time.deltaTime);

                // Update the angle to the end rotation
                angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(endRotation));

                await Task.Yield();
            }
        }

        public void DestroyDice()
        {
            //Logic for cleaning up here and spawning shit as needed.
            Destroy(gameObject);
        }
        
        public void HoverOnDice(bool to)
        {
            _diceMat.SetInt(IsHovering, to? 1 : 0);
        }
        public void HighlightDice()
        {
            _diceMat.SetInt(Isflashing, 1);
            _diceMat.SetFloat(StartTime, Time.time);
        }
        public void RemoveHighlight()
        {
            _diceMat.SetInt(Isflashing, 0);
        }
    }
}