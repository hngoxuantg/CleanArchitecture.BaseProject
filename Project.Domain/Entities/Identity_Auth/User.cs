using Microsoft.AspNetCore.Identity;
using Project.Domain.Entities.System_Logs;

namespace Project.Domain.Entities.Identity_Auth
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public byte[]? RowVersion { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
    }
}
