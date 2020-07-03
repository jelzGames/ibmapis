using GetStartedDotnet.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetStartedDotnet.Domain.Services
{
    public class Services : IServices
    {
        private readonly IRepository _repository;

        public Services(IRepository repository)
        {
            _repository = repository;
        }


        public async System.Threading.Tasks.Task<dynamic> CreateAsync(UserModel item)
        {
            return await _repository.CreateAsync(item);
        }

        public async System.Threading.Tasks.Task<dynamic> Delete(Guid id, string rev)
        {
            return await  _repository.Delete(id, rev);
        }

        public async System.Threading.Tasks.Task<dynamic> Get(Guid id)
        {
            return await  _repository.Get(id);
        }

        public async System.Threading.Tasks.Task<dynamic> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async System.Threading.Tasks.Task<dynamic> Update(UserModel item, Guid id)
        {
            return await _repository.Update(item, id);
        }
    }
}
