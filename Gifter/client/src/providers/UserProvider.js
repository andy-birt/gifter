import React, { createContext, useState } from "react";
import { useNavigate } from "react-router-dom";

export const UserContext = createContext();

export const UserProvider = (props) => {

  //* Get the gifterUser value stored in localStorage
  const currentUserInLocalStorage = JSON.parse(localStorage.getItem("gifterUser"));

  //* This state controls the user that currentUser interacts with
  const [user, setUser] = useState();

  //* State that uses currently logged in user via localStorage
  const [currentUser, setCurrentUser] = useState(currentUserInLocalStorage);

  const navigate = useNavigate();

  const login = (userObject) => {
    fetch(`api/userprofile/getbyemail?email=${userObject.email}`)
      .then((r) => r.json())
      .then((userObjFromDB) => {
        localStorage.setItem("gifterUser", JSON.stringify(userObjFromDB));
        setCurrentUser(currentUserInLocalStorage);
        navigate('/');
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
        setCurrentUser(currentUserInLocalStorage);
        navigate('/');
      });
  };

  const logout = () => {
    localStorage.clear();
  };

  return (
    <UserContext.Provider value={{ currentUser, login, register, logout, setUser, user }} >
      {props.children}
    </UserContext.Provider>
  );
};