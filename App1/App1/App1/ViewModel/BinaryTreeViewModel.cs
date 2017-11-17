using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using App1.Model;
using Xamarin.Forms;


namespace App1.ViewModel
{
    class BinaryTreeViewModel:INotifyPropertyChanged
    {
        public const int HEIGHT_OF_LEVEL = 100;

        public double InitialWidth
        {
            get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if(handler!=null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private Tree _binaryTree;
        public Tree BinaryTree
        {
            get { return _binaryTree; }
            private set { }
        }

        public BinaryTreeViewModel()
        {
            _binaryTree=new Tree();
            FillTree(_binaryTree);
        }
        
        public AbsoluteLayout SetAbsLay(AbsoluteLayout _absLay)
        {
            InitialWidth = _absLay.Width;
            return DrawTree(_binaryTree, _absLay);
        }
       
        

        public AbsoluteLayout DrawTree(Tree BinaryTree, AbsoluteLayout AbsLay)
        {

            AddNodeToLayout(BinaryTree.root, AbsLay);
            return AbsLay;
        }

        public void AddNodeToLayout(Node root, AbsoluteLayout AbsLay)
        {
            if (root == null) return;
            AddNodeToLayout(root.Left, AbsLay);
            int level = GetNodeLevel(root);
            Label temp = new Label();
            temp.BackgroundColor = Color.White;
            temp.Text = String.Format(root.Value.ToString() + "\n" + root.Index.ToString());
            Rectangle tempRect = new Rectangle(InitialWidth/Math.Pow(2, level)*GetNodePosition(root), level*HEIGHT_OF_LEVEL, (InitialWidth / Math.Pow(2, level))-5, 90 );
            temp.TextColor=Color.Black;
            temp.Font=Font.Default;
            AbsLay.Children.Add(temp, tempRect);
            AddNodeToLayout(root.Right, AbsLay);

        }

        public static int GetNodeLevel(Node root)
        {
            int count = 0;
            int tempIndex = root.Index;
            while (tempIndex > 1)
            {
                count++;
                tempIndex = tempIndex/10;
            }
            return count;
            
        }

        public static int GetNodePosition(Node root)
        {
            if (root.Parent == null) return 0;
            else
            {
                int tempIndex = root.Index;

                List<bool> path = new List<bool>();
                while (tempIndex > 0)
                {
                    if (tempIndex%10 == 1)
                        path.Add(true);
                    else path.Add(false);
                    tempIndex = tempIndex/10;
                }
                int pos = 0;
                int cof = 1;
                for (int i = 0; i < path.Count-1; i++)
                {
                    if (path[i] == true) pos += cof;
                    cof *= 2;

                }
                return pos;
            }
        }

        public static void FillTree(Tree BinaryTree)
        {
            Random rand=new Random();
            for (int i = 0; i < 20; i++)
            {
                BinaryTree.AddNode(new Node(rand.Next(0, 100)));
            }
           BinaryTree.AddNode(new Node(20));
            BinaryTree.AddNode(new Node(10));
            BinaryTree.AddNode(new Node(45));
            BinaryTree.AddNode(new Node(7));
         //   BinaryTree.AddNode(new Node(15));
            BinaryTree.AddNode(new Node(30));
            BinaryTree.AddNode(new Node(60));
            //BinaryTree.AddNode(new Node(8));
            BinaryTree.AddNode(new Node(40));
        }
    }
}
