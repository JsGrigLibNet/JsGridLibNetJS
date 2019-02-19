namespace JsGridLib.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.OData.Query;
    using JsGridLib.Contracts;
    using JsGridLib.Models;

    public class InMemoryJsGridDataStorage : IJsGridStorage

    {
        static Dictionary<string, List<IDictionary<string, object>>> db1 = new Dictionary<string, List<IDictionary<string, object>>>();

        readonly Func<IEnumerable<dynamic>, dynamic, IEnumerable<dynamic>> defaultFiltering = (x, y) => x;

        Dictionary<string, List<IDictionary<string, object>>> db
        {
            set => db1 = value;
            get => db1;
        }

        public JsGridStorageStatistics LoadAllTop(string storageAccess, int take = 100, dynamic sampleForFilter = null, ODataQueryOptions opt = null, int skip = 0)
        {
            this.TryInitializeStorageAccess(storageAccess);

            //opt = clientSideFiltering ?? this.defaultFiltering;
            //var d = (IEnumerable<dynamic>)clientSideFiltering(this.db[storageAccess], sampleForFilter);
            var d = (IEnumerable<dynamic>)this.db[storageAccess];
            return new JsGridStorageStatistics
            {
                Results = d.Skip(skip).Take(take),
                Total = this.db.Count
            };
        }

        void TryInitializeStorageAccess(string storageAccess)
        {
            if (!this.db.ContainsKey(storageAccess))
            {
                this.db.Add(storageAccess, new List<IDictionary<string, object>>());
            }
        }

        public JsGridStorageStatistics LoadAll(string storageAccess, dynamic sampleForFilter = null, ODataQueryOptions clientSideFiltering = null)
        {
            this.TryInitializeStorageAccess(storageAccess);
            //clientSideFiltering = clientSideFiltering ?? this.defaultFiltering;
            //var d = (IEnumerable<dynamic>)clientSideFiltering(this.db[storageAccess], sampleForFilter);
            var d = (IEnumerable<dynamic>)this.db[storageAccess];
            return new JsGridStorageStatistics
            {
                Results = d.Take(1000000),
                Total = this.db.Count
            };
        }

        public dynamic LoadById(string storageAccess, string id)
        {
            this.TryInitializeStorageAccess(storageAccess);
            return this.db[storageAccess].FirstOrDefault(x => x["Id"].ToString() == id.ToString());
        }

        public void Update(string storageAccess, IDictionary<string, object> client)
        {
            this.TryInitializeStorageAccess(storageAccess);
            bool updated = false;
            for (int i = 0; i < this.db.Count; i++)
                if (this.db[storageAccess][i]["Id"].ToString() == client["Id"].ToString())
                {
                    this.db[storageAccess][i] = client;
                    updated = true;
                    break;
                }

            if (!updated)
                throw new Exception($"Unable to Update : Id of {client["Id"]} not found");
        }

        public void Save(string storageAccess, IDictionary<string, object> client)
        {
            this.TryInitializeStorageAccess(storageAccess);
            client["Id"] = Guid.NewGuid().ToString().Replace("-", "");
            this.db[storageAccess].Add(client);
        }

        public void Delete(string storageAccess, string id)
        {
            this.TryInitializeStorageAccess(storageAccess);
            bool updated = false;
            for (int i = 0; i < this.db[storageAccess].Count; i++)
                if (this.db[storageAccess][i]["Id"].ToString() == id)
                {
                    this.db[storageAccess].RemoveAt(i);
                    updated = true;
                    break;
                }

            if (!updated)
                throw new Exception($"Unable to delete : Id of {id} not found");
        }

        public object GetDataSampleForSchemaByDataAccess(string dataaccess)
        {
            this.TryInitializeStorageAccess(dataaccess);
            dynamic sample = this.db[dataaccess].First();
            return sample;
        }
    }
}