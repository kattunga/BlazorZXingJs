// zxing was downloaded from
// https://www.jsdelivr.com/package/npm/@zxing/library?version=0.18.2

var codeReader;
var codeFormat = "MultiFormat";

export function initLibrary (format) {
    if (codeReader === undefined || format != codeFormat) {
        if (format == "Barcode") {
            stopDecoding();
            codeReader = new ZXing.BrowserBarcodeReader()
        }
        else if (format == "QR") {
            stopDecoding();
            codeReader = new ZXing.BrowserQRCodeReader()
        }
        else if (format == "Aztec") {
            stopDecoding();
            codeReader = new ZXing.BrowserAztecCodeReader()
        }
        else if (format == "Datamatrix") {
            stopDecoding();
            codeReader = new ZXing.BrowserDatamatrixCodeReader()
        }
        else if (format == "PDF417") {
            stopDecoding();
            codeReader = new ZXing.BrowserPDF417Reader()
        }
        else if (format == "MultiFormat") {
            stopDecoding();
            codeReader = new ZXing.BrowserMultiFormatReader();
        }
        else {
            console.error('unknown format '+format);
            return false;
        }
        codeFormat = format;
        console.log("BarcodeReader initilized with "+format+" format");
    }
    return true;
}

export async function listVideoInputNames () {
    var deviceNames = [];

    if (!initLibrary(codeFormat)) {
        return deviceNames;
    }

    var devices = await codeReader.listVideoInputDevices();

    devices.forEach(element => {
        deviceNames.push(element.label);
    });
    deviceNames.sort();

    return deviceNames;
}

export async function getDeviceByName (deviceName) {
    var deviceId;

    if (!initLibrary(codeFormat)) {
        return deviceId;
    }

    var devices = await codeReader.listVideoInputDevices();

    if (devices.length == 0) {
        return deviceId;
    }

    if (deviceName) {
        devices.forEach(element => {
            if (element.label == deviceName) {
                deviceId = element;
            }
        });
    }

    if (deviceId === undefined)
    {
        devices.sort(function(a, b) {
            if (a.label < b.label) return -1;
            if (b.label > a.label) return 1;
            return 0;
        });
        deviceId = devices[0];
    }

    return deviceId;
}

export async function startDecoding (deviceName, format, videoElementId, targetInputId) {
    if (!initLibrary(format)) {
        return null;
    }

    codeReader.reset();

    var device = await getDeviceByName(deviceName);

    codeReader.decodeFromVideoDevice(device.deviceId, videoElementId, (result, err) => {
        if (result) {
            console.log(result);
            var el = document.getElementById(targetInputId);
            if (el != null) {
                el.value = result.text;
                el.dispatchEvent(new Event('change'));
            }
            else {
                console.error('element '+targetInputId+' not found');
            }
        }
        if (err && !(err instanceof ZXing.NotFoundException)) {
            console.error(err);
        }
    })

    console.log('Started continous decode from camera with deviceid '+device.label);

    return device.label;
}

export function stopDecoding () {
    if (codeReader !== undefined) {
        codeReader.reset();
        console.log('Reset.');
    }
}
