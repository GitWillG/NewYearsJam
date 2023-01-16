using TMPro;
using UnityEngine;

namespace DiceGame
{
    public class TextExposer : UnityObjectExposer<TextMeshProUGUI>
    {
        public void UpdateText(string newText)
        {
            Field.text = newText;
        }
    }

    public class UnityObjectExposer<T> : MonoBehaviour
    {
        [SerializeField] private T field;
        public T Field => field;
    }
}
