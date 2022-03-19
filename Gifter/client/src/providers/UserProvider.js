import React, { createContext, useState } from "react";
import { useNavigate } from "react-router-dom";

export const UserContext = createContext();

export const UserProvider = (props) => {

  //* Get the gifterUser value stored in localStorage
  const currentUserInLocalStorage = localStorage.getItem("gifterUser");

  //* This state controls the user that currentUser interacts with
  const [user, setUser] = useState();

  //* State that uses currently logged in user via localStorage
  const [currentUser, setCurrentUser] = useState(JSON.parse(currentUserInLocalStorage));

  //* State that holds a list of users
  const [users, setUsers] = useState([]);

  //* State that holds list of users that currentUser is subscribed to
  const [providerUsers, setProviderUsers] = useState([]);

  //* The navigate function is primarily used in redirecting after creating a new user or loggin in
  const navigate = useNavigate();

  /**
   *  ------------------------------  
   ** ----- User Profile CRUD  ----- 
   *  ------------------------------
   */

  /**
   * Function that gets users from Gifter database except the currently logged in user
   * @author Andy
   * @returns Promise
   */

  const getAllUsers = () => {
    return fetch("/api/userprofile")
      .then(res => res.json())
      .then(users => {
        const usersOtherThanCurrentUser = users.filter(u => u.id !== currentUser.id);
        setUsers(usersOtherThanCurrentUser);
      });
  };

  /**
   * Function that gets users from Gifter database that the currently logged in user is subscribed to
   * @author Andy
   * @param {integer} subscriberId
   * @returns Promise
   */

  const getAllProviderUserByCurrentUserId = (subscriberId) => {
    return fetch(`/api/userprofile/getproviderusersbysubscriberid?subscriberid=${subscriberId}`)
      .then(res => res.json())
      .then(setProviderUsers);
  };

  /**
   *  ---------------------------  
   ** ----- Authentication  ----- 
   *  ---------------------------
   */

  //? I don't know what it is about trying to set currentUser state that is inconsistent
  //? Sometimes it works and sometimes it doesn't

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
        //* If NotFound is returned then we don't want to set localStorage or currentUser
        if (!userObjFromDB.status) {
          localStorage.setItem("gifterUser", JSON.stringify(userObjFromDB));
          setCurrentUser(userObjFromDB);
          navigate('/');
        }
      });
  };

  /**
   * Function that creates a new user in Gifter database using fetch
   * then sets the currentUser in state
   * @author NewForce
   * @param {Object} userObject
   * @returns void
   */

  const register = (userObjectFormBody) => {
    fetch("/api/userprofile", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userObjectFormBody),
    })
      .then((response) => response.json())
      .then((userObject) => {
        //* If BadRequest is returned we don't want to make user with duplicate emails in db
        if (!userObject.status) {
          localStorage.setItem("gifterUser", JSON.stringify(userObject));
          setCurrentUser(userObject);
          navigate('/');
        } else {
          alert(`User with email ${userObjectFormBody.email} already exists. Please enter a different email address.`);
        }
      });
  };

  /**
   * Function that clears localStorage
   * @author NewForce
   */

  const logout = () => {
    localStorage.clear();
  };

  /**
   *  --------------------------  
   ** ----- Subscriptions  ----- 
   *  --------------------------
   */

  /**
   * Creates a new subscription record in the database between a Subscriber User and Provider User
   * @param {integer} subscriberId 
   * @param {integer} providerId 
   * @returns Promise
   */
  const addSubscription = (subscriberId, providerId) => {
    return fetch(`/api/subscription?subscriberid=${subscriberId}&providerid=${providerId}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ subscriberId, providerId })
    }).then(() => getAllProviderUserByCurrentUserId(currentUser.id));
  };

  /**
   * Removes the subscription between a Subscriber User and Provider User
   * @param {integer} subscriberId 
   * @param {integer} providerId 
   * @returns Promise
   */

  const removeSubscription = (subscriberId, providerId) => {
    return fetch(`/api/subscription?subscriberid=${subscriberId}&providerid=${providerId}`, {
      method: "DELETE"
    }).then(() => getAllProviderUserByCurrentUserId(currentUser.id));
  };

  return (
    <UserContext.Provider value={{ 
      currentUser, login, register, logout, setUser, user, users,
      getAllUsers, getAllProviderUserByCurrentUserId, providerUsers,
      addSubscription, removeSubscription
      }} >
      {props.children}
    </UserContext.Provider>
  );
};