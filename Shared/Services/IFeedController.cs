namespace Shared.Services
{
    public interface IFeedController
    {
        void SubscribeOnLineAdded(Action<string> onLineAdded);
        void SubscribeOnCleared(Action onCleared);
        void AddLine(string line);
        void Clear();
    }
}
