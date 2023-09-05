using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMDoctorDetail
    {
        [Column("doctor_id")]
        public long? DoctorId { get; set; }
        [Column("doctor_office_id")]
        public long? OfficeId { get; set; }
        [Column("medical_facility_id")]
        public long? MedicalFacilityId { get; set; }
        [Column("specialization")]
        [StringLength(100)]
        public string? Specialization { get; set; }
        [Column("start_date", TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Column("end_date", TypeName = "date")]
        public DateTime? EndDate { get; set; }
        public string? MedicalFacilityName { get; set; }
        public string? MedicalFacilityCategoryName { get; set; }
        public string? MedicalFacilityAddress { get; set; }
        public string? OfficeScheduleDay { get; set; }
        public string? OfficeScheduleStart { get; set; }
        public string? OfficeScheduleEnd { get; set; }
        public decimal? PriceStartFrom { get; set; }

        [Column("created_by")]
        public long? CreatedBy { get; set; }
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
