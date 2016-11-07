using System.Collections.Generic;
using Combat_Realism.Combat_Realism.DefOfs;
using RimWorld;
using Verse;
using Verse.AI;
using static Combat_Realism.DefOfs.CR_TaleDefOf;

namespace Combat_Realism
{
    class JobDriver_HunkerDown : JobDriver
    {
        private const int getUpCheckInterval = 10;
        private bool startedIncapacitated;
        private int ticksLeft;
        private int maxTicks;
        private bool hasPeed = false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<bool>(ref startedIncapacitated, "startedIncapacitated", false, false);
            Scribe_Values.LookValue<int>(ref this.ticksLeft, "ticksLeft", 0, false);
            Scribe_Values.LookValue<int>(ref this.maxTicks, "ticksLeft", 0, false);
        }

        public override PawnPosture Posture
        {
            get
            {
                return PawnPosture.LayingAny;
            }
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);

            //Define Toil
            Toil toilWait = new Toil();
            toilWait.initAction = () =>
            {
                toilWait.actor.pather.StopDead();
            };

            Toil toilNothing = new Toil
            {
                defaultCompleteMode = ToilCompleteMode.Delay,
                //        defaultDuration = getUpCheckInterval
            };

            toilNothing.initAction = delegate
            {
                ticksLeft = Rand.Range(10, 50);
                maxTicks = ticksLeft;
            };

            toilNothing.tickAction = delegate
            {
                if (maxTicks > 30)
                {
                    if ((float)ticksLeft / maxTicks > 0.5f)
                    {
                        FilthMaker.MakeFilth(pawn.Position, CR_ThingDefOf.FilthPee, pawn.LabelIndefinite(), 3);
                    }
                    hasPeed = true;
                }
                ticksLeft--;
                if (ticksLeft <= 0)
                {
                    ReadyForNextToil();
                    if (hasPeed)
                        TaleRecorder.RecordTale(WetHimself, pawn);
                }
            };

            //toilNothing.initAction = () => {};



            Toil shootInPanic = new Toil
            {
                initAction = delegate
                {
                    Pawn pawnAgressor = TargetThingA as Pawn;
                    if (pawnAgressor != null)
                    {
                        startedIncapacitated = pawnAgressor.Downed;
                    }
                    pawn.pather.StopDead();
                },
                tickAction = delegate
                {
                    if (TargetA.HasThing)
                    {
                        if (TargetA.Thing.Destroyed || (pawn != null && !startedIncapacitated && pawn.Downed))
                        {
                            EndJobWith(JobCondition.Succeeded);
                            return;
                        }
                    }
                    pawn.equipment.TryStartAttack(TargetA);
                },
                defaultDuration = 5,
                defaultCompleteMode = ToilCompleteMode.Delay
            };

            // Start Toil
            //          yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            //        yield return shootInPanic;
            yield return toilWait;
            yield return toilNothing;
            yield return Toils_Jump.JumpIf(toilNothing, () =>
            {
                CompSuppressable comp = pawn.TryGetComp<CompSuppressable>();
                if (comp == null)
                {
                    return false;
                }
                float distToSuppressor = (pawn.Position - comp.suppressorLoc).LengthHorizontal;
                if (distToSuppressor < CompSuppressable.minSuppressionDist)
                {
                    return false;
                }
                return comp.isHunkering;
            });
        }


    }
}
