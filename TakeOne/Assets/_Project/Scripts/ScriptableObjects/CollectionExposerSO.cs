using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.ScriptableObjects
{
    /// <summary>
    /// Inherit from this script to create a scriptable object that will hold a collection of the given type T element.
    /// Then on that element, inherit from ICollectionElement and implement the functionality referencing this SO.
    /// For implementation look at: <see cref="DiceGame.ScriptableObjects.Dice.DiceSlotHolderCollection"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionExposerSO<T> : ScriptableObject
    {
        private HashSet<T> _collectionHashset = new HashSet<T>();

        public HashSet<T> CollectionHashset => _collectionHashset;

        public void RegisterElement(T element)
        {
            if (!_collectionHashset.Contains(element))
            {
                _collectionHashset.Add(element);
            }
        }

        public void UnRegisterElement(T element)
        {
            if(!_collectionHashset.Contains(element)) return;
            _collectionHashset.Remove(element);
        }
    }
}