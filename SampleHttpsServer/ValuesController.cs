namespace SampleHttpsServer
{
    using System.Linq;
    using JsGridLib.Controller;
    using JsGridLib.Implementations;

    public class ValuesController : GenericJsGridController<ViewableEmployee>
    {
        public ValuesController()
            : base((db, filter) =>
                {
                    return db.Where(c => c != null);
                }, 
                ValidationObj.Validation, 
                new InMemoryJsGridDataStorage<ViewableEmployee>()
                /*
                  ,  
                 (s)=>
                 {
                     return new ViewableEmployee()
                     {
                         Position = s.EmployeePosition,
                         Name = s.EmployeeName,
                         Id = s.Id,
                         Contact = s.EmployeeContect
                     };
                 }, 
                 (s) =>
                 {
                     return new Employee()
                     {
                         EmployeePosition = s.Position,
                         EmployeeName = s.Name,
                         Id = s.Id,
                         EmployeeContect = s.Contact
                     };
                 }
                 */
                )
        {
        }
    }
}