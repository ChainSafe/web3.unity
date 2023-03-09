import{c as V,b as ln}from"./skip-3c1bbf36.js";import{aN as d,ac as v,a1 as cn,aO as kn,X as F,a0 as R,a9 as S,aP as M,ah as U,a2 as A,aj as B,aQ as G,aR as C,ab as x,aa as Y,a5 as O,af as I,a4 as Wn,aS as vn,ag as E,Y as T,$ as N,aT as k,aU as $,ao as sn,aE as W,ay as Fn,as as dn,aK as q,aC as Cn,a3 as J,ai as Q,aB as hn,aV as pn,aW as Mn,aD as Ln,al as zn,am as Dn,aJ as P,aX as Un}from"./index-c4efa424.js";function $n(){return d(function(t,n){var e=null;t._refCount++;var r=v(n,void 0,void 0,void 0,function(){if(!t||t._refCount<=0||0<--t._refCount){e=null;return}var a=t._connection,u=e;e=null,a&&(!u||a===u)&&a.unsubscribe(),n.unsubscribe()});t.subscribe(r),r.closed||(e=t.connect())})}var Z=function(t){cn(n,t);function n(e,r){var a=t.call(this)||this;return a.source=e,a.subjectFactory=r,a._subject=null,a._refCount=0,a._connection=null,kn(e)&&(a.lift=e.lift),a}return n.prototype._subscribe=function(e){return this.getSubject().subscribe(e)},n.prototype.getSubject=function(){var e=this._subject;return(!e||e.isStopped)&&(this._subject=this.subjectFactory()),this._subject},n.prototype._teardown=function(){this._refCount=0;var e=this._connection;this._subject=this._connection=null,e==null||e.unsubscribe()},n.prototype.connect=function(){var e=this,r=this._connection;if(!r){r=this._connection=new F;var a=this.getSubject();r.add(this.source.subscribe(v(a,void 0,function(){e._teardown(),a.complete()},function(u){e._teardown(),a.error(u)},function(){return e._teardown()}))),r.closed&&(this._connection=null,r=F.EMPTY)}return r},n.prototype.refCount=function(){return $n()(this)},n}(R),qn=function(t){cn(n,t);function n(){var e=t!==null&&t.apply(this,arguments)||this;return e._value=null,e._hasValue=!1,e._isComplete=!1,e}return n.prototype._checkFinalizedStatuses=function(e){var r=this,a=r.hasError,u=r._hasValue,i=r._value,o=r.thrownError,f=r.isStopped,l=r._isComplete;a?e.error(o):(f||l)&&(u&&e.next(i),e.complete())},n.prototype.next=function(e){this.isStopped||(this._value=e,this._hasValue=!0)},n.prototype.complete=function(){var e=this,r=e._hasValue,a=e._value,u=e._isComplete;u||(this._isComplete=!0,r&&t.prototype.next.call(this,a),t.prototype.complete.call(this))},n}(S);function mn(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];var e=M(t);return U(t,e)}function Pn(t,n){var e=A(t)?t:function(){return t},r=function(a){return a.error(e())};return new R(n?function(a){return n.schedule(r,0,a)}:r)}var rn;(function(t){t.NEXT="N",t.ERROR="E",t.COMPLETE="C"})(rn||(rn={}));var X=function(){function t(n,e,r){this.kind=n,this.value=e,this.error=r,this.hasValue=n==="N"}return t.prototype.observe=function(n){return gn(this,n)},t.prototype.do=function(n,e,r){var a=this,u=a.kind,i=a.value,o=a.error;return u==="N"?n==null?void 0:n(i):u==="E"?e==null?void 0:e(o):r==null?void 0:r()},t.prototype.accept=function(n,e,r){var a;return A((a=n)===null||a===void 0?void 0:a.next)?this.observe(n):this.do(n,e,r)},t.prototype.toObservable=function(){var n=this,e=n.kind,r=n.value,a=n.error,u=e==="N"?mn(r):e==="E"?Pn(function(){return a}):e==="C"?B:0;if(!u)throw new TypeError("Unexpected notification kind "+e);return u},t.createNext=function(n){return new t("N",n)},t.createError=function(n){return new t("E",void 0,n)},t.createComplete=function(){return t.completeNotification},t.completeNotification=new t("C"),t}();function gn(t,n){var e,r,a,u=t,i=u.kind,o=u.value,f=u.error;if(typeof i!="string")throw new TypeError('Invalid notification, missing "kind"');i==="N"?(e=n.next)===null||e===void 0||e.call(n,o):i==="E"?(r=n.error)===null||r===void 0||r.call(n,f):(a=n.complete)===null||a===void 0||a.call(n)}var an=G(function(t){return function(){t(this),this.name="ArgumentOutOfRangeError",this.message="argument out of range"}}),Bn=G(function(t){return function(e){t(this),this.name="NotFoundError",this.message=e}}),Gn=G(function(t){return function(e){t(this),this.name="SequenceError",this.message=e}});function H(t){return t instanceof Date&&!isNaN(t)}var Jn=G(function(t){return function(e){e===void 0&&(e=null),t(this),this.message="Timeout has occurred",this.name="TimeoutError",this.info=e}});function Kn(t,n){var e=H(t)?{first:t}:typeof t=="number"?{each:t}:t,r=e.first,a=e.each,u=e.with,i=u===void 0?Xn:u,o=e.scheduler,f=o===void 0?n??V:o,l=e.meta,c=l===void 0?null:l;if(r==null&&a==null)throw new TypeError("No timeout provided.");return d(function(s,m){var g,h,y=null,p=0,w=function(b){h=C(m,f,function(){try{g.unsubscribe(),x(i({meta:c,lastValue:y,seen:p})).subscribe(m)}catch(j){m.error(j)}},b)};g=s.subscribe(v(m,function(b){h==null||h.unsubscribe(),p++,m.next(y=b),a>0&&w(a)},void 0,void 0,function(){h!=null&&h.closed||h==null||h.unsubscribe(),y=null})),!p&&w(r!=null?typeof r=="number"?r:+r-f.now():a)})}function Xn(t){throw new Jn(t)}var Yn=Array.isArray,On=Object.getPrototypeOf,Qn=Object.prototype,Zn=Object.keys;function Hn(t){if(t.length===1){var n=t[0];if(Yn(n))return{args:n,keys:null};if(_n(n)){var e=Zn(n);return{args:e.map(function(r){return n[r]}),keys:e}}}return{args:t,keys:null}}function _n(t){return t&&typeof t=="object"&&On(t)===Qn}function nt(t,n){return t.reduce(function(e,r,a){return e[r]=n[a],e},{})}function tt(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];var e=M(t),r=Y(t),a=Hn(t),u=a.args,i=a.keys;if(u.length===0)return U([],e);var o=new R(yn(u,e,i?function(f){return nt(i,f)}:I));return r?o.pipe(O(r)):o}function yn(t,n,e){return e===void 0&&(e=I),function(r){un(n,function(){for(var a=t.length,u=new Array(a),i=a,o=a,f=function(c){un(n,function(){var s=U(t[c],n),m=!1;s.subscribe(v(r,function(g){u[c]=g,m||(m=!0,o--),o||r.next(e(u.slice()))},function(){--i||r.complete()}))},r)},l=0;l<a;l++)f(l)},r)}}function un(t,n,e){t?C(e,t,n):n()}function L(t,n,e){t===void 0&&(t=0),e===void 0&&(e=ln);var r=-1;return n!=null&&(Wn(n)?e=n:r=n),new R(function(a){var u=H(t)?+t-e.now():t;u<0&&(u=0);var i=0;return e.schedule(function(){a.closed||(a.next(i++),0<=r?this.schedule(void 0,r):a.complete())},u)})}function et(t,n){return t===void 0&&(t=0),n===void 0&&(n=V),t<0&&(t=0),L(t,t,n)}var rt=Array.isArray;function z(t){return t.length===1&&rt(t[0])?t[0]:t}function at(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];var e=z(t);return new R(function(r){var a=0,u=function(){if(a<e.length){var i=void 0;try{i=x(e[a++])}catch{u();return}var o=new vn(r,void 0,E,E);i.subscribe(o),o.add(u)}else r.complete()};u()})}function Tt(t,n){return function(e,r){return!t.call(n,e,r)}}function Nt(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];return t=z(t),t.length===1?x(t[0]):new R(wn(t))}function wn(t){return function(n){for(var e=[],r=function(u){e.push(x(t[u]).subscribe(v(n,function(i){if(e){for(var o=0;o<e.length;o++)o!==u&&e[o].unsubscribe();e=null}n.next(i)})))},a=0;e&&!n.closed&&a<t.length;a++)r(a)}}function xn(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];var e=Y(t),r=z(t);return r.length?new R(function(a){var u=r.map(function(){return[]}),i=r.map(function(){return!1});a.add(function(){u=i=null});for(var o=function(l){x(r[l]).subscribe(v(a,function(c){if(u[l].push(c),u.every(function(m){return m.length})){var s=u.map(function(m){return m.shift()});a.next(e?e.apply(void 0,T([],N(s))):s),u.some(function(m,g){return!m.length&&i[g]})&&a.complete()}},function(){i[l]=!0,!u[l].length&&a.complete()}))},f=0;!a.closed&&f<r.length;f++)o(f);return function(){u=i=null}}):B}function ut(t){return d(function(n,e){var r=!1,a=null,u=null,i=!1,o=function(){if(u==null||u.unsubscribe(),u=null,r){r=!1;var l=a;a=null,e.next(l)}i&&e.complete()},f=function(){u=null,i&&e.complete()};n.subscribe(v(e,function(l){r=!0,a=l,u||x(t(l)).subscribe(u=v(e,o,f))},function(){i=!0,(!r||!u||u.closed)&&e.complete()}))})}function It(t,n){return n===void 0&&(n=V),ut(function(){return L(t,n)})}function Vt(t){return d(function(n,e){var r=[];return n.subscribe(v(e,function(a){return r.push(a)},function(){e.next(r),e.complete()})),x(t).subscribe(v(e,function(){var a=r;r=[],e.next(a)},E)),function(){r=null}})}function Rt(t,n){return n===void 0&&(n=null),n=n??t,d(function(e,r){var a=[],u=0;e.subscribe(v(r,function(i){var o,f,l,c,s=null;u++%n===0&&a.push([]);try{for(var m=k(a),g=m.next();!g.done;g=m.next()){var h=g.value;h.push(i),t<=h.length&&(s=s??[],s.push(h))}}catch(w){o={error:w}}finally{try{g&&!g.done&&(f=m.return)&&f.call(m)}finally{if(o)throw o.error}}if(s)try{for(var y=k(s),p=y.next();!p.done;p=y.next()){var h=p.value;$(a,h),r.next(h)}}catch(w){l={error:w}}finally{try{p&&!p.done&&(c=y.return)&&c.call(y)}finally{if(l)throw l.error}}},function(){var i,o;try{for(var f=k(a),l=f.next();!l.done;l=f.next()){var c=l.value;r.next(c)}}catch(s){i={error:s}}finally{try{l&&!l.done&&(o=f.return)&&o.call(f)}finally{if(i)throw i.error}}r.complete()},void 0,function(){a=null}))})}function jt(t){for(var n,e,r=[],a=1;a<arguments.length;a++)r[a-1]=arguments[a];var u=(n=M(r))!==null&&n!==void 0?n:V,i=(e=r[0])!==null&&e!==void 0?e:null,o=r[1]||1/0;return d(function(f,l){var c=[],s=!1,m=function(y){var p=y.buffer,w=y.subs;w.unsubscribe(),$(c,y),l.next(p),s&&g()},g=function(){if(c){var y=new F;l.add(y);var p=[],w={buffer:p,subs:y};c.push(w),C(y,u,function(){return m(w)},t)}};i!==null&&i>=0?C(l,u,g,i,!0):s=!0,g();var h=v(l,function(y){var p,w,b=c.slice();try{for(var j=k(b),D=j.next();!D.done;D=j.next()){var tn=D.value,en=tn.buffer;en.push(y),o<=en.length&&m(tn)}}catch(jn){p={error:jn}}finally{try{D&&!D.done&&(w=j.return)&&w.call(j)}finally{if(p)throw p.error}}},function(){for(;c!=null&&c.length;)l.next(c.shift().buffer);h==null||h.unsubscribe(),l.complete(),l.unsubscribe()},void 0,function(){return c=null});f.subscribe(h)})}function kt(t,n){return d(function(e,r){var a=[];x(t).subscribe(v(r,function(u){var i=[];a.push(i);var o=new F,f=function(){$(a,i),r.next(i),o.unsubscribe()};o.add(x(n(u)).subscribe(v(r,f,E)))},E)),e.subscribe(v(r,function(u){var i,o;try{for(var f=k(a),l=f.next();!l.done;l=f.next()){var c=l.value;c.push(u)}}catch(s){i={error:s}}finally{try{l&&!l.done&&(o=f.return)&&o.call(f)}finally{if(i)throw i.error}}},function(){for(;a.length>0;)r.next(a.shift());r.complete()}))})}function Wt(t){return d(function(n,e){var r=null,a=null,u=function(){a==null||a.unsubscribe();var i=r;r=[],i&&e.next(i),x(t()).subscribe(a=v(e,u,E))};u(),n.subscribe(v(e,function(i){return r==null?void 0:r.push(i)},function(){r&&e.next(r),e.complete()},void 0,function(){return r=a=null}))})}function it(t){return d(function(n,e){var r=null,a=!1,u;r=n.subscribe(v(e,void 0,void 0,function(i){u=x(t(i,it(t)(n))),r?(r.unsubscribe(),r=null,u.subscribe(e)):a=!0})),a&&(r.unsubscribe(),r=null,u.subscribe(e))})}function bn(t,n,e,r,a){return function(u,i){var o=e,f=n,l=0;u.subscribe(v(i,function(c){var s=l++;f=o?t(f,c,s):(o=!0,c),r&&i.next(f)},a&&function(){o&&i.next(f),i.complete()}))}}function K(t,n){return d(bn(t,n,arguments.length>=2,!1,!0))}var ot=function(t,n){return t.push(n),t};function ft(){return d(function(t,n){K(ot,[])(t).subscribe(n)})}function En(t,n){return sn(ft(),W(function(e){return t(e)}),n?O(n):I)}function lt(t){return En(tt,t)}var Ft=lt;function Sn(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];var e=Y(t);return e?sn(Sn.apply(void 0,T([],N(t))),O(e)):d(function(r,a){yn(T([r],N(z(t))))(a)})}function Ct(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];return Sn.apply(void 0,T([],N(t)))}function on(t,n){return A(n)?W(t,n,1):W(t,1)}function Mt(t,n){return A(n)?on(function(){return t},n):on(function(){return t})}function ct(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];var e=M(t);return d(function(r,a){Fn()(U(T([r],N(t)),e)).subscribe(a)})}function Lt(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];return ct.apply(void 0,T([],N(t)))}function vt(t){return new R(function(n){return t.subscribe(n)})}var st={connector:function(){return new S}};function An(t,n){n===void 0&&(n=st);var e=n.connector;return d(function(r,a){var u=e();x(t(vt(u))).subscribe(a),a.add(r.subscribe(u))})}function zt(t){return K(function(n,e,r){return!t||t(e,r)?n+1:n},0)}function Dt(t){return d(function(n,e){var r=!1,a=null,u=null,i=function(){if(u==null||u.unsubscribe(),u=null,r){r=!1;var o=a;a=null,e.next(o)}};n.subscribe(v(e,function(o){u==null||u.unsubscribe(),r=!0,a=o,u=v(e,i,E),x(t(o)).subscribe(u)},function(){i(),e.complete()},void 0,function(){a=u=null}))})}function _(t){return d(function(n,e){var r=!1;n.subscribe(v(e,function(a){r=!0,e.next(a)},function(){r||e.next(t),e.complete()}))})}function dt(){return d(function(t,n){t.subscribe(v(n,E))})}function Tn(t,n){return n?function(e){return dn(n.pipe(q(1),dt()),e.pipe(Tn(t)))}:W(function(e,r){return x(t(e,r)).pipe(q(1),Cn(e))})}function Ut(t,n){n===void 0&&(n=V);var e=L(t,n);return Tn(function(){return e})}function $t(){return d(function(t,n){t.subscribe(v(n,function(e){return gn(e,n)}))})}function qt(t,n){return d(function(e,r){var a=new Set;e.subscribe(v(r,function(u){var i=t?t(u):u;a.has(i)||(a.add(i),r.next(u))})),n&&x(n).subscribe(v(r,function(){return a.clear()},E))})}function nn(t){return t===void 0&&(t=ht),d(function(n,e){var r=!1;n.subscribe(v(e,function(a){r=!0,e.next(a)},function(){return r?e.complete():e.error(t())}))})}function ht(){return new J}function Pt(t,n){if(t<0)throw new an;var e=arguments.length>=2;return function(r){return r.pipe(Q(function(a,u){return u===t}),q(1),e?_(n):nn(function(){return new an}))}}function Bt(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];return function(e){return dn(e,mn.apply(void 0,T([],N(t))))}}function Gt(t,n){return d(function(e,r){var a=0;e.subscribe(v(r,function(u){t.call(n,u,a++,e)||(r.next(!1),r.complete())},function(){r.next(!0),r.complete()}))})}function Nn(t,n){return n?function(e){return e.pipe(Nn(function(r,a){return x(t(r,a)).pipe(hn(function(u,i){return n(r,u,a,i)}))}))}:d(function(e,r){var a=0,u=null,i=!1;e.subscribe(v(r,function(o){u||(u=v(r,void 0,function(){u=null,i&&r.complete()}),x(t(o,a++)).subscribe(u))},function(){i=!0,!u&&r.complete()}))})}function pt(){return Nn(I)}var Jt=pt;function Kt(t,n,e){return n===void 0&&(n=1/0),n=(n||0)<1?1/0:n,d(function(r,a){return pn(r,a,t,n,void 0,!0,e)})}function Xt(t){return d(function(n,e){try{n.subscribe(e)}finally{e.add(t)}})}function Yt(t,n){return d(In(t,n,"value"))}function In(t,n,e){var r=e==="index";return function(a,u){var i=0;a.subscribe(v(u,function(o){var f=i++;t.call(n,o,f,a)&&(u.next(r?f:o),u.complete())},function(){u.next(r?-1:void 0),u.complete()}))}}function Ot(t,n){return d(In(t,n,"index"))}function Qt(t,n){var e=arguments.length>=2;return function(r){return r.pipe(t?Q(function(a,u){return t(a,u,r)}):I,q(1),e?_(n):nn(function(){return new J}))}}function Zt(t,n,e,r){return d(function(a,u){var i;!n||typeof n=="function"?i=n:(e=n.duration,i=n.element,r=n.connector);var o=new Map,f=function(h){o.forEach(h),h(u)},l=function(h){return f(function(y){return y.error(h)})},c=0,s=!1,m=new vn(u,function(h){try{var y=t(h),p=o.get(y);if(!p){o.set(y,p=r?r():new S);var w=g(y,p);if(u.next(w),e){var b=v(p,function(){p.complete(),b==null||b.unsubscribe()},void 0,void 0,function(){return o.delete(y)});m.add(x(e(w)).subscribe(b))}}p.next(i?i(h):h)}catch(j){l(j)}},function(){return f(function(h){return h.complete()})},l,function(){return o.clear()},function(){return s=!0,c===0});a.subscribe(m);function g(h,y){var p=new R(function(w){c++;var b=y.subscribe(w);return function(){b.unsubscribe(),--c===0&&s&&m.unsubscribe()}});return p.key=h,p}})}function Ht(){return d(function(t,n){t.subscribe(v(n,function(){n.next(!1),n.complete()},function(){n.next(!0),n.complete()}))})}function mt(t){return t<=0?function(){return B}:d(function(n,e){var r=[];n.subscribe(v(e,function(a){r.push(a),t<r.length&&r.shift()},function(){var a,u;try{for(var i=k(r),o=i.next();!o.done;o=i.next()){var f=o.value;e.next(f)}}catch(l){a={error:l}}finally{try{o&&!o.done&&(u=i.return)&&u.call(i)}finally{if(a)throw a.error}}e.complete()},void 0,function(){r=null}))})}function _t(t,n){var e=arguments.length>=2;return function(r){return r.pipe(t?Q(function(a,u){return t(a,u,r)}):I,mt(1),e?_(n):nn(function(){return new J}))}}function ne(){return d(function(t,n){t.subscribe(v(n,function(e){n.next(X.createNext(e))},function(){n.next(X.createComplete()),n.complete()},function(e){n.next(X.createError(e)),n.complete()}))})}function te(t){return K(A(t)?function(n,e){return t(n,e)>0?n:e}:function(n,e){return n>e?n:e})}var ee=W;function re(t,n,e){return e===void 0&&(e=1/0),A(n)?W(function(){return t},n,e):(typeof n=="number"&&(e=n),W(function(){return t},e))}function ae(t,n,e){return e===void 0&&(e=1/0),d(function(r,a){var u=n;return pn(r,a,function(i,o){return t(u,i,o)},e,function(i){u=i},!1,void 0,function(){return u=null})})}function gt(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];var e=M(t),r=Mn(t,1/0);return t=z(t),d(function(a,u){Ln(r)(U(T([a],N(t)),e)).subscribe(u)})}function ue(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];return gt.apply(void 0,T([],N(t)))}function ie(t){return K(A(t)?function(n,e){return t(n,e)<0?n:e}:function(n,e){return n<e?n:e})}function Vn(t,n){var e=A(t)?t:function(){return t};return A(n)?An(n,{connector:e}):function(r){return new Z(r,e)}}function yt(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];var e=z(t);return function(r){return at.apply(void 0,T([r],N(e)))}}var oe=yt;function fe(){return d(function(t,n){var e,r=!1;t.subscribe(v(n,function(a){var u=e;e=a,r&&n.next([u,a]),r=!0}))})}function le(t){return t?function(n){return An(t)(n)}:function(n){return Vn(new S)(n)}}function ce(t){return function(n){var e=new zn(t);return new Z(n,function(){return e})}}function ve(){return function(t){var n=new qn;return new Z(t,function(){return n})}}function se(t,n,e,r){e&&!A(e)&&(r=e);var a=A(e)?e:void 0;return function(u){return Vn(new Dn(t,n,r),a)(u)}}function de(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];return t.length?d(function(e,r){wn(T([e],N(t)))(r)}):I}function he(t){var n,e=1/0,r;return t!=null&&(typeof t=="object"?(n=t.count,e=n===void 0?1/0:n,r=t.delay):e=t),e<=0?function(){return B}:d(function(a,u){var i=0,o,f=function(){if(o==null||o.unsubscribe(),o=null,r!=null){var c=typeof r=="number"?L(r):x(r(i)),s=v(u,function(){s.unsubscribe(),l()});c.subscribe(s)}else l()},l=function(){var c=!1;o=a.subscribe(v(u,void 0,function(){++i<e?o?f():c=!0:u.complete()})),c&&f()};l()})}function pe(t){return d(function(n,e){var r,a=!1,u,i=!1,o=!1,f=function(){return o&&i&&(e.complete(),!0)},l=function(){return u||(u=new S,x(t(u)).subscribe(v(e,function(){r?c():a=!0},function(){i=!0,f()}))),u},c=function(){o=!1,r=n.subscribe(v(e,void 0,function(){o=!0,!f()&&l().next()})),a&&(r.unsubscribe(),r=null,a=!1,c())};c()})}function me(t){t===void 0&&(t=1/0);var n;t&&typeof t=="object"?n=t:n={count:t};var e=n.count,r=e===void 0?1/0:e,a=n.delay,u=n.resetOnSuccess,i=u===void 0?!1:u;return r<=0?I:d(function(o,f){var l=0,c,s=function(){var m=!1;c=o.subscribe(v(f,function(g){i&&(l=0),f.next(g)},void 0,function(g){if(l++<r){var h=function(){c?(c.unsubscribe(),c=null,s()):m=!0};if(a!=null){var y=typeof a=="number"?L(a):x(a(g,l)),p=v(f,function(){p.unsubscribe(),h()},function(){f.complete()});y.subscribe(p)}else h()}else f.error(g)})),m&&(c.unsubscribe(),c=null,s())};s()})}function ge(t){return d(function(n,e){var r,a=!1,u,i=function(){r=n.subscribe(v(e,void 0,void 0,function(o){u||(u=new S,x(t(u)).subscribe(v(e,function(){return r?i():a=!0}))),u&&u.next(o)})),a&&(r.unsubscribe(),r=null,a=!1,i())};i()})}function wt(t){return d(function(n,e){var r=!1,a=null;n.subscribe(v(e,function(u){r=!0,a=u})),x(t).subscribe(v(e,function(){if(r){r=!1;var u=a;a=null,e.next(u)}},E))})}function ye(t,n){return n===void 0&&(n=V),wt(et(t,n))}function we(t,n){return d(bn(t,n,arguments.length>=2,!0))}function xe(t,n){return n===void 0&&(n=function(e,r){return e===r}),d(function(e,r){var a=fn(),u=fn(),i=function(f){r.next(f),r.complete()},o=function(f,l){var c=v(r,function(s){var m=l.buffer,g=l.complete;m.length===0?g?i(!1):f.buffer.push(s):!n(s,m.shift())&&i(!1)},function(){f.complete=!0;var s=l.complete,m=l.buffer;s&&i(m.length===0),c==null||c.unsubscribe()});return c};e.subscribe(o(a,u)),x(t).subscribe(o(u,a))})}function fn(){return{buffer:[],complete:!1}}function be(t){return d(function(n,e){var r=!1,a,u=!1,i=0;n.subscribe(v(e,function(o){u=!0,(!t||t(o,i++,n))&&(r&&e.error(new Gn("Too many matching values")),r=!0,a=o)},function(){r?(e.next(a),e.complete()):e.error(u?new Bn("No matching values"):new J)}))})}function Ee(t){return t<=0?I:d(function(n,e){var r=new Array(t),a=0;return n.subscribe(v(e,function(u){var i=a++;if(i<t)r[i]=u;else{var o=i%t,f=r[o];r[o]=u,e.next(f)}})),function(){r=null}})}function Se(t){return d(function(n,e){var r=!1,a=v(e,function(){a==null||a.unsubscribe(),r=!0},E);x(t).subscribe(a),n.subscribe(v(e,function(u){return r&&e.next(u)}))})}function Ae(t){return d(function(n,e){var r=!1,a=0;n.subscribe(v(e,function(u){return(r||(r=!t(u,a++)))&&e.next(u)}))})}function Te(){return P(I)}function Ne(t,n){return A(n)?P(function(){return t},n):P(function(){return t})}function Ie(t,n){return d(function(e,r){var a=n;return P(function(u,i){return t(a,u,i)},function(u,i){return a=i,i})(e).subscribe(r),function(){a=null}})}function Ve(t,n){return n===void 0&&(n=!1),d(function(e,r){var a=0;e.subscribe(v(r,function(u){var i=t(u,a++);(i||n)&&r.next(u),!i&&r.complete()}))})}function Re(t,n,e){var r=A(t)||n||e?{next:t,error:n,complete:e}:t;return r?d(function(a,u){var i;(i=r.subscribe)===null||i===void 0||i.call(r);var o=!0;a.subscribe(v(u,function(f){var l;(l=r.next)===null||l===void 0||l.call(r,f),u.next(f)},function(){var f;o=!1,(f=r.complete)===null||f===void 0||f.call(r),u.complete()},function(f){var l;o=!1,(l=r.error)===null||l===void 0||l.call(r,f),u.error(f)},function(){var f,l;o&&((f=r.unsubscribe)===null||f===void 0||f.call(r)),(l=r.finalize)===null||l===void 0||l.call(r)}))}):I}var Rn={leading:!0,trailing:!1};function xt(t,n){return n===void 0&&(n=Rn),d(function(e,r){var a=n.leading,u=n.trailing,i=!1,o=null,f=null,l=!1,c=function(){f==null||f.unsubscribe(),f=null,u&&(g(),l&&r.complete())},s=function(){f=null,l&&r.complete()},m=function(h){return f=x(t(h)).subscribe(v(r,c,s))},g=function(){if(i){i=!1;var h=o;o=null,r.next(h),!l&&m(h)}};e.subscribe(v(r,function(h){i=!0,o=h,!(f&&!f.closed)&&(a?g():m(h))},function(){l=!0,!(u&&i&&f&&!f.closed)&&r.complete()}))})}function je(t,n,e){n===void 0&&(n=V),e===void 0&&(e=Rn);var r=L(t,n);return xt(function(){return r},e)}function ke(t){return t===void 0&&(t=V),d(function(n,e){var r=t.now();n.subscribe(v(e,function(a){var u=t.now(),i=u-r;r=u,e.next(new bt(a,i))}))})}var bt=function(){function t(n,e){this.value=n,this.interval=e}return t}();function We(t,n,e){var r,a,u;if(e=e??ln,H(t)?r=t:typeof t=="number"&&(a=t),n)u=function(){return n};else throw new TypeError("No observable provided to switch to");if(r==null&&a==null)throw new TypeError("No timeout provided.");return Kn({first:r,each:a,scheduler:e,with:u})}function Fe(t){return t===void 0&&(t=Un),hn(function(n){return{value:n,timestamp:t.now()}})}function Ce(t){return d(function(n,e){var r=new S;e.next(r.asObservable());var a=function(u){r.error(u),e.error(u)};return n.subscribe(v(e,function(u){return r==null?void 0:r.next(u)},function(){r.complete(),e.complete()},a)),x(t).subscribe(v(e,function(){r.complete(),e.next(r=new S)},E,a)),function(){r==null||r.unsubscribe(),r=null}})}function Me(t,n){n===void 0&&(n=0);var e=n>0?n:t;return d(function(r,a){var u=[new S],i=[],o=0;a.next(u[0].asObservable()),r.subscribe(v(a,function(f){var l,c;try{for(var s=k(u),m=s.next();!m.done;m=s.next()){var g=m.value;g.next(f)}}catch(p){l={error:p}}finally{try{m&&!m.done&&(c=s.return)&&c.call(s)}finally{if(l)throw l.error}}var h=o-t+1;if(h>=0&&h%e===0&&u.shift().complete(),++o%e===0){var y=new S;u.push(y),a.next(y.asObservable())}},function(){for(;u.length>0;)u.shift().complete();a.complete()},function(f){for(;u.length>0;)u.shift().error(f);a.error(f)},function(){i=null,u=null}))})}function Le(t){for(var n,e,r=[],a=1;a<arguments.length;a++)r[a-1]=arguments[a];var u=(n=M(r))!==null&&n!==void 0?n:V,i=(e=r[0])!==null&&e!==void 0?e:null,o=r[1]||1/0;return d(function(f,l){var c=[],s=!1,m=function(p){var w=p.window,b=p.subs;w.complete(),b.unsubscribe(),$(c,p),s&&g()},g=function(){if(c){var p=new F;l.add(p);var w=new S,b={window:w,subs:p,seen:0};c.push(b),l.next(w.asObservable()),C(p,u,function(){return m(b)},t)}};i!==null&&i>=0?C(l,u,g,i,!0):s=!0,g();var h=function(p){return c.slice().forEach(p)},y=function(p){h(function(w){var b=w.window;return p(b)}),p(l),l.unsubscribe()};return f.subscribe(v(l,function(p){h(function(w){w.window.next(p),o<=++w.seen&&m(w)})},function(){return y(function(p){return p.complete()})},function(p){return y(function(w){return w.error(p)})})),function(){c=null}})}function ze(t,n){return d(function(e,r){var a=[],u=function(i){for(;0<a.length;)a.shift().error(i);r.error(i)};x(t).subscribe(v(r,function(i){var o=new S;a.push(o);var f=new F,l=function(){$(a,o),o.complete(),f.unsubscribe()},c;try{c=x(n(i))}catch(s){u(s);return}r.next(o.asObservable()),f.add(c.subscribe(v(r,l,E,u)))},E)),e.subscribe(v(r,function(i){var o,f,l=a.slice();try{for(var c=k(l),s=c.next();!s.done;s=c.next()){var m=s.value;m.next(i)}}catch(g){o={error:g}}finally{try{s&&!s.done&&(f=c.return)&&f.call(c)}finally{if(o)throw o.error}}},function(){for(;0<a.length;)a.shift().complete();r.complete()},u,function(){for(;0<a.length;)a.shift().unsubscribe()}))})}function De(t){return d(function(n,e){var r,a,u=function(o){r.error(o),e.error(o)},i=function(){a==null||a.unsubscribe(),r==null||r.complete(),r=new S,e.next(r.asObservable());var o;try{o=x(t())}catch(f){u(f);return}o.subscribe(a=v(e,i,i,u))};i(),n.subscribe(v(e,function(o){return r.next(o)},function(){r.complete(),e.complete()},u,function(){a==null||a.unsubscribe(),r=null}))})}function Ue(t){return En(xn,t)}function Et(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];return d(function(e,r){xn.apply(void 0,T([e],N(t))).subscribe(r)})}function $e(){for(var t=[],n=0;n<arguments.length;n++)t[n]=arguments[n];return Et.apply(void 0,T([],N(t)))}export{dt as $,qn as A,Mt as B,Z as C,Lt as D,An as E,zt as F,Dt as G,_ as H,Ut as I,Tn as J,$t as K,qt as L,Pt as M,X as N,Bt as O,Gt as P,Jt as Q,pt as R,Gn as S,Jn as T,Nn as U,Kt as V,Xt as W,Yt as X,Ot as Y,Qt as Z,Zt as _,Hn as a,Ht as a0,_t as a1,ne as a2,te as a3,ee as a4,re as a5,ae as a6,ue as a7,ie as a8,Vn as a9,Re as aA,xt as aB,je as aC,nn as aD,ke as aE,Kn as aF,We as aG,Fe as aH,ft as aI,Ce as aJ,Me as aK,Le as aL,ze as aM,De as aN,Ue as aO,$e as aP,z as aQ,Sn as aR,ct as aS,gt as aT,oe as aU,Et as aV,yt as aa,fe as ab,le as ac,ce as ad,ve as ae,se as af,de as ag,K as ah,he as ai,pe as aj,me as ak,ge as al,$n as am,wt as an,ye as ao,we as ap,xe as aq,be as ar,Ee as as,Se as at,Ae as au,Te as av,Ne as aw,Ie as ax,mt as ay,Ve as az,rn as b,nt as c,an as d,Bn as e,tt as f,at as g,L as h,et as i,ut as j,It as k,Vt as l,Rt as m,Tt as n,mn as o,jt as p,kt as q,Nt as r,Wt as s,Pn as t,it as u,Ft as v,lt as w,Ct as x,on as y,xn as z};
