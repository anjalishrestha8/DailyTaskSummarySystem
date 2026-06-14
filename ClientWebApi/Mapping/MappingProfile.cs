using AutoMapper;
using ClientWebApi.Dto.RequestDto;
using ClientWebApi.Dto.ResponseDto;
using ClientWebApi.Models.Entities;

namespace ClientWebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Request DTOs to Entity
          
            CreateMap<UserTaskSummaryReqDto, UserTaskSummary>().ReverseMap();
            CreateMap<CommentRequestDto, Comments>().ReverseMap();

            //Entity to Response DTOs
           
            CreateMap<UserTaskSummary, UserTaskSummaryResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
            CreateMap<Comments, CommentResponseDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty));
        }
    }
}
