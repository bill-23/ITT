using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    public Texture2D image;
    public int blocksPerLine = 3;
    Block emptyBlock;
    public GamePlay gamePlay;

    private List<Block> blockList = new List<Block>();
    private List<string> numberString = new List<string>();
    int count = 0;
    public Text suggestedMoveText;
    public Text ratingText;


    void Start()
    {
        numberString.Add("0");
        numberString.Add("1");
        numberString.Add("2");
        numberString.Add("3");
        numberString.Add("4");
        numberString.Add("5");
        numberString.Add("6");
        numberString.Add("7");
        numberString.Add("8");

        CreatePuzzle();
    }

    void CreatePuzzle()
    {
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine);
        for (int y = 0; y < blocksPerLine; y++)
        {
            for (int x = 0; x < blocksPerLine; x++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                blockObject.transform.position = new Vector2(x, y);
                blockObject.transform.parent = transform;

                Block block = blockObject.AddComponent<Block>();
                
                //Added mesh collider to each block object so we can interact with it
                MeshCollider block2 = blockObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
                block2.convex = true;
                block2.isTrigger = true;

                //When a block is pressed go here
                block.OnBlockPressed += PlayerMoveBlockInput;

                block.Init(new Vector2Int(x, y), imageSlices[x, y]);

                blockList.Add(block);

                if (y == 0 && x == blocksPerLine - 1)
                {
                    blockObject.SetActive(false);
                    emptyBlock = block;
                }
            }
            count++;
        }
    }

    public void PlayerMoveBlockInput(Block blockToMove)
    {
        
        Debug.Log(blockList.IndexOf(blockToMove));

        gamePlay.setBlockToMove(blockList.IndexOf(blockToMove));

        gamePlay.clearButtonColor();
        suggestedMoveText.text = "Suggest a move:";
        ratingText.text = "Select a rating:";


        if ((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
        {
            Vector2Int targetCoord = emptyBlock.coord;
            emptyBlock.coord = blockToMove.coord;
            blockToMove.coord = targetCoord;

            Vector2 targetPosition = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.transform.position = targetPosition;
        }
    }

    public void makeOtherPlayersMove(int index)
    {
        PlayerMoveBlockInput(blockList[index]);
    }

}
