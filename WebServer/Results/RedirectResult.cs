using HTTP.Headers;
using HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.Results
{
   public class RedirectResult : HttpResponse
    {
        public RedirectResult(string location)
            :base(HTTP.Enums.HttpResponseStatusCode.SeeOther)
        {
            this.Headers.AddHeader(new HttpHeader("Location", location));
        }
    }
}
