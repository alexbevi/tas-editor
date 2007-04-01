/******************************************************************************** 
 * TAS Movie Editor                                                             *
 *                                                                              *
 * Copyright notice for this file:                                              *
 *  Copyright (C) 2006-7 Maximus                                                *
 *                                                                              *
 * This program is free software; you can redistribute it and/or modify         *
 * it under the terms of the GNU General Public License as published by         *
 * the Free Software Foundation; either version 2 of the License, or            *
 * (at your option) any later version.                                          *
 *                                                                              *
 * This program is distributed in the hope that it will be useful,              *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                *
 * GNU General Public License for more details.                                 *
 *                                                                              *
 * You should have received a copy of the GNU General Public License            *
 * along with this program; if not, write to the Free Software                  *
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA    *
 *******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using MovieSplicer.Data;

namespace MovieSplicer.Components
{
    /// <summary>
    /// This is essentially a subclass of System.Windows.Forms.ListView that provides
    /// an auto-expanding last column and virtualization.   
    /// </summary>
    public class TASListView: ListView
    {     
        [DllImport("User32.dll")]
        private static extern bool SendMessage(IntPtr hwnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern int GetScrollInfo(IntPtr hWnd, int n, ref ScrollInfoStruct lpScrollInfo);
        
        private struct ScrollInfoStruct
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }
        
        public event ScrollEventHandler Scrolled = null;

        private const int WM_PAINT        = 0xF;
        private const int WM_SCROLL_WHEEL = 0x20A;        

        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;

        private const int SB_LINELEFT = 0;
        private const int SB_LINERIGHT = 1;
        private const int SB_PAGELEFT = 2;
        private const int SB_PAGERIGHT = 3;
        private const int SB_THUMBPOSITION = 4;
        private const int SB_THUMBTRACK = 5;
        private const int SB_LEFT = 6;
        private const int SB_RIGHT = 7;
        private const int SB_ENDSCROLL = 8;

        private const int SIF_TRACKPOS = 0x10;
        private const int SIF_RANGE = 0x1;
        private const int SIF_POS = 0x4;
        private const int SIF_PAGE = 0x2;
        private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;

        public TASMovieInput[]   VirtualListSource;
        public TASForm.MovieType VirtualMovieType;

        public TASListView AssociatedListView;

        // Cache items
        private ListViewItem[] cache;
        private int            firstItem;        
        
        /// <summary>
        /// Create a new TASListView object with an event handler on RetriveVirtualItem
        /// </summary>
        public TASListView()
        {
            this.DoubleBuffered = true;
            this.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(GetVirtualItem);
            this.CacheVirtualItems   += new CacheVirtualItemsEventHandler(CacheVirtualItemsList);                         
        }               
       
        /// <summary>
        // Various message handlers for this control
        /// </summary>
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                // if the control is in details view mode and columns
                // have been added, then intercept the WM_PAINT message
                // and reset the last column width to fill the list view
                case WM_PAINT:
                    if (this.View == View.Details && this.Columns.Count > 0)
                        this.Columns[this.Columns.Count - 1].Width = -2;
                    break;
                case WM_VSCROLL:
                    if (Scrolled != null)
                    {
                        ScrollInfoStruct si = new ScrollInfoStruct();
                        si.fMask = SIF_ALL;
                        si.cbSize = Marshal.SizeOf(si);
                        GetScrollInfo(message.HWnd, 0, ref si);

                        if (message.WParam.ToInt32() == SB_ENDSCROLL)
                        {
                            ScrollEventArgs sargs = new ScrollEventArgs(
                                ScrollEventType.EndScroll,
                                si.nPos);
                            Scrolled(this, sargs);                            
                        }
                    }
                    break;
                //case WM_SCROLL_WHEEL:
                //    if (AssociatedListView != null)
                //    {
                //        int myInt = message.WParam.ToInt32();
                //        int IntLow = myInt & 0xffff;
                //        long IntHigh = ((long)myInt & 0xffff0000) >> 16;
                //        SendMessage(this.AssociatedListView.Handle, (int)WM_SCROLL_WHEEL, (uint)message.WParam.ToInt64(), (uint)message.LParam.ToInt64());
                //    }
                //    break;
                //case WM_VSCROLL:                  
                //    if (AssociatedListView != null)
                //    {
                //        int myInt = message.WParam.ToInt32();
                //        int IntLow = myInt & 0xffff;
                //        long IntHigh = ((long)myInt & 0xffff0000) >> 16;
                //        SendMessage(this.AssociatedListView.Handle, (int)WM_VSCROLL, (uint)message.WParam.ToInt64(), (uint)message.LParam.ToInt64());
                //    }                
                //    break;     
            }

            // pass messages on to the base control for processing
            base.WndProc(ref message);
        }   
        
    #region Methods

        /// <summary>
        /// Set the number of controller columns to display
        /// 
        /// TODO::Now that the TASMovieInputCollection contains the controller count,
        /// is this necessary anymore?
        /// </summary>
        /// <param name="columns"></param>
        public void SetColumns(int columns)
        {
            this.Columns.Clear();
            this.Columns.Add("Frame");

            if (columns == 0) return;

            for (int i = 0; i < columns; i++)
                this.Columns.Add("Controller " + (i + 1), 75);
        }
        
        /// <summary>
        /// Get the index of the specified item
        /// </summary>        
        private void GetVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            // If we have the item cached, return it. Otherwise, recreate it.
            if (cache != null && e.ItemIndex >= firstItem && e.ItemIndex < firstItem + cache.Length)
                e.Item = cache[e.ItemIndex - firstItem];
            else
                e.Item = GetListItem(e.ItemIndex);
        }

        /// <summary>
        /// Get the item at a specified index
        /// </summary>        
        private ListViewItem GetListItem(int listIndex)
        {
            ListViewItem lv;
            if(VirtualMovieType == TASForm.MovieType.VBM || 
               VirtualMovieType == TASForm.MovieType.SMV)
                    lv = new ListViewItem((listIndex).ToString());
            else
                    lv = new ListViewItem((listIndex + 1).ToString());

            // get each controller used and its input for the frame            
            for (int i = 0; i < this.Columns.Count - 1; i++)
            {
                // DEBUG::handles movie input with null entries in the
                // the main array
                //if (VirtualListSource[listIndex] != null)
                    lv.SubItems.Add(VirtualListSource[listIndex].Controller[i]);
                    
                //else
                //    lv.SubItems.Add("");
            }
               
            if (listIndex % 2 == 0) lv.BackColor = System.Drawing.Color.BlanchedAlmond;

            return lv;
        }

        /// <summary>
        /// Clear the virtual cache (duh :P)
        /// </summary>
        public void ClearVirtualCache()
        {
            cache = null;
        }

        /// <summary>
        /// Cache current view
        /// </summary>        
        private void CacheVirtualItemsList(object sender, CacheVirtualItemsEventArgs e)
        {
            // Only recreate the cache if we need to.
            if (cache != null && e.StartIndex >= firstItem && e.EndIndex <= firstItem + cache.Length)
                return;

            firstItem  = e.StartIndex;
            int length = e.EndIndex - e.StartIndex + 1;
            cache = new ListViewItem[length];

            for (int i = 0; i < cache.Length; i++) 
                cache[i] = GetListItem(firstItem + i);
        }

    #endregion

    }
}
