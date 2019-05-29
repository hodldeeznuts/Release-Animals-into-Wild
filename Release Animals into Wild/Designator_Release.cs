using AllowTool;
using HugsLib.Utils;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Release_Animals_into_Wild
{
    public class Designator_Release : Designator_SelectableThings
    {
        public Designator_Release(ThingDesignatorDef def) : base(def)
        {

        }

        public static bool IsValidDesignationTarget(Thing t)
        {
            var p = t as Pawn;
            return p?.def != null && !p.Dead && p.Faction != null && p.Faction.IsPlayer && p.RaceProps.Animal;
        }

        public static AcceptanceReport PawnMeetsSkillRequirement(Pawn pawn, Pawn targetPawn)
        {
            if (pawn == null) return AcceptanceReport.WasRejected;
            var targetIsAnimal = targetPawn?.RaceProps != null && targetPawn.RaceProps.Animal;
            var skillPass = true;
            if (!targetIsAnimal && !skillPass)
            {
                return new AcceptanceReport("Failed Acceptance Report");
            }
            return AcceptanceReport.WasAccepted;
        }

        public static AcceptanceReport FriendlyPawnIsValidTarget(Thing t)
        {
            var result = !AllowToolUtility.PawnIsFriendly(t);
            return result ? true : new AcceptanceReport("Finish_off_floatMenu_reason_friendly".Translate());
        }

        public override AcceptanceReport CanDesignateThing(Thing t)
        {
            if (!IsValidDesignationTarget(t) || t.HasDesignation(Designation)) return false;
            return true;
        }

        public override void DesignateThing(Thing t)
        {
            if (!CanDesignateThing(t).Accepted) return;
            t.ToggleDesignation(Designation, true);
        }

        protected override DesignationDef Designation => DefDatabase<DesignationDef>.GetNamed("Release");
    }
}
