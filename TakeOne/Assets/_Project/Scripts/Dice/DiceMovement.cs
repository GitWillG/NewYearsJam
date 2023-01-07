using System;
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

        private CancellationTokenSource _cancelSource = new CancellationTokenSource();
        public Transform _anchorTransform;
        private bool _hasRotated;
        private float _distanceCheckThreshold = 0.01f;
        private Vector2 _cachedDiceForce, _cachedDiceTorque;
        private Rigidbody _rigidbody;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
                //
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
            if (_cachedDiceForce == null && _cachedDiceTorque == null) return;
            
            LaunchDice(_cachedDiceForce, _cachedDiceTorque);

        }

    }
}