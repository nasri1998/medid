using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DACustomer
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DACustomer(Med322_BContext _db)
        {
            db = _db;
        }

        private VMCustomer? GetById(int id) 
        {
            return (from c in db.MCustomers
                    where c.Id == id
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
        }

        private VMCustomer? GetByBioId(int id)
        {
            return (from c in db.MCustomers
                    where c.BiodataId == id
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
        }
        public VMResponse GetAll()
        {
            try
            {
                List<VMCustomer> customers = (
                from c in db.MCustomers
                where c.IsDelete == false
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

                }).ToList();

                if (customers.Count < 1)
                {
                    response.Message = "Biodata table has no data!";
                    response.Success = false;
                    return response;
                }
                response.data = customers;
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
            
            try {

                if (id < 1)
                {
                    response.Message = "No Customer Id was submitted";
                    response.Success = false;
                    return response;
                }

                response.data = GetById(id);

                // check if data is null
                if (response.data == null)
                {
                    response.Message = $"Customer with ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"Customer with ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;

        }

        public VMResponse GetCustomerId(int id)
        {

            try
            {

                if (id < 1)
                {
                    response.Message = "Customer biodata Id not match or not submitted ";
                    response.Success = false;
                    return response;
                }

                response.data = GetByBioId(id);

                // check if data is null
                if (response.data == null)
                {
                    response.Message = $"Customer with Biodata ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"Customer with biodata ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;

        }
        public VMResponse CreateUpdate(VMCustomer inputC)
        {
            try
            {
                MCustomer data = new MCustomer();

                data.BiodataId = inputC.BiodataId;
                data.Dob = inputC.Dob;
                data.Gender = inputC.Gender;
                data.BloodGroupId = inputC.BloodGroupId;
                data.RhesusType = inputC.RhesusType;
                data.Height = inputC.Height;
                data.Weight = inputC.Weight;

                if (inputC.Id < 1)
                {

                    data.CreatedBy = inputC.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);
                    response.Message = "New Biodata Data successfully inserted!";
                }
                else
                {
                    VMCustomer? customer = GetById((int)inputC.Id);

                    if (customer == null)
                    {
                        response.Success = false;
                        response.Message = "Customer data does not exist!";
                        return response;
                    }
                    else
                    {
                        data.BiodataId = inputC.BiodataId ?? customer.BiodataId;
                        data.Dob = inputC.Dob ?? customer.Dob;
                        data.Gender = inputC.Gender ?? customer.Gender;
                        data.BloodGroupId = inputC.BloodGroupId ?? customer.BloodGroupId;
                        data.RhesusType = inputC.RhesusType ?? customer.RhesusType;
                        data.Height = inputC.Height ?? customer.Height;
                        data.Weight = inputC.Weight ?? customer.Weight;

                        data.Id = customer.Id;

                        data.CreatedBy = customer.CreatedBy;
                        data.CreatedOn = customer.CreatedOn;

                        data.ModifiedBy = inputC.ModifiedBy;
                        data.ModifiedOn = DateTime.Now;

                        db.Update(data);
                        response.Message = "Customer data Data successfully updated!";
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
                    response.Message = "No Customer Id was submitted";
                    response.Success = false;
                    return response;
                }

                MCustomer data = new MCustomer();
                VMCustomer? customer = GetById(id);

                if (customer == null)
                {
                    response.Success = false;
                    response.Message = "Customer data does not exist!";

                    return response;
                }

                data.Id = customer.Id;
                data.BiodataId = customer.BiodataId;
                data.Dob = customer.Dob;
                data.Gender = customer.Gender;
                data.BloodGroupId = customer.BloodGroupId;
                data.RhesusType = customer.RhesusType;
                data.Height = customer.Height;
                data.Weight = customer.Weight;
                data.CreatedBy = customer.CreatedBy;
                data.CreatedOn = customer.CreatedOn;
                data.ModifiedBy = customer.ModifiedBy;
                data.ModifiedOn = customer.ModifiedOn;
                data.DeletedBy = userid;
                data.DeletedOn = DateTime.Now;
                data.IsDelete = true;

                db.Update(data);
                db.SaveChanges();

                response.data = data;
                response.Message = $"Customer ID {id} Successfully deleted!";

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
