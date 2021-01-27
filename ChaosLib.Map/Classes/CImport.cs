using BinarySerialization;
using System.IO;
using ChaosLib.Map.Structures;

namespace ChaosLib.Map.Classes
{
    class CImport
    {
        public static object Binary(ContentType ct, string fp)
        {
            var fs = File.OpenRead(fp);
            var bs = new BinarySerializer();

            return ct switch
            {
                ContentType.Notice         =>  bs.Deserialize<CNotice>(fs),
                ContentType.Action         =>  bs.Deserialize<CAction>(fs),
                ContentType.Option         =>  bs.Deserialize<COption>(fs),
                ContentType.MonsterCombo   =>  bs.Deserialize<CMonsterCombo>(fs),

                _ => null,
            };
        }
    }
}
