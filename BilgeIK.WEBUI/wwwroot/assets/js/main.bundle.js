"use strict";

if (!Element.prototype.matches) {
  Element.prototype.matches = Element.prototype.msMatchesSelector || Element.prototype.webkitMatchesSelector;
}

if (!Element.prototype.closest) {
  if (!Element.prototype.matches) {
    Element.prototype.matches = Element.prototype.msMatchesSelector || Element.prototype.webkitMatchesSelector;
  }

  Element.prototype.closest = function (s) {
    var el = this;
    var ancestor = this;
    if (!document.documentElement.contains(el)) return null;

    do {
      if (ancestor.matches(s)) return ancestor;
      ancestor = ancestor.parentElement;
    } while (ancestor !== null);

    return null;
  };
}
/*
 * ChildNode.remove() polyfill
 */


(function (elem) {
  for (var i = 0; i < elem.length; i++) {
    if (!window[elem[i]] || "remove" in window[elem[i]].prototype) continue;

    window[elem[i]].prototype.remove = function () {
      this.parentNode.removeChild(this);
    };
  }
})(["Element", "CharacterData", "DocumentType"]); //  http://paulirish.com/2011/requestanimationframe-for-smart-animating/
//  http://my.opera.com/emoller/blog/2011/12/20/requestanimationframe-for-smart-er-animating
//  MIT license
//


(function () {
  var lastTime = 0;
  var vendors = ["webkit", "moz"];

  for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
    window.requestAnimationFrame = window[vendors[x] + "RequestAnimationFrame"];
    window.cancelAnimationFrame = window[vendors[x] + "CancelAnimationFrame"] || window[vendors[x] + "CancelRequestAnimationFrame"];
  }

  if (!window.requestAnimationFrame) window.requestAnimationFrame = function (callback) {
    var currTime = new Date().getTime();
    var timeToCall = Math.max(0, 16 - (currTime - lastTime));
    var id = window.setTimeout(function () {
      callback(currTime + timeToCall);
    }, timeToCall);
    lastTime = currTime + timeToCall;
    return id;
  };
  if (!window.cancelAnimationFrame) window.cancelAnimationFrame = function (id) {
    clearTimeout(id);
  };
})(); // https://github.com/jserz/js_piece/blob/master/DOM/ParentNode/prepend()/prepend().md


(function (arr) {
  arr.forEach(function (item) {
    if (item.hasOwnProperty("prepend")) {
      return;
    }

    Object.defineProperty(item, "prepend", {
      configurable: true,
      enumerable: true,
      writable: true,
      value: function prepend() {
        var argArr = Array.prototype.slice.call(arguments),
            docFrag = document.createDocumentFragment();
        argArr.forEach(function (argItem) {
          var isNode = argItem instanceof Node;
          docFrag.appendChild(isNode ? argItem : document.createTextNode(String(argItem)));
        });
        this.insertBefore(docFrag, this.firstChild);
      }
    });
  });
})([Element.prototype, Document.prototype, DocumentFragment.prototype]); // UL Utils Global variables


window.ULUtilElementDataStore = {};
window.ULUtilElementDataStoreID = 0;
window.ULUtilDelegatedEventHandlers = {};

var ULUtil = function () {
  var resizeHandlers = [];
  /** @type {object} breakpoints The device width breakpoints **/

  var breakpoints = {
    sm: 544,
    // Small screen / phone
    md: 768,
    // Medium screen / tablet
    lg: 1024,
    // Large screen / desktop
    xl: 1200 // Extra large screen / wide desktop

  };
  /**
   * Handle window resize event with some
   * delay to attach event handlers upon resize complete
   */

  var _windowResizeHandler = function () {
    var _runResizeHandlers = function () {
      // reinitialize other subscribed elements
      for (var i = 0; i < resizeHandlers.length; i++) {
        var each = resizeHandlers[i];
        each.call();
      }
    };

    var timeout = false; // holder for timeout id

    var delay = 250; // delay after event is "complete" to run callback

    window.addEventListener("resize", function () {
      clearTimeout(timeout);
      timeout = setTimeout(function () {
        _runResizeHandlers();
      }, delay); // wait 50ms until window resize finishes.
    });
  };

  return {
    /**
     * Main initializer.
     * @param {object} options.
     * @returns null
     */
    //main function to initiate the theme
    init: function (options) {
      if (options && options.breakpoints) {
        breakpoints = options.breakpoints;
      }

      _windowResizeHandler();
    },

    /**
     * @param {function} callback function.
     */
    addResizeHandler: function (callback) {
      resizeHandlers.push(callback);
    },

    /**
     * @param {function} callback function.
     */
    removeResizeHandler: function (callback) {
      for (var i = 0; i < resizeHandlers.length; i++) {
        if (callback === resizeHandlers[i]) {
          delete resizeHandlers[i];
        }
      }
    },
    runResizeHandlers: function () {
      _runResizeHandlers();
    },
    resize: function () {
      if (typeof Event === "function") {
        // modern browsers
        window.dispatchEvent(new Event("resize"));
      } else {
        // IE and other old browsers
        var evt = window.document.createEvent("UIEvents");
        evt.initUIEvent("resize", true, false, window, 0);
        window.dispatchEvent(evt);
      }
    },

    /**
     * @param {function} func Parameter name.
     * @param {number} timeout Parameter name.
     * @returns {function}
     */
    debounce: function (func, timeout) {
      let timer;
      return (...args) => {
        const next = () => func(...args);

        if (timer) {
          clearTimeout(timer);
        }

        timer = setTimeout(next, timeout > 0 ? timeout : 300);
      };
    },

    /**
     * @param {string} paramName Parameter name.
     * @returns {string}
     */
    getURLParam: function (paramName) {
      var searchString = window.location.search.substring(1),
          i,
          val,
          params = searchString.split("&");

      for (i = 0; i < params.length; i++) {
        val = params[i].split("=");

        if (val[0] == paramName) {
          return unescape(val[1]);
        }
      }

      return null;
    },
    isMobile: function () {
      return this.getViewPort().width <= this.getBreakpoint("lg") ? true : false;
    },

    /**
     * @returns {boolean}
     */
    isDesktop: function () {
      return ULUtil.isMobileDevice() ? false : true;
    },

    /**
     * http://andylangton.co.uk/articles/javascript/get-viewport-size-javascript/
     * @returns {object}
     */
    getViewPort: function () {
      var e = window,
          a = "inner";

      if (!("innerWidth" in window)) {
        a = "client";
        e = document.documentElement || document.body;
      }

      return {
        width: e[a + "Width"],
        height: e[a + "Height"]
      };
    },

    /**
     * @param {string} mode Responsive mode name(e.g: desktop,
     *     desktop-and-tablet, tablet, tablet-and-mobile, mobile)
     * @returns {boolean}
     */
    isInResponsiveRange: function (mode) {
      var breakpoint = this.getViewPort().width;

      if (mode == "general") {
        return true;
      } else if (mode == "desktop" && breakpoint >= this.getBreakpoint("lg") + 1) {
        return true;
      } else if (mode == "tablet" && breakpoint >= this.getBreakpoint("md") + 1 && breakpoint < this.getBreakpoint("lg")) {
        return true;
      } else if (mode == "mobile" && breakpoint <= this.getBreakpoint("md")) {
        return true;
      } else if (mode == "desktop-and-tablet" && breakpoint >= this.getBreakpoint("md") + 1) {
        return true;
      } else if (mode == "tablet-and-mobile" && breakpoint <= this.getBreakpoint("lg")) {
        return true;
      } else if (mode == "minimal-desktop-and-below" && breakpoint <= this.getBreakpoint("xl")) {
        return true;
      }

      return false;
    },

    /**
     * @param {string} prefix Prefix for generated ID
     * @returns {boolean}
     */
    getUniqueID: function (prefix) {
      return prefix + Math.floor(Math.random() * new Date().getTime());
    },

    /**
     * Gets window width for give breakpoint mode.
     * @param {string} mode Responsive mode name(e.g: xl, lg, md, sm)
     * @returns {number}
     */
    getBreakpoint: function (mode) {
      return breakpoints[mode];
    },

    /**
     * Checks whether object has property matchs given key path.
     * @param {object} obj Object contains values paired with given key path
     * @param {string} keys Keys path seperated with dots
     * @returns {object}
     */
    isset: function (obj, keys) {
      var stone;
      keys = keys || "";

      if (keys.indexOf("[") !== -1) {
        throw new Error("Unsupported object path notation.");
      }

      keys = keys.split(".");

      do {
        if (obj === undefined) {
          return false;
        }

        stone = keys.shift();

        if (!obj.hasOwnProperty(stone)) {
          return false;
        }

        obj = obj[stone];
      } while (keys.length);

      return true;
    },

    /**
     * Gets highest z-index of the given element parents
     * @param {object} el jQuery element object
     * @returns {number}
     */
    getHighestZindex: function (el) {
      var elem = ULUtil.get(el),
          position,
          value;

      while (elem && elem !== document) {
        position = ULUtil.css(elem, "position");

        if (position === "absolute" || position === "relative" || position === "fixed") {
          value = parseInt(ULUtil.css(elem, "z-index"));

          if (!isNaN(value) && value !== 0) {
            return value;
          }
        }

        elem = elem.parentNode;
      }

      return null;
    },

    /**
     * Checks whether the element has any parent with fixed positionfreg
     * @param {object} el jQuery element object
     * @returns {boolean}
     */
    hasFixedPositionedParent: function (el) {
      var position;

      while (el && el !== document) {
        position = ULUtil.css(el, "position");

        if (position === "fixed") {
          return true;
        }

        el = el.parentNode;
      }

      return false;
    },

    /**
     * Generate random integer value within given min and max range
     * @param {number} min Range start value
     * @param {number} max Range end value
     * @returns {number}
     */
    getRandomInt: function (min, max) {
      return Math.floor(Math.random() * (max - min + 1)) + min;
    },

    /**
     * Delay
     */
    sleep: function (milliseconds) {
      var start = new Date().getTime();

      for (var i = 0; i < 1e7; i++) {
        if (new Date().getTime() - start > milliseconds) {
          break;
        }
      }
    },
    // Deep extend:  $.extend(true, {}, objA, objB);
    deepExtend: function (out) {
      out = out || {};

      for (var i = 1; i < arguments.length; i++) {
        var obj = arguments[i];
        if (!obj) continue;

        for (var key in obj) {
          if (obj.hasOwnProperty(key)) {
            if (typeof obj[key] === "object") out[key] = ULUtil.deepExtend(out[key], obj[key]);else out[key] = obj[key];
          }
        }
      }

      return out;
    },
    // extend:  $.extend({}, objA, objB);
    extend: function (out) {
      out = out || {};

      for (var i = 1; i < arguments.length; i++) {
        if (!arguments[i]) continue;

        for (var key in arguments[i]) {
          if (arguments[i].hasOwnProperty(key)) out[key] = arguments[i][key];
        }
      }

      return out;
    },
    get: function (query) {
      var el;

      if (query === document) {
        return document;
      }

      if (!!(query && query.nodeType === 1)) {
        return query;
      }

      if (el = document.getElementById(query)) {
        return el;
      } else if (el = document.getElementsByTagName(query)) {
        return el[0];
      } else if (el = document.getElementsByClassName(query)) {
        return el[0];
      } else {
        return null;
      }
    },
    getByID: function (query) {
      if (!!(query && query.nodeType === 1)) {
        return query;
      }

      return document.getElementById(query);
    },
    getByTag: function (query) {
      var el;

      if (el = document.getElementsByTagName(query)) {
        return el[0];
      } else {
        return null;
      }
    },
    getByClass: function (query) {
      var el;

      if (el = document.getElementsByClassName(query)) {
        return el[0];
      } else {
        return null;
      }
    },

    /**
     * Checks whether the element has given classes
     * @param {object} el jQuery element object
     * @param {string} Classes string
     * @returns {boolean}
     */
    hasClasses: function (el, classes) {
      if (!el) {
        return;
      }

      var classesArr = classes.split(" ");

      for (var i = 0; i < classesArr.length; i++) {
        if (ULUtil.hasClass(el, ULUtil.trim(classesArr[i])) == false) {
          return false;
        }
      }

      return true;
    },
    hasClass: function (el, className) {
      if (!el) {
        return;
      }

      return el.classList ? el.classList.contains(className) : new RegExp("\\b" + className + "\\b").test(el.className);
    },
    addClass: function (el, className) {
      if (!el || typeof className === "undefined") {
        return;
      }

      var classNames = className.split(" ");

      if (el.classList) {
        for (var i = 0; i < classNames.length; i++) {
          if (classNames[i] && classNames[i].length > 0) {
            el.classList.add(ULUtil.trim(classNames[i]));
          }
        }
      } else if (!ULUtil.hasClass(el, className)) {
        for (var x = 0; x < classNames.length; x++) {
          el.className += " " + ULUtil.trim(classNames[x]);
        }
      }
    },
    removeClass: function (el, className) {
      if (!el || typeof className === "undefined") {
        return;
      }

      var classNames = className.split(" ");

      if (el.classList) {
        for (var i = 0; i < classNames.length; i++) {
          el.classList.remove(ULUtil.trim(classNames[i]));
        }
      } else if (ULUtil.hasClass(el, className)) {
        for (var x = 0; x < classNames.length; x++) {
          el.className = el.className.replace(new RegExp("\\b" + ULUtil.trim(classNames[x]) + "\\b", "g"), "");
        }
      }
    },
    removeClassByPrefix: function (el, prefix) {
      var regx = new RegExp("\\b" + prefix + "[^ ]*[ ]?\\b", "g");
      el.className = el.className.replace(regx, "");
      return el;
    },
    triggerCustomEvent: function (el, eventName, data) {
      var event;

      if (window.CustomEvent) {
        event = new CustomEvent(eventName, {
          detail: data
        });
      } else {
        event = document.createEvent("CustomEvent");
        event.initCustomEvent(eventName, true, true, data);
      }

      el.dispatchEvent(event);
    },
    triggerEvent: function (node, eventName) {
      // Make sure we use the ownerDocument from the provided node to avoid cross-window problems
      var doc;

      if (node.ownerDocument) {
        doc = node.ownerDocument;
      } else if (node.nodeType == 9) {
        // the node may be the document itself, nodeType 9 = DOCUMENT_NODE
        doc = node;
      } else {
        throw new Error("Invalid node passed to fireEvent: " + node.id);
      }

      if (node.dispatchEvent) {
        // Gecko-style approach (now the standard) takes more work
        var eventClass = ""; // Different events have different event classes.
        // If this switch statement can't map an eventName to an eventClass,
        // the event firing is going to fail.

        switch (eventName) {
          case "click": // Dispatching of 'click' appears to not work correctly in Safari. Use 'mousedown' or 'mouseup' instead.

          case "mouseenter":
          case "mouseleave":
          case "mousedown":
          case "mouseup":
            eventClass = "MouseEvents";
            break;

          case "focus":
          case "change":
          case "blur":
          case "select":
            eventClass = "HTMLEvents";
            break;

          default:
            throw "fireEvent: Couldn't find an event class for event '" + eventName + "'.";
            break;
        }

        var event = doc.createEvent(eventClass);
        var bubbles = eventName == "change" ? false : true;
        event.initEvent(eventName, bubbles, true);
        event.synthetic = true;
        node.dispatchEvent(event, true);
      } else if (node.fireEvent) {
        // IE-old
        var event = doc.createEventObject();
        event.synthetic = true;
        node.fireEvent("on" + eventName, event);
      }
    },
    index: function (elm) {
      elm = ULUtil.get(elm);
      var c = elm.parentNode.children,
          i = 0;

      for (; i < c.length; i++) if (c[i] == elm) return i;
    },
    trim: function (string) {
      return string.trim();
    },
    eventTriggered: function (e) {
      if (e.currentTarget.dataset.triggered) {
        return true;
      } else {
        e.currentTarget.dataset.triggered = true;
        return false;
      }
    },
    remove: function (el) {
      if (el && el.parentNode) {
        el.parentNode.removeChild(el);
      }
    },
    find: function (parent, query) {
      parent = ULUtil.get(parent);

      if (parent) {
        return parent.querySelector(query);
      }
    },
    findAll: function (parent, query) {
      parent = ULUtil.get(parent);

      if (parent) {
        return parent.querySelectorAll(query);
      }
    },
    insertAfter: function (el, referenceNode) {
      return referenceNode.parentNode.insertBefore(el, referenceNode.nextSibling);
    },
    parents: function (elem, selector) {
      // Element.matches() polyfill
      if (!Element.prototype.matches) {
        Element.prototype.matches = Element.prototype.matchesSelector || Element.prototype.mozMatchesSelector || Element.prototype.msMatchesSelector || Element.prototype.oMatchesSelector || Element.prototype.webkitMatchesSelector || function (s) {
          var matches = (this.document || this.ownerDocument).querySelectorAll(s),
              i = matches.length;

          while (--i >= 0 && matches.item(i) !== this) {}

          return i > -1;
        };
      } // Parent array


      var parents = []; // Push each parent element to the array

      for (; elem && elem !== document; elem = elem.parentNode) {
        if (selector) {
          if (elem.matches(selector)) {
            parents.push(elem);
          }

          continue;
        }

        parents.push(elem);
      } // Return parent array


      return parents;
    },
    children: function (el, selector, log) {
      if (!el || !el.childNodes) {
        return;
      }

      var result = [],
          i = 0,
          l = el.childNodes.length;

      for (var i; i < l; ++i) {
        if (el.childNodes[i].nodeType == 1 && ULUtil.matches(el.childNodes[i], selector, log)) {
          result.push(el.childNodes[i]);
        }
      }

      return result;
    },
    child: function (el, selector, log) {
      var children = ULUtil.children(el, selector, log);
      return children ? children[0] : null;
    },
    matches: function (el, selector, log) {
      var p = Element.prototype;

      var f = p.matches || p.webkitMatchesSelector || p.mozMatchesSelector || p.msMatchesSelector || function (s) {
        return [].indexOf.call(document.querySelectorAll(s), this) !== -1;
      };

      if (el && el.tagName) {
        return f.call(el, selector);
      } else {
        return false;
      }
    },
    data: function (element) {
      element = ULUtil.get(element);
      return {
        set: function (name, data) {
          if (element === undefined) {
            return;
          }

          if (element.customDataTag === undefined) {
            window.ULUtilElementDataStoreID++;
            element.customDataTag = window.ULUtilElementDataStoreID;
          }

          if (window.ULUtilElementDataStore[element.customDataTag] === undefined) {
            window.ULUtilElementDataStore[element.customDataTag] = {};
          }

          window.ULUtilElementDataStore[element.customDataTag][name] = data;
        },
        get: function (name) {
          if (element === undefined) {
            return;
          }

          if (element.customDataTag === undefined) {
            return null;
          }

          return this.has(name) ? window.ULUtilElementDataStore[element.customDataTag][name] : null;
        },
        has: function (name) {
          if (element === undefined) {
            return false;
          }

          if (element.customDataTag === undefined) {
            return false;
          }

          return window.ULUtilElementDataStore[element.customDataTag] && window.ULUtilElementDataStore[element.customDataTag][name] ? true : false;
        },
        remove: function (name) {
          if (element && this.has(name)) {
            delete window.ULUtilElementDataStore[element.customDataTag][name];
          }
        }
      };
    },
    outerWidth: function (el, margin) {
      var width;

      if (margin === true) {
        width = parseFloat(el.offsetWidth);
        width += parseFloat(ULUtil.css(el, "margin-left")) + parseFloat(ULUtil.css(el, "margin-right"));
        return parseFloat(width);
      } else {
        width = parseFloat(el.offsetWidth);
        return width;
      }
    },
    offset: function (elem) {
      var rect, win;
      elem = ULUtil.get(elem);

      if (!elem) {
        return;
      } // Return zeros for disconnected and hidden (display: none) elements (gh-2310)
      // Support: IE <=11 only
      // Running getBoundingClientRect on a
      // disconnected node in IE throws an error


      if (!elem.getClientRects().length) {
        return {
          top: 0,
          left: 0
        };
      } // Get document-relative position by adding viewport scroll to viewport-relative gBCR


      rect = elem.getBoundingClientRect();
      win = elem.ownerDocument.defaultView;
      return {
        top: rect.top + win.pageYOffset,
        left: rect.left + win.pageXOffset
      };
    },
    height: function (el) {
      return ULUtil.css(el, "height");
    },
    visible: function (el) {
      return !(el.offsetWidth === 0 && el.offsetHeight === 0);
    },
    attr: function (el, name, value) {
      el = ULUtil.get(el);

      if (el == undefined) {
        return;
      }

      if (value !== undefined) {
        el.setAttribute(name, value);
      } else {
        return el.getAttribute(name);
      }
    },
    hasAttr: function (el, name) {
      el = ULUtil.get(el);

      if (el == undefined) {
        return;
      }

      return el.getAttribute(name) ? true : false;
    },
    removeAttr: function (el, name) {
      el = ULUtil.get(el);

      if (el == undefined) {
        return;
      }

      el.removeAttribute(name);
    },
    animate: function (from, to, duration, update, easing, done) {
      /**
       * TinyAnimate.easings
       *  Adapted from jQuery Easing
       */
      var easings = {};
      var easing;

      easings.linear = function (t, b, c, d) {
        return c * t / d + b;
      };

      easing = easings.linear; // Early bail out if called incorrectly

      if (typeof from !== "number" || typeof to !== "number" || typeof duration !== "number" || typeof update !== "function") {
        return;
      } // Create mock done() function if necessary


      if (typeof done !== "function") {
        done = function () {};
      } // Pick implementation (requestAnimationFrame | setTimeout)


      var rAF = window.requestAnimationFrame || function (callback) {
        window.setTimeout(callback, 1000 / 50);
      }; // Animation loop


      var canceled = false;
      var change = to - from;

      function loop(timestamp) {
        var time = (timestamp || +new Date()) - start;

        if (time >= 0) {
          update(easing(time, from, change, duration));
        }

        if (time >= 0 && time >= duration) {
          update(to);
          done();
        } else {
          rAF(loop);
        }
      }

      update(from); // Start animation loop

      var start = window.performance && window.performance.now ? window.performance.now() : +new Date();
      rAF(loop);
    },
    actualCss: function (el, prop, cache) {
      el = ULUtil.get(el);
      var css = "";

      if (el instanceof HTMLElement === false) {
        return;
      }

      if (!el.getAttribute("ul-hidden-" + prop) || cache === false) {
        var value; // the element is hidden so:
        // making the el block so we can meassure its height but still be hidden

        css = el.style.cssText;
        el.style.cssText = "position: absolute; visibility: hidden; display: block;";

        if (prop == "width") {
          value = el.offsetWidth;
        } else if (prop == "height") {
          value = el.offsetHeight;
        }

        el.style.cssText = css; // store it in cache

        el.setAttribute("ul-hidden-" + prop, value);
        return parseFloat(value);
      } else {
        // store it in cache
        return parseFloat(el.getAttribute("ul-hidden-" + prop));
      }
    },
    actualHeight: function (el, cache) {
      return ULUtil.actualCss(el, "height", cache);
    },
    actualWidth: function (el, cache) {
      return ULUtil.actualCss(el, "width", cache);
    },
    getScroll: function (element, method) {
      // The passed in `method` value should be 'Top' or 'Left'
      method = "scroll" + method;
      return element == window || element == document ? self[method == "scrollTop" ? "pageYOffset" : "pageXOffset"] || browserSupportsBoxModel && document.documentElement[method] || document.body[method] : element[method];
    },
    css: function (el, styleProp, value) {
      el = ULUtil.get(el);

      if (!el) {
        return;
      }

      if (value !== undefined) {
        el.style[styleProp] = value;
      } else {
        var defaultView = (el.ownerDocument || document).defaultView; // W3C standard way:

        if (defaultView && defaultView.getComputedStyle) {
          styleProp = styleProp.replace(/([A-Z])/g, "-$1").toLowerCase();
          return defaultView.getComputedStyle(el, null).getPropertyValue(styleProp);
        } else if (el.currentStyle) {
          // IE
          styleProp = styleProp.replace(/\-(\w)/g, function (str, letter) {
            return letter.toUpperCase();
          });
          value = el.currentStyle[styleProp];

          if (/^\d+(em|pt|%|ex)?$/i.test(value)) {
            return function (value) {
              var oldLeft = el.style.left,
                  oldRsLeft = el.runtimeStyle.left;
              el.runtimeStyle.left = el.currentStyle.left;
              el.style.left = value || 0;
              value = el.style.pixelLeft + "px";
              el.style.left = oldLeft;
              el.runtimeStyle.left = oldRsLeft;
              return value;
            }(value);
          }

          return value;
        }
      }
    },
    slide: function (el, dir, speed, callback, recalcMaxHeight) {
      if (!el || dir == "up" && ULUtil.visible(el) === false || dir == "down" && ULUtil.visible(el) === true) {
        return;
      }

      speed = speed ? speed : 600;
      var calcHeight = ULUtil.actualHeight(el);
      var calcPaddingTop = false;
      var calcPaddingBottom = false;

      if (ULUtil.css(el, "padding-top") && ULUtil.data(el).has("slide-padding-top") !== true) {
        ULUtil.data(el).set("slide-padding-top", ULUtil.css(el, "padding-top"));
      }

      if (ULUtil.css(el, "padding-bottom") && ULUtil.data(el).has("slide-padding-bottom") !== true) {
        ULUtil.data(el).set("slide-padding-bottom", ULUtil.css(el, "padding-bottom"));
      }

      if (ULUtil.data(el).has("slide-padding-top")) {
        calcPaddingTop = parseInt(ULUtil.data(el).get("slide-padding-top"));
      }

      if (ULUtil.data(el).has("slide-padding-bottom")) {
        calcPaddingBottom = parseInt(ULUtil.data(el).get("slide-padding-bottom"));
      }

      if (dir == "up") {
        // up
        el.style.cssText = "display: block; overflow: hidden;";

        if (calcPaddingTop) {
          ULUtil.animate(0, calcPaddingTop, speed, function (value) {
            el.style.paddingTop = calcPaddingTop - value + "px";
          }, "linear");
        }

        if (calcPaddingBottom) {
          ULUtil.animate(0, calcPaddingBottom, speed, function (value) {
            el.style.paddingBottom = calcPaddingBottom - value + "px";
          }, "linear");
        }

        ULUtil.animate(0, calcHeight, speed, function (value) {
          el.style.height = calcHeight - value + "px";
        }, "linear", function () {
          callback();
          el.style.height = "";
          el.style.display = "none";
        });
      } else if (dir == "down") {
        // down
        el.style.cssText = "display: block; overflow: hidden;";

        if (calcPaddingTop) {
          ULUtil.animate(0, calcPaddingTop, speed, function (value) {
            el.style.paddingTop = value + "px";
          }, "linear", function () {
            el.style.paddingTop = "";
          });
        }

        if (calcPaddingBottom) {
          ULUtil.animate(0, calcPaddingBottom, speed, function (value) {
            el.style.paddingBottom = value + "px";
          }, "linear", function () {
            el.style.paddingBottom = "";
          });
        }

        ULUtil.animate(0, calcHeight, speed, function (value) {
          el.style.height = value + "px";
        }, "linear", function () {
          callback();
          el.style.height = "";
          el.style.display = "";
          el.style.overflow = "";
        });
      }
    },
    slideUp: function (el, speed, callback) {
      ULUtil.slide(el, "up", speed, callback);
    },
    slideDown: function (el, speed, callback) {
      ULUtil.slide(el, "down", speed, callback);
    },
    show: function (el, display) {
      if (typeof el !== "undefined") {
        el.style.display = display ? display : "block";
      }
    },
    hide: function (el) {
      if (typeof el !== "undefined") {
        el.style.display = "none";
      }
    },
    addEvent: function (el, type, handler, one) {
      el = ULUtil.get(el);

      if (typeof el !== "undefined") {
        el.addEventListener(type, handler);
      }
    },
    removeEvent: function (el, type, handler) {
      el = ULUtil.get(el);
      el.removeEventListener(type, handler);
    },
    on: function (element, selector, event, handler) {
      if (!selector) {
        return;
      }

      var eventId = ULUtil.getUniqueID("event");

      window.ULUtilDelegatedEventHandlers[eventId] = function (e) {
        var targets = element.querySelectorAll(selector);
        var target = e.target;

        while (target && target !== element) {
          for (var i = 0, j = targets.length; i < j; i++) {
            if (target === targets[i]) {
              handler.call(target, e);
            }
          }

          target = target.parentNode;
        }
      };

      ULUtil.addEvent(element, event, window.ULUtilDelegatedEventHandlers[eventId]);
      return eventId;
    },
    off: function (element, event, eventId) {
      if (!element || !window.ULUtilDelegatedEventHandlers[eventId]) {
        return;
      }

      ULUtil.removeEvent(element, event, window.ULUtilDelegatedEventHandlers[eventId]);
      delete window.ULUtilDelegatedEventHandlers[eventId];
    },
    one: function onetime(el, type, callback) {
      el = ULUtil.get(el);
      el.addEventListener(type, function callee(e) {
        // remove event
        if (e.target && e.target.removeEventListener) {
          e.target.removeEventListener(e.type, callee);
        } // call handler


        return callback(e);
      });
    },
    hash: function (str) {
      var hash = 0,
          i,
          chr;
      if (str.length === 0) return hash;

      for (i = 0; i < str.length; i++) {
        chr = str.charCodeAt(i);
        hash = (hash << 5) - hash + chr;
        hash |= 0; // Convert to 32bit integer
      }

      return hash;
    },
    animateClass: function (el, animationName, callback) {
      var animation;
      var animations = {
        animation: "animationend",
        OAnimation: "oAnimationEnd",
        MozAnimation: "mozAnimationEnd",
        WebkitAnimation: "webkitAnimationEnd",
        msAnimation: "msAnimationEnd"
      };

      for (var t in animations) {
        if (el.style[t] !== undefined) {
          animation = animations[t];
        }
      }

      ULUtil.addClass(el, "animated " + animationName);
      ULUtil.one(el, animation, function () {
        ULUtil.removeClass(el, "animated " + animationName);
      });

      if (callback) {
        ULUtil.one(el, animation, callback);
      }
    },
    transitionEnd: function (el, callback) {
      var transition;
      var transitions = {
        transition: "transitionend",
        OTransition: "oTransitionEnd",
        MozTransition: "mozTransitionEnd",
        WebkitTransition: "webkitTransitionEnd",
        msTransition: "msTransitionEnd"
      };

      for (var t in transitions) {
        if (el.style[t] !== undefined) {
          transition = transitions[t];
        }
      }

      ULUtil.one(el, transition, callback);
    },
    animationEnd: function (el, callback) {
      var animation;
      var animations = {
        animation: "animationend",
        OAnimation: "oAnimationEnd",
        MozAnimation: "mozAnimationEnd",
        WebkitAnimation: "webkitAnimationEnd",
        msAnimation: "msAnimationEnd"
      };

      for (var t in animations) {
        if (el.style[t] !== undefined) {
          animation = animations[t];
        }
      }

      ULUtil.one(el, animation, callback);
    },
    animateDelay: function (el, value) {
      var vendors = ["webkit-", "moz-", "ms-", "o-", ""];

      for (var i = 0; i < vendors.length; i++) {
        ULUtil.css(el, vendors[i] + "animation-delay", value);
      }
    },
    animateDuration: function (el, value) {
      var vendors = ["webkit-", "moz-", "ms-", "o-", ""];

      for (var i = 0; i < vendors.length; i++) {
        ULUtil.css(el, vendors[i] + "animation-duration", value);
      }
    },
    scrollTo: function (target, offset, duration) {
      var duration = duration ? duration : 500;
      var target = ULUtil.get(target);
      var targetPos = target ? ULUtil.offset(target).top : 0;
      var scrollPos = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
      var from, to;

      if (targetPos > scrollPos) {
        from = targetPos;
        to = scrollPos;
      } else {
        from = scrollPos;
        to = targetPos;
      }

      if (offset) {
        to += offset;
      }

      ULUtil.animate(from, to, duration, function (value) {
        document.documentElement.scrollTop = value;
        document.body.parentNode.scrollTop = value;
        document.body.scrollTop = value;
      }); //, easing, done
    },
    scrollTop: function (offset, duration) {
      ULUtil.scrollTo(null, offset, duration);
    },
    isArray: function (obj) {
      return obj && Array.isArray(obj);
    },
    ready: function (callback) {
      if (document.attachEvent ? document.readyState === "complete" : document.readyState !== "loading") {
        callback();
      } else {
        document.addEventListener("DOMContentLoaded", callback);
      }
    },
    isEmpty: function (obj) {
      for (var prop in obj) {
        if (obj.hasOwnProperty(prop)) {
          return false;
        }
      }

      return true;
    },
    numberString: function (nStr) {
      nStr += "";
      var x = nStr.split(".");
      var x1 = x[0];
      var x2 = x.length > 1 ? "." + x[1] : "";
      var rgx = /(\d+)(\d{3})/;

      while (rgx.test(x1)) {
        x1 = x1.replace(rgx, "$1" + "," + "$2");
      }

      return x1 + x2;
    },
    detectIE: function () {
      var ua = window.navigator.userAgent; // Test values; Uncomment to check result …
      // IE 10
      // ua = 'Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)';
      // IE 11
      // ua = 'Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko';
      // Edge 12 (Spartan)
      // ua = 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36 Edge/12.0';
      // Edge 13
      // ua = 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586';

      var msie = ua.indexOf("MSIE ");

      if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf(".", msie)), 10);
      }

      var trident = ua.indexOf("Trident/");

      if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf("rv:");
        return parseInt(ua.substring(rv + 3, ua.indexOf(".", rv)), 10);
      }

      var edge = ua.indexOf("Edge/");

      if (edge > 0) {
        // Edge (IE 12+) => return version number
        return parseInt(ua.substring(edge + 5, ua.indexOf(".", edge)), 10);
      } // other browser


      return false;
    },
    isRTL: function () {
      return ULUtil.attr(ULUtil.get("html"), "direction") == "rtl";
    },
    //
    // Scroller
    scrollInit: function (element, options) {
      if (!element) return;

      function init() {
        var ps;
        var height;

        if (options.height instanceof Function) {
          height = parseInt(options.height.call());
        } else {
          height = parseInt(options.height);
        }

        if ((options.mobileNativeScroll || options.disableForMobile) && ULUtil.isInResponsiveRange("tablet-and-mobile")) {
          ps = ULUtil.data(element).get("ps");

          if (ps) {
            if (options.resetHeightOnDestroy) {
              ULUtil.css(element, "height", "auto");
            } else {
              ULUtil.css(element, "overflow", "auto");

              if (height > 0) {
                ULUtil.css(element, "height", height + "px");
              }
            }

            ps.destroy();
            ps = ULUtil.data(element).remove("ps");
          } else if (height > 0) {
            ULUtil.css(element, "overflow", "auto");
            ULUtil.css(element, "height", height + "px");
          }

          return;
        }

        if (height > 0) {
          ULUtil.css(element, "height", height + "px");
        }

        if (options.desktopNativeScroll) {
          ULUtil.css(element, "overflow", "auto");
          return;
        } // Init scroll


        ULUtil.css(element, "overflow", "hidden");
        ps = ULUtil.data(element).get("ps");

        if (ps) {
          ps.update();
        } else {
          ULUtil.addClass(element, "ul-scroll");
          ps = new PerfectScrollbar(element, {
            wheelSpeed: 0.5,
            swipeEasing: true,
            wheelPropagation: options.windowScroll === false ? false : true,
            minScrollbarLength: 40,
            maxScrollbarLength: 300,
            suppressScrollX: ULUtil.attr(element, "data-scroll-x") != "true" ? true : false
          });
          ULUtil.data(element).set("ps", ps);
        } // Remember scroll position in cookie


        var uid = ULUtil.attr(element, "id");

        if (options.rememberPosition === true && Cookies && uid) {
          if (Cookies.get(uid)) {
            var pos = parseInt(Cookies.get(uid));

            if (pos > 0) {
              element.scrollTop = pos;
            }
          }

          element.addEventListener("ps-scroll-y", function () {
            Cookies.set(uid, element.scrollTop);
          });
        }
      } // Init


      init();

      if (options.handleWindowResize) {
        ULUtil.addResizeHandler(function () {
          init();
        });
      }
    },
    scrollUpdate: function (element) {
      var ps = ULUtil.data(element).get("ps");

      if (ps) {
        ps.update();
      }
    },
    scrollUpdateAll: function (parent) {
      var scrollers = ULUtil.findAll(parent, ".ps");

      for (var i = 0, len = scrollers.length; i < len; i++) {
        ULUtil.scrollerUpdate(scrollers[i]);
      }
    },
    scrollDestroy: function (element) {
      var ps = ULUtil.data(element).get("ps");

      if (ps) {
        ps.destroy();
        ps = ULUtil.data(element).remove("ps");
      }
    },
    setHTML: function (el, html) {
      if (ULUtil.get(el)) {
        ULUtil.get(el).innerHTML = html;
      }
    },
    getHTML: function (el) {
      if (ULUtil.get(el)) {
        return ULUtil.get(el).innerHTML;
      }
    },
    getDocumentHeight: function () {
      var body = document.body;
      var html = document.documentElement;
      return Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
    },
    getScrollTop: function () {
      return (document.scrollingElement || document.documentElement).scrollTop;
    }
  };
}();

window.ULUtil = ULUtil; // webpack support

if (typeof module !== "undefined" && typeof module.exports !== "undefined") {
  module.exports = ULUtil;
} // Initialize ULUtil class on document ready


ULUtil.ready(function () {
  ULUtil.init();
});
"use_strict";

var SidebarPanel = function () {
  var initSidebarPanel = function () {
    var overlay = document.querySelector('.ul-sidebar-panel-overlay');
    var toggleButtons = document.querySelectorAll('[data-sidebar-panel]');
    var sidebarPanels = document.querySelectorAll('.ul-sidebar-panel');
    toggleButtons.forEach(element => {
      element.addEventListener('click', function (e) {
        var sidebarPanel = document.getElementById(ULUtil.attr(element, 'data-sidebar-panel'));

        if (sidebarPanel) {
          ULUtil.hasClass(sidebarPanel, 'open') ? ULUtil.removeClass(sidebarPanel, 'open') : ULUtil.addClass(sidebarPanel, 'open');
        }

        if (overlay) {
          ULUtil.hasClass(overlay, 'open') ? ULUtil.removeClass(overlay, 'open') : ULUtil.addClass(overlay, 'open');
        }
      });
    });

    if (overlay) {
      overlay.addEventListener('click', function (e) {
        sidebarPanels.forEach(element => {
          ULUtil.removeClass(element, 'open');
        });
        ULUtil.removeClass(e.target, 'open');
      });
    }

    var sidebarClose = document.querySelectorAll('.ul-sidebar-panel-close');
    sidebarClose.forEach(element => {
      element.addEventListener('click', function (e) {
        var parent = e.target.closest('.ul-sidebar-panel');
        ULUtil.removeClass(parent, 'open');
        ULUtil.removeClass(overlay, 'open');
      });
    });
  };

  return {
    init: function () {
      initSidebarPanel();
    }
  };
}();

jQuery(document).ready(function () {
  SidebarPanel.init();
});
"use_strict";

var HeaderMenu = function () {
  var initHeadermenu = function () {
    var body = ULUtil.get("body");
    var headerMenuWrap = ULUtil.getByClass("ul-header-menu-wrap");

    function closeAllSubmenu() {
      let q = body.querySelectorAll(".ul-menu-item--open");

      for (var i = 0; i < q.length; i++) {
        ULUtil.removeClass(q[i], "ul-menu-item--open");
      }
    }

    document.addEventListener("click", function (e) {
      if (ULUtil.hasClass(e.target, "ul-menu-link") && ULUtil.hasClass(e.target.parentNode, "ul-menu-item--open")) {
        ULUtil.removeClass(e.target.parentNode, "ul-menu-item--open");
        e.preventDefault();
      } else if (ULUtil.hasClass(e.target, "ul-menu-link") && ULUtil.hasClass(e.target.parentNode, "ul-menu-item-submenu")) {
        var menuLink = e.target;
        var menuItem = menuLink.parentNode;
        ULUtil.addClass(menuItem, "ul-menu-item--open");
        e.preventDefault();
      } else if (ULUtil.hasClass(e.target, "ul-menu-link")) {} else {
        // Close submenu
        closeAllSubmenu();
      }

      if (ULUtil.hasClass(e.target, "ul-header-menu-toggle") || ULUtil.hasClass(e.target.parentNode, "ul-header-menu-toggle")) {
        if (ULUtil.hasClass(headerMenuWrap, "ul-header-menu-wrap--open")) {
          ULUtil.removeClass(headerMenuWrap, "ul-header-menu-wrap--open");
        } else {
          ULUtil.addClass(headerMenuWrap, "ul-header-menu-wrap--open");
        }
      }
    });
  };

  return {
    init: function () {
      initHeadermenu();
    }
  };
}();

jQuery(document).ready(function () {
  HeaderMenu.init();
});
"use_strict";

var MobileHeader = function () {
  var initMobileHeader = function () {
    var toggleHeader = document.querySelector('.ul-mobile-header-toggle');
    var topBar = document.querySelector('.ul-header-topbar');

    if (toggleHeader) {
      toggleHeader.addEventListener('click', function (e) {
        if (topBar) {
          ULUtil.hasClass(topBar, 'open') ? ULUtil.removeClass(topBar, 'open') : ULUtil.addClass(topBar, 'open');
        }
      });
    }
  };

  return {
    init: function () {
      initMobileHeader();
    }
  };
}();

jQuery(document).ready(function () {
  MobileHeader.init();
});
"use_strict";

var UlSearch = function () {
  var adminWrap = document.querySelector("div[class^='app-admin-wrap-layout']");
  var searchInputSelector = "#app-search";
  var searchInput = document.querySelector(searchInputSelector);
  var searchResultWrap = document.querySelector(".search-result");

  var initFullWidthSearchInput = function () {
    var wrapper = document.querySelector(".ul-search-full-width");
    var toggle = document.querySelectorAll(".toggle-search");

    if (toggle.length) {
      toggle.forEach(t => {
        t.addEventListener("click", function (e) {
          if (ULUtil.hasClass(wrapper, "open")) {
            ULUtil.removeClass(wrapper, "open"); // Clean input & trigger event so that autosuggest can detect

            searchInput.value = "";
            ULUtil.triggerCustomEvent(searchInput, "change");
          } else {
            ULUtil.addClass(wrapper, "open");
            searchInput.focus();
          }
        });
      });
    }
  };

  var initDropDownSearch = function () {};

  var initUlSearch = function () {
    if (!searchInput) {
      return;
    }

    new autoComplete({
      data: {
        // Data src [Array, Function, Async] | (REQUIRED)
        src: async () => {
          const source = await fetch(`assets/js/data/template-list.json`); // Format data into JSON

          const data = await source.json(); // Return Fetched data

          return data;
        },
        key: ["name"],
        cache: false
      },
      sort: (a, b) => {
        if (a.match < b.match) return -1;
        if (a.match > b.match) return 1;
        return 0;
      },
      placeHolder: "Explore Egret (e.g. drag & drop)",
      selector: searchInputSelector,
      threshold: 1,
      debounce: 300,
      searchEngine: "strict",
      resultsList: {
        render: true,
        container: source => {
          source.setAttribute("class", "ul-list-group-1 pt-sm");
        },
        destination: searchResultWrap,
        position: "beforeend",
        element: "div"
      },
      maxResults: 15,
      highlight: true,
      resultItem: {
        content: (data, source) => {
          source.innerHTML = `<div class="ul-list-item"><a 
          class="d-flex justify-content-start align-items-center text-body"
          href="${data.value.link}">
          
          <i class="material-icons icon icon-md">${data.value.icon || "trending_flat"}</i>
          
          <p class="m-0 text-small font-weight-semi">${data.match}</p>
          </a></div>`;
        },
        element: "div"
      },
      trigger: {
        event: ["input", "change"],
        condition: query => {
          query;

          if (query) {
            ULUtil.addClass(adminWrap, "search-result-open");
            ULUtil.addClass(searchResultWrap, "has-result");
          } else {
            ULUtil.removeClass(adminWrap, "search-result-open");
            ULUtil.removeClass(searchResultWrap, "has-result");
          }

          if (ULUtil.find(searchResultWrap, ".no-result")) {
            searchResultWrap.removeChild(searchResultWrap.querySelector(".no-result"));
          }

          return query;
        }
      },
      noResults: () => {
        if (!ULUtil.find(searchResultWrap, ".no-result")) {
          const result = document.createElement("div");
          result.setAttribute("class", "no-result");
          result.innerHTML = "<span class='text-danger'>No Results</span>";
          searchResultWrap.appendChild(result);
        }
      },
      onSelection: feedback => {
        // Action script onSelection event | (Optional)
        window.location.replace(feedback.selection.value.link);
      }
    });
  };

  return {
    init: function () {
      initFullWidthSearchInput();
      initUlSearch();
    }
  };
}();

jQuery(document).ready(function () {
  UlSearch.init();
});
"use_strict";

var SecondarySidebar = function () {
  var adminWrap = document.querySelector("div[class^='app-admin-wrap-layout']");

  var initSecondarySidebar = function () {
    // Layout script
    var secondarySidebarLayoutButton = document.querySelector(".narrow-sidebar-toggle-button");
    var secondarySidebarLayout = document.querySelector(".narrow-sidebar");
    var sidebarCloseEvent = new Event("secondarySidebarClose");

    if (!secondarySidebarLayout || !secondarySidebarLayoutButton) {
      return;
    }

    secondarySidebarLayoutButton.addEventListener("click", function () {
      if (secondarySidebarLayout) {
        ULUtil.addClass(secondarySidebarLayout, "close-secondary-sidebar");

        if (ULUtil.hasClass(adminWrap, "secondary-sidebar-open")) {
          ULUtil.removeClass(adminWrap, "secondary-sidebar-open");
          secondarySidebarLayout.dispatchEvent(sidebarCloseEvent);
        } else {
          ULUtil.addClass(adminWrap, "secondary-sidebar-open");
        }
      }
    });
  };

  return {
    init: function () {
      initSecondarySidebar();
    }
  };
}();

jQuery(document).ready(function () {
  SecondarySidebar.init();
});
"use_strict";

var StickyHeader = function () {
  var initStickyHeader = function () {
    var body = ULUtil.get("body");
    window.addEventListener('scroll', () => {
      var header = document.querySelector(".ul-header-navigation");
      var yOffset = window.pageYOffset;

      if (yOffset > 300 && !ULUtil.isInResponsiveRange('tablet-and-mobile')) {
        body.classList.add('sticky-header');
        ;
      } else {
        body.classList.remove('sticky-header');
      }
    });
  };

  return {
    init: function () {
      initStickyHeader();
    }
  };
}();

jQuery(document).ready(function () {
  StickyHeader.init();
});
(function () {
  //DOM elements
  const DOMstrings = {
    stepsBtnClass: "multisteps-form__progress-btn",
    stepsBtns: document.querySelectorAll(`.multisteps-form__progress-btn`),
    stepsBar: document.querySelector(".multisteps-form__progress"),
    stepsForm: document.querySelector(".multisteps-form__form"),
    stepsFormTextareas: document.querySelectorAll(".multisteps-form__textarea"),
    stepFormPanelClass: "multisteps-form__panel",
    stepFormPanels: document.querySelectorAll(".multisteps-form__panel"),
    stepPrevBtnClass: "js-btn-prev",
    stepNextBtnClass: "js-btn-next"
  }; //remove class from a set of items

  const removeClasses = (elemSet, className) => {
    elemSet.forEach(elem => {
      elem.classList.remove(className);
    });
  }; //return exect parent node of the element


  const findParent = (elem, parentClass) => {
    let currentNode = elem;

    while (!currentNode.classList.contains(parentClass)) {
      currentNode = currentNode.parentNode;
    }

    return currentNode;
  }; //get active button step number


  const getActiveStep = elem => {
    return Array.from(DOMstrings.stepsBtns).indexOf(elem);
  }; //set all steps before clicked (and clicked too) to active


  const setActiveStep = activeStepNum => {
    //remove active state from all the state
    removeClasses(DOMstrings.stepsBtns, "js-active"); //set picked items to active

    DOMstrings.stepsBtns.forEach((elem, index) => {
      if (index <= activeStepNum) {
        elem.classList.add("js-active");
      }
    });
  }; //get active panel


  const getActivePanel = () => {
    let activePanel;
    DOMstrings.stepFormPanels.forEach(elem => {
      if (elem.classList.contains("js-active")) {
        activePanel = elem;
      }
    });
    return activePanel;
  }; //open active panel (and close unactive panels)


  const setActivePanel = activePanelNum => {
    //remove active class from all the panels
    removeClasses(DOMstrings.stepFormPanels, "js-active"); //show active panel

    DOMstrings.stepFormPanels.forEach((elem, index) => {
      if (index === activePanelNum) {
        elem.classList.add("js-active");
        setFormHeight(elem);
      }
    });
  }; //set form height equal to current panel height


  const formHeight = activePanel => {
    const activePanelHeight = activePanel.offsetHeight;
    DOMstrings.stepsForm.style.height = `${activePanelHeight}px`;
  };

  const setFormHeight = () => {
    const activePanel = getActivePanel();
    formHeight(activePanel);
  }; //STEPS BAR CLICK FUNCTION


  if (!DOMstrings.stepsBar) {
    return;
  }

  DOMstrings.stepsBar.addEventListener("click", e => {
    //check if click target is a step button
    const eventTarget = e.target;

    if (!eventTarget.classList.contains(`${DOMstrings.stepsBtnClass}`)) {
      return;
    } //get active button step number


    const activeStep = getActiveStep(eventTarget); //set all steps before clicked (and clicked too) to active

    setActiveStep(activeStep); //open active panel

    setActivePanel(activeStep);
  }); //PREV/NEXT BTNS CLICk

  if (!DOMstrings.stepsForm) {
    return;
  }

  DOMstrings.stepsForm.addEventListener("click", e => {
    const eventTarget = e.target; //check if we clicked on `PREV` or NEXT` buttons

    if (!(eventTarget.classList.contains(`${DOMstrings.stepPrevBtnClass}`) || eventTarget.classList.contains(`${DOMstrings.stepNextBtnClass}`))) {
      return;
    } //find active panel


    const activePanel = findParent(eventTarget, `${DOMstrings.stepFormPanelClass}`);
    let activePanelNum = Array.from(DOMstrings.stepFormPanels).indexOf(activePanel); //set active step and active panel onclick

    if (eventTarget.classList.contains(`${DOMstrings.stepPrevBtnClass}`)) {
      activePanelNum--;
    } else {
      activePanelNum++;
    }

    setActiveStep(activePanelNum);
    setActivePanel(activePanelNum);
  }); //SETTING PROPER FORM HEIGHT ONLOAD

  window.addEventListener("load", setFormHeight, false); //SETTING PROPER FORM HEIGHT ONRESIZE

  window.addEventListener("resize", setFormHeight, false);
  return;
})();
"use_strict";

var ArcticCustomizer = function () {
  var secondarySidebarLayout = document.querySelector(".narrow-sidebar"); //customizer

  var arcticCustomSettings = document.querySelector(".sidebar-customizer-settings");
  var arcticCustomizer = document.querySelector(".egret-customizer");
  var arcticCustomizerClose = document.querySelector(".customizer-close");

  var closeCustomizer = function () {
    ULUtil.removeClass(arcticCustomizer, "show");
    ULUtil.removeClass(arcticCustomizer, "collapse");
  };

  var initArcticCustomizer = function () {
    if (!arcticCustomizer) {
      return;
    }

    if (arcticCustomSettings) {
      arcticCustomSettings.addEventListener("click", function () {
        if (arcticCustomizer) {
          ULUtil.hasClass(arcticCustomizer, "show") ? ULUtil.removeClass(arcticCustomizer, "show") : ULUtil.addClass(arcticCustomizer, "show");
        }
      });
    }

    if (arcticCustomizerClose) {
      arcticCustomizerClose.addEventListener("click", function () {
        closeCustomizer();
      });
    } //egret customizer color sidebar-theme


    var addColors = document.querySelectorAll("[data-sidebar-color]");
    var adminWrapLayout1 = document.querySelector(".app-admin-wrap-layout-1");
    addColors.forEach(element => {
      element.addEventListener("click", function () {
        var sidebarThemeClass = ULUtil.attr(element, "data-sidebar-color");
        ULUtil.removeClassByPrefix(adminWrapLayout1, "sidebar-theme");
        ULUtil.addClass(adminWrapLayout1, sidebarThemeClass);
      });
    });
    secondarySidebarLayout.addEventListener("secondarySidebarClose", function () {
      closeCustomizer();
    });
  };

  return {
    init: function () {
      initArcticCustomizer();
    }
  };
}();

jQuery(document).ready(function () {
  ArcticCustomizer.init();
});
var Arctic = function () {
  var initArctic = function () {
    var adminWrap = document.querySelector("div[class^='app-admin-wrap-layout']"); // Material Design

    $("body").bootstrapMaterialDesign(); // Tooltip

    $('[data-toggle="tooltip"]').tooltip(); // Popover

    $('[data-toggle="popover"]').each(function () {
      var popoverClass = "";

      if ($(this).data("color")) {
        popoverClass = "popover-" + $(this).data("color");
      }

      $(this).popover({
        trigger: "focus",
        template: '<div class="popover ' + popoverClass + '" role="tooltip"><div class="arrow"></div><h3 class="popover-header"></h3><div class="popover-body"></div></div>'
      });
    }); // Metis menu

    $("#ul-menu").metisMenu(); // Init onload elements

    const showOnLoadElm = document.querySelectorAll('.show-on-load');
    showOnLoadElm.forEach(elm => ULUtil.removeClass(elm, 'show-on-load')); // Perfect scrollbar

    $(".perfect-scrollbar, [data-perfect-scrollbar]").each(function (index) {
      var $el = $(this);
      var ps = new PerfectScrollbar(this, {
        suppressScrollX: $el.data("suppress-scroll-x"),
        suppressScrollY: $el.data("suppress-scroll-y")
      });
    }); // Sidenav consistant position

    const scrollNav = document.querySelector(".scroll-nav");

    if (scrollNav) {
      scrollNav.scrollTop = localStorage.getItem("ul_navigation_pos") || 0;
      scrollNav.addEventListener("ps-scroll-y", ULUtil.debounce(e => {
        localStorage.setItem("ul_navigation_pos", e.target.scrollTop);
      }, 100));
    } // init disabled link


    const disabledLinks = document.querySelectorAll("a[disabled]");
    disabledLinks.forEach(a => {
      a.addEventListener("click", function (e) {
        e.preventDefault();
      });
    }); // feathericon

    if (window.feather) {
      feather.replace();
    }
  };

  return {
    init: function () {
      initArctic();
    }
  };
}();

jQuery(document).ready(function () {
  Arctic.init();
});
"use_strict";

var Layout1 = function () {
  var initLayout1 = function () {
    // Layout script
    var $body = $("body");
    var $appAdminWrap = $(".app-admin-wrap-layout-1");
    var $sidebarPanel = $(".sidebar-panel");
    var $sidebarFullToggle = $(".sidebar-full-toggle");
    var $sidebarCompactToggle = $(".sidebar-compact-switch");

    function openSidebarFull() {
      $appAdminWrap.removeClass("sidebar-closed");
      $appAdminWrap.addClass("sidebar-full");
    }

    function closeSidebarFull() {
      $appAdminWrap.addClass("sidebar-closed");
      $appAdminWrap.removeClass("sidebar-full");
    }

    function openSidebarCompact() {
      $appAdminWrap.addClass("sidebar-compact");
    }

    function closeSidebarCompact() {
      $appAdminWrap.removeClass("sidebar-compact");
    }

    function toggleOnHover() {
      closeSidebarCompact();
      $appAdminWrap.toggleClass("sidebar-compact-onhover");
    }

    function toggleOverlay() {
      if ($body.find(".ul-overlay").length) {
        $(".ul-overlay").remove();
      } else {
        $body.append($('<div class="ul-overlay">').css({
          position: "fixed",
          top: 0,
          left: 0,
          height: "100%",
          width: "100%",
          background: "rgba(0,0,0, .1)",
          zIndex: 10
        }));
      }
    }

    $sidebarFullToggle.on("click", function () {
      $appAdminWrap.toggleClass("sidebar-mobile");
      toggleOverlay();
    });
    $(document).on("click", ".ul-overlay", function () {
      $appAdminWrap.toggleClass("sidebar-mobile");
      toggleOverlay();
    });
    $sidebarCompactToggle.on("click", function () {
      toggleOnHover();
    });
    $sidebarPanel.on("mouseenter", function (e) {
      if ($appAdminWrap.hasClass("sidebar-compact-onhover")) {
        closeSidebarCompact();
        openSidebarFull();
      }
    }).on("mouseleave", function (e) {
      if ($appAdminWrap.hasClass("sidebar-compact-onhover")) {
        closeSidebarFull();
        openSidebarCompact();
      }
    });
  };

  return {
    init: function () {
      initLayout1();
    }
  };
}();

jQuery(document).ready(function () {
  Layout1.init();
});
"use_strict";

var LayoutTwo = function () {
  var initLayoutTwo = function () {// var mainContentWrap2 = document.querySelector('.app-admin-wrap-layout-2');
    // var secondarySidebarLayout = document.querySelector('.narrow-sidebar');
    // var secondarySidebarLayoutButton = document.querySelector('.narrow-sidebar-toggle-button');
    // secondarySidebarLayoutButton.addEventListener("click", function(){
    //   ULUtil.hasClass(mainContentWrap2, 'close-secondary-sidebar') ? ULUtil.removeClass(mainContentWrap2, 'close-secondary-sidebar') : ULUtil.addClass(mainContentWrap2, 'close-secondary-sidebar');
    // });
  };

  return {
    init: function () {
      initLayoutTwo();
    }
  };
}();

jQuery(document).ready(function () {
  LayoutTwo.init();
});
"use_strict";

var Layout3 = function () {
  var initLayout3 = function () {
    var toggleBar = document.querySelector(".ul-header-menu-toggle");
    var overlay = document.querySelector(".ul-sidebar-3-overlay");
    var sidebar = document.querySelector(".sidebar-3");

    if (toggleBar) {
      toggleBar.addEventListener("click", () => {
        var sidebar = document.querySelector(".sidebar-3");

        if (sidebar) {
          ULUtil.hasClass(sidebar, "open") ? ULUtil.removeClass(sidebar, "open") : ULUtil.addClass(sidebar, "open");
        }

        if (overlay) {
          ULUtil.hasClass(overlay, "open") ? ULUtil.removeClass(overlay, "open") : ULUtil.addClass(overlay, "open");
        }
      });
    }

    if (overlay) {
      overlay.addEventListener("click", () => {
        if (overlay) {
          ULUtil.hasClass(overlay, "open") ? ULUtil.removeClass(overlay, "open") : ULUtil.addClass(overlay, "open");
          ULUtil.hasClass(sidebar, "open") ? ULUtil.removeClass(sidebar, "open") : ULUtil.addClass(sidebar, "open");
        }
      });
    }

    var mainContentWrap3 = document.querySelector(".app-admin-wrap-layout-3");
    var subheader = document.querySelector(".subheader");
    var secondarySidebarLayoutButton = document.querySelector(".narrow-sidebar-toggle-button");

    if (!secondarySidebarLayoutButton) {
      return;
    }

    secondarySidebarLayoutButton.addEventListener("click", function () {
      ULUtil.hasClass(mainContentWrap3, "close-secondary-sidebar") ? ULUtil.removeClass(mainContentWrap3, "close-secondary-sidebar") : ULUtil.addClass(mainContentWrap3, "close-secondary-sidebar");
      ULUtil.hasClass(subheader, "close-secondary-sidebar") ? ULUtil.removeClass(subheader, "close-secondary-sidebar") : ULUtil.addClass(subheader, "close-secondary-sidebar");
    });
  };

  return {
    init: function () {
      initLayout3();
    }
  };
}();

jQuery(document).ready(function () {
  Layout3.init();
});
"use_strict";

var Layout4 = function () {
  var initLayout4 = function () {
    var toggleBar = document.querySelector(".ul-header-menu-toggle");
    var overlay = document.querySelector(".ul-sidebar-4-overlay");
    var sidebar = document.querySelector(".ul-sidebar-4");

    if (toggleBar) {
      toggleBar.addEventListener("click", () => {
        var sidebar = document.querySelector(".ul-sidebar-4");

        if (sidebar) {
          ULUtil.hasClass(sidebar, "open") ? ULUtil.removeClass(sidebar, "open") : ULUtil.addClass(sidebar, "open");
        }

        if (overlay) {
          ULUtil.hasClass(overlay, "open") ? ULUtil.removeClass(overlay, "open") : ULUtil.addClass(overlay, "open");
        }
      });
    }

    if (overlay) {
      overlay.addEventListener("click", () => {
        if (overlay) {
          ULUtil.hasClass(overlay, "open") ? ULUtil.removeClass(overlay, "open") : ULUtil.addClass(overlay, "open");
          ULUtil.hasClass(sidebar, "open") ? ULUtil.removeClass(sidebar, "open") : ULUtil.addClass(sidebar, "open");
        }
      });
    } // var mainContentWrap4 = document.querySelector('.app-admin-wrap-layout-4');
    // var header = document.querySelector('.ul-header-4');
    // var secondarySidebarLayoutButton = document.querySelector('.narrow-sidebar-toggle-button');
    // secondarySidebarLayoutButton.addEventListener("click", function(){
    //   ULUtil.hasClass(header, 'close-secondary-sidebar') ? ULUtil.removeClass(header, 'close-secondary-sidebar') : ULUtil.addClass(header, 'close-secondary-sidebar');
    // });

  };

  return {
    init: function () {
      initLayout4();
    }
  };
}();

jQuery(document).ready(function () {
  Layout4.init();
});