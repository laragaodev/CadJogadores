using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Exceptions
{
    public class ValidacaoException: Exception
    {
        public ValidacaoException(string msg) : base(msg)
        {
        }
    }
}
