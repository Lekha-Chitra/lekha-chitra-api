using LekhaChitra.Domain.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Helpers.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyDateFilter<T>(
            this IQueryable<T> query,
            DateTime? fromDate,
            DateTime? toDate)
            where T : IDateAudited
        {
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.AddedDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.AddedDate <= toDate.Value);
            }

            return query;
        }
    }
}
