using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataModels
{
    public class VWLocations
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }

        public string? Wilayah { get; set; }

        public long? ParentId { get; set; }
        public long? LocationLevelId { get; set; }

        public string? NameLocationLevel { get; set; }

        public string? Abbreviation { get; set; }

        public string? Fullinfo { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDelete { get; set; }
    }
}
