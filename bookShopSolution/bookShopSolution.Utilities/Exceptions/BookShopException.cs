using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Utilities.Exceptions
{
    public class BookShopException: Exception
    {
        public BookShopException()
        {
        }

        public BookShopException(string message)
            : base(message)
        {
        }

        public BookShopException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
