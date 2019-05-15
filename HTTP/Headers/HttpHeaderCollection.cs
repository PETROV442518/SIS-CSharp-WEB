using HTTP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }
        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            headers.Add(header.Key, header);
        }

        public bool ContainsHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            bool contains = false;
            if (headers.Keys.Contains(key))
            {
                contains = true;
            }

            return contains;
        }

        public HttpHeader GetHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            HttpHeader header = headers.FirstOrDefault(a => a.Key == key).Value;
            return header;
        }

        public override string ToString() => string.Join("\r\n", this.headers.Values.Select(header => header.ToString()));

    }
}
