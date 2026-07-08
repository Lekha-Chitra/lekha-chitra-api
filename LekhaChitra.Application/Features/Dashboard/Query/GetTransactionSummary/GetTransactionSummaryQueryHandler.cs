using LekhaChitra.Application.Constants.Enums;
using LekhaChitra.Application.DTO.Dashboard;
using LekhaChitra.Application.Helpers.Extensions;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Dashboard.Query.GetTransactionSummary
{
    public class GetTransactionSummaryQueryHandler :
                         IRequestHandler<GetTransactionSummaryQuery,
                                        ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;

        public GetTransactionSummaryQueryHandler(IUnitOfWork uow,
                                                ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(GetTransactionSummaryQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;

            if (tenantId == Guid.Empty)
            {
                return ServiceResponse<object>.Unauthorized("Access denied.");
            }
            var transactions = _uow.AsyncRepositories<ClientTransaction>()
                                .GetQueryable()
                                .AsNoTracking()
                                .Include(x => x.Payment)
                                .Where(x => x.TenantId == tenantId)
                                .ApplyDateFilter(request.FromDate, request.ToDate);

            if (!await transactions.AnyAsync(cancellationToken))
            {
                return ServiceResponse.NotFound("No transaction data found.");
            }
            var totalTransactions = await transactions.CountAsync(cancellationToken);
            var totalTransactionAmount = await transactions
                                                .Select(x => (decimal?)x.Payment.Amount)
                                                .SumAsync(cancellationToken) ?? 0;

            var averageTransactionAmount = await transactions
                                                .Select(x => (decimal?)x.Payment.Amount)
                                                .AverageAsync(cancellationToken) ?? 0;

            var highestTransactionAmount = await transactions
                                                .Select(x => (decimal?)x.Payment.Amount)
                                                .MaxAsync(cancellationToken) ?? 0;
            var lowestTransactionAmount = await transactions
                                               .Select(x => (decimal?)x.Payment.Amount)
                                               .MinAsync(cancellationToken) ?? 0;
            var totalCreditTransactions = await transactions.CountAsync(x => 
                                                x.Payment.PaymentType == PaymentTypeEnums.Credit.ToString(),
                                                cancellationToken);

            var totalDebitTransactions = await transactions.CountAsync(x => 
                                              x.Payment.PaymentType == PaymentTypeEnums.Debit.ToString(),
                                              cancellationToken);

            var totalCreditAmount = await transactions
                                            .Where(x => x.Payment.PaymentType == PaymentTypeEnums.Credit.ToString())
                                            .Select(x => (decimal?)x.Payment.Amount)
                                            .SumAsync(cancellationToken) ?? 0;

            var totalDebitAmount = await transactions
                                           .Where(x => x.Payment.PaymentType == PaymentTypeEnums.Debit.ToString())
                                           .Select(x => (decimal?)x.Payment.Amount)
                                           .SumAsync(cancellationToken) ?? 0;
            var transactionGrowth = await transactions
                               .Where(x => x.AddedDate.HasValue)
                               .GroupBy(x => new
                               {
                                   x.AddedDate.Value.Year,
                                   x.AddedDate.Value.Month
                               })
                               .Select(x => new TransactionGrowthDTO
                               {
                                   Year = x.Key.Year,
                                   Month = x.Key.Month,
                                   MonthName = CultureInfo.CurrentCulture.DateTimeFormat
                                                   .GetAbbreviatedMonthName(x.Key.Month),
                                   TotalTransactions = x.Count(),
                                   TotalAmount = x.Sum(t => t.Payment.Amount)
                               })
                                .OrderBy(x => x.Year)
                                .ThenBy(x => x.Month)
                                .ToListAsync(cancellationToken);

            var transactionTypes = await transactions
                                .GroupBy(x => x.TransactionType)
                                .Select(x => new TransactionTypeSummaryDTO
                                {
                                    TransactionType = x.Key,
                                    TotalTransactions = x.Count(),
                                    TotalAmount = x.Sum(t => t.Payment.Amount)
                                })
                                .ToListAsync(cancellationToken);
            var paymentMethods = await transactions
                                .GroupBy(x => x.Payment.PaymentMethod)
                                .Select(x => new PaymentMethodSummaryDTO
                                {
                                    PaymentMethod = x.Key,
                                    TotalTransactions = x.Count(),
                                    TotalAmount = x.Sum(t => t.Payment.Amount)
                                })
                                .ToListAsync(cancellationToken);

            var response = new GetTransactionSummaryDTO
            {
                TotalTransactions = totalTransactions,
                TotalTransactionAmount = totalTransactionAmount,
                AverageTransactionAmount = averageTransactionAmount,
                HighestTransactionAmount = highestTransactionAmount,
                LowestTransactionAmount = lowestTransactionAmount,

                TotalCreditTransactions = totalCreditTransactions,
                TotalDebitTransactions = totalDebitTransactions,

                TotalCreditAmount = totalCreditAmount,
                TotalDebitAmount = totalDebitAmount,

                TransactionGrowth = transactionGrowth,
                TransactionTypes = transactionTypes,
                PaymentMethods = paymentMethods
            };

            return ServiceResponse<object>.Success(response);
        }
    }
}
