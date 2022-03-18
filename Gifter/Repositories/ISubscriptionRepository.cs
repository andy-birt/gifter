namespace Gifter.Repositories
{
    public interface ISubscriptionRepository
    {
        void Add(int subscriberId, int providerId);
        void Delete(int id);
    }
}