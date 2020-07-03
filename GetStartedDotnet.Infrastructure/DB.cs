using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GetStartedDotnet.Infrastructure
{
    public class DB : IDB
    {
        string _username;
        public string Username => _username;
        string _password;
        public string Password => _password;
        string _host;
        public string Host => _host;
        string _url;
        public string Url => _url;
        string _dbName;
        public string DbName => _dbName;
        HttpClient _client;
        public HttpClient Client => _client;

        public DB(
            string username = "",
            string password = "",
            string host = "",
            string url = "",
            string dbname = "")
        {
            _username = username;
            _password = password;
            _host = host;
            _url = url;
            _dbName = dbname;
            
            var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(_username + ":" + _password));
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
        }

    }
}
