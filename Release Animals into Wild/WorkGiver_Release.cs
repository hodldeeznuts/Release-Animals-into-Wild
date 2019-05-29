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
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DefDatabase<DesignationDef>.GetNamed("Release")))
            {
                yield return des.target.Thing;
            }
            yield break;
        }

        // Token: 0x170000DF RID: 223
        // (get) Token: 0x06000618 RID: 1560 RVA: 0x0003A637 File Offset: 0x00038A37
        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.OnCell;
            }
        }

        // Token: 0x06000619 RID: 1561 RVA: 0x0003A63C File Offset: 0x00038A3C
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn pawn2 = t as Pawn;
            if (pawn2 == null || !pawn2.RaceProps.Animal)
            {
                return false;
            }
            if (pawn.Map.designationManager.DesignationOn(t, DefDatabase<DesignationDef>.GetNamed("Release")) == null)
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
            if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Animals))
            {
                JobFailReason.Is("Pawn incapable of Animal Handling", null);
                return false;
            }
            return true;
        }

        // Token: 0x0600061A RID: 1562 RVA: 0x0003A6F1 File Offset: 0x00038AF1
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(DefDatabase<JobDef>.GetNamed("Release"), t);
        }
    }
}
