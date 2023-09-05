using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DASpecialization
    {
        private readonly Med322_BContext db;
        private readonly VMResponse response = new VMResponse();
        private DADoctors doc;
        public DASpecialization(Med322_BContext _db)
        {
            db = _db;
        }

        public VMResponse GetAll()
        {

            try
            {
                List<VMMSpecialization> specialization = (
                    from s in db.MSpecializations
                    where s.IsDelete == false
                    select new VMMSpecialization
                    {
                        Id = s.Id,
                        Name = s.Name,

                        CreatedBy = s.CreatedBy,
                        CreatedOn = s.CreatedOn,
                        ModifiedBy = s.ModifiedBy,
                        ModifiedOn = s.ModifiedOn,

                        DeletedBy = s.DeletedBy,
                        DeletedOn = s.DeletedOn,
                        IsDelete = s.IsDelete
                    }
                ).ToList();

                response.data = specialization;
                response.Message = "success to get data";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public VMMSpecialization GetById(long id)
        {
            VMMSpecialization? specialization = new VMMSpecialization();
            try
            {
                specialization = (
                    from s in db.MSpecializations
                    where s.IsDelete == false && s.Id == id
                    select new VMMSpecialization
                    {
                        Id = s.Id,
                        Name = s.Name,

                        CreatedBy = s.CreatedBy,
                        CreatedOn = s.CreatedOn,
                        ModifiedBy = s.ModifiedBy,
                        ModifiedOn = s.ModifiedOn,

                        DeletedBy = s.DeletedBy,
                        DeletedOn = s.DeletedOn,
                        IsDelete = s.IsDelete
                    }
                ).FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return specialization;
        }
    }
}
