using EnterpriseResourcesWebAPI.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;

namespace EnterpriseResourcesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EnterpriseResourcesConnection"));

            var dbList = dbClient.GetDatabase("electrogio_db").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EnterpriseResourcesConnection"));

            int LastEmployeeId = dbClient.GetDatabase("electrogio_db").GetCollection<Department>("Employee").AsQueryable().Count();
            emp.EmployeeId = LastEmployeeId + 1;

            dbClient.GetDatabase("electrogio_db").GetCollection<Employee>("Employee").InsertOne(emp);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EnterpriseResourcesConnection"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", emp.EmployeeId);

            var update = Builders<Employee>.Update.Set("EmployeeName", emp.EmployeeName)
                                                    .Set("Department", emp.Department)
                                                    .Set("DateOfJoining", emp.DateOfJoining)
                                                    .Set("PhotoFileName", emp.PhotoFileName);

            dbClient.GetDatabase("electrogio_db").GetCollection<Employee>("Employee").UpdateOne(filter, update);

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EnterpriseResourcesConnection"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", id);


            dbClient.GetDatabase("electrogio_db").GetCollection<Employee>("Employee").DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("user.png");
            }
        }


    }
}
