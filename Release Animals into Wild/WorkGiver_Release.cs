using AllowTool;
using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace Release_Animals_into_Wild
{
    public class WorkGiver_Release : WorkGiver_Scanner
    {
        public static WorkGiver_Release CreateInstance()
        {
            return new WorkGiver_Release
            {
                def = AllowToolReleaseDefOf.Release
            };
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(AllowToolReleaseDefOf.ReleaseDesignation))
            {
                yield return des.target.Thing;
            }
            yield break;
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.OnCell;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn pawn2 = t as Pawn;
            if (pawn2 == null || !pawn2.RaceProps.Animal)
            {
                return false;
            }
            if (pawn.Map.designationManager.DesignationOn(t, AllowToolReleaseDefOf.ReleaseDesignation) == null)
            {
                Log.Error("Did not find release designation");
                return false;
            }
            if (pawn.Faction != t.Faction)
            {
                return false;
            }
            if (pawn2.InAggroMentalState)
            {
                return false;
            }
            LocalTargetInfo target = t;
            if (!pawn.CanReserve(target, 1, -1, null, forced))
            {
                return false;
            }
            if (pawn.WorkTagIsDisabled(WorkTags.Animals))
            {
                JobFailReason.Is("Pawn incapable of Animal Handling", null);
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(DefDatabase<JobDef>.GetNamed("Release"), t);
        }
    }
}
