using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Sisc.Api.Common.Helpers;

namespace Sisc.Api.Data.Common
{
    public static class QueryableExtensions
    {
        private static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
            bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty.First().ToString().ToUpper() + orderByProperty.Substring(1));
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        private static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
            bool desc)
        {
            string command = desc ? "ThenByDescending" : "ThenBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty.First().ToString().ToUpper() + orderByProperty.Substring(1));
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<TEntity> OrderByColumns<TEntity>(this IQueryable<TEntity> source, List<OrderByColumn> orderByColumns)
        {
            if (orderByColumns == null)
            {
                return source;
            }

            if (orderByColumns.Count == 0)
            {
                return source.OrderBy(orderByColumns[0].ColumnName, orderByColumns[0].Descending);
            }
            var src = source.OrderBy(orderByColumns[0].ColumnName, orderByColumns[0].Descending);
            for (int i = 1; i < orderByColumns.Count; i++)
            {
                src= src.ThenBy(orderByColumns[i].ColumnName, orderByColumns[i].Descending);
            }
            return src;


        }
    }
}
