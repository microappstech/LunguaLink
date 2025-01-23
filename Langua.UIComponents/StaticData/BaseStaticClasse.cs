using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Shared.StaticData
{
    public class BaseStaticClasse
    {
        public static string GetPropertyName<TItem>(Expression<Func<TItem, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            MemberExpression memberExpression = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                // Unwrap conversion
                var unaryExpression = (UnaryExpression)expression.Body;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
                throw new ArgumentException("Invalid expression. Must be a simple member access expression.", nameof(expression));

            return memberExpression.Member.Name;
        }
    }
}
