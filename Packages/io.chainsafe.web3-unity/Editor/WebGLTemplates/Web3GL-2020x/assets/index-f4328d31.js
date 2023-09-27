import{U as Lt,n as Wn,V as ke}from"./index-b28afd98.js";import{d as Yn,e as Vn,f as Kn,b as Qn,h as Jn,j as jn,k as Gn,l as Zn,n as Xn,o as er,a as tr,p as nr,q as rr,g as or,t as ar,u as ir,v as lr,w as cr,x as ur,y as sr,i as fr,z as dr,c as _r,A as hr,m as gr,r as pr,B as mr,C as vr,D as wr,s as yr}from"./mobile-122d5339.js";function br(e,t){for(var n=0;n<t.length;n++){const r=t[n];if(typeof r!="string"&&!Array.isArray(r)){for(const o in r)if(o!=="default"&&!(o in e)){const a=Object.getOwnPropertyDescriptor(r,o);a&&Object.defineProperty(e,o,a.get?a:{enumerable:!0,get:()=>r[o]})}}}return Object.freeze(Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}))}const Mt="https://registry.walletconnect.com";function Cr(){return Mt+"/api/v2/wallets"}function Er(){return Mt+"/api/v2/dapps"}function It(e,t="mobile"){var n;return{name:e.name||"",shortName:e.metadata.shortName||"",color:e.metadata.colors.primary||"",logo:(n=e.image_url.sm)!==null&&n!==void 0?n:"",universalLink:e[t].universal||"",deepLink:e[t].native||""}}function xr(e,t="mobile"){return Object.values(e).filter(n=>!!n[t].universal||!!n[t].native).map(n=>It(n,t))}const kr=Object.freeze(Object.defineProperty({__proto__:null,detectEnv:Yn,detectOS:Vn,formatIOSMobile:Kn,formatMobileRegistry:xr,formatMobileRegistryEntry:It,getClientMeta:Qn,getCrypto:Jn,getCryptoOrThrow:jn,getDappRegistryUrl:Er,getDocument:Gn,getDocumentOrThrow:Zn,getFromWindow:Xn,getFromWindowOrThrow:er,getLocal:tr,getLocalStorage:nr,getLocalStorageOrThrow:rr,getLocation:or,getLocationOrThrow:ar,getMobileLinkRegistry:ir,getMobileRegistryEntry:lr,getNavigator:cr,getNavigatorOrThrow:ur,getWalletRegistryUrl:Cr,isAndroid:sr,isBrowser:fr,isIOS:dr,isMobile:_r,isNode:hr,mobileLinkChoiceKey:gr,removeLocal:pr,safeJsonParse:mr,safeJsonStringify:vr,saveMobileLinkInfo:wr,setLocal:yr},Symbol.toStringTag,{value:"Module"})),Tr=Lt(kr);var le={},Ar=function(){return typeof Promise=="function"&&Promise.prototype&&Promise.prototype.then},Dt={},H={},Nr={}.toString,We=Array.isArray||function(e){return Nr.call(e)=="[object Array]"},Sr=We;function Br(){try{var e=new Uint8Array(1);return e.__proto__={__proto__:Uint8Array.prototype,foo:function(){return 42}},e.foo()===42}catch{return!1}}E.TYPED_ARRAY_SUPPORT=Br();var lt=E.TYPED_ARRAY_SUPPORT?2147483647:1073741823;function E(e,t,n){return!E.TYPED_ARRAY_SUPPORT&&!(this instanceof E)?new E(e,t,n):typeof e=="number"?Ut(this,e):Ur(this,e,t,n)}E.TYPED_ARRAY_SUPPORT&&(E.prototype.__proto__=Uint8Array.prototype,E.__proto__=Uint8Array,typeof Symbol<"u"&&Symbol.species&&E[Symbol.species]===E&&Object.defineProperty(E,Symbol.species,{value:null,configurable:!0,enumerable:!1,writable:!1}));function Ye(e){if(e>=lt)throw new RangeError("Attempt to allocate Buffer larger than maximum size: 0x"+lt.toString(16)+" bytes");return e|0}function Rr(e){return e!==e}function K(e,t){var n;return E.TYPED_ARRAY_SUPPORT?(n=new Uint8Array(t),n.__proto__=E.prototype):(n=e,n===null&&(n=new E(t)),n.length=t),n}function Ut(e,t){var n=K(e,t<0?0:Ye(t)|0);if(!E.TYPED_ARRAY_SUPPORT)for(var r=0;r<t;++r)n[r]=0;return n}function Pr(e,t){var n=Ft(t)|0,r=K(e,n),o=r.write(t);return o!==n&&(r=r.slice(0,o)),r}function Le(e,t){for(var n=t.length<0?0:Ye(t.length)|0,r=K(e,n),o=0;o<n;o+=1)r[o]=t[o]&255;return r}function Lr(e,t,n,r){if(n<0||t.byteLength<n)throw new RangeError("'offset' is out of bounds");if(t.byteLength<n+(r||0))throw new RangeError("'length' is out of bounds");var o;return n===void 0&&r===void 0?o=new Uint8Array(t):r===void 0?o=new Uint8Array(t,n):o=new Uint8Array(t,n,r),E.TYPED_ARRAY_SUPPORT?o.__proto__=E.prototype:o=Le(e,o),o}function Mr(e,t){if(E.isBuffer(t)){var n=Ye(t.length)|0,r=K(e,n);return r.length===0||t.copy(r,0,0,n),r}if(t){if(typeof ArrayBuffer<"u"&&t.buffer instanceof ArrayBuffer||"length"in t)return typeof t.length!="number"||Rr(t.length)?K(e,0):Le(e,t);if(t.type==="Buffer"&&Array.isArray(t.data))return Le(e,t.data)}throw new TypeError("First argument must be a string, Buffer, ArrayBuffer, Array, or array-like object.")}function Ot(e,t){t=t||1/0;for(var n,r=e.length,o=null,a=[],i=0;i<r;++i){if(n=e.charCodeAt(i),n>55295&&n<57344){if(!o){if(n>56319){(t-=3)>-1&&a.push(239,191,189);continue}else if(i+1===r){(t-=3)>-1&&a.push(239,191,189);continue}o=n;continue}if(n<56320){(t-=3)>-1&&a.push(239,191,189),o=n;continue}n=(o-55296<<10|n-56320)+65536}else o&&(t-=3)>-1&&a.push(239,191,189);if(o=null,n<128){if((t-=1)<0)break;a.push(n)}else if(n<2048){if((t-=2)<0)break;a.push(n>>6|192,n&63|128)}else if(n<65536){if((t-=3)<0)break;a.push(n>>12|224,n>>6&63|128,n&63|128)}else if(n<1114112){if((t-=4)<0)break;a.push(n>>18|240,n>>12&63|128,n>>6&63|128,n&63|128)}else throw new Error("Invalid code point")}return a}function Ft(e){if(E.isBuffer(e))return e.length;if(typeof ArrayBuffer<"u"&&typeof ArrayBuffer.isView=="function"&&(ArrayBuffer.isView(e)||e instanceof ArrayBuffer))return e.byteLength;typeof e!="string"&&(e=""+e);var t=e.length;return t===0?0:Ot(e).length}function Ir(e,t,n,r){for(var o=0;o<r&&!(o+n>=t.length||o>=e.length);++o)t[o+n]=e[o];return o}function Dr(e,t,n,r){return Ir(Ot(t,e.length-n),e,n,r)}function Ur(e,t,n,r){if(typeof t=="number")throw new TypeError('"value" argument must not be a number');return typeof ArrayBuffer<"u"&&t instanceof ArrayBuffer?Lr(e,t,n,r):typeof t=="string"?Pr(e,t):Mr(e,t)}E.prototype.write=function(t,n,r){n===void 0?(r=this.length,n=0):r===void 0&&typeof n=="string"?(r=this.length,n=0):isFinite(n)&&(n=n|0,isFinite(r)?r=r|0:r=void 0);var o=this.length-n;if((r===void 0||r>o)&&(r=o),t.length>0&&(r<0||n<0)||n>this.length)throw new RangeError("Attempt to write outside buffer bounds");return Dr(this,t,n,r)};E.prototype.slice=function(t,n){var r=this.length;t=~~t,n=n===void 0?r:~~n,t<0?(t+=r,t<0&&(t=0)):t>r&&(t=r),n<0?(n+=r,n<0&&(n=0)):n>r&&(n=r),n<t&&(n=t);var o;if(E.TYPED_ARRAY_SUPPORT)o=this.subarray(t,n),o.__proto__=E.prototype;else{var a=n-t;o=new E(a,void 0);for(var i=0;i<a;++i)o[i]=this[i+t]}return o};E.prototype.copy=function(t,n,r,o){if(r||(r=0),!o&&o!==0&&(o=this.length),n>=t.length&&(n=t.length),n||(n=0),o>0&&o<r&&(o=r),o===r||t.length===0||this.length===0)return 0;if(n<0)throw new RangeError("targetStart out of bounds");if(r<0||r>=this.length)throw new RangeError("sourceStart out of bounds");if(o<0)throw new RangeError("sourceEnd out of bounds");o>this.length&&(o=this.length),t.length-n<o-r&&(o=t.length-n+r);var a=o-r,i;if(this===t&&r<n&&n<o)for(i=a-1;i>=0;--i)t[i+n]=this[i+r];else if(a<1e3||!E.TYPED_ARRAY_SUPPORT)for(i=0;i<a;++i)t[i+n]=this[i+r];else Uint8Array.prototype.set.call(t,this.subarray(r,r+a),n);return a};E.prototype.fill=function(t,n,r){if(typeof t=="string"){if(typeof n=="string"?(n=0,r=this.length):typeof r=="string"&&(r=this.length),t.length===1){var o=t.charCodeAt(0);o<256&&(t=o)}}else typeof t=="number"&&(t=t&255);if(n<0||this.length<n||this.length<r)throw new RangeError("Out of range index");if(r<=n)return this;n=n>>>0,r=r===void 0?this.length:r>>>0,t||(t=0);var a;if(typeof t=="number")for(a=n;a<r;++a)this[a]=t;else{var i=E.isBuffer(t)?t:new E(t),l=i.length;for(a=0;a<r-n;++a)this[a+n]=i[a%l]}return this};E.concat=function(t,n){if(!Sr(t))throw new TypeError('"list" argument must be an Array of Buffers');if(t.length===0)return K(null,0);var r;if(n===void 0)for(n=0,r=0;r<t.length;++r)n+=t[r].length;var o=Ut(null,n),a=0;for(r=0;r<t.length;++r){var i=t[r];if(!E.isBuffer(i))throw new TypeError('"list" argument must be an Array of Buffers');i.copy(o,a),a+=i.length}return o};E.byteLength=Ft;E.prototype._isBuffer=!0;E.isBuffer=function(t){return!!(t!=null&&t._isBuffer)};H.alloc=function(e){var t=new E(e);return t.fill(0),t};H.from=function(e){return new E(e)};var L={},Ve,Or=[0,26,44,70,100,134,172,196,242,292,346,404,466,532,581,655,733,815,901,991,1085,1156,1258,1364,1474,1588,1706,1828,1921,2051,2185,2323,2465,2611,2761,2876,3034,3196,3362,3532,3706];L.getSymbolSize=function(t){if(!t)throw new Error('"version" cannot be null or undefined');if(t<1||t>40)throw new Error('"version" should be in range from 1 to 40');return t*4+17};L.getSymbolTotalCodewords=function(t){return Or[t]};L.getBCHDigit=function(e){for(var t=0;e!==0;)t++,e>>>=1;return t};L.setToSJISFunction=function(t){if(typeof t!="function")throw new Error('"toSJISFunc" is not a valid function.');Ve=t};L.isKanjiModeEnabled=function(){return typeof Ve<"u"};L.toSJIS=function(t){return Ve(t)};var we={};(function(e){e.L={bit:1},e.M={bit:0},e.Q={bit:3},e.H={bit:2};function t(n){if(typeof n!="string")throw new Error("Param is not a string");var r=n.toLowerCase();switch(r){case"l":case"low":return e.L;case"m":case"medium":return e.M;case"q":case"quartile":return e.Q;case"h":case"high":return e.H;default:throw new Error("Unknown EC Level: "+n)}}e.isValid=function(r){return r&&typeof r.bit<"u"&&r.bit>=0&&r.bit<4},e.from=function(r,o){if(e.isValid(r))return r;try{return t(r)}catch{return o}}})(we);function $t(){this.buffer=[],this.length=0}$t.prototype={get:function(e){var t=Math.floor(e/8);return(this.buffer[t]>>>7-e%8&1)===1},put:function(e,t){for(var n=0;n<t;n++)this.putBit((e>>>t-n-1&1)===1)},getLengthInBits:function(){return this.length},putBit:function(e){var t=Math.floor(this.length/8);this.buffer.length<=t&&this.buffer.push(0),e&&(this.buffer[t]|=128>>>this.length%8),this.length++}};var Fr=$t,ct=H;function ce(e){if(!e||e<1)throw new Error("BitMatrix size must be defined and greater than 0");this.size=e,this.data=ct.alloc(e*e),this.reservedBit=ct.alloc(e*e)}ce.prototype.set=function(e,t,n,r){var o=e*this.size+t;this.data[o]=n,r&&(this.reservedBit[o]=!0)};ce.prototype.get=function(e,t){return this.data[e*this.size+t]};ce.prototype.xor=function(e,t,n){this.data[e*this.size+t]^=n};ce.prototype.isReserved=function(e,t){return this.reservedBit[e*this.size+t]};var $r=ce,zt={};(function(e){var t=L.getSymbolSize;e.getRowColCoords=function(r){if(r===1)return[];for(var o=Math.floor(r/7)+2,a=t(r),i=a===145?26:Math.ceil((a-13)/(2*o-2))*2,l=[a-7],u=1;u<o-1;u++)l[u]=l[u-1]-i;return l.push(6),l.reverse()},e.getPositions=function(r){for(var o=[],a=e.getRowColCoords(r),i=a.length,l=0;l<i;l++)for(var u=0;u<i;u++)l===0&&u===0||l===0&&u===i-1||l===i-1&&u===0||o.push([a[l],a[u]]);return o}})(zt);var Ht={},zr=L.getSymbolSize,ut=7;Ht.getPositions=function(t){var n=zr(t);return[[0,0],[n-ut,0],[0,n-ut]]};var qt={};(function(e){e.Patterns={PATTERN000:0,PATTERN001:1,PATTERN010:2,PATTERN011:3,PATTERN100:4,PATTERN101:5,PATTERN110:6,PATTERN111:7};var t={N1:3,N2:3,N3:40,N4:10};e.isValid=function(o){return o!=null&&o!==""&&!isNaN(o)&&o>=0&&o<=7},e.from=function(o){return e.isValid(o)?parseInt(o,10):void 0},e.getPenaltyN1=function(o){for(var a=o.size,i=0,l=0,u=0,s=null,c=null,f=0;f<a;f++){l=u=0,s=c=null;for(var m=0;m<a;m++){var b=o.get(f,m);b===s?l++:(l>=5&&(i+=t.N1+(l-5)),s=b,l=1),b=o.get(m,f),b===c?u++:(u>=5&&(i+=t.N1+(u-5)),c=b,u=1)}l>=5&&(i+=t.N1+(l-5)),u>=5&&(i+=t.N1+(u-5))}return i},e.getPenaltyN2=function(o){for(var a=o.size,i=0,l=0;l<a-1;l++)for(var u=0;u<a-1;u++){var s=o.get(l,u)+o.get(l,u+1)+o.get(l+1,u)+o.get(l+1,u+1);(s===4||s===0)&&i++}return i*t.N2},e.getPenaltyN3=function(o){for(var a=o.size,i=0,l=0,u=0,s=0;s<a;s++){l=u=0;for(var c=0;c<a;c++)l=l<<1&2047|o.get(s,c),c>=10&&(l===1488||l===93)&&i++,u=u<<1&2047|o.get(c,s),c>=10&&(u===1488||u===93)&&i++}return i*t.N3},e.getPenaltyN4=function(o){for(var a=0,i=o.data.length,l=0;l<i;l++)a+=o.data[l];var u=Math.abs(Math.ceil(a*100/i/5)-10);return u*t.N4};function n(r,o,a){switch(r){case e.Patterns.PATTERN000:return(o+a)%2===0;case e.Patterns.PATTERN001:return o%2===0;case e.Patterns.PATTERN010:return a%3===0;case e.Patterns.PATTERN011:return(o+a)%3===0;case e.Patterns.PATTERN100:return(Math.floor(o/2)+Math.floor(a/3))%2===0;case e.Patterns.PATTERN101:return o*a%2+o*a%3===0;case e.Patterns.PATTERN110:return(o*a%2+o*a%3)%2===0;case e.Patterns.PATTERN111:return(o*a%3+(o+a)%2)%2===0;default:throw new Error("bad maskPattern:"+r)}}e.applyMask=function(o,a){for(var i=a.size,l=0;l<i;l++)for(var u=0;u<i;u++)a.isReserved(u,l)||a.xor(u,l,n(o,u,l))},e.getBestMask=function(o,a){for(var i=Object.keys(e.Patterns).length,l=0,u=1/0,s=0;s<i;s++){a(s),e.applyMask(s,o);var c=e.getPenaltyN1(o)+e.getPenaltyN2(o)+e.getPenaltyN3(o)+e.getPenaltyN4(o);e.applyMask(s,o),c<u&&(u=c,l=s)}return l}})(qt);var ye={},$=we,fe=[1,1,1,1,1,1,1,1,1,1,2,2,1,2,2,4,1,2,4,4,2,4,4,4,2,4,6,5,2,4,6,6,2,5,8,8,4,5,8,8,4,5,8,11,4,8,10,11,4,9,12,16,4,9,16,16,6,10,12,18,6,10,17,16,6,11,16,19,6,13,18,21,7,14,21,25,8,16,20,25,8,17,23,25,9,17,23,34,9,18,25,30,10,20,27,32,12,21,29,35,12,23,34,37,12,25,34,40,13,26,35,42,14,28,38,45,15,29,40,48,16,31,43,51,17,33,45,54,18,35,48,57,19,37,51,60,19,38,53,63,20,40,56,66,21,43,59,70,22,45,62,74,24,47,65,77,25,49,68,81],de=[7,10,13,17,10,16,22,28,15,26,36,44,20,36,52,64,26,48,72,88,36,64,96,112,40,72,108,130,48,88,132,156,60,110,160,192,72,130,192,224,80,150,224,264,96,176,260,308,104,198,288,352,120,216,320,384,132,240,360,432,144,280,408,480,168,308,448,532,180,338,504,588,196,364,546,650,224,416,600,700,224,442,644,750,252,476,690,816,270,504,750,900,300,560,810,960,312,588,870,1050,336,644,952,1110,360,700,1020,1200,390,728,1050,1260,420,784,1140,1350,450,812,1200,1440,480,868,1290,1530,510,924,1350,1620,540,980,1440,1710,570,1036,1530,1800,570,1064,1590,1890,600,1120,1680,1980,630,1204,1770,2100,660,1260,1860,2220,720,1316,1950,2310,750,1372,2040,2430];ye.getBlocksCount=function(t,n){switch(n){case $.L:return fe[(t-1)*4+0];case $.M:return fe[(t-1)*4+1];case $.Q:return fe[(t-1)*4+2];case $.H:return fe[(t-1)*4+3];default:return}};ye.getTotalCodewordsCount=function(t,n){switch(n){case $.L:return de[(t-1)*4+0];case $.M:return de[(t-1)*4+1];case $.Q:return de[(t-1)*4+2];case $.H:return de[(t-1)*4+3];default:return}};var Wt={},be={},Yt=H,te=Yt.alloc(512),he=Yt.alloc(256);(function(){for(var t=1,n=0;n<255;n++)te[n]=t,he[t]=n,t<<=1,t&256&&(t^=285);for(n=255;n<512;n++)te[n]=te[n-255]})();be.log=function(t){if(t<1)throw new Error("log("+t+")");return he[t]};be.exp=function(t){return te[t]};be.mul=function(t,n){return t===0||n===0?0:te[he[t]+he[n]]};(function(e){var t=H,n=be;e.mul=function(o,a){for(var i=t.alloc(o.length+a.length-1),l=0;l<o.length;l++)for(var u=0;u<a.length;u++)i[l+u]^=n.mul(o[l],a[u]);return i},e.mod=function(o,a){for(var i=t.from(o);i.length-a.length>=0;){for(var l=i[0],u=0;u<a.length;u++)i[u]^=n.mul(a[u],l);for(var s=0;s<i.length&&i[s]===0;)s++;i=i.slice(s)}return i},e.generateECPolynomial=function(o){for(var a=t.from([1]),i=0;i<o;i++)a=e.mul(a,[1,n.exp(i)]);return a}})(Wt);var st=H,Vt=Wt,Hr=Wn.Buffer;function Ke(e){this.genPoly=void 0,this.degree=e,this.degree&&this.initialize(this.degree)}Ke.prototype.initialize=function(t){this.degree=t,this.genPoly=Vt.generateECPolynomial(this.degree)};Ke.prototype.encode=function(t){if(!this.genPoly)throw new Error("Encoder not initialized");var n=st.alloc(this.degree),r=Hr.concat([t,n],t.length+this.degree),o=Vt.mod(r,this.genPoly),a=this.degree-o.length;if(a>0){var i=st.alloc(this.degree);return o.copy(i,a),i}return o};var qr=Ke,Kt={},q={},Qe={};Qe.isValid=function(t){return!isNaN(t)&&t>=1&&t<=40};var I={},Qt="[0-9]+",Wr="[A-Z $%*+\\-./:]+",oe="(?:[u3000-u303F]|[u3040-u309F]|[u30A0-u30FF]|[uFF00-uFFEF]|[u4E00-u9FAF]|[u2605-u2606]|[u2190-u2195]|u203B|[u2010u2015u2018u2019u2025u2026u201Cu201Du2225u2260]|[u0391-u0451]|[u00A7u00A8u00B1u00B4u00D7u00F7])+";oe=oe.replace(/u/g,"\\u");var Yr="(?:(?![A-Z0-9 $%*+\\-./:]|"+oe+`)(?:.|[\r
]))+`;I.KANJI=new RegExp(oe,"g");I.BYTE_KANJI=new RegExp("[^A-Z0-9 $%*+\\-./:]+","g");I.BYTE=new RegExp(Yr,"g");I.NUMERIC=new RegExp(Qt,"g");I.ALPHANUMERIC=new RegExp(Wr,"g");var Vr=new RegExp("^"+oe+"$"),Kr=new RegExp("^"+Qt+"$"),Qr=new RegExp("^[A-Z0-9 $%*+\\-./:]+$");I.testKanji=function(t){return Vr.test(t)};I.testNumeric=function(t){return Kr.test(t)};I.testAlphanumeric=function(t){return Qr.test(t)};(function(e){var t=Qe,n=I;e.NUMERIC={id:"Numeric",bit:1<<0,ccBits:[10,12,14]},e.ALPHANUMERIC={id:"Alphanumeric",bit:1<<1,ccBits:[9,11,13]},e.BYTE={id:"Byte",bit:1<<2,ccBits:[8,16,16]},e.KANJI={id:"Kanji",bit:1<<3,ccBits:[8,10,12]},e.MIXED={bit:-1},e.getCharCountIndicator=function(a,i){if(!a.ccBits)throw new Error("Invalid mode: "+a);if(!t.isValid(i))throw new Error("Invalid version: "+i);return i>=1&&i<10?a.ccBits[0]:i<27?a.ccBits[1]:a.ccBits[2]},e.getBestModeForData=function(a){return n.testNumeric(a)?e.NUMERIC:n.testAlphanumeric(a)?e.ALPHANUMERIC:n.testKanji(a)?e.KANJI:e.BYTE},e.toString=function(a){if(a&&a.id)return a.id;throw new Error("Invalid mode")},e.isValid=function(a){return a&&a.bit&&a.ccBits};function r(o){if(typeof o!="string")throw new Error("Param is not a string");var a=o.toLowerCase();switch(a){case"numeric":return e.NUMERIC;case"alphanumeric":return e.ALPHANUMERIC;case"kanji":return e.KANJI;case"byte":return e.BYTE;default:throw new Error("Unknown mode: "+o)}}e.from=function(a,i){if(e.isValid(a))return a;try{return r(a)}catch{return i}}})(q);(function(e){var t=L,n=ye,r=we,o=q,a=Qe,i=We,l=1<<12|1<<11|1<<10|1<<9|1<<8|1<<5|1<<2|1<<0,u=t.getBCHDigit(l);function s(b,h,y){for(var w=1;w<=40;w++)if(h<=e.getCapacity(w,y,b))return w}function c(b,h){return o.getCharCountIndicator(b,h)+4}function f(b,h){var y=0;return b.forEach(function(w){var v=c(w.mode,h);y+=v+w.getBitsLength()}),y}function m(b,h){for(var y=1;y<=40;y++){var w=f(b,y);if(w<=e.getCapacity(y,h,o.MIXED))return y}}e.from=function(h,y){return a.isValid(h)?parseInt(h,10):y},e.getCapacity=function(h,y,w){if(!a.isValid(h))throw new Error("Invalid QR Code version");typeof w>"u"&&(w=o.BYTE);var v=t.getSymbolTotalCodewords(h),g=n.getTotalCodewordsCount(h,y),p=(v-g)*8;if(w===o.MIXED)return p;var _=p-c(w,h);switch(w){case o.NUMERIC:return Math.floor(_/10*3);case o.ALPHANUMERIC:return Math.floor(_/11*2);case o.KANJI:return Math.floor(_/13);case o.BYTE:default:return Math.floor(_/8)}},e.getBestVersionForData=function(h,y){var w,v=r.from(y,r.M);if(i(h)){if(h.length>1)return m(h,v);if(h.length===0)return 1;w=h[0]}else w=h;return s(w.mode,w.getLength(),v)},e.getEncodedBits=function(h){if(!a.isValid(h)||h<7)throw new Error("Invalid QR Code version");for(var y=h<<12;t.getBCHDigit(y)-u>=0;)y^=l<<t.getBCHDigit(y)-u;return h<<12|y}})(Kt);var Jt={},Me=L,jt=1<<10|1<<8|1<<5|1<<4|1<<2|1<<1|1<<0,Jr=1<<14|1<<12|1<<10|1<<4|1<<1,ft=Me.getBCHDigit(jt);Jt.getEncodedBits=function(t,n){for(var r=t.bit<<3|n,o=r<<10;Me.getBCHDigit(o)-ft>=0;)o^=jt<<Me.getBCHDigit(o)-ft;return(r<<10|o)^Jr};var Gt={},jr=q;function Q(e){this.mode=jr.NUMERIC,this.data=e.toString()}Q.getBitsLength=function(t){return 10*Math.floor(t/3)+(t%3?t%3*3+1:0)};Q.prototype.getLength=function(){return this.data.length};Q.prototype.getBitsLength=function(){return Q.getBitsLength(this.data.length)};Q.prototype.write=function(t){var n,r,o;for(n=0;n+3<=this.data.length;n+=3)r=this.data.substr(n,3),o=parseInt(r,10),t.put(o,10);var a=this.data.length-n;a>0&&(r=this.data.substr(n),o=parseInt(r,10),t.put(o,a*3+1))};var Gr=Q,Zr=q,Te=["0","1","2","3","4","5","6","7","8","9","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"," ","$","%","*","+","-",".","/",":"];function J(e){this.mode=Zr.ALPHANUMERIC,this.data=e}J.getBitsLength=function(t){return 11*Math.floor(t/2)+6*(t%2)};J.prototype.getLength=function(){return this.data.length};J.prototype.getBitsLength=function(){return J.getBitsLength(this.data.length)};J.prototype.write=function(t){var n;for(n=0;n+2<=this.data.length;n+=2){var r=Te.indexOf(this.data[n])*45;r+=Te.indexOf(this.data[n+1]),t.put(r,11)}this.data.length%2&&t.put(Te.indexOf(this.data[n]),6)};var Xr=J,eo=H,to=q;function j(e){this.mode=to.BYTE,this.data=eo.from(e)}j.getBitsLength=function(t){return t*8};j.prototype.getLength=function(){return this.data.length};j.prototype.getBitsLength=function(){return j.getBitsLength(this.data.length)};j.prototype.write=function(e){for(var t=0,n=this.data.length;t<n;t++)e.put(this.data[t],8)};var no=j,ro=q,oo=L;function G(e){this.mode=ro.KANJI,this.data=e}G.getBitsLength=function(t){return t*13};G.prototype.getLength=function(){return this.data.length};G.prototype.getBitsLength=function(){return G.getBitsLength(this.data.length)};G.prototype.write=function(e){var t;for(t=0;t<this.data.length;t++){var n=oo.toSJIS(this.data[t]);if(n>=33088&&n<=40956)n-=33088;else if(n>=57408&&n<=60351)n-=49472;else throw new Error("Invalid SJIS character: "+this.data[t]+`
Make sure your charset is UTF-8`);n=(n>>>8&255)*192+(n&255),e.put(n,13)}};var ao=G,Ie={},io={get exports(){return Ie},set exports(e){Ie=e}};(function(e){var t={single_source_shortest_paths:function(n,r,o){var a={},i={};i[r]=0;var l=t.PriorityQueue.make();l.push(r,0);for(var u,s,c,f,m,b,h,y,w;!l.empty();){u=l.pop(),s=u.value,f=u.cost,m=n[s]||{};for(c in m)m.hasOwnProperty(c)&&(b=m[c],h=f+b,y=i[c],w=typeof i[c]>"u",(w||y>h)&&(i[c]=h,l.push(c,h),a[c]=s))}if(typeof o<"u"&&typeof i[o]>"u"){var v=["Could not find a path from ",r," to ",o,"."].join("");throw new Error(v)}return a},extract_shortest_path_from_predecessor_list:function(n,r){for(var o=[],a=r;a;)o.push(a),n[a],a=n[a];return o.reverse(),o},find_path:function(n,r,o){var a=t.single_source_shortest_paths(n,r,o);return t.extract_shortest_path_from_predecessor_list(a,o)},PriorityQueue:{make:function(n){var r=t.PriorityQueue,o={},a;n=n||{};for(a in r)r.hasOwnProperty(a)&&(o[a]=r[a]);return o.queue=[],o.sorter=n.sorter||r.default_sorter,o},default_sorter:function(n,r){return n.cost-r.cost},push:function(n,r){var o={value:n,cost:r};this.queue.push(o),this.queue.sort(this.sorter)},pop:function(){return this.queue.shift()},empty:function(){return this.queue.length===0}}};e.exports=t})(io);(function(e){var t=q,n=Gr,r=Xr,o=no,a=ao,i=I,l=L,u=Ie;function s(v){return unescape(encodeURIComponent(v)).length}function c(v,g,p){for(var _=[],x;(x=v.exec(p))!==null;)_.push({data:x[0],index:x.index,mode:g,length:x[0].length});return _}function f(v){var g=c(i.NUMERIC,t.NUMERIC,v),p=c(i.ALPHANUMERIC,t.ALPHANUMERIC,v),_,x;l.isKanjiModeEnabled()?(_=c(i.BYTE,t.BYTE,v),x=c(i.KANJI,t.KANJI,v)):(_=c(i.BYTE_KANJI,t.BYTE,v),x=[]);var T=g.concat(p,_,x);return T.sort(function(k,A){return k.index-A.index}).map(function(k){return{data:k.data,mode:k.mode,length:k.length}})}function m(v,g){switch(g){case t.NUMERIC:return n.getBitsLength(v);case t.ALPHANUMERIC:return r.getBitsLength(v);case t.KANJI:return a.getBitsLength(v);case t.BYTE:return o.getBitsLength(v)}}function b(v){return v.reduce(function(g,p){var _=g.length-1>=0?g[g.length-1]:null;return _&&_.mode===p.mode?(g[g.length-1].data+=p.data,g):(g.push(p),g)},[])}function h(v){for(var g=[],p=0;p<v.length;p++){var _=v[p];switch(_.mode){case t.NUMERIC:g.push([_,{data:_.data,mode:t.ALPHANUMERIC,length:_.length},{data:_.data,mode:t.BYTE,length:_.length}]);break;case t.ALPHANUMERIC:g.push([_,{data:_.data,mode:t.BYTE,length:_.length}]);break;case t.KANJI:g.push([_,{data:_.data,mode:t.BYTE,length:s(_.data)}]);break;case t.BYTE:g.push([{data:_.data,mode:t.BYTE,length:s(_.data)}])}}return g}function y(v,g){for(var p={},_={start:{}},x=["start"],T=0;T<v.length;T++){for(var k=v[T],A=[],S=0;S<k.length;S++){var N=k[S],W=""+T+S;A.push(W),p[W]={node:N,lastCount:0},_[W]={};for(var D=0;D<x.length;D++){var P=x[D];p[P]&&p[P].node.mode===N.mode?(_[P][W]=m(p[P].lastCount+N.length,N.mode)-m(p[P].lastCount,N.mode),p[P].lastCount+=N.length):(p[P]&&(p[P].lastCount=N.length),_[P][W]=m(N.length,N.mode)+4+t.getCharCountIndicator(N.mode,g))}}x=A}for(D=0;D<x.length;D++)_[x[D]].end=0;return{map:_,table:p}}function w(v,g){var p,_=t.getBestModeForData(v);if(p=t.from(g,_),p!==t.BYTE&&p.bit<_.bit)throw new Error('"'+v+'" cannot be encoded with mode '+t.toString(p)+`.
 Suggested mode is: `+t.toString(_));switch(p===t.KANJI&&!l.isKanjiModeEnabled()&&(p=t.BYTE),p){case t.NUMERIC:return new n(v);case t.ALPHANUMERIC:return new r(v);case t.KANJI:return new a(v);case t.BYTE:return new o(v)}}e.fromArray=function(g){return g.reduce(function(p,_){return typeof _=="string"?p.push(w(_,null)):_.data&&p.push(w(_.data,_.mode)),p},[])},e.fromString=function(g,p){for(var _=f(g,l.isKanjiModeEnabled()),x=h(_),T=y(x,p),k=u.find_path(T.map,"start","end"),A=[],S=1;S<k.length-1;S++)A.push(T.table[k[S]].node);return e.fromArray(b(A))},e.rawSplit=function(g){return e.fromArray(f(g,l.isKanjiModeEnabled()))}})(Gt);var dt=H,Ce=L,Ae=we,lo=Fr,co=$r,uo=zt,so=Ht,De=qt,Ue=ye,fo=qr,ge=Kt,_o=Jt,ho=q,Ne=Gt,go=We;function po(e,t){for(var n=e.size,r=so.getPositions(t),o=0;o<r.length;o++)for(var a=r[o][0],i=r[o][1],l=-1;l<=7;l++)if(!(a+l<=-1||n<=a+l))for(var u=-1;u<=7;u++)i+u<=-1||n<=i+u||(l>=0&&l<=6&&(u===0||u===6)||u>=0&&u<=6&&(l===0||l===6)||l>=2&&l<=4&&u>=2&&u<=4?e.set(a+l,i+u,!0,!0):e.set(a+l,i+u,!1,!0))}function mo(e){for(var t=e.size,n=8;n<t-8;n++){var r=n%2===0;e.set(n,6,r,!0),e.set(6,n,r,!0)}}function vo(e,t){for(var n=uo.getPositions(t),r=0;r<n.length;r++)for(var o=n[r][0],a=n[r][1],i=-2;i<=2;i++)for(var l=-2;l<=2;l++)i===-2||i===2||l===-2||l===2||i===0&&l===0?e.set(o+i,a+l,!0,!0):e.set(o+i,a+l,!1,!0)}function wo(e,t){for(var n=e.size,r=ge.getEncodedBits(t),o,a,i,l=0;l<18;l++)o=Math.floor(l/3),a=l%3+n-8-3,i=(r>>l&1)===1,e.set(o,a,i,!0),e.set(a,o,i,!0)}function Se(e,t,n){var r=e.size,o=_o.getEncodedBits(t,n),a,i;for(a=0;a<15;a++)i=(o>>a&1)===1,a<6?e.set(a,8,i,!0):a<8?e.set(a+1,8,i,!0):e.set(r-15+a,8,i,!0),a<8?e.set(8,r-a-1,i,!0):a<9?e.set(8,15-a-1+1,i,!0):e.set(8,15-a-1,i,!0);e.set(r-8,8,1,!0)}function yo(e,t){for(var n=e.size,r=-1,o=n-1,a=7,i=0,l=n-1;l>0;l-=2)for(l===6&&l--;;){for(var u=0;u<2;u++)if(!e.isReserved(o,l-u)){var s=!1;i<t.length&&(s=(t[i]>>>a&1)===1),e.set(o,l-u,s),a--,a===-1&&(i++,a=7)}if(o+=r,o<0||n<=o){o-=r,r=-r;break}}}function bo(e,t,n){var r=new lo;n.forEach(function(s){r.put(s.mode.bit,4),r.put(s.getLength(),ho.getCharCountIndicator(s.mode,e)),s.write(r)});var o=Ce.getSymbolTotalCodewords(e),a=Ue.getTotalCodewordsCount(e,t),i=(o-a)*8;for(r.getLengthInBits()+4<=i&&r.put(0,4);r.getLengthInBits()%8!==0;)r.putBit(0);for(var l=(i-r.getLengthInBits())/8,u=0;u<l;u++)r.put(u%2?17:236,8);return Co(r,e,t)}function Co(e,t,n){for(var r=Ce.getSymbolTotalCodewords(t),o=Ue.getTotalCodewordsCount(t,n),a=r-o,i=Ue.getBlocksCount(t,n),l=r%i,u=i-l,s=Math.floor(r/i),c=Math.floor(a/i),f=c+1,m=s-c,b=new fo(m),h=0,y=new Array(i),w=new Array(i),v=0,g=dt.from(e.buffer),p=0;p<i;p++){var _=p<u?c:f;y[p]=g.slice(h,h+_),w[p]=b.encode(y[p]),h+=_,v=Math.max(v,_)}var x=dt.alloc(r),T=0,k,A;for(k=0;k<v;k++)for(A=0;A<i;A++)k<y[A].length&&(x[T++]=y[A][k]);for(k=0;k<m;k++)for(A=0;A<i;A++)x[T++]=w[A][k];return x}function Eo(e,t,n,r){var o;if(go(e))o=Ne.fromArray(e);else if(typeof e=="string"){var a=t;if(!a){var i=Ne.rawSplit(e);a=ge.getBestVersionForData(i,n)}o=Ne.fromString(e,a||40)}else throw new Error("Invalid data");var l=ge.getBestVersionForData(o,n);if(!l)throw new Error("The amount of data is too big to be stored in a QR Code");if(!t)t=l;else if(t<l)throw new Error(`
The chosen QR Code version cannot contain this amount of data.
Minimum version required to store current data is: `+l+`.
`);var u=bo(t,n,o),s=Ce.getSymbolSize(t),c=new co(s);return po(c,t),mo(c),vo(c,t),Se(c,n,0),t>=7&&wo(c,t),yo(c,u),isNaN(r)&&(r=De.getBestMask(c,Se.bind(null,c,n))),De.applyMask(r,c),Se(c,n,r),{modules:c,version:t,errorCorrectionLevel:n,maskPattern:r,segments:o}}Dt.create=function(t,n){if(typeof t>"u"||t==="")throw new Error("No input text");var r=Ae.M,o,a;return typeof n<"u"&&(r=Ae.from(n.errorCorrectionLevel,Ae.M),o=ge.from(n.version),a=De.from(n.maskPattern),n.toSJISFunc&&Ce.setToSJISFunction(n.toSJISFunc)),Eo(t,o,r,a)};var Zt={},Je={};(function(e){function t(n){if(typeof n=="number"&&(n=n.toString()),typeof n!="string")throw new Error("Color should be defined as hex string");var r=n.slice().replace("#","").split("");if(r.length<3||r.length===5||r.length>8)throw new Error("Invalid hex color: "+n);(r.length===3||r.length===4)&&(r=Array.prototype.concat.apply([],r.map(function(a){return[a,a]}))),r.length===6&&r.push("F","F");var o=parseInt(r.join(""),16);return{r:o>>24&255,g:o>>16&255,b:o>>8&255,a:o&255,hex:"#"+r.slice(0,6).join("")}}e.getOptions=function(r){r||(r={}),r.color||(r.color={});var o=typeof r.margin>"u"||r.margin===null||r.margin<0?4:r.margin,a=r.width&&r.width>=21?r.width:void 0,i=r.scale||4;return{width:a,scale:a?4:i,margin:o,color:{dark:t(r.color.dark||"#000000ff"),light:t(r.color.light||"#ffffffff")},type:r.type,rendererOpts:r.rendererOpts||{}}},e.getScale=function(r,o){return o.width&&o.width>=r+o.margin*2?o.width/(r+o.margin*2):o.scale},e.getImageWidth=function(r,o){var a=e.getScale(r,o);return Math.floor((r+o.margin*2)*a)},e.qrToImageData=function(r,o,a){for(var i=o.modules.size,l=o.modules.data,u=e.getScale(i,a),s=Math.floor((i+a.margin*2)*u),c=a.margin*u,f=[a.color.light,a.color.dark],m=0;m<s;m++)for(var b=0;b<s;b++){var h=(m*s+b)*4,y=a.color.light;if(m>=c&&b>=c&&m<s-c&&b<s-c){var w=Math.floor((m-c)/u),v=Math.floor((b-c)/u);y=f[l[w*i+v]?1:0]}r[h++]=y.r,r[h++]=y.g,r[h++]=y.b,r[h]=y.a}}})(Je);(function(e){var t=Je;function n(o,a,i){o.clearRect(0,0,a.width,a.height),a.style||(a.style={}),a.height=i,a.width=i,a.style.height=i+"px",a.style.width=i+"px"}function r(){try{return document.createElement("canvas")}catch{throw new Error("You need to specify a canvas element")}}e.render=function(a,i,l){var u=l,s=i;typeof u>"u"&&(!i||!i.getContext)&&(u=i,i=void 0),i||(s=r()),u=t.getOptions(u);var c=t.getImageWidth(a.modules.size,u),f=s.getContext("2d"),m=f.createImageData(c,c);return t.qrToImageData(m.data,a,u),n(f,s,c),f.putImageData(m,0,0),s},e.renderToDataURL=function(a,i,l){var u=l;typeof u>"u"&&(!i||!i.getContext)&&(u=i,i=void 0),u||(u={});var s=e.render(a,i,u),c=u.type||"image/png",f=u.rendererOpts||{};return s.toDataURL(c,f.quality)}})(Zt);var Xt={},xo=Je;function _t(e,t){var n=e.a/255,r=t+'="'+e.hex+'"';return n<1?r+" "+t+'-opacity="'+n.toFixed(2).slice(1)+'"':r}function Be(e,t,n){var r=e+t;return typeof n<"u"&&(r+=" "+n),r}function ko(e,t,n){for(var r="",o=0,a=!1,i=0,l=0;l<e.length;l++){var u=Math.floor(l%t),s=Math.floor(l/t);!u&&!a&&(a=!0),e[l]?(i++,l>0&&u>0&&e[l-1]||(r+=a?Be("M",u+n,.5+s+n):Be("m",o,0),o=0,a=!1),u+1<t&&e[l+1]||(r+=Be("h",i),i=0)):o++}return r}Xt.render=function(t,n,r){var o=xo.getOptions(n),a=t.modules.size,i=t.modules.data,l=a+o.margin*2,u=o.color.light.a?"<path "+_t(o.color.light,"fill")+' d="M0 0h'+l+"v"+l+'H0z"/>':"",s="<path "+_t(o.color.dark,"stroke")+' d="'+ko(i,a,o.margin)+'"/>',c='viewBox="0 0 '+l+" "+l+'"',f=o.width?'width="'+o.width+'" height="'+o.width+'" ':"",m='<svg xmlns="http://www.w3.org/2000/svg" '+f+c+' shape-rendering="crispEdges">'+u+s+`</svg>
`;return typeof r=="function"&&r(null,m),m};var To=Ar,Oe=Dt,en=Zt,Ao=Xt;function je(e,t,n,r,o){var a=[].slice.call(arguments,1),i=a.length,l=typeof a[i-1]=="function";if(!l&&!To())throw new Error("Callback required as last argument");if(l){if(i<2)throw new Error("Too few arguments provided");i===2?(o=n,n=t,t=r=void 0):i===3&&(t.getContext&&typeof o>"u"?(o=r,r=void 0):(o=r,r=n,n=t,t=void 0))}else{if(i<1)throw new Error("Too few arguments provided");return i===1?(n=t,t=r=void 0):i===2&&!t.getContext&&(r=n,n=t,t=void 0),new Promise(function(s,c){try{var f=Oe.create(n,r);s(e(f,t,r))}catch(m){c(m)}})}try{var u=Oe.create(n,r);o(null,e(u,t,r))}catch(s){o(s)}}le.create=Oe.create;le.toCanvas=je.bind(null,en.render);le.toDataURL=je.bind(null,en.renderToDataURL);le.toString=je.bind(null,function(e,t,n){return Ao.render(e,n)});var No=function(){var e=document.getSelection();if(!e.rangeCount)return function(){};for(var t=document.activeElement,n=[],r=0;r<e.rangeCount;r++)n.push(e.getRangeAt(r));switch(t.tagName.toUpperCase()){case"INPUT":case"TEXTAREA":t.blur();break;default:t=null;break}return e.removeAllRanges(),function(){e.type==="Caret"&&e.removeAllRanges(),e.rangeCount||n.forEach(function(o){e.addRange(o)}),t&&t.focus()}},So=No,ht={"text/plain":"Text","text/html":"Url",default:"Text"},Bo="Copy to clipboard: #{key}, Enter";function Ro(e){var t=(/mac os x/i.test(navigator.userAgent)?"⌘":"Ctrl")+"+C";return e.replace(/#{\s*key\s*}/g,t)}function Po(e,t){var n,r,o,a,i,l,u=!1;t||(t={}),n=t.debug||!1;try{o=So(),a=document.createRange(),i=document.getSelection(),l=document.createElement("span"),l.textContent=e,l.ariaHidden="true",l.style.all="unset",l.style.position="fixed",l.style.top=0,l.style.clip="rect(0, 0, 0, 0)",l.style.whiteSpace="pre",l.style.webkitUserSelect="text",l.style.MozUserSelect="text",l.style.msUserSelect="text",l.style.userSelect="text",l.addEventListener("copy",function(c){if(c.stopPropagation(),t.format)if(c.preventDefault(),typeof c.clipboardData>"u"){n&&console.warn("unable to use e.clipboardData"),n&&console.warn("trying IE specific stuff"),window.clipboardData.clearData();var f=ht[t.format]||ht.default;window.clipboardData.setData(f,e)}else c.clipboardData.clearData(),c.clipboardData.setData(t.format,e);t.onCopy&&(c.preventDefault(),t.onCopy(c.clipboardData))}),document.body.appendChild(l),a.selectNodeContents(l),i.addRange(a);var s=document.execCommand("copy");if(!s)throw new Error("copy command was unsuccessful");u=!0}catch(c){n&&console.error("unable to copy using execCommand: ",c),n&&console.warn("trying IE specific stuff");try{window.clipboardData.setData(t.format||"text",e),t.onCopy&&t.onCopy(window.clipboardData),u=!0}catch(f){n&&console.error("unable to copy using clipboardData: ",f),n&&console.error("falling back to prompt"),r=Ro("message"in t?t.message:Bo),window.prompt(r,e)}}finally{i&&(typeof i.removeRange=="function"?i.removeRange(a):i.removeAllRanges()),l&&document.body.removeChild(l),o()}return u}var Lo=Po,C,ne,Ge,tn,gt,Ze,nn,O={},Ee=[],Mo=/acit|ex(?:s|g|n|p|$)|rph|grid|ows|mnc|ntw|ine[ch]|zoo|^ord/i;function U(e,t){for(var n in t)e[n]=t[n];return e}function rn(e){var t=e.parentNode;t&&t.removeChild(e)}function F(e,t,n){var r,o=arguments,a={};for(r in t)r!=="key"&&r!=="ref"&&(a[r]=t[r]);if(arguments.length>3)for(n=[n],r=3;r<arguments.length;r++)n.push(o[r]);if(n!=null&&(a.children=n),typeof e=="function"&&e.defaultProps!=null)for(r in e.defaultProps)a[r]===void 0&&(a[r]=e.defaultProps[r]);return pe(e,a,t&&t.key,t&&t.ref,null)}function pe(e,t,n,r,o){var a={type:e,props:t,key:n,ref:r,__k:null,__:null,__b:0,__e:null,__d:void 0,__c:null,constructor:void 0,__v:o};return o==null&&(a.__v=a),C.vnode&&C.vnode(a),a}function on(){return{}}function ue(e){return e.children}function M(e,t){this.props=e,this.context=t}function ae(e,t){if(t==null)return e.__?ae(e.__,e.__.__k.indexOf(e)+1):null;for(var n;t<e.__k.length;t++)if((n=e.__k[t])!=null&&n.__e!=null)return n.__e;return typeof e.type=="function"?ae(e):null}function an(e){var t,n;if((e=e.__)!=null&&e.__c!=null){for(e.__e=e.__c.base=null,t=0;t<e.__k.length;t++)if((n=e.__k[t])!=null&&n.__e!=null){e.__e=e.__c.base=n.__e;break}return an(e)}}function _e(e){(!e.__d&&(e.__d=!0)&&ne.push(e)&&!Ge++||gt!==C.debounceRendering)&&((gt=C.debounceRendering)||tn)(Io)}function Io(){for(var e;Ge=ne.length;)e=ne.sort(function(t,n){return t.__v.__b-n.__v.__b}),ne=[],e.some(function(t){var n,r,o,a,i,l,u;t.__d&&(l=(i=(n=t).__v).__e,(u=n.__P)&&(r=[],(o=U({},i)).__v=o,a=Xe(u,i,o,n.__n,u.ownerSVGElement!==void 0,null,r,l??ae(i)),cn(r,i),a!=l&&an(i)))})}function ln(e,t,n,r,o,a,i,l,u){var s,c,f,m,b,h,y,w=n&&n.__k||Ee,v=w.length;if(l==O&&(l=a!=null?a[0]:v?ae(n,0):null),s=0,t.__k=z(t.__k,function(g){if(g!=null){if(g.__=t,g.__b=t.__b+1,(f=w[s])===null||f&&g.key==f.key&&g.type===f.type)w[s]=void 0;else for(c=0;c<v;c++){if((f=w[c])&&g.key==f.key&&g.type===f.type){w[c]=void 0;break}f=null}if(m=Xe(e,g,f=f||O,r,o,a,i,l,u),(c=g.ref)&&f.ref!=c&&(y||(y=[]),f.ref&&y.push(f.ref,null,g),y.push(c,g.__c||m,g)),m!=null){var p;if(h==null&&(h=m),g.__d!==void 0)p=g.__d,g.__d=void 0;else if(a==f||m!=l||m.parentNode==null){e:if(l==null||l.parentNode!==e)e.appendChild(m),p=null;else{for(b=l,c=0;(b=b.nextSibling)&&c<v;c+=2)if(b==m)break e;e.insertBefore(m,l),p=l}t.type=="option"&&(e.value="")}l=p!==void 0?p:m.nextSibling,typeof t.type=="function"&&(t.__d=l)}else l&&f.__e==l&&l.parentNode!=e&&(l=ae(f))}return s++,g}),t.__e=h,a!=null&&typeof t.type!="function")for(s=a.length;s--;)a[s]!=null&&rn(a[s]);for(s=v;s--;)w[s]!=null&&re(w[s],w[s]);if(y)for(s=0;s<y.length;s++)un(y[s],y[++s],y[++s])}function z(e,t,n){if(n==null&&(n=[]),e==null||typeof e=="boolean")t&&n.push(t(null));else if(Array.isArray(e))for(var r=0;r<e.length;r++)z(e[r],t,n);else n.push(t?t(typeof e=="string"||typeof e=="number"?pe(null,e,null,null,e):e.__e!=null||e.__c!=null?pe(e.type,e.props,e.key,null,e.__v):e):e);return n}function Do(e,t,n,r,o){var a;for(a in n)a==="children"||a==="key"||a in t||me(e,a,null,n[a],r);for(a in t)o&&typeof t[a]!="function"||a==="children"||a==="key"||a==="value"||a==="checked"||n[a]===t[a]||me(e,a,t[a],n[a],r)}function pt(e,t,n){t[0]==="-"?e.setProperty(t,n):e[t]=typeof n=="number"&&Mo.test(t)===!1?n+"px":n??""}function me(e,t,n,r,o){var a,i,l,u,s;if(o?t==="className"&&(t="class"):t==="class"&&(t="className"),t==="style")if(a=e.style,typeof n=="string")a.cssText=n;else{if(typeof r=="string"&&(a.cssText="",r=null),r)for(u in r)n&&u in n||pt(a,u,"");if(n)for(s in n)r&&n[s]===r[s]||pt(a,s,n[s])}else t[0]==="o"&&t[1]==="n"?(i=t!==(t=t.replace(/Capture$/,"")),l=t.toLowerCase(),t=(l in e?l:t).slice(2),n?(r||e.addEventListener(t,mt,i),(e.l||(e.l={}))[t]=n):e.removeEventListener(t,mt,i)):t!=="list"&&t!=="tagName"&&t!=="form"&&t!=="type"&&t!=="size"&&!o&&t in e?e[t]=n??"":typeof n!="function"&&t!=="dangerouslySetInnerHTML"&&(t!==(t=t.replace(/^xlink:?/,""))?n==null||n===!1?e.removeAttributeNS("http://www.w3.org/1999/xlink",t.toLowerCase()):e.setAttributeNS("http://www.w3.org/1999/xlink",t.toLowerCase(),n):n==null||n===!1&&!/^ar/.test(t)?e.removeAttribute(t):e.setAttribute(t,n))}function mt(e){this.l[e.type](C.event?C.event(e):e)}function Xe(e,t,n,r,o,a,i,l,u){var s,c,f,m,b,h,y,w,v,g,p=t.type;if(t.constructor!==void 0)return null;(s=C.__b)&&s(t);try{e:if(typeof p=="function"){if(w=t.props,v=(s=p.contextType)&&r[s.__c],g=s?v?v.props.value:s.__:r,n.__c?y=(c=t.__c=n.__c).__=c.__E:("prototype"in p&&p.prototype.render?t.__c=c=new p(w,g):(t.__c=c=new M(w,g),c.constructor=p,c.render=Oo),v&&v.sub(c),c.props=w,c.state||(c.state={}),c.context=g,c.__n=r,f=c.__d=!0,c.__h=[]),c.__s==null&&(c.__s=c.state),p.getDerivedStateFromProps!=null&&(c.__s==c.state&&(c.__s=U({},c.__s)),U(c.__s,p.getDerivedStateFromProps(w,c.__s))),m=c.props,b=c.state,f)p.getDerivedStateFromProps==null&&c.componentWillMount!=null&&c.componentWillMount(),c.componentDidMount!=null&&c.__h.push(c.componentDidMount);else{if(p.getDerivedStateFromProps==null&&w!==m&&c.componentWillReceiveProps!=null&&c.componentWillReceiveProps(w,g),!c.__e&&c.shouldComponentUpdate!=null&&c.shouldComponentUpdate(w,c.__s,g)===!1||t.__v===n.__v&&!c.__){for(c.props=w,c.state=c.__s,t.__v!==n.__v&&(c.__d=!1),c.__v=t,t.__e=n.__e,t.__k=n.__k,c.__h.length&&i.push(c),s=0;s<t.__k.length;s++)t.__k[s]&&(t.__k[s].__=t);break e}c.componentWillUpdate!=null&&c.componentWillUpdate(w,c.__s,g),c.componentDidUpdate!=null&&c.__h.push(function(){c.componentDidUpdate(m,b,h)})}c.context=g,c.props=w,c.state=c.__s,(s=C.__r)&&s(t),c.__d=!1,c.__v=t,c.__P=e,s=c.render(c.props,c.state,c.context),t.__k=s!=null&&s.type==ue&&s.key==null?s.props.children:Array.isArray(s)?s:[s],c.getChildContext!=null&&(r=U(U({},r),c.getChildContext())),f||c.getSnapshotBeforeUpdate==null||(h=c.getSnapshotBeforeUpdate(m,b)),ln(e,t,n,r,o,a,i,l,u),c.base=t.__e,c.__h.length&&i.push(c),y&&(c.__E=c.__=null),c.__e=!1}else a==null&&t.__v===n.__v?(t.__k=n.__k,t.__e=n.__e):t.__e=Uo(n.__e,t,n,r,o,a,i,u);(s=C.diffed)&&s(t)}catch(_){t.__v=null,C.__e(_,t,n)}return t.__e}function cn(e,t){C.__c&&C.__c(t,e),e.some(function(n){try{e=n.__h,n.__h=[],e.some(function(r){r.call(n)})}catch(r){C.__e(r,n.__v)}})}function Uo(e,t,n,r,o,a,i,l){var u,s,c,f,m,b=n.props,h=t.props;if(o=t.type==="svg"||o,a!=null){for(u=0;u<a.length;u++)if((s=a[u])!=null&&((t.type===null?s.nodeType===3:s.localName===t.type)||e==s)){e=s,a[u]=null;break}}if(e==null){if(t.type===null)return document.createTextNode(h);e=o?document.createElementNS("http://www.w3.org/2000/svg",t.type):document.createElement(t.type,h.is&&{is:h.is}),a=null,l=!1}if(t.type===null)b!==h&&e.data!=h&&(e.data=h);else{if(a!=null&&(a=Ee.slice.call(e.childNodes)),c=(b=n.props||O).dangerouslySetInnerHTML,f=h.dangerouslySetInnerHTML,!l){if(b===O)for(b={},m=0;m<e.attributes.length;m++)b[e.attributes[m].name]=e.attributes[m].value;(f||c)&&(f&&c&&f.__html==c.__html||(e.innerHTML=f&&f.__html||""))}Do(e,h,b,o,l),f?t.__k=[]:(t.__k=t.props.children,ln(e,t,n,r,t.type!=="foreignObject"&&o,a,i,O,l)),l||("value"in h&&(u=h.value)!==void 0&&u!==e.value&&me(e,"value",u,b.value,!1),"checked"in h&&(u=h.checked)!==void 0&&u!==e.checked&&me(e,"checked",u,b.checked,!1))}return e}function un(e,t,n){try{typeof e=="function"?e(t):e.current=t}catch(r){C.__e(r,n)}}function re(e,t,n){var r,o,a;if(C.unmount&&C.unmount(e),(r=e.ref)&&(r.current&&r.current!==e.__e||un(r,null,t)),n||typeof e.type=="function"||(n=(o=e.__e)!=null),e.__e=e.__d=void 0,(r=e.__c)!=null){if(r.componentWillUnmount)try{r.componentWillUnmount()}catch(i){C.__e(i,t)}r.base=r.__P=null}if(r=e.__k)for(a=0;a<r.length;a++)r[a]&&re(r[a],t,n);o!=null&&rn(o)}function Oo(e,t,n){return this.constructor(e,n)}function ie(e,t,n){var r,o,a;C.__&&C.__(e,t),o=(r=n===Ze)?null:n&&n.__k||t.__k,e=F(ue,null,[e]),a=[],Xe(t,(r?t:n||t).__k=e,o||O,O,t.ownerSVGElement!==void 0,n&&!r?[n]:o?null:Ee.slice.call(t.childNodes),a,n||O,r),cn(a,e)}function sn(e,t){ie(e,t,Ze)}function Fo(e,t){var n,r;for(r in t=U(U({},e.props),t),arguments.length>2&&(t.children=Ee.slice.call(arguments,2)),n={},t)r!=="key"&&r!=="ref"&&(n[r]=t[r]);return pe(e.type,n,t.key||e.key,t.ref||e.ref,null)}function fn(e){var t={},n={__c:"__cC"+nn++,__:e,Consumer:function(r,o){return r.children(o)},Provider:function(r){var o,a=this;return this.getChildContext||(o=[],this.getChildContext=function(){return t[n.__c]=a,t},this.shouldComponentUpdate=function(i){a.props.value!==i.value&&o.some(function(l){l.context=i.value,_e(l)})},this.sub=function(i){o.push(i);var l=i.componentWillUnmount;i.componentWillUnmount=function(){o.splice(o.indexOf(i),1),l&&l.call(i)}}),r.children}};return n.Consumer.contextType=n,n.Provider.__=n,n}C={__e:function(e,t){for(var n,r;t=t.__;)if((n=t.__c)&&!n.__)try{if(n.constructor&&n.constructor.getDerivedStateFromError!=null&&(r=!0,n.setState(n.constructor.getDerivedStateFromError(e))),n.componentDidCatch!=null&&(r=!0,n.componentDidCatch(e)),r)return _e(n.__E=n)}catch(o){e=o}throw e}},M.prototype.setState=function(e,t){var n;n=this.__s!==this.state?this.__s:this.__s=U({},this.state),typeof e=="function"&&(e=e(n,this.props)),e&&U(n,e),e!=null&&this.__v&&(t&&this.__h.push(t),_e(this))},M.prototype.forceUpdate=function(e){this.__v&&(this.__e=!0,e&&this.__h.push(e),_e(this))},M.prototype.render=ue,ne=[],Ge=0,tn=typeof Promise=="function"?Promise.prototype.then.bind(Promise.resolve()):setTimeout,Ze=O,nn=0;var Y,R,vt,Z=0,Fe=[],wt=C.__r,yt=C.diffed,bt=C.__c,Ct=C.unmount;function X(e,t){C.__h&&C.__h(R,e,Z||t),Z=0;var n=R.__H||(R.__H={__:[],__h:[]});return e>=n.__.length&&n.__.push({}),n.__[e]}function et(e){return Z=1,tt(vn,e)}function tt(e,t,n){var r=X(Y++,2);return r.__c||(r.__c=R,r.__=[n?n(t):vn(void 0,t),function(o){var a=e(r.__[0],o);r.__[0]!==a&&(r.__[0]=a,r.__c.setState({}))}]),r.__}function dn(e,t){var n=X(Y++,3);!C.__s&&rt(n.__H,t)&&(n.__=e,n.__H=t,R.__H.__h.push(n))}function nt(e,t){var n=X(Y++,4);!C.__s&&rt(n.__H,t)&&(n.__=e,n.__H=t,R.__h.push(n))}function _n(e){return Z=5,xe(function(){return{current:e}},[])}function hn(e,t,n){Z=6,nt(function(){typeof e=="function"?e(t()):e&&(e.current=t())},n==null?n:n.concat(e))}function xe(e,t){var n=X(Y++,7);return rt(n.__H,t)?(n.__H=t,n.__h=e,n.__=e()):n.__}function gn(e,t){return Z=8,xe(function(){return e},t)}function pn(e){var t=R.context[e.__c],n=X(Y++,9);return n.__c=e,t?(n.__==null&&(n.__=!0,t.sub(R)),t.props.value):e.__}function mn(e,t){C.useDebugValue&&C.useDebugValue(t?t(e):e)}function $o(e){var t=X(Y++,10),n=et();return t.__=e,R.componentDidCatch||(R.componentDidCatch=function(r){t.__&&t.__(r),n[1](r)}),[n[0],function(){n[1](void 0)}]}function zo(){Fe.some(function(e){if(e.__P)try{e.__H.__h.forEach($e),e.__H.__h.forEach(ze),e.__H.__h=[]}catch(t){return e.__H.__h=[],C.__e(t,e.__v),!0}}),Fe=[]}function $e(e){e.t&&e.t()}function ze(e){var t=e.__();typeof t=="function"&&(e.t=t)}function rt(e,t){return!e||t.some(function(n,r){return n!==e[r]})}function vn(e,t){return typeof t=="function"?t(e):t}C.__r=function(e){wt&&wt(e),Y=0,(R=e.__c).__H&&(R.__H.__h.forEach($e),R.__H.__h.forEach(ze),R.__H.__h=[])},C.diffed=function(e){yt&&yt(e);var t=e.__c;if(t){var n=t.__H;n&&n.__h.length&&(Fe.push(t)!==1&&vt===C.requestAnimationFrame||((vt=C.requestAnimationFrame)||function(r){var o,a=function(){clearTimeout(i),cancelAnimationFrame(o),setTimeout(r)},i=setTimeout(a,100);typeof window<"u"&&(o=requestAnimationFrame(a))})(zo))}},C.__c=function(e,t){t.some(function(n){try{n.__h.forEach($e),n.__h=n.__h.filter(function(r){return!r.__||ze(r)})}catch(r){t.some(function(o){o.__h&&(o.__h=[])}),t=[],C.__e(r,n.__v)}}),bt&&bt(e,t)},C.unmount=function(e){Ct&&Ct(e);var t=e.__c;if(t){var n=t.__H;if(n)try{n.__.forEach(function(r){return r.t&&r.t()})}catch(r){C.__e(r,t.__v)}}};function ot(e,t){for(var n in t)e[n]=t[n];return e}function He(e,t){for(var n in e)if(n!=="__source"&&!(n in t))return!0;for(var r in t)if(r!=="__source"&&e[r]!==t[r])return!0;return!1}var wn=function(e){var t,n;function r(o){var a;return(a=e.call(this,o)||this).isPureReactComponent=!0,a}return n=e,(t=r).prototype=Object.create(n.prototype),t.prototype.constructor=t,t.__proto__=n,r.prototype.shouldComponentUpdate=function(o,a){return He(this.props,o)||He(this.state,a)},r}(M);function yn(e,t){function n(o){var a=this.props.ref,i=a==o.ref;return!i&&a&&(a.call?a(null):a.current=null),t?!t(this.props,o)||!i:He(this.props,o)}function r(o){return this.shouldComponentUpdate=n,F(e,ot({},o))}return r.prototype.isReactComponent=!0,r.displayName="Memo("+(e.displayName||e.name)+")",r.t=!0,r}var Et=C.__b;function bn(e){function t(n){var r=ot({},n);return delete r.ref,e(r,n.ref)}return t.prototype.isReactComponent=t.t=!0,t.displayName="ForwardRef("+(e.displayName||e.name)+")",t}C.__b=function(e){e.type&&e.type.t&&e.ref&&(e.props.ref=e.ref,e.ref=null),Et&&Et(e)};var xt=function(e,t){return e?z(e).reduce(function(n,r,o){return n.concat(t(r,o))},[]):null},Cn={map:xt,forEach:xt,count:function(e){return e?z(e).length:0},only:function(e){if((e=z(e)).length!==1)throw new Error("Children.only() expects only one child.");return e[0]},toArray:z},Ho=C.__e;function En(e){return e&&((e=ot({},e)).__c=null,e.__k=e.__k&&e.__k.map(En)),e}function ve(){this.__u=0,this.o=null,this.__b=null}function xn(e){var t=e.__.__c;return t&&t.u&&t.u(e)}function kn(e){var t,n,r;function o(a){if(t||(t=e()).then(function(i){n=i.default||i},function(i){r=i}),r)throw r;if(!n)throw t;return F(n,a)}return o.displayName="Lazy",o.t=!0,o}function V(){this.i=null,this.l=null}C.__e=function(e,t,n){if(e.then){for(var r,o=t;o=o.__;)if((r=o.__c)&&r.__c)return r.__c(e,t.__c)}Ho(e,t,n)},(ve.prototype=new M).__c=function(e,t){var n=this;n.o==null&&(n.o=[]),n.o.push(t);var r=xn(n.__v),o=!1,a=function(){o||(o=!0,r?r(i):i())};t.__c=t.componentWillUnmount,t.componentWillUnmount=function(){a(),t.__c&&t.__c()};var i=function(){var l;if(!--n.__u)for(n.__v.__k[0]=n.state.u,n.setState({u:n.__b=null});l=n.o.pop();)l.forceUpdate()};n.__u++||n.setState({u:n.__b=n.__v.__k[0]}),e.then(a,a)},ve.prototype.render=function(e,t){return this.__b&&(this.__v.__k[0]=En(this.__b),this.__b=null),[F(M,null,t.u?null:e.children),t.u&&e.fallback]};var kt=function(e,t,n){if(++n[1]===n[0]&&e.l.delete(t),e.props.revealOrder&&(e.props.revealOrder[0]!=="t"||!e.l.size))for(n=e.i;n;){for(;n.length>3;)n.pop()();if(n[1]<n[0])break;e.i=n=n[2]}};(V.prototype=new M).u=function(e){var t=this,n=xn(t.__v),r=t.l.get(e);return r[0]++,function(o){var a=function(){t.props.revealOrder?(r.push(o),kt(t,e,r)):o()};n?n(a):a()}},V.prototype.render=function(e){this.i=null,this.l=new Map;var t=z(e.children);e.revealOrder&&e.revealOrder[0]==="b"&&t.reverse();for(var n=t.length;n--;)this.l.set(t[n],this.i=[1,0,this.i]);return e.children},V.prototype.componentDidUpdate=V.prototype.componentDidMount=function(){var e=this;e.l.forEach(function(t,n){kt(e,n,t)})};var qo=function(){function e(){}var t=e.prototype;return t.getChildContext=function(){return this.props.context},t.render=function(n){return n.children},e}();function Wo(e){var t=this,n=e.container,r=F(qo,{context:t.context},e.vnode);return t.s&&t.s!==n&&(t.v.parentNode&&t.s.removeChild(t.v),re(t.h),t.p=!1),e.vnode?t.p?(n.__k=t.__k,ie(r,n),t.__k=n.__k):(t.v=document.createTextNode(""),sn("",n),n.appendChild(t.v),t.p=!0,t.s=n,ie(r,n,t.v),t.__k=t.v.__k):t.p&&(t.v.parentNode&&t.s.removeChild(t.v),re(t.h)),t.h=r,t.componentWillUnmount=function(){t.v.parentNode&&t.s.removeChild(t.v),re(t.h)},null}function Tn(e,t){return F(Wo,{vnode:e,container:t})}var Tt=/^(?:accent|alignment|arabic|baseline|cap|clip(?!PathU)|color|fill|flood|font|glyph(?!R)|horiz|marker(?!H|W|U)|overline|paint|stop|strikethrough|stroke|text(?!L)|underline|unicode|units|v|vector|vert|word|writing|x(?!C))[A-Z]/;M.prototype.isReactComponent={};var An=typeof Symbol<"u"&&Symbol.for&&Symbol.for("react.element")||60103;function qe(e,t,n){if(t.__k==null)for(;t.firstChild;)t.removeChild(t.firstChild);return ie(e,t),typeof n=="function"&&n(),e?e.__c:null}function Yo(e,t,n){return sn(e,t),typeof n=="function"&&n(),e?e.__c:null}var At=C.event;function Re(e,t){e["UNSAFE_"+t]&&!e[t]&&Object.defineProperty(e,t,{configurable:!1,get:function(){return this["UNSAFE_"+t]},set:function(n){this["UNSAFE_"+t]=n}})}C.event=function(e){At&&(e=At(e)),e.persist=function(){};var t=!1,n=!1,r=e.stopPropagation;e.stopPropagation=function(){r.call(e),t=!0};var o=e.preventDefault;return e.preventDefault=function(){o.call(e),n=!0},e.isPropagationStopped=function(){return t},e.isDefaultPrevented=function(){return n},e.nativeEvent=e};var Nt={configurable:!0,get:function(){return this.class}},St=C.vnode;C.vnode=function(e){e.$$typeof=An;var t=e.type,n=e.props;if(t){if(n.class!=n.className&&(Nt.enumerable="className"in n,n.className!=null&&(n.class=n.className),Object.defineProperty(n,"className",Nt)),typeof t!="function"){var r,o,a;for(a in n.defaultValue&&n.value!==void 0&&(n.value||n.value===0||(n.value=n.defaultValue),delete n.defaultValue),Array.isArray(n.value)&&n.multiple&&t==="select"&&(z(n.children).forEach(function(i){n.value.indexOf(i.props.value)!=-1&&(i.props.selected=!0)}),delete n.value),n)if(r=Tt.test(a))break;if(r)for(a in o=e.props={},n)o[Tt.test(a)?a.replace(/[A-Z0-9]/,"-$&").toLowerCase():a]=n[a]}(function(i){var l=e.type,u=e.props;if(u&&typeof l=="string"){var s={};for(var c in u)/^on(Ani|Tra|Tou)/.test(c)&&(u[c.toLowerCase()]=u[c],delete u[c]),s[c.toLowerCase()]=c;if(s.ondoubleclick&&(u.ondblclick=u[s.ondoubleclick],delete u[s.ondoubleclick]),s.onbeforeinput&&(u.onbeforeinput=u[s.onbeforeinput],delete u[s.onbeforeinput]),s.onchange&&(l==="textarea"||l.toLowerCase()==="input"&&!/^fil|che|ra/i.test(u.type))){var f=s.oninput||"oninput";u[f]||(u[f]=u[s.onchange],delete u[s.onchange])}}})(),typeof t=="function"&&!t.m&&t.prototype&&(Re(t.prototype,"componentWillMount"),Re(t.prototype,"componentWillReceiveProps"),Re(t.prototype,"componentWillUpdate"),t.m=!0)}St&&St(e)};var Vo="16.8.0";function Nn(e){return F.bind(null,e)}function at(e){return!!e&&e.$$typeof===An}function Sn(e){return at(e)?Fo.apply(null,arguments):e}function Bn(e){return!!e.__k&&(ie(null,e),!0)}function Rn(e){return e&&(e.base||e.nodeType===1&&e)||null}var Pn=function(e,t){return e(t)};const Ko={useState:et,useReducer:tt,useEffect:dn,useLayoutEffect:nt,useRef:_n,useImperativeHandle:hn,useMemo:xe,useCallback:gn,useContext:pn,useDebugValue:mn,version:"16.8.0",Children:Cn,render:qe,hydrate:qe,unmountComponentAtNode:Bn,createPortal:Tn,createElement:F,createContext:fn,createFactory:Nn,cloneElement:Sn,createRef:on,Fragment:ue,isValidElement:at,findDOMNode:Rn,Component:M,PureComponent:wn,memo:yn,forwardRef:bn,unstable_batchedUpdates:Pn,Suspense:ve,SuspenseList:V,lazy:kn},Qo=Object.freeze(Object.defineProperty({__proto__:null,Children:Cn,Component:M,Fragment:ue,PureComponent:wn,Suspense:ve,SuspenseList:V,cloneElement:Sn,createContext:fn,createElement:F,createFactory:Nn,createPortal:Tn,createRef:on,default:Ko,findDOMNode:Rn,forwardRef:bn,hydrate:Yo,isValidElement:at,lazy:kn,memo:yn,render:qe,unmountComponentAtNode:Bn,unstable_batchedUpdates:Pn,useCallback:gn,useContext:pn,useDebugValue:mn,useEffect:dn,useErrorBoundary:$o,useImperativeHandle:hn,useLayoutEffect:nt,useMemo:xe,useReducer:tt,useRef:_n,useState:et,version:Vo},Symbol.toStringTag,{value:"Module"})),Jo=Lt(Qo);function Ln(e){return e&&typeof e=="object"&&"default"in e?e.default:e}var B=Tr,Mn=Ln(le),jo=Ln(Lo),d=Jo;function Go(e){Mn.toString(e,{type:"terminal"}).then(console.log)}var Zo=`:root {
  --animation-duration: 300ms;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes fadeOut {
  from {
    opacity: 1;
  }
  to {
    opacity: 0;
  }
}

.animated {
  animation-duration: var(--animation-duration);
  animation-fill-mode: both;
}

.fadeIn {
  animation-name: fadeIn;
}

.fadeOut {
  animation-name: fadeOut;
}

#walletconnect-wrapper {
  -webkit-user-select: none;
  align-items: center;
  display: flex;
  height: 100%;
  justify-content: center;
  left: 0;
  pointer-events: none;
  position: fixed;
  top: 0;
  user-select: none;
  width: 100%;
  z-index: 99999999999999;
}

.walletconnect-modal__headerLogo {
  height: 21px;
}

.walletconnect-modal__header p {
  color: #ffffff;
  font-size: 20px;
  font-weight: 600;
  margin: 0;
  align-items: flex-start;
  display: flex;
  flex: 1;
  margin-left: 5px;
}

.walletconnect-modal__close__wrapper {
  position: absolute;
  top: 0px;
  right: 0px;
  z-index: 10000;
  background: white;
  border-radius: 26px;
  padding: 6px;
  box-sizing: border-box;
  width: 26px;
  height: 26px;
  cursor: pointer;
}

.walletconnect-modal__close__icon {
  position: relative;
  top: 7px;
  right: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  transform: rotate(45deg);
}

.walletconnect-modal__close__line1 {
  position: absolute;
  width: 100%;
  border: 1px solid rgb(48, 52, 59);
}

.walletconnect-modal__close__line2 {
  position: absolute;
  width: 100%;
  border: 1px solid rgb(48, 52, 59);
  transform: rotate(90deg);
}

.walletconnect-qrcode__base {
  -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
  background: rgba(37, 41, 46, 0.95);
  height: 100%;
  left: 0;
  pointer-events: auto;
  position: fixed;
  top: 0;
  transition: 0.4s cubic-bezier(0.19, 1, 0.22, 1);
  width: 100%;
  will-change: opacity;
  padding: 40px;
  box-sizing: border-box;
}

.walletconnect-qrcode__text {
  color: rgba(60, 66, 82, 0.6);
  font-size: 16px;
  font-weight: 600;
  letter-spacing: 0;
  line-height: 1.1875em;
  margin: 10px 0 20px 0;
  text-align: center;
  width: 100%;
}

@media only screen and (max-width: 768px) {
  .walletconnect-qrcode__text {
    font-size: 4vw;
  }
}

@media only screen and (max-width: 320px) {
  .walletconnect-qrcode__text {
    font-size: 14px;
  }
}

.walletconnect-qrcode__image {
  width: calc(100% - 30px);
  box-sizing: border-box;
  cursor: none;
  margin: 0 auto;
}

.walletconnect-qrcode__notification {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  font-size: 16px;
  padding: 16px 20px;
  border-radius: 16px;
  text-align: center;
  transition: all 0.1s ease-in-out;
  background: white;
  color: black;
  margin-bottom: -60px;
  opacity: 0;
}

.walletconnect-qrcode__notification.notification__show {
  opacity: 1;
}

@media only screen and (max-width: 768px) {
  .walletconnect-modal__header {
    height: 130px;
  }
  .walletconnect-modal__base {
    overflow: auto;
  }
}

@media only screen and (min-device-width: 415px) and (max-width: 768px) {
  #content {
    max-width: 768px;
    box-sizing: border-box;
  }
}

@media only screen and (min-width: 375px) and (max-width: 415px) {
  #content {
    max-width: 414px;
    box-sizing: border-box;
  }
}

@media only screen and (min-width: 320px) and (max-width: 375px) {
  #content {
    max-width: 375px;
    box-sizing: border-box;
  }
}

@media only screen and (max-width: 320px) {
  #content {
    max-width: 320px;
    box-sizing: border-box;
  }
}

.walletconnect-modal__base {
  -webkit-font-smoothing: antialiased;
  background: #ffffff;
  border-radius: 24px;
  box-shadow: 0 10px 50px 5px rgba(0, 0, 0, 0.4);
  font-family: ui-rounded, "SF Pro Rounded", "SF Pro Text", medium-content-sans-serif-font,
    -apple-system, BlinkMacSystemFont, ui-sans-serif, "Segoe UI", Roboto, Oxygen, Ubuntu, Cantarell,
    "Open Sans", "Helvetica Neue", sans-serif;
  margin-top: 41px;
  padding: 24px 24px 22px;
  pointer-events: auto;
  position: relative;
  text-align: center;
  transition: 0.4s cubic-bezier(0.19, 1, 0.22, 1);
  will-change: transform;
  overflow: visible;
  transform: translateY(-50%);
  top: 50%;
  max-width: 500px;
  margin: auto;
}

@media only screen and (max-width: 320px) {
  .walletconnect-modal__base {
    padding: 24px 12px;
  }
}

.walletconnect-modal__base .hidden {
  transform: translateY(150%);
  transition: 0.125s cubic-bezier(0.4, 0, 1, 1);
}

.walletconnect-modal__header {
  align-items: center;
  display: flex;
  height: 26px;
  left: 0;
  justify-content: space-between;
  position: absolute;
  top: -42px;
  width: 100%;
}

.walletconnect-modal__base .wc-logo {
  align-items: center;
  display: flex;
  height: 26px;
  margin-top: 15px;
  padding-bottom: 15px;
  pointer-events: auto;
}

.walletconnect-modal__base .wc-logo div {
  background-color: #3399ff;
  height: 21px;
  margin-right: 5px;
  mask-image: url("images/wc-logo.svg") center no-repeat;
  width: 32px;
}

.walletconnect-modal__base .wc-logo p {
  color: #ffffff;
  font-size: 20px;
  font-weight: 600;
  margin: 0;
}

.walletconnect-modal__base h2 {
  color: rgba(60, 66, 82, 0.6);
  font-size: 16px;
  font-weight: 600;
  letter-spacing: 0;
  line-height: 1.1875em;
  margin: 0 0 19px 0;
  text-align: center;
  width: 100%;
}

.walletconnect-modal__base__row {
  -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
  align-items: center;
  border-radius: 20px;
  cursor: pointer;
  display: flex;
  height: 56px;
  justify-content: space-between;
  padding: 0 15px;
  position: relative;
  margin: 0px 0px 8px;
  text-align: left;
  transition: 0.15s cubic-bezier(0.25, 0.46, 0.45, 0.94);
  will-change: transform;
  text-decoration: none;
}

.walletconnect-modal__base__row:hover {
  background: rgba(60, 66, 82, 0.06);
}

.walletconnect-modal__base__row:active {
  background: rgba(60, 66, 82, 0.06);
  transform: scale(0.975);
  transition: 0.1s cubic-bezier(0.25, 0.46, 0.45, 0.94);
}

.walletconnect-modal__base__row__h3 {
  color: #25292e;
  font-size: 20px;
  font-weight: 700;
  margin: 0;
  padding-bottom: 3px;
}

.walletconnect-modal__base__row__right {
  align-items: center;
  display: flex;
  justify-content: center;
}

.walletconnect-modal__base__row__right__app-icon {
  border-radius: 8px;
  height: 34px;
  margin: 0 11px 2px 0;
  width: 34px;
  background-size: 100%;
  box-shadow: 0 4px 12px 0 rgba(37, 41, 46, 0.25);
}

.walletconnect-modal__base__row__right__caret {
  height: 18px;
  opacity: 0.3;
  transition: 0.1s cubic-bezier(0.25, 0.46, 0.45, 0.94);
  width: 8px;
  will-change: opacity;
}

.walletconnect-modal__base__row:hover .caret,
.walletconnect-modal__base__row:active .caret {
  opacity: 0.6;
}

.walletconnect-modal__mobile__toggle {
  width: 80%;
  display: flex;
  margin: 0 auto;
  position: relative;
  overflow: hidden;
  border-radius: 8px;
  margin-bottom: 18px;
  background: #d4d5d9;
}

.walletconnect-modal__single_wallet {
  display: flex;
  justify-content: center;
  margin-top: 7px;
  margin-bottom: 18px;
}

.walletconnect-modal__single_wallet a {
  cursor: pointer;
  color: rgb(64, 153, 255);
  font-size: 21px;
  font-weight: 800;
  text-decoration: none !important;
  margin: 0 auto;
}

.walletconnect-modal__mobile__toggle_selector {
  width: calc(50% - 8px);
  background: white;
  position: absolute;
  border-radius: 5px;
  height: calc(100% - 8px);
  top: 4px;
  transition: all 0.2s ease-in-out;
  transform: translate3d(4px, 0, 0);
}

.walletconnect-modal__mobile__toggle.right__selected .walletconnect-modal__mobile__toggle_selector {
  transform: translate3d(calc(100% + 12px), 0, 0);
}

.walletconnect-modal__mobile__toggle a {
  font-size: 12px;
  width: 50%;
  text-align: center;
  padding: 8px;
  margin: 0;
  font-weight: 600;
  z-index: 1;
}

.walletconnect-modal__footer {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

@media only screen and (max-width: 768px) {
  .walletconnect-modal__footer {
    margin-top: 5vw;
  }
}

.walletconnect-modal__footer a {
  cursor: pointer;
  color: #898d97;
  font-size: 15px;
  margin: 0 auto;
}

@media only screen and (max-width: 320px) {
  .walletconnect-modal__footer a {
    font-size: 14px;
  }
}

.walletconnect-connect__buttons__wrapper {
  max-height: 44vh;
}

.walletconnect-connect__buttons__wrapper__android {
  margin: 50% 0;
}

.walletconnect-connect__buttons__wrapper__wrap {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  margin: 10px 0;
}

@media only screen and (min-width: 768px) {
  .walletconnect-connect__buttons__wrapper__wrap {
    margin-top: 40px;
  }
}

.walletconnect-connect__button {
  background-color: rgb(64, 153, 255);
  padding: 12px;
  border-radius: 8px;
  text-decoration: none;
  color: rgb(255, 255, 255);
  font-weight: 500;
}

.walletconnect-connect__button__icon_anchor {
  cursor: pointer;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  margin: 8px;
  width: 42px;
  justify-self: center;
  flex-direction: column;
  text-decoration: none !important;
}

@media only screen and (max-width: 320px) {
  .walletconnect-connect__button__icon_anchor {
    margin: 4px;
  }
}

.walletconnect-connect__button__icon {
  border-radius: 10px;
  height: 42px;
  margin: 0;
  width: 42px;
  background-size: cover !important;
  box-shadow: 0 4px 12px 0 rgba(37, 41, 46, 0.25);
}

.walletconnect-connect__button__text {
  color: #424952;
  font-size: 2.7vw;
  text-decoration: none !important;
  padding: 0;
  margin-top: 1.8vw;
  font-weight: 600;
}

@media only screen and (min-width: 768px) {
  .walletconnect-connect__button__text {
    font-size: 16px;
    margin-top: 12px;
  }
}

.walletconnect-search__input {
  border: none;
  background: #d4d5d9;
  border-style: none;
  padding: 8px 16px;
  outline: none;
  font-style: normal;
  font-stretch: normal;
  font-size: 16px;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  letter-spacing: normal;
  text-align: left;
  border-radius: 8px;
  width: calc(100% - 16px);
  margin: 0;
  margin-bottom: 8px;
}
`;typeof Symbol<"u"&&(Symbol.iterator||(Symbol.iterator=Symbol("Symbol.iterator")));typeof Symbol<"u"&&(Symbol.asyncIterator||(Symbol.asyncIterator=Symbol("Symbol.asyncIterator")));function Xo(e,t){try{var n=e()}catch(r){return t(r)}return n&&n.then?n.then(void 0,t):n}var ea="data:image/svg+xml,%3Csvg height='185' viewBox='0 0 300 185' width='300' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='m61.4385429 36.2562612c48.9112241-47.8881663 128.2119871-47.8881663 177.1232091 0l5.886545 5.7634174c2.445561 2.3944081 2.445561 6.2765112 0 8.6709204l-20.136695 19.715503c-1.222781 1.1972051-3.2053 1.1972051-4.428081 0l-8.100584-7.9311479c-34.121692-33.4079817-89.443886-33.4079817-123.5655788 0l-8.6750562 8.4936051c-1.2227816 1.1972041-3.205301 1.1972041-4.4280806 0l-20.1366949-19.7155031c-2.4455612-2.3944092-2.4455612-6.2765122 0-8.6709204zm218.7677961 40.7737449 17.921697 17.546897c2.445549 2.3943969 2.445563 6.2764769.000031 8.6708899l-80.810171 79.121134c-2.445544 2.394426-6.410582 2.394453-8.85616.000062-.00001-.00001-.000022-.000022-.000032-.000032l-57.354143-56.154572c-.61139-.598602-1.60265-.598602-2.21404 0-.000004.000004-.000007.000008-.000011.000011l-57.3529212 56.154531c-2.4455368 2.394432-6.4105755 2.394472-8.8561612.000087-.0000143-.000014-.0000296-.000028-.0000449-.000044l-80.81241943-79.122185c-2.44556021-2.394408-2.44556021-6.2765115 0-8.6709197l17.92172963-17.5468673c2.4455602-2.3944082 6.4105989-2.3944082 8.8561602 0l57.3549775 56.155357c.6113908.598602 1.602649.598602 2.2140398 0 .0000092-.000009.0000174-.000017.0000265-.000024l57.3521031-56.155333c2.445505-2.3944633 6.410544-2.3945531 8.856161-.0002.000034.0000336.000068.0000673.000101.000101l57.354902 56.155432c.61139.598601 1.60265.598601 2.21404 0l57.353975-56.1543249c2.445561-2.3944092 6.410599-2.3944092 8.85616 0z' fill='%233b99fc'/%3E%3C/svg%3E",ta="WalletConnect",na=300,ra="rgb(64, 153, 255)",In="walletconnect-wrapper",Bt="walletconnect-style-sheet",Dn="walletconnect-qrcode-modal",oa="walletconnect-qrcode-close",Un="walletconnect-qrcode-text",aa="walletconnect-connect-button";function ia(e){return d.createElement("div",{className:"walletconnect-modal__header"},d.createElement("img",{src:ea,className:"walletconnect-modal__headerLogo"}),d.createElement("p",null,ta),d.createElement("div",{className:"walletconnect-modal__close__wrapper",onClick:e.onClose},d.createElement("div",{id:oa,className:"walletconnect-modal__close__icon"},d.createElement("div",{className:"walletconnect-modal__close__line1"}),d.createElement("div",{className:"walletconnect-modal__close__line2"}))))}function la(e){return d.createElement("a",{className:"walletconnect-connect__button",href:e.href,id:aa+"-"+e.name,onClick:e.onClick,rel:"noopener noreferrer",style:{backgroundColor:e.color},target:"_blank"},e.name)}var ca="data:image/svg+xml,%3Csvg fill='none' height='18' viewBox='0 0 8 18' width='8' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath clip-rule='evenodd' d='m.586301.213898c-.435947.33907-.5144813.967342-.175411 1.403292l4.87831 6.27212c.28087.36111.28087.86677 0 1.22788l-4.878311 6.27211c-.33907.436-.260536 1.0642.175412 1.4033.435949.3391 1.064219.2605 1.403289-.1754l4.87832-6.2721c.84259-1.08336.84259-2.60034 0-3.68367l-4.87832-6.27212c-.33907-.4359474-.96734-.514482-1.403289-.175412z' fill='%233c4252' fill-rule='evenodd'/%3E%3C/svg%3E";function ua(e){var t=e.color,n=e.href,r=e.name,o=e.logo,a=e.onClick;return d.createElement("a",{className:"walletconnect-modal__base__row",href:n,onClick:a,rel:"noopener noreferrer",target:"_blank"},d.createElement("h3",{className:"walletconnect-modal__base__row__h3"},r),d.createElement("div",{className:"walletconnect-modal__base__row__right"},d.createElement("div",{className:"walletconnect-modal__base__row__right__app-icon",style:{background:"url('"+o+"') "+t,backgroundSize:"100%"}}),d.createElement("img",{src:ca,className:"walletconnect-modal__base__row__right__caret"})))}function sa(e){var t=e.color,n=e.href,r=e.name,o=e.logo,a=e.onClick,i=window.innerWidth<768?(r.length>8?2.5:2.7)+"vw":"inherit";return d.createElement("a",{className:"walletconnect-connect__button__icon_anchor",href:n,onClick:a,rel:"noopener noreferrer",target:"_blank"},d.createElement("div",{className:"walletconnect-connect__button__icon",style:{background:"url('"+o+"') "+t,backgroundSize:"100%"}}),d.createElement("div",{style:{fontSize:i},className:"walletconnect-connect__button__text"},r))}var fa=5,Pe=12;function da(e){var t=B.isAndroid(),n=d.useState(""),r=n[0],o=n[1],a=d.useState(""),i=a[0],l=a[1],u=d.useState(1),s=u[0],c=u[1],f=i?e.links.filter(function(_){return _.name.toLowerCase().includes(i.toLowerCase())}):e.links,m=e.errorMessage,b=i||f.length>fa,h=Math.ceil(f.length/Pe),y=[(s-1)*Pe+1,s*Pe],w=f.length?f.filter(function(_,x){return x+1>=y[0]&&x+1<=y[1]}):[],v=!t&&h>1,g=void 0;function p(_){o(_.target.value),clearTimeout(g),_.target.value?g=setTimeout(function(){l(_.target.value),c(1)},1e3):(o(""),l(""),c(1))}return d.createElement("div",null,d.createElement("p",{id:Un,className:"walletconnect-qrcode__text"},t?e.text.connect_mobile_wallet:e.text.choose_preferred_wallet),!t&&d.createElement("input",{className:"walletconnect-search__input",placeholder:"Search",value:r,onChange:p}),d.createElement("div",{className:"walletconnect-connect__buttons__wrapper"+(t?"__android":b&&f.length?"__wrap":"")},t?d.createElement(la,{name:e.text.connect,color:ra,href:e.uri,onClick:d.useCallback(function(){B.saveMobileLinkInfo({name:"Unknown",href:e.uri})},[])}):w.length?w.map(function(_){var x=_.color,T=_.name,k=_.shortName,A=_.logo,S=B.formatIOSMobile(e.uri,_),N=d.useCallback(function(){B.saveMobileLinkInfo({name:T,href:S})},[w]);return b?d.createElement(sa,{color:x,href:S,name:k||T,logo:A,onClick:N}):d.createElement(ua,{color:x,href:S,name:T,logo:A,onClick:N})}):d.createElement(d.Fragment,null,d.createElement("p",null,m.length?e.errorMessage:e.links.length&&!f.length?e.text.no_wallets_found:e.text.loading))),v&&d.createElement("div",{className:"walletconnect-modal__footer"},Array(h).fill(0).map(function(_,x){var T=x+1,k=s===T;return d.createElement("a",{style:{margin:"auto 10px",fontWeight:k?"bold":"normal"},onClick:function(){return c(T)}},T)})))}function _a(e){var t=!!e.message.trim();return d.createElement("div",{className:"walletconnect-qrcode__notification"+(t?" notification__show":"")},e.message)}var ha=function(e){try{var t="";return Promise.resolve(Mn.toString(e,{margin:0,type:"svg"})).then(function(n){return typeof n=="string"&&(t=n.replace("<svg",'<svg class="walletconnect-qrcode__image"')),t})}catch(n){return Promise.reject(n)}};function ga(e){var t=d.useState(""),n=t[0],r=t[1],o=d.useState(""),a=o[0],i=o[1];d.useEffect(function(){try{return Promise.resolve(ha(e.uri)).then(function(u){i(u)})}catch(u){Promise.reject(u)}},[]);var l=function(){var u=jo(e.uri);u?(r(e.text.copied_to_clipboard),setInterval(function(){return r("")},1200)):(r("Error"),setInterval(function(){return r("")},1200))};return d.createElement("div",null,d.createElement("p",{id:Un,className:"walletconnect-qrcode__text"},e.text.scan_qrcode_with_wallet),d.createElement("div",{dangerouslySetInnerHTML:{__html:a}}),d.createElement("div",{className:"walletconnect-modal__footer"},d.createElement("a",{onClick:l},e.text.copy_to_clipboard)),d.createElement(_a,{message:n}))}function pa(e){var t=B.isAndroid(),n=B.isMobile(),r=n?e.qrcodeModalOptions&&e.qrcodeModalOptions.mobileLinks?e.qrcodeModalOptions.mobileLinks:void 0:e.qrcodeModalOptions&&e.qrcodeModalOptions.desktopLinks?e.qrcodeModalOptions.desktopLinks:void 0,o=d.useState(!1),a=o[0],i=o[1],l=d.useState(!1),u=l[0],s=l[1],c=d.useState(!n),f=c[0],m=c[1],b={mobile:n,text:e.text,uri:e.uri,qrcodeModalOptions:e.qrcodeModalOptions},h=d.useState(""),y=h[0],w=h[1],v=d.useState(!1),g=v[0],p=v[1],_=d.useState([]),x=_[0],T=_[1],k=d.useState(""),A=k[0],S=k[1],N=function(){u||a||r&&!r.length||x.length>0||d.useEffect(function(){var D=function(){try{if(t)return Promise.resolve();i(!0);var P=Xo(function(){var ee=e.qrcodeModalOptions&&e.qrcodeModalOptions.registryUrl?e.qrcodeModalOptions.registryUrl:B.getWalletRegistryUrl();return Promise.resolve(fetch(ee)).then(function($n){return Promise.resolve($n.json()).then(function(zn){var Hn=zn.listings,qn=n?"mobile":"desktop",se=B.getMobileLinkRegistry(B.formatMobileRegistry(Hn,qn),r);i(!1),s(!0),S(se.length?"":e.text.no_supported_wallets),T(se);var it=se.length===1;it&&(w(B.formatIOSMobile(e.uri,se[0])),m(!0)),p(it)})})},function(ee){i(!1),s(!0),S(e.text.something_went_wrong),console.error(ee)});return Promise.resolve(P&&P.then?P.then(function(){}):void 0)}catch(ee){return Promise.reject(ee)}};D()})};N();var W=n?f:!f;return d.createElement("div",{id:Dn,className:"walletconnect-qrcode__base animated fadeIn"},d.createElement("div",{className:"walletconnect-modal__base"},d.createElement(ia,{onClose:e.onClose}),g&&f?d.createElement("div",{className:"walletconnect-modal__single_wallet"},d.createElement("a",{onClick:function(){return B.saveMobileLinkInfo({name:x[0].name,href:y})},href:y,rel:"noopener noreferrer",target:"_blank"},e.text.connect_with+" "+(g?x[0].name:"")+" ›")):t||a||!a&&x.length?d.createElement("div",{className:"walletconnect-modal__mobile__toggle"+(W?" right__selected":"")},d.createElement("div",{className:"walletconnect-modal__mobile__toggle_selector"}),n?d.createElement(d.Fragment,null,d.createElement("a",{onClick:function(){return m(!1),N()}},e.text.mobile),d.createElement("a",{onClick:function(){return m(!0)}},e.text.qrcode)):d.createElement(d.Fragment,null,d.createElement("a",{onClick:function(){return m(!0)}},e.text.qrcode),d.createElement("a",{onClick:function(){return m(!1),N()}},e.text.desktop))):null,d.createElement("div",null,f||!t&&!a&&!x.length?d.createElement(ga,Object.assign({},b)):d.createElement(da,Object.assign({},b,{links:x,errorMessage:A})))))}var ma={choose_preferred_wallet:"Wähle bevorzugte Wallet",connect_mobile_wallet:"Verbinde mit Mobile Wallet",scan_qrcode_with_wallet:"Scanne den QR-code mit einer WalletConnect kompatiblen Wallet",connect:"Verbinden",qrcode:"QR-Code",mobile:"Mobile",desktop:"Desktop",copy_to_clipboard:"In die Zwischenablage kopieren",copied_to_clipboard:"In die Zwischenablage kopiert!",connect_with:"Verbinden mit Hilfe von",loading:"Laden...",something_went_wrong:"Etwas ist schief gelaufen",no_supported_wallets:"Es gibt noch keine unterstützten Wallet",no_wallets_found:"keine Wallet gefunden"},va={choose_preferred_wallet:"Choose your preferred wallet",connect_mobile_wallet:"Connect to Mobile Wallet",scan_qrcode_with_wallet:"Scan QR code with a WalletConnect-compatible wallet",connect:"Connect",qrcode:"QR Code",mobile:"Mobile",desktop:"Desktop",copy_to_clipboard:"Copy to clipboard",copied_to_clipboard:"Copied to clipboard!",connect_with:"Connect with",loading:"Loading...",something_went_wrong:"Something went wrong",no_supported_wallets:"There are no supported wallets yet",no_wallets_found:"No wallets found"},wa={choose_preferred_wallet:"Elige tu billetera preferida",connect_mobile_wallet:"Conectar a billetera móvil",scan_qrcode_with_wallet:"Escanea el código QR con una billetera compatible con WalletConnect",connect:"Conectar",qrcode:"Código QR",mobile:"Móvil",desktop:"Desktop",copy_to_clipboard:"Copiar",copied_to_clipboard:"Copiado!",connect_with:"Conectar mediante",loading:"Cargando...",something_went_wrong:"Algo salió mal",no_supported_wallets:"Todavía no hay billeteras compatibles",no_wallets_found:"No se encontraron billeteras"},ya={choose_preferred_wallet:"Choisissez votre portefeuille préféré",connect_mobile_wallet:"Se connecter au portefeuille mobile",scan_qrcode_with_wallet:"Scannez le QR code avec un portefeuille compatible WalletConnect",connect:"Se connecter",qrcode:"QR Code",mobile:"Mobile",desktop:"Desktop",copy_to_clipboard:"Copier",copied_to_clipboard:"Copié!",connect_with:"Connectez-vous à l'aide de",loading:"Chargement...",something_went_wrong:"Quelque chose a mal tourné",no_supported_wallets:"Il n'y a pas encore de portefeuilles pris en charge",no_wallets_found:"Aucun portefeuille trouvé"},ba={choose_preferred_wallet:"원하는 지갑을 선택하세요",connect_mobile_wallet:"모바일 지갑과 연결",scan_qrcode_with_wallet:"WalletConnect 지원 지갑에서 QR코드를 스캔하세요",connect:"연결",qrcode:"QR 코드",mobile:"모바일",desktop:"데스크탑",copy_to_clipboard:"클립보드에 복사",copied_to_clipboard:"클립보드에 복사되었습니다!",connect_with:"와 연결하다",loading:"로드 중...",something_went_wrong:"문제가 발생했습니다.",no_supported_wallets:"아직 지원되는 지갑이 없습니다",no_wallets_found:"지갑을 찾을 수 없습니다"},Ca={choose_preferred_wallet:"Escolha sua carteira preferida",connect_mobile_wallet:"Conectar-se à carteira móvel",scan_qrcode_with_wallet:"Ler o código QR com uma carteira compatível com WalletConnect",connect:"Conectar",qrcode:"Código QR",mobile:"Móvel",desktop:"Desktop",copy_to_clipboard:"Copiar",copied_to_clipboard:"Copiado!",connect_with:"Ligar por meio de",loading:"Carregamento...",something_went_wrong:"Algo correu mal",no_supported_wallets:"Ainda não há carteiras suportadas",no_wallets_found:"Nenhuma carteira encontrada"},Ea={choose_preferred_wallet:"选择你的钱包",connect_mobile_wallet:"连接至移动端钱包",scan_qrcode_with_wallet:"使用兼容 WalletConnect 的钱包扫描二维码",connect:"连接",qrcode:"二维码",mobile:"移动",desktop:"桌面",copy_to_clipboard:"复制到剪贴板",copied_to_clipboard:"复制到剪贴板成功！",connect_with:"通过以下方式连接",loading:"正在加载...",something_went_wrong:"出了问题",no_supported_wallets:"目前还没有支持的钱包",no_wallets_found:"没有找到钱包"},xa={choose_preferred_wallet:"کیف پول مورد نظر خود را انتخاب کنید",connect_mobile_wallet:"به کیف پول موبایل وصل شوید",scan_qrcode_with_wallet:"کد QR را با یک کیف پول سازگار با WalletConnect اسکن کنید",connect:"اتصال",qrcode:"کد QR",mobile:"سیار",desktop:"دسکتاپ",copy_to_clipboard:"کپی به کلیپ بورد",copied_to_clipboard:"در کلیپ بورد کپی شد!",connect_with:"ارتباط با",loading:"...بارگذاری",something_went_wrong:"مشکلی پیش آمد",no_supported_wallets:"هنوز هیچ کیف پول پشتیبانی شده ای وجود ندارد",no_wallets_found:"هیچ کیف پولی پیدا نشد"},Rt={de:ma,en:va,es:wa,fr:ya,ko:ba,pt:Ca,zh:Ea,fa:xa};function ka(){var e=B.getDocumentOrThrow(),t=e.getElementById(Bt);t&&e.head.removeChild(t);var n=e.createElement("style");n.setAttribute("id",Bt),n.innerText=Zo,e.head.appendChild(n)}function Ta(){var e=B.getDocumentOrThrow(),t=e.createElement("div");return t.setAttribute("id",In),e.body.appendChild(t),t}function On(){var e=B.getDocumentOrThrow(),t=e.getElementById(Dn);t&&(t.className=t.className.replace("fadeIn","fadeOut"),setTimeout(function(){var n=e.getElementById(In);n&&e.body.removeChild(n)},na))}function Aa(e){return function(){On(),e&&e()}}function Na(){var e=B.getNavigatorOrThrow().language.split("-")[0]||"en";return Rt[e]||Rt.en}function Sa(e,t,n){ka();var r=Ta();d.render(d.createElement(pa,{text:Na(),uri:e,onClose:Aa(t),qrcodeModalOptions:n}),r)}function Ba(){On()}var Fn=function(){return typeof ke<"u"&&typeof ke.versions<"u"&&typeof ke.versions.node<"u"};function Ra(e,t,n){console.log(e),Fn()?Go(e):Sa(e,t,n)}function Pa(){Fn()||Ba()}var La={open:Ra,close:Pa},Pt=La;const Da=br({__proto__:null,default:Pt},[Pt]);export{Da as i};
