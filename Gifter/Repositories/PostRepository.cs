using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Gifter.Models;
using Gifter.Utils;

namespace Gifter.Repositories
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(IConfiguration configuration) : base(configuration) { }

        public List<Post> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SelectPostUserStatement()
                                    + FromPostJoinUser()
                                    + OrderByPostDateCreated();

                    //             "SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
                    //                     p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

                    //                     up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                    //                     up.ImageUrl AS UserProfileImageUrl
                    //                FROM Post p 
                    //                     LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                    //              ORDER BY p.DateCreated"

                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        posts.Add( NewPostFromReader( reader ) );
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public List<Post> GetAllWithComments()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SelectPostUserStatement()
                                    + WithComments()
                                    + WithCommentsUser()
                                    + WithLikes()
                                    + FromPostJoinUser()
                                    + JoinComments()
                                    + JoinCommentingUsers()
                                    + JoinLikes()
                                    + GroupByPostProperties()
                                    + OrderByAmountOfLikes();

                    //    "SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated,
                    //            p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

                    //            up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated,
                    //            up.ImageUrl AS UserProfileImageUrl,

                    //            c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId,
                    //            cu.Name AS CommentsUserName, cu.Bio AS CommentsUserBio, cu.Email AS CommentsUserEmail,
                    //            cu.DateCreated AS CommentsUserDateCreated, cu.ImageUrl AS CommentsUserImageUrl,
                    //            COUNT(l.PostId) AS 'Likes'
                    //     FROM Post p
                    //          LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                    //          LEFT JOIN Comment c on c.PostId = p.id
                    //          LEFT JOIN UserProfile cu On c.UserProfileId = cu.id
                    //          LEFT JOIN[Like] l on l.PostId = p.Id
                    //     GROUP BY p.Id, p.Title, p.Caption, p.DateCreated, p.ImageUrl, p.UserProfileId,
                    //              up.Name, up.Bio, up.Email, up.DateCreated, up.ImageUrl, 
                    //              c.Id, c.Message, c.UserProfileId, 
                    //              cu.Name, cu.Bio, cu.Email, cu.DateCreated, cu.ImageUrl,
                    //              l.PostId
                    //     ORDER BY COUNT(l.PostId)"

                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        var postId = DbUtils.GetInt(reader, "PostId");

                        var existingPost = posts.FirstOrDefault(p => p.Id == postId);
                        if (existingPost == null)
                        {
                            existingPost = NewPostFromReader(postId, reader);

                            posts.Add(existingPost);
                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            existingPost.Comments.Add( NewCommentFromReader(postId, reader) );
                        }
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public Post GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SelectPostUserStatement()
                                    + FromPostJoinUser()
                                    + "WHERE p.Id = @Id";
                        
                        // "SELECT p.Title, p.Caption, p.DateCreated AS PostDateCreated, p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,
                        //         up.Name, up.Email, up.DateCreated AS UserProfileDateCreated, up.ImageUrl AS UserProfileImageUrl
                        //    FROM Post p
                        //    LEFT JOIN UserProfile up ON up.Id = p.UserProfileId
                        //   WHERE p.Id = @Id"

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    Post post = null;
                    if (reader.Read())
                    {
                        post = NewPostFromReader(id, reader);
                    }

                    reader.Close();

                    return post;
                }
            }
        }

        public Post GetByIdWithComments(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SelectPostUserStatement()
                                    + WithComments()
                                    + WithCommentsUser()
                                    + WithLikes()
                                    + FromPostJoinUser()
                                    + JoinComments()
                                    + JoinCommentingUsers()
                                    + JoinLikes()
                                    + @"WHERE p.Id = @Id
                                       "
                                    + GroupByPostProperties()
                                    + OrderByAmountOfLikes();
                    // "SELECT p.Title, p.Caption, p.DateCreated AS PostDateCreated, p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,
                    //         up.Name, up.Email, up.DateCreated AS UserProfileDateCreated, up.ImageUrl AS UserProfileImageUrl,
                    //         c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                    //    FROM Post p
                    //    LEFT JOIN UserProfile up ON up.Id = p.UserProfileId
                    //    LEFT JOIN Comment c ON c.PostId = p.Id
                    //   WHERE p.Id = @Id"

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    Post post = null;
                    while (reader.Read())
                    {
                        if (post == null)
                        {
                            post = NewPostFromReader(id, reader);
                        }

                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            post.Comments.Add( NewCommentFromReader(id, reader) );
                        }
                    }

                    reader.Close();

                    return post;
                }
            }
        }

        public List<Post> Search(string criterion, bool sortDescending)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var sql = SelectPostUserStatement()
                            + WithLikes()
                            + FromPostJoinUser()
                            + JoinLikes()
                            + @"WHERE p.Title LIKE @Criterion OR p.Caption LIKE @Criterion
                                GROUP BY p.Id, p.Title, p.Caption, p.DateCreated, p.ImageUrl, p.UserProfileId,
                                         up.Name, up.Bio, up.Email, up.DateCreated, up.ImageUrl,
                                         l.PostId";

                    // "SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
                    //         p.ImageUrl AS PostImageUrl, p.UserProfileId,

                    //         up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                    //         up.ImageUrl AS UserProfileImageUrl,
                    //         COUNT(l.PostId) AS 'Likes'
                    //  FROM Post p 
                    //       LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                    //       LEFT JOIN [Like] l ON l.PostId = p.Id
                    //  GROUP BY p.Id, p.Title, p.Caption, p.DateCreated, p.ImageUrl, p.UserProfileId,
                    //           up.Name, up.Bio, up.Email, up.DateCreated, up.ImageUrl,
                    //           l.PostId"
                    //  WHERE p.Title LIKE @Criterion OR p.Caption LIKE @Criterion";

                    if (sortDescending)
                    {
                        sql += $" {OrderByPostDateCreated()} DESC";
                    }
                    else
                    {
                        sql += $" {OrderByPostDateCreated()}";
                    }

                    cmd.CommandText = sql;
                    DbUtils.AddParameter(cmd, "@Criterion", $"%{criterion}%");
                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        posts.Add( NewPostFromReader( reader ) );
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public List<Post> Hottest(DateTime dateString)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var sql = SelectPostUserStatement()
                            + FromPostJoinUser()
                            + "WHERE p.DateCreated >= @DateString\n"
                            + OrderByPostDateCreated();

                    // "SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
                    //         p.ImageUrl AS PostImageUrl, p.UserProfileId,

                    //         up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                    //         up.ImageUrl AS UserProfileImageUrl
                    //  FROM Post p 
                    //         LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                    //  WHERE p.DateCreated >= @DateString
                    //  ORDER BY p.DateCreated DESC";

                    cmd.CommandText = sql;
                    DbUtils.AddParameter(cmd, "@DateString", dateString);
                    var reader = cmd.ExecuteReader();

                    var posts = new List<Post>();
                    while (reader.Read())
                    {
                        posts.Add( NewPostFromReader( reader ) );
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public void Add(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Post (Title, Caption, DateCreated, ImageUrl, UserProfileId)
                        OUTPUT INSERTED.ID
                        VALUES (@Title, @Caption, @DateCreated, @ImageUrl, @UserProfileId)";

                    DbUtils.AddParameter(cmd, "@Title", post.Title);
                    DbUtils.AddParameter(cmd, "@Caption", post.Caption);
                    DbUtils.AddParameter(cmd, "@DateCreated", post.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", post.ImageUrl);
                    DbUtils.AddParameter(cmd, "@UserProfileId", post.UserProfileId);

                    post.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Post post)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Post
                           SET Title = @Title,
                               Caption = @Caption,
                               DateCreated = @DateCreated,
                               ImageUrl = @ImageUrl,
                               UserProfileId = @UserProfileId
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Title", post.Title);
                    DbUtils.AddParameter(cmd, "@Caption", post.Caption);
                    DbUtils.AddParameter(cmd, "@DateCreated", post.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", post.ImageUrl);
                    DbUtils.AddParameter(cmd, "@UserProfileId", post.UserProfileId);
                    DbUtils.AddParameter(cmd, "@Id", post.Id);

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
                    cmd.CommandText = @"DELETE FROM Comment WHERE PostId = @Id
                                        DELETE FROM Post WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /******
         *
         *  Object Instance Helpers
         *
         ******/

        /// <summary>
        ///  Create a new post using the reader.
        /// </summary>
        /// <param name="id">The Id that references the Post.</param>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <returns>A new Post object along with the User that created it.</returns>
        private Post NewPostFromReader(int id, SqlDataReader reader)
        {
            return new Post()
            {
                Id = id,
                Title = DbUtils.GetString(reader, "Title"),
                Caption = DbUtils.GetString(reader, "Caption"),
                DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
                Likes = DbUtils.GetInt(reader, "Likes"),
                UserProfile = new UserProfile()
                {
                    Id = DbUtils.GetInt(reader, "PostUserProfileId"),
                    Name = DbUtils.GetString(reader, "Name"),
                    Email = DbUtils.GetString(reader, "Email"),
                    DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                    ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                },
                Comments = new List<Comment>()
            };
        }

        /// <summary>
        ///  Create a new post using the reader.
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <returns>A new Post object along with the User that created it.</returns>
        private Post NewPostFromReader(SqlDataReader reader)
        {
            return new Post()
            {
                Id = DbUtils.GetInt(reader, "PostId"),
                Title = DbUtils.GetString(reader, "Title"),
                Caption = DbUtils.GetString(reader, "Caption"),
                DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
                Likes = DbUtils.GetInt(reader, "Likes"),
                UserProfile = new UserProfile()
                {
                    Id = DbUtils.GetInt(reader, "PostUserProfileId"),
                    Name = DbUtils.GetString(reader, "Name"),
                    Email = DbUtils.GetString(reader, "Email"),
                    DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                    ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                },
                Comments = new List<Comment>()
            };
        }

        /// <summary>
        ///  Create a new comment using the reader.
        /// </summary>
        /// <param name="postId">The PostId that references the Post the comment belongs to.</param>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set.</param>
        /// <returns>A new Comment object.</returns>
        private Comment NewCommentFromReader(int postId, SqlDataReader reader)
        {
            return new Comment()
            {
                Id = DbUtils.GetInt(reader, "CommentId"),
                Message = DbUtils.GetString(reader, "Message"),
                PostId = postId,
                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId"),
                UserProfile = new UserProfile()
                {
                    Id = DbUtils.GetInt(reader, "CommentUserProfileId"),
                    Name = DbUtils.GetString(reader, "CommentsUserName"),
                    Email = DbUtils.GetString(reader, "CommentsUserEmail"),
                    DateCreated = DbUtils.GetDateTime(reader, "CommentsUserDateCreated"),
                    ImageUrl = DbUtils.GetString(reader, "CommentsUserImageUrl"),
                }
            };
        }

        /******
         *
         *  SQL Command String Helpers
         *
         ******/

        /// <summary>
        ///  A Select statement which pulls the necessary Post columns and User columns.
        /// </summary>
        /// <value>
        ///     SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
        ///            p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,
        ///            up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
        ///            up.ImageUrl AS UserProfileImageUrl
        /// </value>
        /// <returns>A partial SQL command string.</returns>
        private string SelectPostUserStatement()
        {
            return @"SELECT p.Id AS PostId, p.Title, p.Caption, p.DateCreated AS PostDateCreated, 
                            p.ImageUrl AS PostImageUrl, p.UserProfileId AS PostUserProfileId,

                            up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                            up.ImageUrl AS UserProfileImageUrl
                            ";
        }

        /// <summary>
        ///  An addition to the Post/User Select statement which pulls the necessary Comment columns.
        ///  Use this in addition to SelectPostUserStatement().
        /// </summary>
        /// <value>
        ///     c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
        /// </value>
        /// <returns>An additional partial SQL command string to use with SelectPostUserStatement().</returns>
        private string WithComments()
        {
            return @",

                    c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                    ";
        }

        /// <summary>
        ///  An addition to the Post/User Select statement which pulls the necessary UserProfile related to Comment columns.
        ///  Use this in addition to SelectPostUserStatement().
        /// </summary>
        /// <value>
        ///     cu.Name AS CommentsUserName, cu.Bio AS CommentsUserBio, cu.Email AS CommentsUserEmail, cu.DateCreated AS CommentsUserDateCreated, cu.ImageUrl AS CommentsUserImageUrl
        /// </value>
        /// <returns>An additional partial SQL command string to use with SelectPostUserStatement().</returns>
        private string WithCommentsUser()
        {
            return @",

                    cu.Name AS CommentsUserName, cu.Bio AS CommentsUserBio, cu.Email AS CommentsUserEmail, cu.DateCreated AS CommentsUserDateCreated, cu.ImageUrl AS CommentsUserImageUrl
                    ";
        }

        /// <summary>
        ///  An addition to the Post/User Select statement which pulls the column with the amount of likes for a Post.
        ///  Use this in addition to SelectPostUserStatement().
        /// </summary>
        /// <value>
        ///     COUNT(l.PostId) AS 'Likes'
        /// </value>
        /// <returns>An additional partial SQL command string to use with SelectPostUserStatement().</returns>
        private string WithLikes()
        {
            return @", 
                     COUNT(l.PostId) AS 'Likes'
                    ";
        }

        /// <summary>
        ///  The FROM sequence of the SQL SELECT statement. It can complete a SQL statement when used with SelectPostUserStatement().
        /// </summary>
        /// <value>
        ///     FROM Post p
        ///          LEFT JOIN UserProfile up ON p.UserProfileId = up.id
        /// </value>
        /// <returns>A partial SQL command string.</returns>
        private string FromPostJoinUser()
        {
            return @"FROM Post p
                          LEFT JOIN UserProfile up ON p.UserProfileId = up.id
                          ";
        }

        /// <summary>
        ///  A LEFT JOIN Comment statement for joining onto an existing command string
        /// </summary>
        /// <value>
        ///   LEFT JOIN Comment c on c.PostId = p.id
        /// </value>
        /// <returns>A partial SQL command string.</returns>
        private string JoinComments()
        {
            return @"LEFT JOIN Comment c on c.PostId = p.id
                    ";
        }

        /// <summary>
        ///  A LEFT JOIN UserProfile statement for joining onto an existing command string. This one is for getting the user who posted the comment
        /// </summary>
        /// <value>
        ///   LEFT JOIN Comment c on c.PostId = p.id
        /// </value>
        /// <returns>A partial SQL command string.</returns>
        private string JoinCommentingUsers()
        {
            return @"LEFT JOIN UserProfile cu On c.UserProfileId = cu.id
                    ";
        }

        private string JoinLikes()
        {
            return @"LEFT JOIN[Like] l on l.PostId = p.Id
                    ";
        }

        /// <summary>
        ///  A GROUP BY statement to use when getting Post with User, Comments, and Likes count
        /// </summary>
        /// <value>
        ///   GROUP BY p.Id, p.Title, p.Caption, p.DateCreated, p.ImageUrl, p.UserProfileId,
        ///            up.Name, up.Bio, up.Email, up.DateCreated, up.ImageUrl, 
        ///            c.Id, c.Message, c.UserProfileId, 
        ///            cu.Name, cu.Bio, cu.Email, cu.DateCreated, cu.ImageUrl,
        ///            l.PostId
        /// </value>
        /// <returns>A partial SQL command string.</returns>
        private string GroupByPostProperties()
        {
            return @"GROUP BY p.Id, p.Title, p.Caption, p.DateCreated, p.ImageUrl, p.UserProfileId,
                              up.Name, up.Bio, up.Email, up.DateCreated, up.ImageUrl, 
                              c.Id, c.Message, c.UserProfileId, 
                              cu.Name, cu.Bio, cu.Email, cu.DateCreated, cu.ImageUrl,
                              l.PostId
                    ";
        }

        /// <summary>
        ///  An ORDER BY statement which will order by a Post's created date
        /// </summary>
        /// <value>
        ///   ORDER BY p.DateCreated
        /// </value>
        /// <returns>A partial SQL command string.</returns>
        private string OrderByPostDateCreated()
        {
            return @"ORDER BY p.DateCreated";
        }

        /// <summary>
        ///  An ORDER BY statement which will order by a Post's 'like' count
        /// </summary>
        /// <value>
        ///   ORDER BY COUNT(l.PostId) DESC
        /// </value>
        /// <returns>A partial SQL command string.</returns>
        private string OrderByAmountOfLikes()
        {
            return "ORDER BY COUNT(l.PostId) DESC";
        }
    }
}
