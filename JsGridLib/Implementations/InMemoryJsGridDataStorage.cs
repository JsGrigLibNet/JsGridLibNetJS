namespace JsGridLib.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JsGridLib.Contracts;
    using JsGridLib.Models;

    public class InMemoryJsGridDataStorage : IJsGridStorage

    {
        static List<IDictionary<string, object>> db1 = new List<IDictionary<string, object>>();

        readonly Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> defaultFiltering = (x, y) => x;

        List<IDictionary<string, object>> db
        {
            set => db1 = value;
            get => db1;
        }

        public JsGridStorageStatistics LoadAll(int take = 100, dynamic sampleForFilter = null, Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> clientSideFiltering = null, int skip = 0)
        {
            clientSideFiltering = clientSideFiltering ?? this.defaultFiltering;
            var d = (IEnumerable<dynamic>)clientSideFiltering(this.db, sampleForFilter);
            return new JsGridStorageStatistics
            {
                Results = d.Skip(skip).Take(take),
                Total = this.db.Count
            };
        }

        public dynamic LoadById(string id)
        {
            return this.db.FirstOrDefault(x => x["Id"].ToString() == id.ToString());
        }

        public void Update(IDictionary<string, object> client)
        {
            bool updated = false;
            for (int i = 0; i < this.db.Count; i++)
                if (this.db[i]["Id"].ToString() == client["Id"].ToString())
                {
                    this.db[i] = client;
                    updated = true;
                    break;
                }

            if (!updated)
                throw new Exception($"Unable to Update : Id of {client["Id"]} not found");
        }

        public void Save(IDictionary<string, object> client)
        {
            client["Id"] = Guid.NewGuid().ToString().Replace("-", "");
            this.db.Add(client);
        }

        public void Delete(string id)
        {
            bool updated = false;
            for (int i = 0; i < this.db.Count; i++)
                if (this.db[i]["Id"].ToString() == id)
                {
                    this.db.RemoveAt(i);
                    updated = true;
                    break;
                }

            if (!updated)
                throw new Exception($"Unable to delete : Id of {id} not found");
        }
    }
}