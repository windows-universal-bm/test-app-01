using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Xbox.Services.System;
using Windows.ApplicationModel.Core;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestApp01 {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged {

        private string logined = "false";
        public string Logined {
            get { return logined; }
            set {
                logined = value;
                OnPropertyChanged();
            }
        }

        private string loginCancel = "false";
        public string LoginCancel {
            get { return loginCancel; }
            set {
                loginCancel = value;
                OnPropertyChanged();
            }
        }

        private string needAuth = "false";
        public string NeedAuth {
            get { return needAuth; }
            set {
                needAuth = value;
                OnPropertyChanged();
            }
        }

        public MainPage() {
            InitializeComponent();
            OpenXboxLogin();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void OpenXboxLogin() {
            Debug.WriteLine("func start");
            var xboxUser = new XboxLiveUser();
            Debug.WriteLine("use xbox user");
            var dispatcher = CoreApplication.GetCurrentView().CoreWindow.Dispatcher;
            Debug.WriteLine("use core dispatcher");
            var result = await xboxUser.SignInSilentlyAsync(dispatcher);
            Debug.WriteLine("check xbox login result");
            await CheckResult(xboxUser, result);
            Debug.WriteLine("func exit");
            Debug.WriteLine("Logined --> " + Logined);
            Debug.WriteLine("Cancel --> " + LoginCancel);
            Debug.WriteLine("NeedAuth --> " + NeedAuth);
            return;
        }

        private async Task CheckResult(XboxLiveUser xbox, SignInResult result) {
            if (result.Status == SignInStatus.Success) {
                Logined = "true";
                Debug.WriteLine("logined");
                return;
            }
            if (result.Status == SignInStatus.UserCancel) {
                LoginCancel = "true";
                Debug.WriteLine("loginCancel");
                return;
            }
            NeedAuth = "true";
            Debug.WriteLine("needAuth");
            var dispatcher = CoreApplication.GetCurrentView().CoreWindow.Dispatcher;
            var result2 = await xbox.SignInAsync(dispatcher);
            await CheckResult(xbox, result2);
        }
    }
}
