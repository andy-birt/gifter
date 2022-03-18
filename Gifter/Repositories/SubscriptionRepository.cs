using Gifter.Utils;
using Microsoft.Extensions.Configuration;

namespace Gifter.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IConfiguration configuration) : base(configuration) { }

        public void Add(int subscriberId, int providerId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [Subscription] ([SubscriberId], [ProviderId])
                                        VALUES (@subscriberId, @providerId)";

                    DbUtils.AddParameter(cmd, "@subscriberId", subscriberId);
                    DbUtils.AddParameter(cmd, "@providerId", providerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int subscriberId, int providerId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM [Subscription]
                                        WHERE SubscriberId = @subscriberId AND ProviderId = @providerId";

                    DbUtils.AddParameter(cmd, "@subscriberId", subscriberId);
                    DbUtils.AddParameter(cmd, "@providerId", providerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
