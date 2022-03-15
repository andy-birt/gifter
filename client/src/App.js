import React from "react";
import './App.css';
import { PostProvider } from "./providers/PostProvider";
import { UserProvider } from "./providers/UserProvider";
import ApplicationViews from "./components/Layout/ApplicationViews";
import Header from "./components/Layout/Header";

function App() {
  return (
    <div className="App">
      <UserProvider>
        <PostProvider>
          <Header />
          <ApplicationViews />
        </PostProvider>
      </UserProvider>
    </div>
  );
}

export default App;
