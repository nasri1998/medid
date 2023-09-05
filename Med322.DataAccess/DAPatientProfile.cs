using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DAPatientProfile
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DAPatientProfile(Med322_BContext _db)
        {
            db = _db;
        }
        public VMPatientProfile? GetById(int id)
        {
            return (from c in db.MCustomers
                    join b in db.MBiodata
                    on c.BiodataId equals b.Id
                    where c.Id == id
                    select new VMPatientProfile
                    {
                        Id = c.Id,
                        BiodataId = c.BiodataId,
                        Fullname = b.Fullname,
                        Dob = c.Dob,
                        MobilePhone = b.MobilePhone,

                        CreatedBy = c.CreatedBy,
                        CreatedOn = c.CreatedOn,
                        ModifiedBy = c.ModifiedBy,
                        ModifiedOn = c.ModifiedOn,
                        DeletedBy = c.DeletedBy,
                        DeletedOn = c.DeletedOn,
                        IsDelete = c.IsDelete
                    }).FirstOrDefault();
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


        public VMResponse CreateUpdate(VMPatientProfile inputPP)
        {
            try
            {


                VMCustomer? cust = (from c in db.MCustomers
                        where c.Id == inputPP.Id
                        select new VMCustomer
                        {
                            Id = c.Id,
                            BiodataId = c.BiodataId,
                            Dob = c.Dob,
                            Gender = c.Gender,
                            BloodGroupId = c.BloodGroupId,
                            RhesusType = c.RhesusType,
                            Height = c.Height,
                            Weight = c.Weight,

                            CreatedBy = c.CreatedBy,
                            CreatedOn = c.CreatedOn,
                            ModifiedBy = c.ModifiedBy,
                            ModifiedOn = c.ModifiedOn,
                            DeletedBy = c.DeletedBy,
                            DeletedOn = c.DeletedOn,
                            IsDelete = c.IsDelete
                        }).FirstOrDefault();

                MCustomer customer = new MCustomer();

                customer.Id = cust.Id;
                customer.BiodataId = cust.BiodataId;
                customer.Dob = cust.Dob;
                customer.Gender = cust.Gender;
                customer.BloodGroupId = cust.BloodGroupId;
                customer.RhesusType = cust.RhesusType;
                customer.Height = cust.Height;
                customer.Weight = cust.Weight;
                    
                customer.CreatedBy = cust.CreatedBy;
                customer.CreatedOn = cust.CreatedOn;
                customer.ModifiedBy = cust.ModifiedBy;
                customer.ModifiedOn = cust.ModifiedOn;
                customer.DeletedBy = cust.DeletedBy;
                customer.DeletedOn = cust.DeletedOn;
                customer.IsDelete = cust.IsDelete;


                VMBiodata? bio = (from b in db.MBiodata
                       where b.IsDelete == false && b.Id == customer.BiodataId
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

                MBiodatum biodata = new MBiodatum();

                biodata.Id = bio.Id;
                biodata.Fullname = bio.Fullname;
                biodata.MobilePhone = bio.MobilePhone;
                biodata.Image = bio.Image;
                biodata.ImagePath = bio.ImagePath;
                biodata.CreatedBy = bio.CreatedBy;
                biodata.CreatedOn = bio.CreatedOn;
                biodata.ModifiedBy = bio.ModifiedBy;
                biodata.ModifiedOn = bio.ModifiedOn;
                biodata.DeletedBy = bio.DeletedBy;
                biodata.DeletedOn = bio.DeletedOn;
                biodata.IsDelete = bio.IsDelete;


                if (inputPP.Id < 1)
                {
                    response.Success = false;
                    response.Message = "There is no Biodata Data";
                    return response;
                }
                else
                {


                    if (biodata == null || customer == null)
                    {
                        response.Success = false;
                        response.Message = " Biodata data does not exist!";
                        return response;
                    }
                    else
                    {
                        biodata.Id = bio.Id;
                        biodata.Fullname = inputPP.Fullname;
                        biodata.MobilePhone = inputPP.MobilePhone;
                        biodata.Image = bio.Image;
                        biodata.ImagePath = bio.ImagePath;
                        biodata.CreatedBy = bio.CreatedBy;
                        biodata.CreatedOn = bio.CreatedOn;
                        biodata.ModifiedBy = bio.ModifiedBy;
                        biodata.ModifiedOn = bio.ModifiedOn;
                        biodata.DeletedBy = bio.DeletedBy;
                        biodata.DeletedOn = bio.DeletedOn;
                        biodata.IsDelete = bio.IsDelete;

                        db.Update(biodata);


                        customer.Id = cust.Id;
                        customer.BiodataId = cust.BiodataId;
                        customer.Dob = inputPP.Dob;
                        customer.Gender = cust.Gender;
                        customer.BloodGroupId = cust.BloodGroupId;
                        customer.RhesusType = cust.RhesusType;
                        customer.Height = cust.Height;
                        customer.Weight = cust.Weight;

                        customer.CreatedBy = cust.CreatedBy;
                        customer.CreatedOn = cust.CreatedOn;
                        customer.ModifiedBy = cust.ModifiedBy;
                        customer.ModifiedOn = cust.ModifiedOn;
                        customer.DeletedBy = cust.DeletedBy;
                        customer.DeletedOn = cust.DeletedOn;
                        customer.IsDelete = cust.IsDelete;

                        db.Update(customer);

                        response.Message = " Biodata data successfully updated!";
                    }
                }

                // Add new record to table biodata

                db.SaveChanges();


                response.data = GetById((int)inputPP.Id);

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
