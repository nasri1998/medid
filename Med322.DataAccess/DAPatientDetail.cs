using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DAPatientDetail
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();
        public DAPatientDetail(Med322_BContext _db) {
            db = _db;
        }
        public VMResponse GetAll(int userId)
        {
            try
            {
                List<VMPatientDetail> data = (
                    from pmd in db.DMPatientMemberDetails.FromSqlRaw($"select * from vw_PatientMemberDetail where userid = {userId} and IsDelete = 0").ToList()
                    select new VMPatientDetail
                    {
                        UserId = pmd.userid,
                        CustomerId = pmd.customerId,
                        BiodataId = pmd.biodataId,
                        ParentBioId = pmd.parentBioId,
                        BloodGroupId = pmd.BloodGroupId,
                        CustomerRelationId = pmd.CostumerRelationId,
                        Fullname = pmd.fullName,
                        namauser = pmd.namauser,
                        NameRelation = pmd.RelationName,
                        Dob = pmd.Dob,
                        Gender = pmd.Gender,
                        RhesusType = pmd.RhesusType,
                        Height = pmd.Height,
                        Weight = pmd.Weight,
                        Age = pmd.Age,
                        AppointmentSum = pmd.Appointment,
                        ChatSum = pmd.Chat,

                        CreatedBy = pmd.CreatedBy,
                        CreatedOn = pmd.CreatedOn,
                        ModifiedBy = pmd.ModifiedBy,
                        ModifiedOn = pmd.ModifiedOn,
                        DeletedBy = pmd.DeletedBy,
                        DeletedOn = pmd.DeletedOn,
                        IsDelete = pmd.IsDelete

                    }).ToList();
     
                response.data = data;
                response.Message = "data successfully fetched!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }
        public VMPatientDetail? GetByIdPatient(int custId)
        {
            var pmdQuery = db.DMPatientMemberDetails.FromSqlRaw($"select * from vw_PatientMemberDetail where customerId = {custId} and IsDelete = 0");
            var pmd = pmdQuery.FirstOrDefault();

            if (pmd != null)
            {
                var result = new VMPatientDetail
                {
                    UserId = pmd.userid,
                    CustomerId = pmd.customerId,
                    BiodataId = pmd.biodataId,
                    ParentBioId = pmd.parentBioId,
                    BloodGroupId = pmd.BloodGroupId,
                    CustomerRelationId = pmd.CostumerRelationId,
                    CustomerMemberId = pmd.CostumerMemberId,
                    Fullname = pmd.fullName,
                    NameRelation = pmd.RelationName,
                    Dob = pmd.Dob,
                    Gender = pmd.Gender,
                    RhesusType = pmd.RhesusType,
                    Height = pmd.Height,
                    Weight = pmd.Weight,
                    Age = pmd.Age,
                    AppointmentSum = pmd.Appointment,
                    ChatSum = pmd.Chat,

                    CreatedBy = pmd.CreatedBy,
                    CreatedOn = pmd.CreatedOn,
                    ModifiedBy = pmd.ModifiedBy,
                    ModifiedOn = pmd.ModifiedOn,
                    DeletedBy = pmd.DeletedBy,
                    DeletedOn = pmd.DeletedOn,
                    IsDelete = pmd.IsDelete
                };

                return result;
            }

            return null; // Atau kembalikan null jika tidak ada hasil yang sesuai.
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
                response.data = GetByIdPatient(id);

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

        public VMResponse Create(VMPatientDetail inputpatien)
        {
     
            VMResponse response = new VMResponse();
            try
            {
                // Tambahkan objek dataBio ke basis data
                MBiodatum dataBio = new MBiodatum
                {
                    Fullname = inputpatien.Fullname,
                    CreatedBy = (long)inputpatien.UserId,
                    CreatedOn = DateTime.Now
                };
                db.Add(dataBio);

                // Simpan perubahan ke basis data dan periksa apakah penyimpanan berhasil
                int savedChanges = db.SaveChanges();
                if (savedChanges > 0)
                {
                    // Simpan objek dataCustomer setelah dataBio berhasil disimpan
                    MCustomer dataCustomer = new MCustomer
                    {
                        BiodataId = dataBio.Id,
                        BloodGroupId = inputpatien.BloodGroupId,
                        Dob = inputpatien.Dob,
                        Gender = inputpatien.Gender,
                        RhesusType = inputpatien.RhesusType,
                        Height = inputpatien.Height,
                        Weight = inputpatien.Weight,
                        CreatedBy = (long)inputpatien.UserId,
                        CreatedOn = DateTime.Now
                    };
                    db.Add(dataCustomer);

                    // Simpan perubahan dan periksa apakah penyimpanan berhasil
                    int savedChanges2 = db.SaveChanges();
                    if (savedChanges2 > 0)
                    {
                        //// Sekarang dataCustomer memiliki ID yang benar
                        //dataCustomerMember.CustomerId = dataCustomer.Id;

                        // Simpan objek dataCustomerMember setelah dataCustomer berhasil disimpan
                        MCustomerMember dataCustomerMember = new MCustomerMember
                        {
                            ParentBiodataId = inputpatien.ParentBioId,
                            CustomerRelationId = inputpatien.CustomerRelationId,
                            CustomerId = dataCustomer.Id,
                            CreatedBy = dataBio.CreatedBy,
                            CreatedOn = DateTime.Now
                        };
                        db.Add(dataCustomerMember);

                        // Simpan perubahan lagi setelah dataCustomerMember ditambahkan
                        int savedChanges3 = db.SaveChanges();
                        if (savedChanges3 > 0)
                        {
                            // Mengambil data pasien berdasarkan ID dataBio yang baru saja ditambahkan
                            var newPatient = GetAll((int)inputpatien.UserId);
                            response.data = newPatient;
                            response.Message = "Patient successfully Added/Updated!";
                        }
                        else
                        {
                            response.Message = "Failed to save dataCustomerMember.";
                            response.Success = false;
                        }
                    }
                    else
                    {
                        response.Message = "Failed to save dataCustomer.";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Message = "Failed to save dataBio.";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public VMResponse Update(VMPatientDetail updatedPatient)
        {
            VMResponse response = new VMResponse();

            try
            {
                
                    // Perbarui properti-properti pasien berdasarkan data yang dikirimkan
                    VMCustomer? cust = (from c in db.MCustomers
                                        where c.Id == updatedPatient.CustomerId
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
                    customer.ModifiedBy = updatedPatient.ModifiedBy;
                    customer.ModifiedOn = DateTime.Now;
                    

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
                    biodata.ModifiedBy = updatedPatient.ModifiedBy;
                    biodata.ModifiedOn = DateTime.Now;
                    

                        if (biodata == null || customer == null)
                        {
                            response.Success = false;
                            response.Message = " Biodata data does not exist!";
                            return response;
                        }
                        else
                        {
                            biodata.Id = bio.Id;
                            biodata.Fullname = updatedPatient.Fullname;
                            biodata.MobilePhone = updatedPatient.MobilePhone;
                            biodata.Image = bio.Image;
                            biodata.ImagePath = bio.ImagePath;
                            biodata.CreatedBy = bio.CreatedBy;
                            biodata.CreatedOn = bio.CreatedOn;
                            biodata.ModifiedBy = updatedPatient.ModifiedBy;
                            biodata.ModifiedOn = DateTime.Now;

                            db.Update(biodata);


                                    customer.Id = cust.Id;
                                    customer.BiodataId = cust.BiodataId;
                                    customer.Dob = updatedPatient.Dob;
                                    customer.Gender = updatedPatient.Gender;
                                    customer.BloodGroupId = updatedPatient.BloodGroupId;
                                    customer.RhesusType = updatedPatient.RhesusType;
                                    customer.Height = updatedPatient.Height;
                                    customer.Weight = updatedPatient.Weight;

                                    customer.CreatedBy = cust.CreatedBy;
                                    customer.CreatedOn = cust.CreatedOn;
                                    customer.ModifiedBy = updatedPatient.ModifiedBy;
                                    customer.ModifiedOn = DateTime.Now;

                            db.Update(customer);

                            VMCustomerMember custMember = (from cm in db.MCustomerMembers
                                                           where cm.IsDelete == false && cm.Id == updatedPatient.CustomerMemberId
                                                           select new VMCustomerMember
                                                           {
                                                               Id = cm.Id,
                                                               ParentBiodataId = cm.ParentBiodataId,
                                                               CustomerId = cm.CustomerId,
                                                               CustomerRelationId = cm.CustomerRelationId,

                                                               CreatedBy = cm.CreatedBy,
                                                               CreatedOn = cm.CreatedOn,
                                                               ModifiedBy = cm.ModifiedBy,
                                                               ModifiedOn = cm.ModifiedOn,
                                                               DeletedBy = cm.DeletedBy,
                                                               DeletedOn = cm.DeletedOn,
                                                               IsDelete = cm.IsDelete
                                                           }).FirstOrDefault();
                            MCustomerMember customerMember = new MCustomerMember();
                            customerMember.Id = custMember.Id;
                            customerMember.ParentBiodataId = custMember.ParentBiodataId;
                            customerMember.CustomerId = custMember.CustomerId;
                            customerMember.CustomerRelationId = updatedPatient.CustomerRelationId;
                            customerMember.CreatedBy = custMember.CreatedBy;
                            customerMember.CreatedOn = custMember.CreatedOn;
                            customerMember.ModifiedBy = updatedPatient.ModifiedBy;
                            customerMember.ModifiedOn = DateTime.Now;
                    
                            db.Update(customerMember);

                            response.Message = " Biodata data successfully updated!";
                        }
                    //}
                    // Simpan perubahan ke database
                    db.SaveChanges();

                    response.Message = "Patient successfully updated!";
                
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse Delete(int customermember)
        {
            try
            {
                VMPatientDetail? data = GetByIdPatient(customermember);

              

                MBiodatum biodata = new MBiodatum();
                biodata.Id = (long)data.BiodataId;
                biodata.Fullname = data.Fullname;
                biodata.CreatedBy = data.CreatedBy;
                biodata.CreatedOn = data.CreatedOn;
                biodata.ModifiedBy = data.ModifiedBy;
                biodata.ModifiedOn = data.ModifiedOn;
                biodata.DeletedBy = data.UserId;
                biodata.DeletedOn = DateTime.Now;
                biodata.IsDelete = true;

                db.Update(biodata);

                MCustomer customer = new MCustomer();
                customer.Id = (long)data.CustomerId;
                customer.BiodataId = data.BiodataId;
                customer.Dob = data.Dob;
                customer.Gender = data.Gender;
                customer.BloodGroupId = data.BloodGroupId;
                customer.RhesusType = data.RhesusType;
                customer.Height = data.Height;
                customer.Weight = data.Weight;

                customer.CreatedBy = data.CreatedBy;
                customer.CreatedOn = data.CreatedOn;
                customer.ModifiedBy = data.ModifiedBy;
                customer.ModifiedOn = data.ModifiedOn;
                customer.DeletedBy = data.UserId;
                customer.DeletedOn = DateTime.Now;
                customer.IsDelete = true;

                db.Update(customer);
                MCustomerMember customerMember = new MCustomerMember();
                customerMember.Id = data.CustomerMemberId;
                customerMember.ParentBiodataId = data.ParentBioId;
                customerMember.CustomerId = data.CustomerId;
                customerMember.CustomerRelationId = data.CustomerRelationId;
                customerMember.CreatedBy = data.CreatedBy;
                customerMember.CreatedOn = data.CreatedOn;
                customerMember.ModifiedBy = data.ModifiedBy;
                customerMember.ModifiedOn = data.ModifiedOn;
                customerMember.DeletedBy = data.UserId;
                customerMember.DeletedOn = DateTime.Now;
                customerMember.IsDelete = true;

                db.Update(customerMember);

                // Simpan perubahan ke database
                db.SaveChanges();

                response.Message = "Patient successfully updated!";
            }
            catch (Exception)
            {

                throw;
            }

            return response;
        }
        // Metode untuk mengambil data pasien berdasarkan ID
        public VMPatientDetail? GetAllDataById(int id)
        {
            return (
               from b in db.MBiodata
               join u in db.MUsers on b.Id equals u.BiodataId
               join c in db.MCustomers on b.Id equals c.BiodataId
               join bl in db.MBloodGroups on c.BloodGroupId equals bl.Id
               join cm in db.MCustomerMembers on c.Id equals cm.CustomerId
               join cr in db.MCustomerRelations on cm.CustomerRelationId equals cr.Id
               where cm.IsDelete == false && cm.CustomerId == id
               select new VMPatientDetail
               {
                   UserId = u.Id,
                   Fullname = b.Fullname,
                   CustomerMemberId = cm.Id,
                   ParentBioId = cm.ParentBiodataId,
                   BiodataId = b.Id,
                   BloodGroupId = c.BloodGroupId,
                   Code = bl.Code,
                   CustomerRelationId = cm.CustomerRelationId,
                   NameRelation = cr.Name,
                   CustomerId = c.Id,
                   Dob = c.Dob,
                   Gender = c.Gender,
                   RhesusType = c.RhesusType,
                   Weight = c.Weight,
                   Height = c.Height,

                   CreatedBy = b.CreatedBy,
                   CreatedOn = b.CreatedOn,
                   ModifiedBy = b.ModifiedBy,
                   ModifiedOn = b.ModifiedOn,
                   DeletedBy = b.DeletedBy,
                   DeletedOn = b.DeletedOn,
                   IsDelete = b.IsDelete

               }).FirstOrDefault();
        }

        public VMResponse GetByFilter(string filter, int Id)
        {
            try
            {
                List<VMPatientDetail> data = (
                    from pmd in db.DMPatientMemberDetails.FromSqlRaw($"select * from vw_PatientMemberDetail where IsDelete = 0").ToList()
                    where pmd.userid == Id && pmd.fullName.ToLower().Contains(filter.ToLower())
                    select new VMPatientDetail
                    {
                        UserId = pmd.userid,
                        CustomerId = pmd.customerId,
                        BiodataId = pmd.biodataId,
                        ParentBioId = pmd.parentBioId,
                        BloodGroupId = pmd.BloodGroupId,
                        CustomerRelationId = pmd.CostumerRelationId,
                        Fullname = pmd.fullName,
                        NameRelation = pmd.RelationName,
                        Dob = pmd.Dob,
                        Gender = pmd.Gender,
                        RhesusType = pmd.RhesusType,
                        Height = pmd.Height,
                        Weight = pmd.Weight,
                        Age = pmd.Age,
                        AppointmentSum = pmd.Appointment,
                        ChatSum = pmd.Chat,

                        CreatedBy = pmd.CreatedBy,
                        CreatedOn = pmd.CreatedOn,
                        ModifiedBy = pmd.ModifiedBy,
                        ModifiedOn = pmd.ModifiedOn,
                        DeletedBy = pmd.DeletedBy,
                        DeletedOn = pmd.DeletedOn,
                        IsDelete = pmd.IsDelete

                    }).ToList();

                response.Success = true;
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
        private VMPatientDetail? GetById(int id)
        {
            return (from cm in db.MCustomerMembers
                    join b in db.MBiodata on cm.ParentBiodataId equals b.Id
                    join c in db.MCustomers on cm.CustomerId equals c.Id
                   
                    where cm.IsDelete == false && cm.Id == id
                    select new VMPatientDetail
                    {
                        Fullname = b.Fullname,
                        CustomerMemberId = cm.Id,

                        CustomerId = cm.CustomerId,
                        BiodataId = cm.ParentBiodataId,
                        CustomerRelationId = cm.CustomerRelationId,
                        Dob = c.Dob,
                        Gender = c.Gender,
                        BloodGroupId = c.BloodGroupId,
                        RhesusType = c.RhesusType,
                        Height = c.Height,
                        Weight = c.Weight,

                        CreatedBy = cm.CreatedBy,
                        CreatedOn = cm.CreatedOn,
                        ModifiedBy = cm.ModifiedBy,
                        ModifiedOn = cm.ModifiedOn,
                        DeletedBy = cm.DeletedBy,
                        DeletedOn = cm.DeletedOn,
                        IsDelete = cm.IsDelete
                    }).FirstOrDefault();
        }

        public VMResponse GetId(int id)
        {

            try
            {

                if (id < 1)
                {
                    response.Message = "Customer biodata Id not match or not submitted ";
                    response.Success = false;
                    return response;
                }

                response.data = GetAllDataById(id);

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

        public VMResponse GetCustomerMember(int id)
        {

            try
            {

                if (id < 1)
                {
                    response.Message = "No Customer Id was submitted";
                    response.Success = false;
                    return response;
                }

                response.data = GetCMId(id);

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
        private VMPatientDetail? GetCMId(int id)
        {
            return (from cm in db.MCustomerMembers
                    where cm.IsDelete == false && cm.Id == id
                    select new VMPatientDetail
                    {
                        CustomerId = cm.CustomerId,
                        BiodataId = cm.ParentBiodataId,
                        CustomerRelationId = cm.CustomerRelationId,
             
                        CreatedBy = cm.CreatedBy,
                        CreatedOn = cm.CreatedOn,
                        ModifiedBy = cm.ModifiedBy,
                        ModifiedOn = cm.ModifiedOn,
                        DeletedBy = cm.DeletedBy,
                        DeletedOn = cm.DeletedOn,
                        IsDelete = cm.IsDelete
                    }).FirstOrDefault();
        }

    }
}
//VMCustomer? cust = (from c in db.MCustomers
//                    where c.Id == customerid
//                    select new VMCustomer
//                    {
//                        Id = c.Id,
//                        BiodataId = c.BiodataId,
//                        Dob = c.Dob,
//                        Gender = c.Gender,
//                        BloodGroupId = c.BloodGroupId,
//                        RhesusType = c.RhesusType,
//                        Height = c.Height,
//                        Weight = c.Weight,

//                        CreatedBy = c.CreatedBy,
//                        CreatedOn = c.CreatedOn,
//                        ModifiedBy = c.ModifiedBy,
//                        ModifiedOn = c.ModifiedOn,
//                        DeletedBy = c.DeletedBy,
//                        DeletedOn = c.DeletedOn,
//                        IsDelete = c.IsDelete
//                    }).FirstOrDefault();

//MCustomer customer = new MCustomer();

//customer.Id = cust.Id;
//customer.BiodataId = cust.BiodataId;
//customer.Dob = cust.Dob;
//customer.Gender = cust.Gender;
//customer.BloodGroupId = cust.BloodGroupId;
//customer.RhesusType = cust.RhesusType;
//customer.Height = cust.Height;
//customer.Weight = cust.Weight;

//customer.CreatedBy = cust.CreatedBy;
//customer.CreatedOn = cust.CreatedOn;
//customer.ModifiedBy = cust.ModifiedBy;
//customer.ModifiedOn = cust.ModifiedOn;
//customer.DeletedBy = cust.DeletedBy;
//customer.DeletedOn = cust.DeletedOn;
//customer.IsDelete = cust.IsDelete;

//VMBiodata? bio = (from b in db.MBiodata
//                  where b.IsDelete == false && b.Id == customer.BiodataId
//                  select new VMBiodata
//                  {

//                      Id = b.Id,
//                      Fullname = b.Fullname,
//                      MobilePhone = b.MobilePhone,
//                      Image = b.Image,
//                      ImagePath = b.ImagePath,
//                      CreatedBy = b.CreatedBy,
//                      CreatedOn = b.CreatedOn,
//                      ModifiedBy = b.ModifiedBy,
//                      ModifiedOn = b.ModifiedOn,
//                      DeletedBy = b.DeletedBy,
//                      DeletedOn = b.DeletedOn,
//                      IsDelete = b.IsDelete

//                  }).FirstOrDefault();

//MBiodatum biodata = new MBiodatum();

//biodata.Id = bio.Id;
//biodata.Fullname = bio.Fullname;
//biodata.MobilePhone = bio.MobilePhone;
//biodata.Image = bio.Image;
//biodata.ImagePath = bio.ImagePath;
//biodata.CreatedBy = bio.CreatedBy;
//biodata.CreatedOn = bio.CreatedOn;
//biodata.ModifiedBy = bio.ModifiedBy;
//biodata.ModifiedOn = bio.ModifiedOn;
//biodata.DeletedBy = bio.DeletedBy;
//biodata.DeletedOn = bio.DeletedOn;
//biodata.IsDelete = bio.IsDelete;

//VMCustomerMember custMember = (from cm in db.MCustomerMembers
//                               where cm.IsDelete == false && cm.Id == customermember
//                               select new VMCustomerMember
//                               {
//                                   Id = cm.Id,
//                                   ParentBiodataId = cm.ParentBiodataId,
//                                   CustomerId = cm.CustomerId,
//                                   CustomerRelationId = cm.CustomerRelationId,

//                                   CreatedBy = cm.CreatedBy,
//                                   CreatedOn = cm.CreatedOn,
//                                   ModifiedBy = cm.ModifiedBy,
//                                   ModifiedOn = cm.ModifiedOn,
//                                   DeletedBy = cm.DeletedBy,
//                                   DeletedOn = cm.DeletedOn,
//                                   IsDelete = cm.IsDelete
//                               }).FirstOrDefault();
//MCustomerMember customerMember = new MCustomerMember();

//customerMember.Id = custMember.Id;
//customerMember.ParentBiodataId = custMember.ParentBiodataId;
//customerMember.CustomerId = custMember.CustomerId;
//customerMember.CustomerRelationId = custMember.CustomerRelationId;
//customerMember.CreatedBy = custMember.CreatedBy;
//customerMember.CreatedOn = custMember.CreatedOn;
//customerMember.ModifiedBy = custMember.ModifiedBy;
//customerMember.ModifiedOn = custMember.ModifiedOn;
//customerMember.DeletedBy = custMember.DeletedBy;
//customerMember.DeletedOn = custMember.DeletedOn;
//customerMember.IsDelete = custMember.IsDelete;

//if (biodata == null || customer == null)
//{
//    response.Success = false;
//    response.Message = " Biodata data does not exist!";
//    return response;
//}
//else
//{
//    biodata.Id = bio.Id;
//    biodata.Fullname = bio.Fullname;
//    biodata.MobilePhone = bio.MobilePhone;
//    biodata.Image = bio.Image;
//    biodata.ImagePath = bio.ImagePath;
//    biodata.CreatedBy = bio.CreatedBy;
//    biodata.CreatedOn = bio.CreatedOn;
//    biodata.ModifiedBy = bio.ModifiedBy;
//    biodata.ModifiedOn = bio.ModifiedOn;
//    biodata.DeletedBy = bio.DeletedBy;
//    biodata.DeletedOn = bio.DeletedOn;
//    biodata.IsDelete = true;

//    db.Update(biodata);


//    customer.Id = cust.Id;
//    customer.BiodataId = cust.BiodataId;
//    customer.Dob = cust.Dob;
//    customer.Gender = cust.Gender;
//    customer.BloodGroupId = cust.BloodGroupId;
//    customer.RhesusType = cust.RhesusType;
//    customer.Height = cust.Height;
//    customer.Weight = cust.Weight;

//    customer.CreatedBy = cust.CreatedBy;
//    customer.CreatedOn = cust.CreatedOn;
//    customer.ModifiedBy = cust.ModifiedBy;
//    customer.ModifiedOn = cust.ModifiedOn;
//    customer.DeletedBy = cust.DeletedBy;
//    customer.DeletedOn = cust.DeletedOn;
//    customer.IsDelete = true;

//    db.Update(customer);

//    customerMember.Id = custMember.Id;
//    customerMember.ParentBiodataId = custMember.ParentBiodataId;
//    customerMember.CustomerId = custMember.CustomerId;
//    customerMember.CustomerRelationId = custMember.CustomerRelationId;
//    customerMember.CreatedBy = custMember.CreatedBy;
//    customerMember.CreatedOn = custMember.CreatedOn;
//    customerMember.ModifiedBy = custMember.ModifiedBy;
//    customerMember.ModifiedOn = custMember.ModifiedOn;
//    customerMember.DeletedBy = custMember.DeletedBy;
//    customerMember.DeletedOn = custMember.DeletedOn;
//    customerMember.IsDelete = true;

//    db.Update(customerMember);
//}
//}