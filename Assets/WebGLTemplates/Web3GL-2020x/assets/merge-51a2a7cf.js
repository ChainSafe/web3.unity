import { aP as popScheduler, aW as popNumber, aj as EMPTY, ab as innerFrom, aD as mergeAll, ah as from } from "./index-cfcd831a.js";
function merge() {
  var args = [];
  for (var _i = 0; _i < arguments.length; _i++) {
    args[_i] = arguments[_i];
  }
  var scheduler = popScheduler(args);
  var concurrent = popNumber(args, Infinity);
  var sources = args;
  return !sources.length ? EMPTY : sources.length === 1 ? innerFrom(sources[0]) : mergeAll(concurrent)(from(sources, scheduler));
}
export {
  merge as m
};
