using System;
using UnityEngine;

namespace DiceGame.Dice
{
    public class DiceShaderHandler : MonoBehaviour
    {
        private Material _diceMat;
        
        private static readonly int Isflashing = Shader.PropertyToID("_IsFlashing");
        private static readonly int StartTime = Shader.PropertyToID("_StartTime");
        private static readonly int IsHovering = Shader.PropertyToID("_IsHover");
        
        private void Awake()
        {
            _diceMat = gameObject.GetComponent<MeshRenderer>().material;
        }

        public void HoverOnDice(bool to)
        {
            _diceMat.SetInt(IsHovering, to? 1 : 0);
        }
        public void HighlightDice()
        {
            _diceMat.SetInt(Isflashing, 1);
            _diceMat.SetFloat(StartTime, Time.time);
        }
        public void RemoveHighlight()
        {
            _diceMat.SetInt(Isflashing, 0);
        }
    }
}