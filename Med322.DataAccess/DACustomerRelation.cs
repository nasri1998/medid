using Med322.DataModels;
using Med322.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med322.DataAccess
{
    public class DACustomerRelation
    {
        private readonly Med322_BContext db;
        private VMResponse response = new VMResponse();

        public DACustomerRelation(Med322_BContext _db) {
            db = _db;
        }

        public VMResponse GetAll()
        {
            try
            {
                List<VMTblMCustomerRelation> cusRelation = (
                from cr in db.MCustomerRelations
                where cr.IsDelete == false
                select new VMTblMCustomerRelation
                {
                    Id = cr.Id,
                    Name = cr.Name,
                    CreatedBy = cr.CreatedBy,
                    CreatedOn = cr.CreatedOn,
                    ModifiedBy = cr.ModifiedBy,
                    ModifiedOn = cr.ModifiedOn,
                    DeletedBy = cr.DeletedBy,
                    DeletedOn = cr.DeletedOn,
                    IsDelete = cr.IsDelete
                }).ToList();

                if (cusRelation.Count < 1)
                {
                    response.Message = "Customer Relation table has no data!";
                    response.Success = false;
                    return response;
                }
                response.data = cusRelation;
                response.Message = "Customer Relation data successfully fetched!";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        private VMTblMCustomerRelation? GetById(int id)
        {
            return (from cr in db.MCustomerRelations
                    where cr.IsDelete == false
                    && cr.Id == id
                    select new VMTblMCustomerRelation
                    {
                        Id = cr.Id,
                        Name = cr.Name,
                        CreatedBy = cr.CreatedBy,
                        CreatedOn = cr.CreatedOn,
                        ModifiedBy = cr.ModifiedBy,
                        ModifiedOn = cr.ModifiedOn,
                        DeletedBy = cr.DeletedBy,
                        DeletedOn = cr.DeletedOn,
                        IsDelete = cr.IsDelete

                    }).FirstOrDefault();
        }

        public VMResponse GetByFilter(string filter)
        {
            try
            {
                List<VMTblMCustomerRelation> data = (from cr in db.MCustomerRelations
                                               where cr.IsDelete == false
                                               && cr.Name.ToLower().Contains(filter.ToLower())
                                               select new VMTblMCustomerRelation
                                               {
                                                   Id = cr.Id,
                                                   Name = cr.Name,
                                                   CreatedBy = cr.CreatedBy,
                                                   CreatedOn = cr.CreatedOn,
                                                   ModifiedBy = cr.ModifiedBy,
                                                   ModifiedOn = cr.ModifiedOn,
                                                   DeletedBy = cr.DeletedBy,
                                                   DeletedOn = cr.DeletedOn,
                                                   IsDelete = cr.IsDelete

                                               }).ToList();
                response.Success = true;
                response.Message = " Search data success!";
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

        public VMResponse Get(int id)
        {
            try
            {
                if (id < 1)
                {
                    response.Message = "No Customer Relation Id was submitted";
                    response.Success = false;
                    return response;
                }

                // get all the Customer Relation data
                response.data = GetById(id);

                // check if data is null
                if (response.data == null)
                {
                    response.Message = $"Customer Relation with ID = {id} data is not exist!";
                    response.Success = false;
                }
                else
                {
                    response.Message = $"Customer Relation with ID = {id} data successfully fetched!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public VMResponse CreateUpdate(VMTblMCustomerRelation inputrelation)
        {
            try
            {
                MCustomerRelation data = new MCustomerRelation();

                // Check if the code already exists
                if (db.MCustomerRelations.Any(rel => rel.Name == inputrelation.Name && rel.Id != inputrelation.Id && rel.IsDelete == false))
                {
                    response.Success = false;
                    response.Message = "Family with the same Name already exists!";
                    return response;
                }

                if (inputrelation.Id < 1)
                {
                    data.Name = inputrelation.Name;
                    data.CreatedBy = inputrelation.CreatedBy;
                    data.CreatedOn = DateTime.Now;

                    db.Add(data);
                    response.Message = "New Customer Relation Data successfully inserted!";
                }
                else
                {
                    VMTblMCustomerRelation? dataRelation = GetById((int)inputrelation.Id);

                    if (dataRelation == null)
                    {
                        response.Success = false;
                        response.Message = " Customer Relation data does not exist!";
                        return response;
                    }
                    else
                    {
                        data.Id = dataRelation.Id;
                        data.Name = inputrelation.Name;

                        data.CreatedBy = dataRelation.CreatedBy;
                        data.CreatedOn = dataRelation.CreatedOn;
                        data.ModifiedBy = inputrelation.ModifiedBy;
                        data.ModifiedOn = DateTime.Now;

                        db.Update(data);
                        response.Message = " Customer Relation data successfully updated!";

                    }
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

        public VMResponse Delete(int id, int userid)
        {
            try
            {
                    if (id < 1)
                    {
                        response.Message = "No Customer Relation id was submitted";
                        response.Success = false;
                        return response;
                    }

                    MCustomerRelation data = new MCustomerRelation();
                    VMTblMCustomerRelation? cusRel = GetById(id);

                    if (cusRel == null)
                    {   
                        response.Success=false;
                        response.Message = "Data doesn't exist";

                        return response;
                    }

                    data.Id = cusRel.Id;
                    data.Name = cusRel.Name;
                    data.CreatedBy = cusRel.CreatedBy;
                    data.CreatedOn = cusRel.CreatedOn;
                    data.ModifiedBy = cusRel.ModifiedBy;
                    data.ModifiedOn = cusRel.ModifiedOn;
                    data.DeletedBy = userid;
                    data.DeletedOn = DateTime.Now;
                    data.IsDelete = true;

                db.Update(data);
                db.SaveChanges();

                response.data = data;
                response.Message = "Data Successfully deleted!";
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
