using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    /// <summary>
    /// Access point for all other scripts to set and get values from the die.
    /// </summary>
    public class DiceController : MonoBehaviour
    {
        public UnityEvent OnLaunch, OnSetAnchor, OnSnapToAnchor, OnDetachFromSlot, OnDestroyDice, OnHover, OnUnHover;
        
        private DiceFace _diceFace;
        private DiceMovement _diceMovement;
        private DiceSlotHolder _currentSlotHolder;
        private DiceShaderHandler _diceShader;
        private bool _isHovering;
        private bool _previousHoverState;

        public bool IsInSlot => _currentSlotHolder != null;
        public bool IsResultFound => _diceFace.IsResultFound;
        public int FaceValue => _diceFace.FaceValue;
        public ObjectDirections ObjectDirectionsEnum => _diceFace.ObjectDirectionsEnum;
        
        public DiceSlotHolder CurrentSlotHolder
        {
            get => _currentSlotHolder;
            set => _currentSlotHolder = value;
        }

        public bool IsInTray { get; set; }

        private void Awake()
        {
            _diceFace = GetComponent<DiceFace>();
            _diceMovement = GetComponent<DiceMovement>();
            _diceShader = GetComponent<DiceShaderHandler>();
        }

        public void LaunchDice(Vector2 diceForce, Vector2 diceTorque)
        {
            _diceMovement.LaunchDice(diceForce, diceTorque);
            OnLaunch?.Invoke();
        }

        public void LaunchDice()
        {
            _diceMovement.LaunchDice();
        }

        public void DetachFromSlot()
        {
            if(!IsInSlot) return;
            
            _currentSlotHolder.RemoveFromDiceSlot(this);
            _currentSlotHolder = null;
            OnDetachFromSlot?.Invoke();
        }

        public void SetAnchor(Transform anchorTransform, bool snapToAnchor = false)
        {
            OnSetAnchor?.Invoke();
            _diceMovement.SetAnchor(anchorTransform,ArrivedAtAnchor, snapToAnchor );
        }

        private void ArrivedAtAnchor()
        {
            OnSnapToAnchor?.Invoke();
        }

        public void DestroyDice()
        {
            //Logic for cleaning up here and spawning shit as needed.
            OnDestroyDice?.Invoke();
            Destroy(gameObject);
        }

        public void HoverOnDice(bool to)
        {
            if (_previousHoverState == to) return;
            
            _isHovering = to;

            _diceShader.HoverOnDice(_isHovering);      
            
            _previousHoverState = _isHovering;
            
            if (_isHovering)
            {
                OnHover?.Invoke();
            }
            else
            {
                OnUnHover?.Invoke();
            }
        }

        public void HighlightDice()
        {
            _diceShader.HighlightDice();
        }

        public void RemoveHighlight()
        {
            _diceShader.RemoveHighlight();
        }
    }
}
