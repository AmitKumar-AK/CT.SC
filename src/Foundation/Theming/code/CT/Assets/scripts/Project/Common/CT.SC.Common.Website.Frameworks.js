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

/****************************************************************************
CT.SC.Common.Website.Helper.js has been used to serverd as helper class for all websites
*****************************************************************************/
/* 
* Version:       0.0.0.1
* Created By:   Amit Kumar
* Created Date:   12/26/2018 (MM/DD/YYYY)
* Modified By:  Amit Kumar
* Modified Date:   12/31/2018 (MM/DD/YYYY)
*/
CT.helper = (function ($) {
	var api = {},
		isError = "",
		clientItemPrefix = "CTClientItem-";

	api.baseControllerUrl = "/api/sitecore/";
	api.baseAPIUrl = "http://service.xyz.com/";
	api.errorMessages =
		{
			error_loading: 'There was an error loading information. Please refresh the page or try again later.',
			error_service: 'There was an error getting record from the service .',
			empty_info: "There is no record to display.",
			forbidden: "You are not authorized to view or update this data."
		};

	api.ajaxCallType =
		{
			get: 'GET',
			post: 'POST',
		};

	api.isNullOrEmpty = function (value) {
		if (value == null || typeof (value) == 'undefined' || value === '' || value == 'null')
			return true;
		else
			return false;
	};

	api.logMessage = function (value) {
		console.log(value);
	};

	api.init = function () {
		console.log("helper initiated");
	};

	api.loadJsCssFile = function (filename, filetype) {
		if (filetype == "js") { //if filename is a external JavaScript file
			var fileref = document.createElement('script')
			fileref.setAttribute("type", "text/javascript")
			fileref.setAttribute("src", filename)
		}
		else if (filetype == "css") { //if filename is an external CSS file
			var fileref = document.createElement("link")
			fileref.setAttribute("rel", "stylesheet")
			fileref.setAttribute("type", "text/css")
			fileref.setAttribute("href", filename)
		}
		if (typeof fileref != "undefined")
			document.getElementsByTagName("head")[0].appendChild(fileref)
	};

	api.showHideItem = function (objItem, isHidden) {
		if (window.jQuery('#' + objItem).length > 0) {
			if (!isHidden) {
				window.jQuery('#' + objItem).show();
			}
			else {
				window.jQuery('#' + objItem).hide();
			}
		}
	};

	api.addCssClass = function (e, c) {
		var classList = "";
		if (!api.isNullOrEmpty(document.getElementById(e))) {
			classList = document.getElementById(e).classList;
		}
		else {
			classList = e.classList;
		}
		api.removeCssClass(e, c);
		classList.add(c);
	};

	api.removeCssClass = function (e, c) {
		var classList = "";
		if (!api.isNullOrEmpty(document.getElementById(e))) {
			classList = document.getElementById(e).classList;
		}
		else {
			classList = e.classList;
		}
		classList.remove(c);
	};

	api.logMsg = function (message) {
		if (window.console && !api.isNullOrEmpty(message)) {
			console.log(message + '--::--' + new Date());
		}
	};

	api.getUTCTime = function (d) {
		utcTime = new Date(d)
		var dd = utcTime.getDate();
		var mm = utcTime.getMonth() + 1;
		var yyyy = utcTime.getFullYear();
		var hh = utcTime.getHours();
		var mm = utcTime.getMinutes();
		var ss = utcTime.getSeconds();
		if (dd < 10) { dd = '0' + dd }
		if (mm < 10) { mm = '0' + mm };
		return hh + ":" + mm + ":" + ss;
	};

	api.replaceAll = function (replace, with_this, str) {
		var str_hasil = "";
		var temp;

		for (var i = 0; i < str.length; i++) // not need to be equal. it causes the last change: undefined..
		{
			if (str[i] == replace) {
				temp = with_this;
			}
			else {
				temp = str[i];
			}

			str_hasil += temp;
		}

		return str_hasil;

	};

	api.getCurrentDate = function (format) {
		var currDate = '';
		if (!api.isNullOrEmpty(format)) {
			currDate = new Date().format(format);
		}
		else {
			currDate = new Date();
		}
		return currDate;
	};

	api.getParameterByName = function (e) {
		e = e.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
		var a = new RegExp("[\\?&]" + e + "=([^&#]*)"), n = a.exec(location.search); return null === n ? "" : decodeURIComponent(n[1].replace(/\+/g, " "))
	};

	api.setLocalStorageItem = function (key, value) {
		window.localStorage.setItem(clientItemPrefix + key, value);
	};

	api.getLocalStorageItem = function (key) {
		return window.localStorage.getItem(clientItemPrefix + key);
	};

	api.removeLocalStorageItem = function (key) {
		return window.localStorage.removeItem(clientItemPrefix + key);
	};

	api.clearLocalStorage = function () {
		return window.localStorage.clear();
	};

	api.setCookieItem = function (key, value, exp_y, exp_m, exp_d, path, domain, secure) {
        /*
        //For example, to use this function to set a cookie with no expiry date:
        set_cookie ( "username", "John Smith" );

        To set a cookie with an expiry date of 15 Feb 2003:
        set_cookie ( "username", "John Smith", 2003, 01, 15 );

        To set a secure cookie with an expiry date and a domain of elated.com, but no path:
        set_cookie ( "username", "John Smith", 2003, 01, 15, "",
             "elated.com", "secure" );

        The Domain option allows you to specify whether or not to send the cookie to subdomains. Setting �www.example.com� will mean only
        the exact domain �www.example.com� will be matched, while �.example.com� will also match again
        any subdomaim (forums.example.com, blog.example.com).


        */
		key = clientItemPrefix + key;
		var cookie_string = key + "=" + escape(value);
		if (exp_y) {
			var expires = new Date(exp_y, exp_m, exp_d);
			cookie_string += "; expires=" + expires.toGMTString();
		}

		if (path)
			cookie_string += "; path=" + escape(path);

		if (domain)
			cookie_string += "; domain=" + escape(domain);

		if (secure)
			cookie_string += "; secure";

		document.cookie = cookie_string;
	};

	api.getCookieItem = function (key) {
		key = clientItemPrefix + key;
		var results = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');

		if (results)
			return (unescape(results[2]));
		else
			return null;
	};

	api.removeCookieItem = function (key) {
		key = clientItemPrefix + key;
		var cookie_date = new Date();  // current date & time
		cookie_date.setTime(cookie_date.getTime() - 1);
		document.cookie = key += "=; expires=" + cookie_date.toGMTString();
	};

	api.getClientSideItem = function (key) {

		if (typeof (Storage) !== "undefined" && window.localStorage) {
			//Support of Web storage
			return api.getLocalStorageItem(key);
		} else {
			// Sorry! No Web Storage support..
			return api.getCookieItem(key);
		}
	};

	api.ajaxPost = function (data, route, type, successFunction, errorFunction) {
		$.ajax({
			type: type,
			dataType: "json"
			, url: route,
			data: data,
			success: (function (response, status, headers, config) {
				successFunction(response, status);
			}),
			error: (function (response, status, headers, config) {
				errorFunction(response);
			})

		});
	};

	api.getDynamicGuId = function () {
		return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
			var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
			return v.toString(16);
		});
	};



	return api;
}(jQuery, document));

CT.register("helper", CT.helper);