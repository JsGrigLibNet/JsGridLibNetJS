﻿namespace SampleHttpsServer
{
    using System;
    using JsGridLib.Contracts;

    public class ViewableEmployee 
    {
        public string Id { get; set; }

        public string IdForeign { get; set; }

        public string Name { get; set; }
        public string Position { get; set; }
        public string Contact { get; set; }
        public DateTime StartDate { get; set; }
    }
}