using Project.Domain.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Entities
{
    public class Category : SoftDeleteEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string? Description { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
