using Microsoft.Extensions.Configuration;
using Gifter.Utils;

namespace Gifter.Repositories
{
    public class LikeRepository : BaseRepository, ILikeRepository
    {
        public LikeRepository(IConfiguration configuration) : base(configuration) { }

        public void Add(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [Like] ([PostId])
                                        VALUES (@postId)";

                    DbUtils.AddParameter(cmd, "@postId", postId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
