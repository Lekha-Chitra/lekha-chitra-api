using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Transaction.Query.GetTransaction
{
    public record GetTransactionQuery(
            int PageNumber = 1,
            int PageSize = 10) : IRequest<ServiceResponse>;
}
