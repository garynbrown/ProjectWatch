using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.UI.Core;

namespace ProjectWatch.ViewModel
{

	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class PhaseViewModel : ViewModelCommon
	{

	public override string ViewTitle
	{
		get { return "Phases"; }
	}
	}
}
