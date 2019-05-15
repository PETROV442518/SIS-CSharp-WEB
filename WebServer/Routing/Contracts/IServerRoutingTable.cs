using HTTP.Enums;
using HTTP.Requests;
using HTTP.Requests.Contracts;
using HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.Routing.Contracts
{
    public interface IServerRoutingTable
    {
        void Add(HttpRequestMethod method, string path, Func<IHttpRequest, IHttpResponse> func);

        bool Contains(HttpRequestMethod method, string path);

        Func<IHttpRequest, IHttpResponse> Get(HttpRequestMethod requestMethod, string path);
    }
}
