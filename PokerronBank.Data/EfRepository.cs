using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PokerronBank.Model;
using PokerronBank.Model.Contracts;

namespace PokerronBank.Data
{
    public class EfRepository : IRepository
    {


        public AppDbContext Context { get; set; }

        public EfRepository(string dataBaseFileName)
        {
            Context = new AppDbContext(dataBaseFileName);


            var version = "0.06";
            var versionDb = "";

            Context.Database.EnsureCreated();
            if (Context.DataBaseVersion?.Count() > 0)
            {
                versionDb = Context.DataBaseVersion?.ToList()[0].Version;
            }

            if (versionDb != version)
            {

                Context.Database.EnsureDeleted();
                Context.Database.EnsureCreated();
                Context.DataBaseVersion?.Add(new DataBaseVersion(version));
                Context.SaveChanges();
            }
            

        }


        public IEnumerable<T> GetAll<T>() where T : Entity
        {
            return Context.Set<T>().ToList();
        }

        public T GetById<T>(Guid id) where T : Entity
        {
            return Context.Set<T>().Find(id);
        }

        public void Add<T>(T entity) where T : Entity
        {
            Context.Set<T>().Add(entity);
        }

        public void Delete<T>(T entity) where T : Entity
        {
            Context.Set<T>().Remove(entity);
        }


        //solo se utiliza para web / Servicis
        //no utilizado para desktop (wpf/winforms)
        public void Update<T>(T entity) where T : Entity
        {
            var loaded = GetById<T>(entity.Id);
            if (loaded != null)
                Context.Entry(loaded).CurrentValues.SetValues(entity);
        }

        public void SaveAll()
        {
            Context.SaveChanges();
        }
    }
}