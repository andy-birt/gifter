using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Gifter.Models;
using Gifter.Utils;

namespace Gifter.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT [Id], [Name], [Email], [ImageUrl], [Bio], [DateCreated]
                          FROM UserProfile
                      ORDER BY DateCreated";

                    var reader = cmd.ExecuteReader();

                    var users = new List<UserProfile>();
                    while (reader.Read())
                    {
                        users.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                        });
                    }

                    reader.Close();

                    return users;
                }
            }
        }

        //public List<UserProfile> GetAllWithComments()
        //{
        //    using (var conn = Connection)
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated,
        //                       p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

        //                       up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated,
        //                       up.ImageUrl AS UserProfileImageUrl,

        //                       c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
        //                  FROM Post p
        //                       LEFT JOIN UserProfile up ON p.UserProfileId = up.id
        //                       LEFT JOIN Comment c on c.PostId = p.id
        //              ORDER BY p.DateCreated";

        //            var reader = cmd.ExecuteReader();

        //            var posts = new List<Post>();
        //            while (reader.Read())
        //            {
        //                var postId = DbUtils.GetInt(reader, "PostId");

        //                var existingPost = posts.FirstOrDefault(p => p.Id == postId);
        //                if (existingPost == null)
        //                {
        //                    existingPost = new Post()
        //                    {
        //                        Id = postId,
        //                        Title = DbUtils.GetString(reader, "Title"),
        //                        Caption = DbUtils.GetString(reader, "Caption"),
        //                        DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
        //                        ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
        //                        UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
        //                        UserProfile = new UserProfile()
        //                        {
        //                            Id = DbUtils.GetInt(reader, "PostUserProfileId"),
        //                            Name = DbUtils.GetString(reader, "Name"),
        //                            Email = DbUtils.GetString(reader, "Email"),
        //                            DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
        //                            ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
        //                        },
        //                        Comments = new List<Comment>()
        //                    };

        //                    posts.Add(existingPost);
        //                }

        //                if (DbUtils.IsNotDbNull(reader, "CommentId"))
        //                {
        //                    existingPost.Comments.Add(new Comment()
        //                    {
        //                        Id = DbUtils.GetInt(reader, "CommentId"),
        //                        Message = DbUtils.GetString(reader, "Message"),
        //                        PostId = postId,
        //                        UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
        //                    });
        //                }
        //            }

        //            reader.Close();

        //            return posts;
        //        }
        //    }
        //}

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT [Id], [Name], [Email], [ImageUrl], [Bio], [DateCreated]
                          FROM UserProfile 
                          WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile user = null;
                    if (reader.Read())
                    {
                        user = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                        };
                    }

                    reader.Close();

                    return user;
                }
            }
        }

        //public Post GetByIdWithComments(int id)
        //{
        //    using (var conn = Connection)
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                  SELECT p.Title, p.Caption, p.DateCreated AS PostDateCreated, p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,
        //                         up.Name, up.Email, up.DateCreated AS UserProfileDateCreated, up.ImageUrl AS UserProfileImageUrl,
        //                         c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
        //                    FROM Post p
        //                    LEFT JOIN UserProfile up ON up.Id = p.UserProfileId
        //                    LEFT JOIN Comment c ON c.PostId = p.Id
        //                   WHERE p.Id = @Id";

        //            DbUtils.AddParameter(cmd, "@Id", id);

        //            var reader = cmd.ExecuteReader();

        //            Post post = null;
        //            while (reader.Read())
        //            {
        //                if (post == null)
        //                {
        //                    post = new Post()
        //                    {
        //                        Id = id,
        //                        Title = DbUtils.GetString(reader, "Title"),
        //                        Caption = DbUtils.GetString(reader, "Caption"),
        //                        DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
        //                        ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
        //                        UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
        //                        UserProfile = new UserProfile()
        //                        {
        //                            Id = DbUtils.GetInt(reader, "PostUserProfileId"),
        //                            Name = DbUtils.GetString(reader, "Name"),
        //                            Email = DbUtils.GetString(reader, "Email"),
        //                            DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
        //                            ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl")
        //                        },
        //                        Comments = new List<Comment>()
        //                    };
        //                }

        //                if (DbUtils.IsNotDbNull(reader, "CommentId"))
        //                {
        //                    post.Comments.Add(new Comment()
        //                    {
        //                        Id = DbUtils.GetInt(reader, "CommentId"),
        //                        Message = DbUtils.GetString(reader, "Message"),
        //                        PostId = id,
        //                        UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
        //                    });
        //                }
        //            }

        //            reader.Close();

        //            return post;
        //        }
        //    }
        //}

        public void Add(UserProfile user)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile ([Name], [Email], [ImageUrl], [Bio], [DateCreated])
                        OUTPUT INSERTED.ID
                        VALUES (@Name, @Email, @ImageUrl, @Bio, @DateCreated)";

                    DbUtils.AddParameter(cmd, "@Name", user.Name);
                    DbUtils.AddParameter(cmd, "@Email", user.Email);
                    DbUtils.AddParameter(cmd, "@Bio", user.Bio);
                    DbUtils.AddParameter(cmd, "@ImageUrl", user.ImageUrl);
                    DbUtils.AddParameter(cmd, "@DateCreated", user.DateCreated);

                    user.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile user)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                           SET Name = @Name,
                               Email = @Email,
                               Bio = @Bio,
                               ImageUrl = @ImageUrl,
                               DateCreated = @DateCreated
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Name", user.Name);
                    DbUtils.AddParameter(cmd, "@Email", user.Email);
                    DbUtils.AddParameter(cmd, "@Bio", user.Bio);
                    DbUtils.AddParameter(cmd, "@ImageUrl", user.ImageUrl);
                    DbUtils.AddParameter(cmd, "@DateCreated", user.DateCreated);
                    DbUtils.AddParameter(cmd, "@Id", user.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

