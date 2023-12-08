using AutoMapper;
using RepharmServiceCalendar.DTOs;
using RepharmServiceCalendar.Entities;
using RepharmServiceCalendar.Models;

namespace RepharmServiceCalendar
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentDto>();
            CreateMap <AppointmentViewModel, Appointment>();
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceType, ServiceTypeDto>();
            CreateMap<Customer, CustomerDto>();
        }
    }
}
