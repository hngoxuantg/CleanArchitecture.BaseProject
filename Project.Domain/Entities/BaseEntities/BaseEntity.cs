using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Domain.Entities.BaseEntities
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual Guid Id { get; set; } = Guid.NewGuid();

        public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Guid? CreateBy { get; set; }

        [ForeignKey("CreateBy")]
        public virtual User? CreatedByUser { get; set; }

        public virtual DateTime? UpdateAt { get; set; }

        public virtual Guid? UpdateBy { get; set; }

        [ForeignKey("UpdateBy")]
        public virtual User? UpdatedByUser { get; set; }

        public virtual void SetCreated(Guid? updatedBy)
        {
            CreatedAt = DateTime.UtcNow;
            CreateBy = updatedBy;
        }

        public virtual void SetUpdated(Guid? updatedBy)
        {
            UpdateAt = DateTime.UtcNow;
            UpdateBy = updatedBy;
        }
    }
}
