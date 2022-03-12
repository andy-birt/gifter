import React, { useContext, useState } from "react";
import { Button, Container, Form, FormGroup, Input, Label } from "reactstrap";
import { PostContext } from "../providers/PostProvider";

const PostForm = () => {

  //! Hard-code userProfileId for now until auth is implemented
  const newPost = { title: '', caption: '', imageUrl: '', userProfileId: 3 };

  const [post, setPost] = useState(newPost);

  const { addPost, getAllPosts } = useContext(PostContext);

  const handleChangeInput = (e) => {
    const newPostValue = { ...post };
    newPostValue[e.target.id] = e.target.value;
    setPost(newPostValue);
  };

  const handleSubmitPost = () => {
    addPost({ ...post, dateCreated: new Date().toISOString() })
    .then(getAllPosts);
    setPost(newPost);
  };

  return (
    <Container>
      <Form inline>
        <FormGroup floating>
          <Input
            id="title"
            placeholder="Title"
            onChange={handleChangeInput}
            value={post.title}
            />
          <Label for="title">Title</Label>
        </FormGroup>
        <FormGroup floating>
          <Input
            id="caption"
            placeholder="Caption"
            onChange={handleChangeInput}
            value={post.caption}
            />
          <Label for="caption">Caption</Label>
        </FormGroup>
        <FormGroup floating>
          <Input
            id="imageUrl"
            placeholder="Image URL"
            onChange={handleChangeInput}
            value={post.imageUrl}
            />
          <Label for="imageUrl">Image URL</Label>
        </FormGroup>
        <Button onClick={handleSubmitPost}>Submit</Button>
      </Form>
    </Container>
  );
};

export default PostForm;