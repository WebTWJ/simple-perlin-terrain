using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
public class TerrainScript : MonoBehaviour
{
    // Start is called before the first frame update

    public int width = 20;
    public int height = 20;
    public int size;
    public float scale = 10f;
    public float offsetX = 0f;
    public float offsetY = 0f;
    public float offsetXIncreaser = 0f;
    public float offsetYIncreaser = 0f;
    public float maxHeight = 5f;
    public float minHeight = 0f;
/*    public float parallaxHeight = 0.5f;
    public float offsetXNormal = 0f;
    public float offsetYNormal = 0f;
    public float scaleNormal = 1f;*/

    Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    Color[] colors;
    public Gradient gradient;

    private void Update()
    {
        size = width * height;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateMesh();
        UpdateMesh();

        offsetX += offsetXIncreaser;
        offsetY += offsetYIncreaser;
    }
    Texture2D GenerateTexture(int width, int height, float offsetX, float offsetY, float scale)
    {
        Texture2D texture = new Texture2D(width, height);

        //GENERATE A PERLIN NOISE MAP

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return (texture);
    }

    Color CalculateColor(int x, int y)
    {
        float xCord = (float)x / width * scale + offsetX;
        float yCord = (float)y / height * scale + offsetY;

        float sample = Mathf.PerlinNoise(xCord, yCord);
        return new Color(sample, sample, sample);
    }

    void CreateMesh()
    {
        vertices = new Vector3[(width + 1) * (height + 1)];

        for (int vertexCount = 0, z = 0; z <= width; z++)
        {
            for (int x = 0; x <= height; x++)
            {
                float xCord = (float)x / width * scale + offsetX;
                float yCord = (float)z / height * scale + offsetY;
                float y = Mathf.PerlinNoise(xCord, yCord) * maxHeight;
                if (y > maxHeight)
                {
                    y = maxHeight;
                }
                if (y < minHeight)
                {
                    y = minHeight;
                }
                vertices[vertexCount] = new Vector3(x, y, z);
                vertexCount++;
            }
        }

        triangles = new int[width * height * 6];
        for (int z = 0, triangleCount = 0, vertexCount = 0; z < width; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[triangleCount] = vertexCount;
                triangles[triangleCount + 1] = vertexCount + width + 1;
                triangles[triangleCount + 2] = vertexCount + 1;
                triangles[triangleCount + 3] = vertexCount + 1;
                triangles[triangleCount + 4] = vertexCount + width + 1;
                triangles[triangleCount + 5] = vertexCount + width + 2;
                triangleCount += 6;
                vertexCount++;
            }
            vertexCount++;
        }

        colors = new Color[vertices.Length];
        for (int vertexCount = 0, z = 0; z <= width; z++)
        {
            for (int x = 0; x <= height; x++)
            {
                float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[vertexCount].y);
                colors[vertexCount] = gradient.Evaluate(height);
                vertexCount++;
            }
        }

        uvs = new Vector2[vertices.Length];

        for (int vertexCount = 0, z = 0; z <= width; z++)
        {
            for (int x = 0; x <= height; x++)
            {
                uvs[vertexCount] = new Vector2((float)x / width, (float)z / height);
            }
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

/*    private void OnDrawGizmos()
    {
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }*/
}
