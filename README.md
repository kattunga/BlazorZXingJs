![Nuget](https://img.shields.io/nuget/v/BlazorBarcodeReader?style=flat-square)

![build Actions Status](https://github.com/kattunga/BlazorBarcodeReader/workflows/build/badge.svg)

# BlazorBarcodeReader
Barcode Reader component for Blazor using [zxing-js](https://github.com/zxing-js/library) Interop

This project is based on the work of [sabitertan](https://github.com/sabitertan/BlazorBarcodeScanner)

## Prerequisites

Before you continue, please make sure you have the latest version of Visual Studio and .Net Core installed. Visit official [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/client) site to learn more.

## Installation

### 1. NuGet packages

```
Install-Package BlazorBarcodeReader
```

or

```
dotnet add package BlazorBarcodeReader
```

### 2. Refence to JS libraries

This component require the umd version of [zxing-js](https://github.com/zxing-js/library) library.

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
@using BlazorBarcodeReader

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
