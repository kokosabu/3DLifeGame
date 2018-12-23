using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Cell
{
    public GameObject gameObject;
    public int state;
};

public class LifeGameByCreatePrimitive : MonoBehaviour
{
    [Range(5, 100)] public int width =  8;
    [Range(5, 100)] public int height = 8;
    [Range(5, 100)] public int depth =  8;
    [Range(1, 10)] public float spacing = 2f;
    public Material lifeMaterial;
    public Material deadMaterial;
    private Cell[,,] cells;
    private Color life;
    private Color dead;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 10;

        cells = new Cell[width, height, depth];
        float halfW = width * spacing * 0.5f;
        float halfH = height * spacing * 0.5f;
        float halfD = depth * spacing * 0.5f;    

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                for (int d = 0; d < depth; d++)
                {
                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    gameObject.GetComponent<Renderer>().material = deadMaterial;
                    gameObject.transform.parent = this.transform;
                    gameObject.transform.position = new Vector3(w * spacing - halfW, h * spacing - halfH, d * spacing - halfD);
                    renderer.receiveShadows = false;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                   
                    cells[w, h, d].gameObject = gameObject;
                    cells[w, h, d].state = Random.value < 0.5 ? 1 : 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Cell[,,] nextCells = new Cell[width, height, depth];

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                for (int d = 0; d < depth; d++)
                {
                    if (cells[w, h, d].state == 1)
                    {
                        cells[w, h, d].gameObject.GetComponent<Renderer>().material = lifeMaterial;
                    }
                    else
                    {
                        cells[w, h, d].gameObject.GetComponent<Renderer>().material = deadMaterial;
                    }

                    int neighbors = 0;
                    for (int h2 = height-1; h2 < height+1; h2++)
                    {
                        for(int w2 = width-1; w2 < width+1; w2++)
                        {
                            for(int d2 = depth-1; d2 < depth+1; d2++)
                            {
                                if (h2 == height && w2 == width && d2 == depth) continue;

                                neighbors += cells[(w + w2) % width, (h + h2) % height, (d + d2) % depth].state;
                            }
                        }
                    }

                    int prevState = cells[w, h, d].state;
                    int nextState = (prevState == 1 && (neighbors >= 4 && neighbors <= 13)) ||
                                    (prevState == 0 && (neighbors >= 4 && neighbors <= 7)) ? 1 : 0;
                    nextCells[w, h, d].gameObject = cells[w, h, d].gameObject;
                    nextCells[w, h, d].state = nextState;
                }
            }
        }
        cells = nextCells;
    }
}
