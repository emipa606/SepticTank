using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using Verse;
using Verse.AI;
//using DubsBadHygiene;


namespace FecalToCompost
{
    public class JobDriver_emptySewage : JobDriver
    {
        protected Building_SepticTank SewageProcessing => (Building_SepticTank) job.GetTarget(TargetIndex.A).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job);
        }

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(1400).WithProgressBarToilDelay(TargetIndex.A);
            yield return new Toil
            {
                initAction = delegate
                {
                    var thing = ThingMaker.MakeThing(ThingDef.Named("RawCompost"));
                    thing.stackCount = SewageProcessing.EmptySewage();
                    GenPlace.TryPlaceThing(thing, pawn.Position, Map, ThingPlaceMode.Near);
                    //StoragePriority currentPriority = StoreUtility.StoragePriorityAtFor(thing.Position, thing);
                    if (StoreUtility.TryFindBestBetterStoreCellFor(thing, pawn, Map, StoragePriority.Normal,
                        pawn.Faction, out var c))
                    {
                        job.SetTarget(TargetIndex.C, c);
                        job.SetTarget(TargetIndex.B, thing);
                        job.count = thing.stackCount;
                    }
                    else
                    {
                        EndJobWith(JobCondition.Incompletable);
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return Toils_Reserve.Reserve(TargetIndex.B);
            yield return Toils_Reserve.Reserve(TargetIndex.C);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B);
            var toil = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
            yield return toil;
            yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, toil, true);
        }
    }
}