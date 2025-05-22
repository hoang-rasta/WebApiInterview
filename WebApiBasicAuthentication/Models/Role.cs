using System.ComponentModel.DataAnnotations;

namespace WebApiBasicAuthentication.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Navigation property for the many-to-many relationship with User
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
