import React, { useContext } from "react";
import { useParams } from "react-router-dom";
import { Button } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";

const Like = ({ likes, postId }) => {


  const { addLikeToPost, getAllPosts, getAllPostsByUser, getAllPostsBySearch, getPost, query } = useContext(PostContext);

  const { id } = useParams();

  const handleLike = () => {
    addLikeToPost({ postId })
      .then(() => {
        //* Depending on your current location when liking a post
        //* That particular route will be requested when you like a post
        switch (window.location.pathname) {
          case "/posts/results":
            //* Refresh posts in post search
            getAllPostsBySearch(query);
            break;
          case `/posts/${id}`:
            //* Refresh post in page details
            getPost(id);
            break;
          case `/users/${id}`:
            //* Refresh posts in user's post list
            getAllPostsByUser(id);
            break;
          default:
            //* Refresh posts from the '/' route 
            getAllPosts();
            break;
        }
      });
  };

  return (
    <Button onClick={handleLike}>
      <i className="bi bi-star mx-2"></i>
      {/** 
      //* Dont show likes unless they are more than 0  
      **/}
      { likes > 0 && <span className="mx-2">{likes}</span>}
    </Button>
  );
};

export default Like;