using Project.Domain.Entities.Identity_Auth;

namespace Project.Domain.Entities.BaseEntities
{
    public abstract class SoftDeleteEntity : BaseEntity
    {
        public bool IsDeleted { get; set; } = false;
        public Guid? DeleteBy { get; set; }
        public DateTime? DeleteAt { get; set; }
        public virtual User? DeleteByUser { get; set; }

        public virtual void SetDeleted(Guid? deleteBy)
        {
            IsDeleted = true;
            DeleteAt = DateTime.UtcNow;
            DeleteBy = deleteBy;
        }
        public virtual void UndoDelete()
        {
            IsDeleted = false;
            DeleteBy = null;
            DeleteAt = null;
        }
    }
}
