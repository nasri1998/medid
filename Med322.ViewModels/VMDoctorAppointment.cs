using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMDoctorAppointment
    {
        [Column("id")]
        public long Id { get; set; }

        //name
        [Column("customer_id")]
        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        //nama faskes
        [Column("doctor_office_id")]
        public long? DoctorOfficeId { get; set; }
        public string? OfficeName { get; set; }
        public long? DoctorId { get; set; }

        //jadwal
        [Column("doctor_office_schedule_id")]
        public long? DoctorOfficeScheduleId { get; set; }
        public string? DayName { get; set;}
        public string? StartHour { get; set; }
        public string? EndHour { get; set; }

        //treatment
        [Column("doctor_office_treatment_id")]
        public long? DoctorOfficeTreatmentId { get; set; }
        public string? TreatmentName { get;set; }

        // tanggal
        [Column("appointment_date", TypeName = "datetime")]
        public DateTime? AppointmentDate { get; set; }

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
