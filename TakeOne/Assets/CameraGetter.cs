using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class CameraGetter : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
            GetComponent<Canvas>().planeDistance = 10;
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
