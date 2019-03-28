using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eigth_Puzzle_Problem {
    class UninformedSearch {
        public UninformedSearch() {
        }

        public List<Node> AStarSearch (Node root, Node GoalNode){

            List<Node> PathToSolution = new List<Node>(); //path to solution
            List<Node> OpenList = new List<Node>(); //nodes that still need to be evaluated 
            List<Node> closedList = new List<Node>(); //the nodes that have already been expanded/evaluated

            OpenList.Add(root); //openlist starts with the root node (starting node)
            bool goalFound = false;

            /* Source: A* Search pseudocode from Wikipedia */
            //while openSet is not empty
            while (OpenList.Count > 0 && !goalFound) {

                //current := the node in openSet having the lowest fScore[] value
                //find the lowest f
                int winner = 0;
                for (int i = 0; i < OpenList.Count; i++ ){
                    if (OpenList[i].f < OpenList[winner].f) {
                        winner = i;
                    }
                }
                Node currentNode = OpenList[winner];

                /*if current = goal
                 * return reconstruct_path(cameFrom, current)
                 */
                if (currentNode.GoalTest()){
                    //Console.WriteLine("Goal Found.");
                    goalFound = true;
                    //PathTrace(PathToSolution, currentNode);
                }
                else {
                    /* openSet.Remove(current)
                        closedSet.Add(current) */
                    OpenList.Remove(currentNode);
                    closedList.Add(currentNode);
                }
                //Now we need to figure out which child node to evaluate

                /* for each neighbor of current
                       if neighbor in closedSet
                            continue */
                currentNode.ExpandNode();
                //currentNode.PrintPuzzle();
                
                //for each neighbor of current
                for (int i = 0; i < currentNode.children.Count; i++){
                    Node currentChild = currentNode.children[i];
                    //if neighbor in closedSet, ignore the neighbor
                    if (!Contains(closedList, currentChild)) {
                        //increment the g score
                        int temp_g = currentChild.g + 1;

                     //check to see if the open list has this child
                     //if so, check which g score is better
                        if (Contains(OpenList, currentChild)){
                            if(temp_g < currentChild.g){
                                currentChild.g = temp_g;
                            }
                        } else {
                            currentChild.g = temp_g;
                            OpenList.Add(currentChild);
                        }
                    }

                    //We need to find the f score
                    currentChild.h = Heuristic(currentChild, GoalNode);
                    currentChild.f = currentChild.g + currentChild.h;
                    if (currentChild.GoalTest()){
                        Console.WriteLine("Goal Found.");
                        goalFound = true;
                        PathTrace(PathToSolution, currentChild);
                    }
                    //PathTrace(PathToSolution, currentChild);
                }
            }
            return PathToSolution;
        }
        
        public List<Node> BreadthFirstSearch(Node root) {
            List<Node> PathToSolution = new List<Node>(); //path to solution
            List<Node> OpenList = new List<Node>(); //keeps the list of nodes you can expand
            List<Node> closedList = new List<Node>(); //the nodes that have already been expanded

            OpenList.Add(root);
            bool goalFound = false;
            while (OpenList.Count > 0 && !goalFound){

                Node currentNode = OpenList[0];
                closedList.Add(currentNode);
                OpenList.RemoveAt(0);

                currentNode.ExpandNode();
                //currentNode.PrintPuzzle();

                for(int i = 0; i < currentNode.children.Count; i++){
                    Node currentChild = currentNode.children[i];
                    if (currentChild.GoalTest()){
                        Console.WriteLine("Goal Found.");
                        goalFound = true;
                        PathTrace(PathToSolution, currentChild);
                    }
                    if (!Contains(OpenList, currentChild) && !Contains(closedList, currentChild))
                        OpenList.Add(currentChild);
                }
            }
            return PathToSolution;
        }
        
        public void PathTrace(List<Node> path, Node n){
           // Console.WriteLine("Tracing Path...");
            Node current = n;
            path.Add(current);
            while (current.parent != null){
                current = current.parent;
                path.Add(current);
            }
        }

        //Correct Heuristic Number
        private static int Heuristic(Node currentChild, Node GoalNode){
            int number = (currentChild.x - GoalNode.x);
            int heuristic_number = Math.Abs(number);
            return heuristic_number;
        }

        public static bool Contains(List<Node> list, Node c) {
            bool contains = false;
            for (int i = 0; i < list.Count; i++) {
                if (list[i].IsSamePuzzle(c.puzzle))
                    contains = true;
            }
            return contains;
        }
    }
}
