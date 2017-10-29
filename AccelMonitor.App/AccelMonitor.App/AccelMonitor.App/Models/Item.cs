using System.ComponentModel;

namespace AccelMonitor.App.Models
{
    public class Item : INotifyPropertyChanged
    {
        string text = string.Empty;
        string data = string.Empty;
        bool dataVisable = false;

        public string Text
        {
            get { return text; }
            set
            {
                this.text = value;
                RaisePropertyChanged("Text");
            }
        }

        public string Data
        {
            get { return data; }
            set
            {
                this.data = value;
                RaisePropertyChanged("Data");
            }
        }

        public bool DataVisable
        {
            get { return dataVisable; }
            set
            {
                this.dataVisable = value;
                RaisePropertyChanged("DataVisable");
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
