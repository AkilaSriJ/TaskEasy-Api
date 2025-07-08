using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Models;

namespace GenX.TaskEasyTool.Data.Interface
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        void Add(User user);
        User GetByEmail(string email);
        void Update(User user);
        List<User> GetAll();
        User GetById(int id);
    }
}
