using MongoDB.Bson;

namespace EnterpriseResourcesWebAPI.Models
{
    public class Employee
    {
        public ObjectId Id { get; set; }
        public int EmployeeId { get; set; }
        public required string EmployeeName { get; set; }
        public required string Department { get; set; }
        public required string DateOfJoining { get; set; }
        public required string PhotoFileName { get; set; }
    }
}