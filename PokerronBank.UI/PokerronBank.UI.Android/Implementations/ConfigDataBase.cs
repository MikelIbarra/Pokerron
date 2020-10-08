using PokerronBank.Model.Contracts;
using Path = System.IO.Path;
using PokerronBank.UI.Droid.Implementations;
using Environment = System.Environment;

[assembly: Xamarin.Forms.Dependency(typeof(ConfigDataBase))]

namespace PokerronBank.UI.Droid.Implementations
{
    
    public class ConfigDataBase : IConfigDataBase
    {
        public string GetFullPath(string dataBaseFileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dataBaseFileName);
        }
    }
}