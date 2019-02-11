namespace SampleHttpsServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using JsGridLib.Contracts;
    using JsGridLib.Controller;
    using JsGridLib.Implementations;
    using JsGridLib.Models;
    
    public class OrdersSchemaController : GenericJsGridSchemaController<PhoneBook> {}
    public class OrdersController : GenericJsGridController<PhoneBook>
    {
        static InMemoryJsGridDataStorage<PhoneBook> InMemoryJsGridDataStorage = new InMemoryJsGridDataStorage<PhoneBook>();
        public static List<Order> data = Enumerable.Range(0, 2500).Select(X =>
        {
            InMemoryJsGridDataStorage.Save(new PhoneBook(){Name = "my name " + X.ToString()});
            return new Order();
        }).ToList();
        public OrdersController(): base((db, filter) => db.Where(c => c != null), ValidationObj.Validation,InMemoryJsGridDataStorage)
        {
        }
    }

    public class PhoneBook : IJsGridEntity
    {
        public string Id { get; set; }

        public string IdForeign { get; set; }

        public string Name { get; set; }
        public bool Done { get; set; }

    }

    public class Order : IJsGridEntity
    {
        public Order()
        {
            Id = this.OrderID.ToString();
        }
        public int OrderID;
        public string CustomerID;
        public string EmployeeID;

        public DateTime OrderDate { get; set; }

        public string ShipCity { get; set; }

        public string ShipCountry { get; set; }

        public int Freight { get; set; }

        public string ShipName { get; set; }

        public string Id { get; set; }

        public string IdForeign { get; set; }
    }
    //public class OrdersController : GenericJsGridController<PhoneBook>
    //{
    //   static InMemoryJsGridDataStorage<PhoneBook> InMemoryJsGridDataStorage = new InMemoryJsGridDataStorage<PhoneBook>();
    //    public static List<Order> data = Enumerable.Range(0, 2500).Select(X =>
    //    {
    //        InMemoryJsGridDataStorage.Save(new PhoneBook()
    //        {
    //            Name = "my name "+X.ToString()
    //        });
    //        return new Order();
    //    }).ToList();
    //    public OrdersController()
    //        : base((db, filter) =>
    //            {
    //                return db.Where(c => c != null);
    //            }, 
    //            ValidationObj.Validation,
    //            InMemoryJsGridDataStorage
    //            /*
    //              ,  
    //             (s)=>
    //             {
    //                 return new ViewableEmployee()
    //                 {
    //                     Position = s.EmployeePosition,
    //                     Name = s.EmployeeName,
    //                     Id = s.Id,
    //                     Contact = s.EmployeeContect
    //                 };
    //             }, 
    //             (s) =>
    //             {
    //                 return new Employee()
    //                 {
    //                     EmployeePosition = s.Position,
    //                     EmployeeName = s.Name,
    //                     Id = s.Id,
    //                     EmployeeContect = s.Contact
    //                 };
    //             }
    //             */
    //            )
    //    {
    //    }
    //}


}