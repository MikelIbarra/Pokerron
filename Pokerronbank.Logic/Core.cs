using System;
using PokerronBank.Data;
using Pokerronbank.Logic.Services;
using PokerronBank.Model.Contracts;

namespace Pokerronbank.Logic
{

    public class Core
    {

        public IRepository Repository { get; private set; }


        public Services.Services Services { get; set; }

        public Core(IRepository repo)
        {

           
            Repository = repo;
            Services = new Services.Services(this);
        }

        
        public Core(string dataBaseFileName) : this(new EfRepository(dataBaseFileName))
        { }
       

    }
}
