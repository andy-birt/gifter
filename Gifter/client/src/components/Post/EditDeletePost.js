import React, { useContext } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Button } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";

const EditDeletePost = ({ id }) => {

  const { deletePost } = useContext(PostContext);

  const navigate = useNavigate();

  const handleDelete = (postId) => {
    if (window.confirm('Are you sure you want to delete post?')) {
      deletePost(postId).then(() => navigate("/"));
    }
  };

  return (
    <div className="text-left px-2">
      <Link to={`/posts/${id}/edit`} className="btn btn-warning">
        <i className="bi bi-pencil-square"></i>
      </Link>
      {' '}
      <Button className="btn btn-danger" onClick={() => handleDelete(id)}>
        <i className="bi bi-trash"></i>
      </Button>
    </div>
  );
};

export default EditDeletePost;