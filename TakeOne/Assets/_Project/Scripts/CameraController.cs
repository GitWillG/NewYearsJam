using DiceGame.Dice;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGame
{
    public class CameraController : MonoBehaviour
    {
        private Camera _mainCam;

        private void Awake()
        {
            _mainCam = Camera.main;
        }
        void Update()
        {
            var ray = _mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (!hit.collider.gameObject.CompareTag("Dice")) return;
                
                //TODO: hover selector
                if (Input.GetMouseButtonUp(0))
                {
                    DiceFace hoveredDie = hit.collider.gameObject.GetComponent<DiceFace>();
                    hoveredDie.HighlightDice();
                }
            }
        }
    }
}
