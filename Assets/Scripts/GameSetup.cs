
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    Material sand, road, gold, stone, green, brown;

    void Start()
    {
        CreateWorld();
    }

    Material MakeMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
            shader = Shader.Find("Standard");

        Material mat = new Material(shader);
        mat.color = color;
        return mat;
    }

    void CreateWorld()
    {
        sand = MakeMaterial(new Color(0.86f, 0.68f, 0.42f));
        road = MakeMaterial(new Color(0.48f, 0.34f, 0.23f));
        gold = MakeMaterial(new Color(1f, 0.72f, 0.08f));
        stone = MakeMaterial(new Color(0.88f, 0.72f, 0.48f));
        green = MakeMaterial(new Color(0.12f, 0.38f, 0.12f));
        brown = MakeMaterial(new Color(0.35f, 0.20f, 0.09f));

        Cube("Desert Ground", new Vector3(0, -0.25f, 55),
            new Vector3(50, 0.5f, 150), sand);

        Cube("Ancient Running Road", new Vector3(0, 0.02f, 55),
            new Vector3(12, 0.2f, 150), road);

        Cube("Left Road Edge", new Vector3(-6, 0.16f, 55),
            new Vector3(0.25f, 0.12f, 150), gold);

        Cube("Right Road Edge", new Vector3(6, 0.16f, 55),
            new Vector3(0.25f, 0.12f, 150), gold);

        for (int z = 0; z < 140; z += 7)
        {
            Cube("Left Lane Marking", new Vector3(-2, 0.15f, z),
                new Vector3(0.09f, 0.04f, 3), gold);

            Cube("Right Lane Marking", new Vector3(2, 0.15f, z),
                new Vector3(0.09f, 0.04f, 3), gold);
        }

        Pyramid(new Vector3(-12, 0, 15), 9);
        Pyramid(new Vector3(12, 0, 30), 11);
        Pyramid(new Vector3(-14, 0, 55), 12);
        Pyramid(new Vector3(14, 0, 85), 10);
        Pyramid(new Vector3(-12, 0, 115), 9);

        for (int z = 5; z < 135; z += 16)
        {
            Palm(new Vector3(-9, 0, z));
            Palm(new Vector3(9, 0, z + 8));
        }

        for (int z = 35; z < 115; z += 25)
        {
            Pillar(new Vector3(-7.5f, 0, z));
            Pillar(new Vector3(7.5f, 0, z));
        }

        CreateCoins();
        CreateObstacles();

        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0, 6, -9);
            cam.transform.rotation = Quaternion.Euler(22, 0, 0);
            cam.fieldOfView = 65;
            cam.backgroundColor = new Color(0.65f, 0.82f, 0.95f);
        }

        RenderSettings.ambientLight = new Color(0.75f, 0.68f, 0.55f);

        Debug.Log("Ancient Egypt world and obstacles created!");
    }

    void Cube(string name, Vector3 pos, Vector3 scale, Material mat)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.position = pos;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().material = mat;
    }

    void Pyramid(Vector3 pos, float size)
    {
        GameObject pyramid = new GameObject("Egyptian Pyramid");
        MeshFilter filter = pyramid.AddComponent<MeshFilter>();
        MeshRenderer renderer = pyramid.AddComponent<MeshRenderer>();

        float h = size / 2;

        Vector3[] vertices =
        {
            new Vector3(-h, 0, -h),
            new Vector3(h, 0, -h),
            new Vector3(h, 0, h),
            new Vector3(-h, 0, h),
            new Vector3(0, size, 0)
        };

        int[] triangles =
        {
            0, 2, 1, 0, 3, 2,
            0, 1, 4, 1, 2, 4,
            2, 3, 4, 3, 0, 4
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        filter.mesh = mesh;
        renderer.material = stone;
        pyramid.transform.position = pos;
    }

    void Palm(Vector3 pos)
    {
        GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunk.name = "Palm Tree Trunk";
        trunk.transform.position = pos + new Vector3(0, 2, 0);
        trunk.transform.localScale = new Vector3(0.35f, 2, 0.35f);
        trunk.GetComponent<Renderer>().material = brown;

        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60f;

            GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leaf.name = "Palm Leaf";
            leaf.transform.position = pos + new Vector3(0, 4, 0);
            leaf.transform.rotation = Quaternion.Euler(0, angle, 25);
            leaf.transform.localScale = new Vector3(0.45f, 0.12f, 2.5f);
            leaf.GetComponent<Renderer>().material = green;
        }
    }

    void Pillar(Vector3 pos)
    {
        GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pillar.name = "Egyptian Temple Pillar";
        pillar.transform.position = pos + new Vector3(0, 1.8f, 0);
        pillar.transform.localScale = new Vector3(0.65f, 1.8f, 0.65f);
        pillar.GetComponent<Renderer>().material = stone;

        Cube("Pillar Top", pos + new Vector3(0, 3.7f, 0),
            new Vector3(1.3f, 0.3f, 1.3f), gold);
    }

    void CreateCoins()
    {
        Material coinMaterial = MakeMaterial(new Color(1f, 0.75f, 0.05f));

        for (int z = 12; z < 140; z += 10)
        {
            int lane = (z / 10) % 3;
            float x = (lane - 1) * 2f;

            GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coin.name = "Gold Coin";
            coin.transform.position = new Vector3(x, 1.2f, z);
            coin.transform.rotation = Quaternion.Euler(90, 0, 0);
            coin.transform.localScale = new Vector3(0.8f, 0.12f, 0.8f);
            coin.GetComponent<Renderer>().material = coinMaterial;

            Collider coinCollider = coin.GetComponent<Collider>();
            coinCollider.isTrigger = true;

            Rigidbody rb = coin.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void CreateObstacles()
    {
        Material obstacleMaterial = MakeMaterial(
            new Color(0.55f, 0.36f, 0.20f)
        );

        // Clay pots
        CreatePot(-2, 25, obstacleMaterial);
        CreatePot(2, 55, obstacleMaterial);
        CreatePot(0, 95, obstacleMaterial);

        // Stone blocks
        CreateBlock(2, 35, obstacleMaterial);
        CreateBlock(-2, 65, obstacleMaterial);
        CreateBlock(0, 115, obstacleMaterial);

        // Low stone barriers that the boy can jump over
        CreateBarrier(0, 45, obstacleMaterial);
        CreateBarrier(2, 75, obstacleMaterial);
        CreateBarrier(-2, 105, obstacleMaterial);

        Debug.Log("Egyptian obstacles created!");
    }

    void CreatePot(float x, float z, Material mat)
    {
        GameObject pot = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pot.name = "Egyptian Clay Pot";
        pot.transform.position = new Vector3(x, 0.65f, z);
        pot.transform.localScale = new Vector3(0.9f, 0.65f, 0.9f);
        pot.GetComponent<Renderer>().material = mat;
    }

    void CreateBlock(float x, float z, Material mat)
    {
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.name = "Egyptian Stone Block";
        block.transform.position = new Vector3(x, 0.65f, z);
        block.transform.localScale = new Vector3(1.3f, 1.3f, 1.2f);
        block.GetComponent<Renderer>().material = mat;
    }

    void CreateBarrier(float x, float z, Material mat)
    {
        GameObject barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
        barrier.name = "Low Egyptian Stone Barrier";
        barrier.transform.position = new Vector3(x, 0.35f, z);
        barrier.transform.localScale = new Vector3(1.6f, 0.7f, 0.6f);
        barrier.GetComponent<Renderer>().material = mat;
    }
}
