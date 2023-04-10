using AutoMapper;

namespace MongoDB
{
    public class ArticlesProfile : Profile
    {
        public ArticlesProfile() 
        {
            CreateMap<ArticleImportDto, Article>();
        }
    }
}
