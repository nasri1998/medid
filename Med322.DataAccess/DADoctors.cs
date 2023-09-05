using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.CodeAnalysis;
using System.Security.Cryptography;

namespace Med322.DataAccess
{
    public class DADoctors
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();
        //public  DASpecialization sp;
        public DADoctors(Med322_BContext _db)
        {
            db = _db;
        }

        public VMResponse GetAll()
        {
            try
            {
                List<VMMDoctorProfile>? data = (
                from d in db.MDoctors
                join b in db.MBiodata on d.BiodataId equals b.Id
                join ded in db.MDoctorEducations on d.Id equals ded.DoctorId
                join dedl in db.MEducationLevels on ded.EducationLevelId equals dedl.Id
                join dof in db.TDoctorOffices on d.Id equals dof.DoctorId
                join dt in db.TDoctorTreatments on d.Id equals dt.DoctorId
                join mf in db.MMedicalFacilities on dof.MedicalFacilityId equals mf.Id
                join loc in db.MLocations on mf.LocationId equals loc.Id
                where d.IsDelete == false
                select new VMMDoctorProfile
                {
                    Id = d.Id,
                    Fullname = b.Fullname,
                    ImagePath = b.ImagePath,

                    specializationName = dof.Specialization,

                    treatmentId = dt.Id,
                    treatmentName = dt.Name,

                    MedicalFacilityId = dof.MedicalFacilityId,
                    MedicalFacilityName = mf.Name,
                    location = loc.Name,

                    StartDate = dof.StartDate,
                    EndDate = dof.EndDate,

                    InstitutionName = ded.InstitutionName,
                    Major = ded.Major,
                    edEndYear = ded.EndYear,

                    CreatedBy = d.CreatedBy,
                    CreatedOn = d.CreatedOn,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedOn = d.ModifiedOn,
                    DeletedBy = d.DeletedBy,
                    DeletedOn = d.DeletedOn,
                    IsDelete = d.IsDelete
                }
                ).ToList();

                response.data = data;
                response.Message = "success to get data";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public VMMDoctor? GetId(long Id)
        {
            return (
                from d in db.MDoctors
                join b in db.MBiodata on d.BiodataId equals b.Id
                where d.IsDelete == false && d.Id == Id
                select new VMMDoctor
                {
                    Id = d.Id,

                    BiodataId = b.Id,
                    Str = d.Str,

                    CreatedBy = d.CreatedBy,
                    CreatedOn = d.CreatedOn,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedOn = d.ModifiedOn,
                    DeletedBy = d.DeletedBy,
                    DeletedOn = d.DeletedOn,
                    IsDelete = d.IsDelete
                }
                ).FirstOrDefault();
        }

        public VMBiodata GetBiodataById(long id)
        {
            return (from b in db.MBiodata
                    where b.IsDelete == false && b.Id == id
                    select new VMBiodata
                    {
                        Id = b.Id,
                        Fullname = b.Fullname,
                        ImagePath = b.ImagePath,
                        Image = b.Image,
                        MobilePhone = b.MobilePhone,

                        CreatedBy = b.CreatedBy,
                        CreatedOn = b.CreatedOn,
                        ModifiedBy = b.ModifiedBy,
                        ModifiedOn = b.ModifiedOn,
                        DeletedBy = b.DeletedBy,
                        DeletedOn = b.DeletedOn,
                        IsDelete = b.IsDelete
                    }
                ).FirstOrDefault();
        }

        public VMResponse GetById(long id)
        {
            try
            {
                List<VMMDoctorProfile>? data = (
                from d in db.MDoctors
                join b in db.MBiodata on d.BiodataId equals b.Id
                join ded in db.MDoctorEducations on d.Id equals ded.DoctorId
                join dedl in db.MEducationLevels on ded.EducationLevelId equals dedl.Id
                join dof in db.TDoctorOffices on d.Id equals dof.DoctorId
                join dt in db.TDoctorTreatments on d.Id equals dt.DoctorId
                join mf in db.MMedicalFacilities on dof.MedicalFacilityId equals mf.Id
                join loc in db.MLocations on mf.LocationId equals loc.Id
                where d.IsDelete == false && d.Id == id
                select new VMMDoctorProfile
                {
                    Id = d.Id,
                    Fullname = b.Fullname,
                    ImagePath = b.ImagePath,

                    biodataId = b.Id,
                    specializationName = dof.Specialization,

                    treatmentId = dt.Id,
                    treatmentName = dt.Name,

                    MedicalFacilityId = dof.MedicalFacilityId,
                    MedicalFacilityName = mf.Name,
                    location = loc.Name,

                    StartDate = dof.StartDate,
                    EndDate = dof.EndDate,

                    InstitutionName = ded.InstitutionName,
                    Major = ded.Major,
                    edEndYear = ded.EndYear,

                    CreatedBy = d.CreatedBy,
                    CreatedOn = d.CreatedOn,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedOn = d.ModifiedOn,
                    DeletedBy = d.DeletedBy,
                    DeletedOn = d.DeletedOn,
                    IsDelete = d.IsDelete
                }
                ).ToList();

                response.data = data;
                response.Message = "success to get data";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public VMResponse AddEditImage(VMBiodata inputData)
        {
            try
            {
                MBiodatum biodata = new MBiodatum();

                biodata.ImagePath = inputData.ImagePath;

                VMBiodata data = GetBiodataById(inputData.Id);

                if (response.data == null)
                {
                    response.Success = false;
                    response.Message = "No data";
                }
                else
                {
                    biodata.Id = data.Id;
                    biodata.Fullname = data.Fullname;
                    biodata.Image = data.Image;

                    biodata.CreatedBy = data.CreatedBy;
                    biodata.CreatedOn = data.CreatedOn;
                    biodata.ModifiedBy = inputData.ModifiedBy;
                    biodata.ModifiedOn = DateTime.Now;

                    biodata.DeletedBy = data.DeletedBy;
                    biodata.DeletedOn = data.DeletedOn;
                    biodata.IsDelete = data.IsDelete;


                    db.Update(biodata);
                    response.Success = true;

                    db.SaveChanges();
                    response.data = biodata;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public VMResponse CreateUpdatespecialization(VMTCurrentDoctorSpecialization inputSpecialization)
        {
            try
            {
                TCurrentDoctorSpecialization data = new TCurrentDoctorSpecialization(); // buat vm tcurrent spesialisasi
                //VMMDoctor? doctor = GetId(inputSpecialization.DoctorId); // ngambil dr vm m doctor
                VMTCurrentDoctorSpecialization? Tspecialization = GetTSpecialization(inputSpecialization.DoctorId);
                //VMMSpecialization specialization = GetSpecializationById(inputSpecialization.SpecializationId); // ngambil dari mspecialisasi

                data.SpecializationId = inputSpecialization.SpecializationId;

                if (Tspecialization == null)  // buat nambah
                {
                    data.DoctorId = inputSpecialization.DoctorId;

                    data.CreatedBy = inputSpecialization.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);

                    response.Message = "specialization has been added";
                }
                else
                {
                    //if (Tspecialization == null)
                    //{
                    //    response.Success = false;
                    //    response.Message = "Specialization Not exist";
                    //}
                    //else
                    //{
                    data.Id = Tspecialization.Id;

                    data.DoctorId = Tspecialization.DoctorId;

                    data.CreatedBy = Tspecialization.CreatedBy;
                    data.CreatedOn = Tspecialization.CreatedOn;
                    data.ModifiedBy = inputSpecialization.Id;
                    data.ModifiedOn = DateTime.Now;

                    data.DeletedBy = Tspecialization.DeletedBy;
                    data.DeletedOn = Tspecialization.DeletedOn;
                    data.IsDelete = Tspecialization.IsDelete;

                    db.Update(data);

                    response.Message = "succesfull to edit";
                    //}
                }
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

        public VMMSpecialization GetSpecializationById(long id)
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

        public VMTCurrentDoctorSpecialization GetTSpecialization(long DoctorId)
        {
            VMTCurrentDoctorSpecialization? Tspecialization = new VMTCurrentDoctorSpecialization();
            try
            {
                Tspecialization = (
                    from ts in db.TCurrentDoctorSpecializations
                    where ts.IsDelete == false && ts.DoctorId == DoctorId
                    select new VMTCurrentDoctorSpecialization
                    {
                        Id = ts.Id,
                        DoctorId = ts.DoctorId,
                        SpecializationId = ts.SpecializationId,

                        CreatedBy = ts.CreatedBy,
                        CreatedOn = ts.CreatedOn,
                        ModifiedBy = ts.ModifiedBy,
                        ModifiedOn = ts.ModifiedOn,

                        DeletedBy = ts.DeletedBy,
                        DeletedOn = ts.DeletedOn,
                        IsDelete = ts.IsDelete
                    }
                ).FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return Tspecialization;
        }

        public VMResponse GetByFilter(long filterTreatmentId = 0, long filterLocationId = 0, string filterDoctor = null, string filterSpecialization = null)
        {
            try
            {
                string sql = "select b.id, b.fullname, b.image_path, d.id doctor_id, dof.id doctor_office_id, dof.medical_facility_id, dof.specialization, " +
                    "datediff(year,dof.start_date, GETDATE()) experience ,dof.end_date, mf.name medical_facility_name, mf.location_id location_id, loc.name location_name, " +
                    "dt.id doctor_treatment_id, dt.name doctor_treatment_name, " +
                    "d.created_by, d.created_on, d.modified_by, d.modified_on, d.deleted_by, d.deleted_on, d.is_delete " +
                    "from M_Doctor d " +
                    "inner join M_Biodata b on d.Biodata_Id = b.Id " +
                    "inner join T_Doctor_Office dof on d.Id = dof.doctor_id " +
                    "inner join m_medical_facility mf on dof.medical_facility_id = mf.Id " +
                    "inner join t_doctor_treatment dt on d.Id = dt.doctor_id " +
                    "inner join M_Location loc on mf.Location_Id = loc.Id " +
                    "where 1=1 and d.is_delete = 0 ";


                if (filterTreatmentId > 0)
                {
                    sql += $" and dt.id = {filterTreatmentId}";
                }

                if (filterLocationId > 0)
                {
                    sql += $" and mf.location_id = {filterLocationId}";
                }

                if (filterDoctor != null)
                {
                    sql += $" and b.fullname like \'%{filterDoctor}%\'";
                }

                if (filterSpecialization != null)
                {
                    sql += $" and dof.specialization like \'%{filterSpecialization}%\'";
                }
                List<VMViewFindDoctor>? find = (
                from fd in db.VWFindDoctor.FromSqlRaw(sql)
                select new VMViewFindDoctor
                {
                    Id = fd.Id,
                    Fullname = fd.Fullname,
                    ImagePath = fd.ImagePath,

                    DoctorId = fd.Id,
                    Specialization = fd.Specialization,

                    DoctorOfficeId = fd.DoctorOfficeId,
                    MedicalFacilityId = fd.MedicalFacilityId,
                    MedicalFacilityName = fd.MedicalFacilityName,
                    Experience = fd.Experience,
                    EndDate = fd.EndDate,

                    LocationId = fd.LocationId,
                    LocationName = fd.LocationName,

                    DoctorTreatmentId = fd.DoctorTreatmentId,
                    DoctorTreatmentName = fd.DoctorTreatmentName,

                    CreatedBy = fd.CreatedBy,
                    CreatedOn = fd.CreatedOn,
                    ModifiedBy = fd.ModifiedBy,
                    ModifiedOn = fd.ModifiedOn,
                    DeletedBy = fd.DeletedBy,
                    DeletedOn = fd.DeletedOn,
                    IsDelete = fd.IsDelete
                }
                ).ToList();

                response.data = find;

                if (response.data == null)
                {
                    response.Message = "not find anything";
                }
                response.Message = "success to filter";

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public VMResponse GetTreatmentAll()
        {
            List<VMTDoctorTreatment>? treatment = new List<VMTDoctorTreatment>();
            try
            {
                treatment = (
                    from t in db.TDoctorTreatments
                    where t.IsDelete == false
                    select new VMTDoctorTreatment
                    {
                        Id = t.Id,
                        Name = t.Name,

                        CreatedBy = t.CreatedBy,
                        CreatedOn = t.CreatedOn,
                        ModifiedBy = t.ModifiedBy,
                        ModifiedOn = t.ModifiedOn,

                        DeletedBy = t.DeletedBy,
                        DeletedOn = t.DeletedOn,
                        IsDelete = t.IsDelete
                    }
                ).ToList();

                response.data = treatment;
                response.Message = "data success to be fetched";

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public VMResponse DetailDoctor(long Id)
        {
            try
            {
                string sql = "select d.id doctor_id, dof.id doctor_office_id, dof.medical_facility_id medical_facility_id, mf.name medical_facility_name, " +
                    "mfc.name medical_facility_category_name, dof.specialization, dof.start_date, dof.end_date, mf.full_address medical_facility_address, " +
                    "mfs.day, mfs.time_schedule_start, mfs.time_schedule_end, dotp.price_start_from, " +
                    "d.created_by, d.created_on, d.modified_by, d.modified_on, d.deleted_by, d.deleted_on, d.is_delete " +
                    "from m_doctor d inner join t_doctor_office dof on d.id = dof.doctor_id " +
                    "inner join m_medical_facility mf on dof.medical_facility_id = mf.id " +
                    "inner join m_medical_facility_category mfc on mf.medical_facility_category_id = mfc.id " +
                    "inner join m_medical_facility_schedule mfs on mfs.medical_facility_id = mf.id " +
                    "inner join t_doctor_office_schedule dos on dos.medical_facility_schedule_id = mfs.id " +
                    "inner join t_doctor_office_treatment dot on dot.doctor_office_id = dof.id " +
                    "inner join t_doctor_office_treatment_price dotp on dotp.doctor_office_treatment_id = dot.id " +
                    $"where d.is_delete = 0 and d.id = {Id}";

                List<VMDoctorDetail>? doctor = (
                from fd in db.VWDoctorDetail.FromSqlRaw($"{sql}").OrderByDescending(p => p.OfficeScheduleDay)
                select new VMDoctorDetail
                {
                    DoctorId = fd.DoctorId,

                    Specialization = fd.Specialization,

                    OfficeId = fd.OfficeId,
                    MedicalFacilityId = fd.MedicalFacilityId,
                    MedicalFacilityName = fd.MedicalFacilityName,
                    MedicalFacilityCategoryName = fd.MedicalFacilityCategoryName,
                    MedicalFacilityAddress = fd.MedicalFacilityAddress,
                    OfficeScheduleDay = fd.OfficeScheduleDay,
                    OfficeScheduleStart = fd.OfficeScheduleStart,
                    OfficeScheduleEnd = fd.OfficeScheduleEnd,
                    StartDate = fd.StartDate,
                    EndDate = fd.EndDate,
                    PriceStartFrom = fd.PriceStartFrom,

                    CreatedBy = fd.CreatedBy,
                    CreatedOn = fd.CreatedOn,
                    ModifiedBy = fd.ModifiedBy,
                    ModifiedOn = fd.ModifiedOn,
                    DeletedBy = fd.DeletedBy,
                    DeletedOn = fd.DeletedOn,
                    IsDelete = fd.IsDelete
                }
                ).ToList();

                response.data = doctor;

                if (response.data == null)
                {
                    response.Message = "not find anything";
                }
                response.Message = "success to filter";

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public VMResponse DoctorAppointment(long Id)
        {
            try
            {
                List<VMDoctorAppointment> appointments = (
                    from a in db.TAppointments
                    join c in db.MCustomers on a.CustomerId equals c.Id
                    join b in db.MBiodata on c.BiodataId equals b.Id
                    join dof in db.TDoctorOffices on a.DoctorOfficeId equals dof.Id
                    join mf in db.MMedicalFacilities on dof.MedicalFacilityId equals mf.Id
                    join dos in db.TDoctorOfficeSchedules on a.DoctorOfficeScheduleId equals dos.Id
                    join mfs in db.MMedicalFacilitySchedules on dos.MedicalFacilityScheduleId equals mfs.Id
                    join dot in db.TDoctorOfficeTreatments on a.DoctorOfficeTreatmentId equals dot.Id
                    join dt in db.TDoctorTreatments on dot.DoctorTreatmentId equals dt.Id
                    where a.IsDelete == false && dof.DoctorId == Id
                    select new VMDoctorAppointment
                    {
                        Id = Id,
                        CustomerId = a.CustomerId,
                        CustomerName = b.Fullname,
                        
                        DoctorOfficeId = a.DoctorOfficeId,
                        OfficeName = mf.Name,
                        DoctorId = dof.DoctorId,

                        DoctorOfficeScheduleId = a.DoctorOfficeScheduleId,
                        AppointmentDate = a.AppointmentDate,
                        DayName = mfs.Day,
                        StartHour = mfs.TimeScheduleStart,
                        EndHour = mfs.TimeScheduleStart,

                        DoctorOfficeTreatmentId = a.DoctorOfficeTreatmentId,
                        TreatmentName = dt.Name,

                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        ModifiedBy = a.ModifiedBy,
                        ModifiedOn = a.ModifiedOn,

                        DeletedBy = a.DeletedBy,
                        DeletedOn = a.DeletedOn,
                        IsDelete = a.IsDelete

                    }).ToList();

                response.data = appointments;
                response.Message = "success to get appointment";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
