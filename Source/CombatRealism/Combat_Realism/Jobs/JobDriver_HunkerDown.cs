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
        private Toil toilHunkerDown;
        private bool startedIncapacitated;
        private int ticksLeft;
        private int maxTicks;
        private bool willPee;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue(ref startedIncapacitated, "startedIncapacitated", false, false);
            Scribe_Values.LookValue(ref ticksLeft, "ticksLeft", 0, false);
            Scribe_Values.LookValue(ref maxTicks, "maxTicks", 0, false);
            Scribe_Values.LookValue(ref willPee, "willPee", false, false);
        }

        public override PawnPosture Posture
        {
            get
            {
                return (CurToil != toilHunkerDown) ? PawnPosture.Standing : PawnPosture.LayingAny;
            }
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);

            //Define Toil

            toilHunkerDown = new Toil
            {
                initAction = delegate
                {
                    maxTicks = Rand.Range(60, 240);
                    ticksLeft = maxTicks;
                    willPee = maxTicks>35;
                    toilHunkerDown.actor.pather.StopDead();
                },
                tickAction = delegate
                {
                    ticksLeft--;
                    if (willPee)
                    {
                        if (ticksLeft % maxTicks == maxTicks - 1)
                        {
                      //      MoteMaker.ThrowMetaIcon(pawn.Position, ThingDefOf.Mote_Heart);
                            FilthMaker.MakeFilth(pawn.Position, CR_ThingDefOf.FilthPee, pawn.LabelIndefinite(), 1);
                        }
                    }
                    if (ticksLeft <= 0)
                    {
                        ReadyForNextToil();
                        if (willPee)
                            TaleRecorder.RecordTale(WetHimself, pawn);
                        
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Never,
                
            };


            CompSuppressable comp = pawn.TryGetComp<CompSuppressable>();
            toilHunkerDown.FailOn(() => comp == null);
            if (comp != null)
            {
                float distToSuppressor = (pawn.Position - comp.suppressorLoc).LengthHorizontal;
                toilHunkerDown.FailOn(() => distToSuppressor < CompSuppressable.minSuppressionDist);
                toilHunkerDown.FailOn(() => !comp.isHunkering);
            }

            if (willPee)
                toilHunkerDown.WithEffect("Pee", TargetIndex.A);


            // Start Toil

            yield return toilHunkerDown;
        }


    }
}
