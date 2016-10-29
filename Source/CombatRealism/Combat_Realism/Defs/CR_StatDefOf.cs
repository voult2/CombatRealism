using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Combat_Realism.Combat_Realism.Defs
{
    public class CR_StatDefOf
    {
        public static StatDef Bulk = StatDef.Named("Bulk"); // for items in inventory
        public static StatDef CarryBulk = StatDef.Named("CarryBulk"); // pawn capacity
        public static StatDef CarryWeight = StatDef.Named("CarryWeight"); // pawn capacity
        public static StatDef Weight = StatDef.Named("Weight"); // items in inventory
        public static StatDef WornBulk = StatDef.Named("WornBulk"); // apparel offsets
        public static StatDef WornWeight = StatDef.Named("WornWeight"); // apparel offsets
    }
}
