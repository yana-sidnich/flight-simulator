using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlightGearTestExec.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using static FlightGearTestExec.Models.FlightDataContainer;

namespace FlightGearTestExec.Views
{
    public partial class MapsView : UserControl
    {
        private MapsViewModel vm;

        private SKPaint paintLine;

        private SKPath path;

        private List<float[]> gpsDataXY;

        private SKBitmap bitmap = null;

        private int line_number;

        private int last_draw = 0;
        public MapsView()
        {
            path = new SKPath();
            paintLine = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                Color = SKColors.Black,
                StrokeWidth = 5,
                StrokeCap = SKStrokeCap.Butt,
                StrokeJoin = SKStrokeJoin.Bevel
            };

            InitializeComponent();
            vm = DataContext as MapsViewModel;
            gpsDataXY = vm.GpsDataXY;
            try
            {
                bitmap = SKBitmap.Decode(vm.VM_Maps_BitmapArray);
            }
            catch (Exception e)
            {
                Debug.Print("no bitmap available in vm");
            }

            vm.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName == "VM_Maps_BitmapArray")
                    {
                        bitmap = SKBitmap.Decode(vm.VM_Maps_BitmapArray);
                    }
                    else if (e.PropertyName == "VM_Maps_CurrentLineNumber")
                    {
                        line_number = vm.VM_Maps_CurrentLineNumber;
                    }
                };
        }



        public void doneDraggingThreshold(object sender, RoutedEventArgs e)
        {
            // after zoom has changed - read again xy locations
            gpsDataXY = vm.GpsDataXY;
            // after zoom has changed - reset points
            path.Reset();
            last_draw = 0;
        }

        private async void OnPaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            // the canvas and properties
            var canvas = e.Surface.Canvas;

            // get the screen density for scaling
            var scale = (float)PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;

            // handle the device screen density
            canvas.Scale(scale);

            // make sure the canvas is blank
            canvas.Clear(SKColors.White);

            if (last_draw > line_number)
            {
                path.Reset();
                last_draw = 0;
            }
            if (last_draw == 0)
            {
                path.MoveTo(gpsDataXY[0][0], gpsDataXY[0][1]);
                last_draw++;
            }
            for (int i = last_draw; i < line_number; i++)
            {
                path.AddCircle(gpsDataXY[i][0], gpsDataXY[i][1], 1);
                last_draw++;
            }

            if (bitmap != null)
            {
                canvas.DrawBitmap(bitmap, 0, 0);
                canvas.DrawPath(path, paintLine);
            }
            // check if redraw needs every 5 seconds
            await Task.Delay(5);
            skia.InvalidateVisual();
        }

    }
}

