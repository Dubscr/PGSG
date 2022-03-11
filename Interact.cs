using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Interact : MonoBehaviour
{
    [SerializeField] private Tile[] tileSet;
    [SerializeField] private Tile highlightTile;
    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap highlightMap;

    [Space(5)]

    [SerializeField] public List<int> inventory;
    [SerializeField] public float i;

    [Header("Customizables")]
    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;
    [SerializeField] private AudioSource AS;
    [SerializeField] private AudioClip place;
    [SerializeField] private AudioClip break_;
    [SerializeField] private float breakCooldown;
    [SerializeField] private Animator am;

    private bool breakAble = true;
    private Vector3Int previous;
    private Vector3Int currentCell;
    private Vector3 mouse;



    void Start()
    {

    }

    void Update()
    {
        Controls();
        Highlight();
        PlaceDestroyBlock();
    }

    void Highlight()
    {
        float distance = Vector3.Distance(mouse, transform.position);
        if (currentCell != previous && distance < maxRange)
        {
            // set the new tile
            highlightMap.SetTile(currentCell, highlightTile);

            // erase previous
            highlightMap.SetTile(previous, null);

            // save the new position for next frame
            previous = currentCell;
        } else if (distance > maxRange)
        {
            highlightMap.SetTile(currentCell, null);
            highlightMap.SetTile(previous, null);
        }
    }
    void Controls()
    {
        //Mouse Input
        mouse = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        //Get cell from Input
        currentCell = highlightMap.WorldToCell(mouse);

        //Set scroll input only when scrolling (Prevents overriding i changes)
        if (Mathf.Abs(Input.GetAxisRaw("Scroll Wheel")) > 0 && i <= tileSet.Length)
        {
            i += 10 * Input.GetAxis("Scroll Wheel");
        }
        //Revert i value to 0 if it is out of bounds
        else if (i >= tileSet.Length || i <= -1)
        {
            i = 0;
        }
    }
    void PlaceDestroyBlock()
    {
        float distance = Vector3.Distance(mouse, transform.position);
        //Place Block
        if (Input.GetButtonDown("Fire2") && distance > minRange && distance < maxRange && i < tileSet.Length && i >= -1.4)
        {
            //Remove INV I when over null block and placing
            if (mainTilemap.GetTile(currentCell) == null && inventory[(int)i] > 0)
            {
                inventory[(int)i]--;
                mainTilemap.SetTile(currentCell, tileSet[(int)i]);
                AS.clip = place;
                AS.Play();
            }
            
        }

        //Destroy Block
        if (Input.GetButton("Fire1") && breakAble && distance > minRange && distance < maxRange)
        {
            StartCoroutine(Break());
        } else if (Input.GetButtonUp("Fire1"))
        {
            breakAble = true;
        }
    }

    private IEnumerator Break()
    {
        am.SetBool("Mining", true);

        breakAble = false;
        yield return new WaitForSeconds(breakCooldown);
        if (mainTilemap.GetTile(currentCell) != null && !breakAble)
        {
            AS.clip = break_;
            AS.Play();
            if (mainTilemap.GetTile(currentCell).name == "Grass")
            {
                inventory[0]++;
                mainTilemap.SetTile(currentCell, null);
            }
            else if (mainTilemap.GetTile(currentCell).name == "Dirt")
            {
                inventory[1]++;
                mainTilemap.SetTile(currentCell, null);
            }
            else if (mainTilemap.GetTile(currentCell).name == "Stone")
            {
                inventory[2]++;
                mainTilemap.SetTile(currentCell, null);
            }
        }

        am.SetBool("Mining", false);
        breakAble = true;
    }
}
