using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Context;
using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;


namespace GenX.TaskEasyTool.Data.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) 
        {
            _context = context; 
        }

        public User GetByUsername(string username) 
        {
            return _context.Users.FirstOrDefault(u => u.Username == username); 
        }
        public List<User> GetAll() 
        { 
            return _context.Users.ToList(); 
        }
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges(); 
        }
        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public void Update(User user) 
        { 
            _context.Users.Update(user); 
            _context.SaveChanges(); 
        }
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
