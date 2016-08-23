using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class NodeBroadcastInfo : IEquatable<NodeBroadcastInfo>//, INotifyPropertyChanged
    {
        public NodeBroadcastInfo(ref string ip,ref double cpuTemp,ref double gpuTemp, string name, ref int count)
        {
            this.ip = ip;
            this.cpuTemp=cpuTemp;
            this.gpuTemp=gpuTemp;
            this.name = name;
            this.Count = count;
        }



        //public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName="")
        {
            //if (PropertyChanged != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
        }

        public string ip;
        public string IP
        {
            get{return this.ip;}
            set { this.ip = value; NotifyPropertyChanged();}
        }
        public double cpuTemp;
        public double CpuTemp
        {
            get { return this.cpuTemp; }
            set { this.cpuTemp = value; NotifyPropertyChanged(); }
        }
        public double gpuTemp;
        public double GpuTemp
        {
            get { return this.gpuTemp; }
            set { this.gpuTemp = value; NotifyPropertyChanged(); }
        }
        public string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; NotifyPropertyChanged(); }
        }
        public int Count { get; set; }

        public bool Equals(NodeBroadcastInfo other)
        {
            return this.name.Equals(other.name);
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
            if(infos.Contains(nodeInfo))
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
