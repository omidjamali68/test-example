using System;

namespace Cooking.Infrastructure.Application
{
    public class Pagination
    {
        private Pagination(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber));

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; }
        public int PageSize { get; }

        public static Pagination Of(int pageNumber, int pageSize)
        {
            return new Pagination(pageNumber, pageSize);
        }
    }
}