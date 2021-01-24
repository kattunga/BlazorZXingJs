![build Actions Status](https://github.com/kattunga/BlazorZXingJs/workflows/build/badge.svg)

[![Nuget](https://img.shields.io/nuget/v/BlazorZXingJs?style=flat-square)](https://www.nuget.org/packages/BlazorZXingJs/)

# BlazorZXingJs

Barcode/QRCode Reader and QRCode writer components.

This is a Blazor wrapper arround [zxing-js](https://github.com/zxing-js/library) library.

## Prerequisites

net 5.0 or newer.

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

Add following lines to `wwwroot\index.html` (for server side `_Host.cshtml`) before `</body>` tag or 
before `<script src="_framework/blazor.server.js"></script>` if you need to scan as soon as app start.

You can use embedded version

```html
    <script src="_content/BlazorBarcodeReader/zxingjs-0.18.2/umd/index.min.js"></script>
```

or use other version, from example a cdn from https://www.jsdelivr.com/package/npm/@zxing/library

```html
    <!-- note that you need to double @@ character to scape from razor engine -->
    <script src="https://cdn.jsdelivr.net/npm/@@zxing/library@@0.18.2/umd/index.min.js"></script>
```

## Example


```html
@page "/"

@using BlazorZXingJs

<BarcodeReader 
    VideoWidth="300"
    VideoHeigth="200"
    OnBarcodeReaded="BarcodeReaded"
/>

<h4>@LocalBarcodeText</h4>

@code {
    private string LocalBarcodeText;

    private void BarcodeReaded(string code)
    {
        LocalBarcodeText = code;
    }
}
```

### Supported Formats

This library uses auto-detect feature of zxing-js library. It supports variety of barcode types. For more information: [zxing-js supported types](https://github.com/zxing-js/library#supported-formats)

### Credits

This project is based on the original work of [sabitertan](https://github.com/sabitertan/BlazorBarcodeScanner)
