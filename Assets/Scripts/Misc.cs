using System.Collections.Generic;
using UnityEngine;

public static class MyExtensions
{
    public static Vector3[] Multiply(this Vector3[] a, Vector3 b)
    {
        Vector3[] res = new Vector3[a.Length];
        for ( int i = 0; i < a.Length; i++ )
        {
            res[i] = a[i].Multiply(b);
        }

        return res;
    }

    public static Vector3 Multiply(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static Vector3[] Plus(this Vector3[] a, Vector3 b)
    {
        Vector3[] res = new Vector3[a.Length];
        for ( int i = 0; i < a.Length; i++ )
        {
            res[i] = a[i] + b;
        }

        return res;
    }

    public static Vector3 Add(this Vector3 obj, int x)
    {
        return new Vector3(obj.x + x, obj.y + x, obj.z + x);
    }
    public static Vector3 Add(this Vector3 obj, float x)
    {
        return new Vector3(obj.x + x, obj.y + x, obj.z + x);
    }

    public static int[,] Add(this int[,] a, Vector3 b)
    {
        int[,] res = new int[a.GetLength(0),a.GetLength(1)];

        for ( int i = 0; i < a.GetLength(0); i++ )
        {
            for ( int j = 0; j < a.GetLength(1); j += 3 )
            {
                res[i, j] =     a[i,j] + (int)b.x;
                res[i, j + 1] = a[i,j + 1] + (int)b.y;
                res[i, j + 2] = a[i,j + 2] + (int)b.z;
            }
        }

        return res;
    }


}

public static class Noise
{
    public static float[,] GenerateNoiseMap(int xSize, int ySize, Vector2 offsetPos, float noiseScale = 0.1f, int maximumHeight = 1)
    {
        float[,] noiseMap = new float[xSize, ySize];
        float[] samplePos = new float[2];

        for ( int i = 0; i < xSize; i++ )
        {
            for ( int j = 0; j < ySize; j++ )
            {
                samplePos[0] = i * noiseScale + offsetPos.x;
                samplePos[1] = j * noiseScale + offsetPos.y;

                noiseMap[i, j] = Mathf.PerlinNoise(samplePos[0], samplePos[1]) * maximumHeight;
            }
        }

        return noiseMap;
    }
}
