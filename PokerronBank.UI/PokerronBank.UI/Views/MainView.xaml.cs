using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using PokerronBank.UI.ViewModels.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokerronBank.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();
            TabHost.SelectedIndex = 0;
           
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();



            if (Device.RuntimePlatform == Device.Android)
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);
                    if (results[Permission.Contacts] == PermissionStatus.Granted)
                    {
                        loadContacts();
                    }
                }
                else
                {
                    loadContacts();
                }
            }
            else
            {
                loadContacts();
            }
        }
        async void loadContacts()
        {
            ViewModelViewManager.MainViewModel.Contacts.Clear();
            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            foreach (var contact in contacts)
            {
                foreach (var contactNumber in contact.Numbers)
                {
                    ViewModelViewManager.MainViewModel.Contacts.Add(new ContactViewItem(contact, contactNumber));
                }
            }


            
        }

    

    }
}