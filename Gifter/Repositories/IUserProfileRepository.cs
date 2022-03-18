using Gifter.Models;
using System.Collections.Generic;

namespace Gifter.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile user);
        void Delete(int id);
        List<UserProfile> GetAll();
        UserProfile GetByEmail(string email);
        UserProfile GetById(int id);
        UserProfile GetByIdWithPosts(int id);
        List<UserProfile> GetProviderUsersBySubscriberId(int id);
        void Update(UserProfile user);
    }
}