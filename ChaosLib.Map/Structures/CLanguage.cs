using NPoco;

using BinaryIgnore = BinarySerialization.IgnoreAttribute;

namespace ChaosLib.Map.Structures
{
    // this class is unused for now
    public class ColumnLocalized
    {
        public const string Column = "a_name";

        //[BinaryIgnore]
        [Column(Column)]
        public string KOR { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_twn")]
        public string TWN { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_chn")]
        public string CHN { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_thai_eng")]
        public string THAIENG { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_thai")]
        public string THAI { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_jpn")]
        public string JPN { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_mal")]
        public string MAL { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_mal_eng")]
        public string MALENG { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_usa")]
        public string USA { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_brz")]
        public string BRZ { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_hk")]
        public string HK { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_hk_eng")]
        public string HKENG { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_ger")]
        public string GER { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_spn")]
        public string SPN { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_frc")]
        public string FRC { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_pld")]
        public string PLD { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_rus")]
        public string RUS { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_tur")]
        public string TUR { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_spn2")]
        public string SPN2 { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_frc2")]
        public string FRC2 { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_ita")]
        public string ITA { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_mex")]
        public string MEX { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_nld")]
        public string NLD { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_uk")]
        public string UK { get; set; }

        //[BinaryIgnore]
        [Column(Column + "_dev")]
        public string DEV { get; set; }
    }

}
