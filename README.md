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

This component require the umd version of [zxing-js](https://github.com/zxing-js/library) library.

Embeded with the component is 0.17.1 verion but you can use any other version.

you can download or add a cdn link from here:

https://www.jsdelivr.com/package/npm/@zxing/library

Add following lines to `wwwroot\index.html` (for server side `_Host.cshtml`) before `</body>` tag.

```html
    <script src="_content/BlazorBarcodeReader/zxingjs-0.17.1/umd/index.min.js"></script>
```

## Example


```html
@using BlazorBarcodeReader

<BarcodeReader 
    VideoWidth="300"
    VideoHeigth="200"
    OnBarcodeReaded="BarcodeReaded"
/>

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
