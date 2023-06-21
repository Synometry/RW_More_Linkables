using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Hobbes.MoreLinkables {
    public class CompProperties_SecondLayer : CompProperties {
        public GraphicData graphicData = null;

        //public AltitudeLayer altitude = AltitudeLayer.BuildingOnTop;

        //public float Y => altitude.AltitudeFor() - 0.1f;

        public CompProperties_SecondLayer() {
            compClass = typeof(CompSecondLayer);
        }
    }

    public class CompSecondLayer : ThingComp {

        private Graphic graphicInt;

        private float Y => parent.def.altitudeLayer.AltitudeFor() + 0.2f;

        public Graphic Graphic {
            get {
                if (graphicInt == null) {
                    graphicInt = Props.graphicData.GraphicColoredFor(parent);
                }
                return graphicInt;
            }
        }
        public CompProperties_SecondLayer Props => (CompProperties_SecondLayer) props;

        public override void PostDraw() {
                Graphic.Draw(GenThing.TrueCenter(parent.Position, parent.Rotation, parent.def.size, Y), parent.Rotation, parent);
        }
    }

    public class CompProperties_WorkHelper : CompProperties {
        public float radius = 5f;
        public int tickInterval = 100;

        public CompProperties_WorkHelper() {
            compClass = typeof(CompWorkHelper);
        }
    }

    public class CompWorkHelper : ThingComp {

        protected CompPowerTrader compPower;

        private CompProperties_WorkHelper Props => (CompProperties_WorkHelper)props;
        
        private IntVec3[] areaCache;

        private IntVec3[] Area {
            get {
                if (areaCache == null) {
                    areaCache = GenRadial.RadialCellsAround(parent.Position, Props.radius, false).ToArray();
                }
                return areaCache;
            }
        }

        public override void CompTickLong() {
            base.CompTickLong();
            if (compPower.PowerOn) {
                DoWork();
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad) {
            base.PostSpawnSetup(respawningAfterLoad);
            compPower = parent.GetComp<CompPowerTrader>();
        }

        private void DoWork() {
            IEnumerable<Thing> things = WorkTargets().TakeRandom(1);
            foreach (Thing t in things) {
                UnfinishedThing ut = (UnfinishedThing) t;
                ut.workLeft = Math.Max(1f, ut.workLeft - 60f);
                //Log.Message($"work being done on {t.Label}; work left: {((UnfinishedThing) t).workLeft}");
            }
        }

        private List<Thing> WorkTargets() {
            List<Thing> targets = new List<Thing>();
            for (int i = 0; i < Area.Length; i++) {
                if (!Area[i].InBounds(parent.Map)) {
                    continue;
                }
                foreach (Thing t in Area[i].GetItems(parent.Map)) {
                    if (t.def.isUnfinishedThing && ((UnfinishedThing)t).workLeft > 1f) targets.Add(t);
                }
            }
            return targets;
        }
    }
}
