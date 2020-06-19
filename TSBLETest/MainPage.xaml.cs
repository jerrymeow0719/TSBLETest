using System.ComponentModel;
using TSBLETest.ViewModels;
using Xamarin.Forms;

namespace TSBLETest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        readonly MainViewModel vm = new MainViewModel();
        public MainPage()
        {
            InitializeComponent();
 
            this.BindingContext = vm;

            btn_start.Clicked += (s, e) =>
            {
                vm.StartScan();
            };

            btn_stop.Clicked += (s, e) =>
            {
                vm.StopScan();
            };

            btn_clear.Clicked += (s, e) =>
            {
                vm.ClearCLick();
            };
        }
    }
}
