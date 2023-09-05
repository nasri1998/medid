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
    public class DACancelAppointment
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DACancelAppointment(Med322_BContext _db)
        {
            db = _db;
        }

        public VMResponse GetAll(int userId)
        {
            try
            {
                List<VMListJanji> data = (
                    from ca in db.DMListJanjis.FromSqlRaw($"select * from vwlistpatient where userid = {userId} and IsDelete = 0").ToList()
                    select new VMListJanji
                    {
                        id = ca.id,
                        userid = ca.userid,
                        custid = ca.custid,
                        iddokter = ca.iddokter,
                        idrelasi = ca.idrelasi,
                        namapasien = ca.namapasien,
                        dokname = ca.dokname,
                        dokofid = ca.dokofid,
                        dokofscheid = ca.dokofscheid,
                        dokoftreatid = ca.dokoftreatid,
                        appdate = ca.appdate,
                        namafaskes = ca.namafaskes,
                        tindakanmedis = ca.tindakanmedis,

                        
                        createdby = ca.createdby,
                        createdon = ca.createdon,
                        modifiedby = ca.modifiedby,
                        modifiedon = ca.modifiedon,
                        deletedby = ca.deletedby,
                        deletedon = ca.deletedon,
                        isdelete = ca.isdelete
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

        public VMResponse GetByFilter(string filter, int Id)
        {
            try
            {
                List<VMListJanji> data = (
                    from ca in db.DMListJanjis.FromSqlRaw($"select * from vwlistpatient where userid = {Id} and IsDelete = 0").ToList()
                    where ca.userid == Id && (ca.namapasien?.ToLower() + " " + ca.dokname?.ToLower()).Contains(filter.ToLower())
                    select new VMListJanji
                    {
                        id = ca.id,
                        userid = ca.userid,
                        custid = ca.custid,
                        iddokter = ca.iddokter,
                        namapasien = ca.namapasien,
                        dokname = ca.dokname,
                        dokofid = ca.dokofid,
                        dokofscheid = ca.dokofscheid,
                        dokoftreatid = ca.dokoftreatid,
                        appdate = ca.appdate,
                        namafaskes = ca.namafaskes,
                        tindakanmedis = ca.tindakanmedis,


                        createdby = ca.createdby,
                        createdon = ca.createdon,
                        modifiedby = ca.modifiedby,
                        modifiedon = ca.modifiedon,
                        deletedby = ca.deletedby,
                        deletedon = ca.deletedon,
                        isdelete = ca.isdelete
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
        public VMListJanji? GetByIdPatient(int appID)
        {
            var pmdQuery = db.DMListJanjis.FromSqlRaw($"select * from vwlistpatient where id = {appID} and IsDelete = 0");
            var ca = pmdQuery.FirstOrDefault();

            if (ca != null)
            {
                var result = new VMListJanji
                {
                    userid = ca.userid,
                    id = ca.id,
                    custid = ca.custid,
                    iddokter = ca.iddokter,
                    idrelasi = ca.idrelasi,
                    namapasien = ca.namapasien,
                    dokname = ca.dokname,
                    dokofid = ca.dokofid,
                    dokofscheid = ca.dokofscheid,
                    dokoftreatid = ca.dokoftreatid,
                    appdate = ca.appdate,
                    namafaskes = ca.namafaskes,
                    tindakanmedis = ca.tindakanmedis,


                    createdby = ca.createdby,
                    createdon = ca.createdon,
                    modifiedby = ca.modifiedby,
                    modifiedon = ca.modifiedon,
                    deletedby = ca.deletedby,
                    deletedon = ca.deletedon,
                    isdelete = ca.isdelete
                };

                return result;
            }

            return null; // Atau kembalikan null jika tidak ada hasil yang sesuai.
        }

        public VMListEdit? GetByIdPatientEdit(int appID, int relid)
        {
            var pmdQuery = db.DMListEdits.FromSqlRaw($"select * from vweditpasien where id = {appID} and idrelasi = {relid} and IsDelete = 0");
            var ca = pmdQuery.FirstOrDefault();

            if (ca != null)
            {
                var result = new VMListEdit
                {
                    userid = ca.userid,
                    custid = ca.custid,
                    iddokter = ca.iddokter,
                    namapasien = ca.namapasien,
                    dokid = ca.dokid,
                    dokname = ca.dokname,
                    doktreatid = ca.doktreatid,
                    appdate = ca.appdate,
                    namafaskes = ca.namafaskes,
                    tindakanmedis = ca.tindakanmedis,
                    idrelasi = ca.idrelasi,
                    namarelasi = ca.namarelasi,
                    createdby = ca.createdby,
                    createdon = ca.createdon,
                    modifiedby = ca.modifiedby,
                    modifiedon = ca.modifiedon,
                    deletedby = ca.deletedby,
                    deletedon = ca.deletedon,
                    isdelete = ca.isdelete
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

        public VMResponse GetEdit(int id, int relid)
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
                response.data = GetByIdPatientEdit(id, relid);

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

        public VMResponse GetAppointmentId(int id)
        {
            try
            {
                List<VMCancelAppointment> data = (
                from ca in db.DMCancelAppointments
                    where ca.appointment_id == id
                    select new VMCancelAppointment
                    {
                        userid = ca.userid,
                        parentBioId = ca.parentBioId,
                        biodataId = ca.biodataId,
                        customerId = ca.customerId,
                        Fullname = ca.Fullname,
                        Dob = ca.Dob,
                        Gender = ca.Gender,
                        AppointmentId = ca.appointment_id,
                        DoctorOfficeId = ca.doctor_office_id,
                        DoctorId = ca.doctor_id,
                        doctortreatment = ca.doctortreatment,
                        DoctorName = ca.doctor_name,
                        DoctorSpec = ca.doctor_spec,
                        DoctorSchedule = ca.doctor_schedule,
                        Slot = ca.slot,
                        MedicFacility = ca.medic_facility,
                        AppDate = ca.app_date,
                        IdxDay = ca.idx_day,
                        DoctorTreatId = ca.doctor_treat_id,
                        Treatment = ca.treatment,
                        CostumerRelationId = ca.CostumerRelationId,
                        RelationName = ca.RelationName,

                        CreatedBy = ca.CreatedBy,
                        CreatedOn = ca.CreatedOn,
                        ModifiedBy = ca.ModifiedBy,
                        ModifiedOn = ca.ModifiedOn,
                        DeletedBy = ca.DeletedBy,
                        DeletedOn = ca.DeletedOn,
                        IsDelete = ca.IsDelete
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
        public VMResponse GetAllTreatment(int id)
        {
            try
            {
                List<VMTDoctorTreatment> data = (
                    from dt in db.TDoctorTreatments
                    where dt.DoctorId == id
                    select new VMTDoctorTreatment
                    {
                        Id = dt.Id,
                        DoctorId = dt.DoctorId,
                        Name = dt.Name,
                        CreatedBy = dt.CreatedBy,
                        CreatedOn = dt.CreatedOn,
                        ModifiedBy = dt.ModifiedBy,
                        ModifiedOn = dt.ModifiedOn,
                        DeletedBy = dt.DeletedBy,
                        DeletedOn = dt.DeletedOn,
                        IsDelete = dt.IsDelete
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

        public VMResponse Delete(int appoinid)
        {
            try
            {
                VMListJanji? data = GetByIdPatient(appoinid);

                TAppointmentCancellation customer = new TAppointmentCancellation();

                customer.AppointmentId = (int?)data.id;

                customer.CreatedBy = data.userid;
                customer.CreatedOn = DateTime.Now;


                db.Add(customer);

                TAppointment newData = new TAppointment();

                newData.Id = data.id;
                newData.CustomerId = data.custid;
                newData.DoctorOfficeId = data.dokofid;
                newData.DoctorOfficeScheduleId = data.dokofscheid;
                newData.DoctorOfficeTreatmentId = data.dokoftreatid;
                newData.AppointmentDate = data.appdate;
                
                newData.CreatedBy = (long)data.createdby;
                newData.CreatedOn = data.createdon;
                newData.ModifiedOn = data.modifiedon;
                newData.DeletedBy = data.userid;
                newData.DeletedOn = DateTime.Now;
                newData.IsDelete = true;

                db.Update(newData);

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
    }
}
