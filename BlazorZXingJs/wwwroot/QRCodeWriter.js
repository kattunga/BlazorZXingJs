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

export function writeCode (container, contents, width, height) {
    container.innerHTML = "";
    if (codeWriter !== undefined && contents) {
        codeWriter.writeToDom(container, contents, width, height);
    }
}