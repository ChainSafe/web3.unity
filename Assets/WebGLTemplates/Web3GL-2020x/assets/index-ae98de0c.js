import { U as getAugmentedNamespace, m as buffer, V as browserExports } from "./index-cfcd831a.js";
import { d as detectEnv, e as detectOS, f as formatIOSMobile, b as getClientMeta, h as getCrypto, j as getCryptoOrThrow, k as getDocument, l as getDocumentOrThrow, n as getFromWindow, o as getFromWindowOrThrow, a as getLocal, p as getLocalStorage, q as getLocalStorageOrThrow, g as getLocation, t as getLocationOrThrow, u as getMobileLinkRegistry, v as getMobileRegistryEntry, w as getNavigator, x as getNavigatorOrThrow, y as isAndroid, i as isBrowser, z as isIOS, c as isMobile, A as isNode$1, m as mobileLinkChoiceKey, r as removeLocal, B as safeJsonParse, C as safeJsonStringify, D as saveMobileLinkInfo, s as setLocal } from "./mobile-4fed2fbb.js";
function _mergeNamespaces(n2, m2) {
  for (var i2 = 0; i2 < m2.length; i2++) {
    const e2 = m2[i2];
    if (typeof e2 !== "string" && !Array.isArray(e2)) {
      for (const k2 in e2) {
        if (k2 !== "default" && !(k2 in n2)) {
          const d2 = Object.getOwnPropertyDescriptor(e2, k2);
          if (d2) {
            Object.defineProperty(n2, k2, d2.get ? d2 : {
              enumerable: true,
              get: () => e2[k2]
            });
          }
        }
      }
    }
  }
  return Object.freeze(Object.defineProperty(n2, Symbol.toStringTag, { value: "Module" }));
}
const API_URL = "https://registry.walletconnect.com";
function getWalletRegistryUrl() {
  return API_URL + "/api/v2/wallets";
}
function getDappRegistryUrl() {
  return API_URL + "/api/v2/dapps";
}
function formatMobileRegistryEntry(entry, platform = "mobile") {
  var _a;
  return {
    name: entry.name || "",
    shortName: entry.metadata.shortName || "",
    color: entry.metadata.colors.primary || "",
    logo: (_a = entry.image_url.sm) !== null && _a !== void 0 ? _a : "",
    universalLink: entry[platform].universal || "",
    deepLink: entry[platform].native || ""
  };
}
function formatMobileRegistry(registry, platform = "mobile") {
  return Object.values(registry).filter((entry) => !!entry[platform].universal || !!entry[platform].native).map((entry) => formatMobileRegistryEntry(entry, platform));
}
const esm = /* @__PURE__ */ Object.freeze(/* @__PURE__ */ Object.defineProperty({
  __proto__: null,
  detectEnv,
  detectOS,
  formatIOSMobile,
  formatMobileRegistry,
  formatMobileRegistryEntry,
  getClientMeta,
  getCrypto,
  getCryptoOrThrow,
  getDappRegistryUrl,
  getDocument,
  getDocumentOrThrow,
  getFromWindow,
  getFromWindowOrThrow,
  getLocal,
  getLocalStorage,
  getLocalStorageOrThrow,
  getLocation,
  getLocationOrThrow,
  getMobileLinkRegistry,
  getMobileRegistryEntry,
  getNavigator,
  getNavigatorOrThrow,
  getWalletRegistryUrl,
  isAndroid,
  isBrowser,
  isIOS,
  isMobile,
  isNode: isNode$1,
  mobileLinkChoiceKey,
  removeLocal,
  safeJsonParse,
  safeJsonStringify,
  saveMobileLinkInfo,
  setLocal
}, Symbol.toStringTag, { value: "Module" }));
const require$$0 = /* @__PURE__ */ getAugmentedNamespace(esm);
var browser = {};
var canPromise$1 = function() {
  return typeof Promise === "function" && Promise.prototype && Promise.prototype.then;
};
var qrcode = {};
var typedarrayBuffer = {};
var toString = {}.toString;
var isarray = Array.isArray || function(arr) {
  return toString.call(arr) == "[object Array]";
};
var isArray$1 = isarray;
function typedArraySupport() {
  try {
    var arr = new Uint8Array(1);
    arr.__proto__ = { __proto__: Uint8Array.prototype, foo: function() {
      return 42;
    } };
    return arr.foo() === 42;
  } catch (e2) {
    return false;
  }
}
Buffer$1.TYPED_ARRAY_SUPPORT = typedArraySupport();
var K_MAX_LENGTH = Buffer$1.TYPED_ARRAY_SUPPORT ? 2147483647 : 1073741823;
function Buffer$1(arg, offset, length) {
  if (!Buffer$1.TYPED_ARRAY_SUPPORT && !(this instanceof Buffer$1)) {
    return new Buffer$1(arg, offset, length);
  }
  if (typeof arg === "number") {
    return allocUnsafe(this, arg);
  }
  return from(this, arg, offset, length);
}
if (Buffer$1.TYPED_ARRAY_SUPPORT) {
  Buffer$1.prototype.__proto__ = Uint8Array.prototype;
  Buffer$1.__proto__ = Uint8Array;
  if (typeof Symbol !== "undefined" && Symbol.species && Buffer$1[Symbol.species] === Buffer$1) {
    Object.defineProperty(Buffer$1, Symbol.species, {
      value: null,
      configurable: true,
      enumerable: false,
      writable: false
    });
  }
}
function checked(length) {
  if (length >= K_MAX_LENGTH) {
    throw new RangeError("Attempt to allocate Buffer larger than maximum size: 0x" + K_MAX_LENGTH.toString(16) + " bytes");
  }
  return length | 0;
}
function isnan(val) {
  return val !== val;
}
function createBuffer(that, length) {
  var buf;
  if (Buffer$1.TYPED_ARRAY_SUPPORT) {
    buf = new Uint8Array(length);
    buf.__proto__ = Buffer$1.prototype;
  } else {
    buf = that;
    if (buf === null) {
      buf = new Buffer$1(length);
    }
    buf.length = length;
  }
  return buf;
}
function allocUnsafe(that, size) {
  var buf = createBuffer(that, size < 0 ? 0 : checked(size) | 0);
  if (!Buffer$1.TYPED_ARRAY_SUPPORT) {
    for (var i2 = 0; i2 < size; ++i2) {
      buf[i2] = 0;
    }
  }
  return buf;
}
function fromString(that, string) {
  var length = byteLength(string) | 0;
  var buf = createBuffer(that, length);
  var actual = buf.write(string);
  if (actual !== length) {
    buf = buf.slice(0, actual);
  }
  return buf;
}
function fromArrayLike(that, array) {
  var length = array.length < 0 ? 0 : checked(array.length) | 0;
  var buf = createBuffer(that, length);
  for (var i2 = 0; i2 < length; i2 += 1) {
    buf[i2] = array[i2] & 255;
  }
  return buf;
}
function fromArrayBuffer(that, array, byteOffset, length) {
  if (byteOffset < 0 || array.byteLength < byteOffset) {
    throw new RangeError("'offset' is out of bounds");
  }
  if (array.byteLength < byteOffset + (length || 0)) {
    throw new RangeError("'length' is out of bounds");
  }
  var buf;
  if (byteOffset === void 0 && length === void 0) {
    buf = new Uint8Array(array);
  } else if (length === void 0) {
    buf = new Uint8Array(array, byteOffset);
  } else {
    buf = new Uint8Array(array, byteOffset, length);
  }
  if (Buffer$1.TYPED_ARRAY_SUPPORT) {
    buf.__proto__ = Buffer$1.prototype;
  } else {
    buf = fromArrayLike(that, buf);
  }
  return buf;
}
function fromObject(that, obj) {
  if (Buffer$1.isBuffer(obj)) {
    var len = checked(obj.length) | 0;
    var buf = createBuffer(that, len);
    if (buf.length === 0) {
      return buf;
    }
    obj.copy(buf, 0, 0, len);
    return buf;
  }
  if (obj) {
    if (typeof ArrayBuffer !== "undefined" && obj.buffer instanceof ArrayBuffer || "length" in obj) {
      if (typeof obj.length !== "number" || isnan(obj.length)) {
        return createBuffer(that, 0);
      }
      return fromArrayLike(that, obj);
    }
    if (obj.type === "Buffer" && Array.isArray(obj.data)) {
      return fromArrayLike(that, obj.data);
    }
  }
  throw new TypeError("First argument must be a string, Buffer, ArrayBuffer, Array, or array-like object.");
}
function utf8ToBytes(string, units) {
  units = units || Infinity;
  var codePoint;
  var length = string.length;
  var leadSurrogate = null;
  var bytes = [];
  for (var i2 = 0; i2 < length; ++i2) {
    codePoint = string.charCodeAt(i2);
    if (codePoint > 55295 && codePoint < 57344) {
      if (!leadSurrogate) {
        if (codePoint > 56319) {
          if ((units -= 3) > -1)
            bytes.push(239, 191, 189);
          continue;
        } else if (i2 + 1 === length) {
          if ((units -= 3) > -1)
            bytes.push(239, 191, 189);
          continue;
        }
        leadSurrogate = codePoint;
        continue;
      }
      if (codePoint < 56320) {
        if ((units -= 3) > -1)
          bytes.push(239, 191, 189);
        leadSurrogate = codePoint;
        continue;
      }
      codePoint = (leadSurrogate - 55296 << 10 | codePoint - 56320) + 65536;
    } else if (leadSurrogate) {
      if ((units -= 3) > -1)
        bytes.push(239, 191, 189);
    }
    leadSurrogate = null;
    if (codePoint < 128) {
      if ((units -= 1) < 0)
        break;
      bytes.push(codePoint);
    } else if (codePoint < 2048) {
      if ((units -= 2) < 0)
        break;
      bytes.push(
        codePoint >> 6 | 192,
        codePoint & 63 | 128
      );
    } else if (codePoint < 65536) {
      if ((units -= 3) < 0)
        break;
      bytes.push(
        codePoint >> 12 | 224,
        codePoint >> 6 & 63 | 128,
        codePoint & 63 | 128
      );
    } else if (codePoint < 1114112) {
      if ((units -= 4) < 0)
        break;
      bytes.push(
        codePoint >> 18 | 240,
        codePoint >> 12 & 63 | 128,
        codePoint >> 6 & 63 | 128,
        codePoint & 63 | 128
      );
    } else {
      throw new Error("Invalid code point");
    }
  }
  return bytes;
}
function byteLength(string) {
  if (Buffer$1.isBuffer(string)) {
    return string.length;
  }
  if (typeof ArrayBuffer !== "undefined" && typeof ArrayBuffer.isView === "function" && (ArrayBuffer.isView(string) || string instanceof ArrayBuffer)) {
    return string.byteLength;
  }
  if (typeof string !== "string") {
    string = "" + string;
  }
  var len = string.length;
  if (len === 0)
    return 0;
  return utf8ToBytes(string).length;
}
function blitBuffer(src, dst, offset, length) {
  for (var i2 = 0; i2 < length; ++i2) {
    if (i2 + offset >= dst.length || i2 >= src.length)
      break;
    dst[i2 + offset] = src[i2];
  }
  return i2;
}
function utf8Write(buf, string, offset, length) {
  return blitBuffer(utf8ToBytes(string, buf.length - offset), buf, offset, length);
}
function from(that, value, offset, length) {
  if (typeof value === "number") {
    throw new TypeError('"value" argument must not be a number');
  }
  if (typeof ArrayBuffer !== "undefined" && value instanceof ArrayBuffer) {
    return fromArrayBuffer(that, value, offset, length);
  }
  if (typeof value === "string") {
    return fromString(that, value);
  }
  return fromObject(that, value);
}
Buffer$1.prototype.write = function write(string, offset, length) {
  if (offset === void 0) {
    length = this.length;
    offset = 0;
  } else if (length === void 0 && typeof offset === "string") {
    length = this.length;
    offset = 0;
  } else if (isFinite(offset)) {
    offset = offset | 0;
    if (isFinite(length)) {
      length = length | 0;
    } else {
      length = void 0;
    }
  }
  var remaining = this.length - offset;
  if (length === void 0 || length > remaining)
    length = remaining;
  if (string.length > 0 && (length < 0 || offset < 0) || offset > this.length) {
    throw new RangeError("Attempt to write outside buffer bounds");
  }
  return utf8Write(this, string, offset, length);
};
Buffer$1.prototype.slice = function slice(start, end) {
  var len = this.length;
  start = ~~start;
  end = end === void 0 ? len : ~~end;
  if (start < 0) {
    start += len;
    if (start < 0)
      start = 0;
  } else if (start > len) {
    start = len;
  }
  if (end < 0) {
    end += len;
    if (end < 0)
      end = 0;
  } else if (end > len) {
    end = len;
  }
  if (end < start)
    end = start;
  var newBuf;
  if (Buffer$1.TYPED_ARRAY_SUPPORT) {
    newBuf = this.subarray(start, end);
    newBuf.__proto__ = Buffer$1.prototype;
  } else {
    var sliceLen = end - start;
    newBuf = new Buffer$1(sliceLen, void 0);
    for (var i2 = 0; i2 < sliceLen; ++i2) {
      newBuf[i2] = this[i2 + start];
    }
  }
  return newBuf;
};
Buffer$1.prototype.copy = function copy(target, targetStart, start, end) {
  if (!start)
    start = 0;
  if (!end && end !== 0)
    end = this.length;
  if (targetStart >= target.length)
    targetStart = target.length;
  if (!targetStart)
    targetStart = 0;
  if (end > 0 && end < start)
    end = start;
  if (end === start)
    return 0;
  if (target.length === 0 || this.length === 0)
    return 0;
  if (targetStart < 0) {
    throw new RangeError("targetStart out of bounds");
  }
  if (start < 0 || start >= this.length)
    throw new RangeError("sourceStart out of bounds");
  if (end < 0)
    throw new RangeError("sourceEnd out of bounds");
  if (end > this.length)
    end = this.length;
  if (target.length - targetStart < end - start) {
    end = target.length - targetStart + start;
  }
  var len = end - start;
  var i2;
  if (this === target && start < targetStart && targetStart < end) {
    for (i2 = len - 1; i2 >= 0; --i2) {
      target[i2 + targetStart] = this[i2 + start];
    }
  } else if (len < 1e3 || !Buffer$1.TYPED_ARRAY_SUPPORT) {
    for (i2 = 0; i2 < len; ++i2) {
      target[i2 + targetStart] = this[i2 + start];
    }
  } else {
    Uint8Array.prototype.set.call(
      target,
      this.subarray(start, start + len),
      targetStart
    );
  }
  return len;
};
Buffer$1.prototype.fill = function fill(val, start, end) {
  if (typeof val === "string") {
    if (typeof start === "string") {
      start = 0;
      end = this.length;
    } else if (typeof end === "string") {
      end = this.length;
    }
    if (val.length === 1) {
      var code = val.charCodeAt(0);
      if (code < 256) {
        val = code;
      }
    }
  } else if (typeof val === "number") {
    val = val & 255;
  }
  if (start < 0 || this.length < start || this.length < end) {
    throw new RangeError("Out of range index");
  }
  if (end <= start) {
    return this;
  }
  start = start >>> 0;
  end = end === void 0 ? this.length : end >>> 0;
  if (!val)
    val = 0;
  var i2;
  if (typeof val === "number") {
    for (i2 = start; i2 < end; ++i2) {
      this[i2] = val;
    }
  } else {
    var bytes = Buffer$1.isBuffer(val) ? val : new Buffer$1(val);
    var len = bytes.length;
    for (i2 = 0; i2 < end - start; ++i2) {
      this[i2 + start] = bytes[i2 % len];
    }
  }
  return this;
};
Buffer$1.concat = function concat(list, length) {
  if (!isArray$1(list)) {
    throw new TypeError('"list" argument must be an Array of Buffers');
  }
  if (list.length === 0) {
    return createBuffer(null, 0);
  }
  var i2;
  if (length === void 0) {
    length = 0;
    for (i2 = 0; i2 < list.length; ++i2) {
      length += list[i2].length;
    }
  }
  var buffer2 = allocUnsafe(null, length);
  var pos = 0;
  for (i2 = 0; i2 < list.length; ++i2) {
    var buf = list[i2];
    if (!Buffer$1.isBuffer(buf)) {
      throw new TypeError('"list" argument must be an Array of Buffers');
    }
    buf.copy(buffer2, pos);
    pos += buf.length;
  }
  return buffer2;
};
Buffer$1.byteLength = byteLength;
Buffer$1.prototype._isBuffer = true;
Buffer$1.isBuffer = function isBuffer(b2) {
  return !!(b2 != null && b2._isBuffer);
};
typedarrayBuffer.alloc = function(size) {
  var buffer2 = new Buffer$1(size);
  buffer2.fill(0);
  return buffer2;
};
typedarrayBuffer.from = function(data) {
  return new Buffer$1(data);
};
var utils$1 = {};
var toSJISFunction;
var CODEWORDS_COUNT = [
  0,
  // Not used
  26,
  44,
  70,
  100,
  134,
  172,
  196,
  242,
  292,
  346,
  404,
  466,
  532,
  581,
  655,
  733,
  815,
  901,
  991,
  1085,
  1156,
  1258,
  1364,
  1474,
  1588,
  1706,
  1828,
  1921,
  2051,
  2185,
  2323,
  2465,
  2611,
  2761,
  2876,
  3034,
  3196,
  3362,
  3532,
  3706
];
utils$1.getSymbolSize = function getSymbolSize(version2) {
  if (!version2)
    throw new Error('"version" cannot be null or undefined');
  if (version2 < 1 || version2 > 40)
    throw new Error('"version" should be in range from 1 to 40');
  return version2 * 4 + 17;
};
utils$1.getSymbolTotalCodewords = function getSymbolTotalCodewords(version2) {
  return CODEWORDS_COUNT[version2];
};
utils$1.getBCHDigit = function(data) {
  var digit = 0;
  while (data !== 0) {
    digit++;
    data >>>= 1;
  }
  return digit;
};
utils$1.setToSJISFunction = function setToSJISFunction(f2) {
  if (typeof f2 !== "function") {
    throw new Error('"toSJISFunc" is not a valid function.');
  }
  toSJISFunction = f2;
};
utils$1.isKanjiModeEnabled = function() {
  return typeof toSJISFunction !== "undefined";
};
utils$1.toSJIS = function toSJIS(kanji2) {
  return toSJISFunction(kanji2);
};
var errorCorrectionLevel = {};
(function(exports) {
  exports.L = { bit: 1 };
  exports.M = { bit: 0 };
  exports.Q = { bit: 3 };
  exports.H = { bit: 2 };
  function fromString2(string) {
    if (typeof string !== "string") {
      throw new Error("Param is not a string");
    }
    var lcStr = string.toLowerCase();
    switch (lcStr) {
      case "l":
      case "low":
        return exports.L;
      case "m":
      case "medium":
        return exports.M;
      case "q":
      case "quartile":
        return exports.Q;
      case "h":
      case "high":
        return exports.H;
      default:
        throw new Error("Unknown EC Level: " + string);
    }
  }
  exports.isValid = function isValid2(level) {
    return level && typeof level.bit !== "undefined" && level.bit >= 0 && level.bit < 4;
  };
  exports.from = function from2(value, defaultValue) {
    if (exports.isValid(value)) {
      return value;
    }
    try {
      return fromString2(value);
    } catch (e2) {
      return defaultValue;
    }
  };
})(errorCorrectionLevel);
function BitBuffer$1() {
  this.buffer = [];
  this.length = 0;
}
BitBuffer$1.prototype = {
  get: function(index2) {
    var bufIndex = Math.floor(index2 / 8);
    return (this.buffer[bufIndex] >>> 7 - index2 % 8 & 1) === 1;
  },
  put: function(num, length) {
    for (var i2 = 0; i2 < length; i2++) {
      this.putBit((num >>> length - i2 - 1 & 1) === 1);
    }
  },
  getLengthInBits: function() {
    return this.length;
  },
  putBit: function(bit) {
    var bufIndex = Math.floor(this.length / 8);
    if (this.buffer.length <= bufIndex) {
      this.buffer.push(0);
    }
    if (bit) {
      this.buffer[bufIndex] |= 128 >>> this.length % 8;
    }
    this.length++;
  }
};
var bitBuffer = BitBuffer$1;
var BufferUtil$4 = typedarrayBuffer;
function BitMatrix$1(size) {
  if (!size || size < 1) {
    throw new Error("BitMatrix size must be defined and greater than 0");
  }
  this.size = size;
  this.data = BufferUtil$4.alloc(size * size);
  this.reservedBit = BufferUtil$4.alloc(size * size);
}
BitMatrix$1.prototype.set = function(row, col, value, reserved) {
  var index2 = row * this.size + col;
  this.data[index2] = value;
  if (reserved)
    this.reservedBit[index2] = true;
};
BitMatrix$1.prototype.get = function(row, col) {
  return this.data[row * this.size + col];
};
BitMatrix$1.prototype.xor = function(row, col, value) {
  this.data[row * this.size + col] ^= value;
};
BitMatrix$1.prototype.isReserved = function(row, col) {
  return this.reservedBit[row * this.size + col];
};
var bitMatrix = BitMatrix$1;
var alignmentPattern = {};
(function(exports) {
  var getSymbolSize3 = utils$1.getSymbolSize;
  exports.getRowColCoords = function getRowColCoords(version2) {
    if (version2 === 1)
      return [];
    var posCount = Math.floor(version2 / 7) + 2;
    var size = getSymbolSize3(version2);
    var intervals = size === 145 ? 26 : Math.ceil((size - 13) / (2 * posCount - 2)) * 2;
    var positions = [size - 7];
    for (var i2 = 1; i2 < posCount - 1; i2++) {
      positions[i2] = positions[i2 - 1] - intervals;
    }
    positions.push(6);
    return positions.reverse();
  };
  exports.getPositions = function getPositions2(version2) {
    var coords = [];
    var pos = exports.getRowColCoords(version2);
    var posLength = pos.length;
    for (var i2 = 0; i2 < posLength; i2++) {
      for (var j2 = 0; j2 < posLength; j2++) {
        if (i2 === 0 && j2 === 0 || // top-left
        i2 === 0 && j2 === posLength - 1 || // bottom-left
        i2 === posLength - 1 && j2 === 0) {
          continue;
        }
        coords.push([pos[i2], pos[j2]]);
      }
    }
    return coords;
  };
})(alignmentPattern);
var finderPattern = {};
var getSymbolSize2 = utils$1.getSymbolSize;
var FINDER_PATTERN_SIZE = 7;
finderPattern.getPositions = function getPositions(version2) {
  var size = getSymbolSize2(version2);
  return [
    // top-left
    [0, 0],
    // top-right
    [size - FINDER_PATTERN_SIZE, 0],
    // bottom-left
    [0, size - FINDER_PATTERN_SIZE]
  ];
};
var maskPattern = {};
(function(exports) {
  exports.Patterns = {
    PATTERN000: 0,
    PATTERN001: 1,
    PATTERN010: 2,
    PATTERN011: 3,
    PATTERN100: 4,
    PATTERN101: 5,
    PATTERN110: 6,
    PATTERN111: 7
  };
  var PenaltyScores = {
    N1: 3,
    N2: 3,
    N3: 40,
    N4: 10
  };
  exports.isValid = function isValid2(mask) {
    return mask != null && mask !== "" && !isNaN(mask) && mask >= 0 && mask <= 7;
  };
  exports.from = function from2(value) {
    return exports.isValid(value) ? parseInt(value, 10) : void 0;
  };
  exports.getPenaltyN1 = function getPenaltyN1(data) {
    var size = data.size;
    var points = 0;
    var sameCountCol = 0;
    var sameCountRow = 0;
    var lastCol = null;
    var lastRow = null;
    for (var row = 0; row < size; row++) {
      sameCountCol = sameCountRow = 0;
      lastCol = lastRow = null;
      for (var col = 0; col < size; col++) {
        var module = data.get(row, col);
        if (module === lastCol) {
          sameCountCol++;
        } else {
          if (sameCountCol >= 5)
            points += PenaltyScores.N1 + (sameCountCol - 5);
          lastCol = module;
          sameCountCol = 1;
        }
        module = data.get(col, row);
        if (module === lastRow) {
          sameCountRow++;
        } else {
          if (sameCountRow >= 5)
            points += PenaltyScores.N1 + (sameCountRow - 5);
          lastRow = module;
          sameCountRow = 1;
        }
      }
      if (sameCountCol >= 5)
        points += PenaltyScores.N1 + (sameCountCol - 5);
      if (sameCountRow >= 5)
        points += PenaltyScores.N1 + (sameCountRow - 5);
    }
    return points;
  };
  exports.getPenaltyN2 = function getPenaltyN2(data) {
    var size = data.size;
    var points = 0;
    for (var row = 0; row < size - 1; row++) {
      for (var col = 0; col < size - 1; col++) {
        var last = data.get(row, col) + data.get(row, col + 1) + data.get(row + 1, col) + data.get(row + 1, col + 1);
        if (last === 4 || last === 0)
          points++;
      }
    }
    return points * PenaltyScores.N2;
  };
  exports.getPenaltyN3 = function getPenaltyN3(data) {
    var size = data.size;
    var points = 0;
    var bitsCol = 0;
    var bitsRow = 0;
    for (var row = 0; row < size; row++) {
      bitsCol = bitsRow = 0;
      for (var col = 0; col < size; col++) {
        bitsCol = bitsCol << 1 & 2047 | data.get(row, col);
        if (col >= 10 && (bitsCol === 1488 || bitsCol === 93))
          points++;
        bitsRow = bitsRow << 1 & 2047 | data.get(col, row);
        if (col >= 10 && (bitsRow === 1488 || bitsRow === 93))
          points++;
      }
    }
    return points * PenaltyScores.N3;
  };
  exports.getPenaltyN4 = function getPenaltyN4(data) {
    var darkCount = 0;
    var modulesCount = data.data.length;
    for (var i2 = 0; i2 < modulesCount; i2++)
      darkCount += data.data[i2];
    var k2 = Math.abs(Math.ceil(darkCount * 100 / modulesCount / 5) - 10);
    return k2 * PenaltyScores.N4;
  };
  function getMaskAt(maskPattern2, i2, j2) {
    switch (maskPattern2) {
      case exports.Patterns.PATTERN000:
        return (i2 + j2) % 2 === 0;
      case exports.Patterns.PATTERN001:
        return i2 % 2 === 0;
      case exports.Patterns.PATTERN010:
        return j2 % 3 === 0;
      case exports.Patterns.PATTERN011:
        return (i2 + j2) % 3 === 0;
      case exports.Patterns.PATTERN100:
        return (Math.floor(i2 / 2) + Math.floor(j2 / 3)) % 2 === 0;
      case exports.Patterns.PATTERN101:
        return i2 * j2 % 2 + i2 * j2 % 3 === 0;
      case exports.Patterns.PATTERN110:
        return (i2 * j2 % 2 + i2 * j2 % 3) % 2 === 0;
      case exports.Patterns.PATTERN111:
        return (i2 * j2 % 3 + (i2 + j2) % 2) % 2 === 0;
      default:
        throw new Error("bad maskPattern:" + maskPattern2);
    }
  }
  exports.applyMask = function applyMask(pattern, data) {
    var size = data.size;
    for (var col = 0; col < size; col++) {
      for (var row = 0; row < size; row++) {
        if (data.isReserved(row, col))
          continue;
        data.xor(row, col, getMaskAt(pattern, row, col));
      }
    }
  };
  exports.getBestMask = function getBestMask(data, setupFormatFunc) {
    var numPatterns = Object.keys(exports.Patterns).length;
    var bestPattern = 0;
    var lowerPenalty = Infinity;
    for (var p2 = 0; p2 < numPatterns; p2++) {
      setupFormatFunc(p2);
      exports.applyMask(p2, data);
      var penalty = exports.getPenaltyN1(data) + exports.getPenaltyN2(data) + exports.getPenaltyN3(data) + exports.getPenaltyN4(data);
      exports.applyMask(p2, data);
      if (penalty < lowerPenalty) {
        lowerPenalty = penalty;
        bestPattern = p2;
      }
    }
    return bestPattern;
  };
})(maskPattern);
var errorCorrectionCode = {};
var ECLevel$1 = errorCorrectionLevel;
var EC_BLOCKS_TABLE = [
  // L  M  Q  H
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  2,
  2,
  1,
  2,
  2,
  4,
  1,
  2,
  4,
  4,
  2,
  4,
  4,
  4,
  2,
  4,
  6,
  5,
  2,
  4,
  6,
  6,
  2,
  5,
  8,
  8,
  4,
  5,
  8,
  8,
  4,
  5,
  8,
  11,
  4,
  8,
  10,
  11,
  4,
  9,
  12,
  16,
  4,
  9,
  16,
  16,
  6,
  10,
  12,
  18,
  6,
  10,
  17,
  16,
  6,
  11,
  16,
  19,
  6,
  13,
  18,
  21,
  7,
  14,
  21,
  25,
  8,
  16,
  20,
  25,
  8,
  17,
  23,
  25,
  9,
  17,
  23,
  34,
  9,
  18,
  25,
  30,
  10,
  20,
  27,
  32,
  12,
  21,
  29,
  35,
  12,
  23,
  34,
  37,
  12,
  25,
  34,
  40,
  13,
  26,
  35,
  42,
  14,
  28,
  38,
  45,
  15,
  29,
  40,
  48,
  16,
  31,
  43,
  51,
  17,
  33,
  45,
  54,
  18,
  35,
  48,
  57,
  19,
  37,
  51,
  60,
  19,
  38,
  53,
  63,
  20,
  40,
  56,
  66,
  21,
  43,
  59,
  70,
  22,
  45,
  62,
  74,
  24,
  47,
  65,
  77,
  25,
  49,
  68,
  81
];
var EC_CODEWORDS_TABLE = [
  // L  M  Q  H
  7,
  10,
  13,
  17,
  10,
  16,
  22,
  28,
  15,
  26,
  36,
  44,
  20,
  36,
  52,
  64,
  26,
  48,
  72,
  88,
  36,
  64,
  96,
  112,
  40,
  72,
  108,
  130,
  48,
  88,
  132,
  156,
  60,
  110,
  160,
  192,
  72,
  130,
  192,
  224,
  80,
  150,
  224,
  264,
  96,
  176,
  260,
  308,
  104,
  198,
  288,
  352,
  120,
  216,
  320,
  384,
  132,
  240,
  360,
  432,
  144,
  280,
  408,
  480,
  168,
  308,
  448,
  532,
  180,
  338,
  504,
  588,
  196,
  364,
  546,
  650,
  224,
  416,
  600,
  700,
  224,
  442,
  644,
  750,
  252,
  476,
  690,
  816,
  270,
  504,
  750,
  900,
  300,
  560,
  810,
  960,
  312,
  588,
  870,
  1050,
  336,
  644,
  952,
  1110,
  360,
  700,
  1020,
  1200,
  390,
  728,
  1050,
  1260,
  420,
  784,
  1140,
  1350,
  450,
  812,
  1200,
  1440,
  480,
  868,
  1290,
  1530,
  510,
  924,
  1350,
  1620,
  540,
  980,
  1440,
  1710,
  570,
  1036,
  1530,
  1800,
  570,
  1064,
  1590,
  1890,
  600,
  1120,
  1680,
  1980,
  630,
  1204,
  1770,
  2100,
  660,
  1260,
  1860,
  2220,
  720,
  1316,
  1950,
  2310,
  750,
  1372,
  2040,
  2430
];
errorCorrectionCode.getBlocksCount = function getBlocksCount(version2, errorCorrectionLevel2) {
  switch (errorCorrectionLevel2) {
    case ECLevel$1.L:
      return EC_BLOCKS_TABLE[(version2 - 1) * 4 + 0];
    case ECLevel$1.M:
      return EC_BLOCKS_TABLE[(version2 - 1) * 4 + 1];
    case ECLevel$1.Q:
      return EC_BLOCKS_TABLE[(version2 - 1) * 4 + 2];
    case ECLevel$1.H:
      return EC_BLOCKS_TABLE[(version2 - 1) * 4 + 3];
    default:
      return void 0;
  }
};
errorCorrectionCode.getTotalCodewordsCount = function getTotalCodewordsCount(version2, errorCorrectionLevel2) {
  switch (errorCorrectionLevel2) {
    case ECLevel$1.L:
      return EC_CODEWORDS_TABLE[(version2 - 1) * 4 + 0];
    case ECLevel$1.M:
      return EC_CODEWORDS_TABLE[(version2 - 1) * 4 + 1];
    case ECLevel$1.Q:
      return EC_CODEWORDS_TABLE[(version2 - 1) * 4 + 2];
    case ECLevel$1.H:
      return EC_CODEWORDS_TABLE[(version2 - 1) * 4 + 3];
    default:
      return void 0;
  }
};
var polynomial = {};
var galoisField = {};
var BufferUtil$3 = typedarrayBuffer;
var EXP_TABLE = BufferUtil$3.alloc(512);
var LOG_TABLE = BufferUtil$3.alloc(256);
(function initTables() {
  var x2 = 1;
  for (var i2 = 0; i2 < 255; i2++) {
    EXP_TABLE[i2] = x2;
    LOG_TABLE[x2] = i2;
    x2 <<= 1;
    if (x2 & 256) {
      x2 ^= 285;
    }
  }
  for (i2 = 255; i2 < 512; i2++) {
    EXP_TABLE[i2] = EXP_TABLE[i2 - 255];
  }
})();
galoisField.log = function log(n2) {
  if (n2 < 1)
    throw new Error("log(" + n2 + ")");
  return LOG_TABLE[n2];
};
galoisField.exp = function exp(n2) {
  return EXP_TABLE[n2];
};
galoisField.mul = function mul(x2, y2) {
  if (x2 === 0 || y2 === 0)
    return 0;
  return EXP_TABLE[LOG_TABLE[x2] + LOG_TABLE[y2]];
};
(function(exports) {
  var BufferUtil2 = typedarrayBuffer;
  var GF = galoisField;
  exports.mul = function mul2(p1, p2) {
    var coeff = BufferUtil2.alloc(p1.length + p2.length - 1);
    for (var i2 = 0; i2 < p1.length; i2++) {
      for (var j2 = 0; j2 < p2.length; j2++) {
        coeff[i2 + j2] ^= GF.mul(p1[i2], p2[j2]);
      }
    }
    return coeff;
  };
  exports.mod = function mod(divident, divisor) {
    var result = BufferUtil2.from(divident);
    while (result.length - divisor.length >= 0) {
      var coeff = result[0];
      for (var i2 = 0; i2 < divisor.length; i2++) {
        result[i2] ^= GF.mul(divisor[i2], coeff);
      }
      var offset = 0;
      while (offset < result.length && result[offset] === 0)
        offset++;
      result = result.slice(offset);
    }
    return result;
  };
  exports.generateECPolynomial = function generateECPolynomial(degree) {
    var poly = BufferUtil2.from([1]);
    for (var i2 = 0; i2 < degree; i2++) {
      poly = exports.mul(poly, [1, GF.exp(i2)]);
    }
    return poly;
  };
})(polynomial);
var BufferUtil$2 = typedarrayBuffer;
var Polynomial = polynomial;
var Buffer = buffer.Buffer;
function ReedSolomonEncoder$1(degree) {
  this.genPoly = void 0;
  this.degree = degree;
  if (this.degree)
    this.initialize(this.degree);
}
ReedSolomonEncoder$1.prototype.initialize = function initialize(degree) {
  this.degree = degree;
  this.genPoly = Polynomial.generateECPolynomial(this.degree);
};
ReedSolomonEncoder$1.prototype.encode = function encode(data) {
  if (!this.genPoly) {
    throw new Error("Encoder not initialized");
  }
  var pad = BufferUtil$2.alloc(this.degree);
  var paddedData = Buffer.concat([data, pad], data.length + this.degree);
  var remainder = Polynomial.mod(paddedData, this.genPoly);
  var start = this.degree - remainder.length;
  if (start > 0) {
    var buff = BufferUtil$2.alloc(this.degree);
    remainder.copy(buff, start);
    return buff;
  }
  return remainder;
};
var reedSolomonEncoder = ReedSolomonEncoder$1;
var version = {};
var mode = {};
var versionCheck = {};
versionCheck.isValid = function isValid(version2) {
  return !isNaN(version2) && version2 >= 1 && version2 <= 40;
};
var regex = {};
var numeric = "[0-9]+";
var alphanumeric = "[A-Z $%*+\\-./:]+";
var kanji = "(?:[u3000-u303F]|[u3040-u309F]|[u30A0-u30FF]|[uFF00-uFFEF]|[u4E00-u9FAF]|[u2605-u2606]|[u2190-u2195]|u203B|[u2010u2015u2018u2019u2025u2026u201Cu201Du2225u2260]|[u0391-u0451]|[u00A7u00A8u00B1u00B4u00D7u00F7])+";
kanji = kanji.replace(/u/g, "\\u");
var byte = "(?:(?![A-Z0-9 $%*+\\-./:]|" + kanji + ")(?:.|[\r\n]))+";
regex.KANJI = new RegExp(kanji, "g");
regex.BYTE_KANJI = new RegExp("[^A-Z0-9 $%*+\\-./:]+", "g");
regex.BYTE = new RegExp(byte, "g");
regex.NUMERIC = new RegExp(numeric, "g");
regex.ALPHANUMERIC = new RegExp(alphanumeric, "g");
var TEST_KANJI = new RegExp("^" + kanji + "$");
var TEST_NUMERIC = new RegExp("^" + numeric + "$");
var TEST_ALPHANUMERIC = new RegExp("^[A-Z0-9 $%*+\\-./:]+$");
regex.testKanji = function testKanji(str) {
  return TEST_KANJI.test(str);
};
regex.testNumeric = function testNumeric(str) {
  return TEST_NUMERIC.test(str);
};
regex.testAlphanumeric = function testAlphanumeric(str) {
  return TEST_ALPHANUMERIC.test(str);
};
(function(exports) {
  var VersionCheck = versionCheck;
  var Regex = regex;
  exports.NUMERIC = {
    id: "Numeric",
    bit: 1 << 0,
    ccBits: [10, 12, 14]
  };
  exports.ALPHANUMERIC = {
    id: "Alphanumeric",
    bit: 1 << 1,
    ccBits: [9, 11, 13]
  };
  exports.BYTE = {
    id: "Byte",
    bit: 1 << 2,
    ccBits: [8, 16, 16]
  };
  exports.KANJI = {
    id: "Kanji",
    bit: 1 << 3,
    ccBits: [8, 10, 12]
  };
  exports.MIXED = {
    bit: -1
  };
  exports.getCharCountIndicator = function getCharCountIndicator(mode2, version2) {
    if (!mode2.ccBits)
      throw new Error("Invalid mode: " + mode2);
    if (!VersionCheck.isValid(version2)) {
      throw new Error("Invalid version: " + version2);
    }
    if (version2 >= 1 && version2 < 10)
      return mode2.ccBits[0];
    else if (version2 < 27)
      return mode2.ccBits[1];
    return mode2.ccBits[2];
  };
  exports.getBestModeForData = function getBestModeForData(dataStr) {
    if (Regex.testNumeric(dataStr))
      return exports.NUMERIC;
    else if (Regex.testAlphanumeric(dataStr))
      return exports.ALPHANUMERIC;
    else if (Regex.testKanji(dataStr))
      return exports.KANJI;
    else
      return exports.BYTE;
  };
  exports.toString = function toString2(mode2) {
    if (mode2 && mode2.id)
      return mode2.id;
    throw new Error("Invalid mode");
  };
  exports.isValid = function isValid2(mode2) {
    return mode2 && mode2.bit && mode2.ccBits;
  };
  function fromString2(string) {
    if (typeof string !== "string") {
      throw new Error("Param is not a string");
    }
    var lcStr = string.toLowerCase();
    switch (lcStr) {
      case "numeric":
        return exports.NUMERIC;
      case "alphanumeric":
        return exports.ALPHANUMERIC;
      case "kanji":
        return exports.KANJI;
      case "byte":
        return exports.BYTE;
      default:
        throw new Error("Unknown mode: " + string);
    }
  }
  exports.from = function from2(value, defaultValue) {
    if (exports.isValid(value)) {
      return value;
    }
    try {
      return fromString2(value);
    } catch (e2) {
      return defaultValue;
    }
  };
})(mode);
(function(exports) {
  var Utils2 = utils$1;
  var ECCode2 = errorCorrectionCode;
  var ECLevel2 = errorCorrectionLevel;
  var Mode2 = mode;
  var VersionCheck = versionCheck;
  var isArray2 = isarray;
  var G18 = 1 << 12 | 1 << 11 | 1 << 10 | 1 << 9 | 1 << 8 | 1 << 5 | 1 << 2 | 1 << 0;
  var G18_BCH = Utils2.getBCHDigit(G18);
  function getBestVersionForDataLength(mode2, length, errorCorrectionLevel2) {
    for (var currentVersion = 1; currentVersion <= 40; currentVersion++) {
      if (length <= exports.getCapacity(currentVersion, errorCorrectionLevel2, mode2)) {
        return currentVersion;
      }
    }
    return void 0;
  }
  function getReservedBitsCount(mode2, version2) {
    return Mode2.getCharCountIndicator(mode2, version2) + 4;
  }
  function getTotalBitsFromDataArray(segments2, version2) {
    var totalBits = 0;
    segments2.forEach(function(data) {
      var reservedBits = getReservedBitsCount(data.mode, version2);
      totalBits += reservedBits + data.getBitsLength();
    });
    return totalBits;
  }
  function getBestVersionForMixedData(segments2, errorCorrectionLevel2) {
    for (var currentVersion = 1; currentVersion <= 40; currentVersion++) {
      var length = getTotalBitsFromDataArray(segments2, currentVersion);
      if (length <= exports.getCapacity(currentVersion, errorCorrectionLevel2, Mode2.MIXED)) {
        return currentVersion;
      }
    }
    return void 0;
  }
  exports.from = function from2(value, defaultValue) {
    if (VersionCheck.isValid(value)) {
      return parseInt(value, 10);
    }
    return defaultValue;
  };
  exports.getCapacity = function getCapacity(version2, errorCorrectionLevel2, mode2) {
    if (!VersionCheck.isValid(version2)) {
      throw new Error("Invalid QR Code version");
    }
    if (typeof mode2 === "undefined")
      mode2 = Mode2.BYTE;
    var totalCodewords = Utils2.getSymbolTotalCodewords(version2);
    var ecTotalCodewords = ECCode2.getTotalCodewordsCount(version2, errorCorrectionLevel2);
    var dataTotalCodewordsBits = (totalCodewords - ecTotalCodewords) * 8;
    if (mode2 === Mode2.MIXED)
      return dataTotalCodewordsBits;
    var usableBits = dataTotalCodewordsBits - getReservedBitsCount(mode2, version2);
    switch (mode2) {
      case Mode2.NUMERIC:
        return Math.floor(usableBits / 10 * 3);
      case Mode2.ALPHANUMERIC:
        return Math.floor(usableBits / 11 * 2);
      case Mode2.KANJI:
        return Math.floor(usableBits / 13);
      case Mode2.BYTE:
      default:
        return Math.floor(usableBits / 8);
    }
  };
  exports.getBestVersionForData = function getBestVersionForData(data, errorCorrectionLevel2) {
    var seg;
    var ecl = ECLevel2.from(errorCorrectionLevel2, ECLevel2.M);
    if (isArray2(data)) {
      if (data.length > 1) {
        return getBestVersionForMixedData(data, ecl);
      }
      if (data.length === 0) {
        return 1;
      }
      seg = data[0];
    } else {
      seg = data;
    }
    return getBestVersionForDataLength(seg.mode, seg.getLength(), ecl);
  };
  exports.getEncodedBits = function getEncodedBits2(version2) {
    if (!VersionCheck.isValid(version2) || version2 < 7) {
      throw new Error("Invalid QR Code version");
    }
    var d2 = version2 << 12;
    while (Utils2.getBCHDigit(d2) - G18_BCH >= 0) {
      d2 ^= G18 << Utils2.getBCHDigit(d2) - G18_BCH;
    }
    return version2 << 12 | d2;
  };
})(version);
var formatInfo = {};
var Utils$3 = utils$1;
var G15 = 1 << 10 | 1 << 8 | 1 << 5 | 1 << 4 | 1 << 2 | 1 << 1 | 1 << 0;
var G15_MASK = 1 << 14 | 1 << 12 | 1 << 10 | 1 << 4 | 1 << 1;
var G15_BCH = Utils$3.getBCHDigit(G15);
formatInfo.getEncodedBits = function getEncodedBits(errorCorrectionLevel2, mask) {
  var data = errorCorrectionLevel2.bit << 3 | mask;
  var d2 = data << 10;
  while (Utils$3.getBCHDigit(d2) - G15_BCH >= 0) {
    d2 ^= G15 << Utils$3.getBCHDigit(d2) - G15_BCH;
  }
  return (data << 10 | d2) ^ G15_MASK;
};
var segments = {};
var Mode$4 = mode;
function NumericData(data) {
  this.mode = Mode$4.NUMERIC;
  this.data = data.toString();
}
NumericData.getBitsLength = function getBitsLength(length) {
  return 10 * Math.floor(length / 3) + (length % 3 ? length % 3 * 3 + 1 : 0);
};
NumericData.prototype.getLength = function getLength() {
  return this.data.length;
};
NumericData.prototype.getBitsLength = function getBitsLength2() {
  return NumericData.getBitsLength(this.data.length);
};
NumericData.prototype.write = function write2(bitBuffer2) {
  var i2, group, value;
  for (i2 = 0; i2 + 3 <= this.data.length; i2 += 3) {
    group = this.data.substr(i2, 3);
    value = parseInt(group, 10);
    bitBuffer2.put(value, 10);
  }
  var remainingNum = this.data.length - i2;
  if (remainingNum > 0) {
    group = this.data.substr(i2);
    value = parseInt(group, 10);
    bitBuffer2.put(value, remainingNum * 3 + 1);
  }
};
var numericData = NumericData;
var Mode$3 = mode;
var ALPHA_NUM_CHARS = [
  "0",
  "1",
  "2",
  "3",
  "4",
  "5",
  "6",
  "7",
  "8",
  "9",
  "A",
  "B",
  "C",
  "D",
  "E",
  "F",
  "G",
  "H",
  "I",
  "J",
  "K",
  "L",
  "M",
  "N",
  "O",
  "P",
  "Q",
  "R",
  "S",
  "T",
  "U",
  "V",
  "W",
  "X",
  "Y",
  "Z",
  " ",
  "$",
  "%",
  "*",
  "+",
  "-",
  ".",
  "/",
  ":"
];
function AlphanumericData(data) {
  this.mode = Mode$3.ALPHANUMERIC;
  this.data = data;
}
AlphanumericData.getBitsLength = function getBitsLength3(length) {
  return 11 * Math.floor(length / 2) + 6 * (length % 2);
};
AlphanumericData.prototype.getLength = function getLength2() {
  return this.data.length;
};
AlphanumericData.prototype.getBitsLength = function getBitsLength4() {
  return AlphanumericData.getBitsLength(this.data.length);
};
AlphanumericData.prototype.write = function write3(bitBuffer2) {
  var i2;
  for (i2 = 0; i2 + 2 <= this.data.length; i2 += 2) {
    var value = ALPHA_NUM_CHARS.indexOf(this.data[i2]) * 45;
    value += ALPHA_NUM_CHARS.indexOf(this.data[i2 + 1]);
    bitBuffer2.put(value, 11);
  }
  if (this.data.length % 2) {
    bitBuffer2.put(ALPHA_NUM_CHARS.indexOf(this.data[i2]), 6);
  }
};
var alphanumericData = AlphanumericData;
var BufferUtil$1 = typedarrayBuffer;
var Mode$2 = mode;
function ByteData(data) {
  this.mode = Mode$2.BYTE;
  this.data = BufferUtil$1.from(data);
}
ByteData.getBitsLength = function getBitsLength5(length) {
  return length * 8;
};
ByteData.prototype.getLength = function getLength3() {
  return this.data.length;
};
ByteData.prototype.getBitsLength = function getBitsLength6() {
  return ByteData.getBitsLength(this.data.length);
};
ByteData.prototype.write = function(bitBuffer2) {
  for (var i2 = 0, l2 = this.data.length; i2 < l2; i2++) {
    bitBuffer2.put(this.data[i2], 8);
  }
};
var byteData = ByteData;
var Mode$1 = mode;
var Utils$2 = utils$1;
function KanjiData(data) {
  this.mode = Mode$1.KANJI;
  this.data = data;
}
KanjiData.getBitsLength = function getBitsLength7(length) {
  return length * 13;
};
KanjiData.prototype.getLength = function getLength4() {
  return this.data.length;
};
KanjiData.prototype.getBitsLength = function getBitsLength8() {
  return KanjiData.getBitsLength(this.data.length);
};
KanjiData.prototype.write = function(bitBuffer2) {
  var i2;
  for (i2 = 0; i2 < this.data.length; i2++) {
    var value = Utils$2.toSJIS(this.data[i2]);
    if (value >= 33088 && value <= 40956) {
      value -= 33088;
    } else if (value >= 57408 && value <= 60351) {
      value -= 49472;
    } else {
      throw new Error(
        "Invalid SJIS character: " + this.data[i2] + "\nMake sure your charset is UTF-8"
      );
    }
    value = (value >>> 8 & 255) * 192 + (value & 255);
    bitBuffer2.put(value, 13);
  }
};
var kanjiData = KanjiData;
var dijkstraExports = {};
var dijkstra = {
  get exports() {
    return dijkstraExports;
  },
  set exports(v2) {
    dijkstraExports = v2;
  }
};
(function(module) {
  var dijkstra2 = {
    single_source_shortest_paths: function(graph, s2, d2) {
      var predecessors = {};
      var costs = {};
      costs[s2] = 0;
      var open2 = dijkstra2.PriorityQueue.make();
      open2.push(s2, 0);
      var closest, u2, v2, cost_of_s_to_u, adjacent_nodes, cost_of_e, cost_of_s_to_u_plus_cost_of_e, cost_of_s_to_v, first_visit;
      while (!open2.empty()) {
        closest = open2.pop();
        u2 = closest.value;
        cost_of_s_to_u = closest.cost;
        adjacent_nodes = graph[u2] || {};
        for (v2 in adjacent_nodes) {
          if (adjacent_nodes.hasOwnProperty(v2)) {
            cost_of_e = adjacent_nodes[v2];
            cost_of_s_to_u_plus_cost_of_e = cost_of_s_to_u + cost_of_e;
            cost_of_s_to_v = costs[v2];
            first_visit = typeof costs[v2] === "undefined";
            if (first_visit || cost_of_s_to_v > cost_of_s_to_u_plus_cost_of_e) {
              costs[v2] = cost_of_s_to_u_plus_cost_of_e;
              open2.push(v2, cost_of_s_to_u_plus_cost_of_e);
              predecessors[v2] = u2;
            }
          }
        }
      }
      if (typeof d2 !== "undefined" && typeof costs[d2] === "undefined") {
        var msg = ["Could not find a path from ", s2, " to ", d2, "."].join("");
        throw new Error(msg);
      }
      return predecessors;
    },
    extract_shortest_path_from_predecessor_list: function(predecessors, d2) {
      var nodes = [];
      var u2 = d2;
      while (u2) {
        nodes.push(u2);
        predecessors[u2];
        u2 = predecessors[u2];
      }
      nodes.reverse();
      return nodes;
    },
    find_path: function(graph, s2, d2) {
      var predecessors = dijkstra2.single_source_shortest_paths(graph, s2, d2);
      return dijkstra2.extract_shortest_path_from_predecessor_list(
        predecessors,
        d2
      );
    },
    /**
     * A very naive priority queue implementation.
     */
    PriorityQueue: {
      make: function(opts) {
        var T2 = dijkstra2.PriorityQueue, t2 = {}, key;
        opts = opts || {};
        for (key in T2) {
          if (T2.hasOwnProperty(key)) {
            t2[key] = T2[key];
          }
        }
        t2.queue = [];
        t2.sorter = opts.sorter || T2.default_sorter;
        return t2;
      },
      default_sorter: function(a2, b2) {
        return a2.cost - b2.cost;
      },
      /**
       * Add a new item to the queue and ensure the highest priority element
       * is at the front of the queue.
       */
      push: function(value, cost) {
        var item = { value, cost };
        this.queue.push(item);
        this.queue.sort(this.sorter);
      },
      /**
       * Return the highest priority element in the queue.
       */
      pop: function() {
        return this.queue.shift();
      },
      empty: function() {
        return this.queue.length === 0;
      }
    }
  };
  {
    module.exports = dijkstra2;
  }
})(dijkstra);
(function(exports) {
  var Mode2 = mode;
  var NumericData2 = numericData;
  var AlphanumericData2 = alphanumericData;
  var ByteData2 = byteData;
  var KanjiData2 = kanjiData;
  var Regex = regex;
  var Utils2 = utils$1;
  var dijkstra2 = dijkstraExports;
  function getStringByteLength(str) {
    return unescape(encodeURIComponent(str)).length;
  }
  function getSegments(regex2, mode2, str) {
    var segments2 = [];
    var result;
    while ((result = regex2.exec(str)) !== null) {
      segments2.push({
        data: result[0],
        index: result.index,
        mode: mode2,
        length: result[0].length
      });
    }
    return segments2;
  }
  function getSegmentsFromString(dataStr) {
    var numSegs = getSegments(Regex.NUMERIC, Mode2.NUMERIC, dataStr);
    var alphaNumSegs = getSegments(Regex.ALPHANUMERIC, Mode2.ALPHANUMERIC, dataStr);
    var byteSegs;
    var kanjiSegs;
    if (Utils2.isKanjiModeEnabled()) {
      byteSegs = getSegments(Regex.BYTE, Mode2.BYTE, dataStr);
      kanjiSegs = getSegments(Regex.KANJI, Mode2.KANJI, dataStr);
    } else {
      byteSegs = getSegments(Regex.BYTE_KANJI, Mode2.BYTE, dataStr);
      kanjiSegs = [];
    }
    var segs = numSegs.concat(alphaNumSegs, byteSegs, kanjiSegs);
    return segs.sort(function(s1, s2) {
      return s1.index - s2.index;
    }).map(function(obj) {
      return {
        data: obj.data,
        mode: obj.mode,
        length: obj.length
      };
    });
  }
  function getSegmentBitsLength(length, mode2) {
    switch (mode2) {
      case Mode2.NUMERIC:
        return NumericData2.getBitsLength(length);
      case Mode2.ALPHANUMERIC:
        return AlphanumericData2.getBitsLength(length);
      case Mode2.KANJI:
        return KanjiData2.getBitsLength(length);
      case Mode2.BYTE:
        return ByteData2.getBitsLength(length);
    }
  }
  function mergeSegments(segs) {
    return segs.reduce(function(acc, curr) {
      var prevSeg = acc.length - 1 >= 0 ? acc[acc.length - 1] : null;
      if (prevSeg && prevSeg.mode === curr.mode) {
        acc[acc.length - 1].data += curr.data;
        return acc;
      }
      acc.push(curr);
      return acc;
    }, []);
  }
  function buildNodes(segs) {
    var nodes = [];
    for (var i2 = 0; i2 < segs.length; i2++) {
      var seg = segs[i2];
      switch (seg.mode) {
        case Mode2.NUMERIC:
          nodes.push([
            seg,
            { data: seg.data, mode: Mode2.ALPHANUMERIC, length: seg.length },
            { data: seg.data, mode: Mode2.BYTE, length: seg.length }
          ]);
          break;
        case Mode2.ALPHANUMERIC:
          nodes.push([
            seg,
            { data: seg.data, mode: Mode2.BYTE, length: seg.length }
          ]);
          break;
        case Mode2.KANJI:
          nodes.push([
            seg,
            { data: seg.data, mode: Mode2.BYTE, length: getStringByteLength(seg.data) }
          ]);
          break;
        case Mode2.BYTE:
          nodes.push([
            { data: seg.data, mode: Mode2.BYTE, length: getStringByteLength(seg.data) }
          ]);
      }
    }
    return nodes;
  }
  function buildGraph(nodes, version2) {
    var table = {};
    var graph = { "start": {} };
    var prevNodeIds = ["start"];
    for (var i2 = 0; i2 < nodes.length; i2++) {
      var nodeGroup = nodes[i2];
      var currentNodeIds = [];
      for (var j2 = 0; j2 < nodeGroup.length; j2++) {
        var node = nodeGroup[j2];
        var key = "" + i2 + j2;
        currentNodeIds.push(key);
        table[key] = { node, lastCount: 0 };
        graph[key] = {};
        for (var n2 = 0; n2 < prevNodeIds.length; n2++) {
          var prevNodeId = prevNodeIds[n2];
          if (table[prevNodeId] && table[prevNodeId].node.mode === node.mode) {
            graph[prevNodeId][key] = getSegmentBitsLength(table[prevNodeId].lastCount + node.length, node.mode) - getSegmentBitsLength(table[prevNodeId].lastCount, node.mode);
            table[prevNodeId].lastCount += node.length;
          } else {
            if (table[prevNodeId])
              table[prevNodeId].lastCount = node.length;
            graph[prevNodeId][key] = getSegmentBitsLength(node.length, node.mode) + 4 + Mode2.getCharCountIndicator(node.mode, version2);
          }
        }
      }
      prevNodeIds = currentNodeIds;
    }
    for (n2 = 0; n2 < prevNodeIds.length; n2++) {
      graph[prevNodeIds[n2]]["end"] = 0;
    }
    return { map: graph, table };
  }
  function buildSingleSegment(data, modesHint) {
    var mode2;
    var bestMode = Mode2.getBestModeForData(data);
    mode2 = Mode2.from(modesHint, bestMode);
    if (mode2 !== Mode2.BYTE && mode2.bit < bestMode.bit) {
      throw new Error('"' + data + '" cannot be encoded with mode ' + Mode2.toString(mode2) + ".\n Suggested mode is: " + Mode2.toString(bestMode));
    }
    if (mode2 === Mode2.KANJI && !Utils2.isKanjiModeEnabled()) {
      mode2 = Mode2.BYTE;
    }
    switch (mode2) {
      case Mode2.NUMERIC:
        return new NumericData2(data);
      case Mode2.ALPHANUMERIC:
        return new AlphanumericData2(data);
      case Mode2.KANJI:
        return new KanjiData2(data);
      case Mode2.BYTE:
        return new ByteData2(data);
    }
  }
  exports.fromArray = function fromArray(array) {
    return array.reduce(function(acc, seg) {
      if (typeof seg === "string") {
        acc.push(buildSingleSegment(seg, null));
      } else if (seg.data) {
        acc.push(buildSingleSegment(seg.data, seg.mode));
      }
      return acc;
    }, []);
  };
  exports.fromString = function fromString2(data, version2) {
    var segs = getSegmentsFromString(data, Utils2.isKanjiModeEnabled());
    var nodes = buildNodes(segs);
    var graph = buildGraph(nodes, version2);
    var path = dijkstra2.find_path(graph.map, "start", "end");
    var optimizedSegs = [];
    for (var i2 = 1; i2 < path.length - 1; i2++) {
      optimizedSegs.push(graph.table[path[i2]].node);
    }
    return exports.fromArray(mergeSegments(optimizedSegs));
  };
  exports.rawSplit = function rawSplit(data) {
    return exports.fromArray(
      getSegmentsFromString(data, Utils2.isKanjiModeEnabled())
    );
  };
})(segments);
var BufferUtil = typedarrayBuffer;
var Utils$1 = utils$1;
var ECLevel = errorCorrectionLevel;
var BitBuffer = bitBuffer;
var BitMatrix = bitMatrix;
var AlignmentPattern = alignmentPattern;
var FinderPattern = finderPattern;
var MaskPattern = maskPattern;
var ECCode = errorCorrectionCode;
var ReedSolomonEncoder = reedSolomonEncoder;
var Version = version;
var FormatInfo = formatInfo;
var Mode = mode;
var Segments = segments;
var isArray = isarray;
function setupFinderPattern(matrix, version2) {
  var size = matrix.size;
  var pos = FinderPattern.getPositions(version2);
  for (var i2 = 0; i2 < pos.length; i2++) {
    var row = pos[i2][0];
    var col = pos[i2][1];
    for (var r2 = -1; r2 <= 7; r2++) {
      if (row + r2 <= -1 || size <= row + r2)
        continue;
      for (var c2 = -1; c2 <= 7; c2++) {
        if (col + c2 <= -1 || size <= col + c2)
          continue;
        if (r2 >= 0 && r2 <= 6 && (c2 === 0 || c2 === 6) || c2 >= 0 && c2 <= 6 && (r2 === 0 || r2 === 6) || r2 >= 2 && r2 <= 4 && c2 >= 2 && c2 <= 4) {
          matrix.set(row + r2, col + c2, true, true);
        } else {
          matrix.set(row + r2, col + c2, false, true);
        }
      }
    }
  }
}
function setupTimingPattern(matrix) {
  var size = matrix.size;
  for (var r2 = 8; r2 < size - 8; r2++) {
    var value = r2 % 2 === 0;
    matrix.set(r2, 6, value, true);
    matrix.set(6, r2, value, true);
  }
}
function setupAlignmentPattern(matrix, version2) {
  var pos = AlignmentPattern.getPositions(version2);
  for (var i2 = 0; i2 < pos.length; i2++) {
    var row = pos[i2][0];
    var col = pos[i2][1];
    for (var r2 = -2; r2 <= 2; r2++) {
      for (var c2 = -2; c2 <= 2; c2++) {
        if (r2 === -2 || r2 === 2 || c2 === -2 || c2 === 2 || r2 === 0 && c2 === 0) {
          matrix.set(row + r2, col + c2, true, true);
        } else {
          matrix.set(row + r2, col + c2, false, true);
        }
      }
    }
  }
}
function setupVersionInfo(matrix, version2) {
  var size = matrix.size;
  var bits = Version.getEncodedBits(version2);
  var row, col, mod;
  for (var i2 = 0; i2 < 18; i2++) {
    row = Math.floor(i2 / 3);
    col = i2 % 3 + size - 8 - 3;
    mod = (bits >> i2 & 1) === 1;
    matrix.set(row, col, mod, true);
    matrix.set(col, row, mod, true);
  }
}
function setupFormatInfo(matrix, errorCorrectionLevel2, maskPattern2) {
  var size = matrix.size;
  var bits = FormatInfo.getEncodedBits(errorCorrectionLevel2, maskPattern2);
  var i2, mod;
  for (i2 = 0; i2 < 15; i2++) {
    mod = (bits >> i2 & 1) === 1;
    if (i2 < 6) {
      matrix.set(i2, 8, mod, true);
    } else if (i2 < 8) {
      matrix.set(i2 + 1, 8, mod, true);
    } else {
      matrix.set(size - 15 + i2, 8, mod, true);
    }
    if (i2 < 8) {
      matrix.set(8, size - i2 - 1, mod, true);
    } else if (i2 < 9) {
      matrix.set(8, 15 - i2 - 1 + 1, mod, true);
    } else {
      matrix.set(8, 15 - i2 - 1, mod, true);
    }
  }
  matrix.set(size - 8, 8, 1, true);
}
function setupData(matrix, data) {
  var size = matrix.size;
  var inc = -1;
  var row = size - 1;
  var bitIndex = 7;
  var byteIndex = 0;
  for (var col = size - 1; col > 0; col -= 2) {
    if (col === 6)
      col--;
    while (true) {
      for (var c2 = 0; c2 < 2; c2++) {
        if (!matrix.isReserved(row, col - c2)) {
          var dark = false;
          if (byteIndex < data.length) {
            dark = (data[byteIndex] >>> bitIndex & 1) === 1;
          }
          matrix.set(row, col - c2, dark);
          bitIndex--;
          if (bitIndex === -1) {
            byteIndex++;
            bitIndex = 7;
          }
        }
      }
      row += inc;
      if (row < 0 || size <= row) {
        row -= inc;
        inc = -inc;
        break;
      }
    }
  }
}
function createData(version2, errorCorrectionLevel2, segments2) {
  var buffer2 = new BitBuffer();
  segments2.forEach(function(data) {
    buffer2.put(data.mode.bit, 4);
    buffer2.put(data.getLength(), Mode.getCharCountIndicator(data.mode, version2));
    data.write(buffer2);
  });
  var totalCodewords = Utils$1.getSymbolTotalCodewords(version2);
  var ecTotalCodewords = ECCode.getTotalCodewordsCount(version2, errorCorrectionLevel2);
  var dataTotalCodewordsBits = (totalCodewords - ecTotalCodewords) * 8;
  if (buffer2.getLengthInBits() + 4 <= dataTotalCodewordsBits) {
    buffer2.put(0, 4);
  }
  while (buffer2.getLengthInBits() % 8 !== 0) {
    buffer2.putBit(0);
  }
  var remainingByte = (dataTotalCodewordsBits - buffer2.getLengthInBits()) / 8;
  for (var i2 = 0; i2 < remainingByte; i2++) {
    buffer2.put(i2 % 2 ? 17 : 236, 8);
  }
  return createCodewords(buffer2, version2, errorCorrectionLevel2);
}
function createCodewords(bitBuffer2, version2, errorCorrectionLevel2) {
  var totalCodewords = Utils$1.getSymbolTotalCodewords(version2);
  var ecTotalCodewords = ECCode.getTotalCodewordsCount(version2, errorCorrectionLevel2);
  var dataTotalCodewords = totalCodewords - ecTotalCodewords;
  var ecTotalBlocks = ECCode.getBlocksCount(version2, errorCorrectionLevel2);
  var blocksInGroup2 = totalCodewords % ecTotalBlocks;
  var blocksInGroup1 = ecTotalBlocks - blocksInGroup2;
  var totalCodewordsInGroup1 = Math.floor(totalCodewords / ecTotalBlocks);
  var dataCodewordsInGroup1 = Math.floor(dataTotalCodewords / ecTotalBlocks);
  var dataCodewordsInGroup2 = dataCodewordsInGroup1 + 1;
  var ecCount = totalCodewordsInGroup1 - dataCodewordsInGroup1;
  var rs = new ReedSolomonEncoder(ecCount);
  var offset = 0;
  var dcData = new Array(ecTotalBlocks);
  var ecData = new Array(ecTotalBlocks);
  var maxDataSize = 0;
  var buffer2 = BufferUtil.from(bitBuffer2.buffer);
  for (var b2 = 0; b2 < ecTotalBlocks; b2++) {
    var dataSize = b2 < blocksInGroup1 ? dataCodewordsInGroup1 : dataCodewordsInGroup2;
    dcData[b2] = buffer2.slice(offset, offset + dataSize);
    ecData[b2] = rs.encode(dcData[b2]);
    offset += dataSize;
    maxDataSize = Math.max(maxDataSize, dataSize);
  }
  var data = BufferUtil.alloc(totalCodewords);
  var index2 = 0;
  var i2, r2;
  for (i2 = 0; i2 < maxDataSize; i2++) {
    for (r2 = 0; r2 < ecTotalBlocks; r2++) {
      if (i2 < dcData[r2].length) {
        data[index2++] = dcData[r2][i2];
      }
    }
  }
  for (i2 = 0; i2 < ecCount; i2++) {
    for (r2 = 0; r2 < ecTotalBlocks; r2++) {
      data[index2++] = ecData[r2][i2];
    }
  }
  return data;
}
function createSymbol(data, version2, errorCorrectionLevel2, maskPattern2) {
  var segments2;
  if (isArray(data)) {
    segments2 = Segments.fromArray(data);
  } else if (typeof data === "string") {
    var estimatedVersion = version2;
    if (!estimatedVersion) {
      var rawSegments = Segments.rawSplit(data);
      estimatedVersion = Version.getBestVersionForData(
        rawSegments,
        errorCorrectionLevel2
      );
    }
    segments2 = Segments.fromString(data, estimatedVersion || 40);
  } else {
    throw new Error("Invalid data");
  }
  var bestVersion = Version.getBestVersionForData(
    segments2,
    errorCorrectionLevel2
  );
  if (!bestVersion) {
    throw new Error("The amount of data is too big to be stored in a QR Code");
  }
  if (!version2) {
    version2 = bestVersion;
  } else if (version2 < bestVersion) {
    throw new Error(
      "\nThe chosen QR Code version cannot contain this amount of data.\nMinimum version required to store current data is: " + bestVersion + ".\n"
    );
  }
  var dataBits = createData(version2, errorCorrectionLevel2, segments2);
  var moduleCount = Utils$1.getSymbolSize(version2);
  var modules = new BitMatrix(moduleCount);
  setupFinderPattern(modules, version2);
  setupTimingPattern(modules);
  setupAlignmentPattern(modules, version2);
  setupFormatInfo(modules, errorCorrectionLevel2, 0);
  if (version2 >= 7) {
    setupVersionInfo(modules, version2);
  }
  setupData(modules, dataBits);
  if (isNaN(maskPattern2)) {
    maskPattern2 = MaskPattern.getBestMask(
      modules,
      setupFormatInfo.bind(null, modules, errorCorrectionLevel2)
    );
  }
  MaskPattern.applyMask(maskPattern2, modules);
  setupFormatInfo(modules, errorCorrectionLevel2, maskPattern2);
  return {
    modules,
    version: version2,
    errorCorrectionLevel: errorCorrectionLevel2,
    maskPattern: maskPattern2,
    segments: segments2
  };
}
qrcode.create = function create(data, options) {
  if (typeof data === "undefined" || data === "") {
    throw new Error("No input text");
  }
  var errorCorrectionLevel2 = ECLevel.M;
  var version2;
  var mask;
  if (typeof options !== "undefined") {
    errorCorrectionLevel2 = ECLevel.from(options.errorCorrectionLevel, ECLevel.M);
    version2 = Version.from(options.version);
    mask = MaskPattern.from(options.maskPattern);
    if (options.toSJISFunc) {
      Utils$1.setToSJISFunction(options.toSJISFunc);
    }
  }
  return createSymbol(data, version2, errorCorrectionLevel2, mask);
};
var canvas = {};
var utils = {};
(function(exports) {
  function hex2rgba(hex) {
    if (typeof hex === "number") {
      hex = hex.toString();
    }
    if (typeof hex !== "string") {
      throw new Error("Color should be defined as hex string");
    }
    var hexCode = hex.slice().replace("#", "").split("");
    if (hexCode.length < 3 || hexCode.length === 5 || hexCode.length > 8) {
      throw new Error("Invalid hex color: " + hex);
    }
    if (hexCode.length === 3 || hexCode.length === 4) {
      hexCode = Array.prototype.concat.apply([], hexCode.map(function(c2) {
        return [c2, c2];
      }));
    }
    if (hexCode.length === 6)
      hexCode.push("F", "F");
    var hexValue = parseInt(hexCode.join(""), 16);
    return {
      r: hexValue >> 24 & 255,
      g: hexValue >> 16 & 255,
      b: hexValue >> 8 & 255,
      a: hexValue & 255,
      hex: "#" + hexCode.slice(0, 6).join("")
    };
  }
  exports.getOptions = function getOptions(options) {
    if (!options)
      options = {};
    if (!options.color)
      options.color = {};
    var margin = typeof options.margin === "undefined" || options.margin === null || options.margin < 0 ? 4 : options.margin;
    var width = options.width && options.width >= 21 ? options.width : void 0;
    var scale = options.scale || 4;
    return {
      width,
      scale: width ? 4 : scale,
      margin,
      color: {
        dark: hex2rgba(options.color.dark || "#000000ff"),
        light: hex2rgba(options.color.light || "#ffffffff")
      },
      type: options.type,
      rendererOpts: options.rendererOpts || {}
    };
  };
  exports.getScale = function getScale(qrSize, opts) {
    return opts.width && opts.width >= qrSize + opts.margin * 2 ? opts.width / (qrSize + opts.margin * 2) : opts.scale;
  };
  exports.getImageWidth = function getImageWidth(qrSize, opts) {
    var scale = exports.getScale(qrSize, opts);
    return Math.floor((qrSize + opts.margin * 2) * scale);
  };
  exports.qrToImageData = function qrToImageData(imgData, qr, opts) {
    var size = qr.modules.size;
    var data = qr.modules.data;
    var scale = exports.getScale(size, opts);
    var symbolSize = Math.floor((size + opts.margin * 2) * scale);
    var scaledMargin = opts.margin * scale;
    var palette = [opts.color.light, opts.color.dark];
    for (var i2 = 0; i2 < symbolSize; i2++) {
      for (var j2 = 0; j2 < symbolSize; j2++) {
        var posDst = (i2 * symbolSize + j2) * 4;
        var pxColor = opts.color.light;
        if (i2 >= scaledMargin && j2 >= scaledMargin && i2 < symbolSize - scaledMargin && j2 < symbolSize - scaledMargin) {
          var iSrc = Math.floor((i2 - scaledMargin) / scale);
          var jSrc = Math.floor((j2 - scaledMargin) / scale);
          pxColor = palette[data[iSrc * size + jSrc] ? 1 : 0];
        }
        imgData[posDst++] = pxColor.r;
        imgData[posDst++] = pxColor.g;
        imgData[posDst++] = pxColor.b;
        imgData[posDst] = pxColor.a;
      }
    }
  };
})(utils);
(function(exports) {
  var Utils2 = utils;
  function clearCanvas(ctx, canvas2, size) {
    ctx.clearRect(0, 0, canvas2.width, canvas2.height);
    if (!canvas2.style)
      canvas2.style = {};
    canvas2.height = size;
    canvas2.width = size;
    canvas2.style.height = size + "px";
    canvas2.style.width = size + "px";
  }
  function getCanvasElement() {
    try {
      return document.createElement("canvas");
    } catch (e2) {
      throw new Error("You need to specify a canvas element");
    }
  }
  exports.render = function render2(qrData, canvas2, options) {
    var opts = options;
    var canvasEl = canvas2;
    if (typeof opts === "undefined" && (!canvas2 || !canvas2.getContext)) {
      opts = canvas2;
      canvas2 = void 0;
    }
    if (!canvas2) {
      canvasEl = getCanvasElement();
    }
    opts = Utils2.getOptions(opts);
    var size = Utils2.getImageWidth(qrData.modules.size, opts);
    var ctx = canvasEl.getContext("2d");
    var image = ctx.createImageData(size, size);
    Utils2.qrToImageData(image.data, qrData, opts);
    clearCanvas(ctx, canvasEl, size);
    ctx.putImageData(image, 0, 0);
    return canvasEl;
  };
  exports.renderToDataURL = function renderToDataURL(qrData, canvas2, options) {
    var opts = options;
    if (typeof opts === "undefined" && (!canvas2 || !canvas2.getContext)) {
      opts = canvas2;
      canvas2 = void 0;
    }
    if (!opts)
      opts = {};
    var canvasEl = exports.render(qrData, canvas2, opts);
    var type = opts.type || "image/png";
    var rendererOpts = opts.rendererOpts || {};
    return canvasEl.toDataURL(type, rendererOpts.quality);
  };
})(canvas);
var svgTag = {};
var Utils = utils;
function getColorAttrib(color, attrib) {
  var alpha = color.a / 255;
  var str = attrib + '="' + color.hex + '"';
  return alpha < 1 ? str + " " + attrib + '-opacity="' + alpha.toFixed(2).slice(1) + '"' : str;
}
function svgCmd(cmd, x2, y2) {
  var str = cmd + x2;
  if (typeof y2 !== "undefined")
    str += " " + y2;
  return str;
}
function qrToPath(data, size, margin) {
  var path = "";
  var moveBy = 0;
  var newRow = false;
  var lineLength = 0;
  for (var i2 = 0; i2 < data.length; i2++) {
    var col = Math.floor(i2 % size);
    var row = Math.floor(i2 / size);
    if (!col && !newRow)
      newRow = true;
    if (data[i2]) {
      lineLength++;
      if (!(i2 > 0 && col > 0 && data[i2 - 1])) {
        path += newRow ? svgCmd("M", col + margin, 0.5 + row + margin) : svgCmd("m", moveBy, 0);
        moveBy = 0;
        newRow = false;
      }
      if (!(col + 1 < size && data[i2 + 1])) {
        path += svgCmd("h", lineLength);
        lineLength = 0;
      }
    } else {
      moveBy++;
    }
  }
  return path;
}
svgTag.render = function render(qrData, options, cb) {
  var opts = Utils.getOptions(options);
  var size = qrData.modules.size;
  var data = qrData.modules.data;
  var qrcodesize = size + opts.margin * 2;
  var bg = !opts.color.light.a ? "" : "<path " + getColorAttrib(opts.color.light, "fill") + ' d="M0 0h' + qrcodesize + "v" + qrcodesize + 'H0z"/>';
  var path = "<path " + getColorAttrib(opts.color.dark, "stroke") + ' d="' + qrToPath(data, size, opts.margin) + '"/>';
  var viewBox = 'viewBox="0 0 ' + qrcodesize + " " + qrcodesize + '"';
  var width = !opts.width ? "" : 'width="' + opts.width + '" height="' + opts.width + '" ';
  var svgTag2 = '<svg xmlns="http://www.w3.org/2000/svg" ' + width + viewBox + ' shape-rendering="crispEdges">' + bg + path + "</svg>\n";
  if (typeof cb === "function") {
    cb(null, svgTag2);
  }
  return svgTag2;
};
var canPromise = canPromise$1;
var QRCode$1 = qrcode;
var CanvasRenderer = canvas;
var SvgRenderer = svgTag;
function renderCanvas(renderFunc, canvas2, text, opts, cb) {
  var args = [].slice.call(arguments, 1);
  var argsNum = args.length;
  var isLastArgCb = typeof args[argsNum - 1] === "function";
  if (!isLastArgCb && !canPromise()) {
    throw new Error("Callback required as last argument");
  }
  if (isLastArgCb) {
    if (argsNum < 2) {
      throw new Error("Too few arguments provided");
    }
    if (argsNum === 2) {
      cb = text;
      text = canvas2;
      canvas2 = opts = void 0;
    } else if (argsNum === 3) {
      if (canvas2.getContext && typeof cb === "undefined") {
        cb = opts;
        opts = void 0;
      } else {
        cb = opts;
        opts = text;
        text = canvas2;
        canvas2 = void 0;
      }
    }
  } else {
    if (argsNum < 1) {
      throw new Error("Too few arguments provided");
    }
    if (argsNum === 1) {
      text = canvas2;
      canvas2 = opts = void 0;
    } else if (argsNum === 2 && !canvas2.getContext) {
      opts = text;
      text = canvas2;
      canvas2 = void 0;
    }
    return new Promise(function(resolve, reject) {
      try {
        var data2 = QRCode$1.create(text, opts);
        resolve(renderFunc(data2, canvas2, opts));
      } catch (e2) {
        reject(e2);
      }
    });
  }
  try {
    var data = QRCode$1.create(text, opts);
    cb(null, renderFunc(data, canvas2, opts));
  } catch (e2) {
    cb(e2);
  }
}
browser.create = QRCode$1.create;
browser.toCanvas = renderCanvas.bind(null, CanvasRenderer.render);
browser.toDataURL = renderCanvas.bind(null, CanvasRenderer.renderToDataURL);
browser.toString = renderCanvas.bind(null, function(data, _2, opts) {
  return SvgRenderer.render(data, opts);
});
var toggleSelection = function() {
  var selection = document.getSelection();
  if (!selection.rangeCount) {
    return function() {
    };
  }
  var active = document.activeElement;
  var ranges = [];
  for (var i2 = 0; i2 < selection.rangeCount; i2++) {
    ranges.push(selection.getRangeAt(i2));
  }
  switch (active.tagName.toUpperCase()) {
    case "INPUT":
    case "TEXTAREA":
      active.blur();
      break;
    default:
      active = null;
      break;
  }
  selection.removeAllRanges();
  return function() {
    selection.type === "Caret" && selection.removeAllRanges();
    if (!selection.rangeCount) {
      ranges.forEach(function(range) {
        selection.addRange(range);
      });
    }
    active && active.focus();
  };
};
var deselectCurrent = toggleSelection;
var clipboardToIE11Formatting = {
  "text/plain": "Text",
  "text/html": "Url",
  "default": "Text"
};
var defaultMessage = "Copy to clipboard: #{key}, Enter";
function format(message) {
  var copyKey = (/mac os x/i.test(navigator.userAgent) ? "⌘" : "Ctrl") + "+C";
  return message.replace(/#{\s*key\s*}/g, copyKey);
}
function copy$1(text, options) {
  var debug, message, reselectPrevious, range, selection, mark, success = false;
  if (!options) {
    options = {};
  }
  debug = options.debug || false;
  try {
    reselectPrevious = deselectCurrent();
    range = document.createRange();
    selection = document.getSelection();
    mark = document.createElement("span");
    mark.textContent = text;
    mark.ariaHidden = "true";
    mark.style.all = "unset";
    mark.style.position = "fixed";
    mark.style.top = 0;
    mark.style.clip = "rect(0, 0, 0, 0)";
    mark.style.whiteSpace = "pre";
    mark.style.webkitUserSelect = "text";
    mark.style.MozUserSelect = "text";
    mark.style.msUserSelect = "text";
    mark.style.userSelect = "text";
    mark.addEventListener("copy", function(e2) {
      e2.stopPropagation();
      if (options.format) {
        e2.preventDefault();
        if (typeof e2.clipboardData === "undefined") {
          debug && console.warn("unable to use e.clipboardData");
          debug && console.warn("trying IE specific stuff");
          window.clipboardData.clearData();
          var format2 = clipboardToIE11Formatting[options.format] || clipboardToIE11Formatting["default"];
          window.clipboardData.setData(format2, text);
        } else {
          e2.clipboardData.clearData();
          e2.clipboardData.setData(options.format, text);
        }
      }
      if (options.onCopy) {
        e2.preventDefault();
        options.onCopy(e2.clipboardData);
      }
    });
    document.body.appendChild(mark);
    range.selectNodeContents(mark);
    selection.addRange(range);
    var successful = document.execCommand("copy");
    if (!successful) {
      throw new Error("copy command was unsuccessful");
    }
    success = true;
  } catch (err) {
    debug && console.error("unable to copy using execCommand: ", err);
    debug && console.warn("trying IE specific stuff");
    try {
      window.clipboardData.setData(options.format || "text", text);
      options.onCopy && options.onCopy(window.clipboardData);
      success = true;
    } catch (err2) {
      debug && console.error("unable to copy using clipboardData: ", err2);
      debug && console.error("falling back to prompt");
      message = format("message" in options ? options.message : defaultMessage);
      window.prompt(message, text);
    }
  } finally {
    if (selection) {
      if (typeof selection.removeRange == "function") {
        selection.removeRange(range);
      } else {
        selection.removeAllRanges();
      }
    }
    if (mark) {
      document.body.removeChild(mark);
    }
    reselectPrevious();
  }
  return success;
}
var copyToClipboard = copy$1;
var n, u$1, i$1, t$1, r$1, o$1, f$1, e$1 = {}, c$1 = [], s$1 = /acit|ex(?:s|g|n|p|$)|rph|grid|ows|mnc|ntw|ine[ch]|zoo|^ord/i;
function a$1(n2, l2) {
  for (var u2 in l2)
    n2[u2] = l2[u2];
  return n2;
}
function v$1(n2) {
  var l2 = n2.parentNode;
  l2 && l2.removeChild(n2);
}
function h$1(n2, l2, u2) {
  var i2, t2 = arguments, r2 = {};
  for (i2 in l2)
    "key" !== i2 && "ref" !== i2 && (r2[i2] = l2[i2]);
  if (arguments.length > 3)
    for (u2 = [u2], i2 = 3; i2 < arguments.length; i2++)
      u2.push(t2[i2]);
  if (null != u2 && (r2.children = u2), "function" == typeof n2 && null != n2.defaultProps)
    for (i2 in n2.defaultProps)
      void 0 === r2[i2] && (r2[i2] = n2.defaultProps[i2]);
  return p$1(n2, r2, l2 && l2.key, l2 && l2.ref, null);
}
function p$1(l2, u2, i2, t2, r2) {
  var o2 = { type: l2, props: u2, key: i2, ref: t2, __k: null, __: null, __b: 0, __e: null, __d: void 0, __c: null, constructor: void 0, __v: r2 };
  return null == r2 && (o2.__v = o2), n.vnode && n.vnode(o2), o2;
}
function y$1() {
  return {};
}
function d$1(n2) {
  return n2.children;
}
function m$1(n2, l2) {
  this.props = n2, this.context = l2;
}
function w$2(n2, l2) {
  if (null == l2)
    return n2.__ ? w$2(n2.__, n2.__.__k.indexOf(n2) + 1) : null;
  for (var u2; l2 < n2.__k.length; l2++)
    if (null != (u2 = n2.__k[l2]) && null != u2.__e)
      return u2.__e;
  return "function" == typeof n2.type ? w$2(n2) : null;
}
function k$1(n2) {
  var l2, u2;
  if (null != (n2 = n2.__) && null != n2.__c) {
    for (n2.__e = n2.__c.base = null, l2 = 0; l2 < n2.__k.length; l2++)
      if (null != (u2 = n2.__k[l2]) && null != u2.__e) {
        n2.__e = n2.__c.base = u2.__e;
        break;
      }
    return k$1(n2);
  }
}
function g$1(l2) {
  (!l2.__d && (l2.__d = true) && u$1.push(l2) && !i$1++ || r$1 !== n.debounceRendering) && ((r$1 = n.debounceRendering) || t$1)(_$2);
}
function _$2() {
  for (var n2; i$1 = u$1.length; )
    n2 = u$1.sort(function(n3, l2) {
      return n3.__v.__b - l2.__v.__b;
    }), u$1 = [], n2.some(function(n3) {
      var l2, u2, i2, t2, r2, o2, f2;
      n3.__d && (o2 = (r2 = (l2 = n3).__v).__e, (f2 = l2.__P) && (u2 = [], (i2 = a$1({}, r2)).__v = i2, t2 = A$2(f2, r2, i2, l2.__n, void 0 !== f2.ownerSVGElement, null, u2, null == o2 ? w$2(r2) : o2), T$2(u2, r2), t2 != o2 && k$1(r2)));
    });
}
function b(n2, l2, u2, i2, t2, r2, o2, f2, s2) {
  var a2, h2, p2, y2, d2, m2, k2, g2 = u2 && u2.__k || c$1, _2 = g2.length;
  if (f2 == e$1 && (f2 = null != r2 ? r2[0] : _2 ? w$2(u2, 0) : null), a2 = 0, l2.__k = x$1(l2.__k, function(u3) {
    if (null != u3) {
      if (u3.__ = l2, u3.__b = l2.__b + 1, null === (p2 = g2[a2]) || p2 && u3.key == p2.key && u3.type === p2.type)
        g2[a2] = void 0;
      else
        for (h2 = 0; h2 < _2; h2++) {
          if ((p2 = g2[h2]) && u3.key == p2.key && u3.type === p2.type) {
            g2[h2] = void 0;
            break;
          }
          p2 = null;
        }
      if (y2 = A$2(n2, u3, p2 = p2 || e$1, i2, t2, r2, o2, f2, s2), (h2 = u3.ref) && p2.ref != h2 && (k2 || (k2 = []), p2.ref && k2.push(p2.ref, null, u3), k2.push(h2, u3.__c || y2, u3)), null != y2) {
        var c2;
        if (null == m2 && (m2 = y2), void 0 !== u3.__d)
          c2 = u3.__d, u3.__d = void 0;
        else if (r2 == p2 || y2 != f2 || null == y2.parentNode) {
          n:
            if (null == f2 || f2.parentNode !== n2)
              n2.appendChild(y2), c2 = null;
            else {
              for (d2 = f2, h2 = 0; (d2 = d2.nextSibling) && h2 < _2; h2 += 2)
                if (d2 == y2)
                  break n;
              n2.insertBefore(y2, f2), c2 = f2;
            }
          "option" == l2.type && (n2.value = "");
        }
        f2 = void 0 !== c2 ? c2 : y2.nextSibling, "function" == typeof l2.type && (l2.__d = f2);
      } else
        f2 && p2.__e == f2 && f2.parentNode != n2 && (f2 = w$2(p2));
    }
    return a2++, u3;
  }), l2.__e = m2, null != r2 && "function" != typeof l2.type)
    for (a2 = r2.length; a2--; )
      null != r2[a2] && v$1(r2[a2]);
  for (a2 = _2; a2--; )
    null != g2[a2] && D$1(g2[a2], g2[a2]);
  if (k2)
    for (a2 = 0; a2 < k2.length; a2++)
      j$1(k2[a2], k2[++a2], k2[++a2]);
}
function x$1(n2, l2, u2) {
  if (null == u2 && (u2 = []), null == n2 || "boolean" == typeof n2)
    l2 && u2.push(l2(null));
  else if (Array.isArray(n2))
    for (var i2 = 0; i2 < n2.length; i2++)
      x$1(n2[i2], l2, u2);
  else
    u2.push(l2 ? l2("string" == typeof n2 || "number" == typeof n2 ? p$1(null, n2, null, null, n2) : null != n2.__e || null != n2.__c ? p$1(n2.type, n2.props, n2.key, null, n2.__v) : n2) : n2);
  return u2;
}
function P$1(n2, l2, u2, i2, t2) {
  var r2;
  for (r2 in u2)
    "children" === r2 || "key" === r2 || r2 in l2 || N$1(n2, r2, null, u2[r2], i2);
  for (r2 in l2)
    t2 && "function" != typeof l2[r2] || "children" === r2 || "key" === r2 || "value" === r2 || "checked" === r2 || u2[r2] === l2[r2] || N$1(n2, r2, l2[r2], u2[r2], i2);
}
function C$1(n2, l2, u2) {
  "-" === l2[0] ? n2.setProperty(l2, u2) : n2[l2] = "number" == typeof u2 && false === s$1.test(l2) ? u2 + "px" : null == u2 ? "" : u2;
}
function N$1(n2, l2, u2, i2, t2) {
  var r2, o2, f2, e2, c2;
  if (t2 ? "className" === l2 && (l2 = "class") : "class" === l2 && (l2 = "className"), "style" === l2)
    if (r2 = n2.style, "string" == typeof u2)
      r2.cssText = u2;
    else {
      if ("string" == typeof i2 && (r2.cssText = "", i2 = null), i2)
        for (e2 in i2)
          u2 && e2 in u2 || C$1(r2, e2, "");
      if (u2)
        for (c2 in u2)
          i2 && u2[c2] === i2[c2] || C$1(r2, c2, u2[c2]);
    }
  else
    "o" === l2[0] && "n" === l2[1] ? (o2 = l2 !== (l2 = l2.replace(/Capture$/, "")), f2 = l2.toLowerCase(), l2 = (f2 in n2 ? f2 : l2).slice(2), u2 ? (i2 || n2.addEventListener(l2, z$1, o2), (n2.l || (n2.l = {}))[l2] = u2) : n2.removeEventListener(l2, z$1, o2)) : "list" !== l2 && "tagName" !== l2 && "form" !== l2 && "type" !== l2 && "size" !== l2 && !t2 && l2 in n2 ? n2[l2] = null == u2 ? "" : u2 : "function" != typeof u2 && "dangerouslySetInnerHTML" !== l2 && (l2 !== (l2 = l2.replace(/^xlink:?/, "")) ? null == u2 || false === u2 ? n2.removeAttributeNS("http://www.w3.org/1999/xlink", l2.toLowerCase()) : n2.setAttributeNS("http://www.w3.org/1999/xlink", l2.toLowerCase(), u2) : null == u2 || false === u2 && !/^ar/.test(l2) ? n2.removeAttribute(l2) : n2.setAttribute(l2, u2));
}
function z$1(l2) {
  this.l[l2.type](n.event ? n.event(l2) : l2);
}
function A$2(l2, u2, i2, t2, r2, o2, f2, e2, c2) {
  var s2, v2, h2, p2, y2, w2, k2, g2, _2, x2, P2 = u2.type;
  if (void 0 !== u2.constructor)
    return null;
  (s2 = n.__b) && s2(u2);
  try {
    n:
      if ("function" == typeof P2) {
        if (g2 = u2.props, _2 = (s2 = P2.contextType) && t2[s2.__c], x2 = s2 ? _2 ? _2.props.value : s2.__ : t2, i2.__c ? k2 = (v2 = u2.__c = i2.__c).__ = v2.__E : ("prototype" in P2 && P2.prototype.render ? u2.__c = v2 = new P2(g2, x2) : (u2.__c = v2 = new m$1(g2, x2), v2.constructor = P2, v2.render = E$2), _2 && _2.sub(v2), v2.props = g2, v2.state || (v2.state = {}), v2.context = x2, v2.__n = t2, h2 = v2.__d = true, v2.__h = []), null == v2.__s && (v2.__s = v2.state), null != P2.getDerivedStateFromProps && (v2.__s == v2.state && (v2.__s = a$1({}, v2.__s)), a$1(v2.__s, P2.getDerivedStateFromProps(g2, v2.__s))), p2 = v2.props, y2 = v2.state, h2)
          null == P2.getDerivedStateFromProps && null != v2.componentWillMount && v2.componentWillMount(), null != v2.componentDidMount && v2.__h.push(v2.componentDidMount);
        else {
          if (null == P2.getDerivedStateFromProps && g2 !== p2 && null != v2.componentWillReceiveProps && v2.componentWillReceiveProps(g2, x2), !v2.__e && null != v2.shouldComponentUpdate && false === v2.shouldComponentUpdate(g2, v2.__s, x2) || u2.__v === i2.__v && !v2.__) {
            for (v2.props = g2, v2.state = v2.__s, u2.__v !== i2.__v && (v2.__d = false), v2.__v = u2, u2.__e = i2.__e, u2.__k = i2.__k, v2.__h.length && f2.push(v2), s2 = 0; s2 < u2.__k.length; s2++)
              u2.__k[s2] && (u2.__k[s2].__ = u2);
            break n;
          }
          null != v2.componentWillUpdate && v2.componentWillUpdate(g2, v2.__s, x2), null != v2.componentDidUpdate && v2.__h.push(function() {
            v2.componentDidUpdate(p2, y2, w2);
          });
        }
        v2.context = x2, v2.props = g2, v2.state = v2.__s, (s2 = n.__r) && s2(u2), v2.__d = false, v2.__v = u2, v2.__P = l2, s2 = v2.render(v2.props, v2.state, v2.context), u2.__k = null != s2 && s2.type == d$1 && null == s2.key ? s2.props.children : Array.isArray(s2) ? s2 : [s2], null != v2.getChildContext && (t2 = a$1(a$1({}, t2), v2.getChildContext())), h2 || null == v2.getSnapshotBeforeUpdate || (w2 = v2.getSnapshotBeforeUpdate(p2, y2)), b(l2, u2, i2, t2, r2, o2, f2, e2, c2), v2.base = u2.__e, v2.__h.length && f2.push(v2), k2 && (v2.__E = v2.__ = null), v2.__e = false;
      } else
        null == o2 && u2.__v === i2.__v ? (u2.__k = i2.__k, u2.__e = i2.__e) : u2.__e = $$1(i2.__e, u2, i2, t2, r2, o2, f2, c2);
    (s2 = n.diffed) && s2(u2);
  } catch (l3) {
    u2.__v = null, n.__e(l3, u2, i2);
  }
  return u2.__e;
}
function T$2(l2, u2) {
  n.__c && n.__c(u2, l2), l2.some(function(u3) {
    try {
      l2 = u3.__h, u3.__h = [], l2.some(function(n2) {
        n2.call(u3);
      });
    } catch (l3) {
      n.__e(l3, u3.__v);
    }
  });
}
function $$1(n2, l2, u2, i2, t2, r2, o2, f2) {
  var s2, a2, v2, h2, p2, y2 = u2.props, d2 = l2.props;
  if (t2 = "svg" === l2.type || t2, null != r2) {
    for (s2 = 0; s2 < r2.length; s2++)
      if (null != (a2 = r2[s2]) && ((null === l2.type ? 3 === a2.nodeType : a2.localName === l2.type) || n2 == a2)) {
        n2 = a2, r2[s2] = null;
        break;
      }
  }
  if (null == n2) {
    if (null === l2.type)
      return document.createTextNode(d2);
    n2 = t2 ? document.createElementNS("http://www.w3.org/2000/svg", l2.type) : document.createElement(l2.type, d2.is && { is: d2.is }), r2 = null, f2 = false;
  }
  if (null === l2.type)
    y2 !== d2 && n2.data != d2 && (n2.data = d2);
  else {
    if (null != r2 && (r2 = c$1.slice.call(n2.childNodes)), v2 = (y2 = u2.props || e$1).dangerouslySetInnerHTML, h2 = d2.dangerouslySetInnerHTML, !f2) {
      if (y2 === e$1)
        for (y2 = {}, p2 = 0; p2 < n2.attributes.length; p2++)
          y2[n2.attributes[p2].name] = n2.attributes[p2].value;
      (h2 || v2) && (h2 && v2 && h2.__html == v2.__html || (n2.innerHTML = h2 && h2.__html || ""));
    }
    P$1(n2, d2, y2, t2, f2), h2 ? l2.__k = [] : (l2.__k = l2.props.children, b(n2, l2, u2, i2, "foreignObject" !== l2.type && t2, r2, o2, e$1, f2)), f2 || ("value" in d2 && void 0 !== (s2 = d2.value) && s2 !== n2.value && N$1(n2, "value", s2, y2.value, false), "checked" in d2 && void 0 !== (s2 = d2.checked) && s2 !== n2.checked && N$1(n2, "checked", s2, y2.checked, false));
  }
  return n2;
}
function j$1(l2, u2, i2) {
  try {
    "function" == typeof l2 ? l2(u2) : l2.current = u2;
  } catch (l3) {
    n.__e(l3, i2);
  }
}
function D$1(l2, u2, i2) {
  var t2, r2, o2;
  if (n.unmount && n.unmount(l2), (t2 = l2.ref) && (t2.current && t2.current !== l2.__e || j$1(t2, null, u2)), i2 || "function" == typeof l2.type || (i2 = null != (r2 = l2.__e)), l2.__e = l2.__d = void 0, null != (t2 = l2.__c)) {
    if (t2.componentWillUnmount)
      try {
        t2.componentWillUnmount();
      } catch (l3) {
        n.__e(l3, u2);
      }
    t2.base = t2.__P = null;
  }
  if (t2 = l2.__k)
    for (o2 = 0; o2 < t2.length; o2++)
      t2[o2] && D$1(t2[o2], u2, i2);
  null != r2 && v$1(r2);
}
function E$2(n2, l2, u2) {
  return this.constructor(n2, u2);
}
function H$1(l2, u2, i2) {
  var t2, r2, f2;
  n.__ && n.__(l2, u2), r2 = (t2 = i2 === o$1) ? null : i2 && i2.__k || u2.__k, l2 = h$1(d$1, null, [l2]), f2 = [], A$2(u2, (t2 ? u2 : i2 || u2).__k = l2, r2 || e$1, e$1, void 0 !== u2.ownerSVGElement, i2 && !t2 ? [i2] : r2 ? null : c$1.slice.call(u2.childNodes), f2, i2 || e$1, t2), T$2(f2, l2);
}
function I$1(n2, l2) {
  H$1(n2, l2, o$1);
}
function L$1(n2, l2) {
  var u2, i2;
  for (i2 in l2 = a$1(a$1({}, n2.props), l2), arguments.length > 2 && (l2.children = c$1.slice.call(arguments, 2)), u2 = {}, l2)
    "key" !== i2 && "ref" !== i2 && (u2[i2] = l2[i2]);
  return p$1(n2.type, u2, l2.key || n2.key, l2.ref || n2.ref, null);
}
function M$1(n2) {
  var l2 = {}, u2 = { __c: "__cC" + f$1++, __: n2, Consumer: function(n3, l3) {
    return n3.children(l3);
  }, Provider: function(n3) {
    var i2, t2 = this;
    return this.getChildContext || (i2 = [], this.getChildContext = function() {
      return l2[u2.__c] = t2, l2;
    }, this.shouldComponentUpdate = function(n4) {
      t2.props.value !== n4.value && i2.some(function(l3) {
        l3.context = n4.value, g$1(l3);
      });
    }, this.sub = function(n4) {
      i2.push(n4);
      var l3 = n4.componentWillUnmount;
      n4.componentWillUnmount = function() {
        i2.splice(i2.indexOf(n4), 1), l3 && l3.call(n4);
      };
    }), n3.children;
  } };
  return u2.Consumer.contextType = u2, u2.Provider.__ = u2, u2;
}
n = { __e: function(n2, l2) {
  for (var u2, i2; l2 = l2.__; )
    if ((u2 = l2.__c) && !u2.__)
      try {
        if (u2.constructor && null != u2.constructor.getDerivedStateFromError && (i2 = true, u2.setState(u2.constructor.getDerivedStateFromError(n2))), null != u2.componentDidCatch && (i2 = true, u2.componentDidCatch(n2)), i2)
          return g$1(u2.__E = u2);
      } catch (l3) {
        n2 = l3;
      }
  throw n2;
} }, m$1.prototype.setState = function(n2, l2) {
  var u2;
  u2 = this.__s !== this.state ? this.__s : this.__s = a$1({}, this.state), "function" == typeof n2 && (n2 = n2(u2, this.props)), n2 && a$1(u2, n2), null != n2 && this.__v && (l2 && this.__h.push(l2), g$1(this));
}, m$1.prototype.forceUpdate = function(n2) {
  this.__v && (this.__e = true, n2 && this.__h.push(n2), g$1(this));
}, m$1.prototype.render = d$1, u$1 = [], i$1 = 0, t$1 = "function" == typeof Promise ? Promise.prototype.then.bind(Promise.resolve()) : setTimeout, o$1 = e$1, f$1 = 0;
var t, u, r, i = 0, o = [], c = n.__r, f = n.diffed, e = n.__c, a = n.unmount;
function v(t2, r2) {
  n.__h && n.__h(u, t2, i || r2), i = 0;
  var o2 = u.__H || (u.__H = { __: [], __h: [] });
  return t2 >= o2.__.length && o2.__.push({}), o2.__[t2];
}
function m(n2) {
  return i = 1, p(E$1, n2);
}
function p(n2, r2, i2) {
  var o2 = v(t++, 2);
  return o2.__c || (o2.__c = u, o2.__ = [i2 ? i2(r2) : E$1(void 0, r2), function(t2) {
    var u2 = n2(o2.__[0], t2);
    o2.__[0] !== u2 && (o2.__[0] = u2, o2.__c.setState({}));
  }]), o2.__;
}
function l(r2, i2) {
  var o2 = v(t++, 3);
  !n.__s && x(o2.__H, i2) && (o2.__ = r2, o2.__H = i2, u.__H.__h.push(o2));
}
function y(r2, i2) {
  var o2 = v(t++, 4);
  !n.__s && x(o2.__H, i2) && (o2.__ = r2, o2.__H = i2, u.__h.push(o2));
}
function d(n2) {
  return i = 5, h(function() {
    return { current: n2 };
  }, []);
}
function s(n2, t2, u2) {
  i = 6, y(function() {
    "function" == typeof n2 ? n2(t2()) : n2 && (n2.current = t2());
  }, null == u2 ? u2 : u2.concat(n2));
}
function h(n2, u2) {
  var r2 = v(t++, 7);
  return x(r2.__H, u2) ? (r2.__H = u2, r2.__h = n2, r2.__ = n2()) : r2.__;
}
function T$1(n2, t2) {
  return i = 8, h(function() {
    return n2;
  }, t2);
}
function w$1(n2) {
  var r2 = u.context[n2.__c], i2 = v(t++, 9);
  return i2.__c = n2, r2 ? (null == i2.__ && (i2.__ = true, r2.sub(u)), r2.props.value) : n2.__;
}
function A$1(t2, u2) {
  n.useDebugValue && n.useDebugValue(u2 ? u2(t2) : t2);
}
function F$1(n2) {
  var r2 = v(t++, 10), i2 = m();
  return r2.__ = n2, u.componentDidCatch || (u.componentDidCatch = function(n3) {
    r2.__ && r2.__(n3), i2[1](n3);
  }), [i2[0], function() {
    i2[1](void 0);
  }];
}
function _$1() {
  o.some(function(t2) {
    if (t2.__P)
      try {
        t2.__H.__h.forEach(g), t2.__H.__h.forEach(q$1), t2.__H.__h = [];
      } catch (u2) {
        return t2.__H.__h = [], n.__e(u2, t2.__v), true;
      }
  }), o = [];
}
function g(n2) {
  n2.t && n2.t();
}
function q$1(n2) {
  var t2 = n2.__();
  "function" == typeof t2 && (n2.t = t2);
}
function x(n2, t2) {
  return !n2 || t2.some(function(t3, u2) {
    return t3 !== n2[u2];
  });
}
function E$1(n2, t2) {
  return "function" == typeof t2 ? t2(n2) : t2;
}
n.__r = function(n2) {
  c && c(n2), t = 0, (u = n2.__c).__H && (u.__H.__h.forEach(g), u.__H.__h.forEach(q$1), u.__H.__h = []);
}, n.diffed = function(t2) {
  f && f(t2);
  var u2 = t2.__c;
  if (u2) {
    var i2 = u2.__H;
    i2 && i2.__h.length && (1 !== o.push(u2) && r === n.requestAnimationFrame || ((r = n.requestAnimationFrame) || function(n2) {
      var t3, u3 = function() {
        clearTimeout(r2), cancelAnimationFrame(t3), setTimeout(n2);
      }, r2 = setTimeout(u3, 100);
      "undefined" != typeof window && (t3 = requestAnimationFrame(u3));
    })(_$1));
  }
}, n.__c = function(t2, u2) {
  u2.some(function(t3) {
    try {
      t3.__h.forEach(g), t3.__h = t3.__h.filter(function(n2) {
        return !n2.__ || q$1(n2);
      });
    } catch (r2) {
      u2.some(function(n2) {
        n2.__h && (n2.__h = []);
      }), u2 = [], n.__e(r2, t3.__v);
    }
  }), e && e(t2, u2);
}, n.unmount = function(t2) {
  a && a(t2);
  var u2 = t2.__c;
  if (u2) {
    var r2 = u2.__H;
    if (r2)
      try {
        r2.__.forEach(function(n2) {
          return n2.t && n2.t();
        });
      } catch (t3) {
        n.__e(t3, u2.__v);
      }
  }
};
function E(n2, t2) {
  for (var e2 in t2)
    n2[e2] = t2[e2];
  return n2;
}
function w(n2, t2) {
  for (var e2 in n2)
    if ("__source" !== e2 && !(e2 in t2))
      return true;
  for (var r2 in t2)
    if ("__source" !== r2 && n2[r2] !== t2[r2])
      return true;
  return false;
}
var C = function(n2) {
  var t2, e2;
  function r2(t3) {
    var e3;
    return (e3 = n2.call(this, t3) || this).isPureReactComponent = true, e3;
  }
  return e2 = n2, (t2 = r2).prototype = Object.create(e2.prototype), t2.prototype.constructor = t2, t2.__proto__ = e2, r2.prototype.shouldComponentUpdate = function(n3, t3) {
    return w(this.props, n3) || w(this.state, t3);
  }, r2;
}(m$1);
function _(n2, t2) {
  function e2(n3) {
    var e3 = this.props.ref, r3 = e3 == n3.ref;
    return !r3 && e3 && (e3.call ? e3(null) : e3.current = null), t2 ? !t2(this.props, n3) || !r3 : w(this.props, n3);
  }
  function r2(t3) {
    return this.shouldComponentUpdate = e2, h$1(n2, E({}, t3));
  }
  return r2.prototype.isReactComponent = true, r2.displayName = "Memo(" + (n2.displayName || n2.name) + ")", r2.t = true, r2;
}
var A = n.__b;
function S(n2) {
  function t2(t3) {
    var e2 = E({}, t3);
    return delete e2.ref, n2(e2, t3.ref);
  }
  return t2.prototype.isReactComponent = t2.t = true, t2.displayName = "ForwardRef(" + (n2.displayName || n2.name) + ")", t2;
}
n.__b = function(n2) {
  n2.type && n2.type.t && n2.ref && (n2.props.ref = n2.ref, n2.ref = null), A && A(n2);
};
var k = function(n2, t2) {
  return n2 ? x$1(n2).reduce(function(n3, e2, r2) {
    return n3.concat(t2(e2, r2));
  }, []) : null;
}, R = { map: k, forEach: k, count: function(n2) {
  return n2 ? x$1(n2).length : 0;
}, only: function(n2) {
  if (1 !== (n2 = x$1(n2)).length)
    throw new Error("Children.only() expects only one child.");
  return n2[0];
}, toArray: x$1 }, F = n.__e;
function N(n2) {
  return n2 && ((n2 = E({}, n2)).__c = null, n2.__k = n2.__k && n2.__k.map(N)), n2;
}
function U() {
  this.__u = 0, this.o = null, this.__b = null;
}
function M(n2) {
  var t2 = n2.__.__c;
  return t2 && t2.u && t2.u(n2);
}
function L(n2) {
  var t2, e2, r2;
  function o2(o3) {
    if (t2 || (t2 = n2()).then(function(n3) {
      e2 = n3.default || n3;
    }, function(n3) {
      r2 = n3;
    }), r2)
      throw r2;
    if (!e2)
      throw t2;
    return h$1(e2, o3);
  }
  return o2.displayName = "Lazy", o2.t = true, o2;
}
function O() {
  this.i = null, this.l = null;
}
n.__e = function(n2, t2, e2) {
  if (n2.then) {
    for (var r2, o2 = t2; o2 = o2.__; )
      if ((r2 = o2.__c) && r2.__c)
        return r2.__c(n2, t2.__c);
  }
  F(n2, t2, e2);
}, (U.prototype = new m$1()).__c = function(n2, t2) {
  var e2 = this;
  null == e2.o && (e2.o = []), e2.o.push(t2);
  var r2 = M(e2.__v), o2 = false, u2 = function() {
    o2 || (o2 = true, r2 ? r2(i2) : i2());
  };
  t2.__c = t2.componentWillUnmount, t2.componentWillUnmount = function() {
    u2(), t2.__c && t2.__c();
  };
  var i2 = function() {
    var n3;
    if (!--e2.__u)
      for (e2.__v.__k[0] = e2.state.u, e2.setState({ u: e2.__b = null }); n3 = e2.o.pop(); )
        n3.forceUpdate();
  };
  e2.__u++ || e2.setState({ u: e2.__b = e2.__v.__k[0] }), n2.then(u2, u2);
}, U.prototype.render = function(n2, t2) {
  return this.__b && (this.__v.__k[0] = N(this.__b), this.__b = null), [h$1(m$1, null, t2.u ? null : n2.children), t2.u && n2.fallback];
};
var P = function(n2, t2, e2) {
  if (++e2[1] === e2[0] && n2.l.delete(t2), n2.props.revealOrder && ("t" !== n2.props.revealOrder[0] || !n2.l.size))
    for (e2 = n2.i; e2; ) {
      for (; e2.length > 3; )
        e2.pop()();
      if (e2[1] < e2[0])
        break;
      n2.i = e2 = e2[2];
    }
};
(O.prototype = new m$1()).u = function(n2) {
  var t2 = this, e2 = M(t2.__v), r2 = t2.l.get(n2);
  return r2[0]++, function(o2) {
    var u2 = function() {
      t2.props.revealOrder ? (r2.push(o2), P(t2, n2, r2)) : o2();
    };
    e2 ? e2(u2) : u2();
  };
}, O.prototype.render = function(n2) {
  this.i = null, this.l = /* @__PURE__ */ new Map();
  var t2 = x$1(n2.children);
  n2.revealOrder && "b" === n2.revealOrder[0] && t2.reverse();
  for (var e2 = t2.length; e2--; )
    this.l.set(t2[e2], this.i = [1, 0, this.i]);
  return n2.children;
}, O.prototype.componentDidUpdate = O.prototype.componentDidMount = function() {
  var n2 = this;
  n2.l.forEach(function(t2, e2) {
    P(n2, e2, t2);
  });
};
var W = function() {
  function n2() {
  }
  var t2 = n2.prototype;
  return t2.getChildContext = function() {
    return this.props.context;
  }, t2.render = function(n3) {
    return n3.children;
  }, n2;
}();
function j(n2) {
  var t2 = this, e2 = n2.container, r2 = h$1(W, { context: t2.context }, n2.vnode);
  return t2.s && t2.s !== e2 && (t2.v.parentNode && t2.s.removeChild(t2.v), D$1(t2.h), t2.p = false), n2.vnode ? t2.p ? (e2.__k = t2.__k, H$1(r2, e2), t2.__k = e2.__k) : (t2.v = document.createTextNode(""), I$1("", e2), e2.appendChild(t2.v), t2.p = true, t2.s = e2, H$1(r2, e2, t2.v), t2.__k = t2.v.__k) : t2.p && (t2.v.parentNode && t2.s.removeChild(t2.v), D$1(t2.h)), t2.h = r2, t2.componentWillUnmount = function() {
    t2.v.parentNode && t2.s.removeChild(t2.v), D$1(t2.h);
  }, null;
}
function z(n2, t2) {
  return h$1(j, { vnode: n2, container: t2 });
}
var D = /^(?:accent|alignment|arabic|baseline|cap|clip(?!PathU)|color|fill|flood|font|glyph(?!R)|horiz|marker(?!H|W|U)|overline|paint|stop|strikethrough|stroke|text(?!L)|underline|unicode|units|v|vector|vert|word|writing|x(?!C))[A-Z]/;
m$1.prototype.isReactComponent = {};
var H = "undefined" != typeof Symbol && Symbol.for && Symbol.for("react.element") || 60103;
function T(n2, t2, e2) {
  if (null == t2.__k)
    for (; t2.firstChild; )
      t2.removeChild(t2.firstChild);
  return H$1(n2, t2), "function" == typeof e2 && e2(), n2 ? n2.__c : null;
}
function V(n2, t2, e2) {
  return I$1(n2, t2), "function" == typeof e2 && e2(), n2 ? n2.__c : null;
}
var Z = n.event;
function I(n2, t2) {
  n2["UNSAFE_" + t2] && !n2[t2] && Object.defineProperty(n2, t2, { configurable: false, get: function() {
    return this["UNSAFE_" + t2];
  }, set: function(n3) {
    this["UNSAFE_" + t2] = n3;
  } });
}
n.event = function(n2) {
  Z && (n2 = Z(n2)), n2.persist = function() {
  };
  var t2 = false, e2 = false, r2 = n2.stopPropagation;
  n2.stopPropagation = function() {
    r2.call(n2), t2 = true;
  };
  var o2 = n2.preventDefault;
  return n2.preventDefault = function() {
    o2.call(n2), e2 = true;
  }, n2.isPropagationStopped = function() {
    return t2;
  }, n2.isDefaultPrevented = function() {
    return e2;
  }, n2.nativeEvent = n2;
};
var $ = { configurable: true, get: function() {
  return this.class;
} }, q = n.vnode;
n.vnode = function(n2) {
  n2.$$typeof = H;
  var t2 = n2.type, e2 = n2.props;
  if (t2) {
    if (e2.class != e2.className && ($.enumerable = "className" in e2, null != e2.className && (e2.class = e2.className), Object.defineProperty(e2, "className", $)), "function" != typeof t2) {
      var r2, o2, u2;
      for (u2 in e2.defaultValue && void 0 !== e2.value && (e2.value || 0 === e2.value || (e2.value = e2.defaultValue), delete e2.defaultValue), Array.isArray(e2.value) && e2.multiple && "select" === t2 && (x$1(e2.children).forEach(function(n3) {
        -1 != e2.value.indexOf(n3.props.value) && (n3.props.selected = true);
      }), delete e2.value), e2)
        if (r2 = D.test(u2))
          break;
      if (r2)
        for (u2 in o2 = n2.props = {}, e2)
          o2[D.test(u2) ? u2.replace(/[A-Z0-9]/, "-$&").toLowerCase() : u2] = e2[u2];
    }
    !function(t3) {
      var e3 = n2.type, r3 = n2.props;
      if (r3 && "string" == typeof e3) {
        var o3 = {};
        for (var u3 in r3)
          /^on(Ani|Tra|Tou)/.test(u3) && (r3[u3.toLowerCase()] = r3[u3], delete r3[u3]), o3[u3.toLowerCase()] = u3;
        if (o3.ondoubleclick && (r3.ondblclick = r3[o3.ondoubleclick], delete r3[o3.ondoubleclick]), o3.onbeforeinput && (r3.onbeforeinput = r3[o3.onbeforeinput], delete r3[o3.onbeforeinput]), o3.onchange && ("textarea" === e3 || "input" === e3.toLowerCase() && !/^fil|che|ra/i.test(r3.type))) {
          var i2 = o3.oninput || "oninput";
          r3[i2] || (r3[i2] = r3[o3.onchange], delete r3[o3.onchange]);
        }
      }
    }(), "function" == typeof t2 && !t2.m && t2.prototype && (I(t2.prototype, "componentWillMount"), I(t2.prototype, "componentWillReceiveProps"), I(t2.prototype, "componentWillUpdate"), t2.m = true);
  }
  q && q(n2);
};
var B = "16.8.0";
function G(n2) {
  return h$1.bind(null, n2);
}
function J(n2) {
  return !!n2 && n2.$$typeof === H;
}
function K(n2) {
  return J(n2) ? L$1.apply(null, arguments) : n2;
}
function Q(n2) {
  return !!n2.__k && (H$1(null, n2), true);
}
function X(n2) {
  return n2 && (n2.base || 1 === n2.nodeType && n2) || null;
}
var Y = function(n2, t2) {
  return n2(t2);
};
const compat_module = { useState: m, useReducer: p, useEffect: l, useLayoutEffect: y, useRef: d, useImperativeHandle: s, useMemo: h, useCallback: T$1, useContext: w$1, useDebugValue: A$1, version: "16.8.0", Children: R, render: T, hydrate: T, unmountComponentAtNode: Q, createPortal: z, createElement: h$1, createContext: M$1, createFactory: G, cloneElement: K, createRef: y$1, Fragment: d$1, isValidElement: J, findDOMNode: X, Component: m$1, PureComponent: C, memo: _, forwardRef: S, unstable_batchedUpdates: Y, Suspense: U, SuspenseList: O, lazy: L };
const compat_module$1 = /* @__PURE__ */ Object.freeze(/* @__PURE__ */ Object.defineProperty({
  __proto__: null,
  Children: R,
  Component: m$1,
  Fragment: d$1,
  PureComponent: C,
  Suspense: U,
  SuspenseList: O,
  cloneElement: K,
  createContext: M$1,
  createElement: h$1,
  createFactory: G,
  createPortal: z,
  createRef: y$1,
  default: compat_module,
  findDOMNode: X,
  forwardRef: S,
  hydrate: V,
  isValidElement: J,
  lazy: L,
  memo: _,
  render: T,
  unmountComponentAtNode: Q,
  unstable_batchedUpdates: Y,
  useCallback: T$1,
  useContext: w$1,
  useDebugValue: A$1,
  useEffect: l,
  useErrorBoundary: F$1,
  useImperativeHandle: s,
  useLayoutEffect: y,
  useMemo: h,
  useReducer: p,
  useRef: d,
  useState: m,
  version: B
}, Symbol.toStringTag, { value: "Module" }));
const require$$3 = /* @__PURE__ */ getAugmentedNamespace(compat_module$1);
function _interopDefault(ex) {
  return ex && typeof ex === "object" && "default" in ex ? ex["default"] : ex;
}
var browserUtils = require$$0;
var QRCode = _interopDefault(browser);
var copy2 = _interopDefault(copyToClipboard);
var React = require$$3;
function open(uri) {
  QRCode.toString(uri, {
    type: "terminal"
  }).then(console.log);
}
var WALLETCONNECT_STYLE_SHEET = ':root {\n  --animation-duration: 300ms;\n}\n\n@keyframes fadeIn {\n  from {\n    opacity: 0;\n  }\n  to {\n    opacity: 1;\n  }\n}\n\n@keyframes fadeOut {\n  from {\n    opacity: 1;\n  }\n  to {\n    opacity: 0;\n  }\n}\n\n.animated {\n  animation-duration: var(--animation-duration);\n  animation-fill-mode: both;\n}\n\n.fadeIn {\n  animation-name: fadeIn;\n}\n\n.fadeOut {\n  animation-name: fadeOut;\n}\n\n#walletconnect-wrapper {\n  -webkit-user-select: none;\n  align-items: center;\n  display: flex;\n  height: 100%;\n  justify-content: center;\n  left: 0;\n  pointer-events: none;\n  position: fixed;\n  top: 0;\n  user-select: none;\n  width: 100%;\n  z-index: 99999999999999;\n}\n\n.walletconnect-modal__headerLogo {\n  height: 21px;\n}\n\n.walletconnect-modal__header p {\n  color: #ffffff;\n  font-size: 20px;\n  font-weight: 600;\n  margin: 0;\n  align-items: flex-start;\n  display: flex;\n  flex: 1;\n  margin-left: 5px;\n}\n\n.walletconnect-modal__close__wrapper {\n  position: absolute;\n  top: 0px;\n  right: 0px;\n  z-index: 10000;\n  background: white;\n  border-radius: 26px;\n  padding: 6px;\n  box-sizing: border-box;\n  width: 26px;\n  height: 26px;\n  cursor: pointer;\n}\n\n.walletconnect-modal__close__icon {\n  position: relative;\n  top: 7px;\n  right: 0;\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  transform: rotate(45deg);\n}\n\n.walletconnect-modal__close__line1 {\n  position: absolute;\n  width: 100%;\n  border: 1px solid rgb(48, 52, 59);\n}\n\n.walletconnect-modal__close__line2 {\n  position: absolute;\n  width: 100%;\n  border: 1px solid rgb(48, 52, 59);\n  transform: rotate(90deg);\n}\n\n.walletconnect-qrcode__base {\n  -webkit-tap-highlight-color: rgba(0, 0, 0, 0);\n  background: rgba(37, 41, 46, 0.95);\n  height: 100%;\n  left: 0;\n  pointer-events: auto;\n  position: fixed;\n  top: 0;\n  transition: 0.4s cubic-bezier(0.19, 1, 0.22, 1);\n  width: 100%;\n  will-change: opacity;\n  padding: 40px;\n  box-sizing: border-box;\n}\n\n.walletconnect-qrcode__text {\n  color: rgba(60, 66, 82, 0.6);\n  font-size: 16px;\n  font-weight: 600;\n  letter-spacing: 0;\n  line-height: 1.1875em;\n  margin: 10px 0 20px 0;\n  text-align: center;\n  width: 100%;\n}\n\n@media only screen and (max-width: 768px) {\n  .walletconnect-qrcode__text {\n    font-size: 4vw;\n  }\n}\n\n@media only screen and (max-width: 320px) {\n  .walletconnect-qrcode__text {\n    font-size: 14px;\n  }\n}\n\n.walletconnect-qrcode__image {\n  width: calc(100% - 30px);\n  box-sizing: border-box;\n  cursor: none;\n  margin: 0 auto;\n}\n\n.walletconnect-qrcode__notification {\n  position: absolute;\n  bottom: 0;\n  left: 0;\n  right: 0;\n  font-size: 16px;\n  padding: 16px 20px;\n  border-radius: 16px;\n  text-align: center;\n  transition: all 0.1s ease-in-out;\n  background: white;\n  color: black;\n  margin-bottom: -60px;\n  opacity: 0;\n}\n\n.walletconnect-qrcode__notification.notification__show {\n  opacity: 1;\n}\n\n@media only screen and (max-width: 768px) {\n  .walletconnect-modal__header {\n    height: 130px;\n  }\n  .walletconnect-modal__base {\n    overflow: auto;\n  }\n}\n\n@media only screen and (min-device-width: 415px) and (max-width: 768px) {\n  #content {\n    max-width: 768px;\n    box-sizing: border-box;\n  }\n}\n\n@media only screen and (min-width: 375px) and (max-width: 415px) {\n  #content {\n    max-width: 414px;\n    box-sizing: border-box;\n  }\n}\n\n@media only screen and (min-width: 320px) and (max-width: 375px) {\n  #content {\n    max-width: 375px;\n    box-sizing: border-box;\n  }\n}\n\n@media only screen and (max-width: 320px) {\n  #content {\n    max-width: 320px;\n    box-sizing: border-box;\n  }\n}\n\n.walletconnect-modal__base {\n  -webkit-font-smoothing: antialiased;\n  background: #ffffff;\n  border-radius: 24px;\n  box-shadow: 0 10px 50px 5px rgba(0, 0, 0, 0.4);\n  font-family: ui-rounded, "SF Pro Rounded", "SF Pro Text", medium-content-sans-serif-font,\n    -apple-system, BlinkMacSystemFont, ui-sans-serif, "Segoe UI", Roboto, Oxygen, Ubuntu, Cantarell,\n    "Open Sans", "Helvetica Neue", sans-serif;\n  margin-top: 41px;\n  padding: 24px 24px 22px;\n  pointer-events: auto;\n  position: relative;\n  text-align: center;\n  transition: 0.4s cubic-bezier(0.19, 1, 0.22, 1);\n  will-change: transform;\n  overflow: visible;\n  transform: translateY(-50%);\n  top: 50%;\n  max-width: 500px;\n  margin: auto;\n}\n\n@media only screen and (max-width: 320px) {\n  .walletconnect-modal__base {\n    padding: 24px 12px;\n  }\n}\n\n.walletconnect-modal__base .hidden {\n  transform: translateY(150%);\n  transition: 0.125s cubic-bezier(0.4, 0, 1, 1);\n}\n\n.walletconnect-modal__header {\n  align-items: center;\n  display: flex;\n  height: 26px;\n  left: 0;\n  justify-content: space-between;\n  position: absolute;\n  top: -42px;\n  width: 100%;\n}\n\n.walletconnect-modal__base .wc-logo {\n  align-items: center;\n  display: flex;\n  height: 26px;\n  margin-top: 15px;\n  padding-bottom: 15px;\n  pointer-events: auto;\n}\n\n.walletconnect-modal__base .wc-logo div {\n  background-color: #3399ff;\n  height: 21px;\n  margin-right: 5px;\n  mask-image: url("images/wc-logo.svg") center no-repeat;\n  width: 32px;\n}\n\n.walletconnect-modal__base .wc-logo p {\n  color: #ffffff;\n  font-size: 20px;\n  font-weight: 600;\n  margin: 0;\n}\n\n.walletconnect-modal__base h2 {\n  color: rgba(60, 66, 82, 0.6);\n  font-size: 16px;\n  font-weight: 600;\n  letter-spacing: 0;\n  line-height: 1.1875em;\n  margin: 0 0 19px 0;\n  text-align: center;\n  width: 100%;\n}\n\n.walletconnect-modal__base__row {\n  -webkit-tap-highlight-color: rgba(0, 0, 0, 0);\n  align-items: center;\n  border-radius: 20px;\n  cursor: pointer;\n  display: flex;\n  height: 56px;\n  justify-content: space-between;\n  padding: 0 15px;\n  position: relative;\n  margin: 0px 0px 8px;\n  text-align: left;\n  transition: 0.15s cubic-bezier(0.25, 0.46, 0.45, 0.94);\n  will-change: transform;\n  text-decoration: none;\n}\n\n.walletconnect-modal__base__row:hover {\n  background: rgba(60, 66, 82, 0.06);\n}\n\n.walletconnect-modal__base__row:active {\n  background: rgba(60, 66, 82, 0.06);\n  transform: scale(0.975);\n  transition: 0.1s cubic-bezier(0.25, 0.46, 0.45, 0.94);\n}\n\n.walletconnect-modal__base__row__h3 {\n  color: #25292e;\n  font-size: 20px;\n  font-weight: 700;\n  margin: 0;\n  padding-bottom: 3px;\n}\n\n.walletconnect-modal__base__row__right {\n  align-items: center;\n  display: flex;\n  justify-content: center;\n}\n\n.walletconnect-modal__base__row__right__app-icon {\n  border-radius: 8px;\n  height: 34px;\n  margin: 0 11px 2px 0;\n  width: 34px;\n  background-size: 100%;\n  box-shadow: 0 4px 12px 0 rgba(37, 41, 46, 0.25);\n}\n\n.walletconnect-modal__base__row__right__caret {\n  height: 18px;\n  opacity: 0.3;\n  transition: 0.1s cubic-bezier(0.25, 0.46, 0.45, 0.94);\n  width: 8px;\n  will-change: opacity;\n}\n\n.walletconnect-modal__base__row:hover .caret,\n.walletconnect-modal__base__row:active .caret {\n  opacity: 0.6;\n}\n\n.walletconnect-modal__mobile__toggle {\n  width: 80%;\n  display: flex;\n  margin: 0 auto;\n  position: relative;\n  overflow: hidden;\n  border-radius: 8px;\n  margin-bottom: 18px;\n  background: #d4d5d9;\n}\n\n.walletconnect-modal__single_wallet {\n  display: flex;\n  justify-content: center;\n  margin-top: 7px;\n  margin-bottom: 18px;\n}\n\n.walletconnect-modal__single_wallet a {\n  cursor: pointer;\n  color: rgb(64, 153, 255);\n  font-size: 21px;\n  font-weight: 800;\n  text-decoration: none !important;\n  margin: 0 auto;\n}\n\n.walletconnect-modal__mobile__toggle_selector {\n  width: calc(50% - 8px);\n  background: white;\n  position: absolute;\n  border-radius: 5px;\n  height: calc(100% - 8px);\n  top: 4px;\n  transition: all 0.2s ease-in-out;\n  transform: translate3d(4px, 0, 0);\n}\n\n.walletconnect-modal__mobile__toggle.right__selected .walletconnect-modal__mobile__toggle_selector {\n  transform: translate3d(calc(100% + 12px), 0, 0);\n}\n\n.walletconnect-modal__mobile__toggle a {\n  font-size: 12px;\n  width: 50%;\n  text-align: center;\n  padding: 8px;\n  margin: 0;\n  font-weight: 600;\n  z-index: 1;\n}\n\n.walletconnect-modal__footer {\n  display: flex;\n  justify-content: center;\n  margin-top: 20px;\n}\n\n@media only screen and (max-width: 768px) {\n  .walletconnect-modal__footer {\n    margin-top: 5vw;\n  }\n}\n\n.walletconnect-modal__footer a {\n  cursor: pointer;\n  color: #898d97;\n  font-size: 15px;\n  margin: 0 auto;\n}\n\n@media only screen and (max-width: 320px) {\n  .walletconnect-modal__footer a {\n    font-size: 14px;\n  }\n}\n\n.walletconnect-connect__buttons__wrapper {\n  max-height: 44vh;\n}\n\n.walletconnect-connect__buttons__wrapper__android {\n  margin: 50% 0;\n}\n\n.walletconnect-connect__buttons__wrapper__wrap {\n  display: grid;\n  grid-template-columns: repeat(4, 1fr);\n  margin: 10px 0;\n}\n\n@media only screen and (min-width: 768px) {\n  .walletconnect-connect__buttons__wrapper__wrap {\n    margin-top: 40px;\n  }\n}\n\n.walletconnect-connect__button {\n  background-color: rgb(64, 153, 255);\n  padding: 12px;\n  border-radius: 8px;\n  text-decoration: none;\n  color: rgb(255, 255, 255);\n  font-weight: 500;\n}\n\n.walletconnect-connect__button__icon_anchor {\n  cursor: pointer;\n  display: flex;\n  justify-content: flex-start;\n  align-items: center;\n  margin: 8px;\n  width: 42px;\n  justify-self: center;\n  flex-direction: column;\n  text-decoration: none !important;\n}\n\n@media only screen and (max-width: 320px) {\n  .walletconnect-connect__button__icon_anchor {\n    margin: 4px;\n  }\n}\n\n.walletconnect-connect__button__icon {\n  border-radius: 10px;\n  height: 42px;\n  margin: 0;\n  width: 42px;\n  background-size: cover !important;\n  box-shadow: 0 4px 12px 0 rgba(37, 41, 46, 0.25);\n}\n\n.walletconnect-connect__button__text {\n  color: #424952;\n  font-size: 2.7vw;\n  text-decoration: none !important;\n  padding: 0;\n  margin-top: 1.8vw;\n  font-weight: 600;\n}\n\n@media only screen and (min-width: 768px) {\n  .walletconnect-connect__button__text {\n    font-size: 16px;\n    margin-top: 12px;\n  }\n}\n\n.walletconnect-search__input {\n  border: none;\n  background: #d4d5d9;\n  border-style: none;\n  padding: 8px 16px;\n  outline: none;\n  font-style: normal;\n  font-stretch: normal;\n  font-size: 16px;\n  font-style: normal;\n  font-stretch: normal;\n  line-height: normal;\n  letter-spacing: normal;\n  text-align: left;\n  border-radius: 8px;\n  width: calc(100% - 16px);\n  margin: 0;\n  margin-bottom: 8px;\n}\n';
typeof Symbol !== "undefined" ? Symbol.iterator || (Symbol.iterator = Symbol("Symbol.iterator")) : "@@iterator";
typeof Symbol !== "undefined" ? Symbol.asyncIterator || (Symbol.asyncIterator = Symbol("Symbol.asyncIterator")) : "@@asyncIterator";
function _catch(body, recover) {
  try {
    var result = body();
  } catch (e2) {
    return recover(e2);
  }
  if (result && result.then) {
    return result.then(void 0, recover);
  }
  return result;
}
var WALLETCONNECT_LOGO_SVG_URL = "data:image/svg+xml,%3Csvg height='185' viewBox='0 0 300 185' width='300' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='m61.4385429 36.2562612c48.9112241-47.8881663 128.2119871-47.8881663 177.1232091 0l5.886545 5.7634174c2.445561 2.3944081 2.445561 6.2765112 0 8.6709204l-20.136695 19.715503c-1.222781 1.1972051-3.2053 1.1972051-4.428081 0l-8.100584-7.9311479c-34.121692-33.4079817-89.443886-33.4079817-123.5655788 0l-8.6750562 8.4936051c-1.2227816 1.1972041-3.205301 1.1972041-4.4280806 0l-20.1366949-19.7155031c-2.4455612-2.3944092-2.4455612-6.2765122 0-8.6709204zm218.7677961 40.7737449 17.921697 17.546897c2.445549 2.3943969 2.445563 6.2764769.000031 8.6708899l-80.810171 79.121134c-2.445544 2.394426-6.410582 2.394453-8.85616.000062-.00001-.00001-.000022-.000022-.000032-.000032l-57.354143-56.154572c-.61139-.598602-1.60265-.598602-2.21404 0-.000004.000004-.000007.000008-.000011.000011l-57.3529212 56.154531c-2.4455368 2.394432-6.4105755 2.394472-8.8561612.000087-.0000143-.000014-.0000296-.000028-.0000449-.000044l-80.81241943-79.122185c-2.44556021-2.394408-2.44556021-6.2765115 0-8.6709197l17.92172963-17.5468673c2.4455602-2.3944082 6.4105989-2.3944082 8.8561602 0l57.3549775 56.155357c.6113908.598602 1.602649.598602 2.2140398 0 .0000092-.000009.0000174-.000017.0000265-.000024l57.3521031-56.155333c2.445505-2.3944633 6.410544-2.3945531 8.856161-.0002.000034.0000336.000068.0000673.000101.000101l57.354902 56.155432c.61139.598601 1.60265.598601 2.21404 0l57.353975-56.1543249c2.445561-2.3944092 6.410599-2.3944092 8.85616 0z' fill='%233b99fc'/%3E%3C/svg%3E";
var WALLETCONNECT_HEADER_TEXT = "WalletConnect";
var ANIMATION_DURATION = 300;
var DEFAULT_BUTTON_COLOR = "rgb(64, 153, 255)";
var WALLETCONNECT_WRAPPER_ID = "walletconnect-wrapper";
var WALLETCONNECT_STYLE_ID = "walletconnect-style-sheet";
var WALLETCONNECT_MODAL_ID = "walletconnect-qrcode-modal";
var WALLETCONNECT_CLOSE_BUTTON_ID = "walletconnect-qrcode-close";
var WALLETCONNECT_CTA_TEXT_ID = "walletconnect-qrcode-text";
var WALLETCONNECT_CONNECT_BUTTON_ID = "walletconnect-connect-button";
function Header(props) {
  return React.createElement("div", {
    className: "walletconnect-modal__header"
  }, React.createElement("img", {
    src: WALLETCONNECT_LOGO_SVG_URL,
    className: "walletconnect-modal__headerLogo"
  }), React.createElement("p", null, WALLETCONNECT_HEADER_TEXT), React.createElement("div", {
    className: "walletconnect-modal__close__wrapper",
    onClick: props.onClose
  }, React.createElement("div", {
    id: WALLETCONNECT_CLOSE_BUTTON_ID,
    className: "walletconnect-modal__close__icon"
  }, React.createElement("div", {
    className: "walletconnect-modal__close__line1"
  }), React.createElement("div", {
    className: "walletconnect-modal__close__line2"
  }))));
}
function ConnectButton(props) {
  return React.createElement("a", {
    className: "walletconnect-connect__button",
    href: props.href,
    id: WALLETCONNECT_CONNECT_BUTTON_ID + "-" + props.name,
    onClick: props.onClick,
    rel: "noopener noreferrer",
    style: {
      backgroundColor: props.color
    },
    target: "_blank"
  }, props.name);
}
var CARET_SVG_URL = "data:image/svg+xml,%3Csvg fill='none' height='18' viewBox='0 0 8 18' width='8' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath clip-rule='evenodd' d='m.586301.213898c-.435947.33907-.5144813.967342-.175411 1.403292l4.87831 6.27212c.28087.36111.28087.86677 0 1.22788l-4.878311 6.27211c-.33907.436-.260536 1.0642.175412 1.4033.435949.3391 1.064219.2605 1.403289-.1754l4.87832-6.2721c.84259-1.08336.84259-2.60034 0-3.68367l-4.87832-6.27212c-.33907-.4359474-.96734-.514482-1.403289-.175412z' fill='%233c4252' fill-rule='evenodd'/%3E%3C/svg%3E";
function WalletButton(props) {
  var color = props.color;
  var href = props.href;
  var name = props.name;
  var logo = props.logo;
  var onClick = props.onClick;
  return React.createElement("a", {
    className: "walletconnect-modal__base__row",
    href,
    onClick,
    rel: "noopener noreferrer",
    target: "_blank"
  }, React.createElement("h3", {
    className: "walletconnect-modal__base__row__h3"
  }, name), React.createElement("div", {
    className: "walletconnect-modal__base__row__right"
  }, React.createElement("div", {
    className: "walletconnect-modal__base__row__right__app-icon",
    style: {
      background: "url('" + logo + "') " + color,
      backgroundSize: "100%"
    }
  }), React.createElement("img", {
    src: CARET_SVG_URL,
    className: "walletconnect-modal__base__row__right__caret"
  })));
}
function WalletIcon(props) {
  var color = props.color;
  var href = props.href;
  var name = props.name;
  var logo = props.logo;
  var onClick = props.onClick;
  var fontSize = window.innerWidth < 768 ? (name.length > 8 ? 2.5 : 2.7) + "vw" : "inherit";
  return React.createElement("a", {
    className: "walletconnect-connect__button__icon_anchor",
    href,
    onClick,
    rel: "noopener noreferrer",
    target: "_blank"
  }, React.createElement("div", {
    className: "walletconnect-connect__button__icon",
    style: {
      background: "url('" + logo + "') " + color,
      backgroundSize: "100%"
    }
  }), React.createElement("div", {
    style: {
      fontSize
    },
    className: "walletconnect-connect__button__text"
  }, name));
}
var GRID_MIN_COUNT = 5;
var LINKS_PER_PAGE = 12;
function LinkDisplay(props) {
  var android = browserUtils.isAndroid();
  var ref = React.useState("");
  var input = ref[0];
  var setInput = ref[1];
  var ref$1 = React.useState("");
  var filter = ref$1[0];
  var setFilter = ref$1[1];
  var ref$2 = React.useState(1);
  var page = ref$2[0];
  var setPage = ref$2[1];
  var links = filter ? props.links.filter(function(link) {
    return link.name.toLowerCase().includes(filter.toLowerCase());
  }) : props.links;
  var errorMessage = props.errorMessage;
  var grid = filter || links.length > GRID_MIN_COUNT;
  var pages = Math.ceil(links.length / LINKS_PER_PAGE);
  var range = [(page - 1) * LINKS_PER_PAGE + 1, page * LINKS_PER_PAGE];
  var pageLinks = links.length ? links.filter(function(_2, index2) {
    return index2 + 1 >= range[0] && index2 + 1 <= range[1];
  }) : [];
  var hasPaging = !!(!android && pages > 1);
  var filterTimeout = void 0;
  function handleInput(e2) {
    setInput(e2.target.value);
    clearTimeout(filterTimeout);
    if (e2.target.value) {
      filterTimeout = setTimeout(function() {
        setFilter(e2.target.value);
        setPage(1);
      }, 1e3);
    } else {
      setInput("");
      setFilter("");
      setPage(1);
    }
  }
  return React.createElement("div", null, React.createElement("p", {
    id: WALLETCONNECT_CTA_TEXT_ID,
    className: "walletconnect-qrcode__text"
  }, android ? props.text.connect_mobile_wallet : props.text.choose_preferred_wallet), !android && React.createElement("input", {
    className: "walletconnect-search__input",
    placeholder: "Search",
    value: input,
    onChange: handleInput
  }), React.createElement("div", {
    className: "walletconnect-connect__buttons__wrapper" + (android ? "__android" : grid && links.length ? "__wrap" : "")
  }, !android ? pageLinks.length ? pageLinks.map(function(entry) {
    var color = entry.color;
    var name = entry.name;
    var shortName = entry.shortName;
    var logo = entry.logo;
    var href = browserUtils.formatIOSMobile(props.uri, entry);
    var handleClickIOS = React.useCallback(function() {
      browserUtils.saveMobileLinkInfo({
        name,
        href
      });
    }, [pageLinks]);
    return !grid ? React.createElement(WalletButton, {
      color,
      href,
      name,
      logo,
      onClick: handleClickIOS
    }) : React.createElement(WalletIcon, {
      color,
      href,
      name: shortName || name,
      logo,
      onClick: handleClickIOS
    });
  }) : React.createElement(React.Fragment, null, React.createElement("p", null, errorMessage.length ? props.errorMessage : !!props.links.length && !links.length ? props.text.no_wallets_found : props.text.loading)) : React.createElement(ConnectButton, {
    name: props.text.connect,
    color: DEFAULT_BUTTON_COLOR,
    href: props.uri,
    onClick: React.useCallback(function() {
      browserUtils.saveMobileLinkInfo({
        name: "Unknown",
        href: props.uri
      });
    }, [])
  })), hasPaging && React.createElement("div", {
    className: "walletconnect-modal__footer"
  }, Array(pages).fill(0).map(function(_2, index2) {
    var pageNumber = index2 + 1;
    var selected = page === pageNumber;
    return React.createElement("a", {
      style: {
        margin: "auto 10px",
        fontWeight: selected ? "bold" : "normal"
      },
      onClick: function() {
        return setPage(pageNumber);
      }
    }, pageNumber);
  })));
}
function Notification(props) {
  var show = !!props.message.trim();
  return React.createElement("div", {
    className: "walletconnect-qrcode__notification" + (show ? " notification__show" : "")
  }, props.message);
}
var formatQRCodeImage = function(data) {
  try {
    var result = "";
    return Promise.resolve(QRCode.toString(data, {
      margin: 0,
      type: "svg"
    })).then(function(dataString) {
      if (typeof dataString === "string") {
        result = dataString.replace("<svg", '<svg class="walletconnect-qrcode__image"');
      }
      return result;
    });
  } catch (e2) {
    return Promise.reject(e2);
  }
};
function QRCodeDisplay(props) {
  var ref = React.useState("");
  var notification = ref[0];
  var setNotification = ref[1];
  var ref$1 = React.useState("");
  var svg = ref$1[0];
  var setSvg = ref$1[1];
  React.useEffect(function() {
    try {
      return Promise.resolve(formatQRCodeImage(props.uri)).then(function(_formatQRCodeImage) {
        setSvg(_formatQRCodeImage);
      });
    } catch (e2) {
      Promise.reject(e2);
    }
  }, []);
  var copyToClipboard2 = function() {
    var success = copy2(props.uri);
    if (success) {
      setNotification(props.text.copied_to_clipboard);
      setInterval(function() {
        return setNotification("");
      }, 1200);
    } else {
      setNotification("Error");
      setInterval(function() {
        return setNotification("");
      }, 1200);
    }
  };
  return React.createElement("div", null, React.createElement("p", {
    id: WALLETCONNECT_CTA_TEXT_ID,
    className: "walletconnect-qrcode__text"
  }, props.text.scan_qrcode_with_wallet), React.createElement("div", {
    dangerouslySetInnerHTML: {
      __html: svg
    }
  }), React.createElement("div", {
    className: "walletconnect-modal__footer"
  }, React.createElement("a", {
    onClick: copyToClipboard2
  }, props.text.copy_to_clipboard)), React.createElement(Notification, {
    message: notification
  }));
}
function Modal(props) {
  var android = browserUtils.isAndroid();
  var mobile = browserUtils.isMobile();
  var whitelist = mobile ? props.qrcodeModalOptions && props.qrcodeModalOptions.mobileLinks ? props.qrcodeModalOptions.mobileLinks : void 0 : props.qrcodeModalOptions && props.qrcodeModalOptions.desktopLinks ? props.qrcodeModalOptions.desktopLinks : void 0;
  var ref = React.useState(false);
  var loading = ref[0];
  var setLoading = ref[1];
  var ref$1 = React.useState(false);
  var fetched = ref$1[0];
  var setFetched = ref$1[1];
  var ref$2 = React.useState(!mobile);
  var displayQRCode = ref$2[0];
  var setDisplayQRCode = ref$2[1];
  var displayProps = {
    mobile,
    text: props.text,
    uri: props.uri,
    qrcodeModalOptions: props.qrcodeModalOptions
  };
  var ref$3 = React.useState("");
  var singleLinkHref = ref$3[0];
  var setSingleLinkHref = ref$3[1];
  var ref$4 = React.useState(false);
  var hasSingleLink = ref$4[0];
  var setHasSingleLink = ref$4[1];
  var ref$5 = React.useState([]);
  var links = ref$5[0];
  var setLinks = ref$5[1];
  var ref$6 = React.useState("");
  var errorMessage = ref$6[0];
  var setErrorMessage = ref$6[1];
  var getLinksIfNeeded = function() {
    if (fetched || loading || whitelist && !whitelist.length || links.length > 0) {
      return;
    }
    React.useEffect(function() {
      var initLinks = function() {
        try {
          if (android) {
            return Promise.resolve();
          }
          setLoading(true);
          var _temp = _catch(function() {
            var url = props.qrcodeModalOptions && props.qrcodeModalOptions.registryUrl ? props.qrcodeModalOptions.registryUrl : browserUtils.getWalletRegistryUrl();
            return Promise.resolve(fetch(url)).then(function(registryResponse) {
              return Promise.resolve(registryResponse.json()).then(function(_registryResponse$jso) {
                var registry = _registryResponse$jso.listings;
                var platform = mobile ? "mobile" : "desktop";
                var _links = browserUtils.getMobileLinkRegistry(browserUtils.formatMobileRegistry(registry, platform), whitelist);
                setLoading(false);
                setFetched(true);
                setErrorMessage(!_links.length ? props.text.no_supported_wallets : "");
                setLinks(_links);
                var hasSingleLink2 = _links.length === 1;
                if (hasSingleLink2) {
                  setSingleLinkHref(browserUtils.formatIOSMobile(props.uri, _links[0]));
                  setDisplayQRCode(true);
                }
                setHasSingleLink(hasSingleLink2);
              });
            });
          }, function(e2) {
            setLoading(false);
            setFetched(true);
            setErrorMessage(props.text.something_went_wrong);
            console.error(e2);
          });
          return Promise.resolve(_temp && _temp.then ? _temp.then(function() {
          }) : void 0);
        } catch (e2) {
          return Promise.reject(e2);
        }
      };
      initLinks();
    });
  };
  getLinksIfNeeded();
  var rightSelected = mobile ? displayQRCode : !displayQRCode;
  return React.createElement("div", {
    id: WALLETCONNECT_MODAL_ID,
    className: "walletconnect-qrcode__base animated fadeIn"
  }, React.createElement("div", {
    className: "walletconnect-modal__base"
  }, React.createElement(Header, {
    onClose: props.onClose
  }), hasSingleLink && displayQRCode ? React.createElement("div", {
    className: "walletconnect-modal__single_wallet"
  }, React.createElement("a", {
    onClick: function() {
      return browserUtils.saveMobileLinkInfo({
        name: links[0].name,
        href: singleLinkHref
      });
    },
    href: singleLinkHref,
    rel: "noopener noreferrer",
    target: "_blank"
  }, props.text.connect_with + " " + (hasSingleLink ? links[0].name : "") + " ›")) : android || loading || !loading && links.length ? React.createElement("div", {
    className: "walletconnect-modal__mobile__toggle" + (rightSelected ? " right__selected" : "")
  }, React.createElement("div", {
    className: "walletconnect-modal__mobile__toggle_selector"
  }), mobile ? React.createElement(React.Fragment, null, React.createElement("a", {
    onClick: function() {
      return setDisplayQRCode(false), getLinksIfNeeded();
    }
  }, props.text.mobile), React.createElement("a", {
    onClick: function() {
      return setDisplayQRCode(true);
    }
  }, props.text.qrcode)) : React.createElement(React.Fragment, null, React.createElement("a", {
    onClick: function() {
      return setDisplayQRCode(true);
    }
  }, props.text.qrcode), React.createElement("a", {
    onClick: function() {
      return setDisplayQRCode(false), getLinksIfNeeded();
    }
  }, props.text.desktop))) : null, React.createElement("div", null, displayQRCode || !android && !loading && !links.length ? React.createElement(QRCodeDisplay, Object.assign({}, displayProps)) : React.createElement(LinkDisplay, Object.assign(
    {},
    displayProps,
    {
      links,
      errorMessage
    }
  )))));
}
var de = {
  choose_preferred_wallet: "Wähle bevorzugte Wallet",
  connect_mobile_wallet: "Verbinde mit Mobile Wallet",
  scan_qrcode_with_wallet: "Scanne den QR-code mit einer WalletConnect kompatiblen Wallet",
  connect: "Verbinden",
  qrcode: "QR-Code",
  mobile: "Mobile",
  desktop: "Desktop",
  copy_to_clipboard: "In die Zwischenablage kopieren",
  copied_to_clipboard: "In die Zwischenablage kopiert!",
  connect_with: "Verbinden mit Hilfe von",
  loading: "Laden...",
  something_went_wrong: "Etwas ist schief gelaufen",
  no_supported_wallets: "Es gibt noch keine unterstützten Wallet",
  no_wallets_found: "keine Wallet gefunden"
};
var en = {
  choose_preferred_wallet: "Choose your preferred wallet",
  connect_mobile_wallet: "Connect to Mobile Wallet",
  scan_qrcode_with_wallet: "Scan QR code with a WalletConnect-compatible wallet",
  connect: "Connect",
  qrcode: "QR Code",
  mobile: "Mobile",
  desktop: "Desktop",
  copy_to_clipboard: "Copy to clipboard",
  copied_to_clipboard: "Copied to clipboard!",
  connect_with: "Connect with",
  loading: "Loading...",
  something_went_wrong: "Something went wrong",
  no_supported_wallets: "There are no supported wallets yet",
  no_wallets_found: "No wallets found"
};
var es = {
  choose_preferred_wallet: "Elige tu billetera preferida",
  connect_mobile_wallet: "Conectar a billetera móvil",
  scan_qrcode_with_wallet: "Escanea el código QR con una billetera compatible con WalletConnect",
  connect: "Conectar",
  qrcode: "Código QR",
  mobile: "Móvil",
  desktop: "Desktop",
  copy_to_clipboard: "Copiar",
  copied_to_clipboard: "Copiado!",
  connect_with: "Conectar mediante",
  loading: "Cargando...",
  something_went_wrong: "Algo salió mal",
  no_supported_wallets: "Todavía no hay billeteras compatibles",
  no_wallets_found: "No se encontraron billeteras"
};
var fr = {
  choose_preferred_wallet: "Choisissez votre portefeuille préféré",
  connect_mobile_wallet: "Se connecter au portefeuille mobile",
  scan_qrcode_with_wallet: "Scannez le QR code avec un portefeuille compatible WalletConnect",
  connect: "Se connecter",
  qrcode: "QR Code",
  mobile: "Mobile",
  desktop: "Desktop",
  copy_to_clipboard: "Copier",
  copied_to_clipboard: "Copié!",
  connect_with: "Connectez-vous à l'aide de",
  loading: "Chargement...",
  something_went_wrong: "Quelque chose a mal tourné",
  no_supported_wallets: "Il n'y a pas encore de portefeuilles pris en charge",
  no_wallets_found: "Aucun portefeuille trouvé"
};
var ko = {
  choose_preferred_wallet: "원하는 지갑을 선택하세요",
  connect_mobile_wallet: "모바일 지갑과 연결",
  scan_qrcode_with_wallet: "WalletConnect 지원 지갑에서 QR코드를 스캔하세요",
  connect: "연결",
  qrcode: "QR 코드",
  mobile: "모바일",
  desktop: "데스크탑",
  copy_to_clipboard: "클립보드에 복사",
  copied_to_clipboard: "클립보드에 복사되었습니다!",
  connect_with: "와 연결하다",
  loading: "로드 중...",
  something_went_wrong: "문제가 발생했습니다.",
  no_supported_wallets: "아직 지원되는 지갑이 없습니다",
  no_wallets_found: "지갑을 찾을 수 없습니다"
};
var pt = {
  choose_preferred_wallet: "Escolha sua carteira preferida",
  connect_mobile_wallet: "Conectar-se à carteira móvel",
  scan_qrcode_with_wallet: "Ler o código QR com uma carteira compatível com WalletConnect",
  connect: "Conectar",
  qrcode: "Código QR",
  mobile: "Móvel",
  desktop: "Desktop",
  copy_to_clipboard: "Copiar",
  copied_to_clipboard: "Copiado!",
  connect_with: "Ligar por meio de",
  loading: "Carregamento...",
  something_went_wrong: "Algo correu mal",
  no_supported_wallets: "Ainda não há carteiras suportadas",
  no_wallets_found: "Nenhuma carteira encontrada"
};
var zh = {
  choose_preferred_wallet: "选择你的钱包",
  connect_mobile_wallet: "连接至移动端钱包",
  scan_qrcode_with_wallet: "使用兼容 WalletConnect 的钱包扫描二维码",
  connect: "连接",
  qrcode: "二维码",
  mobile: "移动",
  desktop: "桌面",
  copy_to_clipboard: "复制到剪贴板",
  copied_to_clipboard: "复制到剪贴板成功！",
  connect_with: "通过以下方式连接",
  loading: "正在加载...",
  something_went_wrong: "出了问题",
  no_supported_wallets: "目前还没有支持的钱包",
  no_wallets_found: "没有找到钱包"
};
var fa = {
  choose_preferred_wallet: "کیف پول مورد نظر خود را انتخاب کنید",
  connect_mobile_wallet: "به کیف پول موبایل وصل شوید",
  scan_qrcode_with_wallet: "کد QR را با یک کیف پول سازگار با WalletConnect اسکن کنید",
  connect: "اتصال",
  qrcode: "کد QR",
  mobile: "سیار",
  desktop: "دسکتاپ",
  copy_to_clipboard: "کپی به کلیپ بورد",
  copied_to_clipboard: "در کلیپ بورد کپی شد!",
  connect_with: "ارتباط با",
  loading: "...بارگذاری",
  something_went_wrong: "مشکلی پیش آمد",
  no_supported_wallets: "هنوز هیچ کیف پول پشتیبانی شده ای وجود ندارد",
  no_wallets_found: "هیچ کیف پولی پیدا نشد"
};
var languages = {
  de,
  en,
  es,
  fr,
  ko,
  pt,
  zh,
  fa
};
function injectStyleSheet() {
  var doc = browserUtils.getDocumentOrThrow();
  var prev = doc.getElementById(WALLETCONNECT_STYLE_ID);
  if (prev) {
    doc.head.removeChild(prev);
  }
  var style = doc.createElement("style");
  style.setAttribute("id", WALLETCONNECT_STYLE_ID);
  style.innerText = WALLETCONNECT_STYLE_SHEET;
  doc.head.appendChild(style);
}
function renderWrapper() {
  var doc = browserUtils.getDocumentOrThrow();
  var wrapper = doc.createElement("div");
  wrapper.setAttribute("id", WALLETCONNECT_WRAPPER_ID);
  doc.body.appendChild(wrapper);
  return wrapper;
}
function triggerCloseAnimation() {
  var doc = browserUtils.getDocumentOrThrow();
  var modal = doc.getElementById(WALLETCONNECT_MODAL_ID);
  if (modal) {
    modal.className = modal.className.replace("fadeIn", "fadeOut");
    setTimeout(function() {
      var wrapper = doc.getElementById(WALLETCONNECT_WRAPPER_ID);
      if (wrapper) {
        doc.body.removeChild(wrapper);
      }
    }, ANIMATION_DURATION);
  }
}
function getWrappedCallback(cb) {
  return function() {
    triggerCloseAnimation();
    if (cb) {
      cb();
    }
  };
}
function getText() {
  var lang = browserUtils.getNavigatorOrThrow().language.split("-")[0] || "en";
  return languages[lang] || languages["en"];
}
function open$1(uri, cb, qrcodeModalOptions) {
  injectStyleSheet();
  var wrapper = renderWrapper();
  React.render(React.createElement(Modal, {
    text: getText(),
    uri,
    onClose: getWrappedCallback(cb),
    qrcodeModalOptions
  }), wrapper);
}
function close$1() {
  triggerCloseAnimation();
}
var isNode = function() {
  return typeof browserExports !== "undefined" && typeof browserExports.versions !== "undefined" && typeof browserExports.versions.node !== "undefined";
};
function open$2(uri, cb, qrcodeModalOptions) {
  console.log(uri);
  if (isNode()) {
    open(uri);
  } else {
    open$1(uri, cb, qrcodeModalOptions);
  }
}
function close$2() {
  if (isNode())
    ;
  else {
    close$1();
  }
}
var index = {
  open: open$2,
  close: close$2
};
var cjs = index;
const index$1 = /* @__PURE__ */ _mergeNamespaces({
  __proto__: null,
  default: cjs
}, [cjs]);
export {
  index$1 as i
};
