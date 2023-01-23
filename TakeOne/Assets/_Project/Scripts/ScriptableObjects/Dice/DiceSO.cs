using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame.ScriptableObjects.Dice
{
    [CreateAssetMenu(fileName = "CreateDiceSO", menuName = "Assets/Create/CreateDiceSO", order = 1)]
    public class DiceSO : ScriptableObject
    {
        [SerializeField] private List<int> dieSides;
        [SerializeField] private GameObject dicePrefab;

        public List<int> DieSides
        {
            get
            {
                if (dieSides.Count == 6) return dieSides;
                
                var returnList = new int[6];
                
                for (int i = 0; i <= 5; i++)
                {
                    returnList[i] = dieSides[i % dieSides.Count];
                }
                
                return returnList.ToList();
            }
        }

        public GameObject DicePrefab => dicePrefab;
    }
}
