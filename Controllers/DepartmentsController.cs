using EnterpriseResourcesWebAPI.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;

namespace EnterpriseResourcesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient client = new MongoClient(_configuration.GetConnectionString("EnterpriseResourcesConnection"));
            var dbList = client.GetDatabase("electrogio_db").GetCollection<Department>("Department").AsQueryable();
            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EnterpriseResourcesConnection"));

            int lastDepartmentId = dbClient.GetDatabase("electrogio_db").GetCollection<Department>("Department").AsQueryable().Count();
            dep.DepartmentId = lastDepartmentId + 1;

            dbClient.GetDatabase("electrogio_db").GetCollection<Department>("Department").InsertOne(dep);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EnterpriseResourcesConnection"));

            var filter = Builders<Department>.Filter.Eq("DepartmentId", dep.DepartmentId);

            var update = Builders<Department>.Update.Set("DepartmentName", dep.DepartmentName);

            dbClient.GetDatabase("electrogio_db").GetCollection<Department>("Department").UpdateOne(filter, update);

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EnterpriseResourcesConnection"));

            var filter = Builders<Department>.Filter.Eq("DepartmentId", id);

            dbClient.GetDatabase("electrogio_db").GetCollection<Department>("Department").DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }
    }
}