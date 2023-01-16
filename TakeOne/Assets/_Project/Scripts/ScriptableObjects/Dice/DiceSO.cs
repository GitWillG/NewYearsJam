using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    [CreateAssetMenu(fileName = "CreateDiceSO", menuName = "Assets/Create/CreateDiceSO", order = 1)]
    public class DiceSO : ScriptableObject
    {
        [SerializeField] private Sprite[] faceSprites = new Sprite[6];
        [SerializeField] private List<int> dieSides;

        public List<int> DieSides 
        { 
            get => dieSides; 
            set => dieSides = value; 
        }
        public Sprite[] FaceSprites 
        { 
            get => faceSprites; 
            set => faceSprites = value; 
        }
    }
}