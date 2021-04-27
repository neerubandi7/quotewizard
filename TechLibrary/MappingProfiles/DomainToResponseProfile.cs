using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
namespace TechLibrary.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Book, BookData>().ForMember(x => x.Descr, opt => opt.MapFrom(src => src.ShortDescr));
            // CreateMap<BookData, Book>().ForMember(x => x.ShortDescr, opt => opt.MapFrom(src => src.Descr));
        }
    }
}