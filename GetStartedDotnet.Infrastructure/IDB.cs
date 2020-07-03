using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GetStartedDotnet.Infrastructure
{
    public interface IDB
    {
        string Username { get; }
        string Password { get; }
        string Host { get; }
        string Url { get; }
        string DbName { get; }
        HttpClient Client { get; }


    }
}
