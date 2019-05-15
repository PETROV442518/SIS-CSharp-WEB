using HTTP.Requests.Contracts;
using HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using WebServer.Results;

namespace Demo
{
   public class HomeController
    {
        public IHttpResponse Index( IHttpRequest request)
        {
            string content = "<h1>Hello, World!</h1>";
            return new HtmlResult(content, HTTP.Enums.HttpResponseStatusCode.OK);
        }
    }
}
