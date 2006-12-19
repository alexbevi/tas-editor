using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MovieSplicer.Components
{
    /// <summary>
    /// This is essentially a subclass of System.Windows.Forms.ListView that provides
    /// an auto-expanding last column and virtualization.
    /// 
    /// TODO::There's an issue with item caching. When enabled, the caches (if multiple listviews
    /// are defined and accessed) collide (overwrite each other), so access goes beyond the range
    /// of the chache's source pointer (this definition sucks, but it sorta makes sense to me so :P)
    /// </summary>
    public class TASListView: ListView
    {
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int WM_PAINT   = 0xF;

        public ArrayList VirtualListSource;

        //private ListViewItem[] m_cache;
        //private int            m_firstItem;

        /// <summary>
        /// Horizontal scroll position has changed event
        /// </summary>
        public event ScrollEventHandler HorzScrollValueChanged;

        /// <summary>
        /// Vertical scroll position has changed event
        /// </summary>
        public event ScrollEventHandler VertScrollValueChanged;        

        // Based on SB_* constants
        private static ScrollEventType[] _events =
            new ScrollEventType[] {
									  ScrollEventType.SmallDecrement,
									  ScrollEventType.SmallIncrement,
									  ScrollEventType.LargeDecrement,
									  ScrollEventType.LargeIncrement,
									  ScrollEventType.ThumbPosition,
									  ScrollEventType.ThumbTrack,
									  ScrollEventType.First,
									  ScrollEventType.Last,
									  ScrollEventType.EndScroll
								  };
        /// <summary>
        /// Decode the type of scroll message
        /// </summary>
        /// <param name="wParam">Lower word of scroll notification</param>
        /// <returns></returns>
        private ScrollEventType GetEventType(uint wParam)
        {
            if (wParam < _events.Length)
                return _events[wParam];
            else
                return ScrollEventType.EndScroll;
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

                // Was this a horizontal scroll message?
                case WM_HSCROLL:                    
                    if (HorzScrollValueChanged != null)
                    {
                        uint wParam = (uint)message.WParam.ToInt32();
                        HorzScrollValueChanged(this,
                            new ScrollEventArgs(
                                GetEventType(wParam & 0xffff), (int)(wParam >> 16)));
                    }
                    break;
                    
                // or a vertical scroll message?
                case WM_VSCROLL:                    
                    if (VertScrollValueChanged != null)
                    {
                        uint wParam = (uint)message.WParam.ToInt32();
                        VertScrollValueChanged(this,
                            new ScrollEventArgs(
                            GetEventType(wParam & 0xffff), (int)(wParam >> 16)));
                    }
                    break;                    
            }

            // pass messages on to the base control for processing
            base.WndProc(ref message);
        }

        /// <summary>
        /// Create a new TASListView object with an event handler on RetriveVirtualItem
        /// </summary>
        public TASListView()
        {
            this.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(GetVirtualItem);
            //this.CacheVirtualItems +=new CacheVirtualItemsEventHandler(CacheVirtualItemsList);
        }        

        //public void ClearVirtualCache()
        //{
        //    m_cache = null;
        //}

        private void GetVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            // If we have the item cached, return it. Otherwise, recreate it.
            //if (m_cache != null && e.ItemIndex >= m_firstItem && e.ItemIndex < m_firstItem + m_cache.Length)
            //    e.Item = m_cache[e.ItemIndex - m_firstItem];
            //else
                e.Item = GetListItem(e.ItemIndex);
        }

        private ListViewItem GetListItem(int i)
        {
            string[] input = (string[])VirtualListSource[i];
            ListViewItem lv = new ListViewItem((i + 1).ToString());

            // get each controller used and its input for the frame
            // (based on the number of colums set)
            for (int j = 0; j < this.Columns.Count - 1; j++)
                lv.SubItems.Add(input[j]);
            lv.Tag = i + 1;

            if (i % 2 == 0) lv.BackColor = System.Drawing.Color.BlanchedAlmond;

            return lv;
        }

        //private void CacheVirtualItemsList(object sender, CacheVirtualItemsEventArgs e)
        //{
        //    // Only recreate the cache if we need to.
        //    if (m_cache != null && e.StartIndex >= m_firstItem && e.EndIndex <= m_firstItem + m_cache.Length)
        //        return;

        //    m_firstItem = e.StartIndex;
        //    int length = e.EndIndex - e.StartIndex + 1;
        //    m_cache = new ListViewItem[length];

        //    for (int i = 0; i < m_cache.Length; i++)
        //        m_cache[i] = GetListItem(m_firstItem + i);
        //}
       
    }
}
