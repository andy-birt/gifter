import React from "react";
import './App.css';
import { PostProvider } from "./providers/PostProvider";
import ApplicationViews from "./components/ApplicationViews";
import Header from "./components/Header";

function App() {
  return (
    <div className="App">
      <PostProvider>
        <Header />
        <ApplicationViews />
        {/* <PostForm />
        <SearchPosts />
        <PostList /> */}
      </PostProvider>
    </div>
  );
}

export default App;
