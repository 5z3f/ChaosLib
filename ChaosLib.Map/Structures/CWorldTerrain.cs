using System.Drawing;
using System.Numerics;

namespace ChaosLib.Map.Structures
{
    public enum TerrainLayerType : int
    {
        LT_NORMAL,
        LT_TILE,
    };

    public class CTerrainHeader
    {
        public string FileTypeMagic;
        public int TerrainCount;
        public int FileVersion;
    }

    public class CWorldTerrain
    {
        public CTerrainHeader Header;
        public CTerrainShadow[] Shadow;
        public CTerrainLayer[] Layer;

        public string
            GlobalDataMagic,
            EOTAorVersionMagic,
            HeightMapMagic,
            EdgeMapMagic,
            ShadowMapMagic,
            TopMapMagic,
            AttributeMapMagic,
            DataEndMagic;

        public int HeightMapWidth, HeightMapHeight;
        public int TopMapWidth, TopMapHeight;

        public int ShadowMapSizeAspect;
        public int ShadingMapSizeAspect;

        public int ShadowMapWidth => (ShadowMapSizeAspect < 0)
            ? (HeightMapWidth - 1) >> -ShadowMapSizeAspect
            : (HeightMapWidth - 1) << ShadowMapSizeAspect;

        public int ShadowMapHeight => (ShadowMapSizeAspect < 0)
            ? HeightMapHeight >> -ShadowMapSizeAspect
            : HeightMapHeight << ShadowMapSizeAspect;

        public int EdgeMapWidth => HeightMapWidth - 1;
        public int EdgeMapHeight => HeightMapHeight - 1;
        public int EdgeMapSize => EdgeMapWidth * EdgeMapHeight;

        public int HeightMapSize => HeightMapWidth * HeightMapHeight;
        public int ShadowMapSize => ShadowMapWidth * ShadowMapHeight;
        public int TopMapSize => TopMapWidth * TopMapHeight;

        public int AttributeMapSizeAspect;

        public int FirstTopMapLOD;
        public int LayerCount;
        public int ShadowMapCount;

        public float DistFactor, ShadowOverbright;

        public Vector3 Stretch, MetricSize;

        public int AttributeMapWidth => (int)MetricSize.X;
        public int AttributeMapHeight => (int)MetricSize.Z;


        public ushort[,] HeightMap;
        public byte[] EdgeMap;


        public string EdgeMapFileName;
        public string ShadowMapFileName;
        public string TopMapFileName;

        public dynamic AttributeMap;
        public dynamic AttributeBitmap;
    }

    public class CTerrainShadow
    {
        public uint ShadowTimes;
        public float BlurRadius;
        public byte[] ObjectColor;

        public string ShadowMapFileName;
    }

    public class CTerrainLayer
    {
        public string LayerMagic, 
            LayerTextureMagic,
            LayerVersionMagic,
            LayerDataMagic,
            LayerMaskMagic;

        public int LayerVersion;

        public string LayerFileName;
        public string LayerName;

        public int IsLayerVisible;
        public int LayerType; // TerrainLayerType enum
        public int Multiply;
        public int Flags;

        public int MaskStretch;
        public int SoundIndex;

        public int LayerMaskWidth;
        public int LayerMaskHeight;
        public int LayerMaskSize;

        public float OffsetX, OffsetY;
        public float RotateX, RotateY;
        public float StretchX, StretchY;

        public int IsAutoRegenerated;

        public float Coverage, CoverageNoise, CoverageRandom;

        public int ApplyMinAltitude, ApplyMaxAltitude;

        public float MinAltitude, MaxAltitude;
        public float MinAltitudeFade, MaxAltitudeFade;
        public float MinAltitudeNoise, MaxAltitudeNoise;
        public float MinAltitudeRandom, MaxAltitudeRandom;

        public int ApplyMinSlope, ApplyMaxSlope;

        public float MinSlope, MaxSlope;
        public float MinSlopeFade, MaxSlopeFade;
        public float MinSlopeNoise, MaxSlopeNoise;
        public float MinSlopeRandom, MaxSlopeRandom;

        public byte[] LayerMask;
        public string LayerAlphaMaskTextureName;

    }
}
