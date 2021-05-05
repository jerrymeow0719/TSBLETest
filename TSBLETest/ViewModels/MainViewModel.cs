using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace TSBLETest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        string PairDeviceName = "DPB30_920BE7";

        //Plugin
        IBluetoothLE bleInterface;
        IAdapter bleAdapter;
        public IDevice _device = null;
        public bool _isScan = false;
        public int _ScanCount = 0;
        public event PropertyChangedEventHandler PropertyChanged;

        public IDevice DeviceInfo_ID
        {
            get { return _device; }
            set
            {
                _device = value;
                RaisePropertyChanged();
            }
        }

        public bool IsScanning
        {
            get { return _isScan; }
            set
            {
                _isScan = value;
                RaisePropertyChanged();
            }
        }

        public int ScanCount
        {
            get { return _ScanCount; }
            set
            {
                _ScanCount = value;
                RaisePropertyChanged();
            }
        }

        protected void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        public async void StartScan()
        {
            bleInterface = CrossBluetoothLE.Current;
            bleAdapter = CrossBluetoothLE.Current.Adapter;

            try
            {
                bleAdapter.ScanMode = ScanMode.LowLatency;
                bleAdapter.DeviceDiscovered += BleAdapter_DeviceDiscovered;
                bleAdapter.ScanTimeoutElapsed += BleAdapter_ScanTimeoutElapsed;
                bleAdapter.ScanTimeout = 3000;

                if (!bleInterface.Adapter.IsScanning)
                {
                    await bleAdapter.StartScanningForDevicesAsync();
                    IsScanning =true;
                    if (_ScanCount == 0)
                        ScanCount = 1;
                }
            }
            catch (Exception)
            {
            }
        }

        public async void StopScan()
        {
            await bleAdapter.StopScanningForDevicesAsync();
            IsScanning = false;
            //ScanCount = 0;
        }

        private void BleAdapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            //Re-Start
            if (!bleInterface.Adapter.IsScanning)
                bleAdapter.StartScanningForDevicesAsync();
        }

        private async void BleAdapter_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if (string.Compare(PairDeviceName, e.Device.Name) == 0)
            {
                if (DeviceInfo_ID == null || DeviceInfo_ID.Id != e.Device.Id)
                {
                    DeviceInfo_ID = e.Device;
                    ScanCount++;
                    if (_ScanCount == 10)
                        ScanCount = 1;

                    //Connect to device
                    if (DeviceInfo_ID != null)
                    {
                        try
                        {
                            await bleAdapter.ConnectToDeviceAsync(DeviceInfo_ID);
                        }
                        catch 
                        {
                            await bleAdapter.StopScanningForDevicesAsync();
                            IsScanning = false;
                            return;
                        }
                    }

                    await bleAdapter.StopScanningForDevicesAsync();

                    //Read specific service
                    //ParseServices();
                    IList<IService> Services;
                    IService Service;
                    Services = (IList<IService>)await DeviceInfo_ID.GetServicesAsync();

                    Service = Services.First(item => item.Id.ToString().Contains("fd00"));

                    //Read specific characteristic
                    IList<ICharacteristic> characteristics;
                    ICharacteristic characteristic;
                    characteristics = (IList<ICharacteristic>)await Service.GetCharacteristicsAsync();
                    characteristic = characteristics.First(item => item.Uuid.Contains("fd01"));

                    byte[] data = new byte[3] { 0, 1, 1 };
                    await characteristic.WriteAsync(data);

                    byte[] getdata = new byte[3];
                    getdata = await characteristic.ReadAsync();

                    if (ByteArraryCompare(data, getdata))
                    {
                        await bleAdapter.DisconnectDeviceAsync(DeviceInfo_ID);

                        await bleAdapter.StartScanningForDevicesAsync();
                    }
                }
            }
        }

        public void ClearCLick()
        {
            DeviceInfo_ID = null;
        }

        private bool ByteArraryCompare(byte[] Array1,byte[] Array2)
        {
            int byteLength = (Array1.Length > Array2.Length) ? Array2.Length : Array1.Length;
            for(int index=0;index<byteLength-1;index++)
            {
                if (Array1[index] != Array2[index])
                    return false;
            }
            return true;
        }
    }
}
