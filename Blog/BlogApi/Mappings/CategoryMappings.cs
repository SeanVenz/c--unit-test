using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Utils;

namespace BlogApi.Mappings
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<CategoryCreationDto, Category>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.Name!)));

            CreateMap<CategoryUpdationDto, Category>()
                 .ForMember(dto => dto.Name, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.Name!)));
        }
    }
}
