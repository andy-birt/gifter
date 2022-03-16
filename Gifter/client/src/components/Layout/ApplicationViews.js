import React from "react";
import { Route, Routes } from "react-router-dom";
import PostList from "../Post/PostList";
import PostForm from "../Post/PostForm";
import PostDetails from "../Post/PostDetails";
import UserPosts from "../Post/UserPosts";

const ApplicationViews = () => {
  return (
    <Routes>
      <Route path="/" exact element={<PostList />}/>

      <Route path="/posts/add" element={<PostForm />} />

      <Route path="/posts/results" element={<PostList />} />

      <Route path="/posts/:id/edit" element={<PostForm />} />

      <Route path="/posts/:id" element={<PostDetails />} />

      <Route path="/users/:id" element={<UserPosts />} />
    </Routes>
  );
};

export default ApplicationViews;