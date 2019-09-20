using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Models
{
    public class ActionField
    {
        public ActionField(int height, int width)
        {
            Height = height;
            Width = width;
            FieldNodes = new NodeType[height, width];

            InitializeFieldRandomly();
        }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public NodeType[,] FieldNodes { get; private set; }

        private void InitializeFieldRandomly()
        {
            var random = new Random();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    FieldNodes[i, j] =
                        random.Next(4) == 0 ? NodeType.Rock : NodeType.Gross;
                }
            }

            PutObjectsOnSomeNodes(NodeType.Star, 1);
            PutObjectsOnSomeNodes(NodeType.Cookie, 3);
            PutObjectsOnSomeNodes(NodeType.Agent, 1);
        }

        Random r = new Random();

        private void PutObjectsOnSomeNodes(NodeType objectType, int objectsLeft)
        {
            if (objectsLeft == 0)
            {
                return;
            }
            
            int randomX = r.Next(Width);
            int randomY = r.Next(Height);
            if (FieldNodes[randomX, randomY] == NodeType.Gross)
            {
                FieldNodes[randomX, randomY] = objectType;
                PutObjectsOnSomeNodes(objectType, objectsLeft - 1);
            }
            else
            {
                PutObjectsOnSomeNodes(objectType, objectsLeft);
            }
        }
    }
}