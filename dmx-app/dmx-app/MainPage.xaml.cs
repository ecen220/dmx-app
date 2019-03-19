using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dmxapp;
using Xamarin.Forms;

namespace dmx_app
{
    public partial class MainPage : ContentPage
    {
        public List<Channel> Channels = new List<Channel>();

        uDMX dmx;

        public MainPage()
        {
            dmx = new uDMX();

            for (int i = 1; i <= 512; i++)
            {
                Channels.Add(new Channel()
                {
                    Dmx = dmx,
                    Index = i,
                    Value = 0
                });
            }


            InitializeComponent();

            channels.ItemsSource = Channels;
        }
    }
}
