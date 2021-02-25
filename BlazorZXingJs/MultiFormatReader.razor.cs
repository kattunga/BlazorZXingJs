using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorZXingJs
{
    public partial class MultiFormatReader: IAsyncDisposable
    {
        [Parameter]
        public EventCallback<string> OnBarcodeRead { get; set; }

        [Parameter]
        public BarcodeFormat[] Format
        {
            get => _format;
            set
            {
                if (FormatToList(_format) != FormatToList(value))
                {
                    _format = value;
                    _shouldRestart = true;
                }
            }
        }

        [Parameter]
        public string InputDevice
        {
            get => _inputDevice;
            set
            {
                if (_inputDevice != value)
                {
                    _inputDevice = value;
                    _shouldRestart = true;
                }
            }
        }

        [Parameter]
        public int VideoWidth { get; set; } = 300;

        [Parameter]
        public int VideoHeigth { get; set; } = 200;

        [Parameter]
        public EventCallback<string> InputDeviceChanged { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [Inject]
        public IJSRuntime jsRuntime { get; set; }

        public bool Initialized => _initialized;
        public List<string> VideoInputDevices => _videoInputDevices;
        public bool CameraPermission => _cameraPermission;

        private IJSObjectReference _jsModule;
        private string _inputDevice = null;
        private BarcodeFormat[] _format;
        private List<string> _videoInputDevices = new List<string>();
        private bool _initialized;
        private bool _cameraPermission;
        private bool _starting = false;
        private bool _shouldRestart;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorZXingJs/MultiFormatReader.js");
                await _jsModule.InvokeVoidAsync("initLibrary", FormatToList(_format));
                _cameraPermission = await _jsModule.InvokeAsync<bool>("checkVideoPermission");
                _videoInputDevices = await _jsModule.InvokeAsync<List<string>>("listVideoInputNames");
                _initialized = true;
            }
            if (firstRender || _shouldRestart)
            {
                _shouldRestart = false;
                await StartDecoding();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await StopDecoding();
        }

        public async Task ToggleDevice()
        {
            if (_videoInputDevices.Count > 1)
            {
                var i = _videoInputDevices.IndexOf(_inputDevice);
                i++;
                if (i >= _videoInputDevices.Count)
                {
                    i = 0;
                }
                _inputDevice = _videoInputDevices[i];
                await StartDecoding();
            }
        }

        public async Task StartDecoding()
        {
            if (_jsModule != null && !_starting)
            {
                _starting = true;
                try
                {
                    var inputDevice = await _jsModule.InvokeAsync<string>("startDecoding", _inputDevice, FormatToList(_format), "zxingVideo", "zxingInput");
                    if (inputDevice != _inputDevice)
                    {
                        await InputDeviceChanged.InvokeAsync(inputDevice);
                    }
                }
                finally
                {
                    _starting = false;
                }
            }
        }

        public async Task StopDecoding()
        {
            if (_jsModule != null)
            {
                await _jsModule.InvokeVoidAsync("stopDecoding");
            }
        }

        private static string FormatToList(BarcodeFormat[] format)
        {
            if (format != null)
            {
                return string.Join(',', format.Select(x => x.ToString()));
            }
            else
            {
                return null;
            }
        }

        private async Task OnCodeRead(ChangeEventArgs args) {
            if (OnBarcodeRead.HasDelegate)
            {
                await OnBarcodeRead.InvokeAsync(args.Value.ToString());
            }
        }
    }
}