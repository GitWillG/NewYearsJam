using DiceGame.ScriptableObjects;

namespace DiceGame.Utility
{
    /// <summary>
    /// Implement this onto an element that you want to register to a unique pool.
    /// Create a backing field for CollectionReference property of the type you want for the ScriptableObject collection
    /// For Implementation look at: <see cref="DiceGame.Dice.DiceSlotHolder"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionElement<T>
    {
        public CollectionExposerSO<T> CollectionReference { get;}

        public void Register()
        {
            CollectionReference.RegisterElement((T)this);
        }

        public void UnRegister()
        {
            CollectionReference.UnRegisterElement((T)this);
        }
    }
}