using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace WebcamBasic
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaCapture _mediaCapture;
        MediaCaptureInitializationSettings setting;

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += async(s, e) =>
            {
                await InitializeMediaCapture();
            };

            Application.Current.Suspending += async (s, e) =>
            {
                await _mediaCapture.StopPreviewAsync();
                _mediaCapture.Dispose();
            };

            Application.Current.Resuming += async (s, e) =>
            {
                await InitializeMediaCapture();
            };
        }

        private async Task InitializeMediaCapture()
        {
            
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                    DeviceInformation cameraId = devices.ElementAt(0);
                    setting = new MediaCaptureInitializationSettings();
                    setting.VideoDeviceId = cameraId.Id;

                    _mediaCapture = new MediaCapture();

                    await _mediaCapture.InitializeAsync(setting);
                    captureElement.Source = _mediaCapture;
                    await _mediaCapture.StartPreviewAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }
    }
}
