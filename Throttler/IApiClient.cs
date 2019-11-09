using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Throttler
{
    public interface IApiClient
    {
        Task<bool> PostAsync(int userId);
    }
}
