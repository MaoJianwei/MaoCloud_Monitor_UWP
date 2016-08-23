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
using Windows.UI.ViewManagement;
using Windows.UI.Popups;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using System.Collections.ObjectModel;
using Windows.System.Threading;


//The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409


//for (int i = 0; i<list.Count;i++ )
//{
//    ListViewItem list_item = new ListViewItem();
//    Model model = list[i];//实例一个实体对象=list中的实体对象
//    list_item.SubItems.Add(model.xx);//xx为相应属性
//    list_item.SubItems.Add(model.xx);
//    list_item.SubItems.Add(model.xx);
//    list_item.SubItems.Add(model.xx);
//    listView1.Items.Add(list_item);//将设置好的listiem添加到items中
//}

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private DatagramSocket socket;
        public NodeBroadcastInfoViewModel nodeBroadcastInfoViewModel { get; set; }


        public MainPage()
        {
            this.InitializeComponent();

            this.nodeBroadcastInfoViewModel = new NodeBroadcastInfoViewModel();
        }

        private void addCount(object sender, RoutedEventArgs e)
        {
            CountText.Text = (int.Parse(CountText.Text) + 1).ToString();
        }

        private async void fullScreen(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("in", "qingdao").ShowAsync();
            ApplicationView view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                view.ExitFullScreenMode();
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
            }
            else
            {
                if (view.TryEnterFullScreenMode())
                {
                    ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
                    await new MessageDialog("fullscreen", "beijing").ShowAsync();

                }
            }
        }

        private async void FullScreen(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            string manipulate = "Expansion = " + e.Cumulative.Expansion + "\n" +
                "Rotation = " + e.Cumulative.Rotation + "\n" +
                "Scale = " + e.Cumulative.Scale;
            await new MessageDialog(manipulate, "Done").ShowAsync();
            //fullScreen(null, null);
        }

        private void ChangeFullScreen(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Expansion > 50)
            {
                ApplicationView view = ApplicationView.GetForCurrentView();
                if (!view.IsFullScreenMode)
                {
                    if (view.TryEnterFullScreenMode())
                    {
                        ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
                    }
                }
            }
            else if (e.Cumulative.Expansion < -50)
            {
                ApplicationView view = ApplicationView.GetForCurrentView();
                if (view.IsFullScreenMode)
                {
                    view.ExitFullScreenMode();
                    ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
                }
            }
        }

        private async void DoRecv(object sender, RoutedEventArgs e)
        {
            if (socket == null)
            {
                socket = new DatagramSocket();
                socket.MessageReceived += RecvData;
                await socket.BindServiceNameAsync("7181");
                await new MessageDialog("socket open !\n" + socket.ToString(), "QINGDAO-well-done").ShowAsync();
            }
            else
            {
                await new MessageDialog("socket ready\n" + socket.ToString(), "QINGDAO-socket-ready").ShowAsync();
            }
        }
        private async void RecvData(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs args)
        {
            StreamReader reader = new StreamReader(args.GetDataStream().AsStreamForRead());
            string data = reader.ReadToEnd();


            string ip;
            double cpuTemp;
            double gpuTemp;
            string name;
            int count;
            ParseData(ref data, out ip, out cpuTemp, out gpuTemp, out name, out count);
            NodeBroadcastInfo nodeInfo1 = new NodeBroadcastInfo(ref ip, ref cpuTemp, ref gpuTemp,  (name+"1"), ref count);
            NodeBroadcastInfo nodeInfo2 = new NodeBroadcastInfo(ref ip, ref cpuTemp, ref gpuTemp,  (name + "222"), ref count);
            NodeBroadcastInfo nodeInfo3 = new NodeBroadcastInfo(ref ip, ref cpuTemp, ref gpuTemp,  (name + "333333"), ref count);

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, new DispatchedHandler(() =>
            {
                nodeBroadcastInfoViewModel.updateNode(nodeInfo1);
                nodeBroadcastInfoViewModel.updateNode(nodeInfo2);
                nodeBroadcastInfoViewModel.updateNode(nodeInfo3);
            }));


            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, new DispatchedHandler(() =>
            {
                BroadcastReport.Text = data.Replace(";", "\n");
                CountText.Text = (int.Parse(CountText.Text) + 1).ToString();
            }));
        }
        private void ParseData(ref string data, out string ip, out double cpuTemp, out double gpuTemp, out string name, out int count)
        {
            ip = "";
            cpuTemp = 0;
            gpuTemp = 0;
            name = "";
            count = 0;
            string [] dataSet = data.Split(';');
            foreach(string s in dataSet)
            {
                string[] pair = s.Split('=');
                switch (pair[0])
                {
                    case "IP": ip = pair[1]; break;
                    case "CPU_Temp": cpuTemp = double.Parse(pair[1]); break;
                    case "GPU_Temp": gpuTemp = double.Parse(pair[1]); break;
                    case "NodeName": name = pair[1]; break;
                    case "Count": count = int.Parse(pair[1]); break;
                }

            }
        }

        private void SendCount(object sender, RoutedEventArgs e)
        {

        }
    }
}