import React, { useContext, useState } from "react";
import { Form, Input,  Button, InputGroup } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";
import { UserContext } from "../../providers/UserProvider";

const CommentForm = ({ postId }) => {

  const { currentUser } = useContext(UserContext);
  const { addCommentToPost } = useContext(PostContext);

  //* Just make a state for the message of the comment
  const [message, setMessage] = useState("");

  const handleSubmit = (e) => {
    //? Clear the input field
    e.target.previousSibling.value = "";
    //* Handle comment creation in the submit function, seems to work better.
    const newComment = {
      postId,
      message,
      userProfileId: currentUser.id
    };
    
    addCommentToPost(newComment);
  };

  return (
    <Form className="pt-3">
      <InputGroup>
        <Input id="message" placeholder="Write a comment here..."  onChange={(e) => setMessage(e.target.value)}/>
        <Button color="primary" onClick={handleSubmit} >Submit Comment</Button>
      </InputGroup>
    </Form>
  );
};

export default CommentForm;