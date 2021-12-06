using UnityEngine;

public class LoadingLevels : MonoBehaviour
{
    public TilemapVisual tilemapVisual;
    private static int areas = 4; // Change this based on how many saves you have
                                  // (if you only have 1 save you want to load then areas = 1, if you have 2 then areas = 2 etc.)
    private Tilemap[] tilemap = new Tilemap[areas];
    private Vector3[] locations = new Vector3 [areas];
    private const int width = 16, height = 11;
    private const float size = 0.9f;
    string num = "";

    float areaSizeX = (width * size);
    float areaSizeY = (height * size);

    void Start()
    {
        locations[0] = new Vector3(-areaSizeX / 2, -areaSizeY / 2, 2);
        locations[1] = new Vector3(areaSizeX / 2, -areaSizeY / 2, 2);
        locations[2] = new Vector3(-areaSizeX - (areaSizeX / 2), -areaSizeY / 2, 2);
        locations[3] = new Vector3(-areaSizeX / 2, -areaSizeY / 2, 2);

        for (int i = 0; i < areas; i++)
        {

            if (i == 0) { }
            else
            {
                num = "_" + (i + 1).ToString();
            }

            tilemap[i] = new Tilemap(width, height, size, locations[i]);
            //Debug.Log(num);
            TilemapVisual level = Instantiate(tilemapVisual, tilemapVisual.transform.position, tilemapVisual.transform.rotation);
            level.name = "Level " + i.ToString();

            tilemap[i].SetTilemapVisual(level);
            tilemap[i].Load("" + num);
        }
    }
}