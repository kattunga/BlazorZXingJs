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
        [Inject]
        public IJSRuntime jsRuntime {get; set;} = default!;

        [Parameter]
        public BarcodeFormat[]? Format
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
        public string? VideoDeviceId
        {
            get => _videoDeviceId;
            set
            {
                if (_videoDeviceId != value)
                {
                    _videoDeviceId = value;
                    _shouldRestart = true;
                }
            }
        }

        [Parameter]
        public int VideoWidth { get; set; } = 300;

        [Parameter]
        public int VideoHeigth { get; set; } = 200;

        [Parameter]
        public EventCallback<MultiFormatReaderStartEventArgs> OnStartVideo { get; set; }

        [Parameter]
        public EventCallback<string> OnBarcodeRead { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        [Parameter]
        public RenderFragment? VideoForbidden { get; set; }

        [Parameter]
        public RenderFragment? NoVideoDevices { get; set; }

        private IJSObjectReference? _jsModule;
        private string? _videoDeviceId;
        private BarcodeFormat[]? _format;
        private List<MediaDeviceInfo> _videoInputDevices = new List<MediaDeviceInfo>();
        private bool _initialized;
        private string? _domException;
        private bool _starting;
        private bool _shouldRestart;

        private class StartResponse
        {
            public string? DeviceId { get; set; }
            public string? ErrorName { get; set; }
            public string? ErrorMessage { get; set; }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorZXingJs/MultiFormatReader.js");
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
                var i = _videoInputDevices.FindIndex(item => item.DeviceId == _videoDeviceId);
                i++;
                if (i >= _videoInputDevices.Count)
                {
                    i = 0;
                }
                _videoDeviceId = _videoInputDevices[i].DeviceId;
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
                    var resp = await _jsModule.InvokeAsync<StartResponse>("startDecoding", _videoDeviceId, FormatToList(_format), "zxingVideo", "zxingInput");

                    _domException = resp.ErrorName;

                    if (_domException != null)
                    {
                        _videoDeviceId = resp.DeviceId;

                        if (!_initialized)
                        {
                            _initialized = true;
                            _videoInputDevices = await _jsModule.InvokeAsync<List<MediaDeviceInfo>>("listVideoInputDevices");
                        }
                    }

                    await OnStartVideo.InvokeAsync(new MultiFormatReaderStartEventArgs(_videoDeviceId, _videoInputDevices, resp.ErrorName, resp.ErrorMessage));

                    StateHasChanged();
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

        private static string? FormatToList(BarcodeFormat[]? format)
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

        private async Task OnCodeRead(ChangeEventArgs args)
        {
            if (OnBarcodeRead.HasDelegate && args.Value != null)
            {
                await OnBarcodeRead.InvokeAsync(args.Value.ToString());
            }
        }
    }
}