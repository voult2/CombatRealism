using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace Combat_Realism
{
    public class MassBulkUtility
    {
        public const float WeightCapacityPerBodySize = 35f;
        public const float BulkCapacityPerBodySize = 20f;

        public static float BaseCarryWeight(Pawn p)
        {
            return p.BodySize * WeightCapacityPerBodySize;
        }

        public static float BaseCarryBulk(Pawn p)
        {
            return p.BodySize * BulkCapacityPerBodySize;
        }


    }
}
