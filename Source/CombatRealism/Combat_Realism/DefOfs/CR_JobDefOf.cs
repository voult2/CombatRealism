﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;

namespace Combat_Realism
{
    [DefOf]
    public static class CR_JobDefOf
    {
        public static readonly JobDef ReloadWeapon = DefDatabase<JobDef>.GetNamed("ReloadWeapon");
        public static readonly JobDef ReloadTurret = DefDatabase<JobDef>.GetNamed("ReloadTurret");
        public static readonly JobDef HunkerDown = DefDatabase<JobDef>.GetNamed("HunkerDown");
        public static readonly JobDef RunForCover = DefDatabase<JobDef>.GetNamed("RunForCover");
    }
}
