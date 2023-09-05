using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Med322.DataModels
{
    [Keyless]
    public class VWDoctorDetail
    {
        [Column("doctor_id")]
        public long DoctorId { get; set; }
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
        [Column("medical_facility_name")]
        [StringLength(50)]
        public string? MedicalFacilityName { get; set; }
        [Column("medical_facility_category_name")]
        [StringLength(50)]
        public string? MedicalFacilityCategoryName { get; set; }
        [Column("medical_facility_address", TypeName = "text")]
        public string? MedicalFacilityAddress { get; set; }
        [Column("day")]
        [StringLength(10)]
        public string? OfficeScheduleDay { get; set; }
        [Column("time_schedule_start")]
        [StringLength(10)]
        public string? OfficeScheduleStart { get; set; }
        [Column("time_schedule_end")]
        [StringLength(10)]
        public string? OfficeScheduleEnd { get; set; }
        [Column("price_start_from", TypeName = "decimal(18, 0)")]
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
