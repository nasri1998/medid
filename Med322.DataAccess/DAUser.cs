using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DAUser
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DAUser(Med322_BContext _db)
        {
            db = _db;
        }
        private VMUser? GetById(int id)
        {
            return (from u in db.MUsers
                    where u.IsDelete == false && u.Id == id
                    select new VMUser
                    {
                        Id = u.Id,
                        BiodataId = (int?)u.BiodataId,
                        RoleId = u.RoleId,
                        Email = u.Email,
                        Password = u.Password,
                        LoginAttempt = u.LoginAttempt,
                        IsLocked = u.IsLocked,
                        LastLogin = u.LastLogin,

                        CreatedBy = u.CreatedBy,
                        CreatedOn = u.CreatedOn,
                        ModifiedBy = u.ModifiedBy,
                        ModifiedOn = u.ModifiedOn,
                        DeletedBy = u.DeletedBy,
                        DeletedOn = u.DeletedOn,
                        IsDelete = u.IsDelete

                    }).FirstOrDefault();
        }

        public VMResponse GetAll()
        {
            try {

                List<VMUser> User = (from u in db.MUsers
                                     where u.IsDelete == false
                                     select new VMUser
                                     {
                                         Id = u.Id,
                                         BiodataId = (int?)u.BiodataId,
                                         RoleId = u.RoleId,
                                         Email = u.Email,
                                         Password = u.Password,
                                         LoginAttempt = u.LoginAttempt,
                                         IsLocked = u.IsLocked,
                                         LastLogin = u.LastLogin,

                                         CreatedBy = u.CreatedBy,
                                         CreatedOn = u.CreatedOn,
                                         ModifiedBy = u.ModifiedBy,
                                         ModifiedOn = u.ModifiedOn,
                                         DeletedBy = u.DeletedBy,
                                         DeletedOn = u.DeletedOn,
                                         IsDelete = u.IsDelete

                                     }).ToList();

                if (User.Count < 1) 
                {
                    response.Success = false;
                    response.Message = "User table has no data!";
                    return response;
                }

                response.data = User;
                response.Message = "User data successfully fetched!";
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

        public VMResponse CreateUpdate(VMUser inputU)
        {
            try
            {
                MUser data = new MUser();

                data.BiodataId = inputU.BiodataId;
                data.RoleId = inputU.RoleId;
                data.Email = inputU.Email;
                data.Password = inputU.Password;
                data.LoginAttempt = inputU.LoginAttempt;
                data.IsLocked = inputU.IsLocked;
                data.LastLogin = inputU.LastLogin;

                if (inputU.Id < 1)
                {

                    data.CreatedBy = inputU.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);
                    response.Message = "New User Data successfully inserted!";
                }
                else
                {
                    VMUser? user = GetById((int)inputU.Id);

				    data = new MUser();

					data.BiodataId = inputU.BiodataId ?? user.BiodataId;
					data.RoleId = inputU.RoleId ?? user.RoleId;
					data.Email = inputU.Email ?? user.Email;
					data.Password = inputU.Password ?? user.Password;
					data.LoginAttempt = inputU.LoginAttempt ?? user.LoginAttempt;
					data.IsLocked = inputU.IsLocked ?? user.IsLocked;
					data.LastLogin = inputU.LastLogin ?? user.LastLogin;

					if (user == null)
                    {
                        response.Success = false;
                        response.Message = "User data does not exist!";
                        return response;
                    }
                    else
                    {
                        data.Id = user.Id;

                        data.CreatedBy = user.CreatedBy;
                        data.CreatedOn = user.CreatedOn;

                        data.ModifiedBy = inputU.ModifiedBy;
                        data.ModifiedOn = DateTime.Now;

                        db.Update(data);
                        response.Message = "User Data successfully updated!";
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
                    response.Message = "No User Id was submitted";
                    response.Success = false;
                    return response;
                }

                MUser data = new MUser();
                VMUser? User = GetById(id);

                if (User == null)
                {
                    response.Success = false;
                    response.Message = "User data does not exist!";

                    return response;
                }

                data.Id = User.Id;
                data.BiodataId = User.BiodataId;
                data.RoleId = User.RoleId;
                data.Email = User.Email;
                data.Password = User.Password;
                data.LoginAttempt = User.LoginAttempt;
                data.IsLocked = User.IsLocked;
                data.LastLogin = User.LastLogin;

                data.CreatedBy = User.CreatedBy;
                data.CreatedOn = User.CreatedOn;
                data.ModifiedBy = User.ModifiedBy;
                data.ModifiedOn = User.ModifiedOn;
                data.DeletedBy = userid;
                data.DeletedOn = DateTime.Now;
                data.IsDelete = true;

                db.Update(data);
                db.SaveChanges();

                response.data = data;
                response.Message = $"User ID {id} Successfully deleted!";

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
