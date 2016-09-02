using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MaoCloud
{


    public class NodeBroadcastInfo : IEquatable<NodeBroadcastInfo>//, INotifyPropertyChanged
    {
        private NodeBroadcastInfo(
            ref string name,
            ref string ip,
            ref string sysTime,
            ref int count,
            ref double cpuTemp,
            ref double gpuTemp,
            ref double envTemp,
            ref string gpsTime,
            ref double latitude,
            ref double longitude,
            ref int satellite)
        {
            Name = name;
            IP = ip;
            SysTime = sysTime;
            Count = count;
            CpuTemp = cpuTemp;
            GpuTemp = gpuTemp;
            EnvTemp = envTemp;

            GpsTime = gpsTime;
            Latitude = latitude;
            Longitude = longitude;
            Satellite = satellite;
        }



        // ----- 通过“集合中的对象的属性发生改变”事件提醒UI线程更新界面 -----
        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    //if (PropertyChanged != null)
        //    //{
        //    //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    //}
        //}
        //public string name;
        //public string Name { get; set; }
        //{
        //    get { return this.name; }
        //    set { this.name = value; NotifyPropertyChanged(); }
        //}


        public string Name { get; set; }
        public string IP { get; set; }
        public string SysTime { get; set; }
        public int Count { get; set; }
        public double CpuTemp { get; set; }
        public double GpuTemp { get; set; }
        public double EnvTemp { get; set; }
        
        public string GpsTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Satellite { get; set; }


        public bool Equals(NodeBroadcastInfo other)
        {
            return this.Name.Equals(other.Name);
        }


        public static Builder builder() { return new Builder(); }
        public class Builder
        {
            private string name = "";
            private string ip = "";
            private string sysTime = "";
            private int count = 0;
            private double cpuTemp = 0;
            private double gpuTemp = 0;
            private double envTemp = 0;
            private string gpsTime = "";
            private double latitude = 0;
            private double longitude = 0;
            private int satellite = 0;

            public NodeBroadcastInfo build()
            {
                return new NodeBroadcastInfo(
                    ref name,
                    ref ip,
                    ref sysTime,
                    ref count,
                    ref cpuTemp,
                    ref gpuTemp,
                    ref envTemp,
                    ref gpsTime,
                    ref latitude,
                    ref longitude,
                    ref satellite
                    );
            }

            public Builder Name(string name) { this.name = name; return this; }
            public Builder IP(string ip) { this.ip = ip; return this; }
            public Builder SysTime(string sysTime) { this.sysTime = sysTime; return this; }
            public Builder Count(int count) { this.count = count; return this; }
            public Builder CpuTemp(double cpuTemp) { this.cpuTemp = cpuTemp; return this; }
            public Builder GpuTemp(double gpuTemp) { this.gpuTemp = gpuTemp; return this; }
            public Builder EnvTemp(double envTemp) { this.envTemp = envTemp; return this; }

            public Builder GpsTime(string gpsTime) { this.gpsTime = gpsTime; return this; }
            public Builder Latitude(double latitude) { this.latitude = latitude; return this; }
            public Builder Longitude(double longitude) { this.longitude = longitude; return this; }
            public Builder Satellite(int satellite) { this.satellite = satellite; return this; }
        }
    }

    public class NodeBroadcastInfoViewModel
    {
        private ObservableCollection<NodeBroadcastInfo> infos = new ObservableCollection<NodeBroadcastInfo>();
        public ObservableCollection<NodeBroadcastInfo> NodeBroInfos
        {
            get { return infos; }
        }

        public void updateNode(NodeBroadcastInfo nodeInfo)
        {
            if (infos.Contains(nodeInfo))
            {
                infos[infos.IndexOf(nodeInfo)] = nodeInfo;
            }
            else
            {
                infos.Add(nodeInfo);
            }
        }
    }
}
