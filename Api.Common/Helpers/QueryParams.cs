using System;
using System.Collections.Generic;
using System.Text;

namespace Sisc.Api.Common.Helpers
{
    public class BaseQueryParams
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 1000;
        public List<OrderByColumn> OrderByColumns { get; set; } = new List<OrderByColumn>();
    }
}
