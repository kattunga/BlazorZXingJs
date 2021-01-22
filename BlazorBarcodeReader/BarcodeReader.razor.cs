using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorBarcodeReader
{
    public enum BarcodeFormat
    {

        /** Aztec 2D barcode format. */
        AZTEC,

        /** CODABAR 1D format. */
        CODABAR,

        /** Code 39 1D format. */
        CODE_39,

        /** Code 93 1D format. */
        CODE_93,

        /** Code 128 1D format. */
        CODE_128,

        /** Data Matrix 2D barcode format. */
        DATA_MATRIX,

        /** EAN-8 1D format. */
        EAN_8,

        /** EAN-13 1D format. */
        EAN_13,

        /** ITF (Interleaved Two of Five) 1D format. */
        ITF,

        /** MaxiCode 2D barcode format. */
        MAXICODE,

        /** PDF417 format. */
        PDF_417,

        /** QR Code 2D barcode format. */
        QR_CODE,

        /** RSS 14 */
        RSS_14,

        /** RSS EXPANDED */
        RSS_EXPANDED,

        /** UPC-A 1D format. */
        UPC_A,

        /** UPC-E 1D format. */
        UPC_E,

        /** UPC/EAN extension format. Not a stand-alone format. */
        UPC_EAN_EXTENSION
    }

    public partial class BarcodeReader: IAsyncDisposable
    {
        [Parameter]
        public EventCallback<string> OnBarcodeReaded { get; set; }

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

        public List<string> VideoInputDevices => _videoInputDevices;

        private IJSObjectReference _jsModule;
        private string _inputDevice = null;
        private BarcodeFormat[] _format;
        private List<string> _videoInputDevices = new List<string>();
        private bool _starting = false;
        private bool _shouldRestart;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorBarcodeReader/BlazorBarcodeReader.js");
                await _jsModule.InvokeVoidAsync("initLibrary", FormatToList(_format));
                _videoInputDevices = await _jsModule.InvokeAsync<List<string>>("listVideoInputNames", "get");;
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

        private async Task OnCodeReaded(ChangeEventArgs args) {
            if (OnBarcodeReaded.HasDelegate)
            {
                await OnBarcodeReaded.InvokeAsync(args.Value.ToString());
            }
        }
    }
}