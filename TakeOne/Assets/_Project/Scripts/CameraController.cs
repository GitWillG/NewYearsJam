using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour
    {
        Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }
        void Update()
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Dice"))
                {
                    //TODO: hover selector
                    if (Input.GetMouseButtonUp(0))
                    {
                        DiceFace hoveredDie = hit.collider.gameObject.GetComponent<DiceFace>();
                        int diceVal = hoveredDie.FaceValue;

                        //TODO: Rahul - minor thing, but add a "Select" method in the CardGO, that way if ever the selection logic needs to do more you do it in the right place.
                        Debug.Log(diceVal);
                    }
                }
            
            }
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

      
    }
}
