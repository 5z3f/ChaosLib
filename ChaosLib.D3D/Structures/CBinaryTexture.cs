using System.Drawing;

namespace ChaosLib.D3D.Structures
{
    public class CTextureHeader
    {
        public string FileTypeMagic;
        public int FileVersion;
        public long EncodedVersion;
    }

    public class CBinaryTexture
    {
        public CTextureHeader Header;
        public string DataTypeMagic,
            FrameTypeMagic,
            AnimationTypeMagic,
            AnimationDataTypeMagic;

        public TextureFormat TextureFormat;

        public uint MipMapCount,
            FirstMipLevel,
            Width,
            Height,
            Flags,
            FrameCount,
            MipLevelCount,
            FrameSize;

        public int CompressedFrameSize;

        public byte[][] PixelData;

        public const int TEX_DATA_VER = 1;
        private readonly byte[] CODE_TEX = { 17, 02, 41, 01, 6 };
        public byte ubChecker = byte.MinValue;

        public uint TexBitwise(uint i)
        {
            uint ulChecker = 0;

            ulChecker |= (uint)(ubChecker += CODE_TEX[0]) << 24;
            ulChecker |= (uint)(ubChecker += CODE_TEX[1]) << 16;
            ulChecker |= (uint)(ubChecker += CODE_TEX[2]) << 8;
            ulChecker |= (uint)(ubChecker += CODE_TEX[3]) << 0;

            ubChecker += CODE_TEX[4];
            return i ^ ulChecker;
        }

        public int AnimationCount;
        public CTextureAnimation[] Animation;
        public Bitmap[] BitmapFrames;
    }

    public class CTextureAnimation
    {
        public string Name;
        public float FrameDuration;
        public int FrameCount;
        // public int[] FrameIndices;
    }

    public enum TextureFormat
    {
        DXT1,
        DXT3,
        DXT5,
        RGBA,
        RGB
    }

    public enum TextureFlag : ulong
    {
        TEX_ALPHACHANNEL = (1UL << 0),
        TEX_32BIT = (1UL << 1),
        TEX_COMPRESSED = (1UL << 2),
        TEX_TRANSPARENT = (1UL << 3),
        TEX_EQUALIZED = (1UL << 4),
        TEX_COMPRESSEDALPHA = (1UL << 5),
        TEX_STATIC = (1UL << 8),
        TEX_CONSTANT = (1UL << 9),
        TEX_GRAY = (1UL << 10),
        TEX_COMPRESS = (1UL << 16),
        TEX_COMPRESSALPHA = (1UL << 17),
        TEX_SINGLEMIPMAP = (1UL << 18),
        TEX_PROBED = (1UL << 19),
        TEX_DISPOSED = (1UL << 20),
        TEX_DITHERED = (1UL << 21),
        TEX_FILTERED = (1UL << 22),
        TEX_COLORIZED = (1UL << 24),
        TEX_WASOLD = (1UL << 30)
    }
}
