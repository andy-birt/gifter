using Microsoft.Extensions.Configuration;
using Gifter.Models;
using Gifter.Utils;

namespace Gifter.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration configuration) : base(configuration) { }

        public void Add(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Comment ([Message], [UserProfileId], [PostId])
                                        OUTPUT INSERTED.ID
                                        VALUES (@message, @userProfileId, @postId)";

                    DbUtils.AddParameter(cmd, "@message", comment.Message);
                    DbUtils.AddParameter(cmd, "@userProfileId", comment.UserProfileId);
                    DbUtils.AddParameter(cmd, "@postId", comment.PostId);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
