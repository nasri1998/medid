using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Med322.DataAccess
{
    public class DABloodGroup
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DABloodGroup(Med322_BContext _db)
        {
            db = _db;
        }

        public VMResponse GetAll()
        {
            try
            {
                List<VMTblMBloodGroup> bloodGroups = (
                    from bg in db.MBloodGroups
                    join u in db.MUsers on bg.ModifiedBy equals u.Id into userGroup
                    from u in userGroup.DefaultIfEmpty()
                    join b in db.MBiodata on u != null ? u.BiodataId : null equals b.Id into biodataGroup
                    from b in biodataGroup.DefaultIfEmpty()
                    where bg.IsDelete == false
                    select new VMTblMBloodGroup
                    {
                        Id = bg.Id,
                        Code = bg.Code,
                        Fullname = b.Fullname,
                        Description = bg.Description,
                        CreatedBy = bg.CreatedBy,
                        CreatedOn = bg.CreatedOn,
                        ModifiedBy = bg.ModifiedBy,
                        ModifiedOn = bg.ModifiedOn,
                        DeletedBy = bg.DeletedBy,
                        DeletedOn = bg.DeletedOn,
                        IsDelete = bg.IsDelete
                    }).ToList();

                if (bloodGroups.Count < 1)
                {
                    response.Message = "Blood Group table has no data!";
                    response.Success = false;
                    return response;
                }
                response.data = bloodGroups;
                response.Message = "Blood Group data successfully fetched!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        private VMTblMBloodGroup? GetById(int id)
        {
            return (from bg in db.MBloodGroups
                    where bg.IsDelete == false
                    && bg.Id == id
                    select new VMTblMBloodGroup
                    {

                        Id = bg.Id,
                        Code = bg.Code,
                        Description = bg.Description,
                        CreatedBy = bg.CreatedBy,
                        CreatedOn = bg.CreatedOn,
                        ModifiedBy = bg.ModifiedBy,
                        ModifiedOn = bg.ModifiedOn,
                        DeletedBy = bg.DeletedBy,
                        DeletedOn = bg.DeletedOn,
                        IsDelete = bg.IsDelete

                    }).FirstOrDefault();
        }

        public VMResponse Get(int id)
        {
            try
            {
                if (id < 1)
                {
                    response.Message = "No Blood Group Id was submitted";
                    response.Success = false;
                    return response;
                }

                // get all the category data
                response.data = GetById(id);

                // check if data is null
                if (response.data == null)
                {
                    response.Message = $"Blood Group with ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"Blood Group with ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse GetByFilter(string filter)
        {
            try
            {
                List<VMTblMBloodGroup> data = (from bg in db.MBloodGroups
                                               where bg.IsDelete == false
                                               && bg.Code.ToLower().Contains(filter.ToLower())
                                               select new VMTblMBloodGroup
                                               {

                                                   Id = bg.Id,
                                                   Code = bg.Code,
                                                   Description = bg.Description,
                                                   CreatedBy = bg.CreatedBy,
                                                   CreatedOn = bg.CreatedOn,
                                                   ModifiedBy = bg.ModifiedBy,
                                                   ModifiedOn = bg.ModifiedOn,
                                                   DeletedBy = bg.DeletedBy,
                                                   DeletedOn = bg.DeletedOn,
                                                   IsDelete = bg.IsDelete

                                               }).ToList();
                response.Success = true;
                response.Message = " Search data success!";
                response.data = data;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                response.data = null;
                return response;
            }
        }
        public VMResponse CreateUpdate(VMTblMBloodGroup inputbg)
        {
            try
            {
                MBloodGroup data = new MBloodGroup();

                data.Code = inputbg.Code;
                data.Description = inputbg.Description;

                // Check if the code already exists
                if (db.MBloodGroups.Any(bg => bg.Code == inputbg.Code && bg.Id != inputbg.Id && bg.IsDelete == false))
                {
                    response.Success = false;
                    response.Message = "Blood Group with the same code already exists!";
                    return response;
                }

                if (inputbg.Id < 1)
                {

                    data.CreatedBy = inputbg.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);
                    response.Message = "New Blood Group Data successfully inserted!";
                }
                else
                {
                    VMTblMBloodGroup? dataBlood = GetById((int)inputbg.Id);

                    if (dataBlood == null)
                    {
                        response.Success = false;
                        response.Message = " Blood Group data does not exist!";
                        return response;    
                    }
                    else
                    {
                        data.Id = dataBlood.Id;

                        data.CreatedBy = dataBlood.CreatedBy;
                        data.CreatedOn = dataBlood.CreatedOn;

                        data.ModifiedBy = inputbg.ModifiedBy;
                        data.ModifiedOn = DateTime.Now;

                        db.Update(data);
                        response.Message = " Blood Group Data successfully updated!";
                    }
                }

                // Add new record to table blood group

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
                    response.Message = "No Id was submitted";
                    response.Success = false;
                    return response;
                }

                MBloodGroup data = new MBloodGroup();
                VMTblMBloodGroup? bloodGroup = GetById(id);

                if (bloodGroup == null)
                {
                    response.Success = false;
                    response.Message = "data does not exist!";

                    return response;
                }

                data.Id = bloodGroup.Id;
                data.Code = bloodGroup.Code;
                data.Description = bloodGroup.Description;
                data.CreatedBy = bloodGroup.CreatedBy;
                data.CreatedOn = bloodGroup.CreatedOn;
                data.ModifiedBy = bloodGroup.ModifiedBy;
                data.ModifiedOn = bloodGroup.ModifiedOn;
                data.DeletedBy = userid;
                data.DeletedOn = DateTime.Now;
                data.IsDelete = true;

                db.Update(data);
                db.SaveChanges();

                response.data = data;
                response.Message = "Blood Data Successfully deleted!";

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
