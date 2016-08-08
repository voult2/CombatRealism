using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.Sound;
using RimWorld;
using UnityEngine;

namespace Combat_Realism
{
    public class BulletCR : ProjectileCR
    {
        private const float StunChance = 0.1f;
        
        public bool isShotgunShellDropper = false;
        public bool isBigGunDropper = false;
        public string casingMoteDefname = "Mote_EmptyCasing";


        public override void SpawnSetup()
        {
            base.SpawnSetup();
            ProjectilePropertiesCR propsCR = def.projectile as ProjectilePropertiesCR;
            if (propsCR.DropsCasings == true)
            {
                this.ThrowEmptyCasing(base.Position.ToVector3Shifted(), 1f);
            }
        }

        public void ThrowEmptyCasing(Vector3 loc, float size)
        {
            ThingDef_CaseLeaver thingDef_CaseLeaver = this.def as ThingDef_CaseLeaver;
            bool isShotgunShellDropper = thingDef_CaseLeaver.isShotgunShellDropper;
            bool isBigGunDropper = thingDef_CaseLeaver.isBigGunDropper;
            string casingMoteDefname = thingDef_CaseLeaver.casingMoteDefname;
            if (isShotgunShellDropper)
            {
                casingMoteDefname = "Mote_ShotgunShell";
            }
            if (isBigGunDropper)
            {
                casingMoteDefname = "Mote_BigShell";
            }
            if (!loc.ShouldSpawnMotesAt() || MoteCounter.SaturatedLowPriority)
            {
                return;
            }
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named(casingMoteDefname), null);
            moteThrown.ScaleUniform = Rand.Range(0.5f, 0.3f) * size;
            moteThrown.exactRotationRate = Rand.Range(-3f, 4f);
            moteThrown.exactPosition = loc;
            moteThrown.airTicksLeft = 60;
            moteThrown.SetVelocityAngleSpeed((float)Rand.Range(160, 200), Rand.Range(0.020f, 0.0115f));
            if (--moteThrown.airTicksLeft <= 35)
            {
                moteThrown.airTicksLeft = 0;
                moteThrown.ExactSpeed = 0f;
                moteThrown.exactRotationRate = 0f;
                this.def.mote.landSound.PlayOneShot(base.Position);
            }
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3());
        }

        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
            if (hitThing != null)
            {
                int damageAmountBase = this.def.projectile.damageAmountBase;

                BodyPartDamageInfo value;
                DamageDef_CR damDefCR = def.projectile.damageDef as DamageDef_CR;
                if (damDefCR != null && damDefCR.harmOnlyOutsideLayers) value = new BodyPartDamageInfo(null, BodyPartDepth.Outside);
                else value = new BodyPartDamageInfo(null, null);

                DamageInfo dinfo = new DamageInfo(this.def.projectile.damageDef, damageAmountBase, this.launcher, this.ExactRotation.eulerAngles.y, new BodyPartDamageInfo?(value), this.def);

                ProjectilePropertiesCR propsCR = def.projectile as ProjectilePropertiesCR;
                if (propsCR != null && !propsCR.secondaryDamage.NullOrEmpty())
                {
                    // Get the correct body part
                    Pawn pawn = hitThing as Pawn;
                    if (pawn != null && def.projectile.damageDef.workerClass == typeof(DamageWorker_AddInjuryCR))
                    {
                        dinfo = new DamageInfo(dinfo.Def, 
                            dinfo.Amount, 
                            dinfo.Instigator, 
                            dinfo.Angle, 
                            new BodyPartDamageInfo(DamageWorker_AddInjuryCR.GetExactPartFromDamageInfo(dinfo, pawn), false, (HediffDef)null), 
                            dinfo.Source);
                    }
                    List<DamageInfo> dinfoList = new List<DamageInfo>() { dinfo };
                    foreach(SecondaryDamage secDamage in propsCR.secondaryDamage)
                    {
                        dinfoList.Add(new DamageInfo(secDamage.def, secDamage.amount, dinfo.Instigator, dinfo.Part, dinfo.Source));
                    }
                    foreach(DamageInfo curDinfo in dinfoList)
                    {
                        hitThing.TakeDamage(curDinfo);
                    }
                }
                else
                {
                    hitThing.TakeDamage(dinfo);
                }
            }
            else
            {
                SoundDefOf.BulletImpactGround.PlayOneShot(base.Position);
                MoteThrower.ThrowStatic(this.ExactPosition, ThingDefOf.Mote_ShotHit_Dirt, 1f);
            }
        }
    }
}
