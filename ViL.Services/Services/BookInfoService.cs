using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViL.Data;
using ViL.Data.Models;
using ViL.Data.Repositories;
using ViL.Services.Infrastructure;

namespace ViL.Services.Services
{
    public interface IBookInfoService: IServices<BookInfo>
    {
    }
    public class BookInfoService : ServiceBase<BookInfo>, IBookInfoService
    {
        public BookInfoService(IBookInfoRepository bookInfoRepository, ViLDbContext dbContext) : base(bookInfoRepository, dbContext)
        {
        }   
    }
}
