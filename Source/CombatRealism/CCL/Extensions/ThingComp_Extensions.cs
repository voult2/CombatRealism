using System.Collections.Generic;
using Verse;
using RimWorld;

namespace CommunityCoreLibrary
{

	public static class ThingComp_Extensions
	{

        // TODO:  Is any of this really needed?  1000101

		#region Comp Properties

		public static CompProperties_Rottable CompProperties_Rottable( this ThingComp thingComp )
		{
			return thingComp.props as CompProperties_Rottable;
		}

		#endregion

	}

}
