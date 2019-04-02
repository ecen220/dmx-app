
using System;
using System.ComponentModel;

namespace dmxapp
{
    public class Channel : INotifyPropertyChanged
    {
        public DMX Dmx {get; set; }
        public int Index { get; set; }
        private int channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Value
        {
            get { return channel; }
            set {
                channel = value;
                Dmx?.SetSingleChannel((short)(Index-1), (byte)value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }


        public Channel()
        {
        }
    }
}
