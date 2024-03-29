﻿using Sitecore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CT.SC.Foundation.SitecoreExtensions.BranchPresets
{
	public abstract class Disabler<TSwitchType> : Switcher<DisablerState, TSwitchType>
	{
		// ReSharper disable once PublicConstructorInAbstractClass
		public Disabler() : base(DisablerState.Enabled)
		{
		}

		public static bool IsActive => CurrentValue == DisablerState.Enabled;
	}

	public enum DisablerState
	{
		Disabled,
		Enabled
	}
}