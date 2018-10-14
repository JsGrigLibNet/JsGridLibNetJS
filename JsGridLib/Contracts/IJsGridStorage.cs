namespace SampleHttpsServer
{
    using System;
    using System.Collections.Generic;

    public interface IJsGridStorage<T>
        where T : IJsGridEntity
    {
        JsGridStorageStatistics<T> LoadAll(
            T sampleForFilter, 
            Func<IEnumerable<T>, T, IEnumerable<T>> clientSideFiltering, 
            int take, 
            int skip);




        T LoadById(string id);

        void Update(string id, T client);

        void Save(T client);

        void Delete(string id, T client);
    }
}