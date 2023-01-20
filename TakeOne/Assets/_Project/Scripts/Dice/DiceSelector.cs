using UnityEngine;

namespace DiceGame.Dice
{
    public class DiceSelector : MonoBehaviour
    {
        [SerializeField] private Camera diceCam;
        public DiceController HoveredDie { get; set; }
        public DiceController SelectedDie { get; set; }

        public bool ShouldRaycast
        {
            set => _shouldRaycast = value;
        }

        private UIManager _uiManager;
        private bool _shouldRaycast;

        private void Awake()
        {
            _uiManager = FindObjectOfType<UIManager>();
        }
        
        private void Update()
        {
            if(!_shouldRaycast) return;
            
            DiceSelection();
        }

        //several checks to see if dice has the proper flags set
        private void DiceSelection()
        {
            var ray = diceCam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            if (HoveredDie != null)
            {
                HoveredDie.HoverOnDice(false);
            }

            if (!hit.collider.gameObject.CompareTag("Dice")) return;

            HoveredDie = hit.collider.gameObject.GetComponent<DiceController>();
                
            if (!HoveredDie.IsInTray)
            {
                HoveredDie.HoverOnDice(true);
            }
                
            if (!Input.GetMouseButtonUp(0) || HoveredDie.IsInTray || !HoveredDie.IsResultFound) return;
                
            SelectAndHighlightDice(hit);
        }

        //Selection logic
        private void SelectAndHighlightDice(RaycastHit hit)
        {
            if (SelectedDie != null)
            {
                SelectedDie.RemoveHighlight();
            }

            if (HoveredDie != SelectedDie)
            {
                SelectedDie = hit.collider.gameObject.GetComponent<DiceController>();
                SelectedDie.HighlightDice();
                _uiManager.ConfirmDice.SetActive(true);
            }
            else
            {
                SelectedDie.RemoveHighlight();
                SelectedDie = null;
                _uiManager.ConfirmDice.SetActive(false);
            }
        }
    }
}