using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChartJs
{
    public class ChartJs
    {
        private int errorCode = 0;
        private string errorDesk = "";

        public int ErrorCode
        {
            get
            {
                return this.errorCode;
            }
            set
            {
                this.errorCode = value;
            }
        }
        public string ErrorDesk
        {
            get
            {
                return this.errorDesk;
            }
            set
            {
                this.errorDesk = value;
            }
        }

        public string getChartJsPieScript(string canvasId, string chartType, double[,] data_, string[] dataLabels, string[] dataLabel, string[] backgroundColor, bool valueDisplay,string titleText, bool titleTextDisplay)
        //---------------------------
        //Ilia Bulaevskiy 28/05/2021.
        //---------------------------
        {
            StringBuilder sb = null;

            Doughnut.Data d = new Doughnut.Data();
            try
            {
                d.labels = new List<string>();
                d.datasets = new List<Doughnut.Dataset>();

                for (int i = 0; i <= dataLabel.Length - 1; i++)
                {
                    d.labels.Add(dataLabel[i]);
                }

                Doughnut.Dataset dataset; ;
                for (int j = 0; j <= data_.GetLength(1) - 1; j++)
                {
                    dataset = new Doughnut.Dataset();
                    dataset.data = new List<int>();
                    for (int i = 0; i <= data_.GetLength(0) - 1; i++)
                    {
                        dataset.data.Add((int)data_[i, j]);
                    }

                    dataset.label = dataLabels[j];
                    dataset.backgroundColor = new List<string>();
                    for (int i = 0; i <= backgroundColor.Length - 1; i++)
                    {
                        dataset.backgroundColor.Add(backgroundColor[i]);
                    }
                    dataset.borderColor = "rgba(0, 99, 132, 1)";
                    dataset.borderWidth = "1";
                    d.datasets.Add(dataset);
                }

                Doughnut.Datalabels datalabels = new Doughnut.Datalabels()
                {
                    display = valueDisplay,
                    align = "center",
                    anchor = "center"
                };

                Doughnut.Plugins plugins = new Doughnut.Plugins()
                {
                    datalabels = datalabels
                };

                Doughnut.Legend legend = new Doughnut.Legend();
                legend.position = "top";

                Doughnut.Title title = new Doughnut.Title();
                title.display = titleTextDisplay;
                title.text = titleText;

                Doughnut.Options option = new Doughnut.Options()
                {
                    responsive = true,
                    plugins = plugins,
                    legend = legend,
                    title = title,
                    onClick = "function" 
                };

                Doughnut.Chart chart = new Doughnut.Chart();
                chart.type = chartType;
                chart.data = d;
                chart.options = option;
                //----------------------
                //Script chart for Aspx.
                //----------------------
                sb = new StringBuilder();
                sb.Append("<script>\n\t");

                StringBuilder sb1 = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb1))
                using (JsonTextWriter writer = new JsonTextWriter(sw))
                {
                    writer.QuoteChar = '\'';

                    JsonSerializer ser = new JsonSerializer();
                    ser.Serialize(writer, chart);
                }

                string json1 = sb1.ToString();
                json1 = json1.Replace("'function'", "function (e, item) {GraphBack(e, item);}");

                sb.Append("var ctx = document.getElementById('").Append(canvasId).Append("').getContext('2d');\n\t");
                sb.Append("var ").Append(canvasId).Append(" = new Chart(ctx,").Append(json1).Append(");\n\t");
                sb.Append("charts.set('").Append(canvasId).Append("',").Append(canvasId).Append(");");
                sb.Append("</script>");
            }
            catch (Exception ex)
            {
                errorCode = 10;
                errorDesk = ex.Message;
            }
            return sb.ToString();
        }


        public string getChartJsScript(string canvasId, string chartType, double[,] data, string[] dataLabels, string[] dataLabel, string[] color, bool valueDisplay, string titleText, bool titleTextDisplay)
        //---------------------------
        //Ilia Bulaevskiy 28/05/2021.
        //---------------------------
        {
            double[] v = null;
            Datasets[] sdatsets = null;
            StringBuilder sb = null;
            try
            {
                sdatsets = new Datasets[data.GetLength(0)];
                for (int i = 0; i <= data.GetLength(0) - 1; i++)
                {
                    Datasets datasets = new Datasets();
                    v = new double[data.GetLength(1)];
                    for (int j = 0; j <= data.GetLength(1) - 1; j++)
                    {
                        v[j] = data[i, j];
                    }
                    datasets.label = dataLabel[i];
                    datasets.data = v;
                    datasets.backgroundColor = color[i];
                    datasets.borderColor = "rgba(0, 99, 132, 1)";
                    datasets.borderWidth = "1";
                    sdatsets[i] = datasets;
                }
                Data d = new Data()
                {
                    labels = dataLabels,
                    datasets = sdatsets
                };

                Legend legend = new Legend()
                {
                    display = "false"
                };

                Ticks ticks = new Ticks()
                {
                    beginAtZero = "true"
                };

                YAxes yAxes = new YAxes()
                {
                    ticks = ticks
                };

                YAxes[] yA = new YAxes[1];
                yA[0] = yAxes;
                Scales scales = new Scales()
                {
                    yAxes = yA
                };

                Datalabels datalabels = new Datalabels()
                {
                    display = valueDisplay,
                    align = "right",
                    anchor = "center"
                };

                Plugins plugins = new Plugins()
                {
                    datalabels = datalabels
                };

                Title title = new Title()
                {
                    display = titleTextDisplay,
                    text = titleText
                };

                Options options = new Options()
                {
                    legend = legend,
                    scales = scales,
                    plugins = plugins,
                    title = title,
                    onClick = "function" // "function(e, item){alert('ilia')}";
                };

                Chart chart = new Chart()
                {
                    type = chartType,
                    data = d,
                    options = options
                };
                //----------------------
                //Convert Class to Json.
                //----------------------
                StringBuilder sb1 = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb1))
                using (JsonTextWriter writer = new JsonTextWriter(sw))
                {
                    writer.QuoteChar = '\'';

                    JsonSerializer ser = new JsonSerializer();
                    ser.Serialize(writer, chart);
                }

                string json1 = sb1.ToString();
                json1 = json1.Replace("'function'", "function (e, item) {GraphBack(e, item);}");
                //----------------------
                //Script chart for Aspx.
                //----------------------
                sb = new StringBuilder();
                sb.Append("<script>\n\t");
                sb.Append("var ctx = document.getElementById('").Append(canvasId).Append("').getContext('2d');");
                sb.Append("var ").Append(canvasId).Append(" = new Chart(ctx,").Append(json1).Append(");\n\t");
                sb.Append("charts.set('").Append(canvasId).Append("',").Append(canvasId).Append(");");
                sb.Append("</script>");
            }
            catch (Exception ex)
            {
                errorCode = 10;
                errorDesk = ex.Message;
            }
            return sb.ToString();
        }
    }
}
