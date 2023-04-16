using MupiModel.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupiBussines.Services
{
    public interface IContactService
    {
        public Task<List<ContactDto>> GetContactsToSync(DateTime from);
    }
}
