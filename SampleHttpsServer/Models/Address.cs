namespace SampleHttpsServer
{
    public  class Employee : IJsGridEntity
    {
        public string EmployeeName { get; set; }
        public string EmployeePosition { get; set; }
        public string EmployeeContect { get; set; }

        public string Id { get; set; }
    }
}