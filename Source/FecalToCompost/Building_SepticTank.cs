using System.Collections.Generic;
using System.Text;
using DubsBadHygiene;
using UnityEngine;
using Verse;
using Verse.AI;

namespace FecalToCompost;

public class Building_SepticTank : Building
{
    private CompSepticTank GetCompSepticTank => GetComp<CompSepticTank>();

    private float Pcnt => GetSewage() / 960;

    public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
    {
        var list = new List<FloatMenuOption>();
        {
            if (!myPawn.CanReserve(this))
            {
                var item = new FloatMenuOption("CannotUseReserved", null);
                return new List<FloatMenuOption>
                {
                    item
                };
            }

            if (!myPawn.CanReach(this, PathEndMode.Touch, Danger.Some))
            {
                var item = new FloatMenuOption("CannotUseNoPath", null);
                return new List<FloatMenuOption>
                {
                    item
                };
            }

            if (GetSewage() >= 80)
            {
                list.Add(new FloatMenuOption("Empty Sewage", action));
                return list;

                void action()
                {
                    var job1 = new Job(DefDatabase<JobDef>.GetNamed("emptySewage"), this);
                    myPawn.jobs.TryTakeOrderedJob(job1);
                    myPawn.Reserve(this, job1);
                }
            }

            {
                var item = new FloatMenuOption("NotEnoughSewer", null);
                return new List<FloatMenuOption>
                {
                    item
                };
            }
        }
    }

    protected override void Tick()
    {
    }

    public float GetSewage()
    {
        return GetCompSepticTank.sewageBuffer;
    }

    private void setSewage(float value)
    {
        GetCompSepticTank.sewageBuffer = value;
    }

    public int EmptySewage()
    {
        //return int
        //Every
        var numberOfCompost = (int)(GetSewage() / 80);
        setSewage(0);
        return numberOfCompost;
    }

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        //Draw nothing
    }


    public override string GetInspectString()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"{GetSewage():0.0}/960L ({Pcnt.ToStringPercent("0.0")})");
        return stringBuilder.ToString().Trim();
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
        var baseGizmos = base.GetGizmos();
        var skipDubGizmo = new List<Gizmo>();
        foreach (var gizmo in baseGizmos)
        {
            if (gizmo is Command_Action { icon.name: "BurnBarrel" })
            {
                continue;
            }

            skipDubGizmo.Add(gizmo);
        }

        return skipDubGizmo;
    }
}