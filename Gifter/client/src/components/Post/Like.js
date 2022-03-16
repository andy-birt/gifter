import React, { useContext } from "react";
import { useParams } from "react-router-dom";
import { Button } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";

const Like = ({ likes, postId }) => {


  const { addLikeToPost } = useContext(PostContext);

  const { id } = useParams();

  const handleLike = () => {
    switch (window.location.pathname) {
      case "/post/results":
        console.log("liked in search");
        break;
      case `/posts/${id}`:
        console.log("liked in details?")
        break;
      default:
        console.log("home... root")
        break;
    }
    // addLikeToPost({ postId });
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