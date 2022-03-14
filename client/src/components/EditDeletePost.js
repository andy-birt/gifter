import React from "react";
import { Link } from "react-router-dom";

const EditDeletePost = ({ id }) => {
  return (
    <div className="text-left px-2">
      <Link to={`/posts/${id}/edit`} className="btn btn-warning">
        <i className="bi bi-pencil-square"></i>
      </Link>
      {' '}
      <Link to={`/posts/${id}/delete`} className="btn btn-danger">
        <i className="bi bi-trash"></i>
      </Link>
    </div>
  );
};

export default EditDeletePost;