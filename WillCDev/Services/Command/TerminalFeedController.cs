using Shared.Services;

namespace WillCDev.Services.Command
{
    public class TerminalFeedController : IFeedController
    {
        private event Action<string> OnLineAdded = delegate { };
        private event Action OnCleared = delegate { };

        public void SubscribeOnLineAdded(Action<string> onLineAdded)
        {
            OnLineAdded += onLineAdded;
        }
        public void SubscribeOnCleared(Action onCleared)
        {
            OnCleared += onCleared;
        }
        public void AddLine(string line)
        {
            OnLineAdded(line);
        }
        public void Clear()
        {
            OnCleared();
        }
    }
}
