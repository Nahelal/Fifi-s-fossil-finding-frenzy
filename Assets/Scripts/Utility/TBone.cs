using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TBone;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable CS8602
#nullable enable
namespace TBone
{
    //GENERAL UTILITY CLASS, mainly used for 3d enviroment. Check 
    public class Utils
    {
        public enum dirs
        {
            FORWARD = 1,
            BACKWARDS,
            LEFT,
            RIGHT,
            UP,
            DOWN
        }
        public Vector2[] vectors = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        public enum tile_names
        {
            DIRT,
            SAND,
            STONE,
        }
        public static Dictionary<dirs, Vector3> return_dir_dict()
        {
            Dictionary<dirs, Vector3> direction_directory = new Dictionary<dirs, Vector3>
            {
                { dirs.FORWARD, new Vector3(1, 0, 0) },
                { dirs.BACKWARDS, new Vector3(-1, 0, 0) },
                { dirs.LEFT, new Vector3(0, 0, 1) },
                { dirs.RIGHT, new Vector3(0, 0, -1) },
                { dirs.DOWN, new Vector3(0, -1, 0) }
            };
            direction_directory.Add(dirs.UP, -direction_directory[dirs.DOWN]);

            return direction_directory;
        }

        public static void apply_gravity(GameObject character, Dictionary<dirs, Vector3> directory, float mult)
        {
            character.GetComponent<CharacterController>().SimpleMove(directory[dirs.UP] * mult);
        }

        public static void rotate_character(GameObject us, Vector3 look_target)
        {
            us.transform.rotation = Quaternion.RotateTowards(us.transform.rotation, Quaternion.LookRotation(look_target), 1f);
        }



        //Gravity for tiles and player. called in world tick
        public static void fall_with_direction(GameObject current_obj, Vector2 target_position)
        {
            var content = current_obj.GetComponent<simple_tile>().contents;
            IEnumerator interior_function(GameObject current_obj, Vector2 target_position)
            {
                do
                {
                    current_obj.transform.position = Vector2.MoveTowards(current_obj.transform.position, target_position, 0.1f);
                    yield return null;
                }
                while ((Vector2)current_obj.transform.position != target_position);
                current_obj.transform.position = target_position;
                content.positional_cord = target_position;
                content.update_neighours(Abstract_Ammonites.World_Utils.grab_collection());
                content.gravity_worker = null;
            }
            if (content.gravity_worker == null)
            {
                content.gravity_worker = Singleton.Instance.StartCoroutine(interior_function(current_obj, target_position));
            }
        }
    }
}



namespace Abstract_Ammonites
{
    public class World_Utils : MonoBehaviour
    {

        public enum p_atts
        {
            fragile = 0,
            has_gravity = 1,
            creates_pebble = 2,
            fossil = 3,
            pebble = 4,
            unbreakable = 5,
            gets_squished_by_falling_blocks = 6,
            player = 7,
            spawnpoint = 8,
            returnpoint = 9,
            fullbackground = 10,
            not_climbable = 11,
            cant_be_pebbled = 12,

            appear_at_end = 13
        }


        public static List<GameObject> grab_collection()
        {
            return GameObject.Find("Matrix").GetComponent<matrix>().tile_list;
        }

        //tick the enviroment or a collection of tiles, technically we could impliment batch processing to allieviate performance?
        public static int Tick_Tile_Collection(List<GameObject> tiles)
        {
            int fos_count = 0;
            foreach (GameObject tile in tiles)
            {
                if (tile != null)
                {
                    tile_base current_tile = tile.GetComponent<simple_tile>().contents;
                    current_tile.update_neighours(tiles);
                    //Condition_bus
                    if (current_tile.properties.Contains(p_atts.fossil))
                    {
                        fos_count++;
                    }
                    if (current_tile.properties.Contains(p_atts.has_gravity))
                    {
                        //gravity tick!
                        if (!current_tile.properties.Contains(p_atts.pebble) && !current_tile.properties.Contains(p_atts.fossil))
                        {
                            if (current_tile.Neighbors[Vector2.down] == null || current_tile.Neighbors[Vector2.down].properties.Contains(p_atts.fullbackground))
                            {
                                TBone.Utils.fall_with_direction(tile, current_tile.positional_cord + Vector2.down);
                            }
                        }
                    }
                }
            }
            return fos_count;
        }

        //grab nearby neighbours.
        public static List<tile_base> recall_neighbors(tile_base? input_tile)
        {
            List<tile_base> neighbors = new List<tile_base>();
            foreach (tile_base? itm in input_tile.Neighbors.Values)
            {
                if (itm != null) neighbors.Add(itm);
            }
            return neighbors;
        }

        //explore and return breakable neighbours, uses BFS to grab tiles and add them to hashset, breaks slowly and triggers sound objs 
        public static IEnumerator recursively_break(player_tile player_self, tile_base start, p_atts property)
        {
            int num = 0;
            HashSet<tile_base> visited = new HashSet<tile_base>();
            Queue<tile_base> queue = new Queue<tile_base>();
            queue.Enqueue(start);
            visited.Add(start);
            do
            {
                tile_base current = queue.Dequeue();
                List<tile_base> valid_directions = recall_neighbors(current);
                for (var i = 0; i < valid_directions.Count; i++)
                {
                    tile_base itm = valid_directions.ElementAt(i);
                    if (!visited.Contains(itm) && itm.properties.Contains(property) && !itm.properties.Contains(p_atts.fossil) && !itm.properties.Contains(p_atts.pebble))
                    {
                        queue.Enqueue(itm);
                        visited.Add(itm);
                        num++;
                    }
                }
                World_Utils.create_soundob(5);
                yield return new WaitForSeconds(0.15f);
                if (item_try_destroy(current) != null) break;
            }
            while (queue.Count > 0);
            player_self.self.GetComponent<TempDiggingPlayer>().num_of_moves_left -= num;
            yield break;
        }

        //pebble a single tile
        public static void pebble(tile_base? item)
        {
            if (item != null && !item.properties.Contains(p_atts.cant_be_pebbled))
            {
                item.self.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                item.properties.Add(p_atts.pebble);
            }
        }
        //try pebbling surrounding tiles
        public static void pebble_surrounding(tile_base? current)
        {
            foreach (tile_base? item in recall_neighbors(current)) pebble(item);
        }

        //unpebbling a tile.
        public static void unpebble(tile_base? tile)
        {
            if (tile != null)
            {
                tile.self.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                tile.self.GetComponent<create_particle>().create_peb(tile.self.transform);
                tile.properties.Remove(p_atts.pebble);
            }
        }
        //try to destroy item if not pebbled. else return null.
        public static tile_base? item_try_destroy(tile_base tile)
        {
            //4 is for pebble
            if (tile.properties.Contains(p_atts.pebble))
            {
                unpebble(tile);

                return tile;
            }
            Destroy(tile.self);
            tile.update_neighours(grab_collection());
            return null;
        }

        //singleton ref to instanciate sound obs
        public static void create_soundob(int index)
        {
            GameObject ob = GameObject.Find("GravityManager").GetComponent<TileWatcher>().sound_prefabs[index];
            Singleton.Instantiate(ob);
        }


        //grab tile by type from entire map.
        public static List<tile_base?> grab_all_tiles_of_type(List<GameObject> collection, p_atts property)
        {
            List<tile_base?> found_tiles = new List<tile_base?>();
            var i = 0;
            do
            {
                if (collection != null)
                {
                    if (collection[i] != null)
                    {
                        tile_base dat = collection[i].GetComponent<simple_tile>().contents;
                        if (dat.properties.Contains(property) == true) found_tiles.Add(dat);
                    }
                    i++;
                }
            }
            while (i < collection.Count - 1);
            return found_tiles;
        }

        //find a particular tile from index, used often.
        public static tile_base? grab_tile(List<GameObject> objects, Vector2 searchspace)
        {
            GameObject current;
            var i = 0;
            do
            {
                current = objects[i];
                if (objects[i] != null)
                {
                    tile_base dat = current.GetComponent<simple_tile>().contents;
                    if (dat.positional_cord == searchspace) return current.GetComponent<simple_tile>().contents;
                }
                i++;
            }
            while (i < objects.Count - 1);
            return null;
        }
    }

    //base abstract of tilebase, contains shells of functions to be inherited.
    public abstract class tile_base
    {
        public GameObject? self;
        public bool is_player;
        public Vector2 positional_cord;
        public int puzzle_moves;
        public Coroutine? gravity_worker = null;
        public Dictionary<Vector2, tile_base?>? Neighbors;
        public HashSet<World_Utils.p_atts>? properties;
        public virtual void init(GameObject obj, HashSet<World_Utils.p_atts> properties) { }
        //apply stuff here
        public virtual Dictionary<Vector2, tile_base?>? update_neighours(List<GameObject> puzzle)
        {
            Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            return lazy_get_neighbours(puzzle, dirs);
        }

        //however shouldn't be an issue if we need to impliment batch. grabs neighbors via vector index.
        public virtual Dictionary<Vector2, tile_base?>? lazy_get_neighbours(List<GameObject> ls, params Vector2[] directions)
        {
            Neighbors = new Dictionary<Vector2, tile_base?>();
            for (var i = 0; i < directions.Length; i++)
            {
                var ourpos = this.positional_cord;
                Neighbors.Add(directions[i], World_Utils.grab_tile(ls, ourpos + directions[i]));
            }
            if (Neighbors.Count < 1) return null;

            return Neighbors;
        }
    }

    public class player_tile : tile_base
    {
        public bool move_cooldown = false;
        public Vector3 svd_pos;
        public bool is_grabbing;
        public int fossils_left_in_puz;
        public int puzzle_bone_amount;

        public IEnumerator delay(player_tile player_self)
        {
            //silly paradoxical check.
            player_self.move_cooldown = true;
            yield return new WaitForSeconds(0.25f);
            player_self.move_cooldown = false;
            yield break;
        }

        public void get_squished(List<GameObject> ls, GameObject scene_switch_obj)
        {
            int die()
            {
                GameObject a = GameObject.Find("lose_game");
                a.GetComponent<CameraShake>().enabled = true;
                World_Utils.create_soundob(4);
                Singleton.Destroy(this.self.GetComponent<TempDiggingPlayer>());
                return 0;
            }

            var tile_above = this.lazy_get_neighbours(ls, Vector2.up).First().Value;
            if (tile_above != null && tile_above.properties.Contains(World_Utils.p_atts.has_gravity) && !tile_above.properties.Contains(World_Utils.p_atts.pebble))
            {
                die();
            }

            if (self.GetComponent<TempDiggingPlayer>().num_of_moves_left < 1) die();
        }


        public void try_win_game(GameObject scene_switcher, player_tile player)
        {
            if (this.fossils_left_in_puz == 0)
            {
                Debug.Log("Game over?");
                scene_switcher.transform.ActivateChildren(true);
                player.self.GetComponent<simple_tile>().gameObject.SetActive(false);
                player.self.GetComponent<TempDiggingPlayer>().gameObject.SetActive(false);
                PlayerPrefs.SetInt("level_to_load", PlayerPrefs.GetInt("level_to_load") + 1);
                PersistentBoneUI.Instance.AppendBoneToCounter(this.puzzle_bone_amount);
            }
            else
            {
                Debug.Log("There are " + fossils_left_in_puz + "fossils left in this puzzle");
            }
        }


        public static void try_break(Vector2 inputDirection, player_tile playerself, Vector2 pos, List<GameObject> collection, GameObject scene_switcher)
        {
            tile_base? tile = World_Utils.grab_tile(collection, pos + inputDirection);
            var temp_player_data = playerself.self.GetComponent<TempDiggingPlayer>();
            if (!playerself.move_cooldown)
            {

                if (tile != null)
                {
                    if (tile.properties.Contains(World_Utils.p_atts.returnpoint))
                    {
                        playerself.try_win_game(scene_switcher, playerself);
                    }
                    else if (tile.properties.Contains(World_Utils.p_atts.creates_pebble))
                    {
                        World_Utils.pebble_surrounding(tile);
                        World_Utils.item_try_destroy(tile);
                        World_Utils.create_soundob(2);
                    }
                    if (tile.properties.Contains(World_Utils.p_atts.fullbackground))
                    {
                        //check for victory condition. still breaks spawner...
                        playerself.self.transform.position += (Vector3)inputDirection;
                        playerself.self.GetComponent<simple_tile>().contents.positional_cord += inputDirection;
                    }
                    else if (tile.properties.Contains(World_Utils.p_atts.fragile) && !tile.properties.Contains(World_Utils.p_atts.pebble))
                    {
                        World_Utils.create_soundob(1);
                        Singleton.Instance.StartCoroutine(World_Utils.recursively_break(playerself, tile, 0));
                    }
                    else if (tile.properties.Contains(World_Utils.p_atts.fossil))
                    {
                        World_Utils.item_try_destroy(tile);
                        World_Utils.create_soundob(3);
                    }
                    else if (tile.properties.Contains(World_Utils.p_atts.unbreakable))
                    {
                        inputDirection = Vector2.zero;
                        World_Utils.create_soundob(7);
                    }
                    else
                    {
                        World_Utils.item_try_destroy(tile);
                        World_Utils.create_soundob(0);
                    }
                    playerself.Neighbors = playerself.update_neighours(collection);
                    if (temp_player_data.num_of_moves_left != 0)
                    {
                        if (!(tile.properties.Contains(World_Utils.p_atts.fossil) || tile.properties.Contains(World_Utils.p_atts.fullbackground) || tile.properties.Contains(World_Utils.p_atts.unbreakable)))
                        {
                            if (temp_player_data.num_of_moves_left < 0)
                            {
                                playerself.get_squished(collection, scene_switcher);
                            }
                            else temp_player_data.num_of_moves_left -= 1;
                        }
                    }
                }
                else
                {
                    Utils.fall_with_direction(playerself.self, playerself.positional_cord + inputDirection);
                    World_Utils.create_soundob(6);
                }
                Singleton.Instance.StartCoroutine(playerself.delay(playerself));
                return;
            }
            return;
        }

        public override void init(GameObject our_game_object, HashSet<World_Utils.p_atts> special_effect)
        {
            self = our_game_object;
            properties = special_effect;
            is_grabbing = false;
            is_player = true;

        }
    }

    public class basic_material : tile_base
    {
        // public override int hit_points = 0; -> not required so we leave it as base
        public int[]? fields;
        //public basic_material[] neigbours = new basic_material[4];
        public override void init(GameObject obj, HashSet<World_Utils.p_atts> special_effects)
        {
            Neighbors = new Dictionary<Vector2, tile_base?>();
            properties = special_effects;
            self = obj;
            is_player = false;
        }
    }

}