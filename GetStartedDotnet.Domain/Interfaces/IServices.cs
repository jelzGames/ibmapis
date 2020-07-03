using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedDotnet.Domain.Interfaces
{
    public interface IServices
    {
        Task<dynamic> CreateAsync(UserModel item);
        Task<dynamic> GetAllAsync();
        Task<dynamic> Get(Guid id);
        Task<dynamic> Update(UserModel item, Guid id);
        Task<dynamic> Delete(Guid id, string rev);

    }
}
