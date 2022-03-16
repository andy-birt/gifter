import React from "react";
import { ListGroupItem} from "reactstrap";

const Comment = ({ comment }) => {
  return (
    <ListGroupItem>
      <strong>{comment.userProfile.name}</strong>: { comment.message }
    </ListGroupItem>
  );
};

export default Comment;