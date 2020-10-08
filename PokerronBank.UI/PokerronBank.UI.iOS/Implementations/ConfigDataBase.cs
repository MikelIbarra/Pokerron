using System;
using System.IO;
using PokerronBank.Model.Contracts;

namespace PokerronBank.UI.iOS.Implementations
{
    public class ConfigDataBase : IConfigDataBase
    {
        public string GetFullPath(string dataBaseFileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", dataBaseFileName);
        }
    }
}