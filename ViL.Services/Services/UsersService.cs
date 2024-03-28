using ViL.Data.Models;
using ViL.Data;
using ViL.Data.Repositories;
using ViL.Common.Exceptions;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IUsersService: IServices<Users>
    {
        public Users Login(string username, string password);
    }

    public class UsersService : ServiceBase<Users>, IUsersService
    {
        public UsersService(IUsersRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }


        public Users Login(string username, string password)
        {
            var user = _repository.Table.Where(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                throw new VilUnauthorizeExceptions("Tên người dùng không tồn tại");
            }
            else if (user.Password != password)
            {
                throw new VilUnauthorizeExceptions("Mật khẩu không đúng");
            }
            return user;
        }

        protected override bool validate(Users entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity);
            }

            var isValid = true;
            var query = _repository.GetById(entity.UserId);
            if (query != null)
            {
                if (query.Username != entity.Username)
                {
                    isValid = false;
                    listErrorMsgs.Add($"Username không được phép thay đổi");
                }                
            } else
            {
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
