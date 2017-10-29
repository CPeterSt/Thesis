using AccelMonitor.App.Models;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccelMonitor.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Process : ContentPage
    {
        FastLineSeries seriesX = new FastLineSeries();
        FastLineSeries seriesY = new FastLineSeries();
        FastLineSeries seriesZ = new FastLineSeries();
        bool pollData = false, streamEnabled = false;

        public Process()
        {
            InitializeComponent();

            List<DataTable> firstGraghData = new List<DataTable>();

            StreamData();

            for (int i = 1; i < 101; i++)
            {
                FilterPicker.Items.Add(i.ToString());
            }

            for (double i = 0; i < 100; i++)
            {
                TollerancePicker.Items.Add((i / 10).ToString());
            }

            for (double i = -50; i < 100; i++)
            {
                ShiftXPicker.Items.Add((i / 10).ToString());
            }

            GraphTypePicker.Items.Add("Acceleration");
            GraphTypePicker.Items.Add("Velocity");
            GraphTypePicker.Items.Add("Displacement");
            GraphTypePicker.Items.Add("FFT");
            StartDatePicker.Date = DateTime.Now.Date;
            EndDatePicker.Date = DateTime.Now.Date;
            StartTimePicker.Time = DateTime.Now.TimeOfDay;
            EndTimePicker.Time = DateTime.Now.TimeOfDay;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pollData = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (pollData == false)
            {
                streamEnabled = true;
                pollData = true;
            }
            else
            {
                streamEnabled = false;
                pollData = false;
            }

        }

        async void DisplayGraphData()
        {
            IsBusy = true;

            List<DataTable> tvdList = new List<DataTable>();
            List<DataTable> tvdList2 = new List<DataTable>();
            DateTime now = DateTime.Now;
            DateTime startDT, endDT;
            startDT = StartDatePicker.Date;
            startDT = startDT.Add(StartTimePicker.Time);
            endDT = EndDatePicker.Date;
            endDT = endDT.Add(EndTimePicker.Time);
            tvdList = await App.Database.GetTimeVsDataByTimeAsync(startDT, endDT);
            foreach (var item in tvdList)
            {
                item.dataX = item.dataX * -1;
                if (TollerancePicker.SelectedIndex >= 0)
                {
                    if (item.dataX > (((double)TollerancePicker.SelectedIndex) / -10) && item.dataX < (((double)TollerancePicker.SelectedIndex) / 10))
                    {
                        item.dataX = 0;
                    }
                    if (item.dataY > (((double)TollerancePicker.SelectedIndex) / -10) && item.dataY < (((double)TollerancePicker.SelectedIndex) / 10))
                    {
                        item.dataY = 0;
                    }
                    if (item.dataZ > (((double)TollerancePicker.SelectedIndex) / -10) && item.dataZ < (((double)TollerancePicker.SelectedIndex) / 10))
                    {
                        item.dataZ = 0;
                    }
                }
                if (ShiftXPicker.SelectedIndex >= 0)
                {
                    item.dataX = item.dataX - 5 + ((double)ShiftXPicker.SelectedIndex)/10;
                }
            }
            tvdList = FilterData(tvdList);
            ChartX.SecondaryAxis = new NumericalAxis() { Title = new ChartAxisTitle() { Text = "m/s^2" } };
            if (GraphTypePicker.SelectedIndex == 1 || GraphTypePicker.SelectedIndex == 2)
            {
                tvdList = RiemannSumDataTable(tvdList, App.samplePeriod);
                ChartX.SecondaryAxis = new NumericalAxis() { Title = new ChartAxisTitle() { Text = "m/s" } };
            }
            if (GraphTypePicker.SelectedIndex == 2)
            {
                tvdList = RiemannSumDataTable(tvdList, App.samplePeriod);
                ChartX.SecondaryAxis = new NumericalAxis() { Title = new ChartAxisTitle() { Text = "m" } };
            }
            App.timeVsDataListX.Clear();

            seriesX.ItemsSource = tvdList;
            seriesY.ItemsSource = tvdList;
            seriesZ.ItemsSource = tvdList;

            seriesX.YBindingPath = "dataX";
            seriesY.YBindingPath = "dataY";
            seriesZ.YBindingPath = "dataZ";


            seriesX.XBindingPath = "dateTime";
            seriesY.XBindingPath = "dateTime";
            seriesZ.XBindingPath = "dateTime";


            ChartX.Series.Clear();
            ChartX.Series.Add(seriesX);

            ChartY.Series.Clear();
            ChartY.Series.Add(seriesY);

            ChartZ.Series.Clear();
            ChartZ.Series.Add(seriesZ);

            //Initializing column series
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayGraphData();
        }

        public List<DataTable> FilterData(List<DataTable> dataTableList)
        {
            List<DataTable> result = new List<DataTable>();
            int filterNum = FilterPicker.SelectedIndex + 1;
            for (int i = 0; i < dataTableList.Count - 1; i++)
            {
                if (i < filterNum)
                {
                    result.Add(new DataTable()
                    {
                        dataX = 0,
                        dataY = 0,
                        dataZ = 0,
                        dateTime = dataTableList[i].dateTime
                    });
                }
                else
                {
                    double dx = 0, dy = 0, dz = 0;
                    for (int k = 0; k < filterNum; k++)
                    {
                        dx += dataTableList[i - k].dataX;
                        dy += dataTableList[i - k].dataY;
                        dz += dataTableList[i - k].dataZ;
                    }
                    dx = dx / filterNum;
                    dy = dy / filterNum;
                    dz = dz / filterNum;

                    result.Add(new DataTable()
                    {
                        dataX = dx,
                        dataY = dy,
                        dataZ = dz,
                        dateTime = dataTableList[i].dateTime
                    });
                }

            }
            return result;
        }

        public List<DataTable> RiemannSumDataTable(List<DataTable> dataTableList, int periodms)
        {
            List<DataTable> result = new List<DataTable>();
            for (int i = 0; i < dataTableList.Count; i++)
            {
                if (i == 0)
                {
                    result.Add(new DataTable() { dataX = 0, dataY = 0, dataZ = 0, dateTime = dataTableList[0].dateTime });
                }
                else
                {
                    result.Add(new DataTable()
                    {

                        dataX = (result[i - 1].dataX + (dataTableList[i - 1].dataX * (periodms * 0.001))),
                        dataY = (result[i - 1].dataY + (dataTableList[i - 1].dataY * (periodms * 0.001))),
                        dataZ = (result[i - 1].dataZ + (dataTableList[i - 1].dataZ * (periodms * 0.001))),
                        dateTime = dataTableList[i].dateTime

                    });
                }
            }
            return result;
        }

        private void FilterPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayGraphData();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            App.Database.DeleteTimeVsData();
        }

        async void StreamData()
        {
            int blockCount = 1;
            await Task.Run(() =>
            {
                while (true)
                {
                    if (App.samplePeriod > 0)
                    {
                        int time = (blockCount) * App.samplePeriod * App.sampleCount + 1;

                        Task.Delay(1);
                        if (App.streamSendNext && streamEnabled == true)
                        {
                            App.commsClass.HandleTCPIPSend(new Item { Text = "Sample & Stream Block", DataVisable = true, Data = blockCount.ToString() });
                        }
                    }
                }
            });
        }
    }

}