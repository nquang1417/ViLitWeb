using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViL.Data.Models;
using ViL.Data;
using ViL.Data.Repositories;
using System.Linq.Expressions;
using ViL.Common.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace ViL.Services.Services
{
    public interface IUsersService
    {
        public List<Users> GetAllUsers();
        public Users? GetById(string id);
        public void AddUser(Users user);
        public void UpdateUser(Users user);
        public void DeleteUser(Users user);
        public void Delete(Expression<Func<Users, bool>> where);
        public Users Login(string username, string password);
    }

    public class UsersService : IUsersService
    {
        private IUsersRepository _usersRepository;
        private ViLDbContext _dbContext;
        
        public UsersService(IUsersRepository usersRepository, ViLDbContext context)
        {
            _usersRepository = usersRepository;
            _dbContext = context;
        }

        public List<Users> GetAllUsers()
        {
            return _usersRepository.Table.ToList();
        }

        public Users? GetById(string id)
        {
            return (_usersRepository.GetById(id));
        }        

        public void AddUser(Users user)
        {
            if (validate(user))
            {
                _usersRepository.Add(user);
            }
        }

        public void UpdateUser(Users user)
        {
            if (validate(user, true))
            {
                _usersRepository.Update(user);
            }
        }

        public void DeleteUser(Users user)
        {
            _usersRepository.Delete(user);
        }
        
        public void Delete(Expression<Func<Users, bool>> where) {
            _usersRepository.Delete(where);
        }

        public Users Login(string username, string password)
        {
            var user = _usersRepository.Table.Where(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                throw new VilUnauthorizeExceptions("Tên người dùng không tồn tại");
            } else if (user.Password != password)
            {
                throw new VilUnauthorizeExceptions("Mật khẩu không đúng");
            }
            return user;
        }
       
        private bool validate(Users user, bool isUpdate = false)
        {
            if (user == null)
            {
                return false;
            }
            if (isUpdate)
            {
                var query = _usersRepository.Get(u => u.UserId == user.UserId);
                if (query == null)
                {
                    return false;
                }
                if (user.Username != query.Username)
                {
                    return false;
                }
                if (user.Role != query.Role)
                {
                    return false;
                }
            }
            else
            {
                if (user.Username.IsNullOrEmpty() || user.Password.IsNullOrEmpty())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
