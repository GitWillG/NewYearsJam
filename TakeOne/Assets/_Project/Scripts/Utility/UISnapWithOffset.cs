using UnityEngine;

namespace DiceGame.Utility
{
    public class UISnapWithOffset : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Vector3 offset;
        private Camera _diceCamera;

        private RectTransform _rectTransform;
        
        public void SetTarget(Transform target)
        {
            targetTransform = target;
            SnapToTarget();
        }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            foreach (var cam in FindObjectsOfType<Camera>())
            {
                if (!cam.CompareTag("DiceCam")) continue;
                _diceCamera = cam;
                break;
            }
        }

        private void SnapToTarget()
        {
            _rectTransform.position = _diceCamera.WorldToScreenPoint(targetTransform.position + offset);
        }
    }
}
