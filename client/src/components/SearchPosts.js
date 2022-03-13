import React, { useContext, useState } from "react";
import { Button, Container, Form, FormGroup, Input, Label } from "reactstrap";
import { PostContext } from "../providers/PostProvider";

const SearchPosts = () => {

  const [search, setSearch] = useState('');

  const {getAllPostsBySearch} = useContext(PostContext);

  const searchPosts = () => {
    getAllPostsBySearch(search);
  };

  return (
    <Container className="pt-5">
      <Form inline>
        <FormGroup floating>
          <Input 
            id="q"
            placeholder="Search Posts..."
            onChange={(e) => setSearch(e.target.value)}
          />
          <Label>Search Posts...</Label>
        </FormGroup>
        <Button onClick={searchPosts} >Search</Button>
      </Form>
    </Container>
  );
};

export default SearchPosts;