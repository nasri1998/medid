using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DALocationLevel
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DALocationLevel(Med322_BContext _db)
        {
            db = _db;
        }

        public VMResponse GetAll()
        {
            try
            {
                List<VMLocationLevel> locationLevels = (
                from ll in db.MLocationLevels
                where ll.IsDelete == false
                select new VMLocationLevel
                {   
                    Id= ll.Id,
                    Name = ll.Name,
                    Abbreviation= ll.Abbreviation,
                    CreatedBy = ll.CreatedBy,
                    CreatedOn= ll.CreatedOn,
                    ModifiedBy= ll.ModifiedBy,
                    ModifiedOn= ll.ModifiedOn,
                    DeletedBy= ll.DeletedBy,
                    DeletedOn= ll.DeletedOn,
                    IsDelete= ll.IsDelete,
                }).ToList();

                if (locationLevels.Count < 1)
                {
                    response.Message = "location level table has no data!";
                    response.Success = false;
                    return response;
                }
                response.data = locationLevels;
                response.Message = "location level data successfully fetched!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        private VMLocationLevel? GetById(int id)
        {
            return (from ll in db.MLocationLevels
                    where ll.Id==id && ll.IsDelete == false
                    select new VMLocationLevel
                    {
                        Id = ll.Id,
                        Name = ll.Name,
                        Abbreviation = ll.Abbreviation,
                        CreatedBy = ll.CreatedBy,
                        CreatedOn = ll.CreatedOn,
                        ModifiedBy = ll.ModifiedBy,
                        ModifiedOn = ll.ModifiedOn,
                        DeletedBy = ll.DeletedBy,
                        DeletedOn = ll.DeletedOn,
                        IsDelete = ll.IsDelete,
                    }).FirstOrDefault();
                
        }

        public VMResponse Get(int id)
        {
            try
            {
                if (id < 1)
                {
                    response.Message = "No location level Id was submitted";
                    response.Success = false;
                    return response;
                }

                // get all the category data
                response.data = GetById(id);

                // check if data is null
                if (response.data == null)
                {
                    response.Message = $"location level with ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"location level with ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse CreateUpdate(VMLocationLevel inputll)
        {
            try
            {
                MLocationLevel data = new MLocationLevel();

                
                data.Name = inputll.Name;
                data.Abbreviation = inputll.Abbreviation;

                if (inputll.Id < 1)
                {

                    data.CreatedBy = inputll.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);
                    response.Message = "New location level Data successfully inserted!";
                }
                else
                {
                    VMLocationLevel? locationLevel = GetById((int)inputll.Id);

                    if (locationLevel == null)
                    {
                        response.Success = false;
                        response.Message = " location level data does not exist!";
                        return response;
                    }
                    else
                    {
                        data.Id = locationLevel.Id;

                        data.CreatedBy = locationLevel.CreatedBy;
                        data.CreatedOn = locationLevel.CreatedOn;

                        data.ModifiedBy = inputll.ModifiedBy;
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

                MLocationLevel data = new MLocationLevel();
                VMLocationLevel ? levellocation = GetById(id);

                if (levellocation == null)
                {
                    response.Success = false;
                    response.Message = "data does not exist!";

                    return response;
                }

                data.Id = levellocation.Id;
                data.Name = levellocation.Name;
                data.Abbreviation = levellocation.Abbreviation;
                data.CreatedBy = levellocation.CreatedBy;
                data.CreatedOn = levellocation.CreatedOn;
                data.ModifiedBy = levellocation.ModifiedBy;
                data.ModifiedOn = levellocation.ModifiedOn;
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
