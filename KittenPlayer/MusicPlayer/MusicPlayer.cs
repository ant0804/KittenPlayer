﻿using System;

namespace KittenPlayer
{
    public partial class MusicPlayer
    {
        public static MusicPlayer Instance = new MusicPlayer();
        public Player player;

        private MusicPlayer()
        {
            //OperatingSystem OSVersion = Environment.OSVersion;
            //OSVersion = new OperatingSystem(PlatformID.Win32NT, new Version(5, 1));
            //if (OSVersion.Version.Major < 6)
            player = new WMPlayer();
            //else player = new MFPlayer();

            player.OnTrackEnded += OnTrackEnd;
        }

        private void OnTrackEnd(object sender, EventArgs args)
        {
            //Track track = CurrentTab.GetNextTrack(player.CurrentTrack);
            //CurrentTab.Play(CurrentTab.Tracks.IndexOf(track));
        }

        public Track CurrentTrack = null;
        public MusicTab CurrentTab = null;

        public double Progress
        {
            get { return player.Progress; }
            set { player.Progress = value; }
        }

        public double Volume
        {
            get { return player.Volume; }
            set { player.Volume = value; }
        }

        private String GetTime()
        {
            if (player.IsPlaying)
            {
                int seconds = (int)player.TotalMilliseconds / 1000 % 60;
                int minutes = (int)player.TotalMilliseconds / 1000 / 60 % 60;
                int hours = (int)player.TotalMilliseconds / 1000 / 60 / 60;

                if (hours > 0)
                {
                    return String.Format("{0}:{1:00}:{2:00}", hours, minutes, seconds);
                }
                else
                {
                    return String.Format("{0:00}:{1:00}", minutes, seconds);
                }
            }
            else return "0:00";
        }

        public bool IsPlaying { get => player.IsPlaying; }
        public bool IsPaused { get => player.IsPaused; }

        public void Load(Track track) =>
            player.Load(track);

        public void Play(Track track)
        {
            Load(track);
            player.Play();
        }

        public void Play() => player.Play();

        public void Pause() => player.Pause();

        public void Stop() => player.Stop();

        public void Next() => player.Next();

        public void Previous() => player.Previous();

        public void PlayAutomatic()
        {
            MusicTab tab = MainWindow.ActiveTab;
            int Index = tab.PlaylistView.SelectedIndices[0];
            Play(tab.Tracks[Index]);
        }
    }
}