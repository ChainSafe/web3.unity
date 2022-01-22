var myGameInstance = null;

function createUnityInstance(canvas, config, onProgress) {
  onProgress = onProgress || function () {};

  function errorListener(e) {
    var error = e.type == "unhandledrejection" && typeof e.reason == "object" ? e.reason : typeof e.error == "object" ? e.error : null;
    var message = error ? error.toString() : typeof e.message == "string" ? e.message : typeof e.reason == "string" ? e.reason : "";
    if (error && typeof error.stack == "string")
      message += "\n" + error.stack.substring(!error.stack.lastIndexOf(message, 0) ? message.length : 0).replace(/(^\n*|\n*$)/g, "");
    if (!message || !Module.stackTraceRegExp || !Module.stackTraceRegExp.test(message))
      return;
    var filename =
      e instanceof ErrorEvent ? e.filename :
      error && typeof error.fileName == "string" ? error.fileName :
      error && typeof error.sourceURL == "string" ? error.sourceURL :
      "";
    var lineno =
      e instanceof ErrorEvent ? e.lineno :
      error && typeof error.lineNumber == "number" ? error.lineNumber :
      error && typeof error.line == "number" ? error.line :
      0;
#if SYMBOLS_FILENAME
    demanglingErrorHandler(message, filename, lineno);
#else // SYMBOLS_FILENAME
    errorHandler(message, filename, lineno);
#endif // SYMBOLS_FILENAME
  }

  var Module = {
    canvas: canvas,
    webglContextAttributes: {
      preserveDrawingBuffer: false,
    },
#if USE_DATA_CACHING
    cacheControl: function (url) {
      return url == Module.dataUrl ? "must-revalidate" : "no-store";
    },
#endif // USE_DATA_CACHING
#if !USE_WASM
    TOTAL_MEMORY: {{{ TOTAL_MEMORY }}},
#endif // !USE_WASM
    streamingAssetsUrl: "StreamingAssets",
    downloadProgress: {},
    deinitializers: [],
    intervals: {},
    setInterval: function (func, ms) {
      var id = window.setInterval(func, ms);
      this.intervals[id] = true;
      return id;
    },
    clearInterval: function(id) {
      delete this.intervals[id];
      window.clearInterval(id);
    },
    preRun: [],
    postRun: [],
    print: function (message) {
      console.log(message);
    },
    printErr: function (message) {
      console.error(message);
    },
    locateFile: function (url) {
      return (
#if USE_WASM && !DECOMPRESSION_FALLBACK
        url == "build.wasm" ? this.codeUrl :
#endif // USE_WASM && !DECOMPRESSION_FALLBACK
#if USE_THREADS
#if DECOMPRESSION_FALLBACK
        url == "pthread-main.js" ? this.frameworkBlobUrl :
#else // DECOMPRESSION_FALLBACK
        url == "pthread-main.js" ? this.frameworkUrl :
#endif // DECOMPRESSION_FALLBACK
#endif // USE_THREADS
        url
      );
    },
#if USE_THREADS
    // The contents of "pthread-main.js" is embedded in the framework, which is used as a worker source.
    // Therefore Module.mainScriptUrlOrBlob is no longer needed and is set to a dummy blob for compatibility reasons.
    mainScriptUrlOrBlob: new Blob([" "], { type: "application/javascript" }),
#endif // USE_THREADS
    disabledCanvasEvents: [
      "contextmenu",
      "dragstart",
    ],
  };

  for (var parameter in config)
    Module[parameter] = config[parameter];

  Module.streamingAssetsUrl = new URL(Module.streamingAssetsUrl, document.URL).href;

  // Operate on a clone of Module.disabledCanvasEvents field so that at Quit time
  // we will ensure we'll remove the events that we created (in case user has
  // modified/cleared Module.disabledCanvasEvents in between)
  var disabledCanvasEvents = Module.disabledCanvasEvents.slice();

  function preventDefault(e) {
    e.preventDefault();
  }

  disabledCanvasEvents.forEach(function (disabledCanvasEvent) {
    canvas.addEventListener(disabledCanvasEvent, preventDefault);
  });

  window.addEventListener("error", errorListener);
  window.addEventListener("unhandledrejection", errorListener);

  var unityInstance = {
    Module: Module,
    SetFullscreen: function () {
      if (Module.SetFullscreen)
        return Module.SetFullscreen.apply(Module, arguments);
      Module.print("Failed to set Fullscreen mode: Player not loaded yet.");
    },
    SendMessage: function () {
      if (Module.SendMessage)
        return Module.SendMessage.apply(Module, arguments);
      Module.print("Failed to execute SendMessage: Player not loaded yet.");
    },
    Quit: function () {
      return new Promise(function (resolve, reject) {
        Module.shouldQuit = true;
        Module.onQuit = resolve;

        // Clear the event handlers we added above, so that the event handler
        // functions will not hold references to this JS function scope after
        // exit, to allow JS garbage collection to take place.
        disabledCanvasEvents.forEach(function (disabledCanvasEvent) {
          canvas.removeEventListener(disabledCanvasEvent, preventDefault);
        });
        window.removeEventListener("error", errorListener);
        window.removeEventListener("unhandledrejection", errorListener);
      });
    },
  };

  Module.SystemInfo = (function () {
#if 0
    // Recognize and parse the following formats of user agents:

    // Opera 71 on Windows 10:         Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 OPR/71.0.3770.228
    // Edge 85 on Windows 10:          Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 Edg/85.0.564.70
    // Firefox 81 on Windows 10:       Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:81.0) Gecko/20100101 Firefox/81.0
    // Chrome 85 on Windows 10:        Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36
    // IE 11 on Windows 7:             Mozilla/5.0 CK={} (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
    // IE 10 on Windows 7:             Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)

    // Chrome 80 on Android 8.0.0:     Mozilla/5.0 (Linux; Android 8.0.0; VKY-L29) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Mobile Safari/537.36
    // Firefox 68 on Android 8.0.0:    Mozilla/5.0 (Android 8.0.0; Mobile; rv:68.0) Gecko/68.0 Firefox/68.0

    // Samsung Browser on Android 9:   Mozilla/5.0 (Linux; Android 9; SAMSUNG SM-G960U) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/10.2 Chrome/71.0.3578.99 Mobile Safari/537.36
    // Safari 13.0.5 on iPhone 13.3.1: Mozilla/5.0 (iPhone; CPU iPhone OS 13_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.5 Mobile/15E148 Safari/604.1

    // Safari 12.1 on iPad OS 12.2     Mozilla/5.0 (iPad; CPU OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1 Mobile/15E148 Safari/604.1

    // Safari 14 on macOS 11.0:        Mozilla/5.0 (Macintosh; Intel Mac OS X 11_0) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1
    // Safari 14 on macOS 10.15.6:     Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Safari/605.1.15
    // Firefox 80 on macOS 10.15:      Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:80.0) Gecko/20100101 Firefox/80.0
    // Chrome 65 on macOS 10.15.6:     Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36

    // Firefox 57 on FreeBSD:          Mozilla/5.0 (X11; FreeBSD amd64; rv:57.0) Gecko/20100101 Firefox/57.0
    // Chrome 43 on OpenBSD:           Mozilla/5.0 (X11; OpenBSD amd64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.125 Safari/537.36
#endif

    var browser, browserVersion, os, osVersion, canvas, gpu;

    var ua = navigator.userAgent + ' ';
    var browsers = [
      ['Firefox', 'Firefox'],
      ['OPR', 'Opera'],
      ['Edg', 'Edge'],
      ['SamsungBrowser', 'Samsung Browser'],
      ['Trident', 'Internet Explorer'],
      ['MSIE', 'Internet Explorer'],
      ['Chrome', 'Chrome'],
      ['Safari', 'Safari'],
    ];

    function extractRe(re, str, idx) {
      re = RegExp(re, 'i').exec(str);
      return re && re[idx];
    }
    for(var b = 0; b < browsers.length; ++b) {
      browserVersion = extractRe(browsers[b][0] + '[\/ ](.*?)[ \\)]', ua, 1);
      if (browserVersion) {
        browser = browsers[b][1];
        break;
      }
    }
    if (browser == 'Safari') browserVersion = extractRe('Version\/(.*?) ', ua, 1);
    if (browser == 'Internet Explorer') browserVersion = extractRe('rv:(.*?)\\)? ', ua, 1) || browserVersion;

    var oses = [
      ['Windows (.*?)[;\)]', 'Windows'],
      ['Android ([0-9_\.]+)', 'Android'],
      ['iPhone OS ([0-9_\.]+)', 'iPhoneOS'],
      ['iPad.*? OS ([0-9_\.]+)', 'iPadOS'],
      ['FreeBSD( )', 'FreeBSD'],
      ['OpenBSD( )', 'OpenBSD'],
      ['Linux|X11()', 'Linux'],
      ['Mac OS X ([0-9_\.]+)', 'macOS'],
      ['bot|google|baidu|bing|msn|teoma|slurp|yandex', 'Search Bot']
    ];
    for(var o = 0; o < oses.length; ++o) {
      osVersion = extractRe(oses[o][0], ua, 1);
      if (osVersion) {
        os = oses[o][1];
        osVersion = osVersion.replace(/_/g, '.');
        break;
      }
    }
    var versionMappings = {
      'NT 5.0': '2000',
      'NT 5.1': 'XP',
      'NT 5.2': 'Server 2003',
      'NT 6.0': 'Vista',
      'NT 6.1': '7',
      'NT 6.2': '8',
      'NT 6.3': '8.1',
      'NT 10.0': '10'
    };
    osVersion = versionMappings[osVersion] || osVersion;

    // TODO: Add mobile device identifier, e.g. SM-G960U

    canvas = document.createElement("canvas");
    if (canvas) {
      gl = canvas.getContext("webgl2");
      glVersion = gl ? 2 : 0;
      if (!gl) {
        if (gl = canvas && canvas.getContext("webgl")) glVersion = 1;
      }

      if (gl) {
        gpu = (gl.getExtension("WEBGL_debug_renderer_info") && gl.getParameter(0x9246 /*debugRendererInfo.UNMASKED_RENDERER_WEBGL*/)) || gl.getParameter(0x1F01 /*gl.RENDERER*/);
      }
    }

    var hasThreads = typeof SharedArrayBuffer !== 'undefined';
    var hasWasm = typeof WebAssembly === "object" && typeof WebAssembly.compile === "function";
    return {
      width: screen.width,
      height: screen.height,
      userAgent: ua.trim(),
      browser: browser,
      browserVersion: browserVersion,
      mobile: /Mobile|Android|iP(ad|hone)/.test(navigator.appVersion),
      os: os,
      osVersion: osVersion,
      gpu: gpu,
      language: navigator.userLanguage || navigator.language,
      hasWebGL: glVersion,
      hasCursorLock: !!document.body.requestPointerLock,
      hasFullscreen: !!document.body.requestFullscreen,
      hasThreads: hasThreads,
      hasWasm: hasWasm,
      hasWasmThreads: (function() {
        var wasmMemory = hasWasm && hasThreads && new WebAssembly.Memory({"initial": 1, "maximum": 1, "shared": true});
        return wasmMemory && wasmMemory.buffer instanceof SharedArrayBuffer;
      })(),
    };
  })();

  function errorHandler(message, filename, lineno) {
    if (Module.startupErrorHandler) {
      Module.startupErrorHandler(message, filename, lineno);
      return;
    }
    if (Module.errorHandler && Module.errorHandler(message, filename, lineno))
      return;
    console.log("Invoking error handler due to\n" + message);
    if (typeof dump == "function")
      dump("Invoking error handler due to\n" + message);
    // Firefox has a bug where it's IndexedDB implementation will throw UnknownErrors, which are harmless, and should not be shown.
    if (message.indexOf("UnknownError") != -1)
      return;
    // Ignore error when application terminated with return code 0
    if (message.indexOf("Program terminated with exit(0)") != -1)
      return;
    if (errorHandler.didShowErrorMessage)
      return;
    var message = "An error occurred running the Unity content on this page. See your browser JavaScript console for more info. The error was:\n" + message;
    if (message.indexOf("DISABLE_EXCEPTION_CATCHING") != -1) {
      message = "An exception has occurred, but exception handling has been disabled in this build. If you are the developer of this content, enable exceptions in your project WebGL player settings to be able to catch the exception or see the stack trace.";
    } else if (message.indexOf("Cannot enlarge memory arrays") != -1) {
      message = "Out of memory. If you are the developer of this content, try allocating more memory to your WebGL build in the WebGL player settings.";
    } else if (message.indexOf("Invalid array buffer length") != -1  || message.indexOf("Invalid typed array length") != -1 || message.indexOf("out of memory") != -1 || message.indexOf("could not allocate memory") != -1) {
      message = "The browser could not allocate enough memory for the WebGL content. If you are the developer of this content, try allocating less memory to your WebGL build in the WebGL player settings.";
    }
    alert(message);
    errorHandler.didShowErrorMessage = true;
  }

#if SYMBOLS_FILENAME
  function demangleMessage(message, symbols) {
#if USE_WASM
    var symbolExp = "(wasm-function\\[)(\\d+)(\\])";
#else // USE_WASM
    var symbolExp = "(\\n|\\n    at |\\n    at Array\\.)([a-zA-Z0-9_$]+)(@| \\()";
#endif // USE_WASM
    var symbolRegExp = new RegExp(symbolExp);
    return message.replace(new RegExp(symbolExp, "g"), function (symbol) {
      var match = symbol.match(symbolRegExp);
#if USE_WASM
      return match[1] + (symbols[match[2]] ? symbols[match[2]] + "@" : "") + match[2] + match[3];
#else // USE_WASM
      return match[1] + match[2] + (symbols[match[2]] ? "[" + symbols[match[2]] + "]" : "") + match[3];
#endif // USE_WASM
    });
  }

  function demanglingErrorHandler(message, filename, lineno) {
    if (Module.symbols) {
      errorHandler(demangleMessage(message, Module.symbols), filename, lineno);
    } else if (!Module.symbolsUrl) {
      errorHandler(message, filename, lineno);
    } else {
      downloadBinary("symbolsUrl").then(function (data) {
        var json = "";
        for (var i = 0; i < data.length; i++)
          json += String.fromCharCode(data[i]);
        Module.symbols = JSON.parse(json);
        errorHandler(demangleMessage(message, Module.symbols), filename, lineno);
      }).catch(function (error) {
        errorHandler(message, filename, lineno);
      });
    }
  }

#endif // SYMBOLS_FILENAME

  Module.abortHandler = function (message) {
#if SYMBOLS_FILENAME
    demanglingErrorHandler(message, "", 0);
#else // SYMBOLS_FILENAME
    errorHandler(message, "", 0);
#endif // SYMBOLS_FILENAME
    return true;
  };

  Error.stackTraceLimit = Math.max(Error.stackTraceLimit || 0, 50);

  function progressUpdate(id, e) {
    if (id == "symbolsUrl")
      return;
    var progress = Module.downloadProgress[id];
    if (!progress)
      progress = Module.downloadProgress[id] = {
        started: false,
        finished: false,
        lengthComputable: false,
        total: 0,
        loaded: 0,
      };
    if (typeof e == "object" && (e.type == "progress" || e.type == "load")) {
      if (!progress.started) {
        progress.started = true;
        progress.lengthComputable = e.lengthComputable;
        progress.total = e.total;
      }
      progress.loaded = e.loaded;
      if (e.type == "load")
        progress.finished = true;
    }
    var loaded = 0, total = 0, started = 0, computable = 0, unfinishedNonComputable = 0;
    for (var id in Module.downloadProgress) {
      var progress = Module.downloadProgress[id];
      if (!progress.started)
        return 0;
      started++;
      if (progress.lengthComputable) {
        loaded += progress.loaded;
        total += progress.total;
        computable++;
      } else if (!progress.finished) {
        unfinishedNonComputable++;
      }
    }
    var totalProgress = started ? (started - unfinishedNonComputable - (total ? computable * (total - loaded) / total : 0)) / started : 0;
    onProgress(0.9 * totalProgress);
  }

#if USE_DATA_CACHING
  {{{ read("UnityLoader/XMLHttpRequest.js") }}}
#endif // USE_DATA_CACHING

#if DECOMPRESSION_FALLBACK
  var decompressors = {
#if DECOMPRESSION_FALLBACK == "Gzip"
    gzip: {
      require: {{{ read("UnityLoader/Gzip.js") }}},
      decompress: function (data) {
        if (!this.exports)
          this.exports = this.require("inflate.js");
        try { return this.exports.inflate(data) } catch (e) {};
      },
      hasUnityMarker: function (data) {
        var commentOffset = 10, expectedComment = "UnityWeb Compressed Content (gzip)";
        if (commentOffset > data.length || data[0] != 0x1F || data[1] != 0x8B)
          return false;
        var flags = data[3];
        if (flags & 0x04) {
          if (commentOffset + 2 > data.length)
            return false;
          commentOffset += 2 + data[commentOffset] + (data[commentOffset + 1] << 8);
          if (commentOffset > data.length)
            return false;
        }
        if (flags & 0x08) {
          while (commentOffset < data.length && data[commentOffset])
            commentOffset++;
          if (commentOffset + 1 > data.length)
            return false;
          commentOffset++;
        }
        return (flags & 0x10) && String.fromCharCode.apply(null, data.subarray(commentOffset, commentOffset + expectedComment.length + 1)) == expectedComment + "\0";
      },
    },
#endif // DECOMPRESSION_FALLBACK == "Gzip"
#if DECOMPRESSION_FALLBACK == "Brotli"
    br: {
      require: {{{ read("UnityLoader/Brotli.js") }}},
      decompress: function (data) {
        if (!this.exports)
          this.exports = this.require("decompress.js");
        try { return this.exports(data) } catch (e) {};
      },
      hasUnityMarker: function (data) {
        var expectedComment = "UnityWeb Compressed Content (brotli)";
        if (!data.length)
          return false;
        var WBITS_length = (data[0] & 0x01) ? (data[0] & 0x0E) ? 4 : 7 : 1,
            WBITS = data[0] & ((1 << WBITS_length) - 1),
            MSKIPBYTES = 1 + ((Math.log(expectedComment.length - 1) / Math.log(2)) >> 3);
            commentOffset = (WBITS_length + 1 + 2 + 1 + 2 + (MSKIPBYTES << 3) + 7) >> 3;
        if (WBITS == 0x11 || commentOffset > data.length)
          return false;
        var expectedCommentPrefix = WBITS + (((3 << 1) + (MSKIPBYTES << 4) + ((expectedComment.length - 1) << 6)) << WBITS_length);
        for (var i = 0; i < commentOffset; i++, expectedCommentPrefix >>>= 8) {
          if (data[i] != (expectedCommentPrefix & 0xFF))
            return false;
        }
        return String.fromCharCode.apply(null, data.subarray(commentOffset, commentOffset + expectedComment.length)) == expectedComment;
      },
    },
#endif // DECOMPRESSION_FALLBACK == "Brotli"
  };

  function decompress(compressed, url, callback) {
    for (var contentEncoding in decompressors) {
      if (decompressors[contentEncoding].hasUnityMarker(compressed)) {
        if (url)
          console.log("You can reduce startup time if you configure your web server to add \"Content-Encoding: " + contentEncoding + "\" response header when serving \"" + url + "\" file.");
        var decompressor = decompressors[contentEncoding];
        if (!decompressor.worker) {
          var workerUrl = URL.createObjectURL(new Blob(["this.require = ", decompressor.require.toString(), "; this.decompress = ", decompressor.decompress.toString(), "; this.onmessage = ", function (e) {
            var data = { id: e.data.id, decompressed: this.decompress(e.data.compressed) };
            postMessage(data, data.decompressed ? [data.decompressed.buffer] : []);
          }.toString(), "; postMessage({ ready: true });"], { type: "application/javascript" }));
          decompressor.worker = new Worker(workerUrl);
          decompressor.worker.onmessage = function (e) {
            if (e.data.ready) {
              URL.revokeObjectURL(workerUrl);
              return;
            }
            this.callbacks[e.data.id](e.data.decompressed);
            delete this.callbacks[e.data.id];
          };
          decompressor.worker.callbacks = {};
          decompressor.worker.nextCallbackId = 0;
        }
        var id = decompressor.worker.nextCallbackId++;
        decompressor.worker.callbacks[id] = callback;
        decompressor.worker.postMessage({id: id, compressed: compressed}, [compressed.buffer]);
        return;
      }
    }
    callback(compressed);
  }
#endif // DECOMPRESSION_FALLBACK

  function downloadBinary(urlId) {
    return new Promise(function (resolve, reject) {
      progressUpdate(urlId);
#if USE_DATA_CACHING
      var xhr = Module.companyName && Module.productName ? new Module.XMLHttpRequest({
        companyName: Module.companyName,
        productName: Module.productName,
        cacheControl: Module.cacheControl(Module[urlId]),
      }) : new XMLHttpRequest();
#else // USE_DATA_CACHING
      var xhr = new XMLHttpRequest();
#endif // USE_DATA_CACHING
      xhr.open("GET", Module[urlId]);
      xhr.responseType = "arraybuffer";
      xhr.addEventListener("progress", function (e) {
        progressUpdate(urlId, e);
      });
      xhr.addEventListener("load", function(e) {
        progressUpdate(urlId, e);
#if DECOMPRESSION_FALLBACK
        decompress(new Uint8Array(xhr.response), Module[urlId], resolve);
#else // DECOMPRESSION_FALLBACK
        resolve(new Uint8Array(xhr.response));
#endif // DECOMPRESSION_FALLBACK
      });
      xhr.send();
    });
  }

  function downloadFramework() {
#if DECOMPRESSION_FALLBACK
    return downloadBinary("frameworkUrl").then(function (code) {
      var blobUrl = URL.createObjectURL(new Blob([code], { type: "application/javascript" }));
#if USE_THREADS
      Module.frameworkBlobUrl = blobUrl;
#endif // USE_THREADS
#endif // DECOMPRESSION_FALLBACK
      return new Promise(function (resolve, reject) {
        var script = document.createElement("script");
#if DECOMPRESSION_FALLBACK
        script.src = blobUrl;
#else // DECOMPRESSION_FALLBACK
        script.src = Module.frameworkUrl;
#endif // DECOMPRESSION_FALLBACK
        script.onload = function () {
          // Adding the framework.js script to DOM created a global
          // 'unityFramework' variable that should be considered internal.
          // Capture the variable to local scope and clear it from global
          // scope so that JS garbage collection can take place on
          // application quit.
          var fw = unityFramework;
          unityFramework = null;
          // Also ensure this function will not hold any JS scope
          // references to prevent JS garbage collection.
          script.onload = null;
#if DECOMPRESSION_FALLBACK && !USE_THREADS
          URL.revokeObjectURL(blobUrl);
#endif // DECOMPRESSION_FALLBACK && !USE_THREADS
          resolve(fw);
        }
        document.body.appendChild(script);
        Module.deinitializers.push(function() {
          document.body.removeChild(script);
        });
      });
#if DECOMPRESSION_FALLBACK
    });
#endif // DECOMPRESSION_FALLBACK
  }

#if !USE_WASM
  function downloadAsm() {
#if DECOMPRESSION_FALLBACK
    return downloadBinary("codeUrl").then(function (code) {
      var blobUrl = URL.createObjectURL(new Blob([code], { type: "application/javascript" }));
#endif // DECOMPRESSION_FALLBACK
      return new Promise(function (resolve, reject) {
        var script = document.createElement("script");
#if DECOMPRESSION_FALLBACK
        script.src = blobUrl;
#else // DECOMPRESSION_FALLBACK
        script.src = Module.codeUrl;
#endif // DECOMPRESSION_FALLBACK
#if USE_THREADS
        Module.asmJsUrlOrBlob = script.src;
#endif // USE_THREADS
        script.onload = function () {
          delete script.onload;
#if DECOMPRESSION_FALLBACK && !USE_THREADS
          URL.revokeObjectURL(blobUrl);
#endif // DECOMPRESSION_FALLBACK && !USE_THREADS
          resolve();
        }
        document.body.appendChild(script);
        Module.deinitializers.push(function() {
          document.body.removeChild(script);
        });
      });
#if DECOMPRESSION_FALLBACK
    });
#endif // DECOMPRESSION_FALLBACK
  }

#endif // !USE_WASM
  function loadBuild() {
#if USE_WASM
#if DECOMPRESSION_FALLBACK
    Promise.all([
      downloadFramework(),
      downloadBinary("codeUrl"),
    ]).then(function (results) {
      Module.wasmBinary = results[1];
      results[0](Module);
    });

#else // DECOMPRESSION_FALLBACK
    downloadFramework().then(function (unityFramework) {
      unityFramework(Module);
    });

#endif // DECOMPRESSION_FALLBACK
#else // USE_WASM
    Promise.all([
      downloadFramework(),
      downloadAsm(),
    ]).then(function (results) {
      results[0](Module);
    });

#endif // USE_WASM
#if MEMORY_FILENAME
    Module.memoryInitializerRequest = {
      addEventListener: function (type, listener) {
        if (type == "load")
          Module.memoryInitializerRequest.useRequest = listener;
      },
    };
    downloadBinary("memoryUrl").then(function (data) {
      Module.memoryInitializerRequest.status = 200;
      Module.memoryInitializerRequest.response = data;
      if (Module.memoryInitializerRequest.useRequest)
        Module.memoryInitializerRequest.useRequest();
    });

#endif // MEMORY_FILENAME
    var dataPromise = downloadBinary("dataUrl");
    Module.preRun.push(function () {
      Module.addRunDependency("dataUrl");
      dataPromise.then(function (data) {
        var view = new DataView(data.buffer, data.byteOffset, data.byteLength);
        var pos = 0;
        var prefix = "UnityWebData1.0\0";
        if (!String.fromCharCode.apply(null, data.subarray(pos, pos + prefix.length)) == prefix)
          throw "unknown data format";
        pos += prefix.length;
        var headerSize = view.getUint32(pos, true); pos += 4;
        while (pos < headerSize) {
          var offset = view.getUint32(pos, true); pos += 4;
          var size = view.getUint32(pos, true); pos += 4;
          var pathLength = view.getUint32(pos, true); pos += 4;
          var path = String.fromCharCode.apply(null, data.subarray(pos, pos + pathLength)); pos += pathLength;
          for (var folder = 0, folderNext = path.indexOf("/", folder) + 1 ; folderNext > 0; folder = folderNext, folderNext = path.indexOf("/", folder) + 1)
            Module.FS_createPath(path.substring(0, folder), path.substring(folder, folderNext - 1), true, true);
          Module.FS_createDataFile(path, null, data.subarray(offset, offset + size), true, true, true);
        }
        Module.removeRunDependency("dataUrl");
      });
    });
  }

  return new Promise(function (resolve, reject) {
    if (!Module.SystemInfo.hasWebGL) {
      reject("Your browser does not support WebGL.");
  #if !USE_WEBGL_1_0
    } else if (Module.SystemInfo.hasWebGL == 1) {
      reject("Your browser does not support graphics API \"WebGL 2.0\" which is required for this content.");
  #endif // !USE_WEBGL_1_0
  #if USE_WASM
    } else if (!Module.SystemInfo.hasWasm) {
      reject("Your browser does not support WebAssembly.");
  #endif // USE_WASM
  #if USE_THREADS
    } else if (!Module.SystemInfo.hasThreads) {
      reject("Your browser does not support multithreading.");
  #endif // USE_THREADS
    } else {
  #if USE_WEBGL_2_0
      if (Module.SystemInfo.hasWebGL == 1)
        Module.print("Warning: Your browser does not support \"WebGL 2.0\" Graphics API, switching to \"WebGL 1.0\"");
  #endif // USE_WEBGL_2_0
      Module.startupErrorHandler = reject;
      onProgress(0);
      Module.postRun.push(function () {
        onProgress(1);
        delete Module.startupErrorHandler;
        resolve(unityInstance);
      });
      loadBuild();
    }
  });
}
