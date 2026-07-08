using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Dashboard.Query.GetTransactionSummary
{
    public record GetTransactionSummaryQuery
                    (DateTime? FromDate,
                    DateTime? ToDate) : IRequest<ServiceResponse>;

}
