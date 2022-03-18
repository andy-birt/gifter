import React, { useContext, useEffect } from "react";
import { UserContext } from "../../providers/UserProvider";
import User from "./User";

const UserList = () => {
  const { users, getAllUsers } = useContext(UserContext);

  useEffect(() => {
    getAllUsers();
  }, []);

  return (
    <div className="container">
      {users.map((user) => (
        <div key={user.id}>
          <User user={user} />
        </div>
      ))}
    </div>
  );
};

export default UserList;