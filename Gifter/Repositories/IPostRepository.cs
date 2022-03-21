using Gifter.Models;
using System;
using System.Collections.Generic;

namespace Gifter.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        void Delete(int id);
        List<Post> Feed(int id);
        List<Post> GetAll();
        List<Post> GetAllWithComments();
        Post GetById(int id);
        Post GetByIdWithComments(int id);
        List<Post> Hottest(DateTime dateString);
        List<Post> Search(string criterion, bool sortDescending);
        void Update(Post post);
    }
}