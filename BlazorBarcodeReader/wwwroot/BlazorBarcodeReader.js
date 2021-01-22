// zxing was downloaded from
// https://www.jsdelivr.com/package/npm/@zxing/library?version=0.17.1

var codeReader = new ZXing.BrowserMultiFormatReader();

export async function listVideoInputNames () {
    var devices = await codeReader.listVideoInputDevices();
    var deviceNames = [];

    devices.forEach(element => {
        deviceNames.push(element.label);
    });
    deviceNames.sort();

    return deviceNames;
}

export async function getDeviceByName (deviceName) {
    var devices = await codeReader.listVideoInputDevices();
    var deviceId;

    if (devices.length == 0) {
        return null
    }

    if (deviceName) {
        devices.forEach(element => {
            if (element.label == deviceName) {
                deviceId = element;
            }
        });
    }
    else {
        devices.sort(function(a, b) {
            if (a.label < b.label) return -1;
            if (b.label > a.label) return 1;
            return 0;
        });
        deviceId = devices[0];
    }

    return deviceId;
}

export async function startDecoding (deviceName, videoElementId, targetInputId) {

    codeReader.reset();

    var device = await getDeviceByName(deviceName);

    codeReader.decodeFromVideoDevice(device.deviceId, videoElementId, (result, err) => {
        if (result) {
            console.log(result);
            el = document.getElementById(targetInputId);
            if (el != null) {
                el.value = result.text;
                el.dispatchEvent(new Event('change'));
            }
            else {
                console.log('element '+targetInputId+' not found');
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
    codeReader.reset();
    console.log('Reset.');
}
