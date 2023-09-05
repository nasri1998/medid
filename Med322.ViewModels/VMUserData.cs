using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMUserData
    {
        [Key]
        [Column("id")]
        public long UserId { get; set; }
        [Column("biodata_id")]
        public int? BiodataId { get; set; }

        [Column("role_id")]
        public long? RoleId { get; set; }
        [Column("role_name")]
        [StringLength(20)]
        public string? RoleName { get; set; }
        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }
        [Column("password")]
        [StringLength(255)]
        public string? Password { get; set; }
        [Column("login_attempt")]
        public int? LoginAttempt { get; set; }
        [Column("is_locked")]
        public bool? IsLocked { get; set; }
        [Column("last_login", TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        public string? Fullname { get; set; }
        [Column("mobile_phone")]
        [StringLength(15)]
        public string? MobilePhone { get; set; }
        [Column("image")]
        [MaxLength(1)]
        public byte[]? Image { get; set; }
        [Column("image_path")]
        [StringLength(255)]
        public string? ImagePath { get; set; }
        public long? DoctorId { get; set; }
    }

}