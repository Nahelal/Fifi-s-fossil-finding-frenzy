using System.Collections;
using System.Collections.Generic;
using Abstract_Ammonites;
using TMPro;
using UnityEngine;

public class TempDiggingPlayer : MonoBehaviour
{
    public List<GameObject> collection;
    public GameObject layer;
    public player_tile player_self;
    [SerializeField] simple_tile comp;
    public Vector2 pos;

    public TextMeshProUGUI numOfBones;
    public TextMeshProUGUI movesLeft;

    public int num_of_moves_left;

    public GameObject scene_switch_obj;
    public Animator animator;


    Quaternion Left = Quaternion.Euler(-90, 0, 0);
    Quaternion Right = Quaternion.Euler(90, 180, 0);
    Quaternion Up = Quaternion.Euler(0, 90, -90);
    Quaternion Down = Quaternion.Euler(0, 270, 90);

    float timeCount = 0f;
    int look_direction = 0;
    bool canMove = true;
    private bool hasFossils;
    private bool has_fired;

    public int mined_fossils;


    public
    void Start()
    {
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        scene_switch_obj = GameObject.Find("win_game");

        GameObject numBones = GameObject.Find("Bone Counter");
        GameObject numMoves = GameObject.Find("Moves Left");

        numOfBones = numBones.GetComponent<TextMeshProUGUI>();
        movesLeft = numMoves.GetComponent<TextMeshProUGUI>();

        layer = GameObject.Find("Matrix");
        comp = gameObject.GetComponent<simple_tile>();
        player_self = comp.contents as player_tile;
        player_self.puzzle_bone_amount = Abstract_Ammonites.World_Utils.grab_all_tiles_of_type(collection, World_Utils.p_atts.fossil).Count;
        player_self.puzzle_moves = layer.GetComponent<matrix>().current.GetComponent<MovesNumber>().number;



    }
    //it looks epic 
    void Update()
    {


        pos = player_self.positional_cord;
        player_self.self = gameObject;
        collection = layer.GetComponent<matrix>().tile_list;

        numOfBones.text = "x" + mined_fossils.ToString() + "/" + player_self.puzzle_bone_amount.ToString();
        movesLeft.text = (num_of_moves_left - 1).ToString();

        player_self.fossils_left_in_puz = World_Utils.Tick_Tile_Collection(collection);
        player_self.get_squished(collection, scene_switch_obj);

        player_self.Neighbors = player_self.update_neighours(collection);

        activate_finish_line();
        FossilSetUp();

        mined_fossils = player_self.puzzle_bone_amount - player_self.fossils_left_in_puz;


        if (canMove)
        {

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (look_direction != 2)
                {
                    timeCount = 0;
                    look_direction = 2;
                }
                else player_tile.try_break(Vector2.up, player_self, pos, collection, scene_switch_obj);
            }

            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (look_direction != 0)
                {
                    timeCount = 0;
                    look_direction = 0;
                }
                else player_tile.try_break(Vector2.left, player_self, pos, collection, scene_switch_obj);
                animator.SetBool("isMoving", true);
            }

            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (look_direction != 3)
                {
                    timeCount = 0;
                    look_direction = 3;
                }
                else player_tile.try_break(-Vector2.up, player_self, pos, collection, scene_switch_obj);

            }

            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (look_direction != 1)
                {
                    timeCount = 0;
                    look_direction = 1;
                }
                else player_tile.try_break(-Vector2.left, player_self, pos, collection, scene_switch_obj);
            }
            animator.SetBool("isMoving", true);



            switch (look_direction)
            {
                case 0:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Left, timeCount);
                    break;

                case 1:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Right, timeCount);
                    break;

                case 2:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Up, timeCount);
                    break;

                case 3:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Down, timeCount);
                    break;
            }
            timeCount += Time.deltaTime;
        }

        if (num_of_moves_left == 0)
        {
            movesLeft.text = (num_of_moves_left).ToString();
            canMove = false;
        }

    }

    public void StopAnim()
    {
        animator.SetBool("isMoving", false);
    }
    private void activate_finish_line()
    {
        if (player_self.fossils_left_in_puz == 0)
        {
            if (has_fired == true) return;
            Abstract_Ammonites.World_Utils.grab_all_tiles_of_type(collection, World_Utils.p_atts.appear_at_end).ForEach(item => item.self.SetActive(true));
            has_fired = true;
        }

    }

    private void FossilSetUp()
    {
        if (player_self.puzzle_bone_amount == 0)
        {
            if (hasFossils == true) return;
            player_self.puzzle_bone_amount = Abstract_Ammonites.World_Utils.grab_all_tiles_of_type(collection, World_Utils.p_atts.fossil).Count;
            hasFossils = true;
        }

    }







}





