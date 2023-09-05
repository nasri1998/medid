using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DALocation
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DALocation (Med322_BContext _db)
        {
            db = _db;
        }

        public VMResponse GetAll()
        {
            try
            {
                List<VMLocation> location = (
                from l in db.MLocations
                join ll in db.MLocationLevels on l.Id equals ll.Id
                where l.IsDelete == false
                select new VMLocation
                {
                    Id = l.Id,
                    Name= l.Name,
                    ParentId= l.ParentId,

                    LocationLevelId= ll.Id,

                    CreatedBy= l.CreatedBy,
                    CreatedOn= l.CreatedOn,
                    ModifiedBy= l.ModifiedBy,
                    ModifiedOn= l.ModifiedOn,
                    DeletedBy= l.DeletedBy,
                    DeletedOn= l.DeletedOn,
                    IsDelete= l.IsDelete,

                }).ToList();

                if (location.Count < 1)
                {
                    response.Message = "location table has no data!";
                    response.Success = false;
                    return response;
                }
                response.data = location;
                response.Message = "location data successfully fetched!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        private VMLocation? GetById(int id)
        {
            return (from l in db.MLocations
                    join ll in db.MLocationLevels on l.Id equals ll.Id
                    where l.Id==id && l.IsDelete == false
                    select new VMLocation
                    {
                        Id = l.Id,
                        Name = l.Name,
                        //ParentId = l.ParentId,

                        LocationLevelId = ll.Id,

                        CreatedBy = l.CreatedBy,
                        CreatedOn = l.CreatedOn,
                        ModifiedBy = l.ModifiedBy,
                        ModifiedOn = l.ModifiedOn,
                        DeletedBy = l.DeletedBy,
                        DeletedOn = l.DeletedOn,
                        IsDelete = l.IsDelete,

                    }).FirstOrDefault();

        }

        public VMResponse Get(int id)
        {
            try
            {
                if (id < 1)
                {
                    response.Message = "No location Id was submitted";
                    response.Success = false;
                    return response;
                }

                // get all the category data
                response.data = GetById(id);

                // check if data is null
                if (response.data == null)
                {
                    response.Message = $"location with ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"location with ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse CreateUpdate(VMLocation inputloc)
        {
            try
            {
                MLocation data = new MLocation();

                data.Name = inputloc.Name;

                if (inputloc.Id < 1)
                {

                    data.CreatedBy = inputloc.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);
                    response.Message = "New location Data successfully inserted!";
                }
                else
                {
                    VMLocation? locationLevel = GetById((int)inputloc.Id);

                    if (locationLevel == null)
                    {
                        response.Success = false;
                        response.Message = " location data does not exist!";
                        return response;
                    }
                    else
                    {
                        data.Id = locationLevel.Id;

                        data.CreatedBy = locationLevel.CreatedBy;
                        data.CreatedOn = locationLevel.CreatedOn;

                        data.ModifiedBy = inputloc.ModifiedBy;
                        data.ModifiedOn = DateTime.Now;

                        db.Update(data);
                        response.Message = " location Data successfully updated!";
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

                MLocation data = new MLocation();
                VMLocation? location = GetById(id);

                if (location == null)
                {
                    response.Success = false;
                    response.Message = "data does not exist!";

                    return response;
                }

                data.Id = location.Id;
                data.Name = location.Name;
                data.LocationLevelId = location.LocationLevelId;
                data.CreatedBy = location.CreatedBy;
                data.CreatedOn = location.CreatedOn;
                data.ModifiedBy = location.ModifiedBy;
                data.ModifiedOn = location.ModifiedOn;
                data.DeletedBy = userid;
                data.DeletedOn = DateTime.Now;
                data.IsDelete = true;

                db.Update(data);
                db.SaveChanges();

                response.data = data;
                response.Message = "level location Successfully deleted!";

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
