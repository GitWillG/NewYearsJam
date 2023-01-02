using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    [CreateAssetMenu(fileName = "CreateDice", menuName = "Assets/Create/CreateDice", order = 1)]
    public class DiceSO : ScriptableObject
    {

        [SerializeField] private Sprite[] faceSprites = new Sprite[6];
        [SerializeField] private List<int> dieSides;
        private int[] faceNumbers = new int[6];

        public int[] FaceNumbers { get => faceNumbers; set => faceNumbers = value; }
        public List<int> DieSides { get => dieSides; set => dieSides = value; }
        public Sprite[] FaceSprites { get => faceSprites; set => faceSprites = value; }
    }
}
