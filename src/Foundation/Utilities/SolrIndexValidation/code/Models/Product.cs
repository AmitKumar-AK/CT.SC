using SolrNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CT.SC.Foundation.SolrIndexValidation.Models
{
	public class Product
	{
		[SolrUniqueKey("id")]
		public string ID { get; set; }

		[SolrField("name")]
		public string Name { get; set; }

	}
}