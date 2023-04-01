using Blog.Infrastructure.Data.Common;
using Blog.Infrastructure.Data.Context;

namespace Blog.Infrastructure.Data.Repositories
{
    public class BlogRepository : Repository, IBlogRepository
    {

        public BlogRepository(BlogContext context) 
            : base(context) 
        { 
        }
    }
}
