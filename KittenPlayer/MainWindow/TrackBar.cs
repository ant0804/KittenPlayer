﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KittenPlayer
{
    public partial class MainWindow : Form
    {
        private Timer trackbarTimer = new Timer();

        public void InitializeTrackbarTimer()
        {
            trackbarTimer.Tick += new EventHandler(trackbarTimer_Tick);

            trackbarTimer.Interval = 500;
            trackbarTimer.Enabled = true;
            trackbarTimer.Start();
        }

        private void trackbarTimer_Tick(object sender, EventArgs e)
        {
            if (IsHolding) return;

            if (musicPlayer.IsPlaying)
            {
                SetTrackbarTime();
            }
        }

        public void SetTrackbarTime()
        {
            int min = trackBar.Minimum;
            int max = trackBar.Maximum;
            double alpha = musicPlayer.Progress;
            if (alpha < 0 || alpha > 1) return;
            if (double.IsNaN(alpha)) return;
            trackBar.Value = (int)Math.Floor(min + alpha * (max - min));
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
        }

        private bool IsHolding = false;

        private void trackBar_MouseDown(object sender, MouseEventArgs e)
        {
            IsHolding = true;
        }

        private void trackBar_MouseUp(object sender, MouseEventArgs e)
        {
            IsHolding = false;
            if (!musicPlayer.IsPlaying) return;

            int min = trackBar.Minimum;
            int max = trackBar.Maximum;
            int val = trackBar.Value;

            int valMouse = e.X / 2;
            //trackBar.

            Debug.WriteLine("Values: " + val + " " + valMouse);

            double alpha = (double)(val - min) / (max - min);

            musicPlayer.Progress = alpha;
        }
    }
}