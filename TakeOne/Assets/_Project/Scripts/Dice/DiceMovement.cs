using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DiceGame.Dice
{
    
    
    /// <summary>
    /// Controls the logic for moving and rotating the die.
    /// </summary>
    public class DiceMovement : MonoBehaviour
    {
        [SerializeField] private float lerpSpeed = 2.5f;
        [SerializeField] private float rotationSpeed = 2.5f;

        private CancellationTokenSource _cancelSource = new CancellationTokenSource();
        public Transform _anchorTransform;
        private bool _hasRotated;
        private float _distanceCheckThreshold = 0.01f;
        private float _rotationCheckThreshold = 0.01f;
        private Vector2 _cachedDiceForce, _cachedDiceTorque;
        private Rigidbody _rigidbody;
        private DiceController _diceController;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _diceController = GetComponent<DiceController>();
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

            if (!_hasRotated)
            {
                RotateAsync(_cancelSource.Token);
            }
        }
        
        async Task LerpAsync(Transform target , CancellationToken cancelToken)
        {
            var targetPosition = target.position;
            float distance = Vector3.Distance(transform.position, targetPosition);

            Vector3 modifiedYPos = new(targetPosition.x, transform.position.y, targetPosition.z);

            while (distance > _distanceCheckThreshold)
            {
                cancelToken.ThrowIfCancellationRequested();

                transform.position = Vector3.Lerp(transform.position, modifiedYPos, lerpSpeed * Time.deltaTime);

                distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(modifiedYPos.x, modifiedYPos.z));

                await Task.Yield();
            }

            transform.position = modifiedYPos;
        }
        
        async Task RotateAsync(CancellationToken cancelToken)
        {
            if (_diceController.ObjectDirectionsEnum == ObjectDirections.up)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
                while (Quaternion.Angle(transform.rotation, targetRotation) > _rotationCheckThreshold)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    await Task.Delay(TimeSpan.FromSeconds(0.01f));
                }

                transform.up = Vector3.up;
            }
            else if (_diceController.ObjectDirectionsEnum == ObjectDirections.down)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(-transform.up, Vector3.down);
                while (Quaternion.Angle(transform.rotation, targetRotation) > _rotationCheckThreshold)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    await Task.Delay(TimeSpan.FromSeconds(0.01f));
                }

                transform.up = Vector3.down;
            }
            else if (_diceController.ObjectDirectionsEnum == ObjectDirections.right)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.right, Vector3.left);
                while (Quaternion.Angle(transform.rotation, targetRotation) > _rotationCheckThreshold)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    await Task.Delay(TimeSpan.FromSeconds(0.01f));
                }

                transform.right = Vector3.up;
            }
            else if (_diceController.ObjectDirectionsEnum == ObjectDirections.left)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(-transform.right, Vector3.right);
                while (Quaternion.Angle(transform.rotation, targetRotation) > _rotationCheckThreshold)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    await Task.Delay(TimeSpan.FromSeconds(0.01f));
                }

                transform.right = Vector3.down;
            }
            else if (_diceController.ObjectDirectionsEnum == ObjectDirections.forward)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, Vector3.back);
                while (Quaternion.Angle(transform.rotation, targetRotation) > _rotationCheckThreshold)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    await Task.Delay(TimeSpan.FromSeconds(0.01f));
                }

                transform.forward = Vector3.up;
            }
            else if (_diceController.ObjectDirectionsEnum == ObjectDirections.backward)
            {
                Quaternion targetRotation = Quaternion.FromToRotation(-transform.forward, Vector3.forward);
                while (Quaternion.Angle(transform.rotation, targetRotation) > _rotationCheckThreshold)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    await Task.Delay(TimeSpan.FromSeconds(0.01f));
                }

                transform.forward = Vector3.down;
            }

            _hasRotated = true;

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

        public void LaunchDice()
        {
            LaunchDice(_cachedDiceForce, _cachedDiceTorque);
        }

    }
}