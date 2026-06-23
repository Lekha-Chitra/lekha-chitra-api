using LekhaChitra.Application.DTO.Clients;
using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Clients.Query.FilterClients
{
    public record FilterClientQuery(GetClientFilter Filter) : IRequest<ServiceResponse>;
}
