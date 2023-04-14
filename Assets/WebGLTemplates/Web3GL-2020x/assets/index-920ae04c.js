import { _ as __extends$1 } from "./tslib.es6-b95777e8.js";
var dist = {};
var MAX_SAFE_INTEGER = 9007199254740991;
var argsTag = "[object Arguments]", funcTag = "[object Function]", genTag = "[object GeneratorFunction]";
var reIsUint = /^(?:0|[1-9]\d*)$/;
function apply(func, thisArg, args) {
  switch (args.length) {
    case 0:
      return func.call(thisArg);
    case 1:
      return func.call(thisArg, args[0]);
    case 2:
      return func.call(thisArg, args[0], args[1]);
    case 3:
      return func.call(thisArg, args[0], args[1], args[2]);
  }
  return func.apply(thisArg, args);
}
function baseTimes(n2, iteratee) {
  var index = -1, result = Array(n2);
  while (++index < n2) {
    result[index] = iteratee(index);
  }
  return result;
}
var objectProto = Object.prototype;
var hasOwnProperty = objectProto.hasOwnProperty;
var objectToString = objectProto.toString;
var propertyIsEnumerable = objectProto.propertyIsEnumerable;
var nativeMax = Math.max;
function arrayLikeKeys(value, inherited) {
  var result = isArray$2(value) || isArguments(value) ? baseTimes(value.length, String) : [];
  var length = result.length, skipIndexes = !!length;
  for (var key in value) {
    if ((inherited || hasOwnProperty.call(value, key)) && !(skipIndexes && (key == "length" || isIndex(key, length)))) {
      result.push(key);
    }
  }
  return result;
}
function assignInDefaults(objValue, srcValue, key, object) {
  if (objValue === void 0 || eq(objValue, objectProto[key]) && !hasOwnProperty.call(object, key)) {
    return srcValue;
  }
  return objValue;
}
function assignValue(object, key, value) {
  var objValue = object[key];
  if (!(hasOwnProperty.call(object, key) && eq(objValue, value)) || value === void 0 && !(key in object)) {
    object[key] = value;
  }
}
function baseKeysIn(object) {
  if (!isObject$2(object)) {
    return nativeKeysIn(object);
  }
  var isProto = isPrototype(object), result = [];
  for (var key in object) {
    if (!(key == "constructor" && (isProto || !hasOwnProperty.call(object, key)))) {
      result.push(key);
    }
  }
  return result;
}
function baseRest(func, start) {
  start = nativeMax(start === void 0 ? func.length - 1 : start, 0);
  return function() {
    var args = arguments, index = -1, length = nativeMax(args.length - start, 0), array = Array(length);
    while (++index < length) {
      array[index] = args[start + index];
    }
    index = -1;
    var otherArgs = Array(start + 1);
    while (++index < start) {
      otherArgs[index] = args[index];
    }
    otherArgs[start] = array;
    return apply(func, this, otherArgs);
  };
}
function copyObject(source, props, object, customizer) {
  object || (object = {});
  var index = -1, length = props.length;
  while (++index < length) {
    var key = props[index];
    var newValue = customizer ? customizer(object[key], source[key], key, object, source) : void 0;
    assignValue(object, key, newValue === void 0 ? source[key] : newValue);
  }
  return object;
}
function createAssigner(assigner) {
  return baseRest(function(object, sources) {
    var index = -1, length = sources.length, customizer = length > 1 ? sources[length - 1] : void 0, guard = length > 2 ? sources[2] : void 0;
    customizer = assigner.length > 3 && typeof customizer == "function" ? (length--, customizer) : void 0;
    if (guard && isIterateeCall(sources[0], sources[1], guard)) {
      customizer = length < 3 ? void 0 : customizer;
      length = 1;
    }
    object = Object(object);
    while (++index < length) {
      var source = sources[index];
      if (source) {
        assigner(object, source, index, customizer);
      }
    }
    return object;
  });
}
function isIndex(value, length) {
  length = length == null ? MAX_SAFE_INTEGER : length;
  return !!length && (typeof value == "number" || reIsUint.test(value)) && (value > -1 && value % 1 == 0 && value < length);
}
function isIterateeCall(value, index, object) {
  if (!isObject$2(object)) {
    return false;
  }
  var type = typeof index;
  if (type == "number" ? isArrayLike$2(object) && isIndex(index, object.length) : type == "string" && index in object) {
    return eq(object[index], value);
  }
  return false;
}
function isPrototype(value) {
  var Ctor = value && value.constructor, proto = typeof Ctor == "function" && Ctor.prototype || objectProto;
  return value === proto;
}
function nativeKeysIn(object) {
  var result = [];
  if (object != null) {
    for (var key in Object(object)) {
      result.push(key);
    }
  }
  return result;
}
function eq(value, other) {
  return value === other || value !== value && other !== other;
}
function isArguments(value) {
  return isArrayLikeObject(value) && hasOwnProperty.call(value, "callee") && (!propertyIsEnumerable.call(value, "callee") || objectToString.call(value) == argsTag);
}
var isArray$2 = Array.isArray;
function isArrayLike$2(value) {
  return value != null && isLength(value.length) && !isFunction$2(value);
}
function isArrayLikeObject(value) {
  return isObjectLike(value) && isArrayLike$2(value);
}
function isFunction$2(value) {
  var tag = isObject$2(value) ? objectToString.call(value) : "";
  return tag == funcTag || tag == genTag;
}
function isLength(value) {
  return typeof value == "number" && value > -1 && value % 1 == 0 && value <= MAX_SAFE_INTEGER;
}
function isObject$2(value) {
  var type = typeof value;
  return !!value && (type == "object" || type == "function");
}
function isObjectLike(value) {
  return !!value && typeof value == "object";
}
var assignInWith = createAssigner(function(object, source, srcIndex, customizer) {
  copyObject(source, keysIn(source), object, customizer);
});
var defaults$1 = baseRest(function(args) {
  args.push(void 0, assignInDefaults);
  return apply(assignInWith, void 0, args);
});
function keysIn(object) {
  return isArrayLike$2(object) ? arrayLikeKeys(object, true) : baseKeysIn(object);
}
var lodash_defaults = defaults$1;
Object.defineProperty(dist, "__esModule", { value: true });
var defaults = lodash_defaults;
var SHOULD_RECONNECT_FALSE_MESSAGE = "Provided shouldReconnect() returned false. Closing permanently.";
var SHOULD_RECONNECT_PROMISE_FALSE_MESSAGE = "Provided shouldReconnect() resolved to false. Closing permanently.";
var SturdyWebSocket = (
  /** @class */
  function() {
    function SturdyWebSocket2(url, protocolsOrOptions, options) {
      this.url = url;
      this.onclose = null;
      this.onerror = null;
      this.onmessage = null;
      this.onopen = null;
      this.ondown = null;
      this.onreopen = null;
      this.CONNECTING = SturdyWebSocket2.CONNECTING;
      this.OPEN = SturdyWebSocket2.OPEN;
      this.CLOSING = SturdyWebSocket2.CLOSING;
      this.CLOSED = SturdyWebSocket2.CLOSED;
      this.hasBeenOpened = false;
      this.isClosed = false;
      this.messageBuffer = [];
      this.nextRetryTime = 0;
      this.reconnectCount = 0;
      this.lastKnownExtensions = "";
      this.lastKnownProtocol = "";
      this.listeners = {};
      if (protocolsOrOptions == null || typeof protocolsOrOptions === "string" || Array.isArray(protocolsOrOptions)) {
        this.protocols = protocolsOrOptions;
      } else {
        options = protocolsOrOptions;
      }
      this.options = defaults({}, options, SturdyWebSocket2.DEFAULT_OPTIONS);
      if (!this.options.wsConstructor) {
        if (typeof WebSocket !== "undefined") {
          this.options.wsConstructor = WebSocket;
        } else {
          throw new Error("WebSocket not present in global scope and no wsConstructor option was provided.");
        }
      }
      this.openNewWebSocket();
    }
    Object.defineProperty(SturdyWebSocket2.prototype, "binaryType", {
      get: function() {
        return this.binaryTypeInternal || "blob";
      },
      set: function(binaryType) {
        this.binaryTypeInternal = binaryType;
        if (this.ws) {
          this.ws.binaryType = binaryType;
        }
      },
      enumerable: true,
      configurable: true
    });
    Object.defineProperty(SturdyWebSocket2.prototype, "bufferedAmount", {
      get: function() {
        var sum = this.ws ? this.ws.bufferedAmount : 0;
        var hasUnknownAmount = false;
        this.messageBuffer.forEach(function(data) {
          var byteLength = getDataByteLength(data);
          if (byteLength != null) {
            sum += byteLength;
          } else {
            hasUnknownAmount = true;
          }
        });
        if (hasUnknownAmount) {
          this.debugLog("Some buffered data had unknown length. bufferedAmount() return value may be below the correct amount.");
        }
        return sum;
      },
      enumerable: true,
      configurable: true
    });
    Object.defineProperty(SturdyWebSocket2.prototype, "extensions", {
      get: function() {
        return this.ws ? this.ws.extensions : this.lastKnownExtensions;
      },
      enumerable: true,
      configurable: true
    });
    Object.defineProperty(SturdyWebSocket2.prototype, "protocol", {
      get: function() {
        return this.ws ? this.ws.protocol : this.lastKnownProtocol;
      },
      enumerable: true,
      configurable: true
    });
    Object.defineProperty(SturdyWebSocket2.prototype, "readyState", {
      get: function() {
        return this.isClosed ? SturdyWebSocket2.CLOSED : SturdyWebSocket2.OPEN;
      },
      enumerable: true,
      configurable: true
    });
    SturdyWebSocket2.prototype.close = function(code, reason) {
      if (this.ws) {
        this.ws.close(code, reason);
      }
      this.shutdown();
      this.debugLog("WebSocket permanently closed by client.");
    };
    SturdyWebSocket2.prototype.send = function(data) {
      if (this.ws && this.ws.readyState === this.OPEN) {
        this.ws.send(data);
      } else {
        this.messageBuffer.push(data);
      }
    };
    SturdyWebSocket2.prototype.addEventListener = function(type, listener) {
      if (!this.listeners[type]) {
        this.listeners[type] = [];
      }
      this.listeners[type].push(listener);
    };
    SturdyWebSocket2.prototype.dispatchEvent = function(event2) {
      return this.dispatchEventOfType(event2.type, event2);
    };
    SturdyWebSocket2.prototype.removeEventListener = function(type, listener) {
      if (this.listeners[type]) {
        this.listeners[type] = this.listeners[type].filter(function(l) {
          return l !== listener;
        });
      }
    };
    SturdyWebSocket2.prototype.openNewWebSocket = function() {
      var _this = this;
      if (this.isClosed) {
        return;
      }
      var _a = this.options, connectTimeout = _a.connectTimeout, wsConstructor = _a.wsConstructor;
      this.debugLog("Opening new WebSocket to " + this.url + ".");
      var ws = new wsConstructor(this.url, this.protocols);
      ws.onclose = function(event2) {
        return _this.handleClose(event2);
      };
      ws.onerror = function(event2) {
        return _this.handleError(event2);
      };
      ws.onmessage = function(event2) {
        return _this.handleMessage(event2);
      };
      ws.onopen = function(event2) {
        return _this.handleOpen(event2);
      };
      this.connectTimeoutId = setTimeout(function() {
        _this.clearConnectTimeout();
        ws.close();
      }, connectTimeout);
      this.ws = ws;
    };
    SturdyWebSocket2.prototype.handleOpen = function(event2) {
      var _this = this;
      if (!this.ws || this.isClosed) {
        return;
      }
      var allClearResetTime = this.options.allClearResetTime;
      this.debugLog("WebSocket opened.");
      if (this.binaryTypeInternal != null) {
        this.ws.binaryType = this.binaryTypeInternal;
      } else {
        this.binaryTypeInternal = this.ws.binaryType;
      }
      this.clearConnectTimeout();
      if (this.hasBeenOpened) {
        this.dispatchEventOfType("reopen", event2);
      } else {
        this.dispatchEventOfType("open", event2);
        this.hasBeenOpened = true;
      }
      this.messageBuffer.forEach(function(message) {
        return _this.send(message);
      });
      this.messageBuffer = [];
      this.allClearTimeoutId = setTimeout(function() {
        _this.clearAllClearTimeout();
        _this.nextRetryTime = 0;
        _this.reconnectCount = 0;
        var openTime = allClearResetTime / 1e3 | 0;
        _this.debugLog("WebSocket remained open for " + openTime + " seconds. Resetting retry time and count.");
      }, allClearResetTime);
    };
    SturdyWebSocket2.prototype.handleMessage = function(event2) {
      if (this.isClosed) {
        return;
      }
      this.dispatchEventOfType("message", event2);
    };
    SturdyWebSocket2.prototype.handleClose = function(event2) {
      var _this = this;
      if (this.isClosed) {
        return;
      }
      var _a = this.options, maxReconnectAttempts = _a.maxReconnectAttempts, shouldReconnect = _a.shouldReconnect;
      this.clearConnectTimeout();
      this.clearAllClearTimeout();
      if (this.ws) {
        this.lastKnownExtensions = this.ws.extensions;
        this.lastKnownProtocol = this.ws.protocol;
        this.ws = void 0;
      }
      this.dispatchEventOfType("down", event2);
      if (this.reconnectCount >= maxReconnectAttempts) {
        this.stopReconnecting(event2, this.getTooManyFailedReconnectsMessage());
        return;
      }
      var willReconnect = shouldReconnect(event2);
      if (typeof willReconnect === "boolean") {
        this.handleWillReconnect(willReconnect, event2, SHOULD_RECONNECT_FALSE_MESSAGE);
      } else {
        willReconnect.then(function(willReconnectResolved) {
          if (_this.isClosed) {
            return;
          }
          _this.handleWillReconnect(willReconnectResolved, event2, SHOULD_RECONNECT_PROMISE_FALSE_MESSAGE);
        });
      }
    };
    SturdyWebSocket2.prototype.handleError = function(event2) {
      this.dispatchEventOfType("error", event2);
      this.debugLog("WebSocket encountered an error.");
    };
    SturdyWebSocket2.prototype.handleWillReconnect = function(willReconnect, event2, denialReason) {
      if (willReconnect) {
        this.reconnect();
      } else {
        this.stopReconnecting(event2, denialReason);
      }
    };
    SturdyWebSocket2.prototype.reconnect = function() {
      var _this = this;
      var _a = this.options, minReconnectDelay = _a.minReconnectDelay, maxReconnectDelay = _a.maxReconnectDelay, reconnectBackoffFactor = _a.reconnectBackoffFactor;
      this.reconnectCount++;
      var retryTime = this.nextRetryTime;
      this.nextRetryTime = Math.max(minReconnectDelay, Math.min(this.nextRetryTime * reconnectBackoffFactor, maxReconnectDelay));
      setTimeout(function() {
        return _this.openNewWebSocket();
      }, retryTime);
      var retryTimeSeconds = retryTime / 1e3 | 0;
      this.debugLog("WebSocket was closed. Re-opening in " + retryTimeSeconds + " seconds.");
    };
    SturdyWebSocket2.prototype.stopReconnecting = function(event2, debugReason) {
      this.debugLog(debugReason);
      this.shutdown();
      this.dispatchEventOfType("close", event2);
    };
    SturdyWebSocket2.prototype.shutdown = function() {
      this.isClosed = true;
      this.clearAllTimeouts();
      this.messageBuffer = [];
    };
    SturdyWebSocket2.prototype.clearAllTimeouts = function() {
      this.clearConnectTimeout();
      this.clearAllClearTimeout();
    };
    SturdyWebSocket2.prototype.clearConnectTimeout = function() {
      if (this.connectTimeoutId != null) {
        clearTimeout(this.connectTimeoutId);
        this.connectTimeoutId = void 0;
      }
    };
    SturdyWebSocket2.prototype.clearAllClearTimeout = function() {
      if (this.allClearTimeoutId != null) {
        clearTimeout(this.allClearTimeoutId);
        this.allClearTimeoutId = void 0;
      }
    };
    SturdyWebSocket2.prototype.dispatchEventOfType = function(type, event2) {
      var _this = this;
      switch (type) {
        case "close":
          if (this.onclose) {
            this.onclose(event2);
          }
          break;
        case "error":
          if (this.onerror) {
            this.onerror(event2);
          }
          break;
        case "message":
          if (this.onmessage) {
            this.onmessage(event2);
          }
          break;
        case "open":
          if (this.onopen) {
            this.onopen(event2);
          }
          break;
        case "down":
          if (this.ondown) {
            this.ondown(event2);
          }
          break;
        case "reopen":
          if (this.onreopen) {
            this.onreopen(event2);
          }
          break;
      }
      if (type in this.listeners) {
        this.listeners[type].slice().forEach(function(listener) {
          return _this.callListener(listener, event2);
        });
      }
      return !event2 || !event2.defaultPrevented;
    };
    SturdyWebSocket2.prototype.callListener = function(listener, event2) {
      if (typeof listener === "function") {
        listener.call(this, event2);
      } else {
        listener.handleEvent.call(this, event2);
      }
    };
    SturdyWebSocket2.prototype.debugLog = function(message) {
      if (this.options.debug) {
        console.log(message);
      }
    };
    SturdyWebSocket2.prototype.getTooManyFailedReconnectsMessage = function() {
      var maxReconnectAttempts = this.options.maxReconnectAttempts;
      return "Failed to reconnect after " + maxReconnectAttempts + " " + pluralize("attempt", maxReconnectAttempts) + ". Closing permanently.";
    };
    SturdyWebSocket2.DEFAULT_OPTIONS = {
      allClearResetTime: 5e3,
      connectTimeout: 5e3,
      debug: false,
      minReconnectDelay: 1e3,
      maxReconnectDelay: 3e4,
      maxReconnectAttempts: Number.POSITIVE_INFINITY,
      reconnectBackoffFactor: 1.5,
      shouldReconnect: function() {
        return true;
      },
      wsConstructor: void 0
    };
    SturdyWebSocket2.CONNECTING = 0;
    SturdyWebSocket2.OPEN = 1;
    SturdyWebSocket2.CLOSING = 2;
    SturdyWebSocket2.CLOSED = 3;
    return SturdyWebSocket2;
  }()
);
var _default = dist.default = SturdyWebSocket;
function getDataByteLength(data) {
  if (typeof data === "string") {
    return 2 * data.length;
  } else if (data instanceof ArrayBuffer) {
    return data.byteLength;
  } else if (data instanceof Blob) {
    return data.size;
  } else {
    return void 0;
  }
}
function pluralize(s, n2) {
  return n2 === 1 ? s : s + "s";
}
class Base {
  /**
   * Extends this object and runs the init method.
   * Arguments to create() will be passed to init().
   *
   * @return {Object} The new object.
   *
   * @static
   *
   * @example
   *
   *     var instance = MyType.create();
   */
  static create(...args) {
    return new this(...args);
  }
  /**
   * Copies properties into this object.
   *
   * @param {Object} properties The properties to mix in.
   *
   * @example
   *
   *     MyType.mixIn({
   *         field: 'value'
   *     });
   */
  mixIn(properties) {
    return Object.assign(this, properties);
  }
  /**
   * Creates a copy of this object.
   *
   * @return {Object} The clone.
   *
   * @example
   *
   *     var clone = instance.clone();
   */
  clone() {
    const clone = new this.constructor();
    Object.assign(clone, this);
    return clone;
  }
}
class WordArray extends Base {
  /**
   * Initializes a newly created word array.
   *
   * @param {Array} words (Optional) An array of 32-bit words.
   * @param {number} sigBytes (Optional) The number of significant bytes in the words.
   *
   * @example
   *
   *     var wordArray = CryptoJS.lib.WordArray.create();
   *     var wordArray = CryptoJS.lib.WordArray.create([0x00010203, 0x04050607]);
   *     var wordArray = CryptoJS.lib.WordArray.create([0x00010203, 0x04050607], 6);
   */
  constructor(words = [], sigBytes = words.length * 4) {
    super();
    let typedArray = words;
    if (typedArray instanceof ArrayBuffer) {
      typedArray = new Uint8Array(typedArray);
    }
    if (typedArray instanceof Int8Array || typedArray instanceof Uint8ClampedArray || typedArray instanceof Int16Array || typedArray instanceof Uint16Array || typedArray instanceof Int32Array || typedArray instanceof Uint32Array || typedArray instanceof Float32Array || typedArray instanceof Float64Array) {
      typedArray = new Uint8Array(typedArray.buffer, typedArray.byteOffset, typedArray.byteLength);
    }
    if (typedArray instanceof Uint8Array) {
      const typedArrayByteLength = typedArray.byteLength;
      const _words = [];
      for (let i = 0; i < typedArrayByteLength; i += 1) {
        _words[i >>> 2] |= typedArray[i] << 24 - i % 4 * 8;
      }
      this.words = _words;
      this.sigBytes = typedArrayByteLength;
    } else {
      this.words = words;
      this.sigBytes = sigBytes;
    }
  }
  /**
   * Creates a word array filled with random bytes.
   *
   * @param {number} nBytes The number of random bytes to generate.
   *
   * @return {WordArray} The random word array.
   *
   * @static
   *
   * @example
   *
   *     var wordArray = CryptoJS.lib.WordArray.random(16);
   */
  static random(nBytes) {
    const words = [];
    const r = (m_w) => {
      let _m_w = m_w;
      let _m_z = 987654321;
      const mask = 4294967295;
      return () => {
        _m_z = 36969 * (_m_z & 65535) + (_m_z >> 16) & mask;
        _m_w = 18e3 * (_m_w & 65535) + (_m_w >> 16) & mask;
        let result = (_m_z << 16) + _m_w & mask;
        result /= 4294967296;
        result += 0.5;
        return result * (Math.random() > 0.5 ? 1 : -1);
      };
    };
    for (let i = 0, rcache; i < nBytes; i += 4) {
      const _r = r((rcache || Math.random()) * 4294967296);
      rcache = _r() * 987654071;
      words.push(_r() * 4294967296 | 0);
    }
    return new WordArray(words, nBytes);
  }
  /**
   * Converts this word array to a string.
   *
   * @param {Encoder} encoder (Optional) The encoding strategy to use. Default: CryptoJS.enc.Hex
   *
   * @return {string} The stringified word array.
   *
   * @example
   *
   *     var string = wordArray + '';
   *     var string = wordArray.toString();
   *     var string = wordArray.toString(CryptoJS.enc.Utf8);
   */
  toString(encoder = Hex) {
    return encoder.stringify(this);
  }
  /**
   * Concatenates a word array to this word array.
   *
   * @param {WordArray} wordArray The word array to append.
   *
   * @return {WordArray} This word array.
   *
   * @example
   *
   *     wordArray1.concat(wordArray2);
   */
  concat(wordArray) {
    const thisWords = this.words;
    const thatWords = wordArray.words;
    const thisSigBytes = this.sigBytes;
    const thatSigBytes = wordArray.sigBytes;
    this.clamp();
    if (thisSigBytes % 4) {
      for (let i = 0; i < thatSigBytes; i += 1) {
        const thatByte = thatWords[i >>> 2] >>> 24 - i % 4 * 8 & 255;
        thisWords[thisSigBytes + i >>> 2] |= thatByte << 24 - (thisSigBytes + i) % 4 * 8;
      }
    } else {
      for (let i = 0; i < thatSigBytes; i += 4) {
        thisWords[thisSigBytes + i >>> 2] = thatWords[i >>> 2];
      }
    }
    this.sigBytes += thatSigBytes;
    return this;
  }
  /**
   * Removes insignificant bits.
   *
   * @example
   *
   *     wordArray.clamp();
   */
  clamp() {
    const { words, sigBytes } = this;
    words[sigBytes >>> 2] &= 4294967295 << 32 - sigBytes % 4 * 8;
    words.length = Math.ceil(sigBytes / 4);
  }
  /**
   * Creates a copy of this word array.
   *
   * @return {WordArray} The clone.
   *
   * @example
   *
   *     var clone = wordArray.clone();
   */
  clone() {
    const clone = super.clone.call(this);
    clone.words = this.words.slice(0);
    return clone;
  }
}
const Hex = {
  /**
   * Converts a word array to a hex string.
   *
   * @param {WordArray} wordArray The word array.
   *
   * @return {string} The hex string.
   *
   * @static
   *
   * @example
   *
   *     var hexString = CryptoJS.enc.Hex.stringify(wordArray);
   */
  stringify(wordArray) {
    const { words, sigBytes } = wordArray;
    const hexChars = [];
    for (let i = 0; i < sigBytes; i += 1) {
      const bite = words[i >>> 2] >>> 24 - i % 4 * 8 & 255;
      hexChars.push((bite >>> 4).toString(16));
      hexChars.push((bite & 15).toString(16));
    }
    return hexChars.join("");
  },
  /**
   * Converts a hex string to a word array.
   *
   * @param {string} hexStr The hex string.
   *
   * @return {WordArray} The word array.
   *
   * @static
   *
   * @example
   *
   *     var wordArray = CryptoJS.enc.Hex.parse(hexString);
   */
  parse(hexStr) {
    const hexStrLength = hexStr.length;
    const words = [];
    for (let i = 0; i < hexStrLength; i += 2) {
      words[i >>> 3] |= parseInt(hexStr.substr(i, 2), 16) << 24 - i % 8 * 4;
    }
    return new WordArray(words, hexStrLength / 2);
  }
};
const Latin1 = {
  /**
   * Converts a word array to a Latin1 string.
   *
   * @param {WordArray} wordArray The word array.
   *
   * @return {string} The Latin1 string.
   *
   * @static
   *
   * @example
   *
   *     var latin1String = CryptoJS.enc.Latin1.stringify(wordArray);
   */
  stringify(wordArray) {
    const { words, sigBytes } = wordArray;
    const latin1Chars = [];
    for (let i = 0; i < sigBytes; i += 1) {
      const bite = words[i >>> 2] >>> 24 - i % 4 * 8 & 255;
      latin1Chars.push(String.fromCharCode(bite));
    }
    return latin1Chars.join("");
  },
  /**
   * Converts a Latin1 string to a word array.
   *
   * @param {string} latin1Str The Latin1 string.
   *
   * @return {WordArray} The word array.
   *
   * @static
   *
   * @example
   *
   *     var wordArray = CryptoJS.enc.Latin1.parse(latin1String);
   */
  parse(latin1Str) {
    const latin1StrLength = latin1Str.length;
    const words = [];
    for (let i = 0; i < latin1StrLength; i += 1) {
      words[i >>> 2] |= (latin1Str.charCodeAt(i) & 255) << 24 - i % 4 * 8;
    }
    return new WordArray(words, latin1StrLength);
  }
};
const Utf8 = {
  /**
   * Converts a word array to a UTF-8 string.
   *
   * @param {WordArray} wordArray The word array.
   *
   * @return {string} The UTF-8 string.
   *
   * @static
   *
   * @example
   *
   *     var utf8String = CryptoJS.enc.Utf8.stringify(wordArray);
   */
  stringify(wordArray) {
    try {
      return decodeURIComponent(escape(Latin1.stringify(wordArray)));
    } catch (e) {
      throw new Error("Malformed UTF-8 data");
    }
  },
  /**
   * Converts a UTF-8 string to a word array.
   *
   * @param {string} utf8Str The UTF-8 string.
   *
   * @return {WordArray} The word array.
   *
   * @static
   *
   * @example
   *
   *     var wordArray = CryptoJS.enc.Utf8.parse(utf8String);
   */
  parse(utf8Str) {
    return Latin1.parse(unescape(encodeURIComponent(utf8Str)));
  }
};
class BufferedBlockAlgorithm extends Base {
  constructor() {
    super();
    this._minBufferSize = 0;
  }
  /**
   * Resets this block algorithm's data buffer to its initial state.
   *
   * @example
   *
   *     bufferedBlockAlgorithm.reset();
   */
  reset() {
    this._data = new WordArray();
    this._nDataBytes = 0;
  }
  /**
   * Adds new data to this block algorithm's buffer.
   *
   * @param {WordArray|string} data
   *
   *     The data to append. Strings are converted to a WordArray using UTF-8.
   *
   * @example
   *
   *     bufferedBlockAlgorithm._append('data');
   *     bufferedBlockAlgorithm._append(wordArray);
   */
  _append(data) {
    let m_data = data;
    if (typeof m_data === "string") {
      m_data = Utf8.parse(m_data);
    }
    this._data.concat(m_data);
    this._nDataBytes += m_data.sigBytes;
  }
  /**
   * Processes available data blocks.
   *
   * This method invokes _doProcessBlock(offset), which must be implemented by a concrete subtype.
   *
   * @param {boolean} doFlush Whether all blocks and partial blocks should be processed.
   *
   * @return {WordArray} The processed data.
   *
   * @example
   *
   *     var processedData = bufferedBlockAlgorithm._process();
   *     var processedData = bufferedBlockAlgorithm._process(!!'flush');
   */
  _process(doFlush) {
    let processedWords;
    const { _data: data, blockSize } = this;
    const dataWords = data.words;
    const dataSigBytes = data.sigBytes;
    const blockSizeBytes = blockSize * 4;
    let nBlocksReady = dataSigBytes / blockSizeBytes;
    if (doFlush) {
      nBlocksReady = Math.ceil(nBlocksReady);
    } else {
      nBlocksReady = Math.max((nBlocksReady | 0) - this._minBufferSize, 0);
    }
    const nWordsReady = nBlocksReady * blockSize;
    const nBytesReady = Math.min(nWordsReady * 4, dataSigBytes);
    if (nWordsReady) {
      for (let offset = 0; offset < nWordsReady; offset += blockSize) {
        this._doProcessBlock(dataWords, offset);
      }
      processedWords = dataWords.splice(0, nWordsReady);
      data.sigBytes -= nBytesReady;
    }
    return new WordArray(processedWords, nBytesReady);
  }
  /**
   * Creates a copy of this object.
   *
   * @return {Object} The clone.
   *
   * @example
   *
   *     var clone = bufferedBlockAlgorithm.clone();
   */
  clone() {
    const clone = super.clone.call(this);
    clone._data = this._data.clone();
    return clone;
  }
}
class Hasher extends BufferedBlockAlgorithm {
  constructor(cfg) {
    super();
    this.blockSize = 512 / 32;
    this.cfg = Object.assign(new Base(), cfg);
    this.reset();
  }
  /**
   * Creates a shortcut function to a hasher's object interface.
   *
   * @param {Hasher} SubHasher The hasher to create a helper for.
   *
   * @return {Function} The shortcut function.
   *
   * @static
   *
   * @example
   *
   *     var SHA256 = CryptoJS.lib.Hasher._createHelper(CryptoJS.algo.SHA256);
   */
  static _createHelper(SubHasher) {
    return (message, cfg) => new SubHasher(cfg).finalize(message);
  }
  /**
   * Creates a shortcut function to the HMAC's object interface.
   *
   * @param {Hasher} SubHasher The hasher to use in this HMAC helper.
   *
   * @return {Function} The shortcut function.
   *
   * @static
   *
   * @example
   *
   *     var HmacSHA256 = CryptoJS.lib.Hasher._createHmacHelper(CryptoJS.algo.SHA256);
   */
  static _createHmacHelper(SubHasher) {
    return (message, key) => new HMAC(SubHasher, key).finalize(message);
  }
  /**
   * Resets this hasher to its initial state.
   *
   * @example
   *
   *     hasher.reset();
   */
  reset() {
    super.reset.call(this);
    this._doReset();
  }
  /**
   * Updates this hasher with a message.
   *
   * @param {WordArray|string} messageUpdate The message to append.
   *
   * @return {Hasher} This hasher.
   *
   * @example
   *
   *     hasher.update('message');
   *     hasher.update(wordArray);
   */
  update(messageUpdate) {
    this._append(messageUpdate);
    this._process();
    return this;
  }
  /**
   * Finalizes the hash computation.
   * Note that the finalize operation is effectively a destructive, read-once operation.
   *
   * @param {WordArray|string} messageUpdate (Optional) A final message update.
   *
   * @return {WordArray} The hash.
   *
   * @example
   *
   *     var hash = hasher.finalize();
   *     var hash = hasher.finalize('message');
   *     var hash = hasher.finalize(wordArray);
   */
  finalize(messageUpdate) {
    if (messageUpdate) {
      this._append(messageUpdate);
    }
    const hash = this._doFinalize();
    return hash;
  }
}
class HMAC extends Base {
  /**
   * Initializes a newly created HMAC.
   *
   * @param {Hasher} SubHasher The hash algorithm to use.
   * @param {WordArray|string} key The secret key.
   *
   * @example
   *
   *     var hmacHasher = CryptoJS.algo.HMAC.create(CryptoJS.algo.SHA256, key);
   */
  constructor(SubHasher, key) {
    super();
    const hasher = new SubHasher();
    this._hasher = hasher;
    let _key = key;
    if (typeof _key === "string") {
      _key = Utf8.parse(_key);
    }
    const hasherBlockSize = hasher.blockSize;
    const hasherBlockSizeBytes = hasherBlockSize * 4;
    if (_key.sigBytes > hasherBlockSizeBytes) {
      _key = hasher.finalize(key);
    }
    _key.clamp();
    const oKey = _key.clone();
    this._oKey = oKey;
    const iKey = _key.clone();
    this._iKey = iKey;
    const oKeyWords = oKey.words;
    const iKeyWords = iKey.words;
    for (let i = 0; i < hasherBlockSize; i += 1) {
      oKeyWords[i] ^= 1549556828;
      iKeyWords[i] ^= 909522486;
    }
    oKey.sigBytes = hasherBlockSizeBytes;
    iKey.sigBytes = hasherBlockSizeBytes;
    this.reset();
  }
  /**
   * Resets this HMAC to its initial state.
   *
   * @example
   *
   *     hmacHasher.reset();
   */
  reset() {
    const hasher = this._hasher;
    hasher.reset();
    hasher.update(this._iKey);
  }
  /**
   * Updates this HMAC with a message.
   *
   * @param {WordArray|string} messageUpdate The message to append.
   *
   * @return {HMAC} This HMAC instance.
   *
   * @example
   *
   *     hmacHasher.update('message');
   *     hmacHasher.update(wordArray);
   */
  update(messageUpdate) {
    this._hasher.update(messageUpdate);
    return this;
  }
  /**
   * Finalizes the HMAC computation.
   * Note that the finalize operation is effectively a destructive, read-once operation.
   *
   * @param {WordArray|string} messageUpdate (Optional) A final message update.
   *
   * @return {WordArray} The HMAC.
   *
   * @example
   *
   *     var hmac = hmacHasher.finalize();
   *     var hmac = hmacHasher.finalize('message');
   *     var hmac = hmacHasher.finalize(wordArray);
   */
  finalize(messageUpdate) {
    const hasher = this._hasher;
    const innerHash = hasher.finalize(messageUpdate);
    hasher.reset();
    const hmac = hasher.finalize(this._oKey.clone().concat(innerHash));
    return hmac;
  }
}
const X32WordArray = WordArray;
class X64Word extends Base {
  /**
   * Initializes a newly created 64-bit word.
   *
   * @param {number} high The high 32 bits.
   * @param {number} low The low 32 bits.
   *
   * @example
   *
   *     var x64Word = CryptoJS.x64.Word.create(0x00010203, 0x04050607);
   */
  constructor(high, low) {
    super();
    this.high = high;
    this.low = low;
  }
}
class X64WordArray extends Base {
  /**
   * Initializes a newly created word array.
   *
   * @param {Array} words (Optional) An array of CryptoJS.x64.Word objects.
   * @param {number} sigBytes (Optional) The number of significant bytes in the words.
   *
   * @example
   *
   *     var wordArray = CryptoJS.x64.WordArray.create();
   *
   *     var wordArray = CryptoJS.x64.WordArray.create([
   *         CryptoJS.x64.Word.create(0x00010203, 0x04050607),
   *         CryptoJS.x64.Word.create(0x18191a1b, 0x1c1d1e1f)
   *     ]);
   *
   *     var wordArray = CryptoJS.x64.WordArray.create([
   *         CryptoJS.x64.Word.create(0x00010203, 0x04050607),
   *         CryptoJS.x64.Word.create(0x18191a1b, 0x1c1d1e1f)
   *     ], 10);
   */
  constructor(words = [], sigBytes = words.length * 8) {
    super();
    this.words = words;
    this.sigBytes = sigBytes;
  }
  /**
   * Converts this 64-bit word array to a 32-bit word array.
   *
   * @return {CryptoJS.lib.WordArray} This word array's data as a 32-bit word array.
   *
   * @example
   *
   *     var x32WordArray = x64WordArray.toX32();
   */
  toX32() {
    const x64Words = this.words;
    const x64WordsLength = x64Words.length;
    const x32Words = [];
    for (let i = 0; i < x64WordsLength; i += 1) {
      const x64Word = x64Words[i];
      x32Words.push(x64Word.high);
      x32Words.push(x64Word.low);
    }
    return X32WordArray.create(x32Words, this.sigBytes);
  }
  /**
   * Creates a copy of this word array.
   *
   * @return {X64WordArray} The clone.
   *
   * @example
   *
   *     var clone = x64WordArray.clone();
   */
  clone() {
    const clone = super.clone.call(this);
    clone.words = this.words.slice(0);
    const { words } = clone;
    const wordsLength = words.length;
    for (let i = 0; i < wordsLength; i += 1) {
      words[i] = words[i].clone();
    }
    return clone;
  }
}
const parseLoop = (base64Str, base64StrLength, reverseMap) => {
  const words = [];
  let nBytes = 0;
  for (let i = 0; i < base64StrLength; i += 1) {
    if (i % 4) {
      const bits1 = reverseMap[base64Str.charCodeAt(i - 1)] << i % 4 * 2;
      const bits2 = reverseMap[base64Str.charCodeAt(i)] >>> 6 - i % 4 * 2;
      const bitsCombined = bits1 | bits2;
      words[nBytes >>> 2] |= bitsCombined << 24 - nBytes % 4 * 8;
      nBytes += 1;
    }
  }
  return WordArray.create(words, nBytes);
};
const Base64 = {
  /**
   * Converts a word array to a Base64 string.
   *
   * @param {WordArray} wordArray The word array.
   *
   * @return {string} The Base64 string.
   *
   * @static
   *
   * @example
   *
   *     const base64String = CryptoJS.enc.Base64.stringify(wordArray);
   */
  stringify(wordArray) {
    const { words, sigBytes } = wordArray;
    const map2 = this._map;
    wordArray.clamp();
    const base64Chars = [];
    for (let i = 0; i < sigBytes; i += 3) {
      const byte1 = words[i >>> 2] >>> 24 - i % 4 * 8 & 255;
      const byte2 = words[i + 1 >>> 2] >>> 24 - (i + 1) % 4 * 8 & 255;
      const byte3 = words[i + 2 >>> 2] >>> 24 - (i + 2) % 4 * 8 & 255;
      const triplet = byte1 << 16 | byte2 << 8 | byte3;
      for (let j = 0; j < 4 && i + j * 0.75 < sigBytes; j += 1) {
        base64Chars.push(map2.charAt(triplet >>> 6 * (3 - j) & 63));
      }
    }
    const paddingChar = map2.charAt(64);
    if (paddingChar) {
      while (base64Chars.length % 4) {
        base64Chars.push(paddingChar);
      }
    }
    return base64Chars.join("");
  },
  /**
   * Converts a Base64 string to a word array.
   *
   * @param {string} base64Str The Base64 string.
   *
   * @return {WordArray} The word array.
   *
   * @static
   *
   * @example
   *
   *     const wordArray = CryptoJS.enc.Base64.parse(base64String);
   */
  parse(base64Str) {
    let base64StrLength = base64Str.length;
    const map2 = this._map;
    let reverseMap = this._reverseMap;
    if (!reverseMap) {
      this._reverseMap = [];
      reverseMap = this._reverseMap;
      for (let j = 0; j < map2.length; j += 1) {
        reverseMap[map2.charCodeAt(j)] = j;
      }
    }
    const paddingChar = map2.charAt(64);
    if (paddingChar) {
      const paddingIndex = base64Str.indexOf(paddingChar);
      if (paddingIndex !== -1) {
        base64StrLength = paddingIndex;
      }
    }
    return parseLoop(base64Str, base64StrLength, reverseMap);
  },
  _map: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="
};
const T$1 = [];
for (let i = 0; i < 64; i += 1) {
  T$1[i] = Math.abs(Math.sin(i + 1)) * 4294967296 | 0;
}
const FF = (a, b, c, d2, x2, s, t) => {
  const n2 = a + (b & c | ~b & d2) + x2 + t;
  return (n2 << s | n2 >>> 32 - s) + b;
};
const GG = (a, b, c, d2, x2, s, t) => {
  const n2 = a + (b & d2 | c & ~d2) + x2 + t;
  return (n2 << s | n2 >>> 32 - s) + b;
};
const HH = (a, b, c, d2, x2, s, t) => {
  const n2 = a + (b ^ c ^ d2) + x2 + t;
  return (n2 << s | n2 >>> 32 - s) + b;
};
const II = (a, b, c, d2, x2, s, t) => {
  const n2 = a + (c ^ (b | ~d2)) + x2 + t;
  return (n2 << s | n2 >>> 32 - s) + b;
};
class MD5Algo extends Hasher {
  _doReset() {
    this._hash = new WordArray([
      1732584193,
      4023233417,
      2562383102,
      271733878
    ]);
  }
  _doProcessBlock(M, offset) {
    const _M = M;
    for (let i = 0; i < 16; i += 1) {
      const offset_i = offset + i;
      const M_offset_i = M[offset_i];
      _M[offset_i] = (M_offset_i << 8 | M_offset_i >>> 24) & 16711935 | (M_offset_i << 24 | M_offset_i >>> 8) & 4278255360;
    }
    const H2 = this._hash.words;
    const M_offset_0 = _M[offset + 0];
    const M_offset_1 = _M[offset + 1];
    const M_offset_2 = _M[offset + 2];
    const M_offset_3 = _M[offset + 3];
    const M_offset_4 = _M[offset + 4];
    const M_offset_5 = _M[offset + 5];
    const M_offset_6 = _M[offset + 6];
    const M_offset_7 = _M[offset + 7];
    const M_offset_8 = _M[offset + 8];
    const M_offset_9 = _M[offset + 9];
    const M_offset_10 = _M[offset + 10];
    const M_offset_11 = _M[offset + 11];
    const M_offset_12 = _M[offset + 12];
    const M_offset_13 = _M[offset + 13];
    const M_offset_14 = _M[offset + 14];
    const M_offset_15 = _M[offset + 15];
    let a = H2[0];
    let b = H2[1];
    let c = H2[2];
    let d2 = H2[3];
    a = FF(a, b, c, d2, M_offset_0, 7, T$1[0]);
    d2 = FF(d2, a, b, c, M_offset_1, 12, T$1[1]);
    c = FF(c, d2, a, b, M_offset_2, 17, T$1[2]);
    b = FF(b, c, d2, a, M_offset_3, 22, T$1[3]);
    a = FF(a, b, c, d2, M_offset_4, 7, T$1[4]);
    d2 = FF(d2, a, b, c, M_offset_5, 12, T$1[5]);
    c = FF(c, d2, a, b, M_offset_6, 17, T$1[6]);
    b = FF(b, c, d2, a, M_offset_7, 22, T$1[7]);
    a = FF(a, b, c, d2, M_offset_8, 7, T$1[8]);
    d2 = FF(d2, a, b, c, M_offset_9, 12, T$1[9]);
    c = FF(c, d2, a, b, M_offset_10, 17, T$1[10]);
    b = FF(b, c, d2, a, M_offset_11, 22, T$1[11]);
    a = FF(a, b, c, d2, M_offset_12, 7, T$1[12]);
    d2 = FF(d2, a, b, c, M_offset_13, 12, T$1[13]);
    c = FF(c, d2, a, b, M_offset_14, 17, T$1[14]);
    b = FF(b, c, d2, a, M_offset_15, 22, T$1[15]);
    a = GG(a, b, c, d2, M_offset_1, 5, T$1[16]);
    d2 = GG(d2, a, b, c, M_offset_6, 9, T$1[17]);
    c = GG(c, d2, a, b, M_offset_11, 14, T$1[18]);
    b = GG(b, c, d2, a, M_offset_0, 20, T$1[19]);
    a = GG(a, b, c, d2, M_offset_5, 5, T$1[20]);
    d2 = GG(d2, a, b, c, M_offset_10, 9, T$1[21]);
    c = GG(c, d2, a, b, M_offset_15, 14, T$1[22]);
    b = GG(b, c, d2, a, M_offset_4, 20, T$1[23]);
    a = GG(a, b, c, d2, M_offset_9, 5, T$1[24]);
    d2 = GG(d2, a, b, c, M_offset_14, 9, T$1[25]);
    c = GG(c, d2, a, b, M_offset_3, 14, T$1[26]);
    b = GG(b, c, d2, a, M_offset_8, 20, T$1[27]);
    a = GG(a, b, c, d2, M_offset_13, 5, T$1[28]);
    d2 = GG(d2, a, b, c, M_offset_2, 9, T$1[29]);
    c = GG(c, d2, a, b, M_offset_7, 14, T$1[30]);
    b = GG(b, c, d2, a, M_offset_12, 20, T$1[31]);
    a = HH(a, b, c, d2, M_offset_5, 4, T$1[32]);
    d2 = HH(d2, a, b, c, M_offset_8, 11, T$1[33]);
    c = HH(c, d2, a, b, M_offset_11, 16, T$1[34]);
    b = HH(b, c, d2, a, M_offset_14, 23, T$1[35]);
    a = HH(a, b, c, d2, M_offset_1, 4, T$1[36]);
    d2 = HH(d2, a, b, c, M_offset_4, 11, T$1[37]);
    c = HH(c, d2, a, b, M_offset_7, 16, T$1[38]);
    b = HH(b, c, d2, a, M_offset_10, 23, T$1[39]);
    a = HH(a, b, c, d2, M_offset_13, 4, T$1[40]);
    d2 = HH(d2, a, b, c, M_offset_0, 11, T$1[41]);
    c = HH(c, d2, a, b, M_offset_3, 16, T$1[42]);
    b = HH(b, c, d2, a, M_offset_6, 23, T$1[43]);
    a = HH(a, b, c, d2, M_offset_9, 4, T$1[44]);
    d2 = HH(d2, a, b, c, M_offset_12, 11, T$1[45]);
    c = HH(c, d2, a, b, M_offset_15, 16, T$1[46]);
    b = HH(b, c, d2, a, M_offset_2, 23, T$1[47]);
    a = II(a, b, c, d2, M_offset_0, 6, T$1[48]);
    d2 = II(d2, a, b, c, M_offset_7, 10, T$1[49]);
    c = II(c, d2, a, b, M_offset_14, 15, T$1[50]);
    b = II(b, c, d2, a, M_offset_5, 21, T$1[51]);
    a = II(a, b, c, d2, M_offset_12, 6, T$1[52]);
    d2 = II(d2, a, b, c, M_offset_3, 10, T$1[53]);
    c = II(c, d2, a, b, M_offset_10, 15, T$1[54]);
    b = II(b, c, d2, a, M_offset_1, 21, T$1[55]);
    a = II(a, b, c, d2, M_offset_8, 6, T$1[56]);
    d2 = II(d2, a, b, c, M_offset_15, 10, T$1[57]);
    c = II(c, d2, a, b, M_offset_6, 15, T$1[58]);
    b = II(b, c, d2, a, M_offset_13, 21, T$1[59]);
    a = II(a, b, c, d2, M_offset_4, 6, T$1[60]);
    d2 = II(d2, a, b, c, M_offset_11, 10, T$1[61]);
    c = II(c, d2, a, b, M_offset_2, 15, T$1[62]);
    b = II(b, c, d2, a, M_offset_9, 21, T$1[63]);
    H2[0] = H2[0] + a | 0;
    H2[1] = H2[1] + b | 0;
    H2[2] = H2[2] + c | 0;
    H2[3] = H2[3] + d2 | 0;
  }
  /* eslint-ensable no-param-reassign */
  _doFinalize() {
    const data = this._data;
    const dataWords = data.words;
    const nBitsTotal = this._nDataBytes * 8;
    const nBitsLeft = data.sigBytes * 8;
    dataWords[nBitsLeft >>> 5] |= 128 << 24 - nBitsLeft % 32;
    const nBitsTotalH = Math.floor(nBitsTotal / 4294967296);
    const nBitsTotalL = nBitsTotal;
    dataWords[(nBitsLeft + 64 >>> 9 << 4) + 15] = (nBitsTotalH << 8 | nBitsTotalH >>> 24) & 16711935 | (nBitsTotalH << 24 | nBitsTotalH >>> 8) & 4278255360;
    dataWords[(nBitsLeft + 64 >>> 9 << 4) + 14] = (nBitsTotalL << 8 | nBitsTotalL >>> 24) & 16711935 | (nBitsTotalL << 24 | nBitsTotalL >>> 8) & 4278255360;
    data.sigBytes = (dataWords.length + 1) * 4;
    this._process();
    const hash = this._hash;
    const H2 = hash.words;
    for (let i = 0; i < 4; i += 1) {
      const H_i = H2[i];
      H2[i] = (H_i << 8 | H_i >>> 24) & 16711935 | (H_i << 24 | H_i >>> 8) & 4278255360;
    }
    return hash;
  }
  clone() {
    const clone = super.clone.call(this);
    clone._hash = this._hash.clone();
    return clone;
  }
}
const MD5 = Hasher._createHelper(MD5Algo);
const HmacMD5 = Hasher._createHmacHelper(MD5Algo);
class EvpKDFAlgo extends Base {
  /**
   * Initializes a newly created key derivation function.
   *
   * @param {Object} cfg (Optional) The configuration options to use for the derivation.
   *
   * @example
   *
   *     const kdf = CryptoJS.algo.EvpKDF.create();
   *     const kdf = CryptoJS.algo.EvpKDF.create({ keySize: 8 });
   *     const kdf = CryptoJS.algo.EvpKDF.create({ keySize: 8, iterations: 1000 });
   */
  constructor(cfg) {
    super();
    this.cfg = Object.assign(
      new Base(),
      {
        keySize: 128 / 32,
        hasher: MD5Algo,
        iterations: 1
      },
      cfg
    );
  }
  /**
   * Derives a key from a password.
   *
   * @param {WordArray|string} password The password.
   * @param {WordArray|string} salt A salt.
   *
   * @return {WordArray} The derived key.
   *
   * @example
   *
   *     const key = kdf.compute(password, salt);
   */
  compute(password, salt) {
    let block;
    const { cfg } = this;
    const hasher = cfg.hasher.create();
    const derivedKey = WordArray.create();
    const derivedKeyWords = derivedKey.words;
    const { keySize, iterations } = cfg;
    while (derivedKeyWords.length < keySize) {
      if (block) {
        hasher.update(block);
      }
      block = hasher.update(password).finalize(salt);
      hasher.reset();
      for (let i = 1; i < iterations; i += 1) {
        block = hasher.finalize(block);
        hasher.reset();
      }
      derivedKey.concat(block);
    }
    derivedKey.sigBytes = keySize * 4;
    return derivedKey;
  }
}
const EvpKDF = (password, salt, cfg) => EvpKDFAlgo.create(cfg).compute(password, salt);
class Cipher extends BufferedBlockAlgorithm {
  /**
   * Initializes a newly created cipher.
   *
   * @param {number} xformMode Either the encryption or decryption transormation mode constant.
   * @param {WordArray} key The key.
   * @param {Object} cfg (Optional) The configuration options to use for this operation.
   *
   * @example
   *
   *     const cipher = CryptoJS.algo.AES.create(
   *       CryptoJS.algo.AES._ENC_XFORM_MODE, keyWordArray, { iv: ivWordArray }
   *     );
   */
  constructor(xformMode, key, cfg) {
    super();
    this.cfg = Object.assign(new Base(), cfg);
    this._xformMode = xformMode;
    this._key = key;
    this.reset();
  }
  /**
   * Creates this cipher in encryption mode.
   *
   * @param {WordArray} key The key.
   * @param {Object} cfg (Optional) The configuration options to use for this operation.
   *
   * @return {Cipher} A cipher instance.
   *
   * @static
   *
   * @example
   *
   *     const cipher = CryptoJS.algo.AES.createEncryptor(keyWordArray, { iv: ivWordArray });
   */
  static createEncryptor(key, cfg) {
    return this.create(this._ENC_XFORM_MODE, key, cfg);
  }
  /**
   * Creates this cipher in decryption mode.
   *
   * @param {WordArray} key The key.
   * @param {Object} cfg (Optional) The configuration options to use for this operation.
   *
   * @return {Cipher} A cipher instance.
   *
   * @static
   *
   * @example
   *
   *     const cipher = CryptoJS.algo.AES.createDecryptor(keyWordArray, { iv: ivWordArray });
   */
  static createDecryptor(key, cfg) {
    return this.create(this._DEC_XFORM_MODE, key, cfg);
  }
  /**
   * Creates shortcut functions to a cipher's object interface.
   *
   * @param {Cipher} cipher The cipher to create a helper for.
   *
   * @return {Object} An object with encrypt and decrypt shortcut functions.
   *
   * @static
   *
   * @example
   *
   *     const AES = CryptoJS.lib.Cipher._createHelper(CryptoJS.algo.AES);
   */
  static _createHelper(SubCipher) {
    const selectCipherStrategy = (key) => {
      if (typeof key === "string") {
        return PasswordBasedCipher;
      }
      return SerializableCipher;
    };
    return {
      encrypt(message, key, cfg) {
        return selectCipherStrategy(key).encrypt(SubCipher, message, key, cfg);
      },
      decrypt(ciphertext, key, cfg) {
        return selectCipherStrategy(key).decrypt(SubCipher, ciphertext, key, cfg);
      }
    };
  }
  /**
   * Resets this cipher to its initial state.
   *
   * @example
   *
   *     cipher.reset();
   */
  reset() {
    super.reset.call(this);
    this._doReset();
  }
  /**
   * Adds data to be encrypted or decrypted.
   *
   * @param {WordArray|string} dataUpdate The data to encrypt or decrypt.
   *
   * @return {WordArray} The data after processing.
   *
   * @example
   *
   *     const encrypted = cipher.process('data');
   *     const encrypted = cipher.process(wordArray);
   */
  process(dataUpdate) {
    this._append(dataUpdate);
    return this._process();
  }
  /**
   * Finalizes the encryption or decryption process.
   * Note that the finalize operation is effectively a destructive, read-once operation.
   *
   * @param {WordArray|string} dataUpdate The final data to encrypt or decrypt.
   *
   * @return {WordArray} The data after final processing.
   *
   * @example
   *
   *     const encrypted = cipher.finalize();
   *     const encrypted = cipher.finalize('data');
   *     const encrypted = cipher.finalize(wordArray);
   */
  finalize(dataUpdate) {
    if (dataUpdate) {
      this._append(dataUpdate);
    }
    const finalProcessedData = this._doFinalize();
    return finalProcessedData;
  }
}
Cipher._ENC_XFORM_MODE = 1;
Cipher._DEC_XFORM_MODE = 2;
Cipher.keySize = 128 / 32;
Cipher.ivSize = 128 / 32;
class StreamCipher extends Cipher {
  constructor(...args) {
    super(...args);
    this.blockSize = 1;
  }
  _doFinalize() {
    const finalProcessedBlocks = this._process(true);
    return finalProcessedBlocks;
  }
}
class BlockCipherMode extends Base {
  /**
   * Initializes a newly created mode.
   *
   * @param {Cipher} cipher A block cipher instance.
   * @param {Array} iv The IV words.
   *
   * @example
   *
   *     const mode = CryptoJS.mode.CBC.Encryptor.create(cipher, iv.words);
   */
  constructor(cipher, iv) {
    super();
    this._cipher = cipher;
    this._iv = iv;
  }
  /**
   * Creates this mode for encryption.
   *
   * @param {Cipher} cipher A block cipher instance.
   * @param {Array} iv The IV words.
   *
   * @static
   *
   * @example
   *
   *     const mode = CryptoJS.mode.CBC.createEncryptor(cipher, iv.words);
   */
  static createEncryptor(cipher, iv) {
    return this.Encryptor.create(cipher, iv);
  }
  /**
   * Creates this mode for decryption.
   *
   * @param {Cipher} cipher A block cipher instance.
   * @param {Array} iv The IV words.
   *
   * @static
   *
   * @example
   *
   *     const mode = CryptoJS.mode.CBC.createDecryptor(cipher, iv.words);
   */
  static createDecryptor(cipher, iv) {
    return this.Decryptor.create(cipher, iv);
  }
}
function xorBlock(words, offset, blockSize) {
  const _words = words;
  let block;
  const iv = this._iv;
  if (iv) {
    block = iv;
    this._iv = void 0;
  } else {
    block = this._prevBlock;
  }
  for (let i = 0; i < blockSize; i += 1) {
    _words[offset + i] ^= block[i];
  }
}
class CBC extends BlockCipherMode {
}
CBC.Encryptor = class extends CBC {
  /**
   * Processes the data block at offset.
   *
   * @param {Array} words The data words to operate on.
   * @param {number} offset The offset where the block starts.
   *
   * @example
   *
   *     mode.processBlock(data.words, offset);
   */
  processBlock(words, offset) {
    const cipher = this._cipher;
    const { blockSize } = cipher;
    xorBlock.call(this, words, offset, blockSize);
    cipher.encryptBlock(words, offset);
    this._prevBlock = words.slice(offset, offset + blockSize);
  }
};
CBC.Decryptor = class extends CBC {
  /**
   * Processes the data block at offset.
   *
   * @param {Array} words The data words to operate on.
   * @param {number} offset The offset where the block starts.
   *
   * @example
   *
   *     mode.processBlock(data.words, offset);
   */
  processBlock(words, offset) {
    const cipher = this._cipher;
    const { blockSize } = cipher;
    const thisBlock = words.slice(offset, offset + blockSize);
    cipher.decryptBlock(words, offset);
    xorBlock.call(this, words, offset, blockSize);
    this._prevBlock = thisBlock;
  }
};
const Pkcs7 = {
  /**
   * Pads data using the algorithm defined in PKCS #5/7.
   *
   * @param {WordArray} data The data to pad.
   * @param {number} blockSize The multiple that the data should be padded to.
   *
   * @static
   *
   * @example
   *
   *     CryptoJS.pad.Pkcs7.pad(wordArray, 4);
   */
  pad(data, blockSize) {
    const blockSizeBytes = blockSize * 4;
    const nPaddingBytes = blockSizeBytes - data.sigBytes % blockSizeBytes;
    const paddingWord = nPaddingBytes << 24 | nPaddingBytes << 16 | nPaddingBytes << 8 | nPaddingBytes;
    const paddingWords = [];
    for (let i = 0; i < nPaddingBytes; i += 4) {
      paddingWords.push(paddingWord);
    }
    const padding = WordArray.create(paddingWords, nPaddingBytes);
    data.concat(padding);
  },
  /**
   * Unpads data that had been padded using the algorithm defined in PKCS #5/7.
   *
   * @param {WordArray} data The data to unpad.
   *
   * @static
   *
   * @example
   *
   *     CryptoJS.pad.Pkcs7.unpad(wordArray);
   */
  unpad(data) {
    const _data = data;
    const nPaddingBytes = _data.words[_data.sigBytes - 1 >>> 2] & 255;
    _data.sigBytes -= nPaddingBytes;
  }
};
class BlockCipher extends Cipher {
  constructor(xformMode, key, cfg) {
    super(xformMode, key, Object.assign(
      {
        mode: CBC,
        padding: Pkcs7
      },
      cfg
    ));
    this.blockSize = 128 / 32;
  }
  reset() {
    let modeCreator;
    super.reset.call(this);
    const { cfg } = this;
    const { iv, mode } = cfg;
    if (this._xformMode === this.constructor._ENC_XFORM_MODE) {
      modeCreator = mode.createEncryptor;
    } else {
      modeCreator = mode.createDecryptor;
      this._minBufferSize = 1;
    }
    this._mode = modeCreator.call(mode, this, iv && iv.words);
    this._mode.__creator = modeCreator;
  }
  _doProcessBlock(words, offset) {
    this._mode.processBlock(words, offset);
  }
  _doFinalize() {
    let finalProcessedBlocks;
    const { padding } = this.cfg;
    if (this._xformMode === this.constructor._ENC_XFORM_MODE) {
      padding.pad(this._data, this.blockSize);
      finalProcessedBlocks = this._process(true);
    } else {
      finalProcessedBlocks = this._process(true);
      padding.unpad(finalProcessedBlocks);
    }
    return finalProcessedBlocks;
  }
}
class CipherParams extends Base {
  /**
   * Initializes a newly created cipher params object.
   *
   * @param {Object} cipherParams An object with any of the possible cipher parameters.
   *
   * @example
   *
   *     var cipherParams = CryptoJS.lib.CipherParams.create({
   *         ciphertext: ciphertextWordArray,
   *         key: keyWordArray,
   *         iv: ivWordArray,
   *         salt: saltWordArray,
   *         algorithm: CryptoJS.algo.AES,
   *         mode: CryptoJS.mode.CBC,
   *         padding: CryptoJS.pad.PKCS7,
   *         blockSize: 4,
   *         formatter: CryptoJS.format.OpenSSL
   *     });
   */
  constructor(cipherParams) {
    super();
    this.mixIn(cipherParams);
  }
  /**
   * Converts this cipher params object to a string.
   *
   * @param {Format} formatter (Optional) The formatting strategy to use.
   *
   * @return {string} The stringified cipher params.
   *
   * @throws Error If neither the formatter nor the default formatter is set.
   *
   * @example
   *
   *     var string = cipherParams + '';
   *     var string = cipherParams.toString();
   *     var string = cipherParams.toString(CryptoJS.format.OpenSSL);
   */
  toString(formatter) {
    return (formatter || this.formatter).stringify(this);
  }
}
const OpenSSLFormatter = {
  /**
   * Converts a cipher params object to an OpenSSL-compatible string.
   *
   * @param {CipherParams} cipherParams The cipher params object.
   *
   * @return {string} The OpenSSL-compatible string.
   *
   * @static
   *
   * @example
   *
   *     var openSSLString = CryptoJS.format.OpenSSL.stringify(cipherParams);
   */
  stringify(cipherParams) {
    let wordArray;
    const { ciphertext, salt } = cipherParams;
    if (salt) {
      wordArray = WordArray.create([1398893684, 1701076831]).concat(salt).concat(ciphertext);
    } else {
      wordArray = ciphertext;
    }
    return wordArray.toString(Base64);
  },
  /**
   * Converts an OpenSSL-compatible string to a cipher params object.
   *
   * @param {string} openSSLStr The OpenSSL-compatible string.
   *
   * @return {CipherParams} The cipher params object.
   *
   * @static
   *
   * @example
   *
   *     var cipherParams = CryptoJS.format.OpenSSL.parse(openSSLString);
   */
  parse(openSSLStr) {
    let salt;
    const ciphertext = Base64.parse(openSSLStr);
    const ciphertextWords = ciphertext.words;
    if (ciphertextWords[0] === 1398893684 && ciphertextWords[1] === 1701076831) {
      salt = WordArray.create(ciphertextWords.slice(2, 4));
      ciphertextWords.splice(0, 4);
      ciphertext.sigBytes -= 16;
    }
    return CipherParams.create({ ciphertext, salt });
  }
};
class SerializableCipher extends Base {
  /**
   * Encrypts a message.
   *
   * @param {Cipher} cipher The cipher algorithm to use.
   * @param {WordArray|string} message The message to encrypt.
   * @param {WordArray} key The key.
   * @param {Object} cfg (Optional) The configuration options to use for this operation.
   *
   * @return {CipherParams} A cipher params object.
   *
   * @static
   *
   * @example
   *
   *     var ciphertextParams = CryptoJS.lib.SerializableCipher
   *       .encrypt(CryptoJS.algo.AES, message, key);
   *     var ciphertextParams = CryptoJS.lib.SerializableCipher
   *       .encrypt(CryptoJS.algo.AES, message, key, { iv: iv });
   *     var ciphertextParams = CryptoJS.lib.SerializableCipher
   *       .encrypt(CryptoJS.algo.AES, message, key, { iv: iv, format: CryptoJS.format.OpenSSL });
   */
  static encrypt(cipher, message, key, cfg) {
    const _cfg = Object.assign(new Base(), this.cfg, cfg);
    const encryptor = cipher.createEncryptor(key, _cfg);
    const ciphertext = encryptor.finalize(message);
    const cipherCfg = encryptor.cfg;
    return CipherParams.create({
      ciphertext,
      key,
      iv: cipherCfg.iv,
      algorithm: cipher,
      mode: cipherCfg.mode,
      padding: cipherCfg.padding,
      blockSize: encryptor.blockSize,
      formatter: _cfg.format
    });
  }
  /**
   * Decrypts serialized ciphertext.
   *
   * @param {Cipher} cipher The cipher algorithm to use.
   * @param {CipherParams|string} ciphertext The ciphertext to decrypt.
   * @param {WordArray} key The key.
   * @param {Object} cfg (Optional) The configuration options to use for this operation.
   *
   * @return {WordArray} The plaintext.
   *
   * @static
   *
   * @example
   *
   *     var plaintext = CryptoJS.lib.SerializableCipher
   *       .decrypt(CryptoJS.algo.AES, formattedCiphertext, key,
   *         { iv: iv, format: CryptoJS.format.OpenSSL });
   *     var plaintext = CryptoJS.lib.SerializableCipher
   *       .decrypt(CryptoJS.algo.AES, ciphertextParams, key,
   *         { iv: iv, format: CryptoJS.format.OpenSSL });
   */
  static decrypt(cipher, ciphertext, key, cfg) {
    let _ciphertext = ciphertext;
    const _cfg = Object.assign(new Base(), this.cfg, cfg);
    _ciphertext = this._parse(_ciphertext, _cfg.format);
    const plaintext = cipher.createDecryptor(key, _cfg).finalize(_ciphertext.ciphertext);
    return plaintext;
  }
  /**
   * Converts serialized ciphertext to CipherParams,
   * else assumed CipherParams already and returns ciphertext unchanged.
   *
   * @param {CipherParams|string} ciphertext The ciphertext.
   * @param {Formatter} format The formatting strategy to use to parse serialized ciphertext.
   *
   * @return {CipherParams} The unserialized ciphertext.
   *
   * @static
   *
   * @example
   *
   *     var ciphertextParams = CryptoJS.lib.SerializableCipher
   *       ._parse(ciphertextStringOrParams, format);
   */
  static _parse(ciphertext, format) {
    if (typeof ciphertext === "string") {
      return format.parse(ciphertext, this);
    }
    return ciphertext;
  }
}
SerializableCipher.cfg = Object.assign(
  new Base(),
  { format: OpenSSLFormatter }
);
const OpenSSLKdf = {
  /**
   * Derives a key and IV from a password.
   *
   * @param {string} password The password to derive from.
   * @param {number} keySize The size in words of the key to generate.
   * @param {number} ivSize The size in words of the IV to generate.
   * @param {WordArray|string} salt
   *     (Optional) A 64-bit salt to use. If omitted, a salt will be generated randomly.
   *
   * @return {CipherParams} A cipher params object with the key, IV, and salt.
   *
   * @static
   *
   * @example
   *
   *     var derivedParams = CryptoJS.kdf.OpenSSL.execute('Password', 256/32, 128/32);
   *     var derivedParams = CryptoJS.kdf.OpenSSL.execute('Password', 256/32, 128/32, 'saltsalt');
   */
  execute(password, keySize, ivSize, salt) {
    let _salt = salt;
    if (!_salt) {
      _salt = WordArray.random(64 / 8);
    }
    const key = EvpKDFAlgo.create({ keySize: keySize + ivSize }).compute(password, _salt);
    const iv = WordArray.create(key.words.slice(keySize), ivSize * 4);
    key.sigBytes = keySize * 4;
    return CipherParams.create({ key, iv, salt: _salt });
  }
};
class PasswordBasedCipher extends SerializableCipher {
  /**
   * Encrypts a message using a password.
   *
   * @param {Cipher} cipher The cipher algorithm to use.
   * @param {WordArray|string} message The message to encrypt.
   * @param {string} password The password.
   * @param {Object} cfg (Optional) The configuration options to use for this operation.
   *
   * @return {CipherParams} A cipher params object.
   *
   * @static
   *
   * @example
   *
   *     var ciphertextParams = CryptoJS.lib.PasswordBasedCipher
   *       .encrypt(CryptoJS.algo.AES, message, 'password');
   *     var ciphertextParams = CryptoJS.lib.PasswordBasedCipher
   *       .encrypt(CryptoJS.algo.AES, message, 'password', { format: CryptoJS.format.OpenSSL });
   */
  static encrypt(cipher, message, password, cfg) {
    const _cfg = Object.assign(new Base(), this.cfg, cfg);
    const derivedParams = _cfg.kdf.execute(password, cipher.keySize, cipher.ivSize);
    _cfg.iv = derivedParams.iv;
    const ciphertext = SerializableCipher.encrypt.call(this, cipher, message, derivedParams.key, _cfg);
    ciphertext.mixIn(derivedParams);
    return ciphertext;
  }
  /**
   * Decrypts serialized ciphertext using a password.
   *
   * @param {Cipher} cipher The cipher algorithm to use.
   * @param {CipherParams|string} ciphertext The ciphertext to decrypt.
   * @param {string} password The password.
   * @param {Object} cfg (Optional) The configuration options to use for this operation.
   *
   * @return {WordArray} The plaintext.
   *
   * @static
   *
   * @example
   *
   *     var plaintext = CryptoJS.lib.PasswordBasedCipher
   *       .decrypt(CryptoJS.algo.AES, formattedCiphertext, 'password',
   *         { format: CryptoJS.format.OpenSSL });
   *     var plaintext = CryptoJS.lib.PasswordBasedCipher
   *       .decrypt(CryptoJS.algo.AES, ciphertextParams, 'password',
   *         { format: CryptoJS.format.OpenSSL });
   */
  static decrypt(cipher, ciphertext, password, cfg) {
    let _ciphertext = ciphertext;
    const _cfg = Object.assign(new Base(), this.cfg, cfg);
    _ciphertext = this._parse(_ciphertext, _cfg.format);
    const derivedParams = _cfg.kdf.execute(password, cipher.keySize, cipher.ivSize, _ciphertext.salt);
    _cfg.iv = derivedParams.iv;
    const plaintext = SerializableCipher.decrypt.call(this, cipher, _ciphertext, derivedParams.key, _cfg);
    return plaintext;
  }
}
PasswordBasedCipher.cfg = Object.assign(SerializableCipher.cfg, { kdf: OpenSSLKdf });
const swapEndian = (word) => word << 8 & 4278255360 | word >>> 8 & 16711935;
const Utf16BE = {
  /**
   * Converts a word array to a UTF-16 BE string.
   *
   * @param {WordArray} wordArray The word array.
   *
   * @return {string} The UTF-16 BE string.
   *
   * @static
   *
   * @example
   *
   *     const utf16String = CryptoJS.enc.Utf16.stringify(wordArray);
   */
  stringify(wordArray) {
    const { words, sigBytes } = wordArray;
    const utf16Chars = [];
    for (let i = 0; i < sigBytes; i += 2) {
      const codePoint = words[i >>> 2] >>> 16 - i % 4 * 8 & 65535;
      utf16Chars.push(String.fromCharCode(codePoint));
    }
    return utf16Chars.join("");
  },
  /**
   * Converts a UTF-16 BE string to a word array.
   *
   * @param {string} utf16Str The UTF-16 BE string.
   *
   * @return {WordArray} The word array.
   *
   * @static
   *
   * @example
   *
   *     const wordArray = CryptoJS.enc.Utf16.parse(utf16String);
   */
  parse(utf16Str) {
    const utf16StrLength = utf16Str.length;
    const words = [];
    for (let i = 0; i < utf16StrLength; i += 1) {
      words[i >>> 1] |= utf16Str.charCodeAt(i) << 16 - i % 2 * 16;
    }
    return WordArray.create(words, utf16StrLength * 2);
  }
};
const Utf16 = Utf16BE;
const Utf16LE = {
  /**
   * Converts a word array to a UTF-16 LE string.
   *
   * @param {WordArray} wordArray The word array.
   *
   * @return {string} The UTF-16 LE string.
   *
   * @static
   *
   * @example
   *
   *     const utf16Str = CryptoJS.enc.Utf16LE.stringify(wordArray);
   */
  stringify(wordArray) {
    const { words, sigBytes } = wordArray;
    const utf16Chars = [];
    for (let i = 0; i < sigBytes; i += 2) {
      const codePoint = swapEndian(words[i >>> 2] >>> 16 - i % 4 * 8 & 65535);
      utf16Chars.push(String.fromCharCode(codePoint));
    }
    return utf16Chars.join("");
  },
  /**
   * Converts a UTF-16 LE string to a word array.
   *
   * @param {string} utf16Str The UTF-16 LE string.
   *
   * @return {WordArray} The word array.
   *
   * @static
   *
   * @example
   *
   *     const wordArray = CryptoJS.enc.Utf16LE.parse(utf16Str);
   */
  parse(utf16Str) {
    const utf16StrLength = utf16Str.length;
    const words = [];
    for (let i = 0; i < utf16StrLength; i += 1) {
      words[i >>> 1] |= swapEndian(utf16Str.charCodeAt(i) << 16 - i % 2 * 16);
    }
    return WordArray.create(words, utf16StrLength * 2);
  }
};
const W$2 = [];
class SHA1Algo extends Hasher {
  _doReset() {
    this._hash = new WordArray([
      1732584193,
      4023233417,
      2562383102,
      271733878,
      3285377520
    ]);
  }
  _doProcessBlock(M, offset) {
    const H2 = this._hash.words;
    let a = H2[0];
    let b = H2[1];
    let c = H2[2];
    let d2 = H2[3];
    let e = H2[4];
    for (let i = 0; i < 80; i += 1) {
      if (i < 16) {
        W$2[i] = M[offset + i] | 0;
      } else {
        const n2 = W$2[i - 3] ^ W$2[i - 8] ^ W$2[i - 14] ^ W$2[i - 16];
        W$2[i] = n2 << 1 | n2 >>> 31;
      }
      let t = (a << 5 | a >>> 27) + e + W$2[i];
      if (i < 20) {
        t += (b & c | ~b & d2) + 1518500249;
      } else if (i < 40) {
        t += (b ^ c ^ d2) + 1859775393;
      } else if (i < 60) {
        t += (b & c | b & d2 | c & d2) - 1894007588;
      } else {
        t += (b ^ c ^ d2) - 899497514;
      }
      e = d2;
      d2 = c;
      c = b << 30 | b >>> 2;
      b = a;
      a = t;
    }
    H2[0] = H2[0] + a | 0;
    H2[1] = H2[1] + b | 0;
    H2[2] = H2[2] + c | 0;
    H2[3] = H2[3] + d2 | 0;
    H2[4] = H2[4] + e | 0;
  }
  _doFinalize() {
    const data = this._data;
    const dataWords = data.words;
    const nBitsTotal = this._nDataBytes * 8;
    const nBitsLeft = data.sigBytes * 8;
    dataWords[nBitsLeft >>> 5] |= 128 << 24 - nBitsLeft % 32;
    dataWords[(nBitsLeft + 64 >>> 9 << 4) + 14] = Math.floor(nBitsTotal / 4294967296);
    dataWords[(nBitsLeft + 64 >>> 9 << 4) + 15] = nBitsTotal;
    data.sigBytes = dataWords.length * 4;
    this._process();
    return this._hash;
  }
  clone() {
    const clone = super.clone.call(this);
    clone._hash = this._hash.clone();
    return clone;
  }
}
const SHA1 = Hasher._createHelper(SHA1Algo);
const HmacSHA1 = Hasher._createHmacHelper(SHA1Algo);
const H = [];
const K$1 = [];
const isPrime = (n2) => {
  const sqrtN = Math.sqrt(n2);
  for (let factor = 2; factor <= sqrtN; factor += 1) {
    if (!(n2 % factor)) {
      return false;
    }
  }
  return true;
};
const getFractionalBits = (n2) => (n2 - (n2 | 0)) * 4294967296 | 0;
let n = 2;
let nPrime = 0;
while (nPrime < 64) {
  if (isPrime(n)) {
    if (nPrime < 8) {
      H[nPrime] = getFractionalBits(n ** (1 / 2));
    }
    K$1[nPrime] = getFractionalBits(n ** (1 / 3));
    nPrime += 1;
  }
  n += 1;
}
const W$1 = [];
class SHA256Algo extends Hasher {
  _doReset() {
    this._hash = new WordArray(H.slice(0));
  }
  _doProcessBlock(M, offset) {
    const _H = this._hash.words;
    let a = _H[0];
    let b = _H[1];
    let c = _H[2];
    let d2 = _H[3];
    let e = _H[4];
    let f = _H[5];
    let g = _H[6];
    let h = _H[7];
    for (let i = 0; i < 64; i += 1) {
      if (i < 16) {
        W$1[i] = M[offset + i] | 0;
      } else {
        const gamma0x = W$1[i - 15];
        const gamma0 = (gamma0x << 25 | gamma0x >>> 7) ^ (gamma0x << 14 | gamma0x >>> 18) ^ gamma0x >>> 3;
        const gamma1x = W$1[i - 2];
        const gamma1 = (gamma1x << 15 | gamma1x >>> 17) ^ (gamma1x << 13 | gamma1x >>> 19) ^ gamma1x >>> 10;
        W$1[i] = gamma0 + W$1[i - 7] + gamma1 + W$1[i - 16];
      }
      const ch = e & f ^ ~e & g;
      const maj = a & b ^ a & c ^ b & c;
      const sigma0 = (a << 30 | a >>> 2) ^ (a << 19 | a >>> 13) ^ (a << 10 | a >>> 22);
      const sigma1 = (e << 26 | e >>> 6) ^ (e << 21 | e >>> 11) ^ (e << 7 | e >>> 25);
      const t1 = h + sigma1 + ch + K$1[i] + W$1[i];
      const t2 = sigma0 + maj;
      h = g;
      g = f;
      f = e;
      e = d2 + t1 | 0;
      d2 = c;
      c = b;
      b = a;
      a = t1 + t2 | 0;
    }
    _H[0] = _H[0] + a | 0;
    _H[1] = _H[1] + b | 0;
    _H[2] = _H[2] + c | 0;
    _H[3] = _H[3] + d2 | 0;
    _H[4] = _H[4] + e | 0;
    _H[5] = _H[5] + f | 0;
    _H[6] = _H[6] + g | 0;
    _H[7] = _H[7] + h | 0;
  }
  _doFinalize() {
    const data = this._data;
    const dataWords = data.words;
    const nBitsTotal = this._nDataBytes * 8;
    const nBitsLeft = data.sigBytes * 8;
    dataWords[nBitsLeft >>> 5] |= 128 << 24 - nBitsLeft % 32;
    dataWords[(nBitsLeft + 64 >>> 9 << 4) + 14] = Math.floor(nBitsTotal / 4294967296);
    dataWords[(nBitsLeft + 64 >>> 9 << 4) + 15] = nBitsTotal;
    data.sigBytes = dataWords.length * 4;
    this._process();
    return this._hash;
  }
  clone() {
    const clone = super.clone.call(this);
    clone._hash = this._hash.clone();
    return clone;
  }
}
const SHA256 = Hasher._createHelper(SHA256Algo);
const HmacSHA256 = Hasher._createHmacHelper(SHA256Algo);
class SHA224Algo extends SHA256Algo {
  _doReset() {
    this._hash = new WordArray([
      3238371032,
      914150663,
      812702999,
      4144912697,
      4290775857,
      1750603025,
      1694076839,
      3204075428
    ]);
  }
  _doFinalize() {
    const hash = super._doFinalize.call(this);
    hash.sigBytes -= 4;
    return hash;
  }
}
const SHA224 = SHA256Algo._createHelper(SHA224Algo);
const HmacSHA224 = SHA256Algo._createHmacHelper(SHA224Algo);
const K = [
  new X64Word(1116352408, 3609767458),
  new X64Word(1899447441, 602891725),
  new X64Word(3049323471, 3964484399),
  new X64Word(3921009573, 2173295548),
  new X64Word(961987163, 4081628472),
  new X64Word(1508970993, 3053834265),
  new X64Word(2453635748, 2937671579),
  new X64Word(2870763221, 3664609560),
  new X64Word(3624381080, 2734883394),
  new X64Word(310598401, 1164996542),
  new X64Word(607225278, 1323610764),
  new X64Word(1426881987, 3590304994),
  new X64Word(1925078388, 4068182383),
  new X64Word(2162078206, 991336113),
  new X64Word(2614888103, 633803317),
  new X64Word(3248222580, 3479774868),
  new X64Word(3835390401, 2666613458),
  new X64Word(4022224774, 944711139),
  new X64Word(264347078, 2341262773),
  new X64Word(604807628, 2007800933),
  new X64Word(770255983, 1495990901),
  new X64Word(1249150122, 1856431235),
  new X64Word(1555081692, 3175218132),
  new X64Word(1996064986, 2198950837),
  new X64Word(2554220882, 3999719339),
  new X64Word(2821834349, 766784016),
  new X64Word(2952996808, 2566594879),
  new X64Word(3210313671, 3203337956),
  new X64Word(3336571891, 1034457026),
  new X64Word(3584528711, 2466948901),
  new X64Word(113926993, 3758326383),
  new X64Word(338241895, 168717936),
  new X64Word(666307205, 1188179964),
  new X64Word(773529912, 1546045734),
  new X64Word(1294757372, 1522805485),
  new X64Word(1396182291, 2643833823),
  new X64Word(1695183700, 2343527390),
  new X64Word(1986661051, 1014477480),
  new X64Word(2177026350, 1206759142),
  new X64Word(2456956037, 344077627),
  new X64Word(2730485921, 1290863460),
  new X64Word(2820302411, 3158454273),
  new X64Word(3259730800, 3505952657),
  new X64Word(3345764771, 106217008),
  new X64Word(3516065817, 3606008344),
  new X64Word(3600352804, 1432725776),
  new X64Word(4094571909, 1467031594),
  new X64Word(275423344, 851169720),
  new X64Word(430227734, 3100823752),
  new X64Word(506948616, 1363258195),
  new X64Word(659060556, 3750685593),
  new X64Word(883997877, 3785050280),
  new X64Word(958139571, 3318307427),
  new X64Word(1322822218, 3812723403),
  new X64Word(1537002063, 2003034995),
  new X64Word(1747873779, 3602036899),
  new X64Word(1955562222, 1575990012),
  new X64Word(2024104815, 1125592928),
  new X64Word(2227730452, 2716904306),
  new X64Word(2361852424, 442776044),
  new X64Word(2428436474, 593698344),
  new X64Word(2756734187, 3733110249),
  new X64Word(3204031479, 2999351573),
  new X64Word(3329325298, 3815920427),
  new X64Word(3391569614, 3928383900),
  new X64Word(3515267271, 566280711),
  new X64Word(3940187606, 3454069534),
  new X64Word(4118630271, 4000239992),
  new X64Word(116418474, 1914138554),
  new X64Word(174292421, 2731055270),
  new X64Word(289380356, 3203993006),
  new X64Word(460393269, 320620315),
  new X64Word(685471733, 587496836),
  new X64Word(852142971, 1086792851),
  new X64Word(1017036298, 365543100),
  new X64Word(1126000580, 2618297676),
  new X64Word(1288033470, 3409855158),
  new X64Word(1501505948, 4234509866),
  new X64Word(1607167915, 987167468),
  new X64Word(1816402316, 1246189591)
];
const W = [];
for (let i = 0; i < 80; i += 1) {
  W[i] = new X64Word();
}
class SHA512Algo extends Hasher {
  constructor() {
    super();
    this.blockSize = 1024 / 32;
  }
  _doReset() {
    this._hash = new X64WordArray([
      new X64Word(1779033703, 4089235720),
      new X64Word(3144134277, 2227873595),
      new X64Word(1013904242, 4271175723),
      new X64Word(2773480762, 1595750129),
      new X64Word(1359893119, 2917565137),
      new X64Word(2600822924, 725511199),
      new X64Word(528734635, 4215389547),
      new X64Word(1541459225, 327033209)
    ]);
  }
  _doProcessBlock(M, offset) {
    const H2 = this._hash.words;
    const H0 = H2[0];
    const H1 = H2[1];
    const H22 = H2[2];
    const H3 = H2[3];
    const H4 = H2[4];
    const H5 = H2[5];
    const H6 = H2[6];
    const H7 = H2[7];
    const H0h = H0.high;
    let H0l = H0.low;
    const H1h = H1.high;
    let H1l = H1.low;
    const H2h = H22.high;
    let H2l = H22.low;
    const H3h = H3.high;
    let H3l = H3.low;
    const H4h = H4.high;
    let H4l = H4.low;
    const H5h = H5.high;
    let H5l = H5.low;
    const H6h = H6.high;
    let H6l = H6.low;
    const H7h = H7.high;
    let H7l = H7.low;
    let ah = H0h;
    let al = H0l;
    let bh = H1h;
    let bl = H1l;
    let ch = H2h;
    let cl = H2l;
    let dh = H3h;
    let dl = H3l;
    let eh = H4h;
    let el = H4l;
    let fh = H5h;
    let fl = H5l;
    let gh = H6h;
    let gl = H6l;
    let hh = H7h;
    let hl = H7l;
    for (let i = 0; i < 80; i += 1) {
      let Wil;
      let Wih;
      const Wi = W[i];
      if (i < 16) {
        Wi.high = M[offset + i * 2] | 0;
        Wih = Wi.high;
        Wi.low = M[offset + i * 2 + 1] | 0;
        Wil = Wi.low;
      } else {
        const gamma0x = W[i - 15];
        const gamma0xh = gamma0x.high;
        const gamma0xl = gamma0x.low;
        const gamma0h = (gamma0xh >>> 1 | gamma0xl << 31) ^ (gamma0xh >>> 8 | gamma0xl << 24) ^ gamma0xh >>> 7;
        const gamma0l = (gamma0xl >>> 1 | gamma0xh << 31) ^ (gamma0xl >>> 8 | gamma0xh << 24) ^ (gamma0xl >>> 7 | gamma0xh << 25);
        const gamma1x = W[i - 2];
        const gamma1xh = gamma1x.high;
        const gamma1xl = gamma1x.low;
        const gamma1h = (gamma1xh >>> 19 | gamma1xl << 13) ^ (gamma1xh << 3 | gamma1xl >>> 29) ^ gamma1xh >>> 6;
        const gamma1l = (gamma1xl >>> 19 | gamma1xh << 13) ^ (gamma1xl << 3 | gamma1xh >>> 29) ^ (gamma1xl >>> 6 | gamma1xh << 26);
        const Wi7 = W[i - 7];
        const Wi7h = Wi7.high;
        const Wi7l = Wi7.low;
        const Wi16 = W[i - 16];
        const Wi16h = Wi16.high;
        const Wi16l = Wi16.low;
        Wil = gamma0l + Wi7l;
        Wih = gamma0h + Wi7h + (Wil >>> 0 < gamma0l >>> 0 ? 1 : 0);
        Wil += gamma1l;
        Wih = Wih + gamma1h + (Wil >>> 0 < gamma1l >>> 0 ? 1 : 0);
        Wil += Wi16l;
        Wih = Wih + Wi16h + (Wil >>> 0 < Wi16l >>> 0 ? 1 : 0);
        Wi.high = Wih;
        Wi.low = Wil;
      }
      const chh = eh & fh ^ ~eh & gh;
      const chl = el & fl ^ ~el & gl;
      const majh = ah & bh ^ ah & ch ^ bh & ch;
      const majl = al & bl ^ al & cl ^ bl & cl;
      const sigma0h = (ah >>> 28 | al << 4) ^ (ah << 30 | al >>> 2) ^ (ah << 25 | al >>> 7);
      const sigma0l = (al >>> 28 | ah << 4) ^ (al << 30 | ah >>> 2) ^ (al << 25 | ah >>> 7);
      const sigma1h = (eh >>> 14 | el << 18) ^ (eh >>> 18 | el << 14) ^ (eh << 23 | el >>> 9);
      const sigma1l = (el >>> 14 | eh << 18) ^ (el >>> 18 | eh << 14) ^ (el << 23 | eh >>> 9);
      const Ki = K[i];
      const Kih = Ki.high;
      const Kil = Ki.low;
      let t1l = hl + sigma1l;
      let t1h = hh + sigma1h + (t1l >>> 0 < hl >>> 0 ? 1 : 0);
      t1l += chl;
      t1h = t1h + chh + (t1l >>> 0 < chl >>> 0 ? 1 : 0);
      t1l += Kil;
      t1h = t1h + Kih + (t1l >>> 0 < Kil >>> 0 ? 1 : 0);
      t1l += Wil;
      t1h = t1h + Wih + (t1l >>> 0 < Wil >>> 0 ? 1 : 0);
      const t2l = sigma0l + majl;
      const t2h = sigma0h + majh + (t2l >>> 0 < sigma0l >>> 0 ? 1 : 0);
      hh = gh;
      hl = gl;
      gh = fh;
      gl = fl;
      fh = eh;
      fl = el;
      el = dl + t1l | 0;
      eh = dh + t1h + (el >>> 0 < dl >>> 0 ? 1 : 0) | 0;
      dh = ch;
      dl = cl;
      ch = bh;
      cl = bl;
      bh = ah;
      bl = al;
      al = t1l + t2l | 0;
      ah = t1h + t2h + (al >>> 0 < t1l >>> 0 ? 1 : 0) | 0;
    }
    H0.low = H0l + al;
    H0l = H0.low;
    H0.high = H0h + ah + (H0l >>> 0 < al >>> 0 ? 1 : 0);
    H1.low = H1l + bl;
    H1l = H1.low;
    H1.high = H1h + bh + (H1l >>> 0 < bl >>> 0 ? 1 : 0);
    H22.low = H2l + cl;
    H2l = H22.low;
    H22.high = H2h + ch + (H2l >>> 0 < cl >>> 0 ? 1 : 0);
    H3.low = H3l + dl;
    H3l = H3.low;
    H3.high = H3h + dh + (H3l >>> 0 < dl >>> 0 ? 1 : 0);
    H4.low = H4l + el;
    H4l = H4.low;
    H4.high = H4h + eh + (H4l >>> 0 < el >>> 0 ? 1 : 0);
    H5.low = H5l + fl;
    H5l = H5.low;
    H5.high = H5h + fh + (H5l >>> 0 < fl >>> 0 ? 1 : 0);
    H6.low = H6l + gl;
    H6l = H6.low;
    H6.high = H6h + gh + (H6l >>> 0 < gl >>> 0 ? 1 : 0);
    H7.low = H7l + hl;
    H7l = H7.low;
    H7.high = H7h + hh + (H7l >>> 0 < hl >>> 0 ? 1 : 0);
  }
  _doFinalize() {
    const data = this._data;
    const dataWords = data.words;
    const nBitsTotal = this._nDataBytes * 8;
    const nBitsLeft = data.sigBytes * 8;
    dataWords[nBitsLeft >>> 5] |= 128 << 24 - nBitsLeft % 32;
    dataWords[(nBitsLeft + 128 >>> 10 << 5) + 30] = Math.floor(nBitsTotal / 4294967296);
    dataWords[(nBitsLeft + 128 >>> 10 << 5) + 31] = nBitsTotal;
    data.sigBytes = dataWords.length * 4;
    this._process();
    const hash = this._hash.toX32();
    return hash;
  }
  clone() {
    const clone = super.clone.call(this);
    clone._hash = this._hash.clone();
    return clone;
  }
}
const SHA512 = Hasher._createHelper(SHA512Algo);
const HmacSHA512 = Hasher._createHmacHelper(SHA512Algo);
class SHA384Algo extends SHA512Algo {
  _doReset() {
    this._hash = new X64WordArray([
      new X64Word(3418070365, 3238371032),
      new X64Word(1654270250, 914150663),
      new X64Word(2438529370, 812702999),
      new X64Word(355462360, 4144912697),
      new X64Word(1731405415, 4290775857),
      new X64Word(2394180231, 1750603025),
      new X64Word(3675008525, 1694076839),
      new X64Word(1203062813, 3204075428)
    ]);
  }
  _doFinalize() {
    const hash = super._doFinalize.call(this);
    hash.sigBytes -= 16;
    return hash;
  }
}
const SHA384 = SHA512Algo._createHelper(SHA384Algo);
const HmacSHA384 = SHA512Algo._createHmacHelper(SHA384Algo);
const RHO_OFFSETS = [];
const PI_INDEXES = [];
const ROUND_CONSTANTS = [];
let _x = 1;
let _y = 0;
for (let t = 0; t < 24; t += 1) {
  RHO_OFFSETS[_x + 5 * _y] = (t + 1) * (t + 2) / 2 % 64;
  const newX = _y % 5;
  const newY = (2 * _x + 3 * _y) % 5;
  _x = newX;
  _y = newY;
}
for (let x2 = 0; x2 < 5; x2 += 1) {
  for (let y = 0; y < 5; y += 1) {
    PI_INDEXES[x2 + 5 * y] = y + (2 * x2 + 3 * y) % 5 * 5;
  }
}
let LFSR = 1;
for (let i = 0; i < 24; i += 1) {
  let roundConstantMsw = 0;
  let roundConstantLsw = 0;
  for (let j = 0; j < 7; j += 1) {
    if (LFSR & 1) {
      const bitPosition = (1 << j) - 1;
      if (bitPosition < 32) {
        roundConstantLsw ^= 1 << bitPosition;
      } else {
        roundConstantMsw ^= 1 << bitPosition - 32;
      }
    }
    if (LFSR & 128) {
      LFSR = LFSR << 1 ^ 113;
    } else {
      LFSR <<= 1;
    }
  }
  ROUND_CONSTANTS[i] = X64Word.create(roundConstantMsw, roundConstantLsw);
}
const T = [];
for (let i = 0; i < 25; i += 1) {
  T[i] = X64Word.create();
}
class SHA3Algo extends Hasher {
  constructor(cfg) {
    super(Object.assign(
      { outputLength: 512 },
      cfg
    ));
  }
  _doReset() {
    this._state = [];
    const state = this._state;
    for (let i = 0; i < 25; i += 1) {
      state[i] = new X64Word();
    }
    this.blockSize = (1600 - 2 * this.cfg.outputLength) / 32;
  }
  _doProcessBlock(M, offset) {
    const state = this._state;
    const nBlockSizeLanes = this.blockSize / 2;
    for (let i = 0; i < nBlockSizeLanes; i += 1) {
      let M2i = M[offset + 2 * i];
      let M2i1 = M[offset + 2 * i + 1];
      M2i = (M2i << 8 | M2i >>> 24) & 16711935 | (M2i << 24 | M2i >>> 8) & 4278255360;
      M2i1 = (M2i1 << 8 | M2i1 >>> 24) & 16711935 | (M2i1 << 24 | M2i1 >>> 8) & 4278255360;
      const lane = state[i];
      lane.high ^= M2i1;
      lane.low ^= M2i;
    }
    for (let round = 0; round < 24; round += 1) {
      for (let x2 = 0; x2 < 5; x2 += 1) {
        let tMsw = 0;
        let tLsw = 0;
        for (let y = 0; y < 5; y += 1) {
          const lane2 = state[x2 + 5 * y];
          tMsw ^= lane2.high;
          tLsw ^= lane2.low;
        }
        const Tx = T[x2];
        Tx.high = tMsw;
        Tx.low = tLsw;
      }
      for (let x2 = 0; x2 < 5; x2 += 1) {
        const Tx4 = T[(x2 + 4) % 5];
        const Tx1 = T[(x2 + 1) % 5];
        const Tx1Msw = Tx1.high;
        const Tx1Lsw = Tx1.low;
        const tMsw = Tx4.high ^ (Tx1Msw << 1 | Tx1Lsw >>> 31);
        const tLsw = Tx4.low ^ (Tx1Lsw << 1 | Tx1Msw >>> 31);
        for (let y = 0; y < 5; y += 1) {
          const lane2 = state[x2 + 5 * y];
          lane2.high ^= tMsw;
          lane2.low ^= tLsw;
        }
      }
      for (let laneIndex = 1; laneIndex < 25; laneIndex += 1) {
        let tMsw;
        let tLsw;
        const lane2 = state[laneIndex];
        const laneMsw = lane2.high;
        const laneLsw = lane2.low;
        const rhoOffset = RHO_OFFSETS[laneIndex];
        if (rhoOffset < 32) {
          tMsw = laneMsw << rhoOffset | laneLsw >>> 32 - rhoOffset;
          tLsw = laneLsw << rhoOffset | laneMsw >>> 32 - rhoOffset;
        } else {
          tMsw = laneLsw << rhoOffset - 32 | laneMsw >>> 64 - rhoOffset;
          tLsw = laneMsw << rhoOffset - 32 | laneLsw >>> 64 - rhoOffset;
        }
        const TPiLane = T[PI_INDEXES[laneIndex]];
        TPiLane.high = tMsw;
        TPiLane.low = tLsw;
      }
      const T0 = T[0];
      const state0 = state[0];
      T0.high = state0.high;
      T0.low = state0.low;
      for (let x2 = 0; x2 < 5; x2 += 1) {
        for (let y = 0; y < 5; y += 1) {
          const laneIndex = x2 + 5 * y;
          const lane2 = state[laneIndex];
          const TLane = T[laneIndex];
          const Tx1Lane = T[(x2 + 1) % 5 + 5 * y];
          const Tx2Lane = T[(x2 + 2) % 5 + 5 * y];
          lane2.high = TLane.high ^ ~Tx1Lane.high & Tx2Lane.high;
          lane2.low = TLane.low ^ ~Tx1Lane.low & Tx2Lane.low;
        }
      }
      const lane = state[0];
      const roundConstant = ROUND_CONSTANTS[round];
      lane.high ^= roundConstant.high;
      lane.low ^= roundConstant.low;
    }
  }
  _doFinalize() {
    const data = this._data;
    const dataWords = data.words;
    const nBitsLeft = data.sigBytes * 8;
    const blockSizeBits = this.blockSize * 32;
    dataWords[nBitsLeft >>> 5] |= 1 << 24 - nBitsLeft % 32;
    dataWords[(Math.ceil((nBitsLeft + 1) / blockSizeBits) * blockSizeBits >>> 5) - 1] |= 128;
    data.sigBytes = dataWords.length * 4;
    this._process();
    const state = this._state;
    const outputLengthBytes = this.cfg.outputLength / 8;
    const outputLengthLanes = outputLengthBytes / 8;
    const hashWords = [];
    for (let i = 0; i < outputLengthLanes; i += 1) {
      const lane = state[i];
      let laneMsw = lane.high;
      let laneLsw = lane.low;
      laneMsw = (laneMsw << 8 | laneMsw >>> 24) & 16711935 | (laneMsw << 24 | laneMsw >>> 8) & 4278255360;
      laneLsw = (laneLsw << 8 | laneLsw >>> 24) & 16711935 | (laneLsw << 24 | laneLsw >>> 8) & 4278255360;
      hashWords.push(laneLsw);
      hashWords.push(laneMsw);
    }
    return new WordArray(hashWords, outputLengthBytes);
  }
  clone() {
    const clone = super.clone.call(this);
    clone._state = this._state.slice(0);
    const state = clone._state;
    for (let i = 0; i < 25; i += 1) {
      state[i] = state[i].clone();
    }
    return clone;
  }
}
const SHA3 = Hasher._createHelper(SHA3Algo);
const HmacSHA3 = Hasher._createHmacHelper(SHA3Algo);
/** @preserve
(c) 2012 by Cédric Mesnil. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted
provided that the following conditions are met:

    - Redistributions of source code must retain the above copyright notice, this list of
    conditions and the following disclaimer.
    - Redistributions in binary form must reproduce the above copyright notice, this list
    of conditions and the following disclaimer in the documentation and/or other materials
    provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS
OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
const _zl = WordArray.create([
  0,
  1,
  2,
  3,
  4,
  5,
  6,
  7,
  8,
  9,
  10,
  11,
  12,
  13,
  14,
  15,
  7,
  4,
  13,
  1,
  10,
  6,
  15,
  3,
  12,
  0,
  9,
  5,
  2,
  14,
  11,
  8,
  3,
  10,
  14,
  4,
  9,
  15,
  8,
  1,
  2,
  7,
  0,
  6,
  13,
  11,
  5,
  12,
  1,
  9,
  11,
  10,
  0,
  8,
  12,
  4,
  13,
  3,
  7,
  15,
  14,
  5,
  6,
  2,
  4,
  0,
  5,
  9,
  7,
  12,
  2,
  10,
  14,
  1,
  3,
  8,
  11,
  6,
  15,
  13
]);
const _zr = WordArray.create([
  5,
  14,
  7,
  0,
  9,
  2,
  11,
  4,
  13,
  6,
  15,
  8,
  1,
  10,
  3,
  12,
  6,
  11,
  3,
  7,
  0,
  13,
  5,
  10,
  14,
  15,
  8,
  12,
  4,
  9,
  1,
  2,
  15,
  5,
  1,
  3,
  7,
  14,
  6,
  9,
  11,
  8,
  12,
  2,
  10,
  0,
  4,
  13,
  8,
  6,
  4,
  1,
  3,
  11,
  15,
  0,
  5,
  12,
  2,
  13,
  9,
  7,
  10,
  14,
  12,
  15,
  10,
  4,
  1,
  5,
  8,
  7,
  6,
  2,
  13,
  14,
  0,
  3,
  9,
  11
]);
const _sl = WordArray.create([
  11,
  14,
  15,
  12,
  5,
  8,
  7,
  9,
  11,
  13,
  14,
  15,
  6,
  7,
  9,
  8,
  7,
  6,
  8,
  13,
  11,
  9,
  7,
  15,
  7,
  12,
  15,
  9,
  11,
  7,
  13,
  12,
  11,
  13,
  6,
  7,
  14,
  9,
  13,
  15,
  14,
  8,
  13,
  6,
  5,
  12,
  7,
  5,
  11,
  12,
  14,
  15,
  14,
  15,
  9,
  8,
  9,
  14,
  5,
  6,
  8,
  6,
  5,
  12,
  9,
  15,
  5,
  11,
  6,
  8,
  13,
  12,
  5,
  12,
  13,
  14,
  11,
  8,
  5,
  6
]);
const _sr = WordArray.create([
  8,
  9,
  9,
  11,
  13,
  15,
  15,
  5,
  7,
  7,
  8,
  11,
  14,
  14,
  12,
  6,
  9,
  13,
  15,
  7,
  12,
  8,
  9,
  11,
  7,
  7,
  12,
  7,
  6,
  15,
  13,
  11,
  9,
  7,
  15,
  11,
  8,
  6,
  6,
  14,
  12,
  13,
  5,
  14,
  13,
  13,
  7,
  5,
  15,
  5,
  8,
  11,
  14,
  14,
  6,
  14,
  6,
  9,
  12,
  9,
  12,
  5,
  15,
  8,
  8,
  5,
  12,
  9,
  12,
  5,
  14,
  6,
  8,
  13,
  6,
  5,
  15,
  13,
  11,
  11
]);
const _hl = WordArray.create([0, 1518500249, 1859775393, 2400959708, 2840853838]);
const _hr = WordArray.create([1352829926, 1548603684, 1836072691, 2053994217, 0]);
const f1 = (x2, y, z) => x2 ^ y ^ z;
const f2 = (x2, y, z) => x2 & y | ~x2 & z;
const f3 = (x2, y, z) => (x2 | ~y) ^ z;
const f4 = (x2, y, z) => x2 & z | y & ~z;
const f5 = (x2, y, z) => x2 ^ (y | ~z);
const rotl = (x2, n2) => x2 << n2 | x2 >>> 32 - n2;
class RIPEMD160Algo extends Hasher {
  _doReset() {
    this._hash = WordArray.create([1732584193, 4023233417, 2562383102, 271733878, 3285377520]);
  }
  _doProcessBlock(M, offset) {
    const _M = M;
    for (let i = 0; i < 16; i += 1) {
      const offset_i = offset + i;
      const M_offset_i = _M[offset_i];
      _M[offset_i] = (M_offset_i << 8 | M_offset_i >>> 24) & 16711935 | (M_offset_i << 24 | M_offset_i >>> 8) & 4278255360;
    }
    const H2 = this._hash.words;
    const hl = _hl.words;
    const hr = _hr.words;
    const zl = _zl.words;
    const zr = _zr.words;
    const sl = _sl.words;
    const sr = _sr.words;
    let al = H2[0];
    let bl = H2[1];
    let cl = H2[2];
    let dl = H2[3];
    let el = H2[4];
    let ar = H2[0];
    let br = H2[1];
    let cr = H2[2];
    let dr = H2[3];
    let er = H2[4];
    let t;
    for (let i = 0; i < 80; i += 1) {
      t = al + _M[offset + zl[i]] | 0;
      if (i < 16) {
        t += f1(bl, cl, dl) + hl[0];
      } else if (i < 32) {
        t += f2(bl, cl, dl) + hl[1];
      } else if (i < 48) {
        t += f3(bl, cl, dl) + hl[2];
      } else if (i < 64) {
        t += f4(bl, cl, dl) + hl[3];
      } else {
        t += f5(bl, cl, dl) + hl[4];
      }
      t |= 0;
      t = rotl(t, sl[i]);
      t = t + el | 0;
      al = el;
      el = dl;
      dl = rotl(cl, 10);
      cl = bl;
      bl = t;
      t = ar + _M[offset + zr[i]] | 0;
      if (i < 16) {
        t += f5(br, cr, dr) + hr[0];
      } else if (i < 32) {
        t += f4(br, cr, dr) + hr[1];
      } else if (i < 48) {
        t += f3(br, cr, dr) + hr[2];
      } else if (i < 64) {
        t += f2(br, cr, dr) + hr[3];
      } else {
        t += f1(br, cr, dr) + hr[4];
      }
      t |= 0;
      t = rotl(t, sr[i]);
      t = t + er | 0;
      ar = er;
      er = dr;
      dr = rotl(cr, 10);
      cr = br;
      br = t;
    }
    t = H2[1] + cl + dr | 0;
    H2[1] = H2[2] + dl + er | 0;
    H2[2] = H2[3] + el + ar | 0;
    H2[3] = H2[4] + al + br | 0;
    H2[4] = H2[0] + bl + cr | 0;
    H2[0] = t;
  }
  _doFinalize() {
    const data = this._data;
    const dataWords = data.words;
    const nBitsTotal = this._nDataBytes * 8;
    const nBitsLeft = data.sigBytes * 8;
    dataWords[nBitsLeft >>> 5] |= 128 << 24 - nBitsLeft % 32;
    dataWords[(nBitsLeft + 64 >>> 9 << 4) + 14] = (nBitsTotal << 8 | nBitsTotal >>> 24) & 16711935 | (nBitsTotal << 24 | nBitsTotal >>> 8) & 4278255360;
    data.sigBytes = (dataWords.length + 1) * 4;
    this._process();
    const hash = this._hash;
    const H2 = hash.words;
    for (let i = 0; i < 5; i += 1) {
      const H_i = H2[i];
      H2[i] = (H_i << 8 | H_i >>> 24) & 16711935 | (H_i << 24 | H_i >>> 8) & 4278255360;
    }
    return hash;
  }
  clone() {
    const clone = super.clone.call(this);
    clone._hash = this._hash.clone();
    return clone;
  }
}
const RIPEMD160 = Hasher._createHelper(RIPEMD160Algo);
const HmacRIPEMD160 = Hasher._createHmacHelper(RIPEMD160Algo);
class PBKDF2Algo extends Base {
  /**
   * Initializes a newly created key derivation function.
   *
   * @param {Object} cfg (Optional) The configuration options to use for the derivation.
   *
   * @example
   *
   *     const kdf = CryptoJS.algo.PBKDF2.create();
   *     const kdf = CryptoJS.algo.PBKDF2.create({ keySize: 8 });
   *     const kdf = CryptoJS.algo.PBKDF2.create({ keySize: 8, iterations: 1000 });
   */
  constructor(cfg) {
    super();
    this.cfg = Object.assign(
      new Base(),
      {
        keySize: 128 / 32,
        hasher: SHA1Algo,
        iterations: 1
      },
      cfg
    );
  }
  /**
   * Computes the Password-Based Key Derivation Function 2.
   *
   * @param {WordArray|string} password The password.
   * @param {WordArray|string} salt A salt.
   *
   * @return {WordArray} The derived key.
   *
   * @example
   *
   *     const key = kdf.compute(password, salt);
   */
  compute(password, salt) {
    const { cfg } = this;
    const hmac = HMAC.create(cfg.hasher, password);
    const derivedKey = WordArray.create();
    const blockIndex = WordArray.create([1]);
    const derivedKeyWords = derivedKey.words;
    const blockIndexWords = blockIndex.words;
    const { keySize, iterations } = cfg;
    while (derivedKeyWords.length < keySize) {
      const block = hmac.update(salt).finalize(blockIndex);
      hmac.reset();
      const blockWords = block.words;
      const blockWordsLength = blockWords.length;
      let intermediate = block;
      for (let i = 1; i < iterations; i += 1) {
        intermediate = hmac.finalize(intermediate);
        hmac.reset();
        const intermediateWords = intermediate.words;
        for (let j = 0; j < blockWordsLength; j += 1) {
          blockWords[j] ^= intermediateWords[j];
        }
      }
      derivedKey.concat(block);
      blockIndexWords[0] += 1;
    }
    derivedKey.sigBytes = keySize * 4;
    return derivedKey;
  }
}
const PBKDF2 = (password, salt, cfg) => PBKDF2Algo.create(cfg).compute(password, salt);
const _SBOX = [];
const INV_SBOX = [];
const _SUB_MIX_0 = [];
const _SUB_MIX_1 = [];
const _SUB_MIX_2 = [];
const _SUB_MIX_3 = [];
const INV_SUB_MIX_0 = [];
const INV_SUB_MIX_1 = [];
const INV_SUB_MIX_2 = [];
const INV_SUB_MIX_3 = [];
const d = [];
for (let i = 0; i < 256; i += 1) {
  if (i < 128) {
    d[i] = i << 1;
  } else {
    d[i] = i << 1 ^ 283;
  }
}
let x = 0;
let xi = 0;
for (let i = 0; i < 256; i += 1) {
  let sx = xi ^ xi << 1 ^ xi << 2 ^ xi << 3 ^ xi << 4;
  sx = sx >>> 8 ^ sx & 255 ^ 99;
  _SBOX[x] = sx;
  INV_SBOX[sx] = x;
  const x2 = d[x];
  const x4 = d[x2];
  const x8 = d[x4];
  let t = d[sx] * 257 ^ sx * 16843008;
  _SUB_MIX_0[x] = t << 24 | t >>> 8;
  _SUB_MIX_1[x] = t << 16 | t >>> 16;
  _SUB_MIX_2[x] = t << 8 | t >>> 24;
  _SUB_MIX_3[x] = t;
  t = x8 * 16843009 ^ x4 * 65537 ^ x2 * 257 ^ x * 16843008;
  INV_SUB_MIX_0[sx] = t << 24 | t >>> 8;
  INV_SUB_MIX_1[sx] = t << 16 | t >>> 16;
  INV_SUB_MIX_2[sx] = t << 8 | t >>> 24;
  INV_SUB_MIX_3[sx] = t;
  if (!x) {
    xi = 1;
    x = xi;
  } else {
    x = x2 ^ d[d[d[x8 ^ x2]]];
    xi ^= d[d[xi]];
  }
}
const RCON = [0, 1, 2, 4, 8, 16, 32, 64, 128, 27, 54];
class AESAlgo extends BlockCipher {
  _doReset() {
    let t;
    if (this._nRounds && this._keyPriorReset === this._key) {
      return;
    }
    this._keyPriorReset = this._key;
    const key = this._keyPriorReset;
    const keyWords = key.words;
    const keySize = key.sigBytes / 4;
    this._nRounds = keySize + 6;
    const nRounds = this._nRounds;
    const ksRows = (nRounds + 1) * 4;
    this._keySchedule = [];
    const keySchedule = this._keySchedule;
    for (let ksRow = 0; ksRow < ksRows; ksRow += 1) {
      if (ksRow < keySize) {
        keySchedule[ksRow] = keyWords[ksRow];
      } else {
        t = keySchedule[ksRow - 1];
        if (!(ksRow % keySize)) {
          t = t << 8 | t >>> 24;
          t = _SBOX[t >>> 24] << 24 | _SBOX[t >>> 16 & 255] << 16 | _SBOX[t >>> 8 & 255] << 8 | _SBOX[t & 255];
          t ^= RCON[ksRow / keySize | 0] << 24;
        } else if (keySize > 6 && ksRow % keySize === 4) {
          t = _SBOX[t >>> 24] << 24 | _SBOX[t >>> 16 & 255] << 16 | _SBOX[t >>> 8 & 255] << 8 | _SBOX[t & 255];
        }
        keySchedule[ksRow] = keySchedule[ksRow - keySize] ^ t;
      }
    }
    this._invKeySchedule = [];
    const invKeySchedule = this._invKeySchedule;
    for (let invKsRow = 0; invKsRow < ksRows; invKsRow += 1) {
      const ksRow = ksRows - invKsRow;
      if (invKsRow % 4) {
        t = keySchedule[ksRow];
      } else {
        t = keySchedule[ksRow - 4];
      }
      if (invKsRow < 4 || ksRow <= 4) {
        invKeySchedule[invKsRow] = t;
      } else {
        invKeySchedule[invKsRow] = INV_SUB_MIX_0[_SBOX[t >>> 24]] ^ INV_SUB_MIX_1[_SBOX[t >>> 16 & 255]] ^ INV_SUB_MIX_2[_SBOX[t >>> 8 & 255]] ^ INV_SUB_MIX_3[_SBOX[t & 255]];
      }
    }
  }
  encryptBlock(M, offset) {
    this._doCryptBlock(
      M,
      offset,
      this._keySchedule,
      _SUB_MIX_0,
      _SUB_MIX_1,
      _SUB_MIX_2,
      _SUB_MIX_3,
      _SBOX
    );
  }
  decryptBlock(M, offset) {
    const _M = M;
    let t = _M[offset + 1];
    _M[offset + 1] = _M[offset + 3];
    _M[offset + 3] = t;
    this._doCryptBlock(
      _M,
      offset,
      this._invKeySchedule,
      INV_SUB_MIX_0,
      INV_SUB_MIX_1,
      INV_SUB_MIX_2,
      INV_SUB_MIX_3,
      INV_SBOX
    );
    t = _M[offset + 1];
    _M[offset + 1] = _M[offset + 3];
    _M[offset + 3] = t;
  }
  _doCryptBlock(M, offset, keySchedule, SUB_MIX_0, SUB_MIX_1, SUB_MIX_2, SUB_MIX_3, SBOX) {
    const _M = M;
    const nRounds = this._nRounds;
    let s0 = _M[offset] ^ keySchedule[0];
    let s1 = _M[offset + 1] ^ keySchedule[1];
    let s2 = _M[offset + 2] ^ keySchedule[2];
    let s3 = _M[offset + 3] ^ keySchedule[3];
    let ksRow = 4;
    for (let round = 1; round < nRounds; round += 1) {
      const t02 = SUB_MIX_0[s0 >>> 24] ^ SUB_MIX_1[s1 >>> 16 & 255] ^ SUB_MIX_2[s2 >>> 8 & 255] ^ SUB_MIX_3[s3 & 255] ^ keySchedule[ksRow];
      ksRow += 1;
      const t12 = SUB_MIX_0[s1 >>> 24] ^ SUB_MIX_1[s2 >>> 16 & 255] ^ SUB_MIX_2[s3 >>> 8 & 255] ^ SUB_MIX_3[s0 & 255] ^ keySchedule[ksRow];
      ksRow += 1;
      const t22 = SUB_MIX_0[s2 >>> 24] ^ SUB_MIX_1[s3 >>> 16 & 255] ^ SUB_MIX_2[s0 >>> 8 & 255] ^ SUB_MIX_3[s1 & 255] ^ keySchedule[ksRow];
      ksRow += 1;
      const t32 = SUB_MIX_0[s3 >>> 24] ^ SUB_MIX_1[s0 >>> 16 & 255] ^ SUB_MIX_2[s1 >>> 8 & 255] ^ SUB_MIX_3[s2 & 255] ^ keySchedule[ksRow];
      ksRow += 1;
      s0 = t02;
      s1 = t12;
      s2 = t22;
      s3 = t32;
    }
    const t0 = (SBOX[s0 >>> 24] << 24 | SBOX[s1 >>> 16 & 255] << 16 | SBOX[s2 >>> 8 & 255] << 8 | SBOX[s3 & 255]) ^ keySchedule[ksRow];
    ksRow += 1;
    const t1 = (SBOX[s1 >>> 24] << 24 | SBOX[s2 >>> 16 & 255] << 16 | SBOX[s3 >>> 8 & 255] << 8 | SBOX[s0 & 255]) ^ keySchedule[ksRow];
    ksRow += 1;
    const t2 = (SBOX[s2 >>> 24] << 24 | SBOX[s3 >>> 16 & 255] << 16 | SBOX[s0 >>> 8 & 255] << 8 | SBOX[s1 & 255]) ^ keySchedule[ksRow];
    ksRow += 1;
    const t3 = (SBOX[s3 >>> 24] << 24 | SBOX[s0 >>> 16 & 255] << 16 | SBOX[s1 >>> 8 & 255] << 8 | SBOX[s2 & 255]) ^ keySchedule[ksRow];
    ksRow += 1;
    _M[offset] = t0;
    _M[offset + 1] = t1;
    _M[offset + 2] = t2;
    _M[offset + 3] = t3;
  }
}
AESAlgo.keySize = 256 / 32;
const AES = BlockCipher._createHelper(AESAlgo);
const PC1 = [
  57,
  49,
  41,
  33,
  25,
  17,
  9,
  1,
  58,
  50,
  42,
  34,
  26,
  18,
  10,
  2,
  59,
  51,
  43,
  35,
  27,
  19,
  11,
  3,
  60,
  52,
  44,
  36,
  63,
  55,
  47,
  39,
  31,
  23,
  15,
  7,
  62,
  54,
  46,
  38,
  30,
  22,
  14,
  6,
  61,
  53,
  45,
  37,
  29,
  21,
  13,
  5,
  28,
  20,
  12,
  4
];
const PC2 = [
  14,
  17,
  11,
  24,
  1,
  5,
  3,
  28,
  15,
  6,
  21,
  10,
  23,
  19,
  12,
  4,
  26,
  8,
  16,
  7,
  27,
  20,
  13,
  2,
  41,
  52,
  31,
  37,
  47,
  55,
  30,
  40,
  51,
  45,
  33,
  48,
  44,
  49,
  39,
  56,
  34,
  53,
  46,
  42,
  50,
  36,
  29,
  32
];
const BIT_SHIFTS = [1, 2, 4, 6, 8, 10, 12, 14, 15, 17, 19, 21, 23, 25, 27, 28];
const SBOX_P = [
  {
    0: 8421888,
    268435456: 32768,
    536870912: 8421378,
    805306368: 2,
    1073741824: 512,
    1342177280: 8421890,
    1610612736: 8389122,
    1879048192: 8388608,
    2147483648: 514,
    2415919104: 8389120,
    2684354560: 33280,
    2952790016: 8421376,
    3221225472: 32770,
    3489660928: 8388610,
    3758096384: 0,
    4026531840: 33282,
    134217728: 0,
    402653184: 8421890,
    671088640: 33282,
    939524096: 32768,
    1207959552: 8421888,
    1476395008: 512,
    1744830464: 8421378,
    2013265920: 2,
    2281701376: 8389120,
    2550136832: 33280,
    2818572288: 8421376,
    3087007744: 8389122,
    3355443200: 8388610,
    3623878656: 32770,
    3892314112: 514,
    4160749568: 8388608,
    1: 32768,
    268435457: 2,
    536870913: 8421888,
    805306369: 8388608,
    1073741825: 8421378,
    1342177281: 33280,
    1610612737: 512,
    1879048193: 8389122,
    2147483649: 8421890,
    2415919105: 8421376,
    2684354561: 8388610,
    2952790017: 33282,
    3221225473: 514,
    3489660929: 8389120,
    3758096385: 32770,
    4026531841: 0,
    134217729: 8421890,
    402653185: 8421376,
    671088641: 8388608,
    939524097: 512,
    1207959553: 32768,
    1476395009: 8388610,
    1744830465: 2,
    2013265921: 33282,
    2281701377: 32770,
    2550136833: 8389122,
    2818572289: 514,
    3087007745: 8421888,
    3355443201: 8389120,
    3623878657: 0,
    3892314113: 33280,
    4160749569: 8421378
  },
  {
    0: 1074282512,
    16777216: 16384,
    33554432: 524288,
    50331648: 1074266128,
    67108864: 1073741840,
    83886080: 1074282496,
    100663296: 1073758208,
    117440512: 16,
    134217728: 540672,
    150994944: 1073758224,
    167772160: 1073741824,
    184549376: 540688,
    201326592: 524304,
    218103808: 0,
    234881024: 16400,
    251658240: 1074266112,
    8388608: 1073758208,
    25165824: 540688,
    41943040: 16,
    58720256: 1073758224,
    75497472: 1074282512,
    92274688: 1073741824,
    109051904: 524288,
    125829120: 1074266128,
    142606336: 524304,
    159383552: 0,
    176160768: 16384,
    192937984: 1074266112,
    209715200: 1073741840,
    226492416: 540672,
    243269632: 1074282496,
    260046848: 16400,
    268435456: 0,
    285212672: 1074266128,
    301989888: 1073758224,
    318767104: 1074282496,
    335544320: 1074266112,
    352321536: 16,
    369098752: 540688,
    385875968: 16384,
    402653184: 16400,
    419430400: 524288,
    436207616: 524304,
    452984832: 1073741840,
    469762048: 540672,
    486539264: 1073758208,
    503316480: 1073741824,
    520093696: 1074282512,
    276824064: 540688,
    293601280: 524288,
    310378496: 1074266112,
    327155712: 16384,
    343932928: 1073758208,
    360710144: 1074282512,
    377487360: 16,
    394264576: 1073741824,
    411041792: 1074282496,
    427819008: 1073741840,
    444596224: 1073758224,
    461373440: 524304,
    478150656: 0,
    494927872: 16400,
    511705088: 1074266128,
    528482304: 540672
  },
  {
    0: 260,
    1048576: 0,
    2097152: 67109120,
    3145728: 65796,
    4194304: 65540,
    5242880: 67108868,
    6291456: 67174660,
    7340032: 67174400,
    8388608: 67108864,
    9437184: 67174656,
    10485760: 65792,
    11534336: 67174404,
    12582912: 67109124,
    13631488: 65536,
    14680064: 4,
    15728640: 256,
    524288: 67174656,
    1572864: 67174404,
    2621440: 0,
    3670016: 67109120,
    4718592: 67108868,
    5767168: 65536,
    6815744: 65540,
    7864320: 260,
    8912896: 4,
    9961472: 256,
    11010048: 67174400,
    12058624: 65796,
    13107200: 65792,
    14155776: 67109124,
    15204352: 67174660,
    16252928: 67108864,
    16777216: 67174656,
    17825792: 65540,
    18874368: 65536,
    19922944: 67109120,
    20971520: 256,
    22020096: 67174660,
    23068672: 67108868,
    24117248: 0,
    25165824: 67109124,
    26214400: 67108864,
    27262976: 4,
    28311552: 65792,
    29360128: 67174400,
    30408704: 260,
    31457280: 65796,
    32505856: 67174404,
    17301504: 67108864,
    18350080: 260,
    19398656: 67174656,
    20447232: 0,
    21495808: 65540,
    22544384: 67109120,
    23592960: 256,
    24641536: 67174404,
    25690112: 65536,
    26738688: 67174660,
    27787264: 65796,
    28835840: 67108868,
    29884416: 67109124,
    30932992: 67174400,
    31981568: 4,
    33030144: 65792
  },
  {
    0: 2151682048,
    65536: 2147487808,
    131072: 4198464,
    196608: 2151677952,
    262144: 0,
    327680: 4198400,
    393216: 2147483712,
    458752: 4194368,
    524288: 2147483648,
    589824: 4194304,
    655360: 64,
    720896: 2147487744,
    786432: 2151678016,
    851968: 4160,
    917504: 4096,
    983040: 2151682112,
    32768: 2147487808,
    98304: 64,
    163840: 2151678016,
    229376: 2147487744,
    294912: 4198400,
    360448: 2151682112,
    425984: 0,
    491520: 2151677952,
    557056: 4096,
    622592: 2151682048,
    688128: 4194304,
    753664: 4160,
    819200: 2147483648,
    884736: 4194368,
    950272: 4198464,
    1015808: 2147483712,
    1048576: 4194368,
    1114112: 4198400,
    1179648: 2147483712,
    1245184: 0,
    1310720: 4160,
    1376256: 2151678016,
    1441792: 2151682048,
    1507328: 2147487808,
    1572864: 2151682112,
    1638400: 2147483648,
    1703936: 2151677952,
    1769472: 4198464,
    1835008: 2147487744,
    1900544: 4194304,
    1966080: 64,
    2031616: 4096,
    1081344: 2151677952,
    1146880: 2151682112,
    1212416: 0,
    1277952: 4198400,
    1343488: 4194368,
    1409024: 2147483648,
    1474560: 2147487808,
    1540096: 64,
    1605632: 2147483712,
    1671168: 4096,
    1736704: 2147487744,
    1802240: 2151678016,
    1867776: 4160,
    1933312: 2151682048,
    1998848: 4194304,
    2064384: 4198464
  },
  {
    0: 128,
    4096: 17039360,
    8192: 262144,
    12288: 536870912,
    16384: 537133184,
    20480: 16777344,
    24576: 553648256,
    28672: 262272,
    32768: 16777216,
    36864: 537133056,
    40960: 536871040,
    45056: 553910400,
    49152: 553910272,
    53248: 0,
    57344: 17039488,
    61440: 553648128,
    2048: 17039488,
    6144: 553648256,
    10240: 128,
    14336: 17039360,
    18432: 262144,
    22528: 537133184,
    26624: 553910272,
    30720: 536870912,
    34816: 537133056,
    38912: 0,
    43008: 553910400,
    47104: 16777344,
    51200: 536871040,
    55296: 553648128,
    59392: 16777216,
    63488: 262272,
    65536: 262144,
    69632: 128,
    73728: 536870912,
    77824: 553648256,
    81920: 16777344,
    86016: 553910272,
    90112: 537133184,
    94208: 16777216,
    98304: 553910400,
    102400: 553648128,
    106496: 17039360,
    110592: 537133056,
    114688: 262272,
    118784: 536871040,
    122880: 0,
    126976: 17039488,
    67584: 553648256,
    71680: 16777216,
    75776: 17039360,
    79872: 537133184,
    83968: 536870912,
    88064: 17039488,
    92160: 128,
    96256: 553910272,
    100352: 262272,
    104448: 553910400,
    108544: 0,
    112640: 553648128,
    116736: 16777344,
    120832: 262144,
    124928: 537133056,
    129024: 536871040
  },
  {
    0: 268435464,
    256: 8192,
    512: 270532608,
    768: 270540808,
    1024: 268443648,
    1280: 2097152,
    1536: 2097160,
    1792: 268435456,
    2048: 0,
    2304: 268443656,
    2560: 2105344,
    2816: 8,
    3072: 270532616,
    3328: 2105352,
    3584: 8200,
    3840: 270540800,
    128: 270532608,
    384: 270540808,
    640: 8,
    896: 2097152,
    1152: 2105352,
    1408: 268435464,
    1664: 268443648,
    1920: 8200,
    2176: 2097160,
    2432: 8192,
    2688: 268443656,
    2944: 270532616,
    3200: 0,
    3456: 270540800,
    3712: 2105344,
    3968: 268435456,
    4096: 268443648,
    4352: 270532616,
    4608: 270540808,
    4864: 8200,
    5120: 2097152,
    5376: 268435456,
    5632: 268435464,
    5888: 2105344,
    6144: 2105352,
    6400: 0,
    6656: 8,
    6912: 270532608,
    7168: 8192,
    7424: 268443656,
    7680: 270540800,
    7936: 2097160,
    4224: 8,
    4480: 2105344,
    4736: 2097152,
    4992: 268435464,
    5248: 268443648,
    5504: 8200,
    5760: 270540808,
    6016: 270532608,
    6272: 270540800,
    6528: 270532616,
    6784: 8192,
    7040: 2105352,
    7296: 2097160,
    7552: 0,
    7808: 268435456,
    8064: 268443656
  },
  {
    0: 1048576,
    16: 33555457,
    32: 1024,
    48: 1049601,
    64: 34604033,
    80: 0,
    96: 1,
    112: 34603009,
    128: 33555456,
    144: 1048577,
    160: 33554433,
    176: 34604032,
    192: 34603008,
    208: 1025,
    224: 1049600,
    240: 33554432,
    8: 34603009,
    24: 0,
    40: 33555457,
    56: 34604032,
    72: 1048576,
    88: 33554433,
    104: 33554432,
    120: 1025,
    136: 1049601,
    152: 33555456,
    168: 34603008,
    184: 1048577,
    200: 1024,
    216: 34604033,
    232: 1,
    248: 1049600,
    256: 33554432,
    272: 1048576,
    288: 33555457,
    304: 34603009,
    320: 1048577,
    336: 33555456,
    352: 34604032,
    368: 1049601,
    384: 1025,
    400: 34604033,
    416: 1049600,
    432: 1,
    448: 0,
    464: 34603008,
    480: 33554433,
    496: 1024,
    264: 1049600,
    280: 33555457,
    296: 34603009,
    312: 1,
    328: 33554432,
    344: 1048576,
    360: 1025,
    376: 34604032,
    392: 33554433,
    408: 34603008,
    424: 0,
    440: 34604033,
    456: 1049601,
    472: 1024,
    488: 33555456,
    504: 1048577
  },
  {
    0: 134219808,
    1: 131072,
    2: 134217728,
    3: 32,
    4: 131104,
    5: 134350880,
    6: 134350848,
    7: 2048,
    8: 134348800,
    9: 134219776,
    10: 133120,
    11: 134348832,
    12: 2080,
    13: 0,
    14: 134217760,
    15: 133152,
    2147483648: 2048,
    2147483649: 134350880,
    2147483650: 134219808,
    2147483651: 134217728,
    2147483652: 134348800,
    2147483653: 133120,
    2147483654: 133152,
    2147483655: 32,
    2147483656: 134217760,
    2147483657: 2080,
    2147483658: 131104,
    2147483659: 134350848,
    2147483660: 0,
    2147483661: 134348832,
    2147483662: 134219776,
    2147483663: 131072,
    16: 133152,
    17: 134350848,
    18: 32,
    19: 2048,
    20: 134219776,
    21: 134217760,
    22: 134348832,
    23: 131072,
    24: 0,
    25: 131104,
    26: 134348800,
    27: 134219808,
    28: 134350880,
    29: 133120,
    30: 2080,
    31: 134217728,
    2147483664: 131072,
    2147483665: 2048,
    2147483666: 134348832,
    2147483667: 133152,
    2147483668: 32,
    2147483669: 134348800,
    2147483670: 134217728,
    2147483671: 134219808,
    2147483672: 134350880,
    2147483673: 134217760,
    2147483674: 134219776,
    2147483675: 0,
    2147483676: 133120,
    2147483677: 2080,
    2147483678: 131104,
    2147483679: 134350848
  }
];
const SBOX_MASK = [
  4160749569,
  528482304,
  33030144,
  2064384,
  129024,
  8064,
  504,
  2147483679
];
function exchangeLR(offset, mask) {
  const t = (this._lBlock >>> offset ^ this._rBlock) & mask;
  this._rBlock ^= t;
  this._lBlock ^= t << offset;
}
function exchangeRL(offset, mask) {
  const t = (this._rBlock >>> offset ^ this._lBlock) & mask;
  this._lBlock ^= t;
  this._rBlock ^= t << offset;
}
class DESAlgo extends BlockCipher {
  _doReset() {
    const key = this._key;
    const keyWords = key.words;
    const keyBits = [];
    for (let i = 0; i < 56; i += 1) {
      const keyBitPos = PC1[i] - 1;
      keyBits[i] = keyWords[keyBitPos >>> 5] >>> 31 - keyBitPos % 32 & 1;
    }
    this._subKeys = [];
    const subKeys = this._subKeys;
    for (let nSubKey = 0; nSubKey < 16; nSubKey += 1) {
      subKeys[nSubKey] = [];
      const subKey = subKeys[nSubKey];
      const bitShift = BIT_SHIFTS[nSubKey];
      for (let i = 0; i < 24; i += 1) {
        subKey[i / 6 | 0] |= keyBits[(PC2[i] - 1 + bitShift) % 28] << 31 - i % 6;
        subKey[4 + (i / 6 | 0)] |= keyBits[28 + (PC2[i + 24] - 1 + bitShift) % 28] << 31 - i % 6;
      }
      subKey[0] = subKey[0] << 1 | subKey[0] >>> 31;
      for (let i = 1; i < 7; i += 1) {
        subKey[i] >>>= (i - 1) * 4 + 3;
      }
      subKey[7] = subKey[7] << 5 | subKey[7] >>> 27;
    }
    this._invSubKeys = [];
    const invSubKeys = this._invSubKeys;
    for (let i = 0; i < 16; i += 1) {
      invSubKeys[i] = subKeys[15 - i];
    }
  }
  encryptBlock(M, offset) {
    this._doCryptBlock(M, offset, this._subKeys);
  }
  decryptBlock(M, offset) {
    this._doCryptBlock(M, offset, this._invSubKeys);
  }
  _doCryptBlock(M, offset, subKeys) {
    const _M = M;
    this._lBlock = M[offset];
    this._rBlock = M[offset + 1];
    exchangeLR.call(this, 4, 252645135);
    exchangeLR.call(this, 16, 65535);
    exchangeRL.call(this, 2, 858993459);
    exchangeRL.call(this, 8, 16711935);
    exchangeLR.call(this, 1, 1431655765);
    for (let round = 0; round < 16; round += 1) {
      const subKey = subKeys[round];
      const lBlock = this._lBlock;
      const rBlock = this._rBlock;
      let f = 0;
      for (let i = 0; i < 8; i += 1) {
        f |= SBOX_P[i][((rBlock ^ subKey[i]) & SBOX_MASK[i]) >>> 0];
      }
      this._lBlock = rBlock;
      this._rBlock = lBlock ^ f;
    }
    const t = this._lBlock;
    this._lBlock = this._rBlock;
    this._rBlock = t;
    exchangeLR.call(this, 1, 1431655765);
    exchangeRL.call(this, 8, 16711935);
    exchangeRL.call(this, 2, 858993459);
    exchangeLR.call(this, 16, 65535);
    exchangeLR.call(this, 4, 252645135);
    _M[offset] = this._lBlock;
    _M[offset + 1] = this._rBlock;
  }
}
DESAlgo.keySize = 64 / 32;
DESAlgo.ivSize = 64 / 32;
DESAlgo.blockSize = 64 / 32;
const DES = BlockCipher._createHelper(DESAlgo);
class TripleDESAlgo extends BlockCipher {
  _doReset() {
    const key = this._key;
    const keyWords = key.words;
    if (keyWords.length !== 2 && keyWords.length !== 4 && keyWords.length < 6) {
      throw new Error("Invalid key length - 3DES requires the key length to be 64, 128, 192 or >192.");
    }
    const key1 = keyWords.slice(0, 2);
    const key2 = keyWords.length < 4 ? keyWords.slice(0, 2) : keyWords.slice(2, 4);
    const key3 = keyWords.length < 6 ? keyWords.slice(0, 2) : keyWords.slice(4, 6);
    this._des1 = DESAlgo.createEncryptor(WordArray.create(key1));
    this._des2 = DESAlgo.createEncryptor(WordArray.create(key2));
    this._des3 = DESAlgo.createEncryptor(WordArray.create(key3));
  }
  encryptBlock(M, offset) {
    this._des1.encryptBlock(M, offset);
    this._des2.decryptBlock(M, offset);
    this._des3.encryptBlock(M, offset);
  }
  decryptBlock(M, offset) {
    this._des3.decryptBlock(M, offset);
    this._des2.encryptBlock(M, offset);
    this._des1.decryptBlock(M, offset);
  }
}
TripleDESAlgo.keySize = 192 / 32;
TripleDESAlgo.ivSize = 64 / 32;
TripleDESAlgo.blockSize = 64 / 32;
const TripleDES = BlockCipher._createHelper(TripleDESAlgo);
const S$1 = [];
const C_$1 = [];
const G$1 = [];
function nextState$1() {
  const X = this._X;
  const C = this._C;
  for (let i = 0; i < 8; i += 1) {
    C_$1[i] = C[i];
  }
  C[0] = C[0] + 1295307597 + this._b | 0;
  C[1] = C[1] + 3545052371 + (C[0] >>> 0 < C_$1[0] >>> 0 ? 1 : 0) | 0;
  C[2] = C[2] + 886263092 + (C[1] >>> 0 < C_$1[1] >>> 0 ? 1 : 0) | 0;
  C[3] = C[3] + 1295307597 + (C[2] >>> 0 < C_$1[2] >>> 0 ? 1 : 0) | 0;
  C[4] = C[4] + 3545052371 + (C[3] >>> 0 < C_$1[3] >>> 0 ? 1 : 0) | 0;
  C[5] = C[5] + 886263092 + (C[4] >>> 0 < C_$1[4] >>> 0 ? 1 : 0) | 0;
  C[6] = C[6] + 1295307597 + (C[5] >>> 0 < C_$1[5] >>> 0 ? 1 : 0) | 0;
  C[7] = C[7] + 3545052371 + (C[6] >>> 0 < C_$1[6] >>> 0 ? 1 : 0) | 0;
  this._b = C[7] >>> 0 < C_$1[7] >>> 0 ? 1 : 0;
  for (let i = 0; i < 8; i += 1) {
    const gx = X[i] + C[i];
    const ga = gx & 65535;
    const gb = gx >>> 16;
    const gh = ((ga * ga >>> 17) + ga * gb >>> 15) + gb * gb;
    const gl = ((gx & 4294901760) * gx | 0) + ((gx & 65535) * gx | 0);
    G$1[i] = gh ^ gl;
  }
  X[0] = G$1[0] + (G$1[7] << 16 | G$1[7] >>> 16) + (G$1[6] << 16 | G$1[6] >>> 16) | 0;
  X[1] = G$1[1] + (G$1[0] << 8 | G$1[0] >>> 24) + G$1[7] | 0;
  X[2] = G$1[2] + (G$1[1] << 16 | G$1[1] >>> 16) + (G$1[0] << 16 | G$1[0] >>> 16) | 0;
  X[3] = G$1[3] + (G$1[2] << 8 | G$1[2] >>> 24) + G$1[1] | 0;
  X[4] = G$1[4] + (G$1[3] << 16 | G$1[3] >>> 16) + (G$1[2] << 16 | G$1[2] >>> 16) | 0;
  X[5] = G$1[5] + (G$1[4] << 8 | G$1[4] >>> 24) + G$1[3] | 0;
  X[6] = G$1[6] + (G$1[5] << 16 | G$1[5] >>> 16) + (G$1[4] << 16 | G$1[4] >>> 16) | 0;
  X[7] = G$1[7] + (G$1[6] << 8 | G$1[6] >>> 24) + G$1[5] | 0;
}
class RabbitAlgo extends StreamCipher {
  constructor(...args) {
    super(...args);
    this.blockSize = 128 / 32;
    this.ivSize = 64 / 32;
  }
  _doReset() {
    const K2 = this._key.words;
    const { iv } = this.cfg;
    for (let i = 0; i < 4; i += 1) {
      K2[i] = (K2[i] << 8 | K2[i] >>> 24) & 16711935 | (K2[i] << 24 | K2[i] >>> 8) & 4278255360;
    }
    this._X = [
      K2[0],
      K2[3] << 16 | K2[2] >>> 16,
      K2[1],
      K2[0] << 16 | K2[3] >>> 16,
      K2[2],
      K2[1] << 16 | K2[0] >>> 16,
      K2[3],
      K2[2] << 16 | K2[1] >>> 16
    ];
    const X = this._X;
    this._C = [
      K2[2] << 16 | K2[2] >>> 16,
      K2[0] & 4294901760 | K2[1] & 65535,
      K2[3] << 16 | K2[3] >>> 16,
      K2[1] & 4294901760 | K2[2] & 65535,
      K2[0] << 16 | K2[0] >>> 16,
      K2[2] & 4294901760 | K2[3] & 65535,
      K2[1] << 16 | K2[1] >>> 16,
      K2[3] & 4294901760 | K2[0] & 65535
    ];
    const C = this._C;
    this._b = 0;
    for (let i = 0; i < 4; i += 1) {
      nextState$1.call(this);
    }
    for (let i = 0; i < 8; i += 1) {
      C[i] ^= X[i + 4 & 7];
    }
    if (iv) {
      const IV = iv.words;
      const IV_0 = IV[0];
      const IV_1 = IV[1];
      const i0 = (IV_0 << 8 | IV_0 >>> 24) & 16711935 | (IV_0 << 24 | IV_0 >>> 8) & 4278255360;
      const i2 = (IV_1 << 8 | IV_1 >>> 24) & 16711935 | (IV_1 << 24 | IV_1 >>> 8) & 4278255360;
      const i1 = i0 >>> 16 | i2 & 4294901760;
      const i3 = i2 << 16 | i0 & 65535;
      C[0] ^= i0;
      C[1] ^= i1;
      C[2] ^= i2;
      C[3] ^= i3;
      C[4] ^= i0;
      C[5] ^= i1;
      C[6] ^= i2;
      C[7] ^= i3;
      for (let i = 0; i < 4; i += 1) {
        nextState$1.call(this);
      }
    }
  }
  _doProcessBlock(M, offset) {
    const _M = M;
    const X = this._X;
    nextState$1.call(this);
    S$1[0] = X[0] ^ X[5] >>> 16 ^ X[3] << 16;
    S$1[1] = X[2] ^ X[7] >>> 16 ^ X[5] << 16;
    S$1[2] = X[4] ^ X[1] >>> 16 ^ X[7] << 16;
    S$1[3] = X[6] ^ X[3] >>> 16 ^ X[1] << 16;
    for (let i = 0; i < 4; i += 1) {
      S$1[i] = (S$1[i] << 8 | S$1[i] >>> 24) & 16711935 | (S$1[i] << 24 | S$1[i] >>> 8) & 4278255360;
      _M[offset + i] ^= S$1[i];
    }
  }
}
const Rabbit = StreamCipher._createHelper(RabbitAlgo);
const S = [];
const C_ = [];
const G = [];
function nextState() {
  const X = this._X;
  const C = this._C;
  for (let i = 0; i < 8; i += 1) {
    C_[i] = C[i];
  }
  C[0] = C[0] + 1295307597 + this._b | 0;
  C[1] = C[1] + 3545052371 + (C[0] >>> 0 < C_[0] >>> 0 ? 1 : 0) | 0;
  C[2] = C[2] + 886263092 + (C[1] >>> 0 < C_[1] >>> 0 ? 1 : 0) | 0;
  C[3] = C[3] + 1295307597 + (C[2] >>> 0 < C_[2] >>> 0 ? 1 : 0) | 0;
  C[4] = C[4] + 3545052371 + (C[3] >>> 0 < C_[3] >>> 0 ? 1 : 0) | 0;
  C[5] = C[5] + 886263092 + (C[4] >>> 0 < C_[4] >>> 0 ? 1 : 0) | 0;
  C[6] = C[6] + 1295307597 + (C[5] >>> 0 < C_[5] >>> 0 ? 1 : 0) | 0;
  C[7] = C[7] + 3545052371 + (C[6] >>> 0 < C_[6] >>> 0 ? 1 : 0) | 0;
  this._b = C[7] >>> 0 < C_[7] >>> 0 ? 1 : 0;
  for (let i = 0; i < 8; i += 1) {
    const gx = X[i] + C[i];
    const ga = gx & 65535;
    const gb = gx >>> 16;
    const gh = ((ga * ga >>> 17) + ga * gb >>> 15) + gb * gb;
    const gl = ((gx & 4294901760) * gx | 0) + ((gx & 65535) * gx | 0);
    G[i] = gh ^ gl;
  }
  X[0] = G[0] + (G[7] << 16 | G[7] >>> 16) + (G[6] << 16 | G[6] >>> 16) | 0;
  X[1] = G[1] + (G[0] << 8 | G[0] >>> 24) + G[7] | 0;
  X[2] = G[2] + (G[1] << 16 | G[1] >>> 16) + (G[0] << 16 | G[0] >>> 16) | 0;
  X[3] = G[3] + (G[2] << 8 | G[2] >>> 24) + G[1] | 0;
  X[4] = G[4] + (G[3] << 16 | G[3] >>> 16) + (G[2] << 16 | G[2] >>> 16) | 0;
  X[5] = G[5] + (G[4] << 8 | G[4] >>> 24) + G[3] | 0;
  X[6] = G[6] + (G[5] << 16 | G[5] >>> 16) + (G[4] << 16 | G[4] >>> 16) | 0;
  X[7] = G[7] + (G[6] << 8 | G[6] >>> 24) + G[5] | 0;
}
class RabbitLegacyAlgo extends StreamCipher {
  constructor(...args) {
    super(...args);
    this.blockSize = 128 / 32;
    this.ivSize = 64 / 32;
  }
  _doReset() {
    const K2 = this._key.words;
    const { iv } = this.cfg;
    this._X = [
      K2[0],
      K2[3] << 16 | K2[2] >>> 16,
      K2[1],
      K2[0] << 16 | K2[3] >>> 16,
      K2[2],
      K2[1] << 16 | K2[0] >>> 16,
      K2[3],
      K2[2] << 16 | K2[1] >>> 16
    ];
    const X = this._X;
    this._C = [
      K2[2] << 16 | K2[2] >>> 16,
      K2[0] & 4294901760 | K2[1] & 65535,
      K2[3] << 16 | K2[3] >>> 16,
      K2[1] & 4294901760 | K2[2] & 65535,
      K2[0] << 16 | K2[0] >>> 16,
      K2[2] & 4294901760 | K2[3] & 65535,
      K2[1] << 16 | K2[1] >>> 16,
      K2[3] & 4294901760 | K2[0] & 65535
    ];
    const C = this._C;
    this._b = 0;
    for (let i = 0; i < 4; i += 1) {
      nextState.call(this);
    }
    for (let i = 0; i < 8; i += 1) {
      C[i] ^= X[i + 4 & 7];
    }
    if (iv) {
      const IV = iv.words;
      const IV_0 = IV[0];
      const IV_1 = IV[1];
      const i0 = (IV_0 << 8 | IV_0 >>> 24) & 16711935 | (IV_0 << 24 | IV_0 >>> 8) & 4278255360;
      const i2 = (IV_1 << 8 | IV_1 >>> 24) & 16711935 | (IV_1 << 24 | IV_1 >>> 8) & 4278255360;
      const i1 = i0 >>> 16 | i2 & 4294901760;
      const i3 = i2 << 16 | i0 & 65535;
      C[0] ^= i0;
      C[1] ^= i1;
      C[2] ^= i2;
      C[3] ^= i3;
      C[4] ^= i0;
      C[5] ^= i1;
      C[6] ^= i2;
      C[7] ^= i3;
      for (let i = 0; i < 4; i += 1) {
        nextState.call(this);
      }
    }
  }
  _doProcessBlock(M, offset) {
    const _M = M;
    const X = this._X;
    nextState.call(this);
    S[0] = X[0] ^ X[5] >>> 16 ^ X[3] << 16;
    S[1] = X[2] ^ X[7] >>> 16 ^ X[5] << 16;
    S[2] = X[4] ^ X[1] >>> 16 ^ X[7] << 16;
    S[3] = X[6] ^ X[3] >>> 16 ^ X[1] << 16;
    for (let i = 0; i < 4; i += 1) {
      S[i] = (S[i] << 8 | S[i] >>> 24) & 16711935 | (S[i] << 24 | S[i] >>> 8) & 4278255360;
      _M[offset + i] ^= S[i];
    }
  }
}
const RabbitLegacy = StreamCipher._createHelper(RabbitLegacyAlgo);
function generateKeystreamWord() {
  const S2 = this._S;
  let i = this._i;
  let j = this._j;
  let keystreamWord = 0;
  for (let n2 = 0; n2 < 4; n2 += 1) {
    i = (i + 1) % 256;
    j = (j + S2[i]) % 256;
    const t = S2[i];
    S2[i] = S2[j];
    S2[j] = t;
    keystreamWord |= S2[(S2[i] + S2[j]) % 256] << 24 - n2 * 8;
  }
  this._i = i;
  this._j = j;
  return keystreamWord;
}
class RC4Algo extends StreamCipher {
  _doReset() {
    const key = this._key;
    const keyWords = key.words;
    const keySigBytes = key.sigBytes;
    this._S = [];
    const S2 = this._S;
    for (let i = 0; i < 256; i += 1) {
      S2[i] = i;
    }
    for (let i = 0, j = 0; i < 256; i += 1) {
      const keyByteIndex = i % keySigBytes;
      const keyByte = keyWords[keyByteIndex >>> 2] >>> 24 - keyByteIndex % 4 * 8 & 255;
      j = (j + S2[i] + keyByte) % 256;
      const t = S2[i];
      S2[i] = S2[j];
      S2[j] = t;
    }
    this._j = 0;
    this._i = this._j;
  }
  _doProcessBlock(M, offset) {
    const _M = M;
    _M[offset] ^= generateKeystreamWord.call(this);
  }
}
RC4Algo.keySize = 256 / 32;
RC4Algo.ivSize = 0;
const RC4 = StreamCipher._createHelper(RC4Algo);
class RC4DropAlgo extends RC4Algo {
  constructor(...args) {
    super(...args);
    Object.assign(this.cfg, { drop: 192 });
  }
  _doReset() {
    super._doReset.call(this);
    for (let i = this.cfg.drop; i > 0; i -= 1) {
      generateKeystreamWord.call(this);
    }
  }
}
const RC4Drop = StreamCipher._createHelper(RC4DropAlgo);
function generateKeystreamAndEncrypt(words, offset, blockSize, cipher) {
  const _words = words;
  let keystream;
  const iv = this._iv;
  if (iv) {
    keystream = iv.slice(0);
    this._iv = void 0;
  } else {
    keystream = this._prevBlock;
  }
  cipher.encryptBlock(keystream, 0);
  for (let i = 0; i < blockSize; i += 1) {
    _words[offset + i] ^= keystream[i];
  }
}
class CFB extends BlockCipherMode {
}
CFB.Encryptor = class extends CFB {
  processBlock(words, offset) {
    const cipher = this._cipher;
    const { blockSize } = cipher;
    generateKeystreamAndEncrypt.call(this, words, offset, blockSize, cipher);
    this._prevBlock = words.slice(offset, offset + blockSize);
  }
};
CFB.Decryptor = class extends CFB {
  processBlock(words, offset) {
    const cipher = this._cipher;
    const { blockSize } = cipher;
    const thisBlock = words.slice(offset, offset + blockSize);
    generateKeystreamAndEncrypt.call(this, words, offset, blockSize, cipher);
    this._prevBlock = thisBlock;
  }
};
class CTR extends BlockCipherMode {
}
CTR.Encryptor = class extends CTR {
  processBlock(words, offset) {
    const _words = words;
    const cipher = this._cipher;
    const { blockSize } = cipher;
    const iv = this._iv;
    let counter = this._counter;
    if (iv) {
      this._counter = iv.slice(0);
      counter = this._counter;
      this._iv = void 0;
    }
    const keystream = counter.slice(0);
    cipher.encryptBlock(keystream, 0);
    counter[blockSize - 1] = counter[blockSize - 1] + 1 | 0;
    for (let i = 0; i < blockSize; i += 1) {
      _words[offset + i] ^= keystream[i];
    }
  }
};
CTR.Decryptor = CTR.Encryptor;
const incWord = (word) => {
  let _word = word;
  if ((word >> 24 & 255) === 255) {
    let b1 = word >> 16 & 255;
    let b2 = word >> 8 & 255;
    let b3 = word & 255;
    if (b1 === 255) {
      b1 = 0;
      if (b2 === 255) {
        b2 = 0;
        if (b3 === 255) {
          b3 = 0;
        } else {
          b3 += 1;
        }
      } else {
        b2 += 1;
      }
    } else {
      b1 += 1;
    }
    _word = 0;
    _word += b1 << 16;
    _word += b2 << 8;
    _word += b3;
  } else {
    _word += 1 << 24;
  }
  return _word;
};
const incCounter = (counter) => {
  const _counter = counter;
  _counter[0] = incWord(_counter[0]);
  if (_counter[0] === 0) {
    _counter[1] = incWord(_counter[1]);
  }
  return _counter;
};
/** @preserve
 * Counter block mode compatible with  Dr Brian Gladman fileenc.c
 * derived from CryptoJS.mode.CTR
 * Jan Hruby jhruby.web@gmail.com
 */
class CTRGladman extends BlockCipherMode {
}
CTRGladman.Encryptor = class extends CTRGladman {
  processBlock(words, offset) {
    const _words = words;
    const cipher = this._cipher;
    const { blockSize } = cipher;
    const iv = this._iv;
    let counter = this._counter;
    if (iv) {
      this._counter = iv.slice(0);
      counter = this._counter;
      this._iv = void 0;
    }
    incCounter(counter);
    const keystream = counter.slice(0);
    cipher.encryptBlock(keystream, 0);
    for (let i = 0; i < blockSize; i += 1) {
      _words[offset + i] ^= keystream[i];
    }
  }
};
CTRGladman.Decryptor = CTRGladman.Encryptor;
class ECB extends BlockCipherMode {
}
ECB.Encryptor = class extends ECB {
  processBlock(words, offset) {
    this._cipher.encryptBlock(words, offset);
  }
};
ECB.Decryptor = class extends ECB {
  processBlock(words, offset) {
    this._cipher.decryptBlock(words, offset);
  }
};
class OFB extends BlockCipherMode {
}
OFB.Encryptor = class extends OFB {
  processBlock(words, offset) {
    const _words = words;
    const cipher = this._cipher;
    const { blockSize } = cipher;
    const iv = this._iv;
    let keystream = this._keystream;
    if (iv) {
      this._keystream = iv.slice(0);
      keystream = this._keystream;
      this._iv = void 0;
    }
    cipher.encryptBlock(keystream, 0);
    for (let i = 0; i < blockSize; i += 1) {
      _words[offset + i] ^= keystream[i];
    }
  }
};
OFB.Decryptor = OFB.Encryptor;
const AnsiX923 = {
  pad(data, blockSize) {
    const _data = data;
    const dataSigBytes = _data.sigBytes;
    const blockSizeBytes = blockSize * 4;
    const nPaddingBytes = blockSizeBytes - dataSigBytes % blockSizeBytes;
    const lastBytePos = dataSigBytes + nPaddingBytes - 1;
    _data.clamp();
    _data.words[lastBytePos >>> 2] |= nPaddingBytes << 24 - lastBytePos % 4 * 8;
    _data.sigBytes += nPaddingBytes;
  },
  unpad(data) {
    const _data = data;
    const nPaddingBytes = _data.words[_data.sigBytes - 1 >>> 2] & 255;
    _data.sigBytes -= nPaddingBytes;
  }
};
const Iso10126 = {
  pad(data, blockSize) {
    const blockSizeBytes = blockSize * 4;
    const nPaddingBytes = blockSizeBytes - data.sigBytes % blockSizeBytes;
    data.concat(WordArray.random(nPaddingBytes - 1)).concat(WordArray.create([nPaddingBytes << 24], 1));
  },
  unpad(data) {
    const _data = data;
    const nPaddingBytes = _data.words[_data.sigBytes - 1 >>> 2] & 255;
    _data.sigBytes -= nPaddingBytes;
  }
};
const ZeroPadding = {
  pad(data, blockSize) {
    const _data = data;
    const blockSizeBytes = blockSize * 4;
    _data.clamp();
    _data.sigBytes += blockSizeBytes - (data.sigBytes % blockSizeBytes || blockSizeBytes);
  },
  unpad(data) {
    const _data = data;
    const dataWords = _data.words;
    for (let i = _data.sigBytes - 1; i >= 0; i -= 1) {
      if (dataWords[i >>> 2] >>> 24 - i % 4 * 8 & 255) {
        _data.sigBytes = i + 1;
        break;
      }
    }
  }
};
const Iso97971 = {
  pad(data, blockSize) {
    data.concat(WordArray.create([2147483648], 1));
    ZeroPadding.pad(data, blockSize);
  },
  unpad(data) {
    const _data = data;
    ZeroPadding.unpad(_data);
    _data.sigBytes -= 1;
  }
};
const NoPadding = {
  pad() {
  },
  unpad() {
  }
};
const HexFormatter = {
  /**
   * Converts the ciphertext of a cipher params object to a hexadecimally encoded string.
   *
   * @param {CipherParams} cipherParams The cipher params object.
   *
   * @return {string} The hexadecimally encoded string.
   *
   * @static
   *
   * @example
   *
   *     var hexString = CryptoJS.format.Hex.stringify(cipherParams);
   */
  stringify(cipherParams) {
    return cipherParams.ciphertext.toString(Hex);
  },
  /**
   * Converts a hexadecimally encoded ciphertext string to a cipher params object.
   *
   * @param {string} input The hexadecimally encoded string.
   *
   * @return {CipherParams} The cipher params object.
   *
   * @static
   *
   * @example
   *
   *     var cipherParams = CryptoJS.format.Hex.parse(hexString);
   */
  parse(input) {
    const ciphertext = Hex.parse(input);
    return CipherParams.create({ ciphertext });
  }
};
const CryptoEs = {
  lib: {
    Base,
    WordArray,
    BufferedBlockAlgorithm,
    Hasher,
    Cipher,
    StreamCipher,
    BlockCipherMode,
    BlockCipher,
    CipherParams,
    SerializableCipher,
    PasswordBasedCipher
  },
  x64: {
    Word: X64Word,
    WordArray: X64WordArray
  },
  enc: {
    Hex,
    Latin1,
    Utf8,
    Utf16,
    Utf16BE,
    Utf16LE,
    Base64
  },
  algo: {
    HMAC,
    MD5: MD5Algo,
    SHA1: SHA1Algo,
    SHA224: SHA224Algo,
    SHA256: SHA256Algo,
    SHA384: SHA384Algo,
    SHA512: SHA512Algo,
    SHA3: SHA3Algo,
    RIPEMD160: RIPEMD160Algo,
    PBKDF2: PBKDF2Algo,
    EvpKDF: EvpKDFAlgo,
    AES: AESAlgo,
    DES: DESAlgo,
    TripleDES: TripleDESAlgo,
    Rabbit: RabbitAlgo,
    RabbitLegacy: RabbitLegacyAlgo,
    RC4: RC4Algo,
    RC4Drop: RC4DropAlgo
  },
  mode: {
    CBC,
    CFB,
    CTR,
    CTRGladman,
    ECB,
    OFB
  },
  pad: {
    Pkcs7,
    AnsiX923,
    Iso10126,
    Iso97971,
    NoPadding,
    ZeroPadding
  },
  format: {
    OpenSSL: OpenSSLFormatter,
    Hex: HexFormatter
  },
  kdf: {
    OpenSSL: OpenSSLKdf
  },
  MD5,
  HmacMD5,
  SHA1,
  HmacSHA1,
  SHA224,
  HmacSHA224,
  SHA256,
  HmacSHA256,
  SHA384,
  HmacSHA384,
  SHA512,
  HmacSHA512,
  SHA3,
  HmacSHA3,
  RIPEMD160,
  HmacRIPEMD160,
  PBKDF2,
  EvpKDF,
  AES,
  DES,
  TripleDES,
  Rabbit,
  RabbitLegacy,
  RC4,
  RC4Drop
};
let nanoid = (size = 21) => crypto.getRandomValues(new Uint8Array(size)).reduce((id, byte) => {
  byte &= 63;
  if (byte < 36) {
    id += byte.toString(36);
  } else if (byte < 62) {
    id += (byte - 26).toString(36).toUpperCase();
  } else if (byte > 62) {
    id += "-";
  } else {
    id += "_";
  }
  return id;
}, "");
function isFunction$1(x2) {
  return typeof x2 === "function";
}
var _enable_super_gross_mode_that_will_cause_bad_things$1 = false;
var config$1 = {
  Promise: void 0,
  set useDeprecatedSynchronousErrorHandling(value) {
    if (value) {
      var error = /* @__PURE__ */ new Error();
      /* @__PURE__ */ console.warn("DEPRECATED! RxJS was set to use deprecated synchronous error handling behavior by code at: \n" + error.stack);
    }
    _enable_super_gross_mode_that_will_cause_bad_things$1 = value;
  },
  get useDeprecatedSynchronousErrorHandling() {
    return _enable_super_gross_mode_that_will_cause_bad_things$1;
  }
};
function hostReportError$1(err) {
  setTimeout(function() {
    throw err;
  }, 0);
}
var empty$2 = {
  closed: true,
  next: function(value) {
  },
  error: function(err) {
    if (config$1.useDeprecatedSynchronousErrorHandling) {
      throw err;
    } else {
      hostReportError$1(err);
    }
  },
  complete: function() {
  }
};
var isArray$1 = /* @__PURE__ */ function() {
  return Array.isArray || function(x2) {
    return x2 && typeof x2.length === "number";
  };
}();
function isObject$1(x2) {
  return x2 !== null && typeof x2 === "object";
}
var UnsubscriptionErrorImpl$1 = /* @__PURE__ */ function() {
  function UnsubscriptionErrorImpl2(errors) {
    Error.call(this);
    this.message = errors ? errors.length + " errors occurred during unsubscription:\n" + errors.map(function(err, i) {
      return i + 1 + ") " + err.toString();
    }).join("\n  ") : "";
    this.name = "UnsubscriptionError";
    this.errors = errors;
    return this;
  }
  UnsubscriptionErrorImpl2.prototype = /* @__PURE__ */ Object.create(Error.prototype);
  return UnsubscriptionErrorImpl2;
}();
var UnsubscriptionError$1 = UnsubscriptionErrorImpl$1;
var Subscription$1 = /* @__PURE__ */ function() {
  function Subscription2(unsubscribe2) {
    this.closed = false;
    this._parentOrParents = null;
    this._subscriptions = null;
    if (unsubscribe2) {
      this._ctorUnsubscribe = true;
      this._unsubscribe = unsubscribe2;
    }
  }
  Subscription2.prototype.unsubscribe = function() {
    var errors;
    if (this.closed) {
      return;
    }
    var _a = this, _parentOrParents = _a._parentOrParents, _ctorUnsubscribe = _a._ctorUnsubscribe, _unsubscribe = _a._unsubscribe, _subscriptions = _a._subscriptions;
    this.closed = true;
    this._parentOrParents = null;
    this._subscriptions = null;
    if (_parentOrParents instanceof Subscription2) {
      _parentOrParents.remove(this);
    } else if (_parentOrParents !== null) {
      for (var index = 0; index < _parentOrParents.length; ++index) {
        var parent_1 = _parentOrParents[index];
        parent_1.remove(this);
      }
    }
    if (isFunction$1(_unsubscribe)) {
      if (_ctorUnsubscribe) {
        this._unsubscribe = void 0;
      }
      try {
        _unsubscribe.call(this);
      } catch (e) {
        errors = e instanceof UnsubscriptionError$1 ? flattenUnsubscriptionErrors$1(e.errors) : [e];
      }
    }
    if (isArray$1(_subscriptions)) {
      var index = -1;
      var len = _subscriptions.length;
      while (++index < len) {
        var sub = _subscriptions[index];
        if (isObject$1(sub)) {
          try {
            sub.unsubscribe();
          } catch (e) {
            errors = errors || [];
            if (e instanceof UnsubscriptionError$1) {
              errors = errors.concat(flattenUnsubscriptionErrors$1(e.errors));
            } else {
              errors.push(e);
            }
          }
        }
      }
    }
    if (errors) {
      throw new UnsubscriptionError$1(errors);
    }
  };
  Subscription2.prototype.add = function(teardown) {
    var subscription = teardown;
    if (!teardown) {
      return Subscription2.EMPTY;
    }
    switch (typeof teardown) {
      case "function":
        subscription = new Subscription2(teardown);
      case "object":
        if (subscription === this || subscription.closed || typeof subscription.unsubscribe !== "function") {
          return subscription;
        } else if (this.closed) {
          subscription.unsubscribe();
          return subscription;
        } else if (!(subscription instanceof Subscription2)) {
          var tmp = subscription;
          subscription = new Subscription2();
          subscription._subscriptions = [tmp];
        }
        break;
      default: {
        throw new Error("unrecognized teardown " + teardown + " added to Subscription.");
      }
    }
    var _parentOrParents = subscription._parentOrParents;
    if (_parentOrParents === null) {
      subscription._parentOrParents = this;
    } else if (_parentOrParents instanceof Subscription2) {
      if (_parentOrParents === this) {
        return subscription;
      }
      subscription._parentOrParents = [_parentOrParents, this];
    } else if (_parentOrParents.indexOf(this) === -1) {
      _parentOrParents.push(this);
    } else {
      return subscription;
    }
    var subscriptions = this._subscriptions;
    if (subscriptions === null) {
      this._subscriptions = [subscription];
    } else {
      subscriptions.push(subscription);
    }
    return subscription;
  };
  Subscription2.prototype.remove = function(subscription) {
    var subscriptions = this._subscriptions;
    if (subscriptions) {
      var subscriptionIndex = subscriptions.indexOf(subscription);
      if (subscriptionIndex !== -1) {
        subscriptions.splice(subscriptionIndex, 1);
      }
    }
  };
  Subscription2.EMPTY = function(empty2) {
    empty2.closed = true;
    return empty2;
  }(new Subscription2());
  return Subscription2;
}();
function flattenUnsubscriptionErrors$1(errors) {
  return errors.reduce(function(errs, err) {
    return errs.concat(err instanceof UnsubscriptionError$1 ? err.errors : err);
  }, []);
}
var rxSubscriber$1 = /* @__PURE__ */ function() {
  return typeof Symbol === "function" ? /* @__PURE__ */ Symbol("rxSubscriber") : "@@rxSubscriber_" + /* @__PURE__ */ Math.random();
}();
var Subscriber$1 = /* @__PURE__ */ function(_super) {
  __extends$1(Subscriber2, _super);
  function Subscriber2(destinationOrNext, error, complete) {
    var _this = _super.call(this) || this;
    _this.syncErrorValue = null;
    _this.syncErrorThrown = false;
    _this.syncErrorThrowable = false;
    _this.isStopped = false;
    switch (arguments.length) {
      case 0:
        _this.destination = empty$2;
        break;
      case 1:
        if (!destinationOrNext) {
          _this.destination = empty$2;
          break;
        }
        if (typeof destinationOrNext === "object") {
          if (destinationOrNext instanceof Subscriber2) {
            _this.syncErrorThrowable = destinationOrNext.syncErrorThrowable;
            _this.destination = destinationOrNext;
            destinationOrNext.add(_this);
          } else {
            _this.syncErrorThrowable = true;
            _this.destination = new SafeSubscriber$1(_this, destinationOrNext);
          }
          break;
        }
      default:
        _this.syncErrorThrowable = true;
        _this.destination = new SafeSubscriber$1(_this, destinationOrNext, error, complete);
        break;
    }
    return _this;
  }
  Subscriber2.prototype[rxSubscriber$1] = function() {
    return this;
  };
  Subscriber2.create = function(next, error, complete) {
    var subscriber = new Subscriber2(next, error, complete);
    subscriber.syncErrorThrowable = false;
    return subscriber;
  };
  Subscriber2.prototype.next = function(value) {
    if (!this.isStopped) {
      this._next(value);
    }
  };
  Subscriber2.prototype.error = function(err) {
    if (!this.isStopped) {
      this.isStopped = true;
      this._error(err);
    }
  };
  Subscriber2.prototype.complete = function() {
    if (!this.isStopped) {
      this.isStopped = true;
      this._complete();
    }
  };
  Subscriber2.prototype.unsubscribe = function() {
    if (this.closed) {
      return;
    }
    this.isStopped = true;
    _super.prototype.unsubscribe.call(this);
  };
  Subscriber2.prototype._next = function(value) {
    this.destination.next(value);
  };
  Subscriber2.prototype._error = function(err) {
    this.destination.error(err);
    this.unsubscribe();
  };
  Subscriber2.prototype._complete = function() {
    this.destination.complete();
    this.unsubscribe();
  };
  Subscriber2.prototype._unsubscribeAndRecycle = function() {
    var _parentOrParents = this._parentOrParents;
    this._parentOrParents = null;
    this.unsubscribe();
    this.closed = false;
    this.isStopped = false;
    this._parentOrParents = _parentOrParents;
    return this;
  };
  return Subscriber2;
}(Subscription$1);
var SafeSubscriber$1 = /* @__PURE__ */ function(_super) {
  __extends$1(SafeSubscriber2, _super);
  function SafeSubscriber2(_parentSubscriber, observerOrNext, error, complete) {
    var _this = _super.call(this) || this;
    _this._parentSubscriber = _parentSubscriber;
    var next;
    var context = _this;
    if (isFunction$1(observerOrNext)) {
      next = observerOrNext;
    } else if (observerOrNext) {
      next = observerOrNext.next;
      error = observerOrNext.error;
      complete = observerOrNext.complete;
      if (observerOrNext !== empty$2) {
        context = Object.create(observerOrNext);
        if (isFunction$1(context.unsubscribe)) {
          _this.add(context.unsubscribe.bind(context));
        }
        context.unsubscribe = _this.unsubscribe.bind(_this);
      }
    }
    _this._context = context;
    _this._next = next;
    _this._error = error;
    _this._complete = complete;
    return _this;
  }
  SafeSubscriber2.prototype.next = function(value) {
    if (!this.isStopped && this._next) {
      var _parentSubscriber = this._parentSubscriber;
      if (!config$1.useDeprecatedSynchronousErrorHandling || !_parentSubscriber.syncErrorThrowable) {
        this.__tryOrUnsub(this._next, value);
      } else if (this.__tryOrSetError(_parentSubscriber, this._next, value)) {
        this.unsubscribe();
      }
    }
  };
  SafeSubscriber2.prototype.error = function(err) {
    if (!this.isStopped) {
      var _parentSubscriber = this._parentSubscriber;
      var useDeprecatedSynchronousErrorHandling = config$1.useDeprecatedSynchronousErrorHandling;
      if (this._error) {
        if (!useDeprecatedSynchronousErrorHandling || !_parentSubscriber.syncErrorThrowable) {
          this.__tryOrUnsub(this._error, err);
          this.unsubscribe();
        } else {
          this.__tryOrSetError(_parentSubscriber, this._error, err);
          this.unsubscribe();
        }
      } else if (!_parentSubscriber.syncErrorThrowable) {
        this.unsubscribe();
        if (useDeprecatedSynchronousErrorHandling) {
          throw err;
        }
        hostReportError$1(err);
      } else {
        if (useDeprecatedSynchronousErrorHandling) {
          _parentSubscriber.syncErrorValue = err;
          _parentSubscriber.syncErrorThrown = true;
        } else {
          hostReportError$1(err);
        }
        this.unsubscribe();
      }
    }
  };
  SafeSubscriber2.prototype.complete = function() {
    var _this = this;
    if (!this.isStopped) {
      var _parentSubscriber = this._parentSubscriber;
      if (this._complete) {
        var wrappedComplete = function() {
          return _this._complete.call(_this._context);
        };
        if (!config$1.useDeprecatedSynchronousErrorHandling || !_parentSubscriber.syncErrorThrowable) {
          this.__tryOrUnsub(wrappedComplete);
          this.unsubscribe();
        } else {
          this.__tryOrSetError(_parentSubscriber, wrappedComplete);
          this.unsubscribe();
        }
      } else {
        this.unsubscribe();
      }
    }
  };
  SafeSubscriber2.prototype.__tryOrUnsub = function(fn, value) {
    try {
      fn.call(this._context, value);
    } catch (err) {
      this.unsubscribe();
      if (config$1.useDeprecatedSynchronousErrorHandling) {
        throw err;
      } else {
        hostReportError$1(err);
      }
    }
  };
  SafeSubscriber2.prototype.__tryOrSetError = function(parent, fn, value) {
    if (!config$1.useDeprecatedSynchronousErrorHandling) {
      throw new Error("bad call");
    }
    try {
      fn.call(this._context, value);
    } catch (err) {
      if (config$1.useDeprecatedSynchronousErrorHandling) {
        parent.syncErrorValue = err;
        parent.syncErrorThrown = true;
        return true;
      } else {
        hostReportError$1(err);
        return true;
      }
    }
    return false;
  };
  SafeSubscriber2.prototype._unsubscribe = function() {
    var _parentSubscriber = this._parentSubscriber;
    this._context = null;
    this._parentSubscriber = null;
    _parentSubscriber.unsubscribe();
  };
  return SafeSubscriber2;
}(Subscriber$1);
function canReportError$1(observer) {
  while (observer) {
    var _a = observer, closed_1 = _a.closed, destination = _a.destination, isStopped = _a.isStopped;
    if (closed_1 || isStopped) {
      return false;
    } else if (destination && destination instanceof Subscriber$1) {
      observer = destination;
    } else {
      observer = null;
    }
  }
  return true;
}
function toSubscriber$1(nextOrObserver, error, complete) {
  if (nextOrObserver) {
    if (nextOrObserver instanceof Subscriber$1) {
      return nextOrObserver;
    }
    if (nextOrObserver[rxSubscriber$1]) {
      return nextOrObserver[rxSubscriber$1]();
    }
  }
  if (!nextOrObserver && !error && !complete) {
    return new Subscriber$1(empty$2);
  }
  return new Subscriber$1(nextOrObserver, error, complete);
}
var observable$1 = /* @__PURE__ */ function() {
  return typeof Symbol === "function" && Symbol.observable || "@@observable";
}();
function identity$1(x2) {
  return x2;
}
function pipeFromArray$1(fns) {
  if (fns.length === 0) {
    return identity$1;
  }
  if (fns.length === 1) {
    return fns[0];
  }
  return function piped(input) {
    return fns.reduce(function(prev, fn) {
      return fn(prev);
    }, input);
  };
}
var Observable$1 = /* @__PURE__ */ function() {
  function Observable2(subscribe2) {
    this._isScalar = false;
    if (subscribe2) {
      this._subscribe = subscribe2;
    }
  }
  Observable2.prototype.lift = function(operator) {
    var observable2 = new Observable2();
    observable2.source = this;
    observable2.operator = operator;
    return observable2;
  };
  Observable2.prototype.subscribe = function(observerOrNext, error, complete) {
    var operator = this.operator;
    var sink = toSubscriber$1(observerOrNext, error, complete);
    if (operator) {
      sink.add(operator.call(sink, this.source));
    } else {
      sink.add(this.source || config$1.useDeprecatedSynchronousErrorHandling && !sink.syncErrorThrowable ? this._subscribe(sink) : this._trySubscribe(sink));
    }
    if (config$1.useDeprecatedSynchronousErrorHandling) {
      if (sink.syncErrorThrowable) {
        sink.syncErrorThrowable = false;
        if (sink.syncErrorThrown) {
          throw sink.syncErrorValue;
        }
      }
    }
    return sink;
  };
  Observable2.prototype._trySubscribe = function(sink) {
    try {
      return this._subscribe(sink);
    } catch (err) {
      if (config$1.useDeprecatedSynchronousErrorHandling) {
        sink.syncErrorThrown = true;
        sink.syncErrorValue = err;
      }
      if (canReportError$1(sink)) {
        sink.error(err);
      } else {
        console.warn(err);
      }
    }
  };
  Observable2.prototype.forEach = function(next, promiseCtor) {
    var _this = this;
    promiseCtor = getPromiseCtor$1(promiseCtor);
    return new promiseCtor(function(resolve, reject) {
      var subscription;
      subscription = _this.subscribe(function(value) {
        try {
          next(value);
        } catch (err) {
          reject(err);
          if (subscription) {
            subscription.unsubscribe();
          }
        }
      }, reject, resolve);
    });
  };
  Observable2.prototype._subscribe = function(subscriber) {
    var source = this.source;
    return source && source.subscribe(subscriber);
  };
  Observable2.prototype[observable$1] = function() {
    return this;
  };
  Observable2.prototype.pipe = function() {
    var operations = [];
    for (var _i = 0; _i < arguments.length; _i++) {
      operations[_i] = arguments[_i];
    }
    if (operations.length === 0) {
      return this;
    }
    return pipeFromArray$1(operations)(this);
  };
  Observable2.prototype.toPromise = function(promiseCtor) {
    var _this = this;
    promiseCtor = getPromiseCtor$1(promiseCtor);
    return new promiseCtor(function(resolve, reject) {
      var value;
      _this.subscribe(function(x2) {
        return value = x2;
      }, function(err) {
        return reject(err);
      }, function() {
        return resolve(value);
      });
    });
  };
  Observable2.create = function(subscribe2) {
    return new Observable2(subscribe2);
  };
  return Observable2;
}();
function getPromiseCtor$1(promiseCtor) {
  if (!promiseCtor) {
    promiseCtor = Promise;
  }
  if (!promiseCtor) {
    throw new Error("no Promise impl found");
  }
  return promiseCtor;
}
var ObjectUnsubscribedErrorImpl = /* @__PURE__ */ function() {
  function ObjectUnsubscribedErrorImpl2() {
    Error.call(this);
    this.message = "object unsubscribed";
    this.name = "ObjectUnsubscribedError";
    return this;
  }
  ObjectUnsubscribedErrorImpl2.prototype = /* @__PURE__ */ Object.create(Error.prototype);
  return ObjectUnsubscribedErrorImpl2;
}();
var ObjectUnsubscribedError = ObjectUnsubscribedErrorImpl;
var SubjectSubscription = /* @__PURE__ */ function(_super) {
  __extends$1(SubjectSubscription2, _super);
  function SubjectSubscription2(subject, subscriber) {
    var _this = _super.call(this) || this;
    _this.subject = subject;
    _this.subscriber = subscriber;
    _this.closed = false;
    return _this;
  }
  SubjectSubscription2.prototype.unsubscribe = function() {
    if (this.closed) {
      return;
    }
    this.closed = true;
    var subject = this.subject;
    var observers = subject.observers;
    this.subject = null;
    if (!observers || observers.length === 0 || subject.isStopped || subject.closed) {
      return;
    }
    var subscriberIndex = observers.indexOf(this.subscriber);
    if (subscriberIndex !== -1) {
      observers.splice(subscriberIndex, 1);
    }
  };
  return SubjectSubscription2;
}(Subscription$1);
var SubjectSubscriber = /* @__PURE__ */ function(_super) {
  __extends$1(SubjectSubscriber2, _super);
  function SubjectSubscriber2(destination) {
    var _this = _super.call(this, destination) || this;
    _this.destination = destination;
    return _this;
  }
  return SubjectSubscriber2;
}(Subscriber$1);
var Subject = /* @__PURE__ */ function(_super) {
  __extends$1(Subject2, _super);
  function Subject2() {
    var _this = _super.call(this) || this;
    _this.observers = [];
    _this.closed = false;
    _this.isStopped = false;
    _this.hasError = false;
    _this.thrownError = null;
    return _this;
  }
  Subject2.prototype[rxSubscriber$1] = function() {
    return new SubjectSubscriber(this);
  };
  Subject2.prototype.lift = function(operator) {
    var subject = new AnonymousSubject(this, this);
    subject.operator = operator;
    return subject;
  };
  Subject2.prototype.next = function(value) {
    if (this.closed) {
      throw new ObjectUnsubscribedError();
    }
    if (!this.isStopped) {
      var observers = this.observers;
      var len = observers.length;
      var copy = observers.slice();
      for (var i = 0; i < len; i++) {
        copy[i].next(value);
      }
    }
  };
  Subject2.prototype.error = function(err) {
    if (this.closed) {
      throw new ObjectUnsubscribedError();
    }
    this.hasError = true;
    this.thrownError = err;
    this.isStopped = true;
    var observers = this.observers;
    var len = observers.length;
    var copy = observers.slice();
    for (var i = 0; i < len; i++) {
      copy[i].error(err);
    }
    this.observers.length = 0;
  };
  Subject2.prototype.complete = function() {
    if (this.closed) {
      throw new ObjectUnsubscribedError();
    }
    this.isStopped = true;
    var observers = this.observers;
    var len = observers.length;
    var copy = observers.slice();
    for (var i = 0; i < len; i++) {
      copy[i].complete();
    }
    this.observers.length = 0;
  };
  Subject2.prototype.unsubscribe = function() {
    this.isStopped = true;
    this.closed = true;
    this.observers = null;
  };
  Subject2.prototype._trySubscribe = function(subscriber) {
    if (this.closed) {
      throw new ObjectUnsubscribedError();
    } else {
      return _super.prototype._trySubscribe.call(this, subscriber);
    }
  };
  Subject2.prototype._subscribe = function(subscriber) {
    if (this.closed) {
      throw new ObjectUnsubscribedError();
    } else if (this.hasError) {
      subscriber.error(this.thrownError);
      return Subscription$1.EMPTY;
    } else if (this.isStopped) {
      subscriber.complete();
      return Subscription$1.EMPTY;
    } else {
      this.observers.push(subscriber);
      return new SubjectSubscription(this, subscriber);
    }
  };
  Subject2.prototype.asObservable = function() {
    var observable2 = new Observable$1();
    observable2.source = this;
    return observable2;
  };
  Subject2.create = function(destination, source) {
    return new AnonymousSubject(destination, source);
  };
  return Subject2;
}(Observable$1);
var AnonymousSubject = /* @__PURE__ */ function(_super) {
  __extends$1(AnonymousSubject2, _super);
  function AnonymousSubject2(destination, source) {
    var _this = _super.call(this) || this;
    _this.destination = destination;
    _this.source = source;
    return _this;
  }
  AnonymousSubject2.prototype.next = function(value) {
    var destination = this.destination;
    if (destination && destination.next) {
      destination.next(value);
    }
  };
  AnonymousSubject2.prototype.error = function(err) {
    var destination = this.destination;
    if (destination && destination.error) {
      this.destination.error(err);
    }
  };
  AnonymousSubject2.prototype.complete = function() {
    var destination = this.destination;
    if (destination && destination.complete) {
      this.destination.complete();
    }
  };
  AnonymousSubject2.prototype._subscribe = function(subscriber) {
    var source = this.source;
    if (source) {
      return this.source.subscribe(subscriber);
    } else {
      return Subscription$1.EMPTY;
    }
  };
  return AnonymousSubject2;
}(Subject);
var Action$1 = /* @__PURE__ */ function(_super) {
  __extends$1(Action2, _super);
  function Action2(scheduler, work) {
    return _super.call(this) || this;
  }
  Action2.prototype.schedule = function(state, delay) {
    return this;
  };
  return Action2;
}(Subscription$1);
var AsyncAction$1 = /* @__PURE__ */ function(_super) {
  __extends$1(AsyncAction2, _super);
  function AsyncAction2(scheduler, work) {
    var _this = _super.call(this, scheduler, work) || this;
    _this.scheduler = scheduler;
    _this.work = work;
    _this.pending = false;
    return _this;
  }
  AsyncAction2.prototype.schedule = function(state, delay) {
    if (delay === void 0) {
      delay = 0;
    }
    if (this.closed) {
      return this;
    }
    this.state = state;
    var id = this.id;
    var scheduler = this.scheduler;
    if (id != null) {
      this.id = this.recycleAsyncId(scheduler, id, delay);
    }
    this.pending = true;
    this.delay = delay;
    this.id = this.id || this.requestAsyncId(scheduler, this.id, delay);
    return this;
  };
  AsyncAction2.prototype.requestAsyncId = function(scheduler, id, delay) {
    if (delay === void 0) {
      delay = 0;
    }
    return setInterval(scheduler.flush.bind(scheduler, this), delay);
  };
  AsyncAction2.prototype.recycleAsyncId = function(scheduler, id, delay) {
    if (delay === void 0) {
      delay = 0;
    }
    if (delay !== null && this.delay === delay && this.pending === false) {
      return id;
    }
    clearInterval(id);
    return void 0;
  };
  AsyncAction2.prototype.execute = function(state, delay) {
    if (this.closed) {
      return new Error("executing a cancelled action");
    }
    this.pending = false;
    var error = this._execute(state, delay);
    if (error) {
      return error;
    } else if (this.pending === false && this.id != null) {
      this.id = this.recycleAsyncId(this.scheduler, this.id, null);
    }
  };
  AsyncAction2.prototype._execute = function(state, delay) {
    var errored = false;
    var errorValue = void 0;
    try {
      this.work(state);
    } catch (e) {
      errored = true;
      errorValue = !!e && e || new Error(e);
    }
    if (errored) {
      this.unsubscribe();
      return errorValue;
    }
  };
  AsyncAction2.prototype._unsubscribe = function() {
    var id = this.id;
    var scheduler = this.scheduler;
    var actions = scheduler.actions;
    var index = actions.indexOf(this);
    this.work = null;
    this.state = null;
    this.pending = false;
    this.scheduler = null;
    if (index !== -1) {
      actions.splice(index, 1);
    }
    if (id != null) {
      this.id = this.recycleAsyncId(scheduler, id, null);
    }
    this.delay = null;
  };
  return AsyncAction2;
}(Action$1);
var Scheduler$1 = /* @__PURE__ */ function() {
  function Scheduler2(SchedulerAction, now) {
    if (now === void 0) {
      now = Scheduler2.now;
    }
    this.SchedulerAction = SchedulerAction;
    this.now = now;
  }
  Scheduler2.prototype.schedule = function(work, delay, state) {
    if (delay === void 0) {
      delay = 0;
    }
    return new this.SchedulerAction(this, work).schedule(state, delay);
  };
  Scheduler2.now = function() {
    return Date.now();
  };
  return Scheduler2;
}();
var AsyncScheduler$1 = /* @__PURE__ */ function(_super) {
  __extends$1(AsyncScheduler2, _super);
  function AsyncScheduler2(SchedulerAction, now) {
    if (now === void 0) {
      now = Scheduler$1.now;
    }
    var _this = _super.call(this, SchedulerAction, function() {
      if (AsyncScheduler2.delegate && AsyncScheduler2.delegate !== _this) {
        return AsyncScheduler2.delegate.now();
      } else {
        return now();
      }
    }) || this;
    _this.actions = [];
    _this.active = false;
    _this.scheduled = void 0;
    return _this;
  }
  AsyncScheduler2.prototype.schedule = function(work, delay, state) {
    if (delay === void 0) {
      delay = 0;
    }
    if (AsyncScheduler2.delegate && AsyncScheduler2.delegate !== this) {
      return AsyncScheduler2.delegate.schedule(work, delay, state);
    } else {
      return _super.prototype.schedule.call(this, work, delay, state);
    }
  };
  AsyncScheduler2.prototype.flush = function(action) {
    var actions = this.actions;
    if (this.active) {
      actions.push(action);
      return;
    }
    var error;
    this.active = true;
    do {
      if (error = action.execute(action.state, action.delay)) {
        break;
      }
    } while (action = actions.shift());
    this.active = false;
    if (error) {
      while (action = actions.shift()) {
        action.unsubscribe();
      }
      throw error;
    }
  };
  return AsyncScheduler2;
}(Scheduler$1);
function isScheduler(value) {
  return value && typeof value.schedule === "function";
}
var subscribeToArray$1 = function(array) {
  return function(subscriber) {
    for (var i = 0, len = array.length; i < len && !subscriber.closed; i++) {
      subscriber.next(array[i]);
    }
    subscriber.complete();
  };
};
function scheduleArray(input, scheduler) {
  return new Observable$1(function(subscriber) {
    var sub = new Subscription$1();
    var i = 0;
    sub.add(scheduler.schedule(function() {
      if (i === input.length) {
        subscriber.complete();
        return;
      }
      subscriber.next(input[i++]);
      if (!subscriber.closed) {
        sub.add(this.schedule());
      }
    }));
    return sub;
  });
}
function fromArray(input, scheduler) {
  if (!scheduler) {
    return new Observable$1(subscribeToArray$1(input));
  } else {
    return scheduleArray(input, scheduler);
  }
}
var asyncScheduler$1 = /* @__PURE__ */ new AsyncScheduler$1(AsyncAction$1);
var async$1 = asyncScheduler$1;
function map(project, thisArg) {
  return function mapOperation(source) {
    if (typeof project !== "function") {
      throw new TypeError("argument is not a function. Are you looking for `mapTo()`?");
    }
    return source.lift(new MapOperator(project, thisArg));
  };
}
var MapOperator = /* @__PURE__ */ function() {
  function MapOperator2(project, thisArg) {
    this.project = project;
    this.thisArg = thisArg;
  }
  MapOperator2.prototype.call = function(subscriber, source) {
    return source.subscribe(new MapSubscriber(subscriber, this.project, this.thisArg));
  };
  return MapOperator2;
}();
var MapSubscriber = /* @__PURE__ */ function(_super) {
  __extends$1(MapSubscriber2, _super);
  function MapSubscriber2(destination, project, thisArg) {
    var _this = _super.call(this, destination) || this;
    _this.project = project;
    _this.count = 0;
    _this.thisArg = thisArg || _this;
    return _this;
  }
  MapSubscriber2.prototype._next = function(value) {
    var result;
    try {
      result = this.project.call(this.thisArg, value, this.count++);
    } catch (err) {
      this.destination.error(err);
      return;
    }
    this.destination.next(result);
  };
  return MapSubscriber2;
}(Subscriber$1);
var subscribeToPromise$1 = function(promise) {
  return function(subscriber) {
    promise.then(function(value) {
      if (!subscriber.closed) {
        subscriber.next(value);
        subscriber.complete();
      }
    }, function(err) {
      return subscriber.error(err);
    }).then(null, hostReportError$1);
    return subscriber;
  };
};
function getSymbolIterator$1() {
  if (typeof Symbol !== "function" || !Symbol.iterator) {
    return "@@iterator";
  }
  return Symbol.iterator;
}
var iterator$1 = /* @__PURE__ */ getSymbolIterator$1();
var subscribeToIterable$1 = function(iterable) {
  return function(subscriber) {
    var iterator2 = iterable[iterator$1]();
    do {
      var item = void 0;
      try {
        item = iterator2.next();
      } catch (err) {
        subscriber.error(err);
        return subscriber;
      }
      if (item.done) {
        subscriber.complete();
        break;
      }
      subscriber.next(item.value);
      if (subscriber.closed) {
        break;
      }
    } while (true);
    if (typeof iterator2.return === "function") {
      subscriber.add(function() {
        if (iterator2.return) {
          iterator2.return();
        }
      });
    }
    return subscriber;
  };
};
var subscribeToObservable$1 = function(obj) {
  return function(subscriber) {
    var obs = obj[observable$1]();
    if (typeof obs.subscribe !== "function") {
      throw new TypeError("Provided object does not correctly implement Symbol.observable");
    } else {
      return obs.subscribe(subscriber);
    }
  };
};
var isArrayLike$1 = function(x2) {
  return x2 && typeof x2.length === "number" && typeof x2 !== "function";
};
function isPromise$1(value) {
  return !!value && typeof value.subscribe !== "function" && typeof value.then === "function";
}
var subscribeTo$1 = function(result) {
  if (!!result && typeof result[observable$1] === "function") {
    return subscribeToObservable$1(result);
  } else if (isArrayLike$1(result)) {
    return subscribeToArray$1(result);
  } else if (isPromise$1(result)) {
    return subscribeToPromise$1(result);
  } else if (!!result && typeof result[iterator$1] === "function") {
    return subscribeToIterable$1(result);
  } else {
    var value = isObject$1(result) ? "an invalid object" : "'" + result + "'";
    var msg = "You provided " + value + " where a stream was expected. You can provide an Observable, Promise, Array, or Iterable.";
    throw new TypeError(msg);
  }
};
function scheduleObservable(input, scheduler) {
  return new Observable$1(function(subscriber) {
    var sub = new Subscription$1();
    sub.add(scheduler.schedule(function() {
      var observable2 = input[observable$1]();
      sub.add(observable2.subscribe({
        next: function(value) {
          sub.add(scheduler.schedule(function() {
            return subscriber.next(value);
          }));
        },
        error: function(err) {
          sub.add(scheduler.schedule(function() {
            return subscriber.error(err);
          }));
        },
        complete: function() {
          sub.add(scheduler.schedule(function() {
            return subscriber.complete();
          }));
        }
      }));
    }));
    return sub;
  });
}
function schedulePromise(input, scheduler) {
  return new Observable$1(function(subscriber) {
    var sub = new Subscription$1();
    sub.add(scheduler.schedule(function() {
      return input.then(function(value) {
        sub.add(scheduler.schedule(function() {
          subscriber.next(value);
          sub.add(scheduler.schedule(function() {
            return subscriber.complete();
          }));
        }));
      }, function(err) {
        sub.add(scheduler.schedule(function() {
          return subscriber.error(err);
        }));
      });
    }));
    return sub;
  });
}
function scheduleIterable(input, scheduler) {
  if (!input) {
    throw new Error("Iterable cannot be null");
  }
  return new Observable$1(function(subscriber) {
    var sub = new Subscription$1();
    var iterator2;
    sub.add(function() {
      if (iterator2 && typeof iterator2.return === "function") {
        iterator2.return();
      }
    });
    sub.add(scheduler.schedule(function() {
      iterator2 = input[iterator$1]();
      sub.add(scheduler.schedule(function() {
        if (subscriber.closed) {
          return;
        }
        var value;
        var done;
        try {
          var result = iterator2.next();
          value = result.value;
          done = result.done;
        } catch (err) {
          subscriber.error(err);
          return;
        }
        if (done) {
          subscriber.complete();
        } else {
          subscriber.next(value);
          this.schedule();
        }
      }));
    }));
    return sub;
  });
}
function isInteropObservable(input) {
  return input && typeof input[observable$1] === "function";
}
function isIterable(input) {
  return input && typeof input[iterator$1] === "function";
}
function scheduled(input, scheduler) {
  if (input != null) {
    if (isInteropObservable(input)) {
      return scheduleObservable(input, scheduler);
    } else if (isPromise$1(input)) {
      return schedulePromise(input, scheduler);
    } else if (isArrayLike$1(input)) {
      return scheduleArray(input, scheduler);
    } else if (isIterable(input) || typeof input === "string") {
      return scheduleIterable(input, scheduler);
    }
  }
  throw new TypeError((input !== null && typeof input || input) + " is not observable");
}
function from(input, scheduler) {
  if (!scheduler) {
    if (input instanceof Observable$1) {
      return input;
    }
    return new Observable$1(subscribeTo$1(input));
  } else {
    return scheduled(input, scheduler);
  }
}
var SimpleInnerSubscriber$1 = /* @__PURE__ */ function(_super) {
  __extends$1(SimpleInnerSubscriber2, _super);
  function SimpleInnerSubscriber2(parent) {
    var _this = _super.call(this) || this;
    _this.parent = parent;
    return _this;
  }
  SimpleInnerSubscriber2.prototype._next = function(value) {
    this.parent.notifyNext(value);
  };
  SimpleInnerSubscriber2.prototype._error = function(error) {
    this.parent.notifyError(error);
    this.unsubscribe();
  };
  SimpleInnerSubscriber2.prototype._complete = function() {
    this.parent.notifyComplete();
    this.unsubscribe();
  };
  return SimpleInnerSubscriber2;
}(Subscriber$1);
var SimpleOuterSubscriber$1 = /* @__PURE__ */ function(_super) {
  __extends$1(SimpleOuterSubscriber2, _super);
  function SimpleOuterSubscriber2() {
    return _super !== null && _super.apply(this, arguments) || this;
  }
  SimpleOuterSubscriber2.prototype.notifyNext = function(innerValue) {
    this.destination.next(innerValue);
  };
  SimpleOuterSubscriber2.prototype.notifyError = function(err) {
    this.destination.error(err);
  };
  SimpleOuterSubscriber2.prototype.notifyComplete = function() {
    this.destination.complete();
  };
  return SimpleOuterSubscriber2;
}(Subscriber$1);
function innerSubscribe$1(result, innerSubscriber) {
  if (innerSubscriber.closed) {
    return void 0;
  }
  if (result instanceof Observable$1) {
    return result.subscribe(innerSubscriber);
  }
  var subscription;
  try {
    subscription = subscribeTo$1(result)(innerSubscriber);
  } catch (error) {
    innerSubscriber.error(error);
  }
  return subscription;
}
function mergeMap(project, resultSelector, concurrent) {
  if (concurrent === void 0) {
    concurrent = Number.POSITIVE_INFINITY;
  }
  if (typeof resultSelector === "function") {
    return function(source) {
      return source.pipe(mergeMap(function(a, i) {
        return from(project(a, i)).pipe(map(function(b, ii) {
          return resultSelector(a, b, i, ii);
        }));
      }, concurrent));
    };
  } else if (typeof resultSelector === "number") {
    concurrent = resultSelector;
  }
  return function(source) {
    return source.lift(new MergeMapOperator(project, concurrent));
  };
}
var MergeMapOperator = /* @__PURE__ */ function() {
  function MergeMapOperator2(project, concurrent) {
    if (concurrent === void 0) {
      concurrent = Number.POSITIVE_INFINITY;
    }
    this.project = project;
    this.concurrent = concurrent;
  }
  MergeMapOperator2.prototype.call = function(observer, source) {
    return source.subscribe(new MergeMapSubscriber(observer, this.project, this.concurrent));
  };
  return MergeMapOperator2;
}();
var MergeMapSubscriber = /* @__PURE__ */ function(_super) {
  __extends$1(MergeMapSubscriber2, _super);
  function MergeMapSubscriber2(destination, project, concurrent) {
    if (concurrent === void 0) {
      concurrent = Number.POSITIVE_INFINITY;
    }
    var _this = _super.call(this, destination) || this;
    _this.project = project;
    _this.concurrent = concurrent;
    _this.hasCompleted = false;
    _this.buffer = [];
    _this.active = 0;
    _this.index = 0;
    return _this;
  }
  MergeMapSubscriber2.prototype._next = function(value) {
    if (this.active < this.concurrent) {
      this._tryNext(value);
    } else {
      this.buffer.push(value);
    }
  };
  MergeMapSubscriber2.prototype._tryNext = function(value) {
    var result;
    var index = this.index++;
    try {
      result = this.project(value, index);
    } catch (err) {
      this.destination.error(err);
      return;
    }
    this.active++;
    this._innerSub(result);
  };
  MergeMapSubscriber2.prototype._innerSub = function(ish) {
    var innerSubscriber = new SimpleInnerSubscriber$1(this);
    var destination = this.destination;
    destination.add(innerSubscriber);
    var innerSubscription = innerSubscribe$1(ish, innerSubscriber);
    if (innerSubscription !== innerSubscriber) {
      destination.add(innerSubscription);
    }
  };
  MergeMapSubscriber2.prototype._complete = function() {
    this.hasCompleted = true;
    if (this.active === 0 && this.buffer.length === 0) {
      this.destination.complete();
    }
    this.unsubscribe();
  };
  MergeMapSubscriber2.prototype.notifyNext = function(innerValue) {
    this.destination.next(innerValue);
  };
  MergeMapSubscriber2.prototype.notifyComplete = function() {
    var buffer = this.buffer;
    this.active--;
    if (buffer.length > 0) {
      this._next(buffer.shift());
    } else if (this.active === 0 && this.hasCompleted) {
      this.destination.complete();
    }
  };
  return MergeMapSubscriber2;
}(SimpleOuterSubscriber$1);
function mergeAll(concurrent) {
  if (concurrent === void 0) {
    concurrent = Number.POSITIVE_INFINITY;
  }
  return mergeMap(identity$1, concurrent);
}
function fromEvent(target, eventName, options, resultSelector) {
  if (isFunction$1(options)) {
    resultSelector = options;
    options = void 0;
  }
  if (resultSelector) {
    return fromEvent(target, eventName, options).pipe(map(function(args) {
      return isArray$1(args) ? resultSelector.apply(void 0, args) : resultSelector(args);
    }));
  }
  return new Observable$1(function(subscriber) {
    function handler(e) {
      if (arguments.length > 1) {
        subscriber.next(Array.prototype.slice.call(arguments));
      } else {
        subscriber.next(e);
      }
    }
    setupSubscription(target, eventName, handler, subscriber, options);
  });
}
function setupSubscription(sourceObj, eventName, handler, subscriber, options) {
  var unsubscribe2;
  if (isEventTarget(sourceObj)) {
    var source_1 = sourceObj;
    sourceObj.addEventListener(eventName, handler, options);
    unsubscribe2 = function() {
      return source_1.removeEventListener(eventName, handler, options);
    };
  } else if (isJQueryStyleEventEmitter(sourceObj)) {
    var source_2 = sourceObj;
    sourceObj.on(eventName, handler);
    unsubscribe2 = function() {
      return source_2.off(eventName, handler);
    };
  } else if (isNodeStyleEventEmitter(sourceObj)) {
    var source_3 = sourceObj;
    sourceObj.addListener(eventName, handler);
    unsubscribe2 = function() {
      return source_3.removeListener(eventName, handler);
    };
  } else if (sourceObj && sourceObj.length) {
    for (var i = 0, len = sourceObj.length; i < len; i++) {
      setupSubscription(sourceObj[i], eventName, handler, subscriber, options);
    }
  } else {
    throw new TypeError("Invalid event target");
  }
  subscriber.add(unsubscribe2);
}
function isNodeStyleEventEmitter(sourceObj) {
  return sourceObj && typeof sourceObj.addListener === "function" && typeof sourceObj.removeListener === "function";
}
function isJQueryStyleEventEmitter(sourceObj) {
  return sourceObj && typeof sourceObj.on === "function" && typeof sourceObj.off === "function";
}
function isEventTarget(sourceObj) {
  return sourceObj && typeof sourceObj.addEventListener === "function" && typeof sourceObj.removeEventListener === "function";
}
function isNumeric(val) {
  return !isArray$1(val) && val - parseFloat(val) + 1 >= 0;
}
function merge() {
  var observables = [];
  for (var _i = 0; _i < arguments.length; _i++) {
    observables[_i] = arguments[_i];
  }
  var concurrent = Number.POSITIVE_INFINITY;
  var scheduler = null;
  var last2 = observables[observables.length - 1];
  if (isScheduler(last2)) {
    scheduler = observables.pop();
    if (observables.length > 1 && typeof observables[observables.length - 1] === "number") {
      concurrent = observables.pop();
    }
  } else if (typeof last2 === "number") {
    concurrent = observables.pop();
  }
  if (scheduler === null && observables.length === 1 && observables[0] instanceof Observable$1) {
    return observables[0];
  }
  return mergeAll(concurrent)(fromArray(observables, scheduler));
}
function timer(dueTime, periodOrScheduler, scheduler) {
  if (dueTime === void 0) {
    dueTime = 0;
  }
  var period = -1;
  if (isNumeric(periodOrScheduler)) {
    period = Number(periodOrScheduler) < 1 && 1 || Number(periodOrScheduler);
  } else if (isScheduler(periodOrScheduler)) {
    scheduler = periodOrScheduler;
  }
  if (!isScheduler(scheduler)) {
    scheduler = async$1;
  }
  return new Observable$1(function(subscriber) {
    var due = isNumeric(dueTime) ? dueTime : +dueTime - scheduler.now();
    return scheduler.schedule(dispatch$1, due, {
      index: 0,
      period,
      subscriber
    });
  });
}
function dispatch$1(state) {
  var index = state.index, period = state.period, subscriber = state.subscriber;
  subscriber.next(index);
  if (subscriber.closed) {
    return;
  } else if (period === -1) {
    return subscriber.complete();
  }
  state.index = index + 1;
  this.schedule(state, period);
}
const networks = {
  ethereum: {
    "1": "main",
    "5": "goerli",
    "100": "xdai",
    "137": "matic-main",
    "80001": "matic-mumbai"
  }
};
const DEPRECATED_NETWORK_IDS = [2, 3, 4, 42, 56, 250];
const DEFAULT_RATE_LIMIT_RULES = {
  points: 150,
  duration: 1
};
const QUEUE_LIMIT = 1e4;
function validateType(options) {
  const {
    name,
    value,
    type,
    optional,
    customValidation
  } = options;
  if (!optional && typeof value === "undefined") {
    throw new Error(`"${name}" is required`);
  }
  if (typeof value !== "undefined" && (type === "array" ? Array.isArray(type) : typeof value !== type)) {
    throw new Error(`"${name}" must be of type: ${type}, received type: ${typeof value} from value: ${value}`);
  }
  if (typeof value !== "undefined" && customValidation && !customValidation(value)) {
    throw new Error(`"${value}" is not a valid "${name}"`);
  }
}
function validateOptions(options) {
  validateType({
    name: "sdk options",
    value: options,
    type: "object"
  });
  const {
    dappId,
    system,
    name,
    appVersion,
    networkId,
    transactionHandlers,
    apiUrl,
    ws,
    onopen,
    ondown,
    onreopen,
    onerror,
    onclose,
    ...otherParams
  } = options;
  invalidParams(otherParams, ["dappId", "system", "name", "appVersion", "networkId", "transactionHandlers", "apiUrl", "ws", "onopen", "ondown", "onreopen", "onerror", "onclose"], "Initialization Options");
  validateType({
    name: "dappId",
    value: dappId,
    type: "string",
    optional: true
  });
  validateType({
    name: "system",
    value: system,
    type: "string",
    optional: true,
    customValidation: validSystem
  });
  validateType({
    name: "name",
    value: name,
    type: "string",
    optional: true
  });
  validateType({
    name: "appVersion",
    value: appVersion,
    type: "string",
    optional: true
  });
  validateType({
    name: "networkId",
    value: networkId,
    type: "number"
  });
  if (DEPRECATED_NETWORK_IDS.includes(networkId)) {
    console.error(`Blocknative SDK: Network with ID: ${networkId} has been deprecated and you will no longer receive transaction events on this network.`);
  }
  validateType({
    name: "transactionHandler",
    value: transactionHandlers,
    type: "array",
    optional: true
  });
  if (transactionHandlers) {
    transactionHandlers.forEach((handler) => validateType({
      name: "transactionHandler",
      value: handler,
      type: "function"
    }));
  }
  validateType({
    name: "apiUrl",
    value: apiUrl,
    type: "string",
    optional: true
  });
  validateType({
    name: "ws",
    value: ws,
    type: "function",
    optional: true
  });
  validateType({
    name: "onopen",
    value: onopen,
    type: "function",
    optional: true
  });
  validateType({
    name: "ondown",
    value: ondown,
    type: "function",
    optional: true
  });
  validateType({
    name: "onreopen",
    value: onreopen,
    type: "function",
    optional: true
  });
  validateType({
    name: "onerror",
    value: onerror,
    type: "function",
    optional: true
  });
  validateType({
    name: "onclose",
    value: onclose,
    type: "function",
    optional: true
  });
}
function validSystem(system) {
  return !!networks[system];
}
function invalidParams(params, validParams, functionName) {
  const invalid = Object.keys(params);
  if (invalid.length > 0) {
    throw new Error(`${invalid[0]} is not a valid parameter for ${functionName}, must be one of the following valid parameters: ${validParams.join(", ")}`);
  }
}
function createEmitter() {
  return {
    listeners: {},
    on: function(eventCode, listener) {
      switch (eventCode) {
        case "txSent":
        case "txPool":
        case "txConfirmed":
        case "txSpeedUp":
        case "txCancel":
        case "txFailed":
        case "txDropped":
        case "txRequest":
        case "nsfFail":
        case "txRepeat":
        case "txAwaitingApproval":
        case "txConfirmReminder":
        case "txSendFail":
        case "txError":
        case "txUnderPriced":
        case "txPoolSimulation":
        case "all":
          break;
        default:
          throw new Error(`${eventCode} is not a valid event code, for a list of valid event codes see: https://docs.blocknative.com/notify-sdk#event-codes`);
      }
      if (typeof listener !== "function") {
        throw new Error("Listener must be a function");
      }
      this.listeners[eventCode] = listener;
    },
    off: function(eventCode) {
      delete this.listeners[eventCode];
    },
    emit: function(state) {
      if (this.listeners[state.eventCode]) {
        return this.listeners[state.eventCode](state);
      }
      if (this.listeners.all) {
        return this.listeners.all(state);
      }
    }
  };
}
function networkName(blockchain, id) {
  return networks[blockchain][id];
}
function serverEcho(eventCode) {
  switch (eventCode) {
    case "txRequest":
    case "nsfFail":
    case "txRepeat":
    case "txAwaitingApproval":
    case "txConfirmReminder":
    case "txSendFail":
    case "txError":
    case "txUnderPriced":
    case "txSent":
      return true;
    default:
      return false;
  }
}
function last(arr) {
  return arr.reverse()[0];
}
function isAddress(blockchain, addressOrHash) {
  switch (blockchain) {
    case "ethereum":
      return addressOrHash.length === 42;
    case "bitcoin":
      return addressOrHash.length !== 64;
    default:
      return false;
  }
}
function isTxid(blockchain, addressOrHash) {
  switch (blockchain) {
    case "ethereum":
      return addressOrHash.length === 66;
    case "bitcoin":
      return addressOrHash.length === 64;
    default:
      return false;
  }
}
function wait(time) {
  return new Promise((resolve) => {
    setTimeout(resolve, time);
  });
}
const jsonPreserveUndefined = (k, v) => v === void 0 ? "undefined" : v;
function isLocalStorageAvailable() {
  const isSupported = typeof window !== "undefined" && "localStorage" in window;
  if (isSupported) {
    const testKey = "__testLocalStorage";
    try {
      window.localStorage.setItem(testKey, "1");
      window.localStorage.removeItem(testKey);
      return true;
    } catch (err) {
      return false;
    }
  }
  return false;
}
function transaction(hash, id) {
  if (this._destroyed)
    throw new Error("The WebSocket instance has been destroyed, re-initialize to continue making requests.");
  const startTime = Date.now();
  const emitter = createEmitter();
  const eventCode = "txSent";
  this.watchedTransactions.push({
    hash,
    emitter
  });
  const transactionId = this._system === "ethereum" ? {
    hash
  } : {
    txid: hash
  };
  const transaction2 = {
    ...transactionId,
    id: id || hash,
    startTime,
    status: "sent"
  };
  const newState = {
    ...transaction2,
    eventCode
  };
  this._sendMessage({
    eventCode,
    categoryCode: "activeTransaction",
    transaction: transaction2
  });
  const transactionObj = {
    details: newState,
    emitter
  };
  function emitState() {
    const emitterResult = emitter.emit(newState);
    this._transactionHandlers.forEach((handler) => handler({
      transaction: newState,
      emitterResult
    }));
  }
  setTimeout(emitState.bind(this), 5);
  return transactionObj;
}
function account(address) {
  if (this._destroyed)
    throw new Error("The WebSocket instance has been destroyed, re-initialize to continue making requests.");
  address = this._system === "ethereum" ? address.toLowerCase() : address;
  const emitter = createEmitter();
  const eventCode = "watch";
  const existingAddressWatcher = this.watchedAccounts.find((ac) => ac.address === address);
  if (existingAddressWatcher) {
    existingAddressWatcher.emitters.push(emitter);
  } else {
    this.watchedAccounts.push({
      address,
      emitters: [emitter]
    });
  }
  this._sendMessage({
    eventCode,
    categoryCode: "accountAddress",
    account: {
      address
    }
  });
  return {
    emitter,
    details: {
      address
    }
  };
}
function event(eventObj) {
  if (this._destroyed)
    throw new Error("The WebSocket instance has been destroyed, re-initialize to continue making requests.");
  this._sendMessage(eventObj);
}
/*! *****************************************************************************
Copyright (c) Microsoft Corporation. All rights reserved.
Licensed under the Apache License, Version 2.0 (the "License"); you may not use
this file except in compliance with the License. You may obtain a copy of the
License at http://www.apache.org/licenses/LICENSE-2.0

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
MERCHANTABLITY OR NON-INFRINGEMENT.

See the Apache Version 2.0 License for specific language governing permissions
and limitations under the License.
***************************************************************************** */
var extendStatics = function(d2, b) {
  extendStatics = Object.setPrototypeOf || {
    __proto__: []
  } instanceof Array && function(d3, b2) {
    d3.__proto__ = b2;
  } || function(d3, b2) {
    for (var p in b2)
      if (b2.hasOwnProperty(p))
        d3[p] = b2[p];
  };
  return extendStatics(d2, b);
};
function __extends(d2, b) {
  extendStatics(d2, b);
  function __() {
    this.constructor = d2;
  }
  d2.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
}
function isFunction(x2) {
  return typeof x2 === "function";
}
var _enable_super_gross_mode_that_will_cause_bad_things = false;
var config = {
  Promise: void 0,
  set useDeprecatedSynchronousErrorHandling(value) {
    if (value) {
      var error = /* @__PURE__ */ new Error();
      /* @__PURE__ */ console.warn("DEPRECATED! RxJS was set to use deprecated synchronous error handling behavior by code at: \n" + error.stack);
    }
    _enable_super_gross_mode_that_will_cause_bad_things = value;
  },
  get useDeprecatedSynchronousErrorHandling() {
    return _enable_super_gross_mode_that_will_cause_bad_things;
  }
};
function hostReportError(err) {
  setTimeout(function() {
    throw err;
  }, 0);
}
var empty = {
  closed: true,
  next: function(value) {
  },
  error: function(err) {
    if (config.useDeprecatedSynchronousErrorHandling) {
      throw err;
    } else {
      hostReportError(err);
    }
  },
  complete: function() {
  }
};
var isArray = /* @__PURE__ */ function() {
  return Array.isArray || function(x2) {
    return x2 && typeof x2.length === "number";
  };
}();
function isObject(x2) {
  return x2 !== null && typeof x2 === "object";
}
var UnsubscriptionErrorImpl = /* @__PURE__ */ function() {
  function UnsubscriptionErrorImpl2(errors) {
    Error.call(this);
    this.message = errors ? errors.length + " errors occurred during unsubscription:\n" + errors.map(function(err, i) {
      return i + 1 + ") " + err.toString();
    }).join("\n  ") : "";
    this.name = "UnsubscriptionError";
    this.errors = errors;
    return this;
  }
  UnsubscriptionErrorImpl2.prototype = /* @__PURE__ */ Object.create(Error.prototype);
  return UnsubscriptionErrorImpl2;
}();
var UnsubscriptionError = UnsubscriptionErrorImpl;
var Subscription = /* @__PURE__ */ function() {
  function Subscription2(unsubscribe2) {
    this.closed = false;
    this._parentOrParents = null;
    this._subscriptions = null;
    if (unsubscribe2) {
      this._ctorUnsubscribe = true;
      this._unsubscribe = unsubscribe2;
    }
  }
  Subscription2.prototype.unsubscribe = function() {
    var errors;
    if (this.closed) {
      return;
    }
    var _a = this, _parentOrParents = _a._parentOrParents, _ctorUnsubscribe = _a._ctorUnsubscribe, _unsubscribe = _a._unsubscribe, _subscriptions = _a._subscriptions;
    this.closed = true;
    this._parentOrParents = null;
    this._subscriptions = null;
    if (_parentOrParents instanceof Subscription2) {
      _parentOrParents.remove(this);
    } else if (_parentOrParents !== null) {
      for (var index = 0; index < _parentOrParents.length; ++index) {
        var parent_1 = _parentOrParents[index];
        parent_1.remove(this);
      }
    }
    if (isFunction(_unsubscribe)) {
      if (_ctorUnsubscribe) {
        this._unsubscribe = void 0;
      }
      try {
        _unsubscribe.call(this);
      } catch (e) {
        errors = e instanceof UnsubscriptionError ? flattenUnsubscriptionErrors(e.errors) : [e];
      }
    }
    if (isArray(_subscriptions)) {
      var index = -1;
      var len = _subscriptions.length;
      while (++index < len) {
        var sub = _subscriptions[index];
        if (isObject(sub)) {
          try {
            sub.unsubscribe();
          } catch (e) {
            errors = errors || [];
            if (e instanceof UnsubscriptionError) {
              errors = errors.concat(flattenUnsubscriptionErrors(e.errors));
            } else {
              errors.push(e);
            }
          }
        }
      }
    }
    if (errors) {
      throw new UnsubscriptionError(errors);
    }
  };
  Subscription2.prototype.add = function(teardown) {
    var subscription = teardown;
    if (!teardown) {
      return Subscription2.EMPTY;
    }
    switch (typeof teardown) {
      case "function":
        subscription = new Subscription2(teardown);
      case "object":
        if (subscription === this || subscription.closed || typeof subscription.unsubscribe !== "function") {
          return subscription;
        } else if (this.closed) {
          subscription.unsubscribe();
          return subscription;
        } else if (!(subscription instanceof Subscription2)) {
          var tmp = subscription;
          subscription = new Subscription2();
          subscription._subscriptions = [tmp];
        }
        break;
      default: {
        throw new Error("unrecognized teardown " + teardown + " added to Subscription.");
      }
    }
    var _parentOrParents = subscription._parentOrParents;
    if (_parentOrParents === null) {
      subscription._parentOrParents = this;
    } else if (_parentOrParents instanceof Subscription2) {
      if (_parentOrParents === this) {
        return subscription;
      }
      subscription._parentOrParents = [_parentOrParents, this];
    } else if (_parentOrParents.indexOf(this) === -1) {
      _parentOrParents.push(this);
    } else {
      return subscription;
    }
    var subscriptions = this._subscriptions;
    if (subscriptions === null) {
      this._subscriptions = [subscription];
    } else {
      subscriptions.push(subscription);
    }
    return subscription;
  };
  Subscription2.prototype.remove = function(subscription) {
    var subscriptions = this._subscriptions;
    if (subscriptions) {
      var subscriptionIndex = subscriptions.indexOf(subscription);
      if (subscriptionIndex !== -1) {
        subscriptions.splice(subscriptionIndex, 1);
      }
    }
  };
  Subscription2.EMPTY = function(empty2) {
    empty2.closed = true;
    return empty2;
  }(new Subscription2());
  return Subscription2;
}();
function flattenUnsubscriptionErrors(errors) {
  return errors.reduce(function(errs, err) {
    return errs.concat(err instanceof UnsubscriptionError ? err.errors : err);
  }, []);
}
var rxSubscriber = /* @__PURE__ */ function() {
  return typeof Symbol === "function" ? /* @__PURE__ */ Symbol("rxSubscriber") : "@@rxSubscriber_" + /* @__PURE__ */ Math.random();
}();
var Subscriber = /* @__PURE__ */ function(_super) {
  __extends(Subscriber2, _super);
  function Subscriber2(destinationOrNext, error, complete) {
    var _this = _super.call(this) || this;
    _this.syncErrorValue = null;
    _this.syncErrorThrown = false;
    _this.syncErrorThrowable = false;
    _this.isStopped = false;
    switch (arguments.length) {
      case 0:
        _this.destination = empty;
        break;
      case 1:
        if (!destinationOrNext) {
          _this.destination = empty;
          break;
        }
        if (typeof destinationOrNext === "object") {
          if (destinationOrNext instanceof Subscriber2) {
            _this.syncErrorThrowable = destinationOrNext.syncErrorThrowable;
            _this.destination = destinationOrNext;
            destinationOrNext.add(_this);
          } else {
            _this.syncErrorThrowable = true;
            _this.destination = new SafeSubscriber(_this, destinationOrNext);
          }
          break;
        }
      default:
        _this.syncErrorThrowable = true;
        _this.destination = new SafeSubscriber(_this, destinationOrNext, error, complete);
        break;
    }
    return _this;
  }
  Subscriber2.prototype[rxSubscriber] = function() {
    return this;
  };
  Subscriber2.create = function(next, error, complete) {
    var subscriber = new Subscriber2(next, error, complete);
    subscriber.syncErrorThrowable = false;
    return subscriber;
  };
  Subscriber2.prototype.next = function(value) {
    if (!this.isStopped) {
      this._next(value);
    }
  };
  Subscriber2.prototype.error = function(err) {
    if (!this.isStopped) {
      this.isStopped = true;
      this._error(err);
    }
  };
  Subscriber2.prototype.complete = function() {
    if (!this.isStopped) {
      this.isStopped = true;
      this._complete();
    }
  };
  Subscriber2.prototype.unsubscribe = function() {
    if (this.closed) {
      return;
    }
    this.isStopped = true;
    _super.prototype.unsubscribe.call(this);
  };
  Subscriber2.prototype._next = function(value) {
    this.destination.next(value);
  };
  Subscriber2.prototype._error = function(err) {
    this.destination.error(err);
    this.unsubscribe();
  };
  Subscriber2.prototype._complete = function() {
    this.destination.complete();
    this.unsubscribe();
  };
  Subscriber2.prototype._unsubscribeAndRecycle = function() {
    var _parentOrParents = this._parentOrParents;
    this._parentOrParents = null;
    this.unsubscribe();
    this.closed = false;
    this.isStopped = false;
    this._parentOrParents = _parentOrParents;
    return this;
  };
  return Subscriber2;
}(Subscription);
var SafeSubscriber = /* @__PURE__ */ function(_super) {
  __extends(SafeSubscriber2, _super);
  function SafeSubscriber2(_parentSubscriber, observerOrNext, error, complete) {
    var _this = _super.call(this) || this;
    _this._parentSubscriber = _parentSubscriber;
    var next;
    var context = _this;
    if (isFunction(observerOrNext)) {
      next = observerOrNext;
    } else if (observerOrNext) {
      next = observerOrNext.next;
      error = observerOrNext.error;
      complete = observerOrNext.complete;
      if (observerOrNext !== empty) {
        context = Object.create(observerOrNext);
        if (isFunction(context.unsubscribe)) {
          _this.add(context.unsubscribe.bind(context));
        }
        context.unsubscribe = _this.unsubscribe.bind(_this);
      }
    }
    _this._context = context;
    _this._next = next;
    _this._error = error;
    _this._complete = complete;
    return _this;
  }
  SafeSubscriber2.prototype.next = function(value) {
    if (!this.isStopped && this._next) {
      var _parentSubscriber = this._parentSubscriber;
      if (!config.useDeprecatedSynchronousErrorHandling || !_parentSubscriber.syncErrorThrowable) {
        this.__tryOrUnsub(this._next, value);
      } else if (this.__tryOrSetError(_parentSubscriber, this._next, value)) {
        this.unsubscribe();
      }
    }
  };
  SafeSubscriber2.prototype.error = function(err) {
    if (!this.isStopped) {
      var _parentSubscriber = this._parentSubscriber;
      var useDeprecatedSynchronousErrorHandling = config.useDeprecatedSynchronousErrorHandling;
      if (this._error) {
        if (!useDeprecatedSynchronousErrorHandling || !_parentSubscriber.syncErrorThrowable) {
          this.__tryOrUnsub(this._error, err);
          this.unsubscribe();
        } else {
          this.__tryOrSetError(_parentSubscriber, this._error, err);
          this.unsubscribe();
        }
      } else if (!_parentSubscriber.syncErrorThrowable) {
        this.unsubscribe();
        if (useDeprecatedSynchronousErrorHandling) {
          throw err;
        }
        hostReportError(err);
      } else {
        if (useDeprecatedSynchronousErrorHandling) {
          _parentSubscriber.syncErrorValue = err;
          _parentSubscriber.syncErrorThrown = true;
        } else {
          hostReportError(err);
        }
        this.unsubscribe();
      }
    }
  };
  SafeSubscriber2.prototype.complete = function() {
    var _this = this;
    if (!this.isStopped) {
      var _parentSubscriber = this._parentSubscriber;
      if (this._complete) {
        var wrappedComplete = function() {
          return _this._complete.call(_this._context);
        };
        if (!config.useDeprecatedSynchronousErrorHandling || !_parentSubscriber.syncErrorThrowable) {
          this.__tryOrUnsub(wrappedComplete);
          this.unsubscribe();
        } else {
          this.__tryOrSetError(_parentSubscriber, wrappedComplete);
          this.unsubscribe();
        }
      } else {
        this.unsubscribe();
      }
    }
  };
  SafeSubscriber2.prototype.__tryOrUnsub = function(fn, value) {
    try {
      fn.call(this._context, value);
    } catch (err) {
      this.unsubscribe();
      if (config.useDeprecatedSynchronousErrorHandling) {
        throw err;
      } else {
        hostReportError(err);
      }
    }
  };
  SafeSubscriber2.prototype.__tryOrSetError = function(parent, fn, value) {
    if (!config.useDeprecatedSynchronousErrorHandling) {
      throw new Error("bad call");
    }
    try {
      fn.call(this._context, value);
    } catch (err) {
      if (config.useDeprecatedSynchronousErrorHandling) {
        parent.syncErrorValue = err;
        parent.syncErrorThrown = true;
        return true;
      } else {
        hostReportError(err);
        return true;
      }
    }
    return false;
  };
  SafeSubscriber2.prototype._unsubscribe = function() {
    var _parentSubscriber = this._parentSubscriber;
    this._context = null;
    this._parentSubscriber = null;
    _parentSubscriber.unsubscribe();
  };
  return SafeSubscriber2;
}(Subscriber);
function canReportError(observer) {
  while (observer) {
    var _a = observer, closed_1 = _a.closed, destination = _a.destination, isStopped = _a.isStopped;
    if (closed_1 || isStopped) {
      return false;
    } else if (destination && destination instanceof Subscriber) {
      observer = destination;
    } else {
      observer = null;
    }
  }
  return true;
}
function toSubscriber(nextOrObserver, error, complete) {
  if (nextOrObserver) {
    if (nextOrObserver instanceof Subscriber) {
      return nextOrObserver;
    }
    if (nextOrObserver[rxSubscriber]) {
      return nextOrObserver[rxSubscriber]();
    }
  }
  if (!nextOrObserver && !error && !complete) {
    return new Subscriber(empty);
  }
  return new Subscriber(nextOrObserver, error, complete);
}
var observable = /* @__PURE__ */ function() {
  return typeof Symbol === "function" && Symbol.observable || "@@observable";
}();
function identity(x2) {
  return x2;
}
function pipeFromArray(fns) {
  if (fns.length === 0) {
    return identity;
  }
  if (fns.length === 1) {
    return fns[0];
  }
  return function piped(input) {
    return fns.reduce(function(prev, fn) {
      return fn(prev);
    }, input);
  };
}
var Observable = /* @__PURE__ */ function() {
  function Observable2(subscribe2) {
    this._isScalar = false;
    if (subscribe2) {
      this._subscribe = subscribe2;
    }
  }
  Observable2.prototype.lift = function(operator) {
    var observable2 = new Observable2();
    observable2.source = this;
    observable2.operator = operator;
    return observable2;
  };
  Observable2.prototype.subscribe = function(observerOrNext, error, complete) {
    var operator = this.operator;
    var sink = toSubscriber(observerOrNext, error, complete);
    if (operator) {
      sink.add(operator.call(sink, this.source));
    } else {
      sink.add(this.source || config.useDeprecatedSynchronousErrorHandling && !sink.syncErrorThrowable ? this._subscribe(sink) : this._trySubscribe(sink));
    }
    if (config.useDeprecatedSynchronousErrorHandling) {
      if (sink.syncErrorThrowable) {
        sink.syncErrorThrowable = false;
        if (sink.syncErrorThrown) {
          throw sink.syncErrorValue;
        }
      }
    }
    return sink;
  };
  Observable2.prototype._trySubscribe = function(sink) {
    try {
      return this._subscribe(sink);
    } catch (err) {
      if (config.useDeprecatedSynchronousErrorHandling) {
        sink.syncErrorThrown = true;
        sink.syncErrorValue = err;
      }
      if (canReportError(sink)) {
        sink.error(err);
      } else {
        console.warn(err);
      }
    }
  };
  Observable2.prototype.forEach = function(next, promiseCtor) {
    var _this = this;
    promiseCtor = getPromiseCtor(promiseCtor);
    return new promiseCtor(function(resolve, reject) {
      var subscription;
      subscription = _this.subscribe(function(value) {
        try {
          next(value);
        } catch (err) {
          reject(err);
          if (subscription) {
            subscription.unsubscribe();
          }
        }
      }, reject, resolve);
    });
  };
  Observable2.prototype._subscribe = function(subscriber) {
    var source = this.source;
    return source && source.subscribe(subscriber);
  };
  Observable2.prototype[observable] = function() {
    return this;
  };
  Observable2.prototype.pipe = function() {
    var operations = [];
    for (var _i = 0; _i < arguments.length; _i++) {
      operations[_i] = arguments[_i];
    }
    if (operations.length === 0) {
      return this;
    }
    return pipeFromArray(operations)(this);
  };
  Observable2.prototype.toPromise = function(promiseCtor) {
    var _this = this;
    promiseCtor = getPromiseCtor(promiseCtor);
    return new promiseCtor(function(resolve, reject) {
      var value;
      _this.subscribe(function(x2) {
        return value = x2;
      }, function(err) {
        return reject(err);
      }, function() {
        return resolve(value);
      });
    });
  };
  Observable2.create = function(subscribe2) {
    return new Observable2(subscribe2);
  };
  return Observable2;
}();
function getPromiseCtor(promiseCtor) {
  if (!promiseCtor) {
    promiseCtor = Promise;
  }
  if (!promiseCtor) {
    throw new Error("no Promise impl found");
  }
  return promiseCtor;
}
var subscribeToArray = function(array) {
  return function(subscriber) {
    for (var i = 0, len = array.length; i < len && !subscriber.closed; i++) {
      subscriber.next(array[i]);
    }
    subscriber.complete();
  };
};
var subscribeToPromise = function(promise) {
  return function(subscriber) {
    promise.then(function(value) {
      if (!subscriber.closed) {
        subscriber.next(value);
        subscriber.complete();
      }
    }, function(err) {
      return subscriber.error(err);
    }).then(null, hostReportError);
    return subscriber;
  };
};
function getSymbolIterator() {
  if (typeof Symbol !== "function" || !Symbol.iterator) {
    return "@@iterator";
  }
  return Symbol.iterator;
}
var iterator = /* @__PURE__ */ getSymbolIterator();
var subscribeToIterable = function(iterable) {
  return function(subscriber) {
    var iterator$12 = iterable[iterator]();
    do {
      var item = void 0;
      try {
        item = iterator$12.next();
      } catch (err) {
        subscriber.error(err);
        return subscriber;
      }
      if (item.done) {
        subscriber.complete();
        break;
      }
      subscriber.next(item.value);
      if (subscriber.closed) {
        break;
      }
    } while (true);
    if (typeof iterator$12.return === "function") {
      subscriber.add(function() {
        if (iterator$12.return) {
          iterator$12.return();
        }
      });
    }
    return subscriber;
  };
};
var subscribeToObservable = function(obj) {
  return function(subscriber) {
    var obs = obj[observable]();
    if (typeof obs.subscribe !== "function") {
      throw new TypeError("Provided object does not correctly implement Symbol.observable");
    } else {
      return obs.subscribe(subscriber);
    }
  };
};
var isArrayLike = function(x2) {
  return x2 && typeof x2.length === "number" && typeof x2 !== "function";
};
function isPromise(value) {
  return !!value && typeof value.subscribe !== "function" && typeof value.then === "function";
}
var subscribeTo = function(result) {
  if (!!result && typeof result[observable] === "function") {
    return subscribeToObservable(result);
  } else if (isArrayLike(result)) {
    return subscribeToArray(result);
  } else if (isPromise(result)) {
    return subscribeToPromise(result);
  } else if (!!result && typeof result[iterator] === "function") {
    return subscribeToIterable(result);
  } else {
    var value = isObject(result) ? "an invalid object" : "'" + result + "'";
    var msg = "You provided " + value + " where a stream was expected. You can provide an Observable, Promise, Array, or Iterable.";
    throw new TypeError(msg);
  }
};
var SimpleInnerSubscriber = /* @__PURE__ */ function(_super) {
  __extends(SimpleInnerSubscriber2, _super);
  function SimpleInnerSubscriber2(parent) {
    var _this = _super.call(this) || this;
    _this.parent = parent;
    return _this;
  }
  SimpleInnerSubscriber2.prototype._next = function(value) {
    this.parent.notifyNext(value);
  };
  SimpleInnerSubscriber2.prototype._error = function(error) {
    this.parent.notifyError(error);
    this.unsubscribe();
  };
  SimpleInnerSubscriber2.prototype._complete = function() {
    this.parent.notifyComplete();
    this.unsubscribe();
  };
  return SimpleInnerSubscriber2;
}(Subscriber);
var SimpleOuterSubscriber = /* @__PURE__ */ function(_super) {
  __extends(SimpleOuterSubscriber2, _super);
  function SimpleOuterSubscriber2() {
    return _super !== null && _super.apply(this, arguments) || this;
  }
  SimpleOuterSubscriber2.prototype.notifyNext = function(innerValue) {
    this.destination.next(innerValue);
  };
  SimpleOuterSubscriber2.prototype.notifyError = function(err) {
    this.destination.error(err);
  };
  SimpleOuterSubscriber2.prototype.notifyComplete = function() {
    this.destination.complete();
  };
  return SimpleOuterSubscriber2;
}(Subscriber);
function innerSubscribe(result, innerSubscriber) {
  if (innerSubscriber.closed) {
    return void 0;
  }
  if (result instanceof Observable) {
    return result.subscribe(innerSubscriber);
  }
  return subscribeTo(result)(innerSubscriber);
}
var Action = /* @__PURE__ */ function(_super) {
  __extends(Action2, _super);
  function Action2(scheduler, work) {
    return _super.call(this) || this;
  }
  Action2.prototype.schedule = function(state, delay) {
    return this;
  };
  return Action2;
}(Subscription);
var AsyncAction = /* @__PURE__ */ function(_super) {
  __extends(AsyncAction2, _super);
  function AsyncAction2(scheduler, work) {
    var _this = _super.call(this, scheduler, work) || this;
    _this.scheduler = scheduler;
    _this.work = work;
    _this.pending = false;
    return _this;
  }
  AsyncAction2.prototype.schedule = function(state, delay) {
    if (delay === void 0) {
      delay = 0;
    }
    if (this.closed) {
      return this;
    }
    this.state = state;
    var id = this.id;
    var scheduler = this.scheduler;
    if (id != null) {
      this.id = this.recycleAsyncId(scheduler, id, delay);
    }
    this.pending = true;
    this.delay = delay;
    this.id = this.id || this.requestAsyncId(scheduler, this.id, delay);
    return this;
  };
  AsyncAction2.prototype.requestAsyncId = function(scheduler, id, delay) {
    if (delay === void 0) {
      delay = 0;
    }
    return setInterval(scheduler.flush.bind(scheduler, this), delay);
  };
  AsyncAction2.prototype.recycleAsyncId = function(scheduler, id, delay) {
    if (delay === void 0) {
      delay = 0;
    }
    if (delay !== null && this.delay === delay && this.pending === false) {
      return id;
    }
    clearInterval(id);
    return void 0;
  };
  AsyncAction2.prototype.execute = function(state, delay) {
    if (this.closed) {
      return new Error("executing a cancelled action");
    }
    this.pending = false;
    var error = this._execute(state, delay);
    if (error) {
      return error;
    } else if (this.pending === false && this.id != null) {
      this.id = this.recycleAsyncId(this.scheduler, this.id, null);
    }
  };
  AsyncAction2.prototype._execute = function(state, delay) {
    var errored = false;
    var errorValue = void 0;
    try {
      this.work(state);
    } catch (e) {
      errored = true;
      errorValue = !!e && e || new Error(e);
    }
    if (errored) {
      this.unsubscribe();
      return errorValue;
    }
  };
  AsyncAction2.prototype._unsubscribe = function() {
    var id = this.id;
    var scheduler = this.scheduler;
    var actions = scheduler.actions;
    var index = actions.indexOf(this);
    this.work = null;
    this.state = null;
    this.pending = false;
    this.scheduler = null;
    if (index !== -1) {
      actions.splice(index, 1);
    }
    if (id != null) {
      this.id = this.recycleAsyncId(scheduler, id, null);
    }
    this.delay = null;
  };
  return AsyncAction2;
}(Action);
var Scheduler = /* @__PURE__ */ function() {
  function Scheduler2(SchedulerAction, now) {
    if (now === void 0) {
      now = Scheduler2.now;
    }
    this.SchedulerAction = SchedulerAction;
    this.now = now;
  }
  Scheduler2.prototype.schedule = function(work, delay, state) {
    if (delay === void 0) {
      delay = 0;
    }
    return new this.SchedulerAction(this, work).schedule(state, delay);
  };
  Scheduler2.now = function() {
    return Date.now();
  };
  return Scheduler2;
}();
var AsyncScheduler = /* @__PURE__ */ function(_super) {
  __extends(AsyncScheduler2, _super);
  function AsyncScheduler2(SchedulerAction, now) {
    if (now === void 0) {
      now = Scheduler.now;
    }
    var _this = _super.call(this, SchedulerAction, function() {
      if (AsyncScheduler2.delegate && AsyncScheduler2.delegate !== _this) {
        return AsyncScheduler2.delegate.now();
      } else {
        return now();
      }
    }) || this;
    _this.actions = [];
    _this.active = false;
    _this.scheduled = void 0;
    return _this;
  }
  AsyncScheduler2.prototype.schedule = function(work, delay, state) {
    if (delay === void 0) {
      delay = 0;
    }
    if (AsyncScheduler2.delegate && AsyncScheduler2.delegate !== this) {
      return AsyncScheduler2.delegate.schedule(work, delay, state);
    } else {
      return _super.prototype.schedule.call(this, work, delay, state);
    }
  };
  AsyncScheduler2.prototype.flush = function(action) {
    var actions = this.actions;
    if (this.active) {
      actions.push(action);
      return;
    }
    var error;
    this.active = true;
    do {
      if (error = action.execute(action.state, action.delay)) {
        break;
      }
    } while (action = actions.shift());
    this.active = false;
    if (error) {
      while (action = actions.shift()) {
        action.unsubscribe();
      }
      throw error;
    }
  };
  return AsyncScheduler2;
}(Scheduler);
var asyncScheduler = /* @__PURE__ */ new AsyncScheduler(AsyncAction);
var async = asyncScheduler;
function isDate(value) {
  return value instanceof Date && !isNaN(+value);
}
var EMPTY = /* @__PURE__ */ new Observable(function(subscriber) {
  return subscriber.complete();
});
function empty$1(scheduler) {
  return scheduler ? emptyScheduled(scheduler) : EMPTY;
}
function emptyScheduled(scheduler) {
  return new Observable(function(subscriber) {
    return scheduler.schedule(function() {
      return subscriber.complete();
    });
  });
}
function throwError(error, scheduler) {
  if (!scheduler) {
    return new Observable(function(subscriber) {
      return subscriber.error(error);
    });
  } else {
    return new Observable(function(subscriber) {
      return scheduler.schedule(dispatch, 0, {
        error,
        subscriber
      });
    });
  }
}
function dispatch(_a) {
  var error = _a.error, subscriber = _a.subscriber;
  subscriber.error(error);
}
var ArgumentOutOfRangeErrorImpl = /* @__PURE__ */ function() {
  function ArgumentOutOfRangeErrorImpl2() {
    Error.call(this);
    this.message = "argument out of range";
    this.name = "ArgumentOutOfRangeError";
    return this;
  }
  ArgumentOutOfRangeErrorImpl2.prototype = /* @__PURE__ */ Object.create(Error.prototype);
  return ArgumentOutOfRangeErrorImpl2;
}();
var ArgumentOutOfRangeError = ArgumentOutOfRangeErrorImpl;
function filter(predicate, thisArg) {
  return function filterOperatorFunction(source) {
    return source.lift(new FilterOperator(predicate, thisArg));
  };
}
var FilterOperator = /* @__PURE__ */ function() {
  function FilterOperator2(predicate, thisArg) {
    this.predicate = predicate;
    this.thisArg = thisArg;
  }
  FilterOperator2.prototype.call = function(subscriber, source) {
    return source.subscribe(new FilterSubscriber(subscriber, this.predicate, this.thisArg));
  };
  return FilterOperator2;
}();
var FilterSubscriber = /* @__PURE__ */ function(_super) {
  __extends(FilterSubscriber2, _super);
  function FilterSubscriber2(destination, predicate, thisArg) {
    var _this = _super.call(this, destination) || this;
    _this.predicate = predicate;
    _this.thisArg = thisArg;
    _this.count = 0;
    return _this;
  }
  FilterSubscriber2.prototype._next = function(value) {
    var result;
    try {
      result = this.predicate.call(this.thisArg, value, this.count++);
    } catch (err) {
      this.destination.error(err);
      return;
    }
    if (result) {
      this.destination.next(value);
    }
  };
  return FilterSubscriber2;
}(Subscriber);
function take(count) {
  return function(source) {
    if (count === 0) {
      return empty$1();
    } else {
      return source.lift(new TakeOperator(count));
    }
  };
}
var TakeOperator = /* @__PURE__ */ function() {
  function TakeOperator2(total) {
    this.total = total;
    if (this.total < 0) {
      throw new ArgumentOutOfRangeError();
    }
  }
  TakeOperator2.prototype.call = function(subscriber, source) {
    return source.subscribe(new TakeSubscriber(subscriber, this.total));
  };
  return TakeOperator2;
}();
var TakeSubscriber = /* @__PURE__ */ function(_super) {
  __extends(TakeSubscriber2, _super);
  function TakeSubscriber2(destination, total) {
    var _this = _super.call(this, destination) || this;
    _this.total = total;
    _this.count = 0;
    return _this;
  }
  TakeSubscriber2.prototype._next = function(value) {
    var total = this.total;
    var count = ++this.count;
    if (count <= total) {
      this.destination.next(value);
      if (count === total) {
        this.destination.complete();
        this.unsubscribe();
      }
    }
  };
  return TakeSubscriber2;
}(Subscriber);
function finalize(callback) {
  return function(source) {
    return source.lift(new FinallyOperator(callback));
  };
}
var FinallyOperator = /* @__PURE__ */ function() {
  function FinallyOperator2(callback) {
    this.callback = callback;
  }
  FinallyOperator2.prototype.call = function(subscriber, source) {
    return source.subscribe(new FinallySubscriber(subscriber, this.callback));
  };
  return FinallyOperator2;
}();
var FinallySubscriber = /* @__PURE__ */ function(_super) {
  __extends(FinallySubscriber2, _super);
  function FinallySubscriber2(destination, callback) {
    var _this = _super.call(this, destination) || this;
    _this.add(new Subscription(callback));
    return _this;
  }
  return FinallySubscriber2;
}(Subscriber);
function takeWhile(predicate, inclusive) {
  if (inclusive === void 0) {
    inclusive = false;
  }
  return function(source) {
    return source.lift(new TakeWhileOperator(predicate, inclusive));
  };
}
var TakeWhileOperator = /* @__PURE__ */ function() {
  function TakeWhileOperator2(predicate, inclusive) {
    this.predicate = predicate;
    this.inclusive = inclusive;
  }
  TakeWhileOperator2.prototype.call = function(subscriber, source) {
    return source.subscribe(new TakeWhileSubscriber(subscriber, this.predicate, this.inclusive));
  };
  return TakeWhileOperator2;
}();
var TakeWhileSubscriber = /* @__PURE__ */ function(_super) {
  __extends(TakeWhileSubscriber2, _super);
  function TakeWhileSubscriber2(destination, predicate, inclusive) {
    var _this = _super.call(this, destination) || this;
    _this.predicate = predicate;
    _this.inclusive = inclusive;
    _this.index = 0;
    return _this;
  }
  TakeWhileSubscriber2.prototype._next = function(value) {
    var destination = this.destination;
    var result;
    try {
      result = this.predicate(value, this.index++);
    } catch (err) {
      destination.error(err);
      return;
    }
    this.nextOrComplete(value, result);
  };
  TakeWhileSubscriber2.prototype.nextOrComplete = function(value, predicateResult) {
    var destination = this.destination;
    if (Boolean(predicateResult)) {
      destination.next(value);
    } else {
      if (this.inclusive) {
        destination.next(value);
      }
      destination.complete();
    }
  };
  return TakeWhileSubscriber2;
}(Subscriber);
var TimeoutErrorImpl = /* @__PURE__ */ function() {
  function TimeoutErrorImpl2() {
    Error.call(this);
    this.message = "Timeout has occurred";
    this.name = "TimeoutError";
    return this;
  }
  TimeoutErrorImpl2.prototype = /* @__PURE__ */ Object.create(Error.prototype);
  return TimeoutErrorImpl2;
}();
var TimeoutError = TimeoutErrorImpl;
function timeoutWith(due, withObservable, scheduler) {
  if (scheduler === void 0) {
    scheduler = async;
  }
  return function(source) {
    var absoluteTimeout = isDate(due);
    var waitFor = absoluteTimeout ? +due - scheduler.now() : Math.abs(due);
    return source.lift(new TimeoutWithOperator(waitFor, absoluteTimeout, withObservable, scheduler));
  };
}
var TimeoutWithOperator = /* @__PURE__ */ function() {
  function TimeoutWithOperator2(waitFor, absoluteTimeout, withObservable, scheduler) {
    this.waitFor = waitFor;
    this.absoluteTimeout = absoluteTimeout;
    this.withObservable = withObservable;
    this.scheduler = scheduler;
  }
  TimeoutWithOperator2.prototype.call = function(subscriber, source) {
    return source.subscribe(new TimeoutWithSubscriber(subscriber, this.absoluteTimeout, this.waitFor, this.withObservable, this.scheduler));
  };
  return TimeoutWithOperator2;
}();
var TimeoutWithSubscriber = /* @__PURE__ */ function(_super) {
  __extends(TimeoutWithSubscriber2, _super);
  function TimeoutWithSubscriber2(destination, absoluteTimeout, waitFor, withObservable, scheduler) {
    var _this = _super.call(this, destination) || this;
    _this.absoluteTimeout = absoluteTimeout;
    _this.waitFor = waitFor;
    _this.withObservable = withObservable;
    _this.scheduler = scheduler;
    _this.scheduleTimeout();
    return _this;
  }
  TimeoutWithSubscriber2.dispatchTimeout = function(subscriber) {
    var withObservable = subscriber.withObservable;
    subscriber._unsubscribeAndRecycle();
    subscriber.add(innerSubscribe(withObservable, new SimpleInnerSubscriber(subscriber)));
  };
  TimeoutWithSubscriber2.prototype.scheduleTimeout = function() {
    var action = this.action;
    if (action) {
      this.action = action.schedule(this, this.waitFor);
    } else {
      this.add(this.action = this.scheduler.schedule(TimeoutWithSubscriber2.dispatchTimeout, this.waitFor, this));
    }
  };
  TimeoutWithSubscriber2.prototype._next = function(value) {
    if (!this.absoluteTimeout) {
      this.scheduleTimeout();
    }
    _super.prototype._next.call(this, value);
  };
  TimeoutWithSubscriber2.prototype._unsubscribe = function() {
    this.action = void 0;
    this.scheduler = null;
    this.withObservable = null;
  };
  return TimeoutWithSubscriber2;
}(SimpleOuterSubscriber);
function timeout(due, scheduler) {
  if (scheduler === void 0) {
    scheduler = async;
  }
  return timeoutWith(due, throwError(new TimeoutError()), scheduler);
}
const simulations$ = new Subject();
function simulate(system, network, transaction2) {
  if (this._destroyed)
    throw new Error("The WebSocket instance has been destroyed, re-initialize to continue making requests.");
  const id = nanoid();
  this._sendMessage({
    categoryCode: "simulate",
    eventCode: "txSimulation",
    eventId: id,
    transaction: transaction2
  });
  return new Promise((resolve, reject) => {
    simulations$.pipe(filter(({
      eventId
    }) => {
      return eventId === id;
    }), take(1)).subscribe({
      next: ({
        transaction: transaction3
      }) => resolve(transaction3),
      error: ({
        error
      }) => reject(error.message)
    });
  });
}
function multiSim(transactions) {
  if (this._destroyed)
    throw new Error("The WebSocket instance has been destroyed, re-initialize to continue making requests.");
  const id = nanoid();
  this._sendMessage({
    categoryCode: "simulate",
    eventCode: "txSimulation",
    eventId: id,
    transaction: transactions
  });
  return new Promise((resolve, reject) => {
    simulations$.pipe(filter(({
      eventId
    }) => {
      return eventId === id;
    }), take(1)).subscribe({
      next: ({
        transaction: transaction2
      }) => resolve(transaction2),
      error: ({
        error
      }) => reject(error.message)
    });
  });
}
function unsubscribe(addressOrHash) {
  if (this._destroyed)
    throw new Error("The WebSocket instance has been destroyed, re-initialize to continue making requests.");
  const address = isAddress(this._system, addressOrHash);
  const txid = isTxid(this._system, addressOrHash);
  if (address) {
    const normalizedAddress = this._system === "ethereum" ? addressOrHash.toLowerCase() : addressOrHash;
    this.watchedAccounts = this.watchedAccounts.filter((ac) => ac.address !== normalizedAddress);
    this.configurations.delete(normalizedAddress);
    this._sendMessage({
      categoryCode: "accountAddress",
      eventCode: "unwatch",
      account: {
        address: normalizedAddress
      }
    });
  } else if (txid) {
    this.watchedTransactions = this.watchedTransactions.filter((tx) => tx.hash !== addressOrHash);
    const transactionId = this._system === "ethereum" ? {
      hash: addressOrHash
    } : {
      txid: addressOrHash
    };
    const transaction2 = {
      ...transactionId,
      id: addressOrHash,
      status: "unsubscribed"
    };
    this._sendMessage({
      categoryCode: "activeTransaction",
      eventCode: "unwatch",
      transaction: transaction2
    });
  } else {
    throw new Error(`Error trying to unsubscribe ${addressOrHash}: not a valid address or transaction id/hash`);
  }
}
function configuration(config2) {
  if (this._destroyed) {
    throw new Error("The WebSocket instance has been destroyed, re-initialize to continue making requests.");
  }
  const casedScope = this._system === "ethereum" ? config2.scope.toLowerCase() : config2.scope;
  const previousConfiguration = this.configurations.get(casedScope);
  previousConfiguration && previousConfiguration.subscription && previousConfiguration.subscription.next();
  const subscription = new Subject();
  const emitter = config2.watchAddress ? {
    emitter: createEmitter()
  } : {};
  this.configurations.set(casedScope, {
    ...config2,
    ...emitter,
    subscription
  });
  this._sendMessage({
    categoryCode: "configs",
    eventCode: "put",
    config: config2
  });
  return new Promise((resolve, reject) => {
    subscription.pipe(take(1), timeout(5e3)).subscribe({
      next: () => resolve({
        ...emitter,
        details: {
          config: config2
        }
      }),
      error: (error) => {
        const message = error.message === "Timeout has occurred" ? `Configuration with scope: ${config2.scope} has been sent to the Blocknative server, but has not received a reply within 5 seconds.` : error.message;
        if (this._onerror) {
          this._onerror({
            message
          });
          resolve(`Error: ${message}`);
        } else {
          reject(message);
        }
      }
    });
  });
}
function subscribe(subscription) {
  const {
    id,
    chainId,
    type
  } = subscription;
  if (!networkName("ethereum", parseInt(chainId, 16))) {
    throw new Error(`chainId: ${chainId} is an unsupported network`);
  }
  if (!this.connections[chainId]) {
    this.connections[chainId] = new this.Blocknative({
      system: "ethereum",
      networkId: parseInt(chainId, 16),
      dappId: this.apiKey,
      ws: this.ws,
      apiUrl: this.apiUrl,
      transactionHandlers: [({
        transaction: transaction2
      }) => {
        this.onTransaction$.next(transaction2);
      }],
      onerror: (error) => this.errors$.next(error)
    });
  }
  const sdk = this.connections[chainId];
  if (type === "account") {
    const {
      filters = [],
      abi
    } = subscription;
    sdk.configuration({
      scope: id,
      filters,
      ...abi ? {
        abi
      } : {},
      watchAddress: true
    });
    return this.transactions$.pipe(filter(({
      watchedAddress
    }) => watchedAddress === id), finalize(() => {
      this.unsubscribe({
        id,
        chainId
      });
    }));
  } else {
    const {
      emitter
    } = sdk.transaction(id);
    return fromEvent(
      // eslint-disable-next-line
      // @ts-ignore - string does not match specific eventcode string
      emitter,
      "all"
    ).pipe(
      // automatically complete stream on a finalized status
      takeWhile(({
        status
      }) => status !== "confirmed" && status !== "failed" && status !== "dropped", true),
      // cleanup subscription and SDK on completion
      finalize(() => {
        this.unsubscribe({
          id,
          chainId
        });
      })
    );
  }
}
function unsubscribe$1(options) {
  const {
    id,
    chainId,
    timeout: timeout2 = 0
  } = options;
  const time$ = timer(timeout2);
  const transactionEvent$ = this.transactions$.pipe(filter(({
    hash,
    watchedAddress
  }) => hash === id || watchedAddress === id));
  merge(transactionEvent$, time$).pipe(take(1)).subscribe((res) => {
    if (typeof res === "number") {
      const sdkConnections = Object.entries(this.connections).filter(([chainId2, sdk]) => sdk !== null);
      sdkConnections.forEach(([connectionChainId, sdk]) => {
        if (chainId && connectionChainId !== chainId)
          return;
        sdk.unsubscribe(id);
        if (!sdk.watchedAccounts.length && !sdk.watchedTransactions.length && !sdk.configurations.size) {
          sdk.destroy();
          this.connections[connectionChainId] = null;
        }
      });
    } else {
      this.unsubscribe(options);
    }
  });
}
class MultiChain {
  constructor(options, Blocknative) {
    const {
      ws
    } = options;
    const {
      apiKey
    } = options;
    const {
      apiUrl
    } = options;
    this.apiKey = apiKey;
    this.apiUrl = apiUrl;
    this.ws = ws;
    this.connections = {};
    this.onTransaction$ = new Subject();
    this.transactions$ = this.onTransaction$.asObservable();
    this.errors$ = new Subject();
    this.Blocknative = Blocknative;
    this.subscribe = subscribe.bind(this);
    this.unsubscribe = unsubscribe$1.bind(this);
  }
}
var version = "4.6.7";
function sendMessage(msg) {
  if (this._queuedMessages.length > QUEUE_LIMIT) {
    throw new Error(`Queue limit of ${QUEUE_LIMIT} messages has been reached.`);
  }
  this._queuedMessages.push(createEventLog.bind(this)(msg));
  if (!this._processingQueue) {
    this._processQueue();
  }
}
async function processQueue() {
  this._processingQueue = true;
  if (!this._connected) {
    await waitForConnectionOpen.bind(this)();
  }
  while (this._queuedMessages.length > 0) {
    await wait(1);
    if (this._waitToRetry !== null) {
      await this._waitToRetry;
      this._waitToRetry = null;
    }
    const msg = this._queuedMessages.shift();
    const delay = this._limitRules.duration / this._limitRules.points * 1e3;
    await wait(delay);
    this._socket.send(msg);
  }
  this._processingQueue = false;
  this._limitRules = DEFAULT_RATE_LIMIT_RULES;
}
function handleMessage(msg) {
  const {
    status,
    reason,
    event: event2,
    connectionId,
    serverVersion,
    retryMs,
    limitRules,
    blockedMsg,
    dispatchTimestamp
  } = JSON.parse(msg.data);
  if (connectionId) {
    if (isLocalStorageAvailable()) {
      window.localStorage.setItem(this._storageKey, connectionId);
    }
    this._connectionId = connectionId;
  }
  if (status === "error") {
    if (reason.includes("ratelimit") && !reason.match(/IP (PendingSimulation|Notification) ratelimit reached/)) {
      this._waitToRetry = wait(retryMs);
      this._limitRules = limitRules;
      blockedMsg && this._queuedMessages.unshift(blockedMsg);
      return;
    }
    if (reason.includes("upgrade your plan")) {
      if (this._onerror) {
        this._onerror({
          message: reason
        });
        return;
      } else {
        throw new Error(reason);
      }
    }
    if (reason.includes("not a valid API key")) {
      if (this._onerror) {
        this._onerror({
          message: reason
        });
        return;
      } else {
        throw new Error(reason);
      }
    }
    if (reason.includes("network not supported")) {
      if (this._onerror) {
        this._onerror({
          message: reason
        });
        return;
      } else {
        throw new Error(reason);
      }
    }
    if (reason.includes("maximum allowed amount")) {
      if (this._onerror) {
        this._onerror({
          message: reason
        });
        return;
      } else {
        throw new Error(reason);
      }
    }
    if (reason.includes("invalid txid")) {
      const reason2 = `${event2.transaction.txid} is an invalid txid`;
      if (this._onerror) {
        this._onerror({
          message: reason2,
          transaction: event2.transaction.txid
        });
        return;
      } else {
        throw new Error(reason2);
      }
    }
    if (reason.includes("invalid hash")) {
      const reason2 = `${event2.transaction.hash} is an invalid transaction hash`;
      if (this._onerror) {
        this._onerror({
          message: reason2,
          transaction: event2.transaction.hash
        });
        return;
      } else {
        throw new Error(reason2);
      }
    }
    if (reason.includes("invalid address")) {
      const reason2 = `${event2.account.address} is an invalid address`;
      if (this._onerror) {
        this._onerror({
          message: reason2,
          account: event2.account.address
        });
        return;
      } else {
        throw new Error(reason2);
      }
    }
    if (reason.includes("not a valid Bitcoin")) {
      if (this._onerror) {
        this._onerror({
          message: reason,
          account: event2.account.address
        });
        return;
      } else {
        throw new Error(reason);
      }
    }
    if (reason.includes("not a valid Ethereum")) {
      if (this._onerror) {
        this._onerror({
          message: reason,
          account: event2.account.address
        });
        return;
      } else {
        throw new Error(reason);
      }
    }
    if (event2 && event2.categoryCode === "simulate") {
      simulations$.error({
        eventId: event2.eventId,
        error: {
          message: reason
        }
      });
      return;
    }
    if (event2 && event2.config) {
      const configuration2 = this.configurations.get(event2.config.scope);
      if (configuration2 && configuration2.subscription) {
        configuration2.subscription.error({
          message: reason
        });
      }
      return;
    }
    if (this._onerror) {
      this._onerror({
        message: reason
      });
      return;
    } else {
      throw new Error(reason);
    }
  }
  if (event2 && event2.config) {
    const casedScope = this._system === "ethereum" ? event2.config.scope.toLowerCase() : event2.config.scope;
    const configuration2 = this.configurations.get(casedScope);
    if (configuration2 && configuration2.subscription) {
      configuration2.subscription.next();
    }
  }
  if (event2 && event2.transaction) {
    const {
      eventId,
      transaction: transaction2,
      eventCode,
      contractCall,
      timeStamp,
      blockchain: {
        system,
        network
      }
    } = event2;
    const newState = this._system === "ethereum" ? {
      ...transaction2,
      serverVersion,
      eventCode,
      timeStamp,
      dispatchTimestamp,
      system,
      network,
      contractCall
    } : {
      ...transaction2,
      serverVersion,
      eventCode,
      timeStamp,
      dispatchTimestamp,
      system,
      network
    };
    if (serverEcho(eventCode) || transaction2.status === "unsubscribed") {
      return;
    }
    if (newState.originalHash) {
      newState.replaceHash = newState.hash;
      newState.hash = newState.originalHash;
      delete newState.originalHash;
    }
    if (eventCode === "txSpeedUp" && newState.status !== "speedup") {
      newState.status = "speedup";
    }
    if (eventCode === "txCancel" && newState.status !== "cancel") {
      newState.status = "cancel";
    }
    if (eventCode === "txSpeedUp" || eventCode === "txCancel") {
      this.watchedTransactions = this.watchedTransactions.map((tx) => {
        if (tx.hash === newState.replaceHash) {
          tx.hash = transaction2.hash || transaction2.txid;
        }
        return tx;
      });
    }
    if (event2 && event2.categoryCode === "simulate") {
      newState.contractCall = event2.transaction.contractCall;
      delete newState.dispatchTimestamp;
      simulations$.next({
        eventId,
        transaction: newState
      });
      return;
    }
    const watchedAddress = transaction2.watchedAddress && this._system === "ethereum" ? transaction2.watchedAddress.toLowerCase() : transaction2.watchedAddress;
    if (watchedAddress) {
      const accountObj = this.watchedAccounts.find((ac) => ac.address === watchedAddress);
      const accountEmitterResult = accountObj ? last(accountObj.emitters.map((emitter) => emitter.emit(newState))) : false;
      const configuration2 = this.configurations.get(watchedAddress);
      const emitterResult = configuration2 && configuration2.emitter ? configuration2.emitter.emit(newState) || accountEmitterResult : accountEmitterResult;
      this._transactionHandlers.forEach((handler) => handler({
        transaction: newState,
        emitterResult
      }));
    } else {
      const transactionObj = this.watchedTransactions.find((tx) => tx.hash === newState.hash || newState.txid);
      const emitterResult = transactionObj && transactionObj.emitter.emit(newState);
      this._transactionHandlers.forEach((handler) => handler({
        transaction: newState,
        emitterResult
      }));
      if (newState.status === "speedup" || newState.status === "cancel") {
        this.watchedTransactions = this.watchedTransactions.map((tx) => {
          if (tx.hash === newState.hash || newState.txid) {
            return {
              ...tx,
              hash: newState.replaceHash
            };
          }
          return tx;
        });
      }
    }
  }
}
function createEventLog(msg) {
  return JSON.stringify({
    timeStamp: new Date().toISOString(),
    dappId: this._dappId,
    version,
    appName: this._appName,
    appVersion: this._appVersion,
    blockchain: {
      system: this._system,
      network: networkName(this._system, this._networkId) || "local"
    },
    ...msg
  }, msg.categoryCode === "configs" ? jsonPreserveUndefined : void 0);
}
function waitForConnectionOpen() {
  return new Promise((resolve) => {
    const interval = setInterval(() => {
      if (this._connected) {
        setTimeout(resolve, 100);
        clearInterval(interval);
      }
    });
  });
}
const DEFAULT_APP_NAME = "unknown";
const DEFAULT_APP_VERSION = "unknown";
const DEFAULT_SYSTEM = "ethereum";
class SDK {
  constructor(options) {
    validateOptions(options);
    const {
      system = DEFAULT_SYSTEM,
      name = DEFAULT_APP_NAME,
      appVersion = DEFAULT_APP_VERSION,
      networkId,
      transactionHandlers = [],
      ws,
      onopen,
      ondown,
      onreopen,
      onerror,
      onclose
    } = options;
    const {
      apiUrl
    } = options;
    const {
      dappId
    } = options;
    const timeout2 = {
      connectTimeout: 1e4
    };
    const socket = new _default(apiUrl || "wss://api.blocknative.com/v0", ws ? {
      wsConstructor: ws,
      ...timeout2
    } : {
      ...timeout2
    });
    socket.onopen = onOpen.bind(this, onopen);
    socket.ondown = onDown.bind(this, ondown);
    socket.onreopen = onReopen.bind(this, onreopen);
    socket.onmessage = handleMessage.bind(this);
    socket.onerror = (error) => onerror && onerror({
      message: "There was a WebSocket error",
      error
    });
    socket.onclose = () => {
      this._pingTimeout && clearInterval(this._pingTimeout);
      onclose && onclose();
    };
    const storageKey = CryptoEs.SHA1(`${dappId} - ${name}`).toString();
    const storedConnectionId = isLocalStorageAvailable() && window.localStorage.getItem(storageKey);
    this._storageKey = storageKey;
    this._connectionId = storedConnectionId || void 0;
    this._dappId = dappId;
    this._system = system;
    this._networkId = networkId;
    this._appName = name;
    this._appVersion = appVersion;
    this._transactionHandlers = transactionHandlers;
    this._socket = socket;
    this._connected = false;
    this._sendMessage = sendMessage.bind(this);
    this._pingTimeout = void 0;
    this._destroyed = false;
    this._onerror = onerror;
    this._queuedMessages = [];
    this._limitRules = DEFAULT_RATE_LIMIT_RULES;
    this._waitToRetry = null;
    this._processingQueue = false;
    this._processQueue = processQueue.bind(this);
    if (this._socket.ws.on) {
      this._heartbeat = () => {
        this._pingTimeout && clearTimeout(this._pingTimeout);
        this._pingTimeout = setTimeout(() => {
          this._socket.ws.terminate();
        }, 3e4 + 1e3);
      };
      this._socket.ws.on("ping", () => {
        this._heartbeat && this._heartbeat();
      });
    }
    this.watchedTransactions = [];
    this.watchedAccounts = [];
    this.configurations = /* @__PURE__ */ new Map();
    this.transaction = transaction.bind(this);
    this.account = account.bind(this);
    this.event = event.bind(this);
    this.simulate = simulate.bind(this);
    this.multiSim = multiSim.bind(this);
    this.unsubscribe = unsubscribe.bind(this);
    this.configuration = configuration.bind(this);
    this.destroy = () => {
      this._socket.close();
      this._destroyed = true;
      this._socket.onclose();
    };
  }
  static multichain(options) {
    return new MultiChain(options, this);
  }
}
function onOpen(handler) {
  this._connected = true;
  const msg = {
    categoryCode: "initialize",
    eventCode: "checkDappId",
    connectionId: this._connectionId
  };
  this._socket.send(createEventLog.bind(this)(msg));
  this._heartbeat && this._heartbeat();
  handler && handler();
}
function onDown(handler, closeEvent) {
  this._connected = false;
  if (handler) {
    handler(closeEvent);
  }
  this._pingTimeout && clearTimeout(this._pingTimeout);
}
async function onReopen(handler) {
  this._connected = true;
  const msg = {
    categoryCode: "initialize",
    eventCode: "checkDappId",
    connectionId: this._connectionId
  };
  this._socket.send(createEventLog.bind(this)(msg));
  const configurations = Array.from(this.configurations.values());
  const globalConfiguration = this.configurations.get("global");
  if (globalConfiguration) {
    try {
      const {
        emitter,
        subscription,
        ...config2
      } = globalConfiguration;
      await this.configuration(config2);
    } catch (error) {
      console.warn("Error re-sending global configuration upon reconnection:", error);
    }
  }
  const addressConfigurations = configurations.filter(({
    scope
  }) => scope !== "global");
  addressConfigurations.forEach((enhancedConfig) => {
    const {
      emitter,
      subscription,
      ...config2
    } = enhancedConfig;
    this._sendMessage({
      categoryCode: "configs",
      eventCode: "put",
      config: config2
    });
  });
  this.watchedAccounts.forEach((account2) => {
    this._sendMessage({
      eventCode: "accountAddress",
      categoryCode: "watch",
      account: {
        address: account2.address
      }
    });
  });
  if (handler) {
    handler();
  }
  if (this._socket.ws && this._socket.ws.on) {
    this._socket.ws.on("ping", () => {
      this._heartbeat && this._heartbeat();
    });
    this._heartbeat();
  }
}
export {
  SDK as default
};
