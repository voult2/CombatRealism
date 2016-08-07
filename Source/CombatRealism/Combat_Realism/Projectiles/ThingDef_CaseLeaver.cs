using UnityEngine;
using Verse;
using RimWorld;
using System;

namespace Combat_Realism
{

    public class ThingDef_CaseLeaver : ThingDef
    {
        public bool isShotgunShellDropper = false;
        public bool isBigGunDropper = false;
        public string casingMoteDefname = "Mote_EmptyCasing";
    }
}
