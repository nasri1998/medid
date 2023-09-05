using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMTblMBloodGroup
    {
       
        [Column("id")]
        public long Id { get; set; }

        [Column("code")]
        [StringLength(5)]
        public string? Code { get; set; }

        public string? Fullname { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string? Description { get; set; }

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
