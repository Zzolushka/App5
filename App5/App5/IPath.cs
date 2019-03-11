using System;
using System.Collections.Generic;
using System.Text;

namespace App5
{
    public interface IPath
    {
        string GetDatabasePath(string filename);
        string GetSimleString();
    }
}
