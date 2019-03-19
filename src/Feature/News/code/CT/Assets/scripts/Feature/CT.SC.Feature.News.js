/****************************************************************************
CT.SC.Feature.News.Base.js has been used for News module
*****************************************************************************/
/* 
* Version:       0.0.0.1
* Created By:   Amit Kumar
* Created Date:   12/26/2018 (MM/DD/YYYY)
* Modified By:  Amit Kumar
* Modified Date:   12/31/2018 (MM/DD/YYYY)
*/
CT.component.news = (function ($) {
	var api = {},
		cities = null;
	api.returnUrl = "hello";

	privateFunction = function (value) {

		//---Check News Div loaded or not----::Start::--------------
		var chkNewsDiv = setInterval(function () {
			if (window.jQuery('#divNews').length > 0) {
				//----Document.ready-----::Start::---------

				window.jQuery(document).ready(function () {

					window.jQuery('#divNews').html("<div id='divChildNews' ></div>");
					api.addMessage('divNews', '</br><div id="divApi" class="bg-skyblue"></div>');

					//---Check divChildNews loaded or not----::Start::--------------
					var chkData = setInterval(function () {
						if (window.jQuery('#divChildNews').length > 0) {
							{
								CT.component.news.addMessage('divChildNews', 'Dynamic Content-1');
								api.addMessage('divChildNews', '</br>Dynamic Content-2');
								clearInterval(chkData);
							}
						}
					}, 300);
					//---Check divChildNews loaded or not----::End::--------------


				});
				//----Document.ready-----::End::---------
				clearInterval(chkNewsDiv);
			}
		}, 300);
		//---Check News Div loaded or not----::End::--------------


	};

	api.init = function () {
		console.log("News module initiated");
		privateFunction();
	};

	api.addMessage = function (id, message) {
		window.jQuery('#' + id + '').append(message);
	};

	api.getCity = function () {
		//--Make service call to get cities
		//CT.helper.ajaxPost(null, CT.helper.baseAPIUrl + "motorinsurance/master/v1/vehicles?lang=en", "GET", api.getCityOnSuccess, api.getCityOnError);
		CT.helper.ajaxPost(null, "http://api.joind.in/v2.1/talks/10889", CT.helper.ajaxCallType.get, api.getCityOnSuccess, api.getCityOnError);
	}

	api.getCityOnSuccess = function (response) {

		if (!CT.helper.isNullOrEmpty(response) && response.talks) {

			if (!CT.helper.isNullOrEmpty(response.talks) && response.talks.length > 0) {

				//cities = response.masterData.cities;

				var $title = $('<h1>').text(response.talks[0].talk_title);
				var $description = $('<p>').text(response.talks[0].talk_description);
				api.addMessage('divApi', $title);
				api.addMessage('divApi', $description);
			}
			else {
				//--If no record found for city
				CT.helper.logMessage('Error=> No record found for city.');
			}
		}
		else {
			//--If error occured during service call
			CT.helper.logMessage('Error=>' + CT.helper.errorMessages.error_service);
		}
	}

	api.getCityOnError = function (response) {
		if (!CT.helper.isNullOrEmpty(response.responseJSON) && rresponse.responseJSON.length > 0) {
			CT.helper.logMessage('api.getCityOnError -> Error=>' + CT.helper.errorMessages.error_service + "::" + response.responseJSON[0]);
		}
		else {
			CT.helper.logMessage('api.getCityOnError -> Error=>' + CT.helper.errorMessages.error_service);
		}
	}

	return api;
}(jQuery, document));

CT.register("news", CT.component.news);