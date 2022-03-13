import React from "react";
import { Route, Routes } from "react-router-dom";
import PostList from "./PostList";
import PostForm from "./PostForm";
import SearchPosts from "./SearchPosts";
import PostDetails from "./PostDetails";

const ApplicationViews = () => {
  return (
    <Routes>
      <Route path="/" exact element={<PostList />}/>

      <Route path="/posts/add" element={<PostForm />} />

      <Route path="/posts/search" element={<SearchPosts />} />

      <Route path="/posts/:id" element={<PostDetails />}/>
    </Routes>
  );
};

export default ApplicationViews;