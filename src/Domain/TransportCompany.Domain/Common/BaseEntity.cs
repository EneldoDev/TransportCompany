using System.ComponentModel.DataAnnotations;

namespace TransportCompany.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
