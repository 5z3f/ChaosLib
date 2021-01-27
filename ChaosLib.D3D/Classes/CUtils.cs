using System;
using System.Numerics;
using System.Collections.Generic;

using ChaosLib.D3D.Structures;

namespace ChaosLib.D3D.Classes
{
    class CUtils
    {
        public static float[,] Q2M(Quaternion Q)
        {
            float q0 = Q.W;
            float q1 = Q.X;
            float q2 = Q.Y;
            float q3 = Q.Z;

            // First row of the rotation matrix
            float r00 = 2 * (q0 * q0 + q1 * q1) - 1;
            float r01 = 2 * (q1 * q2 - q0 * q3);
            float r02 = 2 * (q1 * q3 + q0 * q2);

            // Second row of the rotation matrix
            float r10 = 2 * (q1 * q2 + q0 * q3);
            float r11 = 2 * (q0 * q0 + q2 * q2) - 1;
            float r12 = 2 * (q2 * q3 - q0 * q1);

            // Third row of the rotation matrix
            float r20 = 2 * (q1 * q3 - q0 * q2);
            float r21 = 2 * (q2 * q3 + q0 * q1);
            float r22 = 2 * (q0 * q0 + q3 * q3) - 1;

            return new float[3, 3]
            {
                { r00, r01, r02 },
                { r10, r11, r12 },
                { r20, r21, r22 }
            };
        }

        public static object SortByFrame(int f, CAnimationPosition[] p, CAnimationRotation[] r)
        {
            var sp = new CAnimationPosition[f];
            for (int i = 0; i < p.Length; i++)
                sp[p[i].Frame] = p[i];

            var sr = new CAnimationRotation[f];
            for (int i = 0; i < r.Length; i++)
                sr[r[i].Frame] = r[i];

            return new { pos = sp, rot = sr };
        }


        public static dynamic Normalize(float x, float y, float z, float w)
        {
            float mag = MathF.Sqrt(x * x + y * y + z * z + w * w);

            if (mag < float.Epsilon)
                return new { X = 0F, Y = 0F, Z = 0F, W = 1F };

            return new { X = x / mag, Y = y / mag, Z = z / mag, W = w / mag };
        }

        public static List<CSkeletonBone> findBones(CSkeletonBone[] bone, string parent)
        {
            List<CSkeletonBone> newBone = new List<CSkeletonBone>();
            for (int j = 0; j < bone.Length; j++)
            {
                if (bone[j].ParentID != parent)
                    continue;

                newBone.Add(bone[j]);
            }

            return newBone;
        }

        public static CSkeletonBone[] sortBones(CSkeletonBone[] bones)
        {
            string boneName = string.Empty;

            List<CSkeletonBone> newBone = findBones(bones, boneName);
            for (int i = 0; i < newBone.Count; i++)
            {
                boneName = newBone[i].ParentID;

                List<CSkeletonBone> newBone2 = findBones(bones, boneName);
                for (int j = 0; j < newBone2.Count; j++)
                    newBone.Add(newBone2[j]);
            }

            return newBone.ToArray();
        }
    }
}
