using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace Release_Animals_into_Wild
{
    public class JobDriver_Release : JobDriver
    {
        protected Pawn Victim
        {
            get
            {
                return (Pawn)this.job.targetA.Thing;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn pawn = this.pawn;
            LocalTargetInfo target = this.Victim;
            Job job = this.job;
            return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnAggroMentalState(TargetIndex.A);
            this.FailOnThingMissingDesignation(TargetIndex.A, DefDatabase<DesignationDef>.GetNamed("Release"));
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false);
            yield return Toils_General.Do(delegate
            {
                this.Victim.SetFaction(null, null);
                Messages.Message("MessageAnimalReturnedWild".Translate(this.Victim.LabelShort, this.Victim), this.Victim, MessageTypeDefOf.NegativeEvent, true);
            });
            yield break;
        }
    }
}
