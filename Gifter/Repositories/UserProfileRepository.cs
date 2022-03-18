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

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT Id, Name, Email, ImageUrl, Bio, DateCreated FROM UserProfile WHERE Email = @email";
                    DbUtils.AddParameter(cmd, "@email", email);



                    var reader = cmd.ExecuteReader();

                    UserProfile user = null;
                    if (reader.Read())
                    {
                        user = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "iD"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                        };
                    }

                    reader.Close();

                    return user;
                }
            }
        }

        public UserProfile GetByIdWithPosts(int id) // And Comments with each post
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // So I know you're probably wondering why I'm left joining userprofile when
                    // I am already using userprofile in the initial select from... well the answer is this...
                    // I want to have user data associated with the comment
                    // Pretty cool, huh?
                    cmd.CommandText = @"
                          SELECT up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, up.ImageUrl AS UserProfileImageUrl,
                                 p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, p.ImageUrl AS PostImageUrl,
                                 c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId,
                                 cup.Name AS CommentersName, cup.Email AS CommentersEmail, cup.ImageUrl AS CommentersImageUrl,
                                 COUNT(l.PostId) AS 'Likes'
                            FROM UserProfile up
                            LEFT JOIN Post p ON up.Id = p.UserProfileId
                            LEFT JOIN Comment c ON c.PostId = p.Id
                            LEFT JOIN UserProfile cup ON c.UserProfileId = cup.Id
                            LEFT JOIN [Like] l ON l.PostId = p.Id
                           WHERE up.Id = @Id
                           GROUP BY up.Name, up.Bio, up.Email, up.DateCreated,  up.ImageUrl,
                                 p.Id, p.Title, p.Caption, p.DateCreated, p.ImageUrl,
                                 c.Id, c.Message, c.UserProfileId,
                                 cup.Name, cup.Email, cup.ImageUrl,
                                 l.PostId
                            ORDER BY l.PostId";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile user = null;

                    // Create a null post to add before db starts reading
                    // This will actually prove to be very useful
                    Post postToAdd = null;

                    // At this point we are really iterating over comments between posts
                    // rather than posts between a user (as we were in the previous exercise)
                    // because there will be multiple comments on a single post
                    // If you know that the reader is going to read each result it gets back
                    // this is easier to understand and if you don't understand it
                    // run this query or a similar one in a sql file and see how many times
                    // you see the same post but each record outputs a different comment
                    // Read over it for a little bit and I'm sure it will 
                    while (reader.Read())
                    {
                        if (user == null)
                        {
                            user = new UserProfile()
                            {
                                Id = id,
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                                Bio = DbUtils.GetString(reader, "Bio"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                Posts = new List<Post>()
                            };
                        }

                        // While there is no null values for the post id were going
                        // to be rather checking each comment
                        // because each read is going to have
                        // a different comment but we can still
                        // have the same post id on multiple consecutive iterations
                        if (DbUtils.IsNotDbNull(reader, "PostId"))
                        {
                            // When the post id reading from the db of the comment is different than
                            // the current post object in the loop then we will create a new post,
                            // reassign postToAdd to the new post and insert the comments into it instead
                            // So I create a variable to store the id of the post
                            // the reader object is currently reading
                            // then we're going to cross reference that id with
                            // the post object's id (the one created in the loop) that's
                            // going to be referenced between iterations
                            int readingPostId = DbUtils.GetInt(reader, "PostId");

                            // On the first iteration of reader.Read() postToAdd will be null
                            
                            // So create a new post object and assign the reference postToAdd to it

                            // After iterating through comments with the same post id
                            // the second condition in the 'if' will be triggered
                            // forcing us to create another post object then reassigning
                            // postToAdd and we will add the next set of comments to it rather than
                            // creating a new post object each iteration of reader.Read()
                            // or putting all of the comments into one post, possibly
                            if (postToAdd == null || postToAdd.Id != readingPostId)
                            {
                                postToAdd = new Post()
                                {
                                    Id = DbUtils.GetInt(reader, "PostId"),
                                    Title = DbUtils.GetString(reader, "Title"),
                                    Caption = DbUtils.GetString(reader, "Caption"),
                                    DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                                    UserProfileId = id,
                                    Likes = DbUtils.GetInt(reader, "Likes"),
                                    UserProfile = new UserProfile()
                                    {
                                        Id = id,
                                        Name = DbUtils.GetString(reader, "Name"),
                                        Email = DbUtils.GetString(reader, "Email"),
                                        ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                                        Bio = DbUtils.GetString(reader, "Bio"),
                                        DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated")
                                    },
                                    Comments = new List<Comment>()
                                };

                                user.Posts.Add(postToAdd);
                            }

                            if (DbUtils.IsNotDbNull(reader, "CommentId"))
                            {
                                postToAdd.Comments.Add(new Comment()
                                {
                                    Id = DbUtils.GetInt(reader, "CommentId"),
                                    Message = DbUtils.GetString(reader, "Message"),
                                    PostId = DbUtils.GetInt(reader, "PostId"),
                                    UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId"),
                                    UserProfile = new UserProfile()
                                    {
                                        Id = DbUtils.GetInt(reader, "CommentUserProfileId"),
                                        Name = DbUtils.GetString(reader, "CommentersName"),
                                        Email = DbUtils.GetString(reader, "CommentersEmail"),
                                        ImageUrl = DbUtils.GetString(reader, "CommentersImageUrl")
                                    }
                                });
                            }
                        }
                    }

                    reader.Close();

                    return user;
                }
            }
        }

        public List<UserProfile> GetProviderUsersBySubscriberId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT pu.Id AS ProviderUserProfileId, pu.Name AS ProviderUserProfileName, pu.Bio AS ProviderUserProfileBio, pu.Email AS ProviderUserProfileEmail, pu.DateCreated AS ProviderUserProfileDateCreated, pu.ImageUrl AS ProviderUserProfileImageUrl
                            FROM UserProfile pu
                            LEFT JOIN Subscription s ON s.ProviderId = pu.Id
                            WHERE s.SubscriberId = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    List<UserProfile> users = new List<UserProfile>();
                    while (reader.Read())
                    {
                        users.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "ProviderUserProfileId"),
                            Name = DbUtils.GetString(reader, "ProviderUserProfileName"),
                            Email = DbUtils.GetString(reader, "ProviderUserProfileEmail"),
                            ImageUrl = DbUtils.GetString(reader, "ProviderUserProfileImageUrl"),
                            Bio = DbUtils.GetString(reader, "ProviderUserProfileBio"),
                            DateCreated = DbUtils.GetDateTime(reader, "ProviderUserProfileDateCreated")
                        });
                    }

                    reader.Close();

                    return users;
                }
            }
        }

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

