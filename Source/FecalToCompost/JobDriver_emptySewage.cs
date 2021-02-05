using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;
//using DubsBadHygiene;


namespace FecalToCompost
{
	public class JobDriver_emptySewage : JobDriver
	{
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(job.targetA, job, 1, -1, (ReservationLayerDef)null);
		}

        protected Building_SepticTank SewageProcessing => (Building_SepticTank)base.job.GetTarget(TargetIndex.A).Thing;

        [DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(1400).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate
				{
					
					Thing thing = ThingMaker.MakeThing(ThingDef.Named("RawCompost"), null);
					thing.stackCount = SewageProcessing.EmptySewage();
					GenPlace.TryPlaceThing(thing, pawn.Position, Map, ThingPlaceMode.Near, null);
					//StoragePriority currentPriority = StoreUtility.StoragePriorityAtFor(thing.Position, thing);
                    if (StoreUtility.TryFindBestBetterStoreCellFor(thing, pawn, Map, StoragePriority.Normal, pawn.Faction, out IntVec3 c, true))
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
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1);
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false);
			Toil toil = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return toil;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, toil, true);
			yield break;
		}
	}


}