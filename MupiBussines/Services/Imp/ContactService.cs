using MupiModel.Dtos;
using MupiSource.DbContext;

namespace MupiBussines.Services.Imp
{
    public class ContactService : IContactService
    {
        private readonly MupiDbContext _context;

        public ContactService(MupiDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContactDto>> GetContactsToSync(DateTime from)
        {
            var contactsQuery = _context.Contact.Where(x=>x.Fecha>=from).ToList();

            return contactsQuery.Select(x=> new ContactDto { 
                Nombres=x.Nombres,
                Apellidos=x.Apellidos,
                Email=x.Email,
                Telefono=x.Telefono,
                Direccion=x.Direccion,
                IdMupiContacto=x.Id.ToString(),
            }).ToList();
        }
    }
}
