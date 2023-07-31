
using System.Xml.Serialization;

namespace XTC.FMP.MOD.ModelSee.LIB.Unity
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class MyConfig : MyConfigBase
    {
        public class SwipH
        {
            [XmlAttribute("speed")]
            public float speed { get; set; }
        }

        public class SwipV
        {
            [XmlAttribute("speed")]
            public float speed { get; set; }
        }

        public class Pinch
        {
            [XmlAttribute("speed")]
            public float speed { get; set; }
            [XmlAttribute("maxScale")]
            public float maxScale { get; set; }
            [XmlAttribute("minScale")]
            public float minScale { get; set; }
        }

        public class Gesture
        {
            [XmlElement("SwipH")]
            public SwipH swipH { get; set; }
            [XmlElement("SwipV")]
            public SwipV swipV { get; set; }
            [XmlElement("Pinch")]
            public Pinch pinch { get; set; }
        }

        public class Debug
        {
            [XmlAttribute("active")]
            public bool active { get; set; } = false;
            [XmlAttribute("moveSpeed")]
            public float moveSpeed { get; set; } = 1;
            [XmlAttribute("pivotScale")]
            public float pivotScale { get; set; } = 0.1f;
        }

        public class Tab
        {
            [XmlAttribute("checkedImage")]
            public string checkedImage { get; set; }
            [XmlAttribute("uncheckedImage")]
            public string uncheckedImage { get; set; }
            [XmlAttribute("width")]
            public int width { get; set; }
            [XmlAttribute("height")]
            public int height { get; set; }
            [XmlAttribute("fontSize")]
            public int fontSize { get; set; }
            [XmlAttribute("fontColor")]
            public string fontColor { get; set; }
        }

        public class SideMenu
        {
            [XmlElement("FocusTab")]
            public Tab focusTab { get; set; } = new Tab();
            [XmlElement("FeatureTab")]
            public Tab featureTab { get; set; } = new Tab();
            [XmlAttribute("itemWidth")]
            public int itemWidth { get; set; }
            [XmlAttribute("itemHeight")]
            public int itemHeight { get; set; }
            [XmlAttribute("itemSpace")]
            public int itemSpace { get; set; }
        }

        public class FeaturePanel
        {
            [XmlAttribute("tabSpace")]
            public int tabSpace { get; set; }
            [XmlElement("FeatureTab")]
            public Tab featureTab { get; set; } = new Tab();
            [XmlElement("FeatureMask")]
            public FeatureMask featureMask { get; set; } = new FeatureMask();
            [XmlElement("FeatureCard")]
            public FeatureCard featureCard { get; set; } = new FeatureCard();
        }

        public class FeatureMask
        {
            [XmlAttribute("active")]
            public bool active { get; set; } = false;
            [XmlAttribute("image")]
            public string image { get; set; }
        }

        public class FeatureCard
        {
            [XmlAttribute("image")]
            public string image { get; set; }
            [XmlAttribute("border")]
            public int border { get; set; }
            [XmlAttribute("width")]
            public int width { get; set; }
            [XmlAttribute("height")]
            public int height { get; set; }
            [XmlAttribute("nameFontSize")]
            public int nameFontSize { get; set; }
            [XmlAttribute("nameFontColor")]
            public string nameFontColor { get; set; }
            [XmlAttribute("descriptionFontSize")]
            public int descriptionFontSize { get; set; }
            [XmlAttribute("descriptionFontColor")]
            public string descriptionFontColor { get; set; }
        }


        public class GridSpace
        {
            [XmlAttribute("anchorX")]
            public int anchorX { get; set; } = 0;
            [XmlAttribute("anchorY")]
            public int anchorY { get; set; } = 0;
            [XmlAttribute("anchorZ")]
            public int anchorZ { get; set; } = 0;
            [XmlAttribute("cellSize")]
            public int cellSize { get; set; } = 10;
        }

        public class Style
        {
            [XmlAttribute("name")]
            public string name { get; set; } = "";
            [XmlElement("Debug")]
            public Debug debug { get; set; } = new Debug();
            [XmlElement("GridSpace")]
            public GridSpace gridSpace { get; set; } = new GridSpace();
            [XmlElement("SideMenu")]
            public SideMenu sideMenu { get; set; } = new SideMenu();
            [XmlElement("FeaturePanel")]
            public FeaturePanel featurePanel { get; set; } = new FeaturePanel();
            [XmlElement("Gesture")]
            public Gesture gesture { get; set; } = new Gesture();
        }


        [XmlArray("Styles"), XmlArrayItem("Style")]
        public Style[] styles { get; set; } = new Style[0];
    }
}

