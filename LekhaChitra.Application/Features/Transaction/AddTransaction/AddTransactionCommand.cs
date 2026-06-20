using LekhaChitra.Application.DTO.Transactions;
using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Transaction.AddTransaction
{
    public record AddTransactionCommand(AddTransactionDTO AddTransaction) : IRequest<ServiceResponse>;
}
 