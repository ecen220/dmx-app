using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace dmxapp
{
    public partial class ChannelPage : ContentPage
    {
        private List<Channel> Channels = new List<Channel>();
        private DMX DMX;

        public ChannelPage(DMX DMX, List<Channel> Channels)
        {
            this.DMX = DMX;
            this.Channels = Channels;
            InitializeComponent();
            channels.ItemsSource = Channels;

        }

    }
}
