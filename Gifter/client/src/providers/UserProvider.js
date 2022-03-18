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

  //* State that holds a list of users
  const [users, setUsers] = useState([]);

  const navigate = useNavigate();

  /**
   *  ------------------------------  
   ** ----- User Profile CRUD  ----- 
   *  ------------------------------
   */

  const getAllUsers = () => {
    return fetch("/api/userprofile")
      .then(res => res.json())
      .then(setUsers);
  };

  /**
   *  ---------------------------  
   ** ----- Authentication  ----- 
   *  ---------------------------
   */

  /**
   * Function that gets user from Gifter database using fetch
   * @author NewForce
   * @param {Object} userObject
   * @returns void
   */

  const login = (userObject) => {
    fetch(`api/userprofile/getbyemail?email=${userObject.email}`)
      .then((r) => r.json())
      .then((userObjFromDB) => {
        localStorage.setItem("gifterUser", JSON.stringify(userObjFromDB));
        setCurrentUser(currentUserInLocalStorage);
        navigate('/');
      })
  };

  /**
   * Function that creates a new user in Gifter database using fetch
   * then sets the currentUser in state
   * @author NewForce
   * @param {Object} userObject
   * @returns void
   */

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

  /**
   * Function that clears localStorage
   * @author NewForce
   */

  const logout = () => {
    localStorage.clear();
  };

  return (
    <UserContext.Provider value={{ 
      currentUser, login, register, logout, setUser, user, users,
      getAllUsers 
      }} >
      {props.children}
    </UserContext.Provider>
  );
};