using Project.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Entities.Business
{
    public class Category : SoftDeleteEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string? Description { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
