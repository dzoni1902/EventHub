using AutoMapper;
using EventHub.Core.Dtos;
using EventHub.Core.Models;

namespace EventHub.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, UserDto>();
            Mapper.CreateMap<Event, EventDto>();
            Mapper.CreateMap<Notification, NotificationDto>();
        }
    }
}