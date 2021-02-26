![build Actions Status](https://github.com/kattunga/BlazorZXingJs/workflows/build/badge.svg)

[![Nuget](https://img.shields.io/nuget/v/BlazorZXingJs?style=flat-square)](https://www.nuget.org/packages/BlazorZXingJs/)

# BlazorZXingJs

Barcode/QRCode Reader and QRCode writer components.

This is a Blazor wrapper arround [zxing-js](https://github.com/zxing-js/library) library.

### Supported Formats

It supports variety of barcode and 2d code types. For more information: [zxing-js supported types](https://github.com/zxing-js/library#supported-formats)

## Prerequisites

NET 5.0 or newer

## Installation

### 1. NuGet packages

```
Install-Package BlazorZXingJs
```

or

```
dotnet add package BlazorZXingJs
```

### 2. Refence to ZXing-JS library

This component requires the umd version of [zxing-js](https://github.com/zxing-js/library) library.

For blazor wasm, in `wwwroot\index.html`
```html
...
<body>
    ...
    <script src="_framework/blazor.webassembly.js"></script>

    <!-- if you need to scan as soon as the app start, add this before _framework/blazor.webassembly.js -->
    <script src="_content/BlazorZXingJs/zxingjs-0.18.2/umd/index.min.js"></script>
</body>
```

For blazor server, in `Pages/_Host.cshtml`
```html
...
<body>
    ...
    <script src="_framework/blazor.server.js"></script>

    <!-- if you need to scan as soon as the app start, add this before _framework/blazor.server.js -->
    <script src="_content/BlazorZXingJs/zxingjs-0.18.2/umd/index.min.js"></script>
</body>
```

## Examples

### Basic Read code with autodetect

```html
@page "/"

@using BlazorZXingJs

<MultiFormatReader
    VideoWidth="300"
    VideoHeigth="200"
    OnBarcodeRead="BarcodeRead">

    <VideoForbidden>
        <h4>no permission for videodevice</h4>
    </VideoForbidden>

    <NoVideoDevices>
        <h4>no devices available</h4>
    </NoVideoDevices>
</MultiFormatReader>

<h4>@LocalBarcodeText</h4>

@code {
    private string LocalBarcodeText;

    private void BarcodeRead(string code)
    {
        LocalBarcodeText = code;
    }
}
```

### Write QRCode

```html
@page "/"

@using BlazorZXingJs

<QRCodeWriter Text="@LocalBarcodeText" Width="200" Heigth="200"></QRCodeWriter>

@code {
    private string LocalBarcodeText = "this is a message";

}

### Credits

This project is based on the original work of [sabitertan](https://github.com/sabitertan/BlazorBarcodeScanner)
