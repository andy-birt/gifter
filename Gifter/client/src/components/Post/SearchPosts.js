import React, { useContext } from "react";
import { useNavigate } from "react-router-dom";
import { Button, Form, Input, InputGroup } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";

const SearchPosts = () => {

  const {getAllPostsBySearch, setQuery, query} = useContext(PostContext);

  const navigate = useNavigate();

  const searchPosts = (e) => {
    //* No page refreshing 
    e.preventDefault();
    //* Clear the search form
    e.target[0].value = "";
    //* Get the results
    getAllPostsBySearch(query)
    .then(() => navigate("/posts/results"));
  };

  return (
    <Form inline onSubmit={searchPosts}>
      <InputGroup>
        <Input 
          id="q"
          placeholder="Search Posts..."
          onChange={(e) => setQuery(e.target.value)}
        />
        <Button outline color="dark" type="submit" ><i className="bi bi-search"></i></Button>
      </InputGroup>
    </Form>
  );
};

export default SearchPosts;