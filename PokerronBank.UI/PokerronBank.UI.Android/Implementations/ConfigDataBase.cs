using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PokerronBank.Model.Contracts;
using Path = System.IO.Path;
using System.Runtime.CompilerServices;
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