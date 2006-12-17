using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSplicer.Data.Structures
{
    /// <summary>
    /// The supported movie formats list
    /// </summary>
    public enum MovieType
    {
        None = 0,
        SMV = 1,
        FCM = 2,
        GMV = 3,
        FMV = 4,
        VBM = 5,
        M64 = 6
    }

    /// <summary>
    /// Lists the different kinds of splicing options that can be
    /// applied to a movie
    /// </summary>
    public enum SpliceOptions
    {
        FromStart = 1,
        Range = 2,
        ToEnd = 3
    }

    /// <summary>
    /// Holds the input for a particular controller (string representation, not binary)
    /// </summary>
    public class ControllerData
    {
        public string Controller1;
        public string Controller2;
        public string Controller3;
        public string Controller4;
    }
}
