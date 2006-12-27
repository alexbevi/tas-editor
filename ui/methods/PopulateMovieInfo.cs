using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using MovieSplicer.Data;
using MovieSplicer.Data.Formats;

namespace MovieSplicer.UI.Methods
{
    /// <summary>
    /// Populate a TreeView control with parsed data from a movie object
    /// </summary>
    class PopulateMovieInfo
    {       
        /// <summary>
        /// Populate an SNES9x movie file's header information
        /// </summary>        
        public static void SMV(ref TreeView tv, ref SNES9x movie)
        {
            tv.Nodes.Add("Header");
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.SMVHeader.Signature);
            tv.Nodes[0].Nodes.Add("Version:        " + movie.SMVHeader.Version.ToString());
            tv.Nodes[0].Nodes.Add("UID:            " + movie.SMVHeader.UID);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.SMVHeader.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.SMVHeader.RerecordCount));
           
            tv.Nodes.Add("Options");
            tv.Nodes[1].Nodes.Add("Movie Start:  " + movie.SMVOptions.MovieStart);
            tv.Nodes[1].Nodes.Add("Movie Timing: " + movie.SMVOptions.MovieTiming);

            tv.Nodes[1].Nodes.Add("Sync Options");
            tv.Nodes[1].Nodes[2].Nodes.Add("FAKEMUTE:   " + movie.SMVSpecific.FAKEMUTE.ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("LEFTRIGHT:  " + movie.SMVSpecific.LEFTRIGHT.ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("SYNCSOUND:  " + movie.SMVSpecific.SYNCSOUND.ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("VOLUMEENVX: " + movie.SMVSpecific.VOLUMEENVX.ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("WIP1TIMING: " + movie.SMVSpecific.WIP1TIMING.ToString());

            tv.Nodes.Add("Metadata");
            tv.Nodes[2].Nodes.Add("Author: " + movie.SMVExtra.Author);

            tv.Nodes.Add("ROM Information");
            tv.Nodes[3].Nodes.Add("ROM Title: " + movie.SMVExtra.ROM);
            tv.Nodes[3].Nodes.Add("ROM CRC:   " + movie.SMVExtra.CRC);

            tv.Nodes.Add("Controllers");
            tv.Nodes[4].Nodes.Add("Controller 1 Present: " + movie.SMVInput.Controllers[0].ToString());
            tv.Nodes[4].Nodes.Add("Controller 2 Present: " + movie.SMVInput.Controllers[1].ToString());
            tv.Nodes[4].Nodes.Add("Controller 3 Present: " + movie.SMVInput.Controllers[2].ToString());
            tv.Nodes[4].Nodes.Add("Controller 4 Present: " + movie.SMVInput.Controllers[3].ToString());
            tv.Nodes[4].Nodes.Add("Controller 5 Present: " + movie.SMVInput.Controllers[4].ToString());

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }        

        /// <summary>
        /// Populate an FCE Ultra movie file's header information
        /// </summary>
        public static void FCM(ref TreeView tv, ref FCEU movie)
        {            
            tv.Nodes.Add("Header");
            tv.Nodes[0].Nodes.Add("Signature:        " + movie.FCMHeader.Signature);
            tv.Nodes[0].Nodes.Add("Version:          " + movie.FCMHeader.Version.ToString());
            tv.Nodes[0].Nodes.Add("Frame Count:      " + String.Format("{0:0,0}", movie.FCMHeader.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count:   " + String.Format("{0:0,0}", movie.FCMHeader.RerecordCount));
            tv.Nodes[0].Nodes.Add("Emulator Version: " + movie.FCMHeader.EmulatorID);
            tv.Nodes[0].Nodes.Add("Movie Start:      " + movie.FCMOptions.MovieStart);
            tv.Nodes[0].Nodes.Add("Movie Timing:     " + movie.FCMOptions.MovieTiming);

            tv.Nodes.Add("Metadata");
            tv.Nodes[1].Nodes.Add("Author: " + movie.FCMExtra.Author);

            tv.Nodes.Add("ROM Information");
            tv.Nodes[2].Nodes.Add("ROM Title: " + movie.FCMExtra.ROM);
            tv.Nodes[2].Nodes.Add("ROM CRC:   " + movie.FCMExtra.CRC);

            tv.Nodes.Add("Controllers");
            tv.Nodes[3].Nodes.Add("Controller 1: " + movie.FCMInput.Controllers[0].ToString());
            tv.Nodes[3].Nodes.Add("Controller 2: " + movie.FCMInput.Controllers[1].ToString());
            tv.Nodes[3].Nodes.Add("Controller 3: " + movie.FCMInput.Controllers[2].ToString());
            tv.Nodes[3].Nodes.Add("Controller 4: " + movie.FCMInput.Controllers[3].ToString());

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }
       
        /// <summary>
        /// Populate a VisualBoyAdvance movie file's header information
        /// </summary>
        public static void VBM(ref TreeView tv, ref VisualBoyAdvance movie)
        {
            tv.Nodes.Add("Header");
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.VBMHeader.Signature);
            tv.Nodes[0].Nodes.Add("Version:        " + movie.VBMHeader.Version.ToString());
            tv.Nodes[0].Nodes.Add("UID:            " + movie.VBMHeader.UID);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.VBMHeader.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.VBMHeader.RerecordCount));

            tv.Nodes.Add("Options");
            tv.Nodes[1].Nodes.Add("Movie Start: " + movie.VBMOptions.MovieStart);
            tv.Nodes[1].Nodes.Add("System Type");
            tv.Nodes[1].Nodes[1].Nodes.Add("Game Boy Advance: " + movie.VBMSpecific.SystemType[0].ToString());
            tv.Nodes[1].Nodes[1].Nodes.Add("Game Boy Colour:  " + movie.VBMSpecific.SystemType[1].ToString());
            tv.Nodes[1].Nodes[1].Nodes.Add("Super Game Boy:   " + movie.VBMSpecific.SystemType[2].ToString());
            tv.Nodes[1].Nodes[1].Nodes.Add("Game Boy:         " + movie.VBMSpecific.SystemType[3].ToString());
            tv.Nodes[1].Nodes.Add("BIOS Flags");
            tv.Nodes[1].Nodes[2].Nodes.Add("useBiosFile:  " + movie.VBMSpecific.BIOSFlags[0].ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("skipBiosFile: " + movie.VBMSpecific.BIOSFlags[1].ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("rtcEnable:    " + movie.VBMSpecific.BIOSFlags[2].ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("lagReduction: " + movie.VBMSpecific.BIOSFlags[4].ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("gbcHdma5Fix:  " + movie.VBMSpecific.BIOSFlags[5].ToString());

            tv.Nodes.Add("Metadata");
            tv.Nodes[2].Nodes.Add("Author:      " + movie.VBMExtra.Author);
            tv.Nodes[2].Nodes.Add("Description: " + movie.VBMExtra.Description);

            tv.Nodes.Add("ROM Information");
            tv.Nodes[3].Nodes.Add("ROM Name: " + movie.VBMExtra.ROM);
            tv.Nodes[3].Nodes.Add("ROM CRC:  " + movie.VBMExtra.CRC);
            //tv.Nodes[3].Nodes.Add("Check:    " + movie.RomInfo.Checksum.ToString());

            tv.Nodes.Add("Controllers");
            tv.Nodes[4].Nodes.Add("Controller 1 Present: " + movie.VBMInput.Controllers[0].ToString());
            tv.Nodes[4].Nodes.Add("Controller 2 Present: " + movie.VBMInput.Controllers[1].ToString());
            tv.Nodes[4].Nodes.Add("Controller 3 Present: " + movie.VBMInput.Controllers[2].ToString());
            tv.Nodes[4].Nodes.Add("Controller 4 Present: " + movie.VBMInput.Controllers[3].ToString());

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }       

        /// <summary>
        /// Populate a Famtasia movie file's header information
        /// </summary>
        public static void FMV(ref TreeView tv, ref Famtasia movie)
        {            
            tv.Nodes.Add("Header");
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.FMVHeader.Signature);
            tv.Nodes[0].Nodes.Add("Movie Title:    " + movie.FMVExtra.Description);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.FMVHeader.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.FMVHeader.RerecordCount));
            tv.Nodes[0].Nodes.Add("Emulator ID:    " + movie.FMVHeader.EmulatorID);
            tv.Nodes[0].Nodes.Add("Movie Start:    " + movie.FMVOptions.MovieStart);

            tv.Nodes.Add("Controllers");
            tv.Nodes[1].Nodes.Add("Controller 1: " + movie.FMVInput.Controllers[0].ToString());
            tv.Nodes[1].Nodes.Add("Controller 2: " + movie.FMVInput.Controllers[1].ToString());

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();            
        }        

        /// <summary>
        /// Populate a Gens movie file's header information
        /// </summary>
        public static void GMV(ref TreeView tv, ref Gens movie)
        {
            tv.Nodes.Add("Header");
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.GMVHeader.Signature);
            tv.Nodes[0].Nodes.Add("Version:        " + movie.GMVHeader.Version.ToString());
            tv.Nodes[0].Nodes.Add("Movie Name:     " + movie.GMVExtra.Description);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.GMVHeader.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.GMVHeader.RerecordCount));

            if (movie.GMVHeader.Version > 0x09)
            {
                tv.Nodes[0].Nodes.Add("FPS:            " + movie.GMVOptions.FPS);                
                tv.Nodes[0].Nodes.Add("Movie Start:    " + movie.GMVOptions.MovieStart);
            }

            tv.Nodes.Add("Controllers");
            tv.Nodes[1].Nodes.Add("Controller 1: true");
            tv.Nodes[1].Nodes[0].Nodes.Add("Config: " + movie.GMVSpecific.Player1Config + " button");
            tv.Nodes[1].Nodes.Add("Controller 2: true");
            tv.Nodes[1].Nodes[1].Nodes.Add("Config: " + movie.GMVSpecific.Player2Config + " button");
            tv.Nodes[1].Nodes.Add("Controller 3: false");
            
            if (movie.GMVHeader.Version > 0x09)
                if (movie.GMVInput.ControllerCount == 3)
                    tv.Nodes[1].Nodes[2].Text = "Controller 3: true";

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }

        /// <summary>
        /// Populate a Mupen64 movie file's header information
        /// </summary>
        public static void M64(ref TreeView tv, ref Mupen64 movie)
        {
            tv.Nodes.Add("Header");
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.M64Header.Signature);
            tv.Nodes[0].Nodes.Add("Version:        " + movie.M64Header.Version.ToString());
            tv.Nodes[0].Nodes.Add("UID:            " + movie.M64Header.UID);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.M64Header.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.M64Header.RerecordCount));

            tv.Nodes.Add("Options");
            tv.Nodes[1].Nodes.Add("FPS:         " + movie.M64Options.FPS);            
            tv.Nodes[1].Nodes.Add("Movie Start: " + movie.M64Options.MovieStart);

            tv.Nodes.Add("ROM Information");
            tv.Nodes[2].Nodes.Add("Name:    " + movie.M64Extra.ROM);
            tv.Nodes[2].Nodes.Add("CRC:     " + movie.M64Extra.CRC);
            tv.Nodes[2].Nodes.Add("Country: " + movie.M64Extra.Country);
            
            tv.Nodes.Add("Extra Information");
            tv.Nodes[3].Nodes.Add("Author:       " + movie.M64Extra.Author);
            tv.Nodes[3].Nodes.Add("Description:  " + movie.M64Extra.Description);
            tv.Nodes[3].Nodes.Add("Video Plugin: " + movie.M64Specific.VideoPlugin);
            tv.Nodes[3].Nodes.Add("Audio Plugin: " + movie.M64Specific.AudioPlugin);
            tv.Nodes[3].Nodes.Add("Input Plugin: " + movie.M64Specific.InputPlugin);
            tv.Nodes[3].Nodes.Add("RSP Plugin:   " + movie.M64Specific.RSPPlugin);

            tv.Nodes.Add("Controller Information");            
            for (int i = 0; i < 4; i++)
            {
                tv.Nodes[4].Nodes.Add("Controller " + (i + 1));
                tv.Nodes[4].Nodes[i].Nodes.Add("Controller Present: " + movie.M64Specific.Controller[i].Option[0]);
                tv.Nodes[4].Nodes[i].Nodes.Add("Mempak Present:     " + movie.M64Specific.Controller[i].Option[1]);
                tv.Nodes[4].Nodes[i].Nodes.Add("Rumblepak Present:  " + movie.M64Specific.Controller[i].Option[2]);
            }

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }

    }
}
