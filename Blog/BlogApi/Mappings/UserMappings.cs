using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Utils;

namespace BlogApi.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserCreationDto, User>(MemberList.None)
                .ForMember(dto => dto.Password, opt => opt.MapFrom(st => StringUtil.HashPlainText(st.Password!, null)))
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.FirstName!)))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.LastName!)))
                .ForMember(dto => dto.DateTimeCreated, opt => opt.MapFrom(st => StringUtil.GetCurrentDateTime()));

            CreateMap<UserUpdationDto, User>(MemberList.None)
                .ForMember(dto => dto.FirstName, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.FirstName!)))
                .ForMember(dto => dto.LastName, opt => opt.MapFrom(st => StringUtil.ToTitleCase(st.LastName!)));
        }
    }
}

