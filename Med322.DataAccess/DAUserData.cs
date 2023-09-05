using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DAUserData
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DAUserData(Med322_BContext _db)
        {
            db = _db;
        }
        //private VMUserData? GetById(int id)
        //{
        //    return (from u in db.MUsers
        //            join b in db.MBiodata
        //            on u.Id equals b.Id
        //            join r in db.MRoles
        //            on u.Id equals r.Id
        //            where u.IsDelete == false && u.Id == id
        //            select new VMUserData
        //            {
        //                UserId = u.Id,
        //                BiodataId = u.BiodataId,
        //                RoleId = u.RoleId,
        //                RoleName = r.Name,
        //                Email = u.Email,
        //                Password = u.Password,
        //                LoginAttempt = u.LoginAttempt,
        //                IsLocked = u.IsLocked,
        //                LastLogin = u.LastLogin,
        //                Fullname = b.Fullname,
        //                MobilePhone = b.MobilePhone,
        //                Image = b.Image,
        //                ImagePath = b.ImagePath,

        //            }).FirstOrDefault();

        //}

        private VMUserData? GetById(int id)
        {
            return (from u in db.MUsers
                    join b in db.MBiodata on (long?)u.BiodataId equals b.Id into userBio
                    from b in userBio.DefaultIfEmpty()
                    join r in db.MRoles on (long?)u.RoleId equals r.Id into userData
                    from r in userData.DefaultIfEmpty()
                    join d in db.MDoctors on b.Id equals d.BiodataId into Doctor
                    from d in Doctor.DefaultIfEmpty()
                    where u.IsDelete == false && u.Id == id
                    select new VMUserData
                    {
                        UserId = u.Id,
                        BiodataId = (int?)u.BiodataId,
                        RoleId = r.Id,
                        RoleName = r.Name,
                        Email = u.Email,
                        Password = u.Password,
                        LoginAttempt = u.LoginAttempt,
                        IsLocked = u.IsLocked,
                        LastLogin = u.LastLogin,
                        Fullname = b.Fullname,
                        MobilePhone = b.MobilePhone,
                        Image = b.Image,
                        ImagePath = b.ImagePath,
                        DoctorId = d.Id,

                    }).FirstOrDefault();

        }
        public VMResponse Get(int id)
        {
            try
            {
                if (id < 1)
                {
                    response.Message = "No User Data Id was submitted";
                    response.Success = false;
                    return response;
                }

                // get all the category data
                response.data = GetById(id);

                // check if data is null
                if (response.data == null)
                {
                    response.Message = $"User Data with ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"User Data with ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse GetAll()
        {
            try
            {

                List<VMUserData> userDatas = (from u in db.MUsers
                                              join b in db.MBiodata on (long?)u.BiodataId equals b.Id into userBio
                                              from b in userBio.DefaultIfEmpty()
                                              join r in db.MRoles on (long?)u.RoleId equals r.Id into userData
                                              from r in userData.DefaultIfEmpty()
                                              join d in db.MDoctors on b.Id equals d.BiodataId into Doctor
                                              from d in Doctor.DefaultIfEmpty()
                                              where u.IsDelete == false
                                              select new VMUserData
                                              {
                                                  UserId = u.Id,
                                                  BiodataId = (int?)u.BiodataId,
                                                  RoleId = r.Id,
                                                  RoleName = r.Name,
                                                  Email = u.Email,
                                                  Password = u.Password,
                                                  LoginAttempt = u.LoginAttempt,
                                                  IsLocked = u.IsLocked,
                                                  LastLogin = u.LastLogin,
                                                  Fullname = b.Fullname,
                                                  MobilePhone = b.MobilePhone,
                                                  Image = b.Image,
                                                  ImagePath = b.ImagePath,
                                                  DoctorId = d.Id,

                                              }).ToList();

                if (userDatas.Count < 1)
                {
                    response.Success = false;
                    response.Message = "User table has no data!";
                    return response;
                }

                response.data = userDatas;
                response.Message = "User data successfully fetched!";
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
