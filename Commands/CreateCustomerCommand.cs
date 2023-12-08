using AutoMapper;
using MediatR;
using RepharmServiceCalendar.DTOs;
using RepharmServiceCalendar.Entities;
using RepharmServiceCalendar.Models;
using System.Net;

namespace RepharmServiceCalendar.Commands
{
    public record CreateCustomerCommand : IRequest<BaseResponse<CustomerDto>>
    {
        public CustomerViewModel Model { get; set; }
    }
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, BaseResponse<CustomerDto>>
    {
        private readonly IMapper _mapper;
        private readonly BackendContext _context;

        public CreateCustomerCommandHandler(BackendContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BaseResponse<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerToAdd = _mapper.Map<Customer>(request.Model);
            customerToAdd.DateCreated = DateTime.UtcNow;

            await _context.Customers.AddAsync(customerToAdd, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<CustomerDto>(customerToAdd);
            return new BaseResponse<CustomerDto>(result, false, null, HttpStatusCode.OK);
        }
    }
}
