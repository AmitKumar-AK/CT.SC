/****************************************************************************
CT.SC.Common.Website.Base.js has been used to serverd as BASE class for all websites
and all custom JS files will be child of this base class Concentra.
*****************************************************************************/
/* 
* Version:       0.0.0.1
* Created By:   Amit Kumar
* Created Date:   12/26/2018 (MM/DD/YYYY)
* Modified By:  Amit Kumar
* Modified Date:   12/26/2018 (MM/DD/YYYY)
*/

var CT = CT || (function ($, document) {
	var api = {}, onPreInitHandlers, onPostInitHandlers, modules = {};
	onPreInitHandlers = new Array();
	onPostInitHandlers = new Array();
	api.register = function (name, api, init) {
		modules[name] = {
			name: name,
			api: api,
			init: init || api.init || (function () { })
		};
	};
	api.registerOnPreInitHandler = function (handler) { onPreInitHandlers.push(handler); };
	api.registerOnPostInitHandler = function (handler) { onPostInitHandlers.push(handler); };
	var initScheduled = false;
	api.init = function () {
		if (!initScheduled) {
			initScheduled = true;
			CT.ready(function () {
				try {
					for (var name in modules)
						if (modules.hasOwnProperty(name)) {
							$.each(onPreInitHandlers, function (i, h) { h.process(name, modules[name]); });
							modules[name].init();
							$.each(onPostInitHandlers, function (i, h) { h.process(name, modules[name]); });
						}
				}
				finally {
					initScheduled = false;
				}
			});
		}
	};
	api.ready = function (fn) {
		$(document).ready(fn);
	};
	api.component = {};
	api.connector = {};
	api.cookies = {
		createCookie: function (name, value, days) {
			if (days) {
				var date = new Date();
				date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
				var expires = "; expires=" + date.toUTCString();
			}
			else {
				expires = "";
			}
			document.cookie = name + "=" + value + expires + "; path=/";
		},
		readCookie: function (name) {
			var nameEQ = name + "=";
			var ca = document.cookie.split(';');
			for (var i = 0; i < ca.length; i++) {
				var c = ca[i];
				while (c.charAt(0) == ' ') {
					c = c.substring(1, c.length);
				}
				if (c.indexOf(nameEQ) == 0) {
					return c.substring(nameEQ.length, c.length);
				}
			}
			return null;
		},
		removeCookieWarning: function () {
			var cookieWarning = $(".privacy-warning");
			cookieWarning.remove();
		}
	};
	return api;
})($, document);
CT.init();
