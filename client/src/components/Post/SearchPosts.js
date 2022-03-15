import React, { useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Form, Input, InputGroup } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";

const SearchPosts = () => {

  const [search, setSearch] = useState('');

  const {getAllPostsBySearch} = useContext(PostContext);

  const navigate = useNavigate();

  const searchPosts = (e) => {
    //* No page refreshing 
    e.preventDefault();
    //* Clear the search form
    e.target[0].value = "";
    //* Get the results
    getAllPostsBySearch(search)
    .then(() => navigate("/posts/results"));
    //* Set to default state
    setSearch("");
  };

  return (
    <Form inline onSubmit={searchPosts}>
      <InputGroup>
        <Input 
          id="q"
          placeholder="Search Posts..."
          onChange={(e) => setSearch(e.target.value)}
        />
        <Button outline color="dark" type="submit" ><i className="bi bi-search"></i></Button>
      </InputGroup>
    </Form>
  );
};

export default SearchPosts;