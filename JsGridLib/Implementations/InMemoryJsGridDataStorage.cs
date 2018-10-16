namespace JsGridLib.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JsGridLib.Contracts;
    using JsGridLib.Models;

    public class InMemoryJsGridDataStorage<T> : IJsGridStorage<T>
        where T : IJsGridEntity
    {
        static List<T> db1 = new List<T>();

        List<T> db
        {
            set => db1 = value;
            get => db1;
        }

        public JsGridStorageStatistics<T> LoadAll(T sampleForFilter, Func<IEnumerable<T>, T, IEnumerable<T>> clientSideFiltering, int take, int skip)
        {
            return new JsGridStorageStatistics<T>
            {
                Results = clientSideFiltering(this.db, sampleForFilter).Skip(skip).Take(take),
                Total = this.db.Count
            };
        }

        public T LoadById(string id)
        {
            return this.db.FirstOrDefault(x => x.Id == id);
        }

        public void Update(string id, T client)
        {
            for (int i = 0; i < this.db.Count; i++)
                if (this.db[i].Id == id)
                    this.db[i] = client;
        }

        public void Save(T client)
        {
            client.Id = Guid.NewGuid().ToString();
            this.db.Add(client);
        }

        public void Delete(string id, T client)
        {
            for (int i = 0; i < this.db.Count; i++)
                if (this.db[i].Id == id)
                    this.db.RemoveAt(i);
        }
    }
}