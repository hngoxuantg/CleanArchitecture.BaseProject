using Microsoft.AspNetCore.Identity;

namespace Project.Domain.Entities
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
    }
}
