using System;
using System.Collections.Generic;

namespace PokerronBank.Model.Contracts
{
    public interface IRepository
    {
        IEnumerable<T> GetAll<T>() where T : Entity;
        T GetById<T>(Guid id) where T : Entity;
        void Add<T>(T entity) where T : Entity;
        void Delete<T>(T entity) where T : Entity;
        void Update<T>(T entity) where T : Entity;
        void SaveAll();
    }
}
