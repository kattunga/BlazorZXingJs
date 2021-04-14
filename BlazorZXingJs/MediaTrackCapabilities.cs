namespace BlazorZXingJs
{
    public struct MediaTrackCapabilitiesZoom
    {
        public double? Max { get; set; }
        public double? Min { get; set; }
        public double? Step { get; set; }
    }

    public struct MediaTrackCapabilities
    {
        public bool Torch { get; set; }
        public string[]? FocusMode { get; set; }
        public MediaTrackCapabilitiesZoom? Zoom { get; set; }
    }
}