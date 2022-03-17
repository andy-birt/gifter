import React, { useContext } from "react";
import './App.css';
import { PostProvider } from "./providers/PostProvider";
import { UserContext, UserProvider } from "./providers/UserProvider";
import ApplicationViews from "./components/Layout/ApplicationViews";
import Header from "./components/Layout/Header";
import { Routes, Route, Navigate } from "react-router-dom";
import { Login } from "./components/Auth/Login";
import { Register } from "./components/Auth/Register";

function App() {

  // const { currentUser } = useContext(UserContext);
  // const { posts } = useContext(PostContext);

  return (
    <div className="App">
      <UserProvider>
        <PostProvider>
          <Routes>
            <Route path="*" element={
              <Auth />
            } />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
          </Routes>
        </PostProvider>
      </UserProvider>
    </div>
  );
}

function Auth() {
  const { currentUser } = useContext(UserContext);

  if (currentUser !== null) {
    return (<>
      <Header />
      <ApplicationViews />
    </>);
  }

  return <Navigate to="/login" />

}

export default App;
