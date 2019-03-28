using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eigth_Puzzle_Problem
{
    class Node  {
        public List<Node> children = new List<Node>();
        public Node parent;
        public int[] puzzle = new int[9];
        public int x = 0; //the index of 0
        public int col = 3;
        public int f = 0;
        public int g = 0;
        public int h = 0;
        
        //When the root node is created in the program.cs, the contructor gets activated 
        public Node(int[] p) {
            SetPuzzle(p);
            this.f = 0;
            this.g = 0;
            this.h = 0;
            this.x = 0;
        }

        //We put everything from the root node into our puzzle variable.
        public void SetPuzzle(int [] p) {
            for (int i = 0; i < puzzle.Length; i++)
                this.puzzle[i] = p[i];
        }

        //Find the 0 on the board (The empty slot)
        public void ExpandNode(){
            for (int index = 0; index < puzzle.Length; index++) {
                if (puzzle[index] == 0)
                    x = index;
            }

            //if its not a legal move, the 0 and the puzzle[index] will not swap
            MoveRight(puzzle, x);
            MoveLeft(puzzle, x);
            MoveUp(puzzle, x);
            MoveDown(puzzle, x);
        }
        
        public void MoveRight(int[] current_puzzle, int index) {
            //First check if it's a legal move
            if (index % col < col - 1) {
                    //create a new array that will eventually be the child node
                    int[] new_array = new int[9];

                    //Copy everything from the current puzzle array into the new_puzzle array then
                    //do the swaps
                    CopyPuzzle(new_array, current_puzzle);
                    //temp will hold the number because your index holds the zero
                    int temp = new_array[index + 1];
                    new_array[index + 1] = new_array[index];
                    new_array[index] = temp;
                    //create a new node and pass in the possible move puzzle
                    //It is now a child node
                    Node child = new Node(new_array);
                    children.Add(child);
                    child.parent = this;
                   // Console.WriteLine("Move Right.");
            }
        }
        public void MoveLeft(int[] p, int index) {
            if(index % col > 0) {
                int[] pc = new int[9];
                CopyPuzzle(pc, p);
                int temp = pc[index - 1];
                pc[index - 1] = pc[index];
                pc[index] = temp;

                Node child = new Node(pc);
                children.Add(child);
                child.parent = this;
                //Console.WriteLine("Move Left.");
            }
            
        }

        public void MoveUp(int[] p, int index) {
            if(index - col >= 0){
                int[] pc = new int[9];
                CopyPuzzle(pc, p);
                int temp = pc[index - 3];
                pc[index - 3] = pc[index];
                pc[index] = temp;

                Node child = new Node(pc);
                children.Add(child);
                child.parent = this;
                //Console.WriteLine("Move Up.");
            }
        }
        public void MoveDown(int[] p, int index) {
            if(index + col < puzzle.Length){
                int[] pc = new int[9];
                CopyPuzzle(pc, p);

                int temp = pc[index + 3];
                pc[index + 3] = pc[index];
                pc[index] = temp;

                Node child = new Node(pc);
                children.Add(child);
                child.parent = this;
                //Console.WriteLine("Move Down.");
            }
        }
        //We need to be able to copy one puzzle to another puzzle
        //copy between two puzzles
        public void CopyPuzzle(int[] pc, int [] current_puzzle) {
            for (int i = 0; i < current_puzzle.Length; i++){
                pc[i] = current_puzzle[i];
            }
        }

        public void PrintPuzzle(){
            //print 3x3 puzzle
            Console.WriteLine();
            int m = 0;
            for (int i = 0; i < col; i++) {
                for (int j = 0; j < col; j++){
                    Console.Write(puzzle[m] + " ");
                    m++;
                }
                Console.WriteLine();
            }
        }
        public bool IsSamePuzzle(int[] p){
            bool samePuzzle = true;
            for (int i = 0; i<p.Length; i++) {
                if(puzzle[i] != p[i]) {
                    samePuzzle = false;
                }
            }
            return samePuzzle;
        }

        //Check if we've reached the goal state
        public bool GoalTest() {
            bool isGoal = true;
            int m = puzzle[0]; //start with first element in puzzle

            for(int i = 1; i < puzzle.Length; i++) {
                // if puzzle[i] > puzzle[i+1]
                if (m > puzzle[i])
                    isGoal = false;
                m = puzzle[i];
            }
            return isGoal;
        }
    }
}
