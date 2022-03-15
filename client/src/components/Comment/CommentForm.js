import React from "react";
import { Form, Input,  Button, InputGroup } from "reactstrap";

const CommentForm = () => {
  return (
    <Form className="pt-3">
      <InputGroup>
        <Input placeholder="Write a comment here..."/>
        <Button color="primary">Submit Comment</Button>
      </InputGroup>
    </Form>
  );
}

export default CommentForm;