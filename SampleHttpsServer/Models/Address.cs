namespace SampleHttpsServer
{
    using JsGridLib.Contracts;

    public  class Employee : IJsGridEntity
    {
        public string EmployeeName { get; set; }
        public string EmployeePosition { get; set; }
        public string EmployeeContect { get; set; }

        public string Id { get; set; }

        public string IdForeign { get; set; }
    }
}