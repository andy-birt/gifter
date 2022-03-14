import React, { createContext, useEffect, useState } from "react";

export const UserContext = createContext();

export const UserProvider = (props) => {
  
  const [currentUser, setCurrentUser] = useState({});

  //! For now we are going to log in as a certain user to implement features of posts, such as editing and deleting own posts
  useEffect(() => {
    return fetch('/api/userprofile/3').then(res => res.json()).then(setCurrentUser);
  }, []);
  

  return (
    <UserContext.Provider value={{ currentUser }} >
      {props.children}
    </UserContext.Provider>
  );
};