using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMPatientProfile
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("biodata_id")]
        public long? BiodataId { get; set; }

        [Column("fullname")]
        [StringLength(255)]
        public string? Fullname { get; set; }

        [Column("dob", TypeName = "date")]
        public DateTime? Dob { get; set; }
        [Column("mobile_phone")]
        [StringLength(15)]
        public string? MobilePhone { get; set; }
        [Column("created_by")]
        public long CreatedBy { get; set; }
        [Column("created_on", TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column("modified_by")]
        public long? ModifiedBy { get; set; }
        [Column("modified_on", TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        [Column("deleted_by")]
        public long? DeletedBy { get; set; }
        [Column("deleted_on", TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column("is_delete")]
        public bool IsDelete { get; set; }
    }
}
