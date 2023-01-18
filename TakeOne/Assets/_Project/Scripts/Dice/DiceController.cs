using DiceGame.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame.Dice
{
    /// <summary>
    /// Access point for all other scripts to set and get values from the die.
    /// Controls <see cref="DiceGame.Dice.DiceFace"/>,  <see cref="DiceGame.Dice.DiceMovement"/> and <see cref="DiceGame.Dice.DiceShaderHandler"/>.
    /// </summary>
    public class DiceController : MonoBehaviour
    {
        public UnityEvent onAwake, onLaunch, onSetAnchor, onSnapToAnchor, onDetachFromSlot, onDestroyDice, onHover, onUnHover, onUse;

        private HeroSO _diceOwner;
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

        public GameObject AttackEffectPrefab => _diceOwner.AttackEffectPrefab;
        
        public DiceSlotHolder CurrentSlotHolder
        { 
            set => _currentSlotHolder = value;
        }

        public bool IsInTray { get; set; }

        private void Awake()
        {
            _diceFace = GetComponent<DiceFace>();
            _diceMovement = GetComponent<DiceMovement>();
            _diceShader = GetComponent<DiceShaderHandler>();
            onAwake?.Invoke();
        }

        public void Initialize(HeroSO diceOwner)
        {
            _diceOwner = diceOwner;
        }

        public void LaunchDice(Vector2 diceForce, Vector2 diceTorque)
        {
            _diceMovement.LaunchDice(diceForce, diceTorque);
            onLaunch?.Invoke();
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
            onDetachFromSlot?.Invoke();
        }

        public void SetAnchor(Transform anchorTransform, bool snapToAnchor = false, bool alertTarget = false)
        {
            onSetAnchor?.Invoke();
            _diceMovement.SetAnchorAsync(anchorTransform,ArrivedAtAnchor, snapToAnchor , alertTarget);
        }

        private void ArrivedAtAnchor()
        {
            onSnapToAnchor?.Invoke();
        }

        public void DestroyDice()
        {
            onDestroyDice?.Invoke();
        }

        public void UseDice()
        {
            onUse?.Invoke();
        }

        public void HoverOnDice(bool to)
        {
            if (_previousHoverState == to) return;
            
            _isHovering = to;

            _diceShader.HoverOnDice(_isHovering);      
            
            _previousHoverState = _isHovering;
            
            if (_isHovering)
            {
                onHover?.Invoke();
            }
            else
            {
                onUnHover?.Invoke();
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
