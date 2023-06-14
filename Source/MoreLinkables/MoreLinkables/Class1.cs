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
}
