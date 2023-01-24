using UnityEngine;

namespace DiceGame.Utility
{
    public class UISnapWithOffset : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private string cameraTag;

        private Camera _diceCamera;

        private RectTransform _rectTransform;
        private Transform _targetTransform;

        
        public void SetTarget(Transform target)
        {
            _targetTransform = target;
            SnapToTarget();
        }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            foreach (var cam in FindObjectsOfType<Camera>())
            {
                if (!cam.CompareTag(cameraTag)) continue;
                _diceCamera = cam;
                break;
            }
        }

        private void SnapToTarget()
        {
            _rectTransform.localPosition = _diceCamera.WorldToScreenPoint(_targetTransform.position + offset);
        }
    }
}
