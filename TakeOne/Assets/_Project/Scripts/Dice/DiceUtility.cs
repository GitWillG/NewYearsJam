using System;
using System.Collections.Generic;

namespace DiceGame.Dice
{
    public static class DiceUtility
    {
        //Look at the dice results from a given dice slot without using the dice.
        public static List<int> PeekDiceResults(ref List<DiceController> diceControllers)
        {
            
            return GetDiceResults(ref diceControllers,false);
        }
        
        public static List<int> GetDiceResults(ref List<DiceController> diceControllers, bool useDice = true)
        {
            if (diceControllers == null) throw new NullReferenceException("The collection of dice passed is null");

            var returnList = new List<int>();
            
            foreach (var diceController in diceControllers)
            {
                returnList.Add(diceController.FaceValue);
            }
            
            for (var i = 0; i < diceControllers.Count; i++)
            {
                var diceFace = diceControllers[i];
                if (useDice)
                {
                    diceFace.UseDice();
                }
            }

            if (!useDice) return returnList;
            
            diceControllers = new List<DiceController>();

            return returnList;
        }
    }
}