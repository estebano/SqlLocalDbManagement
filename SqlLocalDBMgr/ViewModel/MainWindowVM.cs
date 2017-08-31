using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using SqlLocalDBManagementLib;
using SqlLocalDBManagementLib.Models;

namespace SqlLocalDBMgr.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private ObservableCollection<InstanceInfo> instancesinfos;
        public ObservableCollection<InstanceInfo> InstancesInfos
        {
            get
            {
                return instancesinfos;
            }
            set
            {
                instancesinfos = value;
                this.RaisePropertyChanged(() => InstancesInfos);
            }
        }

        public MainWindowVM()
        {
            InitCommands();
            var basic = new Basic();
            var instances = basic.GetInstancesNames();
            InstancesInfos = new ObservableCollection<InstanceInfo>();
            foreach (var instance in instances)
            {
                instancesinfos.Add(basic.GetInstanceStatus(instance));
            }
        }

        private void InitCommands()
        {
        }

        public int SelectedIstanceIndex { get; set; }
        public ObservableCollection<InstanceInfo> Instancesinfos { get => instancesinfos; set => instancesinfos = value; }
    }
}

