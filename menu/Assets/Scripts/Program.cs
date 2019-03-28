using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eigth_Puzzle_Problem{
    public class Program{

        static void Main(string[] args){

            int[] puzzle = {1,2,4,
                            3,0,5,
                            7,6,8
            };
            
            int[] goalNode = {0, 1, 2, 3, 4, 5, 6, 7, 8};
            Node root = new Node(puzzle); //Create the root node
            Node GoalNode = new Node(goalNode); //Create the goal node

            UninformedSearch ui = new UninformedSearch();

            //List<Node> solution = ui.BreadthFirstSearch(root);

            List<Node> faster_solution = ui.AStarSearch(root, GoalNode);

            if (faster_solution.Count > 0){
                for (int i = 0; i < faster_solution.Count; i++)
                    faster_solution[i].PrintPuzzle();
            } else {
                Console.WriteLine("No Path to solution was found");
            }
            Console.Read();
        }
    }
}
