using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataModels
{
    [Keyless]
    public class DMCancelAppointment
    {
        public long userid { get; set; }
        public long parentBioId { get; set; }

        public long biodataId { get; set; }

        public long customerId { get; set; }

        public string Fullname { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public long appointment_id { get; set; }

        public long doctor_office_id { get; set; }

        public long doctor_id { get; set; }

        public long doctortreatment { get; set; }
        public string? doctor_name { get; set; }

        public string? doctor_spec { get; set; }

        public long doctor_schedule { get; set; }

        public int slot { get; set; }

        public string? medic_facility { get; set; }

        public DateTime? app_date { get; set; }

        public int idx_day { get; set; }

        public long doctor_treat_id { get; set; }

        public string treatment { get; set; }

        public long CostumerRelationId { get; set; }

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
