﻿using System.Collections.Generic;

namespace TestExample.Infrastructure.Application
{
    public class PageResult<T>
    {
        public PageResult(IList<T> elements, int totalElements)
        {
            Elements = elements;
            TotalElements = totalElements;
        }

        public IList<T> Elements { get; }
        public int TotalElements { get; }
    }
}