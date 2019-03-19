/****************************************************************************
CT.SC.BranA.Website.Base.js has been used to serverd as main class for BrandA website
*****************************************************************************/
/* 
* Version:       0.0.0.1
* Created By:   Amit Kumar
* Created Date:   12/26/2018 (MM/DD/YYYY)
* Modified By:  Amit Kumar
* Modified Date:   12/26/2018 (MM/DD/YYYY)
*/
CT.branda = (function ($) {
	var api = {};


	api.isNullOrEmpty = function (value) {
		if (value == null || typeof (value) == 'undefined' || value === '' || value == 'null')
			return true;
		else
			return false;
	};


	return api;
}(jQuery, document));

CT.register("branda", CT.branda);