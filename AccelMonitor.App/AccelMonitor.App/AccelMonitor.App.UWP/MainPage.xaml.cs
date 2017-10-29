namespace AccelMonitor.App.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            new Syncfusion.SfChart.XForms.UWP.SfChartRenderer();
            this.InitializeComponent();
            LoadApplication(new AccelMonitor.App.App());
        }
    }
}