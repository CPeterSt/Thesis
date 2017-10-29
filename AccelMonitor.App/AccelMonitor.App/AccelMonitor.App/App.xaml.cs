/*
 * 2017-09-26   v1.0.0      begin of proper development and versioning
 *              v1.1.0      Addition of TCP/IP sockets
 * 2017-09-30   v1.1.1	    Added part of the list of protocol commands.     
 * 2017-10-01   v1.1.2      Added device ID        
 * 2017-10-03   v1.2.0      Add SQL to project
 * 2017-10-06   v1.2.1      Added first table
 * 2017-10-07   v1.2.2      Add data pulled to sqllite
 * 2017-10-15   v1.3.0      Add intergration for velocity and displacemnt aslo added filtering(moving average)
 * 2017-10-21   v1.4.0      Add layout options to enable filtering and set filter samples. Enable velocity and displacement graphs.
 * 
 */

using AccelMonitor.App.Models;
using AccelMonitor.App.Services;
using AccelMonitor.App.Views;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AccelMonitor.App
{
    public partial class App : Application
    {
        public static DevicePage DevPage;
        public static Comms commsClass;
        public static List<string> dataList = new List<string>();
        public static int samplePeriod = 20, sampleCount = -1;
        public static ObservableCollection<DataTable> timeVsDataListX = new ObservableCollection<DataTable>();
        public static ObservableCollection<DataTable> timeVsDataListY = new ObservableCollection<DataTable>();
        public static ObservableCollection<DataTable> timeVsDataListZ = new ObservableCollection<DataTable>();
        public static bool streamSendNext = true;

        static DbManager database;


        public static string version = "v1.2.0";
        public App()
        {
            InitializeComponent();
            DevPage = new DevicePage();

            SetMainPage();
            commsClass = new Comms();
        }

        

        public static void SetMainPage()
        {
            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(DevPage)
                    {
                        Title = "Device",
                    },
                    new NavigationPage(new Process())
                    {
                        Title = "Processing",
                    },
                }
            };
        }

        public static DbManager Database
        {
            get
            {
                if (database == null)
                {
                    database = new DbManager(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return database;
            }
        }
    }
}
