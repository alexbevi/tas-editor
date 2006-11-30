using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MovieSplicer.Components
{
    public class TASListView: ListView
    {            

        protected override void WndProc(ref Message message)
        {
            const int WM_PAINT = 0xf;

            // if the control is in details view mode and columns
            // have been added, then intercept the WM_PAINT message
            // and reset the last column width to fill the list view
            switch (message.Msg)
            {
                case WM_PAINT:
                    if (this.View == View.Details && this.Columns.Count > 0)
                        this.Columns[this.Columns.Count - 1].Width = -2;
                    break;
            }

            // pass messages on to the base control for processing
            base.WndProc(ref message);
        }

        public TASListView()
        {
            this.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(GetVirtualItem);
        }
        public ArrayList VirtualListSource;
        
        //private ListViewItem[] m_cache;
        //private int m_firstItem;

        //private void ClearVirtualCache()
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
