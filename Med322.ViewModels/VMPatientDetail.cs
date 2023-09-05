using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.ViewModels
{
    public class VMPatientDetail
    {

        public long? UserId { get; set; }
        // Mbiodata

       
        public string? Fullname { get; set; }

        public string? namauser { get; set; }
        // M CUstomer
        public long? CustomerId { get; set; }

        public long? ParentBioId { get; set; }

        public long? BiodataId { get; set; }

        public DateTime? Dob { get; set; }

        public string? Gender { get; set; }

        public string? RhesusType { get; set; }

        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }

        public int? Age { get; set; }


        //    // m customer member    
        public long CustomerMemberId { get; set; }


        //// m customer relation
        public long CustomerRelationId { get; set; }
        public string? NameRelation { get; set; }

        //   // blood group     
        public long? BloodGroupId { get; set; }
        public string? Code { get; set; }

        //        //user
        //        public long? UserId { get; set; }

        // t_appointment

        public long? AppointmentId { get; set; }

        public int? AppointmentSum { get; set; }

        // t_customer_chat
        public long? ChatId { get; set; }
        
        public int? ChatSum { get; set; }
     
        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDelete { get; set; }
        public string? MobilePhone { get; set; }

        
    }

}
