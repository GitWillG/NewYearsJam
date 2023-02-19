using DiceGame.Utility;
using UnityEngine;

namespace DiceGame.ScriptableObjects.Dice
{
    [CreateAssetMenu(order = 0, fileName = "New AllGameEventListener Container", menuName = "Create AllGameEventListener Container")]
    public class AllGameEventListenerCollection : CollectionExposerSO<IAllGameEventListener> { }
}