using RimWorld;
using Verse;
using Verse.AI;
//using DubsBadHygiene;

namespace FecalToCompost

{
    public class WorkGiver_emptySewage : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest =>
            //return ThingRequest.ForDef(ThingDef.Named("SewageTreatment"));
            ThingRequest.ForDef(ThingDefOf.SepticTank);


        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            //Pawns will only empty sewage if it is above certain point.
            return t is Building_SepticTank septicBuilding && septicBuilding.GetSewage() > 800 && !t.IsBurning() &&
                   !t.IsForbidden(pawn) && pawn.CanReserveAndReach(t, PathEndMode.Touch, pawn.NormalMaxDanger());
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(DefDatabase<JobDef>.GetNamed("emptySewage"), t);
        }
    }
}