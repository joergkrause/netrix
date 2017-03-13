using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SplashScreenTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Loaded,
                (DispatcherOperationCallback)delegate { CloseSplashScreen(); return null; },
                this);
            base.OnStartup(e);
        }

        private void CloseSplashScreen()
        {
            // signal the native process (that launched us) to close the splash screen
            using (var closeSplashEvent = new EventWaitHandle(false,
                EventResetMode.ManualReset, "CloseSplashScreenEventSplashScreenStarter"))
            {
                closeSplashEvent.Set();
            }
        }
    }
}
