using ChaosLib.Map.Classes;
using ChaosLib.Map.Structures;
using System.ComponentModel;
using System.Threading.Tasks;
using static ChaosLib.Map.Classes.CMySQL;

namespace ChaosLib.Map
{
    // ------------------------------------------------------------
    // ChaosLib - Last Chaos Library (Alpha, under construction)
    // Written in .NET 5 | C# 9.0
    // ------------------------------------------------------------
    // ChaosLib.D3D        3D Utilites
    // ChaosLib.MAP        Game Data Mapper
    // ------------------------------------------------------------

    /// <summary>
    /// Allowed data types
    /// </summary>
    public enum ContentDataType
    {
        /// <summary>Original 16xx client binary file</summary>
        Binary,

        /// <summary>Database table</summary>
        Database,

        /// <summary>JSON file</summary>
        JSON,

        /// <summary>BSON file (under construction)</summary>
        BSON,

        /// <summary>ZeroFormatter file (under construction)</summary>
        ZeroFormatter,

        /// <summary>Messagepack file (under construction)</summary>
        MSGPACK
    }

    /// <summary>
    /// Allowed types of files that can be readed
    /// </summary>
    public enum ContentType
    {
        /// <summary></summary>
        NPC,

        /// <summary></summary>
        Action,

        /// <summary></summary>
        Title,

        /// <summary></summary>
        MonsterCombo,

        /// <summary></summary>
        LevelGuide,

        /// <summary></summary>
        ItemExchange,

        /// <summary></summary>
        Help,

        /// <summary></summary>     // Not synced with database
        Map,

        /// <summary></summary>
        ArmorPreview,

        /// <summary></summary>
        WorldTerrain,

        /// <summary></summary>
        ServerAttributeMap,

        /// <summary></summary>
        ServerHeightMap
    }

    public enum Connection
    {
        /// <summary>Test Connection</summary>
        Test,

        /// <summary>Set Connection</summary>
        Set
    }

    public enum Command
    {
        /// <summary>Insert new record</summary>
        Insert,

        /// <summary>Update record</summary>
        Update,

        /// <summary>Delete record</summary>
        Delete
    }

    public enum Language
    {
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAFnSURBVEhLxZOxSsRAEIbzAIIINhYhtZDCwhMUFLGxTJE0iUVqixRCEMEileALBEkjCGkttdAiYGGjryAHvkD0GgsL/XDCuiQbJUjwZziyu/98Nzu7a42rjxH0H+i6rqfTaTP4UdgwNwNNvegsy5IkKYrCmCZiCQM2zM2UJjOaQmzbdhyHX1X7+8vs9e6B4ENmlC0Igu4WzWjP83zfD8NQlfN8cnY/t6KCocyXZRnHMWZSZEbJjM7z3HVdypFutLgXi1uE0DFgw0zKV+q3zGjEBquq4oPt69y9tX1r9YjY2TiQzmDrdgP1opVoro4WrsTV+XVjMul3NPk38xPhni7t6ujq8akxmdSLVg2pZ2+TzcPLhXX6u7x9rKNZwjCsIa1jzIpbnSjBJEuDj1Eun/4WWnThIi5fmqYDLh8bpBakPxm2T3MJ6QNiSZ5MFEXdnpjRaKyHjkgzHk5X2Ix/34v+u8ZHjyLL+gS/5/KdRE5BHQAAAABJRU5ErkJggg==")]
        KOR,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAC0SURBVEhLtZKxDcJAEAS/BFfgWlyCK7BciytwQkZgModERFABicmQKIEegBVancwGh4O70QR3+7/Sy3LJZrfRVykbNX76fX9p2xPEIEfSdzTYrKp90xyH4dp1Z4gBK0K7IH1Hg004jre6PizLE2LAuj6VvqPBJt44z49pur+/YMCK0C5I39FgE2a9Ovdbw5Q/5K/SdySShkgkDZFIGiKRNEQiaYhE0hCJpCESSUMkkoaYSSkf7VftMkEKoK4AAAAASUVORK5CYII=")]
        TWN,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAADBSURBVEhLY6AtuKcogIkeWfI9C+BBEyQe4TP693Gmr4tZ0QSJR+hGv8rgeuLKC2EDnQxkv4jhBjofroB4hG400Jkfe9jhXKDRQLe/b+CAixCPEEYD3fv3MuP/ewxABGTAQ5ns4EYYDURvSjghRsONe6DLD3QyPIhIQihGA0MDaBAwQOBhAjQa6APkICIeoRgNNAjCgMQb0ESgTUA2daIRGaFFKakIn9FABPcHGYiA0ZSgUaPR0KjRaIj2RtMEMDAAADiwJqrY0852AAAAAElFTkSuQmCC")]
        CHN,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAA7SURBVEhLY6AteCujQnVEe6P/0wAMaaNVZP2pjkaNRkOjRqMh2hsNTeVUBUPaaLTykCqI9kbTBDAwAABnv/fXqj4fOAAAAABJRU5ErkJggg==")]
        THAI,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAAAzklEQVRIS82TSQrDMAxFc//zNWQeNs5AQsBkXJi2n0bNzpEFNfQtjL8MbyFLgV8eHvgPdRRFRVFkWUaZw1U9TdPry77vaZrSgx1Xdd/367qeJ+zLstCDHVd1GIbnJUkSqI0xZ7zBVX3RNA3U27ZRtiNTV1X1/FCWJZXsCNRxHKMP8NZ1TaVbBOqu69AK/CRlDoF6GAaoMdqUOQRqzLJSCm2hzCFQQ9q27e+3EWit0ZDjOChzCNTjOEI9zzNlDoEa5Hl+rSWLTC3Cv9oLQfAGTiryncWb574AAAAASUVORK5CYII=")]
        THAI_ENG,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAACVSURBVEhLY6At+E8DMIyN/n7/6dX46gP8lnsYdIEIyABygYJQaRyAsNGfzl+HG4qMgIJAKagibICA0UCnYTUXgoBSeNxOwGigx9GMQ0NABVClGICA0XicDEFABVClGICA0WgGYUVQpRhg4IymYYDQMBppmPiAgFZZBgKATgN6HG4BkAHk4nEvBBBlNHlgSBtNE8DAAAB+jO1x/uno6QAAAABJRU5ErkJggg==")]
        JPN,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAETSURBVEhLtZK/DgFBEIdXoyLxp6I6jU51le40CqFQUqGQKL3BdSovoFKpJR7ABSUJpWgkXkCj5pfsZOPGskfW5MtmM9n7ZnbnxH8jJvpfcRQZIz+qT8HKCFcX8q352DkvEn7fbXpVmam4dazqDNh1Bka4GtKpX5T7dLIDOyoFkxxWWUly8RpGQmp0d98KGJUCvSMjwV7l7xFCo1bfg3dqNjEtITX6vQbxYbusFN2Gt59l8UpYsVd5NjEtITXA97DjuVFA1sBVkMSqzgA2MS1cDdA7RM8v/gqbmBaNOgo0qY9B6l6q9hVrp2SE1Ad/ZB1S35Yb65Ca/ZJWIDW9vNUgNZuAFUjNJmAFUtMdrAap/xJCPACet+hWvm1oyAAAAABJRU5ErkJggg==")]
        MAL,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAAAzklEQVRIS82TSQrDMAxFc//zNWQeNs5AQsBkXJi2n0bNzpEFNfQtjL8MbyFLgV8eHvgPdRRFRVFkWUaZw1U9TdPry77vaZrSgx1Xdd/367qeJ+zLstCDHVd1GIbnJUkSqI0xZ7zBVX3RNA3U27ZRtiNTV1X1/FCWJZXsCNRxHKMP8NZ1TaVbBOqu69AK/CRlDoF6GAaoMdqUOQRqzLJSCm2hzCFQQ9q27e+3EWit0ZDjOChzCNTjOEI9zzNlDoEa5Hl+rSWLTC3Cv9oLQfAGTiryncWb574AAAAASUVORK5CYII=")]
        MAL_ENG,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAFaSURBVEhLtZO7SgNREIb3WXwGG9E+dhZJYyFoJXgDwRiwcDdFCsGAxkYLJV6LLSwsImIjeEEhkM5mWyGNPoN+OD8rnEKmOBl+DnNmZ7+dOTsnGa1NTa6trnRYF+Z3WGu1jLV0LGgJN2PjTgk9XWkMBkW1mvV6b8tLe5396836EcJhS5BHJJD2/vTilNBWo70MiAJPjm8RDlv7sOV8vvadEtpeNi4rxQJFOGXQEoKu/5HQlAMozx/qG4cX5/dp2m3v5giHLUEekUDat9uEppxW67KZndoKMd3+Vdotg6yk3a1vOSV0edCcLzVCsd+Iw5agHQhpz7OLTv2hmbOi+LAhaTbP7DfisCXIIxtEdeswoSlqOPyy0phimw2Ew9YasoSriYpTQs/NNJx6bB84JbR6iGpCByMZRUIHdzSKhA7uaBQJHfQSRULr5KOa0MEdjSKhg16iSOiRWJL8APz5kyh3zy3LAAAAAElFTkSuQmCC")]
        USA,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB0AAAAUCAIAAAD+/qGQAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAABt0lEQVQ4T2OgJZhkT2UEBWii2BDPFJuUZWpAEk0cO4ICNFEM5LNQ584+9h+HGYAkkI0miwVBAZooEpKcbrliswjQRGQEFAGKo6lEQVCAJgpGTJPtM5arvjrA/H4Pa3emVbBVhLF8JpAEsoEiQHGgLFANmi4oggI00Un2urNNDu/gAzrt5kpBJ+1EDbF8ZAQUAYoDZYFqgCrR9IIQFCAJcU6xbVkj++UQI1Db14OMoVbhtvq12pKFaEYDxYGyQDVAlUD1QF3IhkCNhfNd5+te38MBVA1Bp1d4LN62f9KiLXM271y749iEWZsyS6YHOndDjD4wRQ6uEqgLqBduDtRYIEt0mtWCTWJwdRA0o9jMXr8OYoqmeIGbeXNJzqJlmw+WtSzQki6YXmSCph5oAtAcwuYCdQJN1JIoMFWtADLM1SohdpTnL1674+isOmc09VjMhSC0cAD61NW0yVSlHGiWjnQR0AKI0cDgDnbtObPCHa4SZzjAEWa8Ac211qoGGmetXWMgXwpEtrq1CW5JJMQbHOFKZ9pShQZyJQHWuSSnMziiSb6AI+rnY2RE/XIHjmhSTpKMaAUYGADL6vMzNU1t6gAAAABJRU5ErkJggg==")]
        BRZ,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAFeSURBVEhLY6AtuKcpQHU0mIx+YCb/JNAWTRArIs3oj4um/////1VVFpo4VkSC0W/aK7/u2fL95OH3UzrQpLAiAkY/i/eBs7+fOvJp9cLfTx5+Xr8MLogHETAa6MDfTx8BDX2REw008d2UdiD7ib81kA1kAElg0KNpgSPCRn9et/RNW8XPG5eBsfe2t+FFZvjXAzs/Lpz2uiobGD5AcTQtcETA6Ecuej9vXgUaBDT906oFz5MCXhYmvJ/VB3Ty9zPHgYHz99NHXAmGgNFABNQJ8f6H2f0vMsKeRXu8ba8EWommDBMRNhqIgAYB4xMS1q8bi0Dhc/MqwdRNlNFAJ/+8ful1QyGQBFrz99OHd70NX/duRVOGhggbDUwbwGzyLM77+5ljwCwDdD4wiJ8E2ADtQ1OJhggbDcx7oEg7dQSeU4AMoGXA8IGrwYoIG40V4U/REESm0cSgIW00TQADAwCZ/oOmmT3jfgAAAABJRU5ErkJggg==")]
        HK,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAAAzklEQVRIS82TSQrDMAxFc//zNWQeNs5AQsBkXJi2n0bNzpEFNfQtjL8MbyFLgV8eHvgPdRRFRVFkWUaZw1U9TdPry77vaZrSgx1Xdd/367qeJ+zLstCDHVd1GIbnJUkSqI0xZ7zBVX3RNA3U27ZRtiNTV1X1/FCWJZXsCNRxHKMP8NZ1TaVbBOqu69AK/CRlDoF6GAaoMdqUOQRqzLJSCm2hzCFQQ9q27e+3EWit0ZDjOChzCNTjOEI9zzNlDoEa5Hl+rSWLTC3Cv9oLQfAGTiryncWb574AAAAASUVORK5CYII=")]
        HK_ENG,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAABASURBVEhL7cihEQAxEIDA67+4dJBa8gaFJvMmOyjmef60LwTdJOgmQTcJuknQTcJZfdBNgm4SdJOgmwTdpJtmPirJv6BBy3pMAAAAAElFTkSuQmCC")]
        GER,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAD1SURBVEhLY6AtuMYgQHU0ajQaghr9/zv1ERSgiSKjYwy8aCJEIihAE4Wi/wwTGUBG37sFYqPLEkJQgCYKQv8ZKmuYd29l2LOCeUYDK1Dk/BkMNXgRFKCJ/v/JUJzPAoyKJwwC////j4hhAQoCrUFRQwhBAZro+18Md2357jIIaNvmAY1mZQ7YxMA3PwTkduIRFKCJAhHQvUCzVohxMyivvQJU8JPh+YvPaGrwIyhAEwXG26EDfkCjL+1gBoZGXyozUJA6RqeUpQGDYlkwO9Do3Qw8CVasH54zggIHVRl+BAVoolRBUICcQamFRo1GQ1CjaQIYGADoZs7OVd5SkwAAAABJRU5ErkJggg==")]
        SPN,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAACcSURBVEhLtcyxDcJQAMTQv0sqRmDQjMkM0LiwLB1dLNfvPNx1r79/+7zea+RwHmMUziOH8xijcB45nMcYhfPI4TzGKJxHDucxRuE8cjiPMQrnkcN5jFE4jxzOY4zCeeRwHmMUziOH8xijcB45nMcYhfPI4TzGKJxHDucxRuE8cjiPMQrnkcN5jFE4jxzOY4zCeeRwHmMUziM/0jk/qrpigCJZl+gAAAAASUVORK5CYII=")]
        FRC,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAAsSURBVEhLY6At+E8DMGo0Ghg1Gg0MaaPviNhQHY0ajYZGjUZDQ9pomgAGBgBzMhiZglT4FAAAAABJRU5ErkJggg==")]
        PLD,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAABFSURBVEhL7cyxDQAgEEJRJnEBJ3BhN3IeLaSihuKSe/kVBci6AX0tSl9jbX8kqyWS1RLJaolktUSyWvrOHPb6Wip9HQE8SElEPh9sIjYAAAAASUVORK5CYII=")]
        RUS,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAADGSURBVEhLY6AteMwlTnU0ajQaImz0x9buvx8//gcDIAPIRVOACxEw+telKxBDfxw69iY8AYi+LlkJFERThhXhMxroQIi5QOOQxZ9KqgHRCwvnd2l5cC6yAgjCZzQkHIAkVp1AoyG2/nn4mDSjgarBLgYFBZoUED3XNAEaClHwecosNFkIItNoYFgBjYZ7C+gDNAVAhNNoIMIfIEAEtPV9WS3QXJKNxh+NQAYkGnEhfEYDEa0SHwTRKstQgkaNRkNQo2kCGBgAaZVmlqdQDSsAAAAASUVORK5CYII=")]
        TUR,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAAAzklEQVRIS82TSQrDMAxFc//zNWQeNs5AQsBkXJi2n0bNzpEFNfQtjL8MbyFLgV8eHvgPdRRFRVFkWUaZw1U9TdPry77vaZrSgx1Xdd/367qeJ+zLstCDHVd1GIbnJUkSqI0xZ7zBVX3RNA3U27ZRtiNTV1X1/FCWJZXsCNRxHKMP8NZ1TaVbBOqu69AK/CRlDoF6GAaoMdqUOQRqzLJSCm2hzCFQQ9q27e+3EWit0ZDjOChzCNTjOEI9zzNlDoEa5Hl+rSWLTC3Cv9oLQfAGTiryncWb574AAAAASUVORK5CYII=")]
        SPN2,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAAAzklEQVRIS82TSQrDMAxFc//zNWQeNs5AQsBkXJi2n0bNzpEFNfQtjL8MbyFLgV8eHvgPdRRFRVFkWUaZw1U9TdPry77vaZrSgx1Xdd/367qeJ+zLstCDHVd1GIbnJUkSqI0xZ7zBVX3RNA3U27ZRtiNTV1X1/FCWJZXsCNRxHKMP8NZ1TaVbBOqu69AK/CRlDoF6GAaoMdqUOQRqzLJSCm2hzCFQQ9q27e+3EWit0ZDjOChzCNTjOEI9zzNlDoEa5Hl+rSWLTC3Cv9oLQfAGTiryncWb574AAAAASUVORK5CYII=")]
        FRC2,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAACdSURBVEhLtcyxDcJQAMTQv08GYAMGzGCsBI0Ly9LRxXL9zsPd7/X3b5/rtUYO5zFG4TxyOI8xCueRw3mMUTiPHM5jjMJ55HAeYxTOI4fzGKNwHjmcxxiF88jhPMYonEcO5zFG4TxyOI8xCueRw3mMUTiPHM5jjMJ55HAeYxTOI4fzGKNwHjmcxxiF88jhPMYonEcO5zFG4TzyI53zA3raYyL0wJ5EAAAAAElFTkSuQmCC")]
        ITA,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAFCSURBVEhLtczBK8NxHMbx310OO+1EixoXDrtoOS2aAyl3NQdqB7fJScliay6Ki5bEpHBzknLDRW0Oq/0BuArHpfb1zfdRT099nLand7++fer3inq8/IyV+3eNWNIKsnAcDGPCcZCF42AYE46DLBwHw5hwHGThOBjGhOMgC8fBMCYcB1k4DoZzrwe568JEMZMoTw3i1C364f5sfXp0f3b4ubaymk2Fo3AcZOG4QDzVL99u0idr2fTIQLWwNJ9KhLtwHGThuED0bZW+DuP+MTmezIwN+cd7a89/heMgC8f5/6vHy53vz8XaY//GZmy7Ei/tOtfx9y7Qfken+XZ9rn21cLGTe2meh6OfcBxk4TgYv/u4vcPrb8JxkIXjYBgTjoMsHAfDmHAcZOE4GMaE4yALx8EwJhwHWTgOhjHhOMg9WRT9AHor/bCcLD7MAAAAAElFTkSuQmCC")]
        MEX,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABl0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC4xOdTWsmQAAAAwSURBVEhLY6AtWCejQXU0ajQaGtJG/6cBGDUaDQxpoxXduqmORo1GQ0PaaJoABgYAY2oz1nI02tcAAAAASUVORK5CYII=")]
        NLD,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAAAzklEQVRIS82TSQrDMAxFc//zNWQeNs5AQsBkXJi2n0bNzpEFNfQtjL8MbyFLgV8eHvgPdRRFRVFkWUaZw1U9TdPry77vaZrSgx1Xdd/367qeJ+zLstCDHVd1GIbnJUkSqI0xZ7zBVX3RNA3U27ZRtiNTV1X1/FCWJZXsCNRxHKMP8NZ1TaVbBOqu69AK/CRlDoF6GAaoMdqUOQRqzLJSCm2hzCFQQ9q27e+3EWit0ZDjOChzCNTjOEI9zzNlDoEa5Hl+rSWLTC3Cv9oLQfAGTiryncWb574AAAAASUVORK5CYII=")]
        UK,
        [Description("iVBORw0KGgoAAAANSUhEUgAAAB4AAAAUCAIAAAAVyRqTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMTnU1rJkAAAAzklEQVRIS82TSQrDMAxFc//zNWQeNs5AQsBkXJi2n0bNzpEFNfQtjL8MbyFLgV8eHvgPdRRFRVFkWUaZw1U9TdPry77vaZrSgx1Xdd/367qeJ+zLstCDHVd1GIbnJUkSqI0xZ7zBVX3RNA3U27ZRtiNTV1X1/FCWJZXsCNRxHKMP8NZ1TaVbBOqu69AK/CRlDoF6GAaoMdqUOQRqzLJSCm2hzCFQQ9q27e+3EWit0ZDjOChzCNTjOEI9zzNlDoEa5Hl+rSWLTC3Cv9oLQfAGTiryncWb574AAAAASUVORK5CYII=")]
        DEV
    }

    public class ChaosMap
    {
        readonly CImport cImport = new CImport();
        readonly CExport cExport = new CExport();

        public dynamic Import(ContentType ct, ContentDataType cdt, string fp = null, dynamic additional = null) => cdt switch
        {
            ContentDataType.Binary => cImport.Binary(ct, fp, additional),
            ContentDataType.Database => cImport.Database(ct),

            //ContentDataType.JSON               => Import(contentType, fileDir, cdt),
            //ContentDataType.BSON               => Import(contentType, fileDir, cdt),
            //ContentDataType.ZeroFormatter      => Import(contentType, fileDir, cdt),
            //ContentDataType.MSGPACK            => Import(contentType, fileDir, cdt),

            _ => null,
        };      

        public dynamic Export(dynamic dataObject, string fp = null, ContentDataType cdt = ContentDataType.Binary) => cdt switch
        {
            ContentDataType.Binary => cExport.BinaryFile(dataObject, fp),

            _ => null
        };

        // todo: make it async
        public (bool success, string msg) DatabaseAction(ContentType ct, dynamic dataObject, Command c) => ct switch
        {
            ContentType.Action => CMySQL.Action<Action>("t_action", dataObject, c),
            ContentType.NPC => CMySQL.Action<NPC>("t_npc", dataObject, c),
            ContentType.Title => CMySQL.Action<Title>("t_title", dataObject, c),
            ContentType.MonsterCombo => CMySQL.Action<MonsterCombo>("t_missioncase", dataObject, c),
            ContentType.ItemExchange => CMySQL.Action<ItemExchange>("t_item_exchange", dataObject, c),
            ContentType.Help => CMySQL.Action<Help>("t_help1", dataObject, c),

            _ => null
        };

        public (bool success, string msg) MySQL(string host, int port, string database, string username, string password, Connection ct) => ct switch
        {
            Connection.Set => SetConnection(host, port, database, username, password),
            Connection.Test => TestConnection(host, port, database, username, password),

            _ => (false, string.Empty)
        };

        public dynamic ExecuteQuery(string sql) 
            => CMySQL.ExecuteQuery(sql);
    }
}
