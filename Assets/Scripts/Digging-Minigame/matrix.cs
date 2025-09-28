using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Abstract_Ammonites;
using System.Linq;

public class matrix : MonoBehaviour
{
    public int lvl_num;
    public GameObject level_parent;
    public GameObject current;
    public GameObject pl_tile;
    [System.Serializable]
    public struct TileProperties
    {
        public string tile_name;
        public bool is_pebbled;
        public GameObject gmobject;
        public int[] properties;
    }
    public TileProperties[] TilePropertieses;
    Vector3[,] mtrx;
    //will convert to vec2 later.
    private Tilemap level;
    private const string cube = "Cube";
    public Vector3[] rotations;
    [HideInInspector] public List<GameObject> tile_list;
    [SerializeField] private Vector3 lwr_bounds_left, up_bounds_right;

    private void Awake()
    {
        level_setup(PlayerPrefs.GetInt("level_to_load"));
        //level_setup(lvl_num);
    }
    public void level_setup(int lvl_num)
    {
        current = level_parent.transform.GetChild(lvl_num).gameObject;
        current.SetActive(true);
        create_puzzle();
    }
    private void create_puzzle()
    {
        tile_list = new List<GameObject>();
        GameObject level_container = GameObject.Find("Levels Parent");
        var real_lev_cont = level_container.transform.GetComponentInChildren<Transform>();
        foreach (Transform item in real_lev_cont)
        {
            if (item.parent == level_container.transform)
            {
                if (item.gameObject.activeSelf == true)
                {
                    level = item.GetComponent<Tilemap>();
                    item.GetComponent<TilemapRenderer>().enabled = false;
                    break;
                }
            }
        }
        //Basic Init
        lwr_bounds_left = level.origin;
        up_bounds_right = level.origin + level.size;
        mtrx = new Vector3[(int)up_bounds_right.x, (int)up_bounds_right.y];
        for (int i = 0; i < up_bounds_right.x; i++)
        {
            for (int j = 0; j < up_bounds_right.y; j++)
            {
                //grab them grid cells and assign them cords I actually know what to do with. while not sacrificing tilepainter
                mtrx[i, j] = new Vector3Int(i, j, 0);
                if (level.HasTile(level.LocalToCell(mtrx[i, j])))
                {
                    GameObject tile;
                    var nm = level.GetTile(level.WorldToCell(mtrx[i, j])).name;
                    for (var k = 0; k < TilePropertieses.Length; k++)
                    {
                        if (nm == TilePropertieses[k].tile_name)
                        {
                            //select a randomised rotation
                            Vector2 rando = rotations[Random.Range(0, rotations.Length)];
                            if (TilePropertieses[k].gmobject.name.Contains(cube))
                            {
                                tile = Instantiate(TilePropertieses[k].gmobject, mtrx[i, j], Quaternion.Euler(rando.x, rando.y, rotations[Random.Range(0, rotations.Length)].z));
                            }
                            else
                            {
                                tile = Instantiate(TilePropertieses[k].gmobject, mtrx[i, j], Quaternion.identity);
                            }
                            setup_init(mtrx[i, j], tile, TilePropertieses[k]);
                        }
                        else
                        {
                        }
                    }
                }
            }
        }

        foreach (GameObject item in tile_list)
        {
            item.GetComponent<simple_tile>().contents.update_neighours(tile_list);
        }
    }
    private tile_base setup_init(Vector2 pos, GameObject tile, TileProperties input)
    {
        //null check
        if (tile.TryGetComponent(out simple_tile data))
        {
            var tile_properties = new HashSet<World_Utils.p_atts>();
            foreach (var item in input.properties)
            {
                tile_properties.Add((World_Utils.p_atts)item);
            }
            // is spawnpoint tile
            if (tile_properties.Contains(World_Utils.p_atts.spawnpoint))
            {
                GameObject Player = Singleton.Instantiate(pl_tile, pos, Quaternion.identity);
                Player.GetComponent<simple_tile>().contents = new player_tile();
                var player_data = Player.GetComponent<simple_tile>().contents;
                player_data.init(Player, new HashSet<World_Utils.p_atts> { World_Utils.p_atts.cant_be_pebbled, World_Utils.p_atts.player });
                player_data.positional_cord = pos;
                player_data.puzzle_moves = current.GetComponent<MovesNumber>().number;
                Player.GetComponent<TempDiggingPlayer>().num_of_moves_left = current.GetComponent<MovesNumber>().number;
                tile_list.Add(Player);
            }
            data.contents = new basic_material();
            data.contents.init(tile, tile_properties);
            data.contents.positional_cord = pos;
            if (tile_properties.Contains(World_Utils.p_atts.appear_at_end))
            {
                tile.SetActive(false);
            }
        }
        if (input.is_pebbled) World_Utils.pebble(data.contents);
        tile_list.Add(tile);

        return data.contents;
    }
}

