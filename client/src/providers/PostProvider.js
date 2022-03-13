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
    return fetch("/api/post")
      .then(res => res.json())
      .then(setPosts);
  };

  const addPost = (post) => {
    return fetch("/api/post", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(post)
    });
  };

  return (
    <PostContext.Provider value={{ posts, getAllPosts, addPost, getAllPostsBySearch }}>
      {props.children}
    </PostContext.Provider>
  );
};
