using System;
using VeciHelpAPK.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK
{
    public partial class App : Application
    {
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
                
                 if (MainPage is NavigationPage mainPage)
                {
                        promptToConfirmExit = mainPage.Navigation.NavigationStack.Count <= 2;
                }
              
                return promptToConfirmExit;
            }
        }

    }
}
