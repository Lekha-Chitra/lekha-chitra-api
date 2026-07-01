using LekhaChitra.Application.Constants.Enums;
using LekhaChitra.Application.DTO.Dashboard;
using LekhaChitra.Application.Helpers.Extensions;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Clients;
using LekhaChitra.Domain.Entities.Application.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Dashboard.Query.GetSummary
{
    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery , ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;

        public GetDashboardSummaryQueryHandler(
                        IUnitOfWork uow,
                        ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }


        public async Task<ServiceResponse> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse<object>.Unauthorized("Access denied.");
            }
            var clients =  _uow.AsyncRepositories<Client>()
                                                .GetQueryable()
                                                .AsNoTracking()
                                                .Where(x => x.TenantId == tenantId) 
                                                .ApplyDateFilter(request.FromDate, request.ToDate);

            var payments = _uow.AsyncRepositories<Payment>()
                                                    .GetQueryable()
                                                    .AsNoTracking()
                                                    .Where(x => x.TenantId == tenantId)
                                                    .ApplyDateFilter(request.FromDate, request.ToDate);
            if (!clients.Any() || !payments.Any())
            {
                return ServiceResponse.NotFound("No summary data found!!");
            }
            var clientCount = await clients.CountAsync(cancellationToken);
            var currentBalance = await clients.SumAsync(c => c.Balance, cancellationToken);


            var creditAmount = await payments.Where(x => x.PaymentType == PaymentTypeEnums.Credit.ToString())
                                            .Select(x => (decimal?)x.Amount)
                                            .SumAsync(cancellationToken) ?? 0;

            var debitAmount = await payments.Where(x => x.PaymentType == PaymentTypeEnums.Debit.ToString())
                                             .Select(x => (decimal?)x.Amount)
                                            .SumAsync(cancellationToken) ?? 0;

            var summary = new GetDashboardSummaryDTO()
            { 
                TotalClients = clientCount,
                CreditAmount = creditAmount,
                DebitAmount = debitAmount,
                CurrentBalance = currentBalance,
            };
            return ServiceResponse<object>.Success(summary);
        }
    }
}
