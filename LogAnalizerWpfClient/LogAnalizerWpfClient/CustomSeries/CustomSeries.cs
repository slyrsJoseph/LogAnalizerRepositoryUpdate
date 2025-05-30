
using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;


namespace LogAnalizerWpfClient.CustomSeries
{
    public class RectangleBarSeries : XYAxisSeries
    {
        public List<RectangleBarItem> Items { get; } = new List<RectangleBarItem>();

        public OxyColor FillColor { get; set; } = OxyColors.Automatic;

       public override void Render(IRenderContext rc)
        {
            var xAxis = this.XAxis;
            var yAxis = this.YAxis;

            foreach (var item in this.Items)
            {
                var p0 = xAxis.Transform(item.X0, item.Y0, yAxis);
                var p1 = xAxis.Transform(item.X1, item.Y1, yAxis);
                rc.DrawRectangle(
                    new OxyRect(p0.X, p1.Y, p1.X - p0.X, p0.Y - p1.Y),
                    this.FillColor,
                    OxyColors.Undefined,
                    0, 
                    EdgeRenderingMode.Automatic);
            }
        }

        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            rc.DrawRectangle(legendBox, this.FillColor, OxyColors.Black, 1, EdgeRenderingMode.Automatic);
        }

     protected  override void UpdateAxisMaxMin()
        {
            base.UpdateAxisMaxMin();
            foreach (var item in this.Items)
            {
                this.XAxis.Include(item.X0);
                this.XAxis.Include(item.X1);
                this.YAxis.Include(item.Y0);
                this.YAxis.Include(item.Y1);
            }
        }

      protected  override void UpdateMaxMin()
        {
            foreach (var item in this.Items)
            {
                this.MinX = Math.Min(this.MinX, Math.Min(item.X0, item.X1));
                this.MaxX = Math.Max(this.MaxX, Math.Max(item.X0, item.X1));
                this.MinY = Math.Min(this.MinY, Math.Min(item.Y0, item.Y1));
                this.MaxY = Math.Max(this.MaxY, Math.Max(item.Y0, item.Y1));
            }
        }
    }

    public class RectangleBarItem
    {
        public double X0 { get; set; }
        public double X1 { get; set; }
        public double Y0 { get; set; }
        public double Y1 { get; set; }

        public RectangleBarItem() { }

        public RectangleBarItem(double x0, double x1, double y0, double y1)
        {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;
        }
    }
}
