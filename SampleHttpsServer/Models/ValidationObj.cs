namespace SampleHttpsServer
{
    using System.Collections.Generic;

    public class ValidationObj
    {
        public static object Validation = new
        {
            rules = new
            {
                name = "required",
                age = new
                {
                    required = true,
                    range = new List<int>() { 18, 150 }
                },
                address = new
                {
                    required = true,
                    minlength = 10
                },
                country = "required"
            },
            messages = new
            {
                name = "Please enter name",
                age = "Please enter valid age",
                address = "Please enter address (more than 10 chars)",
                country = "Please select country"
            }
        };
    }
}