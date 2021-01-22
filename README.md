# BlazorBarcodeReader
Barcode Reader component for Blazor using [zxing-js](https://github.com/zxing-js/library) Interop

This project is based in the work of 

https://github.com/sabitertan/BlazorBarcodeScanner

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

Add following lines to `wwwroot\index.html` (for server side `_Host.cshtml`) before `</body>` tag.

```html
    <script src="_content/BlazorBarcodeReader/zxingjs-0.17.1/umd/index.min.js"></script>
    <script src="_content/BlazorBarcodeReader/BlazorBarcodeReader.js"></script>
```

## Usage

Add reference to your `.razor` page/component for this library

```cs
@using BlazorBarcodeReader
```

Add following component ( with `default parameters `) to anywhere you want in your page/component

```html
<BarcodeReader />
```

or with `custom parameters` ( below shows default values of parameters)

```html
<BarcodeReader 
    VideoWidth="300"
    VideoHeigth="200"
    OnBarcodeReaded="BarcodeReaded"
/>

```

Library raises a custom event when barcode scanner reads a value from video stream, you can attach to that event using example below in `@code` block.

```cs
    private string LocalBarcodeText;

    private void BarcodeReaded(string code)
    {
        LocalBarcodeText = code;
    }
```

### Supported Formats
This library uses auto-detect feature of zxing-js library. It supports variety of barcode types. For more information: [zxing-js supported types](https://github.com/zxing-js/library#supported-formats)
