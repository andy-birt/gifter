import React, { useEffect, useContext, useState } from "react";
import { ListGroup } from "reactstrap";
import { PostContext } from "../providers/PostProvider";
import { useParams } from "react-router-dom";
import Post from "./Post";
import Comment from "./Comment";

const PostDetails = () => {
  const [post, setPost] = useState();
  const { getPost } = useContext(PostContext);
  const { id } = useParams();

  useEffect(() => {
    getPost(id).then(setPost);
  }, []);

  if (!post) {
    return null;
  }

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="col-sm-12 col-lg-6">
          <Post post={post} />
          <ListGroup>
            {post.comments.map((c) => (
              <Comment key={c.id} comment={c} />
            ))}
          </ListGroup>
        </div>
      </div>
    </div>
  );
};

export default PostDetails;