using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtilities 
{
    public static Quaternion[] cashedQuaternionEulerArr;

    //creates an empty custom mesh with quadNo of quads put this method into a mesh seperate mesh call later on
    public static void CreateEmptyMeshArray(int quadNo, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
    {
        vertices = new Vector3[4 * quadNo];
        uvs = new Vector2[4 * quadNo];
        triangles = new int[6 * quadNo];
    }

    //put this method into a mesh seperate mesh call later on, adds a material to each of the quads created earlier
    public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rotation, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {
        //relocates the vertices
        int vertexIndex = index * 4;
        int vIndex0 = vertexIndex;
        int vIndex1 = vertexIndex + 1;
        int vIndex2 = vertexIndex + 2;
        int vIndex3 = vertexIndex + 3;

        //relocates the uvs
        uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
        uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
        uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
        uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

        //Create triangles
        int tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex3;
        triangles[tIndex + 2] = vIndex1;

        triangles[tIndex + 3] = vIndex1;
        triangles[tIndex + 4] = vIndex3;
        triangles[tIndex + 5] = vIndex2;

        baseSize *= 0.5f;
        bool skewed = baseSize.x != baseSize.y;
        if (skewed == true)
        {
            vertices[vIndex0] = pos + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, baseSize.y);
            vertices[vIndex1] = pos + GetQuaternionEuler(rotation) * new Vector3(-baseSize.x, -baseSize.y);
            vertices[vIndex2] = pos + GetQuaternionEuler(rotation) * new Vector3(baseSize.x, -baseSize.y);
            vertices[vIndex3] = pos + GetQuaternionEuler(rotation) * baseSize;
        }
        else
        {
            vertices[vIndex0] = pos + GetQuaternionEuler(rotation - 270) * baseSize;
            vertices[vIndex1] = pos + GetQuaternionEuler(rotation - 180) * baseSize;
            vertices[vIndex2] = pos + GetQuaternionEuler(rotation - 90) * baseSize;
            vertices[vIndex3] = pos + GetQuaternionEuler(rotation) * baseSize;
        }

    }

    public static void CasheQuaternionEuler()
    {
        if (cashedQuaternionEulerArr != null)
        {
            return;
        }
        else
        {
            cashedQuaternionEulerArr = new Quaternion[360];
            for (int i = 0; i < 360; i++)
            {
                cashedQuaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
            }
        }
    }

    //put this method into a mesh seperate mesh call later on
    public static Quaternion GetQuaternionEuler(float rotation)
    {
        int rot = Mathf.RoundToInt(rotation);
        rot = rot % 360;
        if (rot < 0)
        {
            rot += 360;
        }
        if (cashedQuaternionEulerArr == null)
        {
            CasheQuaternionEuler();
        }
        return cashedQuaternionEulerArr[rot];
    }
}
