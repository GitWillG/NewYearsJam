using UnityEngine.Events;

namespace DiceGame.Utility
{
    public interface INotificationReceiver
    {
        public UnityEvent OnNotify { get; }
        public void Notify()
        {
            OnNotify.Invoke();
        }
    }
}