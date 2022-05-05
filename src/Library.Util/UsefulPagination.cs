using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Useful
{
    public class UsefulPagination
    {
        public IQueryable<TSource> GetDadosDaPagina<TSource>(
            IQueryable<TSource> source,
            int page,
            int pageSize)
        {
            return Queryable.Take<TSource>(Queryable.Skip<TSource>(source, (page - 1) * pageSize), pageSize);
        }

        public IEnumerable<TSource> GetDadosDaPagina<TSource>(
            IEnumerable<TSource> source,
            int page,
            int pageSize)
        {
            return source.Skip<TSource>((page - 1) * pageSize).Take<TSource>(pageSize);
        }

        public int GetTotalDePaginas(int total, int pageSize)
        {
            return (int)Math.Ceiling((double)total / (double)pageSize);
        }
    }
}
