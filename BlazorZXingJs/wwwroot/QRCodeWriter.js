var codeWriter;

export function initLibrary () {

    if (window.ZXing === undefined) {
        return;
    }

    if (codeWriter === undefined ) {
        codeWriter = new ZXing.BrowserQRCodeSvgWriter();
        console.log("CodeWriter initilized");
    }
}

export function writeCode (selector, contents, width, height) {
    if (codeWriter !== undefined ) {
        codeWriter.writeToDom(selector, contents, width, height);
    }
}