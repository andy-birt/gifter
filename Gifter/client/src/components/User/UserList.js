import React, { useContext, useEffect } from "react";
import { UserContext } from "../../providers/UserProvider";
import User from "./User";

const UserList = () => {
  const { currentUser, users, getAllUsers, providerUsers, getAllProviderUserByCurrentUserId } = useContext(UserContext);

  //* I wasn't sure if it would work better to have two separate useEffects since they control different states

  useEffect(() => {
    getAllProviderUserByCurrentUserId(currentUser.id);
  }, []);

  useEffect(() => {
    getAllUsers();
  }, []);

  return (
    <div className="container">
      {users.map((user) => (
        <div key={user.id}>
          <User user={user} currentUser={currentUser} providerUsers={providerUsers}/>
        </div>
      ))}
    </div>
  );
};

export default UserList;