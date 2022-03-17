import React, { createContext, useState } from "react";

export const UserContext = createContext();

export const UserProvider = (props) => {

  //* This state controls the user that currentUser interacts with
  const [user, setUser] = useState();
  
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const getCurrentUser = () => {
    const currentUser = localStorage.getItem("gifterUser");
    return currentUser;
  };

  const login = (userObject) => {
    debugger;
    fetch(`api/userprofile/getbyemail?email=${userObject.email}`)
      .then((r) => r.json())
      .then((userObjFromDB) => {
        localStorage.setItem("gifterUser", JSON.stringify(userObjFromDB));
        setIsLoggedIn(true);
      })
  };

  const register = (userObject) => {
    fetch("/api/userprofile", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userObject),
    })
      .then((response) => response.json())
      .then((userObject) => {
        localStorage.setItem("gifterUser", JSON.stringify(userObject));
      });
  };

  const logout = () => {
    localStorage.clear();
    setIsLoggedIn(false);
  };

  return (
    <UserContext.Provider value={{ getCurrentUser, login, register, logout, isLoggedIn, setUser, user }} >
      {props.children}
    </UserContext.Provider>
  );
};