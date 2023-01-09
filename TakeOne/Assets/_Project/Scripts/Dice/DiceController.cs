using UnityEngine;

namespace DiceGame.Dice
{
    /// <summary>
    /// Access point for all other scripts to set and get values from the die.
    /// </summary>
    public class DiceController : MonoBehaviour
    {
        private DiceFace _diceFace;
        private DiceMovement _diceMovement;
        private DiceSlot _currentSlot;
        private DiceShaderHandler _diceShader;
        public bool IsInSlot => _currentSlot != null;
        public bool isInTray;
        public bool IsResultFound => _diceFace.IsResultFound;
        public int FaceValue => _diceFace.FaceValue;
        
        public DiceSlot CurrentSlot
        {
            get => _currentSlot;
            set => _currentSlot = value;
        }

        private void Awake()
        {
            _diceFace = GetComponent<DiceFace>();
            _diceMovement = GetComponent<DiceMovement>();
            _diceShader = GetComponent<DiceShaderHandler>();
        }

        public void LaunchDice(Vector2 diceForce, Vector2 diceTorque)
        {
            _diceMovement.LaunchDice(diceForce, diceTorque);
        }

        public void LaunchDice()
        {
            _diceMovement.LaunchDice();
        }

        public void DetachFromSlot()
        {
            if(!IsInSlot) return;
            
            _currentSlot.RemoveFromDiceSlot(this);
            _currentSlot = null;
        }

        public void SetAnchor(Transform anchorTransform, bool snapToAnchor = false)
        {
            _diceMovement.SetAnchor(anchorTransform, snapToAnchor);
        }

        public void DestroyDice()
        {
            //Logic for cleaning up here and spawning shit as needed.
            Destroy(gameObject);
        }

        public void HoverOnDice(bool to)
        {
            _diceShader.HoverOnDice(to);
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
