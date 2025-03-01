using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WinToucher.Models
{
    public class TouchPath : ObservableObject
    {
        public List<Point> Points { get; } = [];
        public List<long> Timestamps { get; } = [];
        public int PointerId { get; set; }

        public void AddNow(Point point)
        {
            long timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            Points.Add(point);
            Timestamps.Add(timestamp);
            OnPropertyChanged(nameof(Points));
        }

        public void Clear()
        {
            Points.Clear();
            Timestamps.Clear();
        }
    }
}