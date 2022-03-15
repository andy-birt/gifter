import React, { useContext, useEffect } from "react";
import { PostContext } from "../../providers/PostProvider";
import CommentForm from "../Comment/CommentForm";
import Post from "./Post";

const PostList = () => {
  const { posts, getAllPosts } = useContext(PostContext);

  useEffect(() => {
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
