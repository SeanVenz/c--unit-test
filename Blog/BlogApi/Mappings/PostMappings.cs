using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Utils;

namespace BlogApi.Mappings
{
    public class PostMappings : Profile
    {
        public PostMappings()
        {
            CreateMap<PostCreationDto, Post>()
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(st => st.UserId))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.Title!)))
                .ForMember(dto => dto.Content, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.Content!)))
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(st => string.Join(",", st.Categories!)))
                .ForMember(dto => dto.Status, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.Status!)))
                .ForMember(dto => dto.DateTimeCreated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()))
                .ForMember(dto => dto.DateTimeUpdated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));

            CreateMap<PostUpdationDto, Post>()
                .ForMember(dto => dto.Title, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.Title!)))
                .ForMember(dto => dto.Content, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.Content!)))
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(st => string.Join(",", st.Categories!)))
                .ForMember(dto => dto.Status, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.Status!)))
                .ForMember(dto => dto.DateTimeUpdated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));
        }
    }
}
