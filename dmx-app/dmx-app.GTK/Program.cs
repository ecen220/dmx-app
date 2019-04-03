using System;
using dmx_app;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace dmxapp.GTK
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();

            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("DMX app");
            window.SetDefaultSize(200, 800);
            window.Show();

            Gtk.Application.Run();
        }
    }
}
