using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosLib.Map.Interfaces
{
    public interface IImport
    {
        object Binary(ContentType contentType, string filePath);
        //void JSON(FileType fileType, string fileDir);
        //void MySQL(FileType fileType);
    }
}
