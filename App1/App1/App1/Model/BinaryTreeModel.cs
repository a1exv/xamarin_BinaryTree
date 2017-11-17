using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace App1.Model
{
    public class Node
    {


        public int Index { get; set; }
        public int Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Parent { get; set; }


        public Node(int _value)
        {
            Value = _value;
            Index = 1;
            Left = null;
            Right = null;
            Parent = null;
        }

        public override string ToString()
        {
            return String.Format(Value+"\n"+Index);
        }
    }
    public class Tree : IEnumerable
    {

        public Node root;


        #region Methods
        //добавление узла
        public void AddNode(Node node)
        {
            if (root == null)
            {
                root = node;
                root.Index = 1;
                return;
            }
            AddNodeRecursion(root, node, 1);
        }

        //рекурсивное добавление узла
        protected void AddNodeRecursion(Node root, Node node, int index)
        {
            if (node.Value > root.Value)
            {
                index = index * 10 + 1;
                if (root.Right == null)
                {
                    root.Right = node;
                    node.Parent = root;
                    root.Right.Index = index;
                }
                else AddNodeRecursion(root.Right, node, index);
            }
            else
            {
                index = index * 10;
                if (root.Left == null)
                {
                    node.Parent = root;
                    root.Left = node;
                    root.Left.Index = index;
                }
                else AddNodeRecursion(root.Left, node, index);
            }
        }

        //удаление узла по индексу и перерисовка индексов
        public void RemoveNodeWithIndex(int index, Node root)
        {
            try
            {
                if (index == 1) root = null;
                else
                {
                    int tempIndex = index;
                    List<bool> path = new List<bool>();
                    while (index > 1)
                    {
                        if (index % 10 == 1)
                            path.Add(true);
                        else path.Add(false);
                        index = index / 10;
                    }
                    for (var i = path.Count - 1; i >= 0; i--)
                    {
                        if (path[i] == false) root = root.Left;
                        else root = root.Right;
                        if (root == null) throw new Exception("Item does not exist");
                    }
                    Node tempParent = root.Parent;
                   
                    if (root.Right == null && root.Left == null)
                    {
                        if (root.Parent.Right == root) root.Parent.Right = null;
                        else root.Parent.Left = null;
                    }
                    if (root.Right == null && root.Left != null)
                    {
                        Node temp = root.Left;
                        temp.Parent = tempParent;
                        if (root.Parent.Right == root)
                        {
                            root.Parent.Right = temp;
                        }
                        else root.Parent.Left = temp;
                        RedrawIndexes(root.Left, tempIndex);
                    }
                    if (root.Right != null && root.Left == null)
                    {
                        Node temp = root.Right;
                        temp.Parent = tempParent;
                        if (root.Parent.Right == root) root.Parent.Right = root.Right;
                        else root.Parent.Left = root.Right;
                        RedrawIndexes(root.Right, tempIndex);
                    }
                    if (root.Right != null && root.Left != null)               //если у узла есть и левый и правый потомки, то на его место подставляется самый малый потомок из правого дерева. Если у самого маленького потомка правого дерева есть дочерний правый потомок - он подставляется на его место
                    {
                        Node temp = FindMin(root.Right);

                        Node tempLeft = root.Left;
                        if (temp == root.Right)
                        {

                            root = root.Right;
                            root.Parent = tempParent;
                            if (path[0] == false) tempParent.Left = root;
                            else tempParent.Right = root;
                            root.Left = tempLeft;
                            root.Left.Parent = root;

                        }
                        else if (temp.Right != null)
                        {
                            Node temptempParent = temp.Parent;

                            temp.Parent.Left = temp.Right;
                            temp.Right.Parent = temptempParent;
                            temp.Right = root.Right;
                            temp.Left = root.Left;
                            temp.Parent = tempParent;
                            if (path[0] == false) tempParent.Left = temp;
                            else tempParent.Right = temp;
                        }
                        else
                        {
                            temp.Parent.Left = null;
                            temp.Right = root.Right;
                            temp.Left = root.Left;
                            temp.Parent = tempParent;
                            if (path[0] == false) tempParent.Left = temp;
                            else tempParent.Right = temp;
                        }
                        RedrawIndexes(temp, tempIndex);

                    }

                }
            }


            catch (Exception e)
            {
               
            }
        }

        //поиск минимального значения ниже от узла
        public Node FindMin(Node root)
        {
            if (root.Left != null)
            {
                return FindMin(root.Left);
            }
            return root;
        }

        //вывод
       

        //вывод с индексами
       


        //для перерисовки индексов в нижележащих узлах после удаления узла
        public void RedrawIndexes(Node root, int rootIndex)
        {
            if (root == null) return;
            root.Index = rootIndex;
            if (root.Left != null)
            {
                RedrawIndexes(root.Left, rootIndex * 10);
            }
            if (root.Right != null)
            {
                RedrawIndexes(root.Right, rootIndex * 10 + 1);
            }
        }


        //проверка, простое число или нет
        public static bool IsSample(int num)
        {
            if (num == 1) return false;
            if (num == 2) return true;
            for (var i = 3; i <= num / 2; i++)
            {
                if (num % i == 0) return false;
            }
            return true;
        }


        #endregion




        #region Implement IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public TreeEnum GetEnumerator()
        {
            return new TreeEnum(root);
        }

        #endregion

    }



    public class TreeEnum : IEnumerator
    {
        #region Fields
        public List<Node> NodeList;
        private int _position = -1;
        #endregion



        //Заполнение коллекции узлами, которые содержат простые числа, по возрастанию
        public void FillNodeList(Node root)
        {
            if (root == null) return;
            FillNodeList(root.Left);
            if (Tree.IsSample(root.Value)) NodeList.Add(root);
            FillNodeList(root.Right);
        }


        public TreeEnum(Node root)
        {
            NodeList = new List<Node>();
            FillNodeList(root);
        }

        #region Implement IEnumerator
        public bool MoveNext()
        {
            _position++;
            return (_position < NodeList.Count);
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion

        public Node Current
        {
            get
            {
                try
                {
                    return NodeList[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
