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

            MainPage = new NavigationPage(new LoginView());

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

                var masterDetail = App.Current.MainPage as MasterDetailPage;

                if (MainPage is MasterDetailPage mainPage)
                {
                    promptToConfirmExit = masterDetail.Detail.Navigation.NavigationStack.Count == 1;
                }
                return promptToConfirmExit;
            }
        }

    }
}
