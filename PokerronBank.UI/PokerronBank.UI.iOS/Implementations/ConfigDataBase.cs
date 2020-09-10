using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using PokerronBank.Model.Contracts;
using UIKit;
using Xamarin.Forms.PlatformConfiguration;

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