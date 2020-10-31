using VeciHelpAPK.Models;
using VeciHelpAPK.Views;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace VeciHelpAPK
{
    public partial class App : Application
    {
        public static bool _variableGlobal { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new VeciHelpAPK.Views.LoginView());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public bool PromptToConfirmExit
        {
            get
            {
                bool promptToConfirmExit = false;

                if (MainPage is NavigationPage mainPage)
                {
                    promptToConfirmExit = MainPage.Navigation.NavigationStack.Count <= 2;

                   // promptToConfirmExit = MenuPage.Navigation.NavigationStack.Count <= 2;
                    //promptToConfirmExit = ((MasterDetailPage)Current.MainPage).Detail.Navigation.NavigationStack.Count <= 2;
                }
                return promptToConfirmExit;
            }
        }

    }
}
