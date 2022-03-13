import React from "react";
import './App.css';
import { PostProvider } from "./providers/PostProvider";
import PostList from "./components/PostList";
import PostForm from "./components/PostForm";
import SearchPosts from "./components/SearchPosts";

function App() {
  return (
    <div className="App">
      <PostProvider>
        <PostForm />
        <SearchPosts />
        <PostList />
      </PostProvider>
    </div>
  );
}

export default App;
