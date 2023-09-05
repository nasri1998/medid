using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMViewFindDoctor
    {
        //biodata
        [Column("id")]
        public long Id { get; set; }

        [Column("fullname")]
        [StringLength(255)]
        public string? Fullname { get; set; }

        [Column("image_path")]
        [StringLength(255)]
        public string? ImagePath { get; set; }

        //doctor
        [Column("doctor_id")]
        public long? DoctorId { get; set; }

        //tempat praktek
        [Column("doctor_office_id")]
        public long? DoctorOfficeId { get; set; }

        [Column("medical_facility_id")]
        public long MedicalFacilityId { get; set; }

        [Column("medical_facility_name")]
        [StringLength(50)]
        public string? MedicalFacilityName { get; set; }

        [Column("specialization")]
        [StringLength(100)]
        public string? Specialization { get; set; }

        [Column("experience")]
        public int? Experience { get; set; }

        [Column("end_date", TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [Column("doctor_treatment_id")]
        public long? DoctorTreatmentId { get; set; }

        [Column("doctor_treatment_name")]
        public string? DoctorTreatmentName { get; set; }

        [Column("location_id")]
        public long? LocationId { get; set; }

        [Column("location_name")]
        public string? LocationName { get; set; }

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
