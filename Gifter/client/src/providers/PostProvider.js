import React, { useContext, useState } from "react";
import { UserContext } from "./UserProvider";

export const PostContext = React.createContext();

export const PostProvider = (props) => {

  //* State for a list of posts 
  const [posts, setPosts] = useState([]);

  //* State for a single post
  const [post, setPost] = useState();

  //* State for a search post query
  const [query, setQuery] = useState("");

  const { currentUser, setUser } = useContext(UserContext);

  //* Get Posts from the current user's feed
  const getFeed = () => {
    return fetch(`/api/post/feed?id=${currentUser.id}`)
      .then(res => res.json())
      .then(setPosts);
  };

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
      .then(res => res.json())
      .then(user => {
        setUser(user);
        setPosts(user.posts);
      });
  };

  const getPost = (id) => {
    return fetch(`/api/post/${id}/getwithcomments`)
      .then(res => res.json())
      .then(setPost);
  };

  const getPostToEdit = (id) => {
    return fetch(`/api/post/${id}`)
      .then(res => res.json())
      .then(setPost);
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

  const editPost = (post) => {
    return fetch(`/api/post/${post.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(post)
    });
  };

  const deletePost = (postId) => {
    return fetch(`/api/post/${postId}`, { method: "DELETE" })
      .then(getAllPosts);
  };

  //* Yes I'm adding the comment functionality in post provider since the comments are developed in the context of a post anyway
  const addCommentToPost = (comment) => {
    return fetch(`api/comment`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(comment)
    }).then(getAllPosts);
  };

  //* Add a like to the post
  const addLikeToPost = (postId) => {
    return fetch(`/api/like`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(postId)
    });
  };

  return (
    <PostContext.Provider value={{ 
      posts, post, setQuery, query, 
      getFeed, getAllPosts, getPost, getPostToEdit, 
      addPost, editPost, deletePost, 
      getAllPostsBySearch, getAllPostsByUser, 
      addCommentToPost, addLikeToPost 
    }}>
      {props.children}
    </PostContext.Provider>
  );
};
