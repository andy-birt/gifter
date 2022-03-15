import React, { useContext, useEffect } from "react";
import { PostContext } from "../../providers/PostProvider";
import CommentForm from "../Comment/CommentForm";
import Post from "./Post";

const PostList = () => {
  const { posts, getAllPosts } = useContext(PostContext);

  useEffect(() => {
    //? So I had a situation where I would use the search bar for posts
    //? They would come back just fine, however
    //? if I were to click on the links to root path
    //? nothing would change so I put the pathname in the dep list
    //? then check to see if I was at root to get all posts
    if (window.location.pathname === '/'){
      getAllPosts();
    }
  }, [window.location.pathname]);

  return (
    <div className="container">
      {posts.map((post) => (
        <div key={post.id}>
          <Post  post={post} />
          {post.comments.length > 0 && <CommentForm  />}
        </div>
      ))}
    </div>
  );
};

export default PostList;
