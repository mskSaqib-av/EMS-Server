using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JWTServer.Model
{
    [Table("Users")]
    public class UserLogin
    {
        [Key]       
        public Guid Id { get; set; }

        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        public string HashPassword { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        public string? Phone { get; set; }

        public DateTime? DOB { get; set; }

        public bool Active { get; set; }

        public string Action { get; set; }

        public DateTime created_at { get; set; }
        
        public Guid created_by { get; set; }
        
        public DateTime? updated_at { get; set; }
        
        public Guid? updated_by { get; set; }
        
        public DateTime? deleted_at { get; set; }
        
        public Guid? deleted_by { get; set; }

    }
}
