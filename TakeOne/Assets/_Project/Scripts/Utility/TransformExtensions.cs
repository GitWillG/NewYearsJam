using UnityEngine;

namespace DiceGame.Utility
{
    public static class TransformExtensions
    {
        public static void RandomizeRotation(this Transform currentTransform)
        {
            var x = Random.Range(0f, 360f);
            var y = Random.Range(0f, 360f);
            var z = Random.Range(0f, 360f);

            currentTransform.rotation = Quaternion.Euler(x, y, z);
        }
    }
}