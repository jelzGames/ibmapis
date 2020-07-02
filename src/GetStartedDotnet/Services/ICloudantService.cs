using GetStartedDotnet.Models;
using System;
using System.Threading.Tasks;

namespace GetStartedDotnet.Services
{
    public interface ICloudantService
    {
        Task<dynamic> CreateAsync(UserModel item);
        Task<dynamic> GetAllAsync();
        Task<dynamic> Get(Guid id);
        Task<dynamic> Update(UserModel item, Guid id);
        Task<dynamic> Delete(Guid id, string rev);

    }
}