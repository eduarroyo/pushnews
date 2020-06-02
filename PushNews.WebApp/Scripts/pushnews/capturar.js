function Capturar(contenedor, opciones) {
    var _contenedor = contenedor,
        video = _contenedor.find("#CaptureDisplay"),
        videoObj = { "video": true },
        _videoStream,
        opDefault = {
            beforeGetUserMedia: function () { },
            afterGetUserMedia: function () { },
            errorGetUserMedia: function (error) {
                console.log("Video capture error: ", error.code);
            }
        },
        _opciones = $.extend({}, opDefault, opciones);

    function init() {

    }

    function start() {
        _opciones.beforeGetUserMedia();
        if (navigator.getUserMedia) { // Standard
            navigator.getUserMedia(videoObj, function (stream) {
                _videoStream = stream;
                video.attr("src", _videoStream);
                video[0].play();
                _opciones.afterGetUserMedia();
            }, _opciones.errorGetUserMedia);
        } else if (navigator.webkitGetUserMedia) { // WebKit-prefixed
            navigator.webkitGetUserMedia(videoObj, function (stream) {
                _videoStream = stream;
                video.attr("src", window.URL.createObjectURL(_videoStream));
                video[0].play();
                _opciones.afterGetUserMedia();
            }, _opciones.errorGetUserMedia);
        }
        else if (navigator.mozGetUserMedia) { // Firefox-prefixed
            navigator.mozGetUserMedia(videoObj, function (stream) {
                _videoStream = stream;
                video.attr("src", window.URL.createObjectURL(_videoStream));
                video[0].play();
                _opciones.afterGetUserMedia();
            }, _opciones.errorGetUserMedia);
        } else {
            opDefault.afterGetUserMedia();
        }
    }

    function stop() {
        if (_videoStream) {
            _videoStream.stop();
            video.removeAttr("src");
            _videoStream = null;
        }
    }

    function capture() {
        contenedor.trigger("capture", [video[0]]);
        return video[0];
    }

    function destroy() {
        if (_videoStream && videostream.hasOwnProperty("stop") && typeof (videostream.stop) === "function") {
            _videoStream.stop();
        }
    }

    return {
        start: start,
        stop: stop,
        capture: capture,
        init: init,
        destroy: destroy
    }
}