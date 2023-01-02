using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{

    public class DiceManager : MonoBehaviour
    {
        public PlayableCharacter CharacterStats;
        public int DiceNo;
        public List<int> Results = new List<int>();
        // Start is called before the first frame update
        private void Start()
        {
            //Results = new List<int>();
            DiceNo = CharacterStats.Stat;

        }

        public List<int> RollDice()
        {
            Results.Clear();
            int tempNum;
            for (int i = 0; i < DiceNo; i++)
            {
                tempNum = Random.Range(1, 6);
                Results.Add(tempNum);
            }

            return Results;
        }
        public void TestDie()
        {
            RollDice();

        }
    }
}
