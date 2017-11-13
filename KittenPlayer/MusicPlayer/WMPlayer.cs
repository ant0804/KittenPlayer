﻿using System;
using System.Diagnostics;
using WMPLib;

namespace KittenPlayer
{
    internal class WMPlayer : Player
    {
        public WindowsMediaPlayer player = new WindowsMediaPlayer();

        public override event EventHandler OnTrackEnded;

        private static int i = 0;

        private bool Locked = false;

        public WMPlayer()
        {
            player.PlayStateChange += (x) =>
            {
                Debug.WriteLine("Disconnected for " + i + "th time"); i++;
                Debug.WriteLine(player.playState);
                if (player.playState == WMPPlayState.wmppsMediaEnded)
                {
                    Locked = true;
                    Next();
                }
                else if (player.playState == WMPPlayState.wmppsReady && Locked)
                {
                    player.controls.play();
                    Locked = false;
                }
            };
        }

        private void LoadNextTrack()
        {
        }

        public override void Load(Track track)
        {
            Debug.WriteLine("Volume: " + player.settings.volume);
            player.URL = track.filePath;
            player.currentMedia.name = track.filePath;
            Debug.WriteLine("Current media: " + player.currentMedia.name);
            CurrentTrack = track;
            CurrentTab = track.MusicTab;
            player.controls.currentPosition = 0;
        }

        public override void Play()
        {
            Debug.WriteLine("play");
            player.controls.play();
            IsPlaying = true;
            Debug.WriteLine(player.status);
        }

        public override void Pause()
        {
            player.controls.pause();
            IsPaused = true;
        }

        public override void Stop() => player.controls.stop();

        public override void Resume()
        {
            if (IsPaused)
            {
                IsPaused = false;
                Play();
            }
        }

        public override double Volume
        {
            get { return player.settings.volume * 100; }
            set { player.settings.volume = (int)(value * 100); }
        }

        public override double Progress
        {
            get
            {
                if (!(IsPlaying || IsPaused)) return 0;
                if (player.currentMedia == null) return 0;
                double ms = player.controls.currentPosition;
                double total = player.currentMedia.duration;
                if (ms >= 0 && ms <= total)
                    return ms / total;
                else return 0;
            }
            set
            {
                if (!(IsPlaying || IsPaused)) return;
                player.controls.currentPosition = value * player.currentMedia.duration;
            }
        }

        public override double TotalMilliseconds => player.currentMedia.duration;
        private bool isPaused = false;

        public override bool IsPaused
        {
            get => isPaused;
            set => isPaused = value;
        }

        private bool isPlaying = false;

        public override bool IsPlaying
        {
            get => isPlaying;
            set => isPlaying = value;
        }
    }
}