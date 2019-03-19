/****************************************************************************
CT.SC.Feature.Account.Base.js has been used for Account module
*****************************************************************************/
/* 
* Version:       0.0.0.1
* Created By:   Amit Kumar
* Created Date:   02/26/2019 (MM/DD/YYYY)
* Modified By:  Amit Kumar
* Modified Date:   02/26/2019 (MM/DD/YYYY)
*/
CT.component.account = (function ($) {
	var api = {},
		cities = null;
	api.returnUrl = "hello";
	api.tabs = {
		header: { title: 'Header', template: '/Views/PageHTML/header.html', tab: '', controllerJS: '' },
		home: { title: 'Home | ', template: '/Views/Home/home.html', tab: '/home', controllerJS: '/Scripts/app/controllers/home/home-controller.js', animationCss: 'page-slide' },
		login: { title: 'Login | ', template: '/Views/Login/Login.html', tab: '/', controllerJS: '/Scripts/app/controllers/login/login-controller.js' },
	};
	api.quiz = null;

	createQuizData = function () {

		var objQuizItem = new Object();
		objQuizItem.id = CT.helper.getDynamicGuId();
		objQuizItem.title = "Quiz -1 For Level 0";
		objQuizItem.questions = [];

		//--1st Question-----::Start::----//
		objQuestionItem = null;
		objQuestionItem = new Object();
		objQuestionItem.id = CT.helper.getDynamicGuId();
		objQuestionItem.title = "How many databases are associated with Sitecore?";
		objQuestionItem.optoins = [];

		var objOptionItem = new Object();
		objOptionItem.id = CT.helper.getDynamicGuId();
		objOptionItem.title = "Master";
		//--Add option into Question
		objQuestionItem.optoins.push(objOptionItem);

		objOptionItem = null;
		objOptionItem = new Object();
		objOptionItem.id = CT.helper.getDynamicGuId();
		objOptionItem.title = "Core";
		//--Add option into Question
		objQuestionItem.optoins.push(objOptionItem);

		objOptionItem = null;
		objOptionItem = new Object();
		objOptionItem.id = CT.helper.getDynamicGuId();
		objOptionItem.title = "Web";
		//--Add option into Question
		objQuestionItem.optoins.push(objOptionItem);
		objOptionItem = null;

		//Add question to quiz
		objQuizItem.questions.push(objQuestionItem);
		objQuestionItem = null;
		//--1st Question-----::End::----//

		//--2nd Question-----::Start::----//
		var objQuestionItem = new Object();
		objQuestionItem.id = "e4bb964f-2376-4fce-a56e-dd2f89eb3894";
		objQuestionItem.title = "What Are Different Types Of Templates In Sitecore?";
		objQuestionItem.optoins = [];

		var objOptionItem = new Object();
		objOptionItem.id = CT.helper.getDynamicGuId();
		objOptionItem.title = "Data templates";
		//--Add option into Question
		objQuestionItem.optoins.push(objOptionItem);

		objOptionItem = null;
		objOptionItem = new Object();
		objOptionItem.id = CT.helper.getDynamicGuId();
		objOptionItem.title = "Parameter Templates";
		//--Add option into Question
		objQuestionItem.optoins.push(objOptionItem);

		objOptionItem = null;
		objOptionItem = new Object();
		objOptionItem.id = CT.helper.getDynamicGuId();
		objOptionItem.title = "Datasource templates";
		//--Add option into Question
		objQuestionItem.optoins.push(objOptionItem);
		objOptionItem = null;

		//Add question to quiz
		objQuizItem.questions.push(objQuestionItem);
		objQuestionItem = null;
		//--2nd Question-----::End::----//



		return objQuizItem;
	};

	api.init = function () {
		console.log("Account module initiated");
		//privateFunction();
		api.quiz = createQuizData();
	};



	api.getCity = function () {
		//--Make service call to get cities
		//CT.helper.ajaxPost(null, CT.helper.baseAPIUrl + "motorinsurance/master/v1/vehicles?lang=en", "GET", api.getCityOnSuccess, api.getCityOnError);
		CT.helper.ajaxPost(null, "http://api.joind.in/v2.1/talks/10889", CT.helper.ajaxCallType.get, api.getCityOnSuccess, api.getCityOnError);
	};



	return api;
}(jQuery, document));

CT.register("account", CT.component.account);