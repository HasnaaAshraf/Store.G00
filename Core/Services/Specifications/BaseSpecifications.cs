using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;

namespace Services.Specifications
{
    public class BaseSpecifications<TEnitity, TKey> : ISpecifications<TEnitity, TKey>
                 where TEnitity : BaseEntity<TKey>
    {
        public Expression<Func<TEnitity, bool>>? Criteria { get ; set ; }
        public List<Expression<Func<TEnitity, object>>> IncludeExpressions { get; set; } = new List<Expression<Func<TEnitity, object>>>();
        public Expression<Func<TEnitity, object>>? OrderBy { get ; set ; }
        public Expression<Func<TEnitity, object>>? OrderByDescending { get ; set ; }
        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPagination { get ; set ; }

        public BaseSpecifications(Expression<Func<TEnitity, bool>>? expression)
        {
            Criteria = expression;
        }

        protected void AddInclude(Expression<Func<TEnitity, object>> expression)
        {
            IncludeExpressions.Add(expression);
        }

        protected void AddOrderBy(Expression<Func<TEnitity, object>> expression)
        {
            OrderBy = expression;
        } 
        
        protected void AddOrderByDescending(Expression<Func<TEnitity, object>> expression)
        {
            OrderByDescending = expression;
        }

    }
}
