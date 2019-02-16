namespace SampleHttpsServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using JsGridLib.Contracts;
    using JsGridLib.Controller;
    using JsGridLib.Implementations;
    using JsGridLib.Models;
    
    public class OrdersController : GenericJsGridController
    {
       public OrdersController() : base((db, filter) => db, null, DbService.InMemoryJsGridDataStorage)
        {
        }
    }
    public class OrdersSchemaController : GenericJsGridSchemaFromDataSourceController
    {
        public OrdersSchemaController(): base(DbService.InMemoryJsGridDataStorage)
        {
        }
    }
    public static class DbService
    {
        public static string TableName = "Orders";
        public static InMemoryJsGridDataStorage InMemoryJsGridDataStorage = new InMemoryJsGridDataStorage();
        public static object data = Enumerable.Range(0, 2500).Select(X =>
        {
            try
            {
                var p = new
                {
                    Birthday = DateTime.UtcNow.AddDays(X),
                    Name = "my name " + X.ToString()
                }.ToExpandoObject();
                InMemoryJsGridDataStorage.Save(TableName, p);

            }
            catch (Exception e)
            {

            }
            return new object();
        }).ToList();

    }
    //public class OrdersSchemaController : GenericJsGridSchemaController
    //{
    //    public OrdersSchemaController()
    //        : base(new
    //        {
    //            Birthday = DateTime.UtcNow,
    //            Name = ""
    //        })
    //    {
    //    }
    //}
    //public class OrdersSchemaController : GenericJsGridSchemaController
    //{
    //    public OrdersSchemaController()
    //        : base(new PhoneBook())
    //    {
    //    }
    //}
    //public class OrdersSchemaController : GenericJsGridSchemaController
    //{
    //    public OrdersSchemaController()
    //        : base(new
    //        {
    //            Birthday = default(DateTime),
    //            Name = "",
    //            Id = Guid.NewGuid().ToString().Replace("-", "")
    //        })
    //    {
    //    }
    //}
    public static class ExpandoExtension
    {
        public static ExpandoObject ToExpandoObject(this object obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
            {
                expando.Add(property.Name, property.GetValue(obj));
            }

            return (ExpandoObject)expando;
        }
    }
    public class PhoneBook 
    {
        public string Id { get; set; }
        public DateTime Birthday  { get; set; }

        public string Name { get; set; }

    }

    public class Order 
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