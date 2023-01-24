using UnityEngine;

namespace DiceGame
{
    public class CameraGetter : MonoBehaviour
    {
        [SerializeField] private string cameraTag;
        private Camera _camera;
        private Canvas _canvas;
        
        private void Awake()
        {
            
            foreach (var cam in FindObjectsOfType<Camera>())
            {
                if (!cam.CompareTag(cameraTag)) continue;
                _camera = cam;
                break;
            }

            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = _camera;
            _canvas.planeDistance = 14.5f;
        }
    }
}
