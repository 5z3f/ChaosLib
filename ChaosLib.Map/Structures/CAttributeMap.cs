using System;
using System.Collections.Generic;
using System.Drawing;
using BinarySerialization;

namespace ChaosLib.Map.Structures
{
    public class CAttributeMap
    {
        enum MapAttribute : ushort
        {
            MATT_WALKABLE = 1,
            MATT_PEACE = 2,
            MATT_PRODUCT_PUBLIC = 4,
            MATT_PRODUCT_PRIVATE = 8,
            MATT_STAIR_UP = 16,
            MATT_STAIR_DOWN = 32,
            MATT_WAR = 64,
            MATT_FREEPKZONE = 128,
            MATT_UNWALKABLE = 32768
        }

        enum MapAttributeOld : byte
        {
            MATT_WALKABLE = 0,
            MATT_PEACE = 10,
            MATT_PRODUCT_PUBLIC = 20,
            MATT_PRODUCT_PRIVATE = 30,
            MATT_STAIR_UP = 40,
            MATT_STAIR_DOWN = 50,
            MATT_WAR = 60,
            MATT_FREEPKZONE = 70,
            MATT_UNWALKABLE = 255
        }

        public static byte[] ColorizeAttributes(byte[,] data, int width, int height)
        {
            var byteMap = new List<byte>();

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    var color = (MapAttributeOld)data[w, h] switch
                    {
                        MapAttributeOld.MATT_WALKABLE => Color.White,
                        MapAttributeOld.MATT_UNWALKABLE => Color.Black,
                        MapAttributeOld.MATT_PEACE => Color.LimeGreen,
                        MapAttributeOld.MATT_FREEPKZONE => Color.LightSalmon,
                        MapAttributeOld.MATT_WAR => Color.IndianRed,
                        MapAttributeOld.MATT_STAIR_UP => Color.Violet,
                        MapAttributeOld.MATT_STAIR_DOWN => Color.Wheat,
                        MapAttributeOld.MATT_PRODUCT_PUBLIC => Color.MistyRose,
                        MapAttributeOld.MATT_PRODUCT_PRIVATE => Color.Maroon,

                        _ => Color.Black
                    };

                    byteMap.Add(color.B);
                    byteMap.Add(color.G);
                    byteMap.Add(color.R);
                    byteMap.Add(byte.MaxValue);

                    // bmp.SetPixel(w, h, color); => another way but slower
                }
            }

            return byteMap.ToArray();
        }

        public static byte[] ColorizeAttributes(ushort[,] data, int width, int height)
        {
            var byteMap = new List<byte>();

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    var color = (MapAttribute)data[w, h] switch
                    {
                        MapAttribute.MATT_WALKABLE => Color.White,
                        MapAttribute.MATT_UNWALKABLE => Color.Black,
                        MapAttribute.MATT_PEACE => Color.LimeGreen,
                        MapAttribute.MATT_FREEPKZONE => Color.LightSalmon,
                        MapAttribute.MATT_WAR => Color.IndianRed,
                        MapAttribute.MATT_STAIR_UP => Color.Violet,
                        MapAttribute.MATT_STAIR_DOWN => Color.Wheat,
                        MapAttribute.MATT_PRODUCT_PUBLIC => Color.MistyRose,
                        MapAttribute.MATT_PRODUCT_PRIVATE => Color.Maroon,

                        _ => Color.Black
                    };

                    byteMap.Add(color.B);
                    byteMap.Add(color.G);
                    byteMap.Add(color.R);
                    byteMap.Add(byte.MaxValue);

                    // bmp.SetPixel(w, h, color); => another way but slower
                }
            }

            return byteMap.ToArray();
        }

        public static Bitmap[] ColorizeAttributesAndKeepLayers(byte[,] data, int width, int height)
        {
            var bmpLayers = new Bitmap[9];

            for (int i = 0; i < 9; i++)
                bmpLayers[i] = new Bitmap(width, height);

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    dynamic layerdata = (MapAttributeOld)data[w, h] switch
                    {
                        MapAttributeOld.MATT_WALKABLE => new { id = 0, color = Color.White },
                        MapAttributeOld.MATT_UNWALKABLE => new { id = 1, color = Color.Black },
                        MapAttributeOld.MATT_PEACE => new { id = 2, color = Color.LimeGreen },
                        MapAttributeOld.MATT_FREEPKZONE => new { id = 3, color = Color.LightSalmon },
                        MapAttributeOld.MATT_WAR => new { id = 4, color = Color.IndianRed },
                        MapAttributeOld.MATT_STAIR_UP => new { id = 5, color = Color.Violet },
                        MapAttributeOld.MATT_STAIR_DOWN => new { id = 6, color = Color.Wheat },
                        MapAttributeOld.MATT_PRODUCT_PUBLIC => new { id = 7, color = Color.MistyRose },
                        MapAttributeOld.MATT_PRODUCT_PRIVATE => new { id = 8, color = Color.Maroon },

                        _ => new { id = 1, color = Color.Black }
                    };

                    bmpLayers[layerdata.id].SetPixel(w, h, layerdata.color);
                }
            }

            return bmpLayers;
        }

        public static Bitmap[] ColorizeAttributesAndKeepLayers(ushort[,] data, int width, int height)
        {
            var bmpLayers = new Bitmap[8];

            for (int i = 0; i < 8; i++)
                bmpLayers[i] = new Bitmap(width, height);

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    dynamic layerdata = (MapAttribute)data[w, h] switch
                    {
                        MapAttribute.MATT_WALKABLE => new { id = 0, color = Color.White },
                        MapAttribute.MATT_UNWALKABLE => new { id = 1, color = Color.Black },
                        MapAttribute.MATT_PEACE => new { id = 2, color = Color.LimeGreen },
                        MapAttribute.MATT_FREEPKZONE => new { id = 3, color = Color.LightSalmon },
                        MapAttribute.MATT_WAR => new { id = 4, color = Color.IndianRed },
                        MapAttribute.MATT_STAIR_UP => new { id = 5, color = Color.Violet },
                        MapAttribute.MATT_STAIR_DOWN => new { id = 6, color = Color.Wheat },
                        MapAttribute.MATT_PRODUCT_PUBLIC => new { id = 7, color = Color.MistyRose },
                        MapAttribute.MATT_PRODUCT_PRIVATE => new { id = 8, color = Color.Maroon },

                        _ => new { id = 1, color = Color.Black }
                    };

                    bmpLayers[layerdata.id].SetPixel(w, h, layerdata.color);
                }
            }

            return bmpLayers;
        }
    }
}
