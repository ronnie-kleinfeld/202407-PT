using System.Collections.Generic;

namespace Doughnut
{
    public class Dataset
    {
        public List<int> data { get; set; }
        public List<string> backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string borderWidth { get; set; }
        public string label { get; set; }
    }

    public class Data
    {
        public List<Dataset> datasets { get; set; }
        public List<string> labels { get; set; }
    }

    public class Legend
    {
        public string position { get; set; }
    }

    public class Title
    {
        public bool display { get; set; }
        public string text { get; set; }
    }

    public class Animation
    {
        public bool animateScale { get; set; }
        public bool animateRotate { get; set; }
    }

    public class Callbacks
    {
        public string label { get; set; }
    }

    public class Tooltips
    {
        public Callbacks callbacks { get; set; }
    }

    public class Options
    {
        public bool responsive { get; set; }
        public Legend legend { get; set; }
        public Title title { get; set; }
        //public Animation animation { get; set; }
        //public Tooltips tooltips { get; set; }
        public Plugins plugins { get; set; }
        public string onClick { get; set; }
    }

    public class Chart
    {
        public string type { get; set; }
        public Data data { get; set; }
        public Options options { get; set; }
    }
    public class Datalabels
    {
        public bool display { get; set; }
        public string align { get; set; }
        public string anchor { get; set; }
    }

    public class Plugins
    {
        public Datalabels datalabels { get; set; }
    }
}
