import React, { useContext, useEffect } from "react";
import { PostContext } from "../../providers/PostProvider";
import { UserContext } from "../../providers/UserProvider";
import CommentForm from "../Comment/CommentForm";
import Post from "./Post";

const PostList = () => {
  
  const { posts, getAllPostsByUser, getFeed } = useContext(PostContext);

  //! I'm not sure localStorage plays well with the Context/State API
  //* UPDATE: I can use currentUser context state with localstorage!!
  const { currentUser } = useContext(UserContext);

  useEffect(() => {
    //? So I had a situation where I would use the search bar for posts
    //? They would come back just fine, however
    //? if I were to click on the links to root path
    //? nothing would change so I put the pathname in the dep list
    //? then check to see if I was at root to get all posts
    if (window.location.pathname === '/'){
      getAllPostsByUser(currentUser.id);
    }

    if (window.location.pathname === "/feed"){
      getFeed();
    }
  }, [window.location.pathname]);

  return (
    <div className="container">
      {posts.map((post) => (
        <div key={post.id}>
          <Post post={post} />
          { window.location.pathname !== "/posts/results" && <CommentForm postId={post.id} />}
        </div>
      ))}
    </div>
  );
};

export default PostList;
