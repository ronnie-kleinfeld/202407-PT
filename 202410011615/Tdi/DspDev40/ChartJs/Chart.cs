using System.Collections.Generic;

namespace ChartJs
{
    public class Chart
    {
        public string type { get; set; }
        public Data data { get; set; }
        public Options options { get; set; }

    }

    public class Datasets
    {
        public string label { get; set; }
        public IList<double> data { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string borderWidth { get; set; }

    }
    public class Data
    {
        public IList<string> labels { get; set; }
        public IList<Datasets> datasets { get; set; }

    }
    public class Legend
    {
        public string display { get; set; }

    }
    public class Ticks
    {
        public string beginAtZero { get; set; }

    }
    public class YAxes
    {
        public Ticks ticks { get; set; }

    }
    public class Scales
    {
        public IList<YAxes> yAxes { get; set; }

    }
    public class Options
    {
        public Title title { get; set; }
        public Legend legend { get; set; }
        public Plugins plugins { get; set; }
        public Scales scales { get; set; }
        public string onClick { get; set; }

    }
    public class Title
    {
        public bool display { get; set; }
        public string text { get; set; }
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
