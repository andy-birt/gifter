import React, { createContext, useState, useEffect } from "react";

export const UserContext = createContext();

export const UserProvider = (props) => {

  //* This state controls the user that currentUser interacts with
  const [user, setUser] = useState();

  // const [currentUser, setCurrentUser] = useState({});

  // useEffect(() => {
  //   const stringifiedUser = localStorage.getItem("gifterUser");
  //   const currentUser = JSON.parse(stringifiedUser);
    
  //   getUserByEmail(currentUser.email).then(setCurrentUser);
    
  //   return () => {
  //     setCurrentUser({});
  //   }
  // }, []);

  const login = (userObject) => {
    fetch(`api/userprofile/getbyemail?email=${userObject.email}`)
      .then((r) => r.json())
      .then((userObjFromDB) => {
        localStorage.setItem("gifterUser", JSON.stringify(userObjFromDB));
        
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

  // const getUserByEmail = (email) => {
  //   return fetch(`api/userprofile/getbyemail?email=${email}`)
  //           .then((r) => r.json());
  // }

  const logout = () => {
    localStorage.clear();
  };

  return (
    <UserContext.Provider value={{ login, register, logout, setUser, user }} >
      {props.children}
    </UserContext.Provider>
  );
};