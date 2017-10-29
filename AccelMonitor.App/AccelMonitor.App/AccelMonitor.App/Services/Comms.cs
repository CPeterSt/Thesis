using AccelMonitor.App.Models;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;

namespace AccelMonitor.App
{

    public class Comms
    {
        TcpSocketClient client = null;
        string address = "192.168.4.1";
        int port = 8884;

        public Comms()
        {
            var minutes = TimeSpan.FromSeconds(10);
        }

        public async void TCPDisconnected()
        {
            try
            {
                if (client != null)
                {
                    await client.DisconnectAsync();
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.DevPage.normalSLayout.IsVisible = false;
                    App.DevPage.connectSLayout.IsVisible = true;
                });
                App.streamSendNext = true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async void TCPConnected()
        {
            await Task.Run(() =>
            {
                HandleTCPIPSend(new Item() { Text = "Device:Id,?" });
            });
            App.DevPage.devPageActIndicator(false);
            App.DevPage.normalSLayout.IsVisible = true;
            App.DevPage.connectSLayout.IsVisible = false;
        }

        public async void CreateTCPClient()
        {
            try
            {
                App.DevPage.devPageActIndicator(true);
                client = new TcpSocketClient();
                await client.ConnectAsync(address, port);
                TCPConnected();

                string finalData = "";

                await Task.Run(() =>
                {
                    try
                    {
                        byte[] byteData = new byte[1];
                        while (client.ReadStream.Read(byteData, 0, 1) != 0)
                        {
                            if (byteData[0] == 0x02)
                            {
                                finalData = "";
                            }
                            else if (byteData[0] == 0x04)
                            {
                                HandelReceivedData(finalData);
                                finalData = "";
                            }
                            else
                            {
                                finalData += Convert.ToChar(byteData[0]);
                            }
                        }
                        TCPDisconnected();
                    }
                    catch (Exception)
                    {
                        TCPDisconnected();
                    }
                });
                await client.DisconnectAsync();
            }
            catch (Exception e)
            {
                client = null;
                Debug.WriteLine(e.ToString());
                TCPDisconnected();
            }
        }

        async void HandelReceivedData(string TCPDataStr)
        {
            try
            {
                switch (TCPDataStr.Split(':')[0])
                {
                    #region Device
                    case "Device":/**************************************************Device**********************************************************/
                        switch (TCPDataStr.Split(':')[1].Split(',')[0])
                        {
                            case "Id":
                                if (TCPDataStr.Split(',')[1] == "Error")// if there is sa error
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        App.DevPage.displayAlert("Device ID Error");
                                    });
                                    App.DevPage.items[0].Text = "Id:0"; // set it = to 0
                                }
                                else
                                {
                                    App.DevPage.items[0].Text = "Id:" + TCPDataStr.Split(',')[1];
                                }
                                if (App.DevPage.items[0].Text != "Id:0")// if the id is not 0 dont allow them to change it.
                                {
                                    App.DevPage.items[0].DataVisable = false;
                                }
                                HandleTCPIPSend(new Item() { Text = "Device:Version,?" });
                                break;

                            case "Version":
                                App.DevPage.items[1].Text = "Version: " + TCPDataStr.Split(',')[1];
                                HandleTCPIPSend(new Item() { Text = "Device:SampleCount,?" });
                                break;

                            case "SampleCount":
                                App.sampleCount = Convert.ToInt16(TCPDataStr.Split(',')[1]);
                                HandleTCPIPSend(new Item() { Text = "Device:SamplePeriod,?" });
                                App.DevPage.items[4].Data = App.sampleCount.ToString();
                                break;
                            case "SamplePeriod":
                                App.samplePeriod = Convert.ToInt16(TCPDataStr.Split(',')[1]);
                                App.DevPage.items[3].Data = App.samplePeriod.ToString();
                                break;
                            case "Calibrate":
                                if (TCPDataStr.Split(',')[1] == "Finished")
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        App.DevPage.displayAlert("Device Calibrated");
                                    });
                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        App.DevPage.displayAlert("Device Calibration Failed");
                                    });
                                }
                                break;
                        }
                        break;
                    #endregion
                    #region Data
                    case "Data":/****************************************************Data************************************************************/
                        switch (TCPDataStr.Split(':')[1].Split(',')[0])
                        {
                            case "Send":
                                switch (TCPDataStr.Split(':')[1].Split(',')[1].Split(':')[0])
                                {
                                    #region Size
                                    case "Size":
                                        int lengthTher = Convert.ToInt16(TCPDataStr.Split(':')[2].Split(',')[0]), lengthPrac = TCPDataStr.Split(',')[2].Length;
                                        if (lengthTher == lengthPrac)
                                        {
                                            App.streamSendNext = true;
                                            TCPIPSend("Data:Send,ACK," + TCPDataStr.Split(',')[3]);
                                            string[] dataArray = TCPDataStr.Split(',')[2].Split('\n');
                                            UInt64 currentTime = 0;

                                            foreach (var item in dataArray)
                                            {
                                                if (item.Length > 15)
                                                {
                                                    if (item.Length == 16)
                                                    {
                                                        currentTime = (Convert.ToUInt64(item, 16) / 1000) - (UInt64)App.samplePeriod;
                                                    }
                                                    else
                                                    {
                                                        await Task.Run(() =>
                                                        {
                                                            currentTime += (UInt64)App.samplePeriod;
                                                            DataTable tvd = new DataTable();

                                                            tvd.dateTime = new DateTime(1970, 1, 1).AddMilliseconds(currentTime);

                                                            tvd.dataX = Convert.ToDouble(item.Split(' ')[0]);
                                                            tvd.dataY = Convert.ToDouble(item.Split(' ')[1]);
                                                            tvd.dataZ = Convert.ToDouble(item.Split(' ')[2]);

                                                            try
                                                            {
                                                                App.Database.SaveTimeVsDataItemAsync(tvd);

                                                            }
                                                            catch (SQLiteException e)
                                                            {
                                                                Debug.WriteLine(e.ToString());
                                                                throw;
                                                            }
                                                        });
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            TCPIPSend("Data:Send,NAK," + TCPDataStr.Split(',')[3]);
                                        }
                                        break;
                                    #endregion
                                    #region Start
                                    case "Start":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            App.DevPage.displayAlert("Sending Data Started");
                                        });
                                        break;
                                    #endregion
                                    #region End
                                    case "End":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            App.DevPage.displayAlert("Sending Data Finished");
                                        });
                                        break;
                                        #endregion
                                }
                                break;
                        }
                        break;
                    #endregion
                    #region Data
                    case "Status":/****************************************************Data************************************************************/
                        switch (TCPDataStr.Split(':')[1].Split(',')[0])
                        {
                            case "Time":
                                DateTime devDateTime = new DateTime(1970, 1, 1).AddMilliseconds(Convert.ToInt64(TCPDataStr.Split(',')[1]) / 1000);
                                App.DevPage.items[2].Data = devDateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                                break;
                        }
                        break;
                        #endregion
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                TCPDisconnected();
                throw;
            }
        }

        public void HandleTCPIPSend(Item item)
        {
            try
            {
                switch (item.Text)
                {
                    case "Set Time":
                        string timeStr = ((DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString();
                        TCPIPSend("Status:Time,Set:" + timeStr + 600);
                        break;
                    case "Sample & Save Block":
                        TCPIPSend("Data:SampleSave,Count:" + item.Data);
                        break;
                    case "Sample & Stream Block":
                        App.streamSendNext = false;
                        TCPIPSend("Data:SampleStream,Count:" + item.Data);
                        break;
                    case "Device:SampleCount,?":
                        TCPIPSend("Device:SampleCount,?");
                        break;
                    case "Device:SamplePeriod,?":
                        TCPIPSend("Device:SamplePeriod,?");
                        break;
                    case "Calibrate":
                        TCPIPSend("Device:Calibrate,Start");
                        break;
                    case "Reset":
                        TCPIPSend("Device:Reset");
                        break;
                    case "Clear Onboard":
                        TCPIPSend("Data:Clear,OnBoard");
                        break;
                    case "Clear SD Card":
                        TCPIPSend("Data:Clear,SDCard");
                        break;
                    case "Send All Data":
                        TCPIPSend("Data:Send,All");
                        break;
                    case "Sample Period":
                        if (Convert.ToInt16(item.Data) < 10)
                        {
                            App.DevPage.displayAlert("Sample period cannot be less than 20ms");
                        }
                        else
                        {
                            TCPIPSend("Device:SamplePeriod,Val:" + item.Data);
                        }
                        break;
                    case "Sample Count":
                        if (Convert.ToInt16(item.Data) <= 19)
                        {
                            App.DevPage.displayAlert("Sample Count cannot be less than 10");
                        }
                        else
                        {
                            TCPIPSend("Device:SampleCount,Val:" + item.Data);
                        }
                        break;
                    case "Id:0":
                        TCPIPSend("Device:Id,Val:" + item.Data);
                        break;
                    case "Device:Id,?":
                        TCPIPSend("Device:Id,?");
                        break;
                    case "Device:Version,?":
                        TCPIPSend("Device:Version,?");
                        break;
                }
            }
            catch (Exception e)
            {
                TCPDisconnected();
                throw;
            }
        }

        public async void TCPIPSend(string str)
        {
            try
            {
                byte[] myByteArr = new byte[255];
                myByteArr[0] = Convert.ToByte(0x02);
                byte[] tmpByteArr1, endByteArr2 = { Convert.ToByte(0x04), Convert.ToByte(0x00) }, b3;
                MemoryStream s;

                tmpByteArr1 = Encoding.UTF8.GetBytes(str);

                s = new MemoryStream();
                s.Write(myByteArr, 0, sizeof(byte));
                s.Write(tmpByteArr1, 0, tmpByteArr1.Length);
                s.Write(endByteArr2, 0, sizeof(byte) * 2);

                b3 = s.ToArray();
                if (client != null)
                {
                    // write to the 'WriteStream' property of the socket client to send data
                    await client.WriteStream.WriteAsync(b3, 0, b3.Length);
                    await client.WriteStream.FlushAsync();
                }
            }
            catch (Exception)
            {
                TCPDisconnected();
            }
        }

    }

}
