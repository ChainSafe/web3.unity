import { V as browserExports } from "./index-cfcd831a.js";
var cjs$2 = {};
var cjs$1 = {};
Object.defineProperty(cjs$1, "__esModule", { value: true });
cjs$1.getLocalStorage = cjs$1.getLocalStorageOrThrow = cjs$1.getCrypto = cjs$1.getCryptoOrThrow = cjs$1.getLocation = cjs$1.getLocationOrThrow = cjs$1.getNavigator = cjs$1.getNavigatorOrThrow = cjs$1.getDocument = cjs$1.getDocumentOrThrow = cjs$1.getFromWindowOrThrow = cjs$1.getFromWindow = void 0;
function getFromWindow$2(name) {
  let res = void 0;
  if (typeof window !== "undefined" && typeof window[name] !== "undefined") {
    res = window[name];
  }
  return res;
}
cjs$1.getFromWindow = getFromWindow$2;
function getFromWindowOrThrow$2(name) {
  const res = getFromWindow$2(name);
  if (!res) {
    throw new Error(`${name} is not defined in Window`);
  }
  return res;
}
cjs$1.getFromWindowOrThrow = getFromWindowOrThrow$2;
function getDocumentOrThrow$2() {
  return getFromWindowOrThrow$2("document");
}
cjs$1.getDocumentOrThrow = getDocumentOrThrow$2;
function getDocument$2() {
  return getFromWindow$2("document");
}
cjs$1.getDocument = getDocument$2;
function getNavigatorOrThrow$2() {
  return getFromWindowOrThrow$2("navigator");
}
cjs$1.getNavigatorOrThrow = getNavigatorOrThrow$2;
function getNavigator$2() {
  return getFromWindow$2("navigator");
}
cjs$1.getNavigator = getNavigator$2;
function getLocationOrThrow$2() {
  return getFromWindowOrThrow$2("location");
}
cjs$1.getLocationOrThrow = getLocationOrThrow$2;
function getLocation$2() {
  return getFromWindow$2("location");
}
cjs$1.getLocation = getLocation$2;
function getCryptoOrThrow$2() {
  return getFromWindowOrThrow$2("crypto");
}
cjs$1.getCryptoOrThrow = getCryptoOrThrow$2;
function getCrypto$2() {
  return getFromWindow$2("crypto");
}
cjs$1.getCrypto = getCrypto$2;
function getLocalStorageOrThrow$2() {
  return getFromWindowOrThrow$2("localStorage");
}
cjs$1.getLocalStorageOrThrow = getLocalStorageOrThrow$2;
function getLocalStorage$2() {
  return getFromWindow$2("localStorage");
}
cjs$1.getLocalStorage = getLocalStorage$2;
Object.defineProperty(cjs$2, "__esModule", { value: true });
var getWindowMetadata_1 = cjs$2.getWindowMetadata = void 0;
const window_getters_1 = cjs$1;
function getWindowMetadata() {
  let doc;
  let loc;
  try {
    doc = window_getters_1.getDocumentOrThrow();
    loc = window_getters_1.getLocationOrThrow();
  } catch (e) {
    return null;
  }
  function getIcons() {
    const links = doc.getElementsByTagName("link");
    const icons2 = [];
    for (let i = 0; i < links.length; i++) {
      const link = links[i];
      const rel = link.getAttribute("rel");
      if (rel) {
        if (rel.toLowerCase().indexOf("icon") > -1) {
          const href = link.getAttribute("href");
          if (href) {
            if (href.toLowerCase().indexOf("https:") === -1 && href.toLowerCase().indexOf("http:") === -1 && href.indexOf("//") !== 0) {
              let absoluteHref = loc.protocol + "//" + loc.host;
              if (href.indexOf("/") === 0) {
                absoluteHref += href;
              } else {
                const path = loc.pathname.split("/");
                path.pop();
                const finalPath = path.join("/");
                absoluteHref += finalPath + "/" + href;
              }
              icons2.push(absoluteHref);
            } else if (href.indexOf("//") === 0) {
              const absoluteUrl = loc.protocol + href;
              icons2.push(absoluteUrl);
            } else {
              icons2.push(href);
            }
          }
        }
      }
    }
    return icons2;
  }
  function getWindowMetadataOfAny(...args) {
    const metaTags = doc.getElementsByTagName("meta");
    for (let i = 0; i < metaTags.length; i++) {
      const tag = metaTags[i];
      const attributes = ["itemprop", "property", "name"].map((target) => tag.getAttribute(target)).filter((attr) => {
        if (attr) {
          return args.includes(attr);
        }
        return false;
      });
      if (attributes.length && attributes) {
        const content = tag.getAttribute("content");
        if (content) {
          return content;
        }
      }
    }
    return "";
  }
  function getName() {
    let name2 = getWindowMetadataOfAny("name", "og:site_name", "og:title", "twitter:title");
    if (!name2) {
      name2 = doc.title;
    }
    return name2;
  }
  function getDescription() {
    const description2 = getWindowMetadataOfAny("description", "og:description", "twitter:description", "keywords");
    return description2;
  }
  const name = getName();
  const description = getDescription();
  const url = loc.origin;
  const icons = getIcons();
  const meta = {
    description,
    url,
    icons,
    name
  };
  return meta;
}
getWindowMetadata_1 = cjs$2.getWindowMetadata = getWindowMetadata;
var cjs = {};
Object.defineProperty(cjs, "__esModule", { value: true });
var getLocalStorage_1 = cjs.getLocalStorage = getLocalStorageOrThrow_1 = cjs.getLocalStorageOrThrow = getCrypto_1 = cjs.getCrypto = getCryptoOrThrow_1 = cjs.getCryptoOrThrow = getLocation_1 = cjs.getLocation = getLocationOrThrow_1 = cjs.getLocationOrThrow = getNavigator_1 = cjs.getNavigator = getNavigatorOrThrow_1 = cjs.getNavigatorOrThrow = getDocument_1 = cjs.getDocument = getDocumentOrThrow_1 = cjs.getDocumentOrThrow = getFromWindowOrThrow_1 = cjs.getFromWindowOrThrow = getFromWindow_1 = cjs.getFromWindow = void 0;
function getFromWindow$1(name) {
  let res = void 0;
  if (typeof window !== "undefined" && typeof window[name] !== "undefined") {
    res = window[name];
  }
  return res;
}
var getFromWindow_1 = cjs.getFromWindow = getFromWindow$1;
function getFromWindowOrThrow$1(name) {
  const res = getFromWindow$1(name);
  if (!res) {
    throw new Error(`${name} is not defined in Window`);
  }
  return res;
}
var getFromWindowOrThrow_1 = cjs.getFromWindowOrThrow = getFromWindowOrThrow$1;
function getDocumentOrThrow$1() {
  return getFromWindowOrThrow$1("document");
}
var getDocumentOrThrow_1 = cjs.getDocumentOrThrow = getDocumentOrThrow$1;
function getDocument$1() {
  return getFromWindow$1("document");
}
var getDocument_1 = cjs.getDocument = getDocument$1;
function getNavigatorOrThrow$1() {
  return getFromWindowOrThrow$1("navigator");
}
var getNavigatorOrThrow_1 = cjs.getNavigatorOrThrow = getNavigatorOrThrow$1;
function getNavigator$1() {
  return getFromWindow$1("navigator");
}
var getNavigator_1 = cjs.getNavigator = getNavigator$1;
function getLocationOrThrow$1() {
  return getFromWindowOrThrow$1("location");
}
var getLocationOrThrow_1 = cjs.getLocationOrThrow = getLocationOrThrow$1;
function getLocation$1() {
  return getFromWindow$1("location");
}
var getLocation_1 = cjs.getLocation = getLocation$1;
function getCryptoOrThrow$1() {
  return getFromWindowOrThrow$1("crypto");
}
var getCryptoOrThrow_1 = cjs.getCryptoOrThrow = getCryptoOrThrow$1;
function getCrypto$1() {
  return getFromWindow$1("crypto");
}
var getCrypto_1 = cjs.getCrypto = getCrypto$1;
function getLocalStorageOrThrow$1() {
  return getFromWindowOrThrow$1("localStorage");
}
var getLocalStorageOrThrow_1 = cjs.getLocalStorageOrThrow = getLocalStorageOrThrow$1;
function getLocalStorage$1() {
  return getFromWindow$1("localStorage");
}
getLocalStorage_1 = cjs.getLocalStorage = getLocalStorage$1;
var __spreadArrays = globalThis && globalThis.__spreadArrays || function() {
  for (var s = 0, i = 0, il = arguments.length; i < il; i++)
    s += arguments[i].length;
  for (var r = Array(s), k = 0, i = 0; i < il; i++)
    for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
      r[k] = a[j];
  return r;
};
var BrowserInfo = (
  /** @class */
  function() {
    function BrowserInfo2(name, version, os) {
      this.name = name;
      this.version = version;
      this.os = os;
      this.type = "browser";
    }
    return BrowserInfo2;
  }()
);
var NodeInfo = (
  /** @class */
  function() {
    function NodeInfo2(version) {
      this.version = version;
      this.type = "node";
      this.name = "node";
      this.os = browserExports.platform;
    }
    return NodeInfo2;
  }()
);
var SearchBotDeviceInfo = (
  /** @class */
  function() {
    function SearchBotDeviceInfo2(name, version, os, bot) {
      this.name = name;
      this.version = version;
      this.os = os;
      this.bot = bot;
      this.type = "bot-device";
    }
    return SearchBotDeviceInfo2;
  }()
);
var BotInfo = (
  /** @class */
  function() {
    function BotInfo2() {
      this.type = "bot";
      this.bot = true;
      this.name = "bot";
      this.version = null;
      this.os = null;
    }
    return BotInfo2;
  }()
);
var ReactNativeInfo = (
  /** @class */
  function() {
    function ReactNativeInfo2() {
      this.type = "react-native";
      this.name = "react-native";
      this.version = null;
      this.os = null;
    }
    return ReactNativeInfo2;
  }()
);
var SEARCHBOX_UA_REGEX = /alexa|bot|crawl(er|ing)|facebookexternalhit|feedburner|google web preview|nagios|postrank|pingdom|slurp|spider|yahoo!|yandex/;
var SEARCHBOT_OS_REGEX = /(nuhk|Googlebot|Yammybot|Openbot|Slurp|MSNBot|Ask\ Jeeves\/Teoma|ia_archiver)/;
var REQUIRED_VERSION_PARTS = 3;
var userAgentRules = [
  ["aol", /AOLShield\/([0-9\._]+)/],
  ["edge", /Edge\/([0-9\._]+)/],
  ["edge-ios", /EdgiOS\/([0-9\._]+)/],
  ["yandexbrowser", /YaBrowser\/([0-9\._]+)/],
  ["kakaotalk", /KAKAOTALK\s([0-9\.]+)/],
  ["samsung", /SamsungBrowser\/([0-9\.]+)/],
  ["silk", /\bSilk\/([0-9._-]+)\b/],
  ["miui", /MiuiBrowser\/([0-9\.]+)$/],
  ["beaker", /BeakerBrowser\/([0-9\.]+)/],
  ["edge-chromium", /EdgA?\/([0-9\.]+)/],
  [
    "chromium-webview",
    /(?!Chrom.*OPR)wv\).*Chrom(?:e|ium)\/([0-9\.]+)(:?\s|$)/
  ],
  ["chrome", /(?!Chrom.*OPR)Chrom(?:e|ium)\/([0-9\.]+)(:?\s|$)/],
  ["phantomjs", /PhantomJS\/([0-9\.]+)(:?\s|$)/],
  ["crios", /CriOS\/([0-9\.]+)(:?\s|$)/],
  ["firefox", /Firefox\/([0-9\.]+)(?:\s|$)/],
  ["fxios", /FxiOS\/([0-9\.]+)/],
  ["opera-mini", /Opera Mini.*Version\/([0-9\.]+)/],
  ["opera", /Opera\/([0-9\.]+)(?:\s|$)/],
  ["opera", /OPR\/([0-9\.]+)(:?\s|$)/],
  ["ie", /Trident\/7\.0.*rv\:([0-9\.]+).*\).*Gecko$/],
  ["ie", /MSIE\s([0-9\.]+);.*Trident\/[4-7].0/],
  ["ie", /MSIE\s(7\.0)/],
  ["bb10", /BB10;\sTouch.*Version\/([0-9\.]+)/],
  ["android", /Android\s([0-9\.]+)/],
  ["ios", /Version\/([0-9\._]+).*Mobile.*Safari.*/],
  ["safari", /Version\/([0-9\._]+).*Safari/],
  ["facebook", /FBAV\/([0-9\.]+)/],
  ["instagram", /Instagram\s([0-9\.]+)/],
  ["ios-webview", /AppleWebKit\/([0-9\.]+).*Mobile/],
  ["ios-webview", /AppleWebKit\/([0-9\.]+).*Gecko\)$/],
  ["searchbot", SEARCHBOX_UA_REGEX]
];
var operatingSystemRules = [
  ["iOS", /iP(hone|od|ad)/],
  ["Android OS", /Android/],
  ["BlackBerry OS", /BlackBerry|BB10/],
  ["Windows Mobile", /IEMobile/],
  ["Amazon OS", /Kindle/],
  ["Windows 3.11", /Win16/],
  ["Windows 95", /(Windows 95)|(Win95)|(Windows_95)/],
  ["Windows 98", /(Windows 98)|(Win98)/],
  ["Windows 2000", /(Windows NT 5.0)|(Windows 2000)/],
  ["Windows XP", /(Windows NT 5.1)|(Windows XP)/],
  ["Windows Server 2003", /(Windows NT 5.2)/],
  ["Windows Vista", /(Windows NT 6.0)/],
  ["Windows 7", /(Windows NT 6.1)/],
  ["Windows 8", /(Windows NT 6.2)/],
  ["Windows 8.1", /(Windows NT 6.3)/],
  ["Windows 10", /(Windows NT 10.0)/],
  ["Windows ME", /Windows ME/],
  ["Open BSD", /OpenBSD/],
  ["Sun OS", /SunOS/],
  ["Chrome OS", /CrOS/],
  ["Linux", /(Linux)|(X11)/],
  ["Mac OS", /(Mac_PowerPC)|(Macintosh)/],
  ["QNX", /QNX/],
  ["BeOS", /BeOS/],
  ["OS/2", /OS\/2/]
];
function detect(userAgent) {
  if (!!userAgent) {
    return parseUserAgent(userAgent);
  }
  if (typeof document === "undefined" && typeof navigator !== "undefined" && navigator.product === "ReactNative") {
    return new ReactNativeInfo();
  }
  if (typeof navigator !== "undefined") {
    return parseUserAgent(navigator.userAgent);
  }
  return getNodeVersion();
}
function matchUserAgent(ua) {
  return ua !== "" && userAgentRules.reduce(function(matched, _a) {
    var browser = _a[0], regex = _a[1];
    if (matched) {
      return matched;
    }
    var uaMatch = regex.exec(ua);
    return !!uaMatch && [browser, uaMatch];
  }, false);
}
function parseUserAgent(ua) {
  var matchedRule = matchUserAgent(ua);
  if (!matchedRule) {
    return null;
  }
  var name = matchedRule[0], match = matchedRule[1];
  if (name === "searchbot") {
    return new BotInfo();
  }
  var versionParts = match[1] && match[1].split(/[._]/).slice(0, 3);
  if (versionParts) {
    if (versionParts.length < REQUIRED_VERSION_PARTS) {
      versionParts = __spreadArrays(versionParts, createVersionParts(REQUIRED_VERSION_PARTS - versionParts.length));
    }
  } else {
    versionParts = [];
  }
  var version = versionParts.join(".");
  var os = detectOS$1(ua);
  var searchBotMatch = SEARCHBOT_OS_REGEX.exec(ua);
  if (searchBotMatch && searchBotMatch[1]) {
    return new SearchBotDeviceInfo(name, version, os, searchBotMatch[1]);
  }
  return new BrowserInfo(name, version, os);
}
function detectOS$1(ua) {
  for (var ii = 0, count = operatingSystemRules.length; ii < count; ii++) {
    var _a = operatingSystemRules[ii], os = _a[0], regex = _a[1];
    var match = regex.exec(ua);
    if (match) {
      return os;
    }
  }
  return null;
}
function getNodeVersion() {
  var isNode2 = typeof browserExports !== "undefined" && browserExports.version;
  return isNode2 ? new NodeInfo(browserExports.version.slice(1)) : null;
}
function createVersionParts(count) {
  var output = [];
  for (var ii = 0; ii < count; ii++) {
    output.push("0");
  }
  return output;
}
function detectEnv(userAgent) {
  return detect(userAgent);
}
function detectOS() {
  const env = detectEnv();
  return env && env.os ? env.os : void 0;
}
function isAndroid() {
  const os = detectOS();
  return os ? os.toLowerCase().includes("android") : false;
}
function isIOS() {
  const os = detectOS();
  return os ? os.toLowerCase().includes("ios") || os.toLowerCase().includes("mac") && navigator.maxTouchPoints > 1 : false;
}
function isMobile() {
  const os = detectOS();
  return os ? isAndroid() || isIOS() : false;
}
function isNode() {
  const env = detectEnv();
  const result = env && env.name ? env.name.toLowerCase() === "node" : false;
  return result;
}
function isBrowser() {
  const result = !isNode() && !!getNavigator();
  return result;
}
const getFromWindow = getFromWindow_1;
const getFromWindowOrThrow = getFromWindowOrThrow_1;
const getDocumentOrThrow = getDocumentOrThrow_1;
const getDocument = getDocument_1;
const getNavigatorOrThrow = getNavigatorOrThrow_1;
const getNavigator = getNavigator_1;
const getLocationOrThrow = getLocationOrThrow_1;
const getLocation = getLocation_1;
const getCryptoOrThrow = getCryptoOrThrow_1;
const getCrypto = getCrypto_1;
const getLocalStorageOrThrow = getLocalStorageOrThrow_1;
const getLocalStorage = getLocalStorage_1;
function getClientMeta() {
  return getWindowMetadata_1();
}
function safeJsonParse$1(value) {
  if (typeof value !== "string") {
    throw new Error(`Cannot safe json parse value of type ${typeof value}`);
  }
  try {
    return JSON.parse(value);
  } catch (_a) {
    return value;
  }
}
function safeJsonStringify$1(value) {
  return typeof value === "string" ? value : JSON.stringify(value);
}
const safeJsonParse = safeJsonParse$1;
const safeJsonStringify = safeJsonStringify$1;
function setLocal(key, data) {
  const raw = safeJsonStringify(data);
  const local = getLocalStorage();
  if (local) {
    local.setItem(key, raw);
  }
}
function getLocal(key) {
  let data = null;
  let raw = null;
  const local = getLocalStorage();
  if (local) {
    raw = local.getItem(key);
  }
  data = raw ? safeJsonParse(raw) : raw;
  return data;
}
function removeLocal(key) {
  const local = getLocalStorage();
  if (local) {
    local.removeItem(key);
  }
}
const mobileLinkChoiceKey = "WALLETCONNECT_DEEPLINK_CHOICE";
function formatIOSMobile(uri, entry) {
  const encodedUri = encodeURIComponent(uri);
  return entry.universalLink ? `${entry.universalLink}/wc?uri=${encodedUri}` : entry.deepLink ? `${entry.deepLink}${entry.deepLink.endsWith(":") ? "//" : "/"}wc?uri=${encodedUri}` : "";
}
function saveMobileLinkInfo(data) {
  const focusUri = data.href.split("?")[0];
  setLocal(mobileLinkChoiceKey, Object.assign(Object.assign({}, data), { href: focusUri }));
}
function getMobileRegistryEntry(registry, name) {
  return registry.filter((entry) => entry.name.toLowerCase().includes(name.toLowerCase()))[0];
}
function getMobileLinkRegistry(registry, whitelist) {
  let links = registry;
  if (whitelist) {
    links = whitelist.map((name) => getMobileRegistryEntry(registry, name)).filter(Boolean);
  }
  return links;
}
export {
  isNode as A,
  safeJsonParse as B,
  safeJsonStringify as C,
  saveMobileLinkInfo as D,
  getLocal as a,
  getClientMeta as b,
  isMobile as c,
  detectEnv as d,
  detectOS as e,
  formatIOSMobile as f,
  getLocation as g,
  getCrypto as h,
  isBrowser as i,
  getCryptoOrThrow as j,
  getDocument as k,
  getDocumentOrThrow as l,
  mobileLinkChoiceKey as m,
  getFromWindow as n,
  getFromWindowOrThrow as o,
  getLocalStorage as p,
  getLocalStorageOrThrow as q,
  removeLocal as r,
  setLocal as s,
  getLocationOrThrow as t,
  getMobileLinkRegistry as u,
  getMobileRegistryEntry as v,
  getNavigator as w,
  getNavigatorOrThrow as x,
  isAndroid as y,
  isIOS as z
};
