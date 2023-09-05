using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DABiodata
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DABiodata(Med322_BContext _db)
        {
            db = _db;
        }
        private VMBiodata? GetById(int id)
        {
            return (from b in db.MBiodata
                    where b.IsDelete == false && b.Id == id
                    select new VMBiodata
                    {

                        Id = b.Id,
                        Fullname = b.Fullname,
                        MobilePhone = b.MobilePhone,
                        Image = b.Image,
                        ImagePath = b.ImagePath,
                        CreatedBy = b.CreatedBy,
                        CreatedOn = b.CreatedOn,
                        ModifiedBy = b.ModifiedBy,
                        ModifiedOn = b.ModifiedOn,
                        DeletedBy = b.DeletedBy,
                        DeletedOn = b.DeletedOn,
                        IsDelete = b.IsDelete

                    }).FirstOrDefault();
        }

        public VMResponse GetAll()
        {
            try
            {
                List<VMBiodata> Biodata = (
                from b in db.MBiodata
                where b.IsDelete == false
                select new VMBiodata
                {
                    Id = b.Id,
                    Fullname = b.Fullname,
                    MobilePhone = b.MobilePhone,
                    Image = b.Image,
                    ImagePath = b.ImagePath,
                    CreatedBy = b.CreatedBy,
                    CreatedOn = b.CreatedOn,
                    ModifiedBy = b.ModifiedBy,
                    ModifiedOn = b.ModifiedOn,
                    DeletedBy = b.DeletedBy,
                    DeletedOn = b.DeletedOn,
                    IsDelete = b.IsDelete

                }).ToList();

                if (Biodata.Count < 1)
                {
                    response.Message = "Biodata table has no data!";
                    response.Success = false;
                    return response;
                }
                response.data = Biodata;
                response.Message = "Biodata data successfully fetched!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse Get(int id)
        {
            try
            {
                if (id < 1)
                {
                    response.Message = "No Biodata Id was submitted";
                    response.Success = false;
                    return response;
                }

                // get all the category data
                response.data = GetById(id);

                // check if data is null
                if (response.data == null)
                {
                    response.Message = $"Biodata with ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"Biodata with ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse CreateUpdate(VMBiodata inputB)
        {
            try
            {

                MBiodatum data = new MBiodatum();

                data.Fullname = inputB.Fullname;
                data.MobilePhone = inputB.MobilePhone;
                data.Image = inputB.Image;
                data.ImagePath = inputB.ImagePath;

                if (inputB.Id < 1)
                {

                    data.CreatedBy = inputB.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);
                    response.Message = "New Biodata Data successfully inserted!";
                }
                else
                {
                    VMBiodata? biodata = GetById((int)inputB.Id);

                    if (biodata == null)
                    {
                        response.Success = false;
                        response.Message = " Biodata data does not exist!";
                        return response;
                    }
                    else
                    {
                        data.Id = biodata.Id;

                        data.Fullname = inputB.Fullname ?? biodata.Fullname;
                        data.MobilePhone = inputB.MobilePhone ?? biodata.MobilePhone;
                        data.Image = inputB.Image ?? biodata.Image;
                        data.ImagePath = inputB.ImagePath ?? inputB.ImagePath;

                        data.CreatedBy = biodata.CreatedBy;
                        data.CreatedOn = biodata.CreatedOn;

                        data.ModifiedBy = inputB.ModifiedBy;
                        data.ModifiedOn = DateTime.Now;

                        db.Update(data);
                        response.Message = " Biodata data Data successfully updated!";
                    }
                }

                // Add new record to table biodata

                db.SaveChanges();

                response.data = data;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse Delete(int id, int userid)
        {
            try
            {
                if (id < 1)
                {
                    response.Message = "No Biodata Id was submitted";
                    response.Success = false;
                    return response;
                }

                MBiodatum data = new MBiodatum();
                VMBiodata? biodata = GetById(id);

                if (biodata == null)
                {
                    response.Success = false;
                    response.Message = "Biodata data does not exist!";

                    return response;
                }

                data.Id = biodata.Id;
                data.Fullname = biodata.Fullname;
                data.MobilePhone = biodata.MobilePhone;
                data.Image = biodata.Image;
                data.ImagePath = biodata.ImagePath;
                data.CreatedBy = biodata.CreatedBy;
                data.CreatedOn = biodata.CreatedOn;
                data.ModifiedBy = biodata.ModifiedBy;
                data.ModifiedOn = biodata.ModifiedOn;
                data.DeletedBy = userid;
                data.DeletedOn = DateTime.Now;
                data.IsDelete = true;

                db.Update(data);
                db.SaveChanges();

                response.data = data;
                response.Message = $"Biodata ID {id} Successfully deleted!";

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }
    }
}
