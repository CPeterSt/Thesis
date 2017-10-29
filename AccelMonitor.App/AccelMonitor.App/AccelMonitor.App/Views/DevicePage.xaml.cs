using AccelMonitor.App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccelMonitor.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicePage : ContentPage
    {
        public StackLayout normalSLayout, connectSLayout, loadingSLayout;
        public ObservableCollection<Item> items = new ObservableCollection<Item>();

        public DevicePage()
        {
            InitializeComponent();
            items.Add(new Item { Text = "Id", DataVisable = true, Data = "00000001" });//fixed position
            items.Add(new Item { Text = "Version" });//fixed position
            items.Add(new Item { Text = "Set Time", DataVisable = true });//fixed position
            items.Add(new Item { Text = "Sample Period", DataVisable = true });//fixed position
            items.Add(new Item { Text = "Sample Count", DataVisable = true});//fixed position
            items.Add(new Item { Text = "Calibrate" });
            items.Add(new Item { Text = "Reset" });
            items.Add(new Item { Text = "Clear Onboard" });
            items.Add(new Item { Text = "Clear SD Card" });
            items.Add(new Item { Text = "Send All Data" });
            items.Add(new Item { Text = "Sample & Save Block", DataVisable = true, Data = "1"});
            items.Add(new Item { Text = "Sample & Stream Block", DataVisable = true, Data = "1" });

            DeviceControlList.ItemsSource = items;
            normalSLayout = NormalSLayout;
            connectSLayout = ConnectSLayout;
            loadingSLayout = LoadingSLayout;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.commsClass.CreateTCPClient();
        }

        private void ItemsListView_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {

            ListView listView = sender as ListView;
            Item selectedItem = listView == null ? null : listView.SelectedItem as Item;
            App.commsClass.HandleTCPIPSend(selectedItem);
            Debug.WriteLine(selectedItem.Text);
        }

        public async void displayAlert(string msg)
        {
            await DisplayAlert("Alert", msg, "OK");

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.commsClass.TCPDisconnected();
        }

        public void devPageActIndicator(bool running)
        {
            actIndicator.IsRunning = running;
        }

    }
}