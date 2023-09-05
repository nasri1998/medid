using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMCancelAppointment
    {

        public long userid { get; set; }
        public long parentBioId { get; set; }

        public long biodataId { get; set; }

        public long customerId { get; set; }

        public string Fullname { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public long AppointmentId { get; set; }

        public long DoctorOfficeId { get; set; }

        public long? DoctorId { get; set; }

        public long doctortreatment { get; set; }
        public string? DoctorName { get; set; }

        public string? DoctorSpec { get; set; }

        public long DoctorSchedule { get; set; }

        public int Slot { get; set; }

        public string? MedicFacility { get; set; }

        public DateTime? AppDate { get; set; }

        public int IdxDay { get; set; }

        public long DoctorTreatId { get; set; }

        public string? Treatment { get; set; }

        public long CostumerRelationId { get; set; }

        public string? RelationName { get; set; }
        public long? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDelete { get; set; }
    }
}

