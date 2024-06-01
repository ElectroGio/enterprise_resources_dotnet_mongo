using MongoDB.Bson;

namespace EnterpriseResourcesWebAPI.Models
{
    public class Department
    {
        public ObjectId Id { get; set; }
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
    }
}
