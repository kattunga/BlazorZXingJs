using System;
using System.Collections.Generic;

namespace BlazorZXingJs
{
    public class MultiFormatReaderStartEventArgs : EventArgs
    {
        public MultiFormatReaderStartEventArgs(string? deviceId, MediaTrackCapabilities? deviceCapabilities, string? deviceCapabilitiesJson, List<MediaDeviceInfo> deviceList, string? domExceptionName, string? domExceptionMessage)
        {
            DeviceId = deviceId;
            DeviceCapabilities = deviceCapabilities;
            DeviceCapabilitiesJson = deviceCapabilitiesJson;
            DeviceList = deviceList;
            DOMExceptionName = domExceptionName;
            DOMExceptionMessage = domExceptionMessage;
        }

        public string? DeviceId { get; init; }
        public MediaTrackCapabilities? DeviceCapabilities { get; init; }
        public string? DeviceCapabilitiesJson { get; init; }
        public List<MediaDeviceInfo> DeviceList { get; init; }
        public string? DOMExceptionName {get; init;}
        public string? DOMExceptionMessage {get; init;}
    }
}