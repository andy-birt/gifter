import React, { useContext, useEffect } from "react";
import { Button } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";

const Like = ({ likes, postId }) => {

  const { addLikeToPost } = useContext(PostContext);

  const handleLike = () => {
    addLikeToPost({ postId });
  };

  useEffect(() => {
    console.log(`Post has ${likes} likes`)
  }, [addLikeToPost]);

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