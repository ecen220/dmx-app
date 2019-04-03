using System;
using System.Collections.Generic;
using dmxapp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace dmx_app
{
    public partial class App : Application
    {
        DMX dmx;
        public List<Channel> Channels = new List<Channel>();
        Page CurrentPage = Page.Connect;

        enum Page : int
        {
            Connect = 1,
            Channel = 2,
        }

        public App()
        {
            InitializeComponent();






            MainPage = new ConnectPage();

            //if (DMX.ScanForDMX())
            //{
            //    CurrentPage = Page.Channel;
            //    MainPage = new ChannelPage(dmx, Channels);
            //}

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                bool isConnected = DMX.ScanForDMX();

                if (isConnected)
                {
                    if (CurrentPage == Page.Connect)
                    {
                        dmx = new DMX();

                        for (int i = 1; i <= 512; i++)
                        {
                            Channels.Add(new Channel()
                            {
                                Dmx = dmx,
                                Index = i,
                                Value = 0,
                            });
                        }
                        CurrentPage = Page.Channel;
                        MainPage = new ChannelPage(dmx, Channels);
                    }

                }
                else
                {
                    if (CurrentPage == Page.Channel)
                    {
                        CurrentPage = Page.Connect;
                        MainPage = new ConnectPage();
                    }
                }




                return true; // True = Repeat again, False = Stop the timer
            });

        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            //dmx.Dispose();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
