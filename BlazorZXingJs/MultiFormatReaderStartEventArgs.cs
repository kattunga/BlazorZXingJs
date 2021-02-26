using System;
using System.Collections.Generic;

namespace BlazorZXingJs
{
    public class MultiFormatReaderStartEventArgs : EventArgs
    {
        public MultiFormatReaderStartEventArgs(string? deviceId, List<MediaDeviceInfo> deviceList, bool videoPermission)
        {
            DeviceId = deviceId;
            DeviceList = deviceList;
            VideoPermission = videoPermission;
        }

        public string? DeviceId { get; init; }
        public List<MediaDeviceInfo> DeviceList { get; init; }
        public bool VideoPermission {get; init;}
    }
}