namespace SampleHttpsServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    public class InMemoryJsGridDataStorage<T>: IJsGridStorage<T>
         where T: IJsGridEntity
    {

        static List<T> db1= new List<T>();
        private List<T> db
        {
            set => db1 = value;
            get => db1;
        }

        public JsGridStorageStatistics<T> LoadAll(T sampleForFilter, Func<IEnumerable<T>, T, IEnumerable<T>> clientSideFiltering, int take, int skip)
        {
          return  new JsGridStorageStatistics<T>()
          {
              Results = clientSideFiltering(db, sampleForFilter).Skip(skip).Take(take),
              Total = db.Count
          };
        }

        public T LoadById(string id)
        {
            return db.FirstOrDefault(x=>x.Id==id);
        }
        public void Update(string id, T client)
        {
            for (var i = 0; i < db.Count; i++)
            {
                if (db[i].Id == id)
                {
                    db[i] = client;
                }
            }
        }
        public void Save(T client)
        {
            client.Id = Guid.NewGuid().ToString();
            db.Add(client);
        }


        public void Delete(string id, T client)
        {
            for (var i = 0; i < db.Count; i++)
            {
                if (db[i].Id == id)
                {
                    db.RemoveAt(i);
                }
            }
        }
    }
}