using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using MovieSplicer.Data;

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
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.Header.Signature);
            tv.Nodes[0].Nodes.Add("Version:        " + movie.Header.Version.ToString());
            tv.Nodes[0].Nodes.Add("UID:            " + movie.Header.UID);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.Header.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.Header.ReRecordCount));
            
            string movieStart = (movie.Options.RESET) ? "From Reset" : "From Savestate";
            string movieTiming = (movie.Options.PAL) ? "PAL" : "NTSC";
            tv.Nodes.Add("Options");
            tv.Nodes[1].Nodes.Add("Movie Start:  " + movieStart);
            tv.Nodes[1].Nodes.Add("Movie Timing: " + movieTiming);
            tv.Nodes[1].Nodes.Add("Sync Options");
            tv.Nodes[1].Nodes[2].Nodes.Add("FAKEMUTE:   " + movie.Options.FAKEMUTE.ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("LEFTRIGHT:  " + movie.Options.LEFTRIGHT.ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("SYNCSOUND:  " + movie.Options.SYNCSOUND.ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("VOLUMEENVX: " + movie.Options.VOLUMEENVX.ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("WIP1TIMING: " + movie.Options.WIP1TIMING.ToString());
          
            tv.Nodes.Add("Metadata");
            tv.Nodes[2].Nodes.Add("Author: " + movie.Header.Metadata);

            tv.Nodes.Add("ROM Information");
            tv.Nodes[3].Nodes.Add("ROM Title: " + movie.ROMInfo.Name);
            tv.Nodes[3].Nodes.Add("ROM CRC:   " + movie.ROMInfo.CRC);

            tv.Nodes.Add("Controllers");
            tv.Nodes[4].Nodes.Add("Controller 1 Present: " + movie.ControllerData.Controller[0].ToString());
            tv.Nodes[4].Nodes.Add("Controller 2 Present: " + movie.ControllerData.Controller[1].ToString());
            tv.Nodes[4].Nodes.Add("Controller 3 Present: " + movie.ControllerData.Controller[2].ToString());
            tv.Nodes[4].Nodes.Add("Controller 4 Present: " + movie.ControllerData.Controller[3].ToString());

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }

        /// <summary>
        /// Populate an FCE Ultra movie file's header information
        /// </summary>
        public static void FCM(ref TreeView tv, ref FCEU movie)
        {
            string movieStart = (movie.Header.StartFromReset) ? "From Reset" : "From Save";
            string movieTiming = (movie.Header.NTSC) ? "NTSC" : "PAL";
            
            tv.Nodes.Add("Header");            
            tv.Nodes[0].Nodes.Add("Signature:        "+ movie.Header.Signature);
            tv.Nodes[0].Nodes.Add("Version:          "+ movie.Header.Version.ToString());
            tv.Nodes[0].Nodes.Add("Frame Count:      "+ String.Format("{0:0,0}", movie.Header.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count:   "+ String.Format("{0:0,0}", movie.Header.ReRecordCount));
            tv.Nodes[0].Nodes.Add("Emulator Version: "+ movie.Header.EmulatorVersion.ToString());
            tv.Nodes[0].Nodes.Add("Movie Start:      "+ movieStart);
            tv.Nodes[0].Nodes.Add("Movie Timing:     "+ movieTiming);
            
            tv.Nodes.Add("Metadata");
            tv.Nodes[1].Nodes.Add("Author: "+ movie.Header.Author);
            
            tv.Nodes.Add("ROM Information");
            tv.Nodes[2].Nodes.Add("ROM Title: "+ movie.Header.ROMName);
            tv.Nodes[2].Nodes.Add("ROM CRC:   "+ movie.Header.ROMCRC);
            
            tv.Nodes.Add("Controllers");
            tv.Nodes[3].Nodes.Add("Controller 1: " + movie.ControllerData.Controller[0].ToString());
            tv.Nodes[3].Nodes.Add("Controller 2: " + movie.ControllerData.Controller[1].ToString());
            tv.Nodes[3].Nodes.Add("Controller 3: " + movie.ControllerData.Controller[2].ToString());
            tv.Nodes[3].Nodes.Add("Controller 4: " + movie.ControllerData.Controller[3].ToString());

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }

        /// <summary>
        /// Populate a VisualBoyAdvance movie file's header information
        /// </summary>
        public static void VBM(ref TreeView tv, ref VisualBoyAdvance movie)
        {
            tv.Nodes.Add("Header");                       
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.Header.Signature);
            tv.Nodes[0].Nodes.Add("Version:        " + movie.Header.Version.ToString());
            tv.Nodes[0].Nodes.Add("UID:            " + movie.Header.UID);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.Header.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.Header.RerecordCount));

            tv.Nodes.Add("Options");
            tv.Nodes[1].Nodes.Add("Movie Start");
            tv.Nodes[1].Nodes[0].Nodes.Add("From Reset:    " + movie.Options.MovieStart[0].ToString());
            tv.Nodes[1].Nodes[0].Nodes.Add("From Save:     " + movie.Options.MovieStart[1].ToString());
            tv.Nodes[1].Nodes[0].Nodes.Add("From Power-On: " + movie.Options.MovieStart[2].ToString());
            tv.Nodes[1].Nodes.Add("System Type");
            tv.Nodes[1].Nodes[1].Nodes.Add("Game Boy Advance: " + movie.Options.SystemType[0].ToString());
            tv.Nodes[1].Nodes[1].Nodes.Add("Game Boy Colour:  " + movie.Options.SystemType[1].ToString());
            tv.Nodes[1].Nodes[1].Nodes.Add("Super Game Boy:   " + movie.Options.SystemType[2].ToString());
            tv.Nodes[1].Nodes[1].Nodes.Add("Game Boy:         " + movie.Options.SystemType[3].ToString());
            tv.Nodes[1].Nodes.Add("BIOS Flags");
            tv.Nodes[1].Nodes[2].Nodes.Add("useBiosFile:  " + movie.Options.BIOSFlags[0].ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("skipBiosFile: " + movie.Options.BIOSFlags[1].ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("rtcEnable:    " + movie.Options.BIOSFlags[2].ToString());            
            tv.Nodes[1].Nodes[2].Nodes.Add("lagReduction: " + movie.Options.BIOSFlags[4].ToString());
            tv.Nodes[1].Nodes[2].Nodes.Add("gbcHdma5Fix:  " + movie.Options.BIOSFlags[5].ToString());

            tv.Nodes.Add("Metadata");
            tv.Nodes[2].Nodes.Add("Author:      " + movie.Metadata.Author);
            tv.Nodes[2].Nodes.Add("Description: " + movie.Metadata.Description);

            tv.Nodes.Add("ROM Information");
            tv.Nodes[3].Nodes.Add("ROM Name: " + movie.RomInfo.Name);
            tv.Nodes[3].Nodes.Add("ROM CRC:  " + movie.RomInfo.CRC.ToString());
            tv.Nodes[3].Nodes.Add("Check:    " + movie.RomInfo.Checksum.ToString());
            
            tv.Nodes.Add("Controllers");
            tv.Nodes[4].Nodes.Add("Controller 1 Present: " + movie.Options.Controllers[0].ToString());
            tv.Nodes[4].Nodes.Add("Controller 2 Present: " + movie.Options.Controllers[1].ToString());
            tv.Nodes[4].Nodes.Add("Controller 3 Present: " + movie.Options.Controllers[2].ToString());
            tv.Nodes[4].Nodes.Add("Controller 4 Present: " + movie.Options.Controllers[3].ToString());

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }

        /// <summary>
        /// Populate a Famtasia movie file's header information
        /// </summary>
        public static void FMV(ref TreeView tv, ref Famtasia movie)
        {
            string movieStart = (movie.Header.StartFromReset) ? "From Reset" : "From Save";            

            tv.Nodes.Add("Header");
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.Header.Signature);            
            tv.Nodes[0].Nodes.Add("Movie Title:    " + movie.Header.MovieTitle);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.Header.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.Header.ReRecordCount));
            tv.Nodes[0].Nodes.Add("Emulator ID:    " + movie.Header.EmulatorID);
            tv.Nodes[0].Nodes.Add("Movie Start:    " + movieStart);

            tv.Nodes.Add("Controllers");
            tv.Nodes[1].Nodes.Add("Controller 1: " + movie.Header.Controllers[0].ToString());
            tv.Nodes[1].Nodes.Add("Controller 2: " + movie.Header.Controllers[1].ToString());

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }

        /// <summary>
        /// Populate a Gens movie file's header information
        /// </summary>
        public static void GMV(ref TreeView tv, ref Gens movie)
        {                        
            tv.Nodes.Add("Header");
            tv.Nodes[0].Nodes.Add("Signature:      " + movie.Header.Signature);
            tv.Nodes[0].Nodes.Add("Version:        " + movie.Header.Version);
            tv.Nodes[0].Nodes.Add("Movie Name:     " + movie.Header.MovieName);
            tv.Nodes[0].Nodes.Add("Frame Count:    " + String.Format("{0:0,0}", movie.Header.FrameCount));
            tv.Nodes[0].Nodes.Add("Rerecord Count: " + String.Format("{0:0,0}", movie.Header.RerecordCount));                        
            if (movie.Header.Version > 0x09)
            {
                tv.Nodes[0].Nodes.Add("FPS:            " + movie.Options.FPS);
                string movieStart = (movie.Options.StartFromReset) ? "From Reset" : "From Save";
                tv.Nodes[0].Nodes.Add("Movie Start:    " + movieStart);
            }
            
            tv.Nodes.Add("Controllers");
            tv.Nodes[1].Nodes.Add("Controller 1: true");
            tv.Nodes[1].Nodes[0].Nodes.Add("Config: " + movie.Header.Player1Config + " button");
            tv.Nodes[1].Nodes.Add("Controller 2: true");
            tv.Nodes[1].Nodes[1].Nodes.Add("Config: " + movie.Header.Player2Config + " button");
            tv.Nodes[1].Nodes.Add("Controller 3: false");
            if (movie.Header.Version > 0x09)
                if(movie.Options.ControllerCount == 3)
                    tv.Nodes[1].Nodes[2].Text = "Controller 3: true";

            tv.ExpandAll(); tv.Nodes[0].EnsureVisible();
        }

    }
}
