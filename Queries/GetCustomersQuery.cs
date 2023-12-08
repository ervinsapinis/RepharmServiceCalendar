using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RepharmServiceCalendar.DTOs;
using System.Net;

namespace RepharmServiceCalendar.Queries
{
    public record GetCustomersQuery : IRequest<BaseResponse<List<CustomerDto>>>;

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, BaseResponse<List<CustomerDto>>>
    {
        private readonly IMapper _mapper;
        private readonly BackendContext _context;

        public GetCustomersQueryHandler(BackendContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _context.Customers.ToListAsync(cancellationToken);
            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

            return new BaseResponse<List<CustomerDto>>(customerDtos, false, null, HttpStatusCode.OK);
        }
    }
}