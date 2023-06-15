using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity
{
    public class ManifestSchema
    {
        public class Point3
        {
            public float x { get; set; }
            public float y { get; set; }
            public float z { get; set; }
        }

        public class RenderCamera
        {
            public Point3 position { get; set; }
        }

        public class Pivot
        {
            public Point3 position { get; set; }
        }

        public class Focus
        {
            public RenderCamera renderCamera { get; set; }
            public Pivot pivot { get; set; }
        }

        public class Feature
        {
            public string name { get; set; }
            public string description { get; set; }
            public RenderCamera renderCamera { get; set; }
            public Pivot pivot { get; set; }
        }

        public Focus focus { get; set; }
        public Feature[] featureS { get; set; }
    }//class

}//namespace