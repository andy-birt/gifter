import React, { useEffect, useContext } from "react";
import { PostContext } from "../../providers/PostProvider";
import { useParams } from "react-router-dom";
import Post from "./Post";

const PostDetails = () => {
  //* Get state from the provider
  const { post, getPost } = useContext(PostContext);
  const { id } = useParams();

  useEffect(() => {
    getPost(id);
  }, []);

  if (!post) {
    return null;
  }

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="col-sm-12 col-lg-6">
          <Post post={post} />
        </div>
      </div>
    </div>
  );
};

export default PostDetails;