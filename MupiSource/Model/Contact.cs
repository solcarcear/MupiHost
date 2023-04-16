using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MupiSource.Model
{
    [Table("Contact")]
    public class Contact
    {
        public long Id { get; set; }

        [StringLength(maximumLength: 100)]
        public string Nombres { get; set; }

        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
        [StringLength(maximumLength: 100)]
        public string Telefono { get; set; }
        [StringLength(maximumLength: 100)]
        public string Direccion { get; set; }
        public DateTime Fecha { get; set; }

    }
}
