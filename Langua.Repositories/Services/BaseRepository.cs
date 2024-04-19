
using System.Linq.Dynamic.Core;
using Langua.DataContext.Data;
using Langua.Repositories.Interfaces;
using Langua.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public class BaseRepositoryCrud<T> : IRepositoryCrudBase<T> where T : class
    {
        private readonly LanguaContext _context;
        public BaseRepositoryCrud(LanguaContext context)
        {
            _context = context;
        }
        void Main()
        {
            // Define a list of integers
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

            // Define the dynamic query expression
            string expression = "x => x > 3";

            // Execute the dynamic query using the Dynamic LINQ library
            IEnumerable<int> results = numbers.AsQueryable().Where(expression);

            // Output the results
            Console.WriteLine("Numbers greater than 3:");
            foreach (int number in results)
            {
                Console.WriteLine(number);
            }
        }
        public Result<T> Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
                return new Result<T>(true,entity);
            }
            catch
            {
                throw new Exception("Creation Failed");
            }
        }

        public Result<T> Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
                return new Result<T>(true);
            }
            catch(Exception ex)
            {
                return new Result<T>(false,Error:ex.Message);
            }
        }

        public Result<IQueryable<T>> GetAll()
        {
            try
            {
                IQueryable<T> result = _context.Set<T>().AsQueryable();
                return new Result<IQueryable<T>>(true, result);
            }catch(Exception ex)
            {
                return new Result<IQueryable<T>>(false, null, Error:ex.Message);
            }
        }

        public Result<IQueryable<T>> GetByExpression(string expression)
        {
            try
            {

                ParameterExpression paramType = Expression.Parameter(typeof(T), expression);
                ParameterExpression paramExpr = Expression.Parameter(typeof(T));
                var arrProp = expression.Split('.').ToList();
                var result = _context.Set<T>().AsQueryable().Where(expression);


                //var predicate = System.Linq.Dynamic.DynamicExpression.ParseLambda();
                //var result = _context.Set<T>().AsQueryable();
                //result = result.ToList().Where(expressionWithValue);
                return new Result<IQueryable<T>>(true, result);
            }catch(Exception ex)
            {
                return new Result<IQueryable<T>>(false, null,ex.Message);
            }
        }

        public Result<T> GetById(int id)
        {
            T result = _context.Set<T>().Find(id);
            return new Result<T>(true, result);
        }

        public Result<T> Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return new Result<T>(true, entity);
            }
            catch(Exception ex)
            {
                return new Result<T>(true, entity, Error: ex.Message); ;
            }
        }
    }
}
