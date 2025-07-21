using RimWorld;
using Verse;
using Verse.AI;

namespace FecalToCompost;

public class WorkGiver_emptySewage : WorkGiver_Scanner
{
    public override ThingRequest PotentialWorkThingRequest =>
        ThingRequest.ForDef(DefOfs.SepticTank);


    public override PathEndMode PathEndMode => PathEndMode.Touch;

    public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        //Pawns will only empty sewage if it is above certain point.
        return t is Building_SepticTank septicBuilding && septicBuilding.GetSewage() > 800 && !t.IsBurning() &&
               !t.IsForbidden(pawn) && pawn.CanReserveAndReach(t, PathEndMode.Touch, pawn.NormalMaxDanger());
    }

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        return new Job(DefOfs.emptySewage, t);
    }
}