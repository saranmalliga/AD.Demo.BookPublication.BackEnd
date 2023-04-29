using AD.Demo.BookPublication.Domain.DTO;
using AD.Demo.BookPublication.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Domain.AutoMapper
{
    public class EntityToDtoMappings : Profile
    {
        public EntityToDtoMappings()
        {
            CreateMap<Book, BookDTO>()
              .ForMember(e => e.Id, opt => opt.MapFrom(f => f.ID))
              .ForMember(e => e.Title, opt => opt.MapFrom(f => f.TITLE))
              .ForMember(e => e.Author, opt => opt.MapFrom(f => f.AUTHOR))
              .ForMember(e => e.Publisher, opt => opt.MapFrom(f => f.PUBLISHER))
              .ForMember(e => e.Genres, opt => opt.MapFrom(f => f.GENRES))
              .ForMember(e => e.TotalPages, opt => opt.MapFrom(f => f.TOTAL_PAGES))
              .ForMember(e => e.ISBN, opt => opt.MapFrom(f => f.ISBN))
              .ForMember(e => e.PublishedYear, opt => opt.MapFrom(f => f.PUBLISHED_YEAR))
              .ForMember(e => e.Language, opt => opt.MapFrom(f => f.BOOK_LANGUAGE))
              .ReverseMap();
        }
    }
}
