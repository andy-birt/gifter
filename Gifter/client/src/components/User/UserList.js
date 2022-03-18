import React, { useContext, useEffect } from "react";
import { UserContext } from "../../providers/UserProvider";
import User from "./User";

const UserList = () => {
  const { currentUser, users, getAllUsers, providerUsers, getAllProviderUserByCurrentUserId } = useContext(UserContext);

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
          <User user={user} providerUsers={providerUsers}/>
        </div>
      ))}
    </div>
  );
};

export default UserList;