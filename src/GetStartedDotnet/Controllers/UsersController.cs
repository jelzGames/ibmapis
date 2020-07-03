using Microsoft.AspNetCore.Mvc;
using GetStartedDotnet.Models;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System;
using GetStartedDotnet.Domain.Interfaces;

namespace GetStartedDotnet.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IServices _cloudantService;

        public UsersController(HtmlEncoder htmlEncoder, IServices cloudantService = null)
        {
            _cloudantService = cloudantService;
            _htmlEncoder = htmlEncoder;
        }

        // GET: api/users
        [HttpGet]
        public async Task<dynamic> GetAll()
        {
            if (_cloudantService == null)
            {
                return new string[] { "No database connection" };
            }
            else
            {
                return await _cloudantService.GetAllAsync();
            }
        }

        // GET: api/users
        [HttpGet("{id}")]
        public async Task<dynamic> Get(Guid id)
        {
            if (_cloudantService == null)
            {
                return new string[] { "No database connection" };
            }
            else
            {
                return await _cloudantService.Get(id);
            }
        }

        // POST api/users
        [HttpPost]
        public async Task<dynamic> Post([FromBody]Domain.Interfaces.UserModel user)
        {
            if (_cloudantService != null)
            {
                user._id = Guid.NewGuid();

                var response = await _cloudantService.CreateAsync(user);
                Console.WriteLine("POST RESULT " + response);

                return response;
            }

            return new { };
        }

        // POST api/users
        [HttpPut("{id}")]
        public async Task<dynamic> Put([FromBody]UserModel user, Guid id, string rev)
        {
            if (_cloudantService != null)
            {
                user._rev = rev;
                var response = await _cloudantService.Update(user, id);
                Console.WriteLine("PUT RESULT " + response);
                return response;
            }

            return new { };
        }

        // POST api/users
        [HttpDelete("{id}")]
        public async Task<dynamic> Delete(Guid id, string rev)
        {
            if (_cloudantService != null)
            {
                var response = await _cloudantService.Delete(id, rev);
                Console.WriteLine("DELETE RESULT " + response);

                return response;
            }

            return new { };
        }
    }
}
