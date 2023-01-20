using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.Dice
{
    public class DiceShaderHandler : MonoBehaviour
    {

        [SerializeField] private List<IntFaceTexture> faceSprites;

        private Dictionary<int, Texture2D> _intToTextureDictionary = new Dictionary<int, Texture2D>();
        private Material _diceMat;
        
        private static readonly int Isflashing = Shader.PropertyToID("_IsFlashing");
        private static readonly int StartTime = Shader.PropertyToID("_StartTime");
        private static readonly int IsHovering = Shader.PropertyToID("_IsHover");
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveTValue");
        
        private static readonly int TopTexture = Shader.PropertyToID("_TopTexture");
        private static readonly int BottomTexture = Shader.PropertyToID("_BottomTexture");
        private static readonly int RightTexture = Shader.PropertyToID("_RightTexture");
        private static readonly int LeftTexture = Shader.PropertyToID("_LeftTexture");
        private static readonly int ForwardTexture = Shader.PropertyToID("_ForwardTexture");
        private static readonly int BackTexture = Shader.PropertyToID("_BackTexture");
        
        public float dissolveVal;


        private void Awake()
        {
            _diceMat = gameObject.GetComponent<MeshRenderer>().material;

            foreach (var intFace in faceSprites)
            {
                _intToTextureDictionary.TryAdd(intFace.value, intFace.texture);
            }
        }

        private void Update()
        {
            _diceMat.SetFloat(DissolveAmount, dissolveVal);
        }
        
        public void UpdateDiceFaceTextures(List<int> faceValues)
        {
            SetSideTexture(TopTexture, _intToTextureDictionary[faceValues[0]]);
            SetSideTexture(RightTexture, _intToTextureDictionary[faceValues[1]]);
            SetSideTexture(BackTexture, _intToTextureDictionary[faceValues[2]]);
            SetSideTexture(ForwardTexture, _intToTextureDictionary[faceValues[3]]);
            SetSideTexture(LeftTexture, _intToTextureDictionary[faceValues[4]]);
            SetSideTexture(BottomTexture, _intToTextureDictionary[faceValues[5]]);
        }

        private void SetSideTexture(int shaderTextureProperty, Texture2D texture)
        {
            _diceMat.SetTexture(shaderTextureProperty, texture);
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

[System.Serializable]
public struct IntFaceTexture
{
    public int value;
    public Texture2D texture;
}