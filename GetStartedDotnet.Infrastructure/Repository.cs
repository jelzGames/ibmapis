using GetStartedDotnet.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GetStartedDotnet.Infrastructure
{
    public class Repository : IRepository
    {
        private readonly IDB _contextDB;
     
        public Repository(
            IDB contextDB = null)
        {
            _contextDB = contextDB;
        }

        public async System.Threading.Tasks.Task<dynamic> CreateAsync(UserModel item)
        {
            string jsonInString = JsonConvert.SerializeObject(item);

            var response = await _contextDB.Client.PostAsync(_contextDB.Client.BaseAddress + _contextDB.DbName, new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return responseJson;
            }
            else if (Equals(response.ReasonPhrase, "Object Not Found")) //need to create database
            {
                var contents = new StringContent("", Encoding.UTF8, "application/json");
                response = await _contextDB.Client.PutAsync(_contextDB.Client.BaseAddress + _contextDB.DbName, contents); //creating database using PUT request
                if (response.IsSuccessStatusCode) //if successful, try POST request again
                {
                    response = await _contextDB.Client.PostAsync(_contextDB.Client.BaseAddress + _contextDB.DbName, new StringContent(jsonInString, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        return responseJson;
                    }
                }

            }

            string msg = "Failure to POST. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
            Console.WriteLine(msg);
            return JsonConvert.SerializeObject(new { msg = "Failure to POST. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase });

        }

        public async System.Threading.Tasks.Task<dynamic> Delete(Guid id, string rev)
        {
            var response = await _contextDB.Client.DeleteAsync(_contextDB.Client.BaseAddress + _contextDB.DbName + "/" + id + "?rev=" + rev);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            string msg = "Failure to PUT. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
            Console.WriteLine(msg);
            return JsonConvert.SerializeObject(new { msg = msg });
        }

        public async System.Threading.Tasks.Task<dynamic> Get(Guid id)
        {
            var response = await _contextDB.Client.GetAsync(_contextDB.Client.BaseAddress + _contextDB.DbName + "/" + id);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            string msg = "Failure to GET. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
            Console.WriteLine(msg);
            return JsonConvert.SerializeObject(new { msg = msg });
        }

        public async System.Threading.Tasks.Task<dynamic> GetAllAsync()
        {
            var response = await _contextDB.Client.GetAsync(_contextDB.Client.BaseAddress + _contextDB.DbName + "/_all_docs?include_docs=true");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else if (Equals(response.ReasonPhrase, "Object Not Found")) //need to create database
            {
                var contents = new StringContent("", Encoding.UTF8, "application/json");
                response = await _contextDB.Client.PutAsync(_contextDB.Client.BaseAddress + _contextDB.DbName, contents); //creating database using PUT request
                if (response.IsSuccessStatusCode) //if successful, try GET request again
                {
                    response = await _contextDB.Client.GetAsync(_contextDB.Client.BaseAddress + _contextDB.DbName + "/_all_docs?include_docs=true");
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }

            }

            string msg = "Failure to GET. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
            Console.WriteLine(msg);
            return JsonConvert.SerializeObject(new { msg = msg });
        }

        public async System.Threading.Tasks.Task<dynamic> Update(UserModel item, Guid id)
        {
            string jsonInString = JsonConvert.SerializeObject(item);

            var response = await _contextDB.Client.PutAsync(_contextDB.Client.BaseAddress + _contextDB.DbName + "/" + id + "?rev=" + item._rev, new StringContent(jsonInString, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            string msg = "Failure to PUT. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
            Console.WriteLine(msg);
            return JsonConvert.SerializeObject(new { msg = msg });
        }
    }
}
