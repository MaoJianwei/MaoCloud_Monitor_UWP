using System;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel.Core;
using Windows.Networking.Sockets;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MaoCloud
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DatagramSocket socket;
        public NodeBroadcastInfoViewModel nodeBroadcastInfoViewModel { get; set; }
        private Dictionary<string, MaoNode> NodeTable = new Dictionary<string, MaoNode>();

        public MainPage()
        {
            this.InitializeComponent();

            this.nodeBroadcastInfoViewModel = new NodeBroadcastInfoViewModel();
        }

        private void ChangeFullScreen(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            //await new MessageDialog("getgetget!\n", "getgetget").ShowAsync();
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

            NodeBroadcastInfo nodeInfo = ParseData(ref data);

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, new DispatchedHandler(() =>
            {
                nodeBroadcastInfoViewModel.updateNode(nodeInfo);
            }));

            if (NodeTable.ContainsKey(nodeInfo.Name))
            {
                MaoNode node = NodeTable[nodeInfo.Name];
                if(node.IP != nodeInfo.IP)
                {
                    node.updateIP(nodeInfo.IP);
                }
                node.seeAgain();
            }
            else
            {
                NodeTable.Add(nodeInfo.Name, new MaoNode(nodeInfo.Name, nodeInfo.IP));
            }

        }
        private NodeBroadcastInfo ParseData(ref string data)
        {
            NodeBroadcastInfo.Builder nodeBroadcastInfoBuilder = NodeBroadcastInfo.builder();

            string[] dataSet = data.Split(';');
            foreach (string s in dataSet)
            {
                string[] pair = s.Split('=');
                switch (pair[0])
                {
                    case "IP": nodeBroadcastInfoBuilder.IP(pair[1]); break;
                    case "CPU_Temp": nodeBroadcastInfoBuilder.CpuTemp(double.Parse(pair[1])); break;
                    case "GPU_Temp": nodeBroadcastInfoBuilder.GpuTemp(double.Parse(pair[1])); break;

                    case "Count": nodeBroadcastInfoBuilder.Count(int.Parse(pair[1])); break;
                    case "SysTime": nodeBroadcastInfoBuilder.SysTime(pair[1]); break;
                    case "NodeName": nodeBroadcastInfoBuilder.Name(pair[1]); break;

                    case "GPS":
                        string[] gpsDataList = pair[1].Split(',');
                        if (!gpsDataList[0].Equals("lost"))
                        {
                            nodeBroadcastInfoBuilder.Latitude(double.Parse(gpsDataList[0]));
                        }
                        if (!gpsDataList[1].Equals("lost"))
                        {
                            nodeBroadcastInfoBuilder.Longitude(double.Parse(gpsDataList[1]));
                        }
                        nodeBroadcastInfoBuilder.Satellite(int.Parse(gpsDataList[2]));
                        break;

                    case "GpsTime": nodeBroadcastInfoBuilder.GpsTime(pair[1]); break;
                    case "Temperature": nodeBroadcastInfoBuilder.EnvTemp(double.Parse(pair[1])); break;
                }
            }
            return nodeBroadcastInfoBuilder.build();
        }
    }
}
