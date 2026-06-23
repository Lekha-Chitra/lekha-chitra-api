using LekhaChitra.Application.DTO.Clients;
using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Clients.Command.AddClients
{
    public record  AddClientCommand(AddClientDTO Client) : IRequest<ServiceResponse>;
}
