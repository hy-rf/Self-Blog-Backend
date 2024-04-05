
using System.Linq.Expressions;
using BBS.Data;
using BBS.IRepository;
using BBS.Models;

namespace BBS.Repository
{
    public class TagRepository(AppDbContext context) : BaseRepository<Tag>(context), ITagRepository
    {
    }


}