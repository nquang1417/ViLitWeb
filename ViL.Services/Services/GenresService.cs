using ViL.Common.Exceptions;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IGenresService : IServices<Genres>
    {
    }

    public class GenresService : ServiceBase<Genres>, IGenresService
    {
        public GenresService(IGenresRepository repository, ViLDbContext dbContext) : base(repository, dbContext)
        {
        }

        protected override bool validate(Genres entity, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                return base.validate(entity, isUpdate);
            }
            var isValid = true;
            var query = _repository.GetById(entity.GenreId);
            if (query == null)
            {
                isValid = false;
                throw new VilNotFoundExceptions("Nội dung không tồn tại");
            }
            return isValid;
        }
    }
}
