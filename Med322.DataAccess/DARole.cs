using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Med322.DataAccess
{
    public class DARole
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DARole(Med322_BContext _db)
        {
            db = _db;
        }

        private VMRole? GetById(int id)
        {
            return (from r in db.MRoles
                    where r.IsDelete == false && r.Id == id
                    select new VMRole
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Code = r.Code,


                        CreatedBy = r.CreatedBy,
                        CreatedOn = r.CreatedOn,
                        ModifiedBy = r.ModifiedBy,
                        ModifiedOn = r.ModifiedOn,
                        DeletedBy = r.DeletedBy,
                        DeletedOn = r.DeletedOn,
                        IsDelete = r.IsDelete

                    }).FirstOrDefault();
        }

        public VMResponse GetAll()
        {
            try
            {

                List<VMRole> role = (from r in db.MRoles
                        where r.IsDelete == false
                        select new VMRole
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Code = r.Code,


                            CreatedBy = r.CreatedBy,
                            CreatedOn = r.CreatedOn,
                            ModifiedBy = r.ModifiedBy,
                            ModifiedOn = r.ModifiedOn,
                            DeletedBy = r.DeletedBy,
                            DeletedOn = r.DeletedOn,
                            IsDelete = r.IsDelete

                        }).ToList();

                if (role.Count < 1)
                {
                    response.Success = false;
                    response.Message = "Role table has no data!";
                    return response;
                }

                response.data = role;
                response.Message = "Role data successfully fetched!";
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
                    response.Message = "No User Id was submitted";
                    response.Success = false;
                    return response;
                }
                response.data = GetById(id);
                if (response.data == null)
                {
                    response.Message = $"User with ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"User with ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse CreateUpdate(VMRole inputR)
        {
            try
            {
                MRole data = new MRole();

                data.Name = inputR.Name;
                data.Code = inputR.Code;

                if (inputR.Id < 1)
                {

                    data.CreatedBy = inputR.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);
                    response.Message = "New Role Data successfully inserted!";
                }
                else
                {
                    VMRole? role = GetById((int)inputR.Id);

                    if (role == null)
                    {
                        response.Success = false;
                        response.Message = "Role data does not exist!";
                        return response;
                    }
                    else
                    {
                        data.Id = role.Id;

                        data.Name = inputR.Name ?? role.Name;
                        data.Code = inputR.Code ?? role.Code;

                        data.CreatedBy = role.CreatedBy;
                        data.CreatedOn = role.CreatedOn;

                        data.ModifiedBy = inputR.ModifiedBy;
                        data.ModifiedOn = DateTime.Now;

                        db.Update(data);
                        response.Message = "Role Data successfully updated!";
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
                    response.Message = "No Role Id was submitted";
                    response.Success = false;
                    return response;
                }

                MRole data = new MRole();
                VMRole? role = GetById(id);

                if (role == null)
                {
                    response.Success = false;
                    response.Message = "Role data does not exist!";

                    return response;
                }

                data.Id = role.Id;
                data.Name = role.Name;
                data.Code = role.Code;

                data.CreatedBy = role.CreatedBy;
                data.CreatedOn = role.CreatedOn;
                data.ModifiedBy = role.ModifiedBy;
                data.ModifiedOn = role.ModifiedOn;
                data.DeletedBy = userid;
                data.DeletedOn = DateTime.Now;
                data.IsDelete = true;

                db.Update(data);
                db.SaveChanges();

                response.data = data;
                response.Message = $"Role ID {id} Successfully deleted!";

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
