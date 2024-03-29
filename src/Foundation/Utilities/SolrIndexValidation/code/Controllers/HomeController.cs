﻿using CT.SC.Foundation.SolrIndexValidation.Models;
using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security.AntiXss;

namespace code.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			List<Product> prodList = GetQuryData();
			return View(prodList);
		}

		public List<Product> GetQuryData()
		{
			Uri uri = new Uri("https://localhost:8983/solr/samplecollection/select?indent=on&q=*:*&wt=json");
			//--Used to bypass the Client Certificate to access the Solr instance
			System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate,
	X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
			WebRequest request = HttpWebRequest.Create(uri);
			request.Method = WebRequestMethods.Http.Get;
			WebResponse response = request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			string jsonResponse = reader.ReadToEnd();
			response.Close();

			JavaScriptSerializer serializer = new JavaScriptSerializer();
			dynamic jsonObject = serializer.Deserialize<dynamic>(jsonResponse);
			dynamic dd = jsonObject["response"]["docs"];
			List<Product> prodList = new List<Product>();
			Product prod;
			foreach (dynamic res in dd)
			{
				prod = new Product();
				try
				{

					if (res["id"] != null)
					{
						prod.ID = Convert.ToString(res["id"]);
					}

					if (res["name"] != null && res["name"][0] != null)
					{
						prod.Name = Convert.ToString(res["name"][0]);
					}
				}
				catch
				{

				}

				prodList.Add(prod);
				prod = null;
			}
			return prodList;
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";
			ViewBag.Key = GetUniqueKey(6);

			return View();
		}

		public string GetUniqueKey(int size)
		{
			char[] chars =
				"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
			byte[] data = new byte[size];
			using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
			{
				crypto.GetBytes(data);
			}
			StringBuilder result = new StringBuilder(size);
			foreach (byte b in data)
			{
				result.Append(chars[b % (chars.Length)]);
			}
			return result.ToString();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult Search(FormCollection form)
		{
			//--If you want to allow tags, then encode it and then decoode it while accessing
			string userinput = AntiXssEncoder.HtmlEncode(form["search"],false);
			//--If you want to remove tags
			var sanitizer = new HtmlSanitizer();
			var sanitized = sanitizer.Sanitize(form["search"]);
			return RedirectToAction("Index", "Search", new { userinput = userinput, sanitized= sanitized });
		}
	}
}