using System;
using System.Collections.Generic;

namespace BlazorZXingJs
{
    public class MultiFormatReaderStartEventArgs : EventArgs
    {
        public MultiFormatReaderStartEventArgs(string? deviceId, List<MediaDeviceInfo> deviceList)
        {
            DeviceId = deviceId;
            DeviceList = deviceList;
        }

        public string? DeviceId { get; set; }
        public List<MediaDeviceInfo> DeviceList { get; set; }
        public bool VideoPermission {
            get {
                return DeviceId != null;
            }
        }
    }
}