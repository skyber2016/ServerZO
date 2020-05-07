using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class BadRequestException : Exception
    {
        public HttpStatusCode Code => HttpStatusCode.BadRequest;
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
