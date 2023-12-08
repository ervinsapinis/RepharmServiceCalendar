using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RepharmServiceCalendar.DTOs;
using System.Net;

namespace RepharmServiceCalendar.Queries
{
    public record GetServiceTypesQuery : IRequest<BaseResponse<List<ServiceTypeDto>>>;

    public class GetServiceTypesQueryHandler : IRequestHandler<GetServiceTypesQuery, BaseResponse<List<ServiceTypeDto>>>
    {
        private readonly IMapper _mapper;
        private readonly BackendContext _context;

        public GetServiceTypesQueryHandler(BackendContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse<List<ServiceTypeDto>>> Handle(GetServiceTypesQuery request, CancellationToken cancellationToken)
        {
            var serviceTypes = await _context.ServiceTypes
                .Include(st => st.Services)
                .ToListAsync(cancellationToken);

            var serviceTypeDtos = _mapper.Map<List<ServiceTypeDto>>(serviceTypes);

            return new BaseResponse<List<ServiceTypeDto>>(serviceTypeDtos, false, null, HttpStatusCode.OK);
        }
    }

}