using UnityEngine;

public class Testing : MonoBehaviour 
{
    [SerializeField] private TilemapVisual tilemapVisual;
    [SerializeField] private Camera mainCamera;
    private Tilemap tilemap;
    private Tilemap.TilemapObject.TilemapSprite tilemapSprite;

    private int width = 16, height = 11;
    private float size = 12.0f;

    private void Start() 
    {
        tilemap = new Tilemap(width, height, size, new Vector2(-(width / 2 * size) + 27.5f, -(height / 2 * size)));
        tilemap.grid.DebugSwitch(true);

        tilemap.SetTilemapVisual(tilemapVisual);
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update() 
    {
        if (Input.GetMouseButton(0)) 
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            tilemap.SetTilemapSprite(mouseWorldPosition, tilemapSprite);
        }
        
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            Save();
            Debug.Log("Saved!");
        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            Load();
            Debug.Log("Loaded!");
        }
    }

    public void Save()
    {
        tilemap.Save();
    }    
    
    public void Load()
    {
        tilemap.Load();
    }

    public void None()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
        Debug.Log("None!");
    }

    public void Dirt()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Dirt;
        Debug.Log("Dirt!");
    }

    public void Grass()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Grass;
        Debug.Log("Grass!");
    }

    public void Cactus()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Cactus;
        Debug.Log("Cactus!");
    }

    public void Tree()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Tree;
        Debug.Log("Tree!");
    }

    public void BLRock()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.BLRock;
        Debug.Log("BLRock!");
    }

    public void BMRock()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.BMRock;
        Debug.Log("BMRock!");
    }

    public void BRRock()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.BRRock;
        Debug.Log("BRRock!");
    }

    public void TRRock()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.TRRock;
        Debug.Log("TRRock!");
    }

    public void TMRock()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.TMRock;
        Debug.Log("TMRock!");
    }

    public void TLRock()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.TLRock;
        Debug.Log("TLRock!");
    }

    public void YDirt()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.YDirt;
        Debug.Log("YDirt!");
    }

    public void TLWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.TLWater;
        Debug.Log("TLWater!");
    }

    public void TMWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.TMWater;
        Debug.Log("TMWater!");
    }

    public void TRWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.TRWater;
        Debug.Log("TRWater!");
    }

    public void MLWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.MLWater;
        Debug.Log("MLWater!");
    }

    public void MMWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.MMWater;
        Debug.Log("MMWater!");
    }

    public void MRWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.MRWater;
        Debug.Log("MRWater!");
    }

    public void BLWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.BLWater;
        Debug.Log("BLWater!");
    }

    public void BMWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.BMWater;
        Debug.Log("BMWater!");
    }

    public void BRWater()
    {
        tilemapSprite = Tilemap.TilemapObject.TilemapSprite.BRWater;
        Debug.Log("BRWater!");
    }
}