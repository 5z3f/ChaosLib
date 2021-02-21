using BinarySerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosLib.Map.Classes
{
    class CExport
    {

        public dynamic BinaryFile(dynamic dataObject, string fp)
        {
            FileStream fs = File.Create(fp);
            BinarySerializer bs = new BinarySerializer();
            bs.Serialize(fs, dataObject);
            fs.Close();

            return true;
        }

    }
}
