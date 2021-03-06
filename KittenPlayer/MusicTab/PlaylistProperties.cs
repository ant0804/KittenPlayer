﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace KittenPlayer
{
    public partial class MusicTab : UserControl
    {
        private readonly List<string> Parameters = new List<string>
        {
            "Artist",
            "Album",
            "Title"
        };

        private void InitPlaylistProperties()
        {
            PlaylistProperties.Items.Clear();
            foreach (var Parameter in Parameters)
                PlaylistProperties.Items.Add(Parameter);
            foreach (ToolStripMenuItem Item in PlaylistProperties.Items)
                Item.CheckOnClick = true;

            ChangeTitleToolStripMenuItem.Click += (x, y) => { ChangeSelectedProperty(0); };
            ChangeArtistToolStripMenuItem.Click += (x, y) => { ChangeSelectedProperty(1); };
            ChangeAlbumToolStripMenuItem.Click += (x, y) => { ChangeSelectedProperty(2); };
            ChangeTrackNumberToolStripMenuItem.Click += (x, y) => { ChangeSelectedProperty(3); };
        }

        private void PlaylistProperties_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                PlaylistProperties.AutoClose = true;
                PlaylistProperties.Hide();
            }
        }

        private void PlaylistProperties_Opened(object sender, EventArgs e)
        {
            PlaylistProperties.AutoClose = false;
        }

        public int GetFocusedItem()
        {
            var ItemIndex = -1;
            foreach (int Index in PlaylistView.SelectedIndices)
                if (PlaylistView.Items[Index].Focused)
                    ItemIndex = Index;

            return ItemIndex;
        }

        public void ChangeSelectedProperty(int SubItemIndex)
        {
            var Indices = PlaylistView.SelectedIndices;
            if (Indices.Count == 0) return;

            var ItemIndex = GetFocusedItem();
            //new RenameBox(PlaylistView, SubItemIndex);

            if (ItemIndex < PlaylistView.Items.Count)
            {
                var Item = PlaylistView.Items[ItemIndex];
                if (Item.SubItems.Count == 0) return;
                if (SubItemIndex == 0)
                {
                    Item.BeginEdit();
                }
                else if (SubItemIndex < Item.SubItems.Count)
                {
                    var renameBox = new RenameBox(PlaylistView, SubItemIndex);
                    renameBox?.Focus();
                }
            }
        }

        public void AfterSubItemEdit(ListViewItem Item)
        {
            var Index = PlaylistView.Items.IndexOf(Item);
            if (Index < 0 || Index >= Tracks.Count) return;
            var track = Tracks[Index];
            track.SetMetadata();
        }

        private void PlaylistView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (sender is ListViewItem s)
                AfterSubItemEdit(s);
        }

        private void DropDownMenu_Opening(object sender, CancelEventArgs e)
        {
            if (PlaylistView.SelectedIndices.Count == 0)
            {
                DropDownMenu.Hide();
            }
            else
            {
                var Index = PlaylistView.SelectedIndices[0];
                foreach (ToolStripItem Item in DropDownMenu.Items)
                    if (Item.Text == "Properties")
                        Item.Enabled = Tracks[Index].Writeable;
            }
        }
    }
}