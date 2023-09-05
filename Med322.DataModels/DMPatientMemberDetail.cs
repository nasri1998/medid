using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataModels
{
    [Keyless]

    public class DMPatientMemberDetail
    {
        public long userid { get; set; }
        public long parentBioId { get; set; }

        public string? namauser { get; set; }
        public long biodataId { get; set; }

        public long customerId { get; set; }

        public string fullName { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public long BloodGroupId { get; set; }

        public string? RhesusType { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public int Age { get; set; }

        public int? Appointment { get; set; }

        public int? Chat { get; set; }

        public long CostumerRelationId { get; set; }
        public long CostumerMemberId { get; set; }

        public string? RelationName { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? DeletedBy { get; set; }
        
        public DateTime? DeletedOn { get; set; }
        
        public bool IsDelete { get; set; }


    }
}
