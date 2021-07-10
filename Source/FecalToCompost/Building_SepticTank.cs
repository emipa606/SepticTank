using System.Collections.Generic;
using System.Text;
using DubsBadHygiene;
using Verse;
using Verse.AI;

namespace FecalToCompost
{
    public class Building_SepticTank : Building
    {
        public CompSepticTank GetCompSepticTank => GetComp<CompSepticTank>();

        public float Pcnt => GetSewage() / 960;

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
                    void action()
                    {
                        var job1 = new Job(DefDatabase<JobDef>.GetNamed("emptySewage"), this);
                        myPawn.jobs.TryTakeOrderedJob(job1);
                        myPawn.Reserve(this, job1);
                    }

                    list.Add(new FloatMenuOption("Empty Sewage", action));
                    return list;
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

        public override void Tick()
        {
        }

        public float GetSewage()
        {
            return GetCompSepticTank.sewageBuffer;
        }

        public void SetSewage(float value)
        {
            GetCompSepticTank.sewageBuffer = value;
        }

        public int EmptySewage()
        {
            //return int
            //Every
            var numberOfCompost = (int) (GetSewage() / 80);
            SetSewage(0);
            return numberOfCompost;
        }

        public override void Draw()
        {
            //Draw nothing
        }


        public override string GetInspectString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Holding {GetSewage():0.0} of 960L ({Pcnt.ToStringPercent("0.0")})");

            //Debuglines
            //stringBuilder.AppendLine("sewageBuffer : " + this.sewageBuffer);
            //stringBuilder.AppendLine("percentUsage : " + this.sewageBuffer/this.capacity);

            return stringBuilder.ToString().Trim();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            var baseGizmos = base.GetGizmos();
            var skipDubGizmo = new List<Gizmo>();
            foreach (var gizmo in baseGizmos)
            {
                if (gizmo is Command_Action action && action.icon?.name == "BurnBarrel")
                {
                    continue;
                }

                skipDubGizmo.Add(gizmo);
            }

            return skipDubGizmo;
        }
    }
}