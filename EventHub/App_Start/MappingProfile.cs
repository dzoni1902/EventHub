using AutoMapper;
using EventHub.Dtos;
using EventHub.Models;

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