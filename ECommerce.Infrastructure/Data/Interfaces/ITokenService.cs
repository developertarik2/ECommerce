using ECommerce.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
