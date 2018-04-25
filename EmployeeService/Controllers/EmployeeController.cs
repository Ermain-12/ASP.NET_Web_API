using EmployeeDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmployeeService.Controllers
{

    [EnableCorsAttribute("*", "*", "*")]
    [RequireHttps]
    public class EmployeeController : ApiController
    {
        
        public IEnumerable<Employee> Get()
        {
            using (StoreDBEntities empEntities = new StoreDBEntities())
            {
                return empEntities.Employees.ToList();
            }
        }
        

        [BasicAuthentication]
        public  HttpResponseMessage Get(string gender = "All")
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            using(StoreDBEntities empEntities = new StoreDBEntities())
            {
                switch (gender.ToLower())
                {
                    case "All":
                        return Request.CreateResponse(HttpStatusCode.OK, empEntities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK,
                                    empEntities.Employees.Where(e => e.Gender.ToLower() == "male"));
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                                    empEntities.Employees.Where(e => e.Gender.ToLower() == "female"));
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    // default:
                        // return Request.CreateResponse(HttpStatusCode.OK, empEntities.Employees.ToList());
                    // default:
                        // return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value for Gender must be" +
                            // " 'all', 'male' or 'female'");
                }
            }
        }

        [HttpGet]
        public HttpResponseMessage EmployeeByID(int id)
        {
            using (StoreDBEntities empEnitities = new StoreDBEntities())
            {
                var entity = empEnitities.Employees.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with {id} Not Found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (StoreDBEntities empEntities = new StoreDBEntities())
                {
                    empEntities.Employees.Add(employee);
                    empEntities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);

                    // Below, the ID of the object is included so that it can be found
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // Prevent a Cross-Origin user from deleting from the resource
        [DisableCors]
        public HttpResponseMessage Delete(int id)
        {

            try
            {
                using (StoreDBEntities empEntities = new StoreDBEntities())
                {
                    var entity = empEntities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with ID: {id} Not Found to delete");
                    }
                    else
                    {
                        empEntities.Employees.Remove(entity);
                        empEntities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put([FromBody]int id, [FromUri]Employee employee)
        {
            try
            {
                using (StoreDBEntities empEntities = new StoreDBEntities())
                {
                    var entity = empEntities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"No Employee with ID: {id} found to update");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;


                        empEntities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
