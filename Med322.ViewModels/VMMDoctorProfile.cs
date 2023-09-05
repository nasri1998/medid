using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMMDoctorProfile
    {
        [Column("id")]
        public long Id { get; set; }

        public long biodataId { get; set; }
        [Column("fullname")]
     
        [StringLength(255)]        
        public string? Fullname { get; set; }
        
        //nama tindakan medis
        public long treatmentId { get; set; }
        
        [StringLength(50)]
        public string? treatmentName { get; set; }

        //riwayat praktek
        public long officeId { get; set; }

        public long? MedicalFacilityId { get; set; }

        [StringLength(50)]
        public string? MedicalFacilityName { get; set; }

        [StringLength(100)]
        public string? location { get; set; }

        //nama spesialisasi
        public string? specializationName { get; set; }

        //id spesialisasi
        public int? spesializationId { get; set; }

        //untuk nampung string spesialisasi sari m_spesialisasi
        public string? Str { get; set; }

        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }

        //pendidikan
        public long? EducationLevelId { get; set; }
        
        [StringLength(100)]        
        public string? InstitutionName { get; set; }
        
        [StringLength(100)]        
        public string? Major { get; set; }
                
        [StringLength(4)]        
        public string? edEndYear { get; set; }


        [Column("mobile_phone")]
        [StringLength(15)]        
        public string? MobilePhone { get; set; }

        [Column("image")]
        [MaxLength(1)]
        public byte[]? Image { get; set; }

        [Column("image_path")]
        [StringLength(255)]        
        public string? ImagePath { get; set; }

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

// tabel yg dipake m_biodata m_doctor m_specialization m_doctor_education m_education_level t_doctor_office () t_doctor_treatment
