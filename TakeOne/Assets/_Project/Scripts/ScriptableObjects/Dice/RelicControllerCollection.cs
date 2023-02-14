using DiceGame.Relics;
using UnityEngine;

namespace DiceGame.ScriptableObjects.Dice
{
    [CreateAssetMenu(order = 0, fileName = "New Relic Container", menuName = "Create Relic Container")]
    public class RelicControllerCollection : CollectionExposerSO<RelicController> { }
}