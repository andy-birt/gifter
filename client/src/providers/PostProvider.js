import React, { useState } from "react";

export const PostContext = React.createContext();

export const PostProvider = (props) => {
  const [posts, setPosts] = useState([]);

  // Add a function to search posts using a query
  const getAllPostsBySearch = (q) => {
    return fetch(`/api/post/search?q=${q}&sortDesc=true`)
      .then(res => res.json())  
      .then(setPosts);
  };

  const getAllPosts = () => {
    return fetch("/api/post/getwithcomments")
      .then(res => res.json())
      .then(setPosts);
  };

  const getAllPostsByUser = (id) => {
    return fetch(`/api/userprofile/${id}/getwithposts`)
      .then(res => res.json());
  };

  const getPost = (id) => {
    return fetch(`/api/post/${id}/getwithcomments`).then((res) => res.json());
  };

  const getPostToEdit = (id) => {
    return fetch(`/api/post/${id}`).then((res) => res.json());
  }

  const addPost = (post) => {
    return fetch("/api/post", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(post)
    });
  };

  const editPost = (post) => {
    return fetch(`/api/post/${post.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(post)
    });
  };

  return (
    <PostContext.Provider value={{ posts, getAllPosts, getPost, getPostToEdit, addPost, editPost, getAllPostsBySearch, getAllPostsByUser }}>
      {props.children}
    </PostContext.Provider>
  );
};
