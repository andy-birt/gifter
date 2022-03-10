using Gifter.Models;
using System.Collections.Generic;

namespace Gifter.Repositories
{
    public interface IUserProfileRepository
    {
        List<UserProfile> GetAll();
        UserProfile GetById(int id);
    }
}