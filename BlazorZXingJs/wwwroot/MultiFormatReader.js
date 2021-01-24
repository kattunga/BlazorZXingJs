// zxing was downloaded from
// https://www.jsdelivr.com/package/npm/@zxing/library?version=0.18.2

var codeReader;
var codeFormat;

export function initLibrary (format) {

    if (window.ZXing === undefined) {
        return;
    }

    if (codeReader !== undefined) {
        codeReader.reset();
    }

    if (codeReader === undefined || format != codeFormat) {

        if (format) {
            var formatList = format.split(',');
            var formatMaps = [];


            if (formatList.includes('AZTEC')) {
                formatMaps.push(ZXing.BarcodeFormat.AZTEC);
            }
            if (formatList.includes('CODABAR')) {
                formatMaps.push(ZXing.BarcodeFormat.CODABAR);
            }
            if (formatList.includes('CODE_39')) {
                formatMaps.push(ZXing.BarcodeFormat.CODE_39);
            }
            if (formatList.includes('CODE_93')) {
                formatMaps.push(ZXing.BarcodeFormat.CODE_93);
            }
            if (formatList.includes('CODE_128')) {
                formatMaps.push(ZXing.BarcodeFormat.CODE_128);
            }
            if (formatList.includes('DATA_MATRIX')) {
                formatMaps.push(ZXing.BarcodeFormat.DATA_MATRIX);
            }
            if (formatList.includes('EAN_8')) {
                formatMaps.push(ZXing.BarcodeFormat.EAN_8);
            }
            if (formatList.includes('EAN_13')) {
                formatMaps.push(ZXing.BarcodeFormat.EAN_13);
            }
            if (formatList.includes('ITF')) {
                formatMaps.push(ZXing.BarcodeFormat.ITF);
            }
            if (formatList.includes('MAXICODE')) {
                formatMaps.push(ZXing.BarcodeFormat.MAXICODE);
            }
            if (formatList.includes('PDF_417')) {
                formatMaps.push(ZXing.BarcodeFormat.PDF_417);
            }
            if (formatList.includes('QR_CODE')) {
                formatMaps.push(ZXing.BarcodeFormat.QR_CODE);
            }
            if (formatList.includes('RSS_14')) {
                formatMaps.push(ZXing.BarcodeFormat.RSS_14);
            }
            if (formatList.includes('RSS_EXPANDED')) {
                formatMaps.push(ZXing.BarcodeFormat.RSS_EXPANDED);
            }
            if (formatList.includes('UPC_A')) {
                formatMaps.push(ZXing.BarcodeFormat.UPC_A);
            }
            if (formatList.includes('UPC_E')) {
                formatMaps.push(ZXing.BarcodeFormat.UPC_E);
            }
            if (formatList.includes('UPC_EAN_EXTENSION')) {
                formatMaps.push(ZXing.BarcodeFormat.UPC_EAN_EXTENSION);
            }

            if (formatMaps.length != formatList.length) {
                console.error("there are wrong formats specified");
            }

            const hints = new Map();
            hints.set(ZXing.DecodeHintType.POSSIBLE_FORMATS, formatMaps);

            codeReader = new ZXing.BrowserMultiFormatReader(hints);
            console.log("BarcodeReader initilized with "+format+" format");
        }
        else {
            codeReader = new ZXing.BrowserMultiFormatReader();
            console.log("BarcodeReader initilized with autodetect format");
        }

        codeFormat = format;
    }
}

export async function listVideoInputNames () {
    var deviceNames = [];

    if (codeReader === undefined) {
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

    if (codeReader === undefined) {
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

    initLibrary(format);

    if (codeReader === undefined) {
        return null;
    }

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
        if (err) {
            // As long as this error belongs into one of the following categories
            // the code reader is going to continue as excepted. Any other error
            // will stop the decoding loop.
            //
            // Excepted Exceptions:
            //
            //  - NotFoundException
            //  - ChecksumException
            //  - FormatException
            if (err instanceof ZXing.ChecksumException) {
                console.log('A code was found, but it\'s read value was not valid.')
            }

            if (err instanceof ZXing.FormatException) {
                console.log('A code was found, but it was in a invalid format.')
            }
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
