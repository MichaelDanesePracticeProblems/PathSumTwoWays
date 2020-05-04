using System;
using System.Collections.Generic;
using System.IO;

namespace PathSumTwoWays
{
    class Program
    {
        //My attempt at the Path Sum: Two  WAys problem from the Euler Project. https://projecteuler.net/problem=81
        //"In a 5 by 5 matrix, the minimal path sum from the top left to the bottom right, by only moving to the right and down, is indicated in bold red and is equal to 2427."
        //"Find the minimal path sum from the top left to the bottom right by only moving right and down in matrix.txt, a 31K text file containing an 80 by 80 matrix."
        //Michael Danese
        static void Main(string[] args)
        {//This section is mostly dedicated to just parsing the values from the txt file given. 
            bool fileFound = false;
            string input = "";
            try{
                input = System.IO.File.ReadAllText(@"..\..\..\p081_matrix.txt");//Hard coded in the path.
                fileFound = true;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found!");
            }
            if (fileFound)
            {
                ParseInput(input);
            } 
        }

        public static void ParseInput(string input)
        {//Takes the input from the txt file an converts it into an array of ints. I have not yet done a lot of exception avoidance but may come back to do so.
            string[] formattedInput = input.Replace("\n",",").Split(',');
            Vertex[,] matrix = new Vertex[80, 80];//At the moment it is not so scalable with other problems. May come back in the future and make it mroe scalable and catch more errors.
            List<Vertex> vertexList = new List<Vertex>();
            Vertex currentVertex;
            VertexGraph graph;
            int currentIndex = 0;
            for (int i = 0; i < 80;i++)
            {
                for (int j = 0; j < 80;j++)
                {
                    matrix[i, j] = new Vertex(Int32.Parse(formattedInput[currentIndex]));
                    currentVertex = matrix[i, j];
                    if ((i - 1) > -1)
                    {
                        matrix[(i - 1), j].downVertex = currentVertex;
                    }
                    if ((j - 1) > -1)
                    {
                        matrix[i, (j - 1)].rightVertex = currentVertex;
                    }
                    vertexList.Add(currentVertex);
                    currentIndex++;
                }
            }
            graph = new VertexGraph(matrix[0,0], vertexList);//This calls the actual program, a graph of the entire matrix. Decided to utilize a little more memory to speed things along.

        }
    }
    //----------------------------------------------------------------------------------------------------------------
    class Vertex
    {
        private int value;
        public int distance = 2147483647;
        public bool rightMarked = false, downMarked = false, vertexVisited = false;
        public Vertex rightVertex, downVertex, preceedingVertex;
        
        public Vertex(int val)
        {
            value = val;
        }
        public int GetValue()
        {
            return value;
        }
    }

    class VertexGraph
    {
        Vertex StartVertex;
        Vertex LastVertex;
        List<Vertex> vertexList;
        public int shortestDistance;
        public VertexGraph(Vertex input, List<Vertex> list)
        {//This graph utilizes a sudo Dijkstra's shortest path algorithm. However the algorith can be simplified as we know there will always be two edges and what they will link to.
         //Also the values of the edges are just based on the values of the nodes vertices themselves
            StartVertex = input;
            StartVertex.distance = StartVertex.GetValue();
            vertexList = list;
            LastVertex = vertexList[vertexList.Count - 1];
            SetShortestPath();
            shortestDistance = LastVertex.distance;
        }

        public void SetShortestPath()
        {//Will build a graph that represents the matrix given.
            Vertex CurrentVertex = StartVertex;
            bool finished = false;
            Stack<Vertex> toDo = new Stack<Vertex>();
            Vertex currentRight, currentDown;
            int rightSum, downSum, downVal, rightVal;
            toDo.Push(CurrentVertex);
            while (!finished)
            {
                if (CurrentVertex.rightVertex != null)
                {
                    currentRight = CurrentVertex.rightVertex;
                    rightVal = currentRight.GetValue();
                    rightSum = CurrentVertex.distance + currentRight.GetValue();
                    if (rightSum < currentRight.distance)
                    {
                        currentRight.distance = rightSum;
                        currentRight.preceedingVertex = CurrentVertex;
                        CurrentVertex.rightMarked = true;
                    }
                }
                if (CurrentVertex.downVertex != null)
                {
                    currentDown = CurrentVertex.downVertex;
                    downSum = CurrentVertex.distance + currentDown.GetValue();
                    downVal = currentDown.GetValue();
                    if (downSum < currentDown.distance)
                    {
                        currentDown.distance = downSum;
                        currentDown.preceedingVertex = CurrentVertex;
                        CurrentVertex.downMarked = true;
                    }
                }
                CurrentVertex.vertexVisited = true;
                CurrentVertex = GetShortestVertex();//Will return null of there are no more vertices to find a shorter distance for.
                
                if (CurrentVertex == null)
                {//Will end because there are no more nodes to find the shortest path for
                    finished = true;
                }
            }
        }

        private Vertex GetShortestVertex()
        {//There can be some improvement with this as I should remove the completed vertices from the list to make the search shorter.
            Vertex smallest = null;
            foreach (Vertex element in vertexList)
            {
                if (smallest == null && element.vertexVisited == false)
                {
                    smallest = element;
                }
                else if (element.vertexVisited == false && element.distance < smallest.distance)
                {
                    smallest = element;
                }
            }
            return smallest;
        }
    }
}
