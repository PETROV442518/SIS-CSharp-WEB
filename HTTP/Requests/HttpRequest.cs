using System;
using System.Collections.Generic;
using HTTP.Common;
using HTTP.Enums;
using HTTP.Exceptions;
using HTTP.Headers;
using System.Linq;
using HTTP.Requests.Contracts;

namespace HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }
        public string Url { get; private set; }
        public Dictionary<string, object> FormData { get; }
        public Dictionary<string, object> QueryData { get; }
        public IHttpHeaderCollection Headers { get; }
        public HttpRequestMethod RequestMethod { get; private set; }

        private bool IsValidRequestLine(string[] requestLine)
        {
            bool result = false;
            if ((requestLine.Length == 3) && (requestLine[2] == GlobalConstants.HttpOneProtocolFragment))
            {
                result = true;
            }
            return result;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            CoreValidator.ThrowIfNullOrEmpty(queryString, nameof(queryString));
            return true;
        }

        private bool HasQueryString()
        {
            return this.Url.Split('?').Length > 1;
        }

        private IEnumerable<string> ParsePlainRequestHeaders(string[] requestLines)
        {
            for (int i = 1; i < requestLines.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(requestLines[i]))
                {
                    yield return requestLines[i];
                }
            }
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            bool parseResult = HttpRequestMethod.TryParse(requestLine[0], true, out HttpRequestMethod method);
            if (!parseResult)
            {
                throw new BadRequestException(
                    string.Format(GlobalConstants.UnsupportedHttpMethodExceptionMessage, requestLine[0]));
            }
            this.RequestMethod = method;
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?')[0];
        }

        private void ParseHeaders(string[] requestContent)
        {
            foreach (var headerString in requestContent)
            {
                var line = headerString.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                var key = line[0];

                var value = line[1];
                HttpHeader header = new HttpHeader(key, value);
                this.Headers.AddHeader(header);

                //plainHeaders.Select(plainHeader => plainHeader.Split(new[] { ':', ' ' }
                //, StringSplitOptions.RemoveEmptyEntries))
                //.ToList()
                //.ForEach(headerKeyValuePair => this.Headers.AddHeader(new HttpHeader(headerKeyValuePair[0], headerKeyValuePair[1])));
            }
        }

        private void ParseRequestQueryParameters()
        {
            if (this.HasQueryString())
            {
                this.Url.Split('?', '#')[1]
                    .Split('&')
                    .Select(plainQueryParameter => plainQueryParameter.Split('='))
                    .ToList()
                    .ForEach(queryParameterKeyValuePair =>
                        this.QueryData.Add(queryParameterKeyValuePair[0], queryParameterKeyValuePair[1]));
            }
        }

        private void ParseRequestFormDataParameters(string requestBody)
        {
            if (!string.IsNullOrEmpty(requestBody))
            {
                //TODO: Parse Multiple Parameters By Name
                requestBody
                    .Split('&')
                    .Select(plainQueryParameter => plainQueryParameter.Split('='))
                    .ToList()
                    .ForEach(queryParameterKeyValuePair =>
                        this.FormData.Add(queryParameterKeyValuePair[0], queryParameterKeyValuePair[1]));
            }
        }

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseRequestQueryParameters();
            this.ParseRequestFormDataParameters(requestBody); //TODO: Split
        }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestString = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            string[] requestLine = splitRequestString[0].Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(this.ParsePlainRequestHeaders(splitRequestString).ToArray());
            //this.ParseCookies();

            this.ParseRequestParameters(splitRequestString[splitRequestString.Length - 1]);

        }
    }
}
