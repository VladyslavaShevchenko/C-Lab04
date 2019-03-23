using System;
using System.IO;
namespace Lab04
{
    internal static class StationManager
    {
        internal static readonly string WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lab04");
        internal static Person CurrentPerson { get; set; }
    }
}
