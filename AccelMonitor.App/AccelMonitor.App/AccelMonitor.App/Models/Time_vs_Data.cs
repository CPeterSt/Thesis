using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccelMonitor.App.Models
{
    public class Time_vs_Data : INotifyPropertyChanged
    {
        public double data { get; set; }

        public DateTime dateTime { get; set; }

        public Time_vs_Data()
        {
        }

        public Time_vs_Data(double value, double size)
        {
            Value = value;
            Size = size;
        }

        private double value;

        public double Value
        {
            get { return value; }
            set
            {
                this.value = value;
                RaisePropertyChanged("Value");
            }
        }

        private double size;

        public double Size
        {
            get { return size; }
            set
            {
                size = value;
                RaisePropertyChanged("Size");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


    }
}
