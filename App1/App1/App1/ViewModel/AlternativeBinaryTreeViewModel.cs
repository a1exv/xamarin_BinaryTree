using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System.Threading;
using App1.Model;
using App1.ViewModel;
using Xamarin.Forms;
using System.Diagnostics;
using  System.Threading;
using Windows.ApplicationModel.Store;

namespace App1.ViewModel
{
    public class ChildList:INotifyPropertyChanged     //класс списка узлов одного уровня
    {
        
        public List<Node> ChildListProp { get; set; }

        public ChildList()
        {
            ChildListProp = new List<Node>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    class AlternativeBinaryTreeViewModel:INotifyPropertyChanged
    {
        #region Fields
        private ObservableCollection<Node> _ListOfSample = new ObservableCollection<Node>();
        private double _ProgressBarLength = 0;
        private double _StepProgressBar = 1;
        #endregion

        #region Properties
        public ICommand ShowSamplesCommand { protected set; get; }
        public Tree BinaryTree { get; set; }
        public List<ChildList> ListOfList { get; set; }
        public ObservableCollection<Node> ListOfSample
        {
            get { return _ListOfSample; }
            set
            {
                if (value != _ListOfSample)
                {
                    _ListOfSample = value;
                    OnPropertyChanged(nameof(ListOfSample));
                }
            }
        }

        public double ProgressBarLength
        {
            get { return _ProgressBarLength; }
            set
            {
                if (value != _ProgressBarLength)
                {
                    _ProgressBarLength = value;
                    OnPropertyChanged(nameof(ProgressBarLength));
                }
            }
        }
        #endregion

        #region Constructors
        public AlternativeBinaryTreeViewModel()
        {
            BinaryTree = new Tree();
            BinaryTreeViewModel.FillTree(BinaryTree);
            ListOfList = FillSource(BinaryTree);
            ShowSamplesCommand = new Command(ShowSamples);
        }
#endregion



        private void ShowSamples()   //метод для команды ShowSamplesCommand
        {
            Task task = new Task(new Action(ThreadShowSamples));
            task.Start();
        }

        public async void ThreadShowSamples()
        {
            int count = 0;
            foreach (var Node in BinaryTree)
            {
                count++;
            }
            _StepProgressBar = (double)1/count;
            foreach (var node in BinaryTree)
            {
                
                Device.BeginInvokeOnMainThread(()=>ListOfSample.Add(node));
               
                await Task.Delay(500);
                Device.BeginInvokeOnMainThread(() => ProgressBarLength += _StepProgressBar);
            }
        }
        public List<ChildList> FillSource(Tree bin)           //создание листа с листами
        {
            List<ChildList> list = new List<ChildList>(10);
            WriteNode(bin.root, list);
            return list;
        }          
       

        public void WriteNode(Node root, List<ChildList> list)
        {
            if (root == null) return;
            WriteNode(root.Left, list);
            int level = BinaryTreeViewModel.GetNodeLevel(root);
            int pos = BinaryTreeViewModel.GetNodePosition(root);
            for (int i = 0; i < Math.Pow(2, level); i++)
            {
               // if(list[level]==null) list[level]=new List<Node>();
                for (int j = 0; j <= level; j++)
                {
                    if(list.Count<=j) list.Add(new ChildList());
                }
                if (i == pos)
                {
                    if(list[level].ChildListProp.Count>i) list[level].ChildListProp[i]=root;
                    else list[level].ChildListProp.Add(root);
                } 
                else if (list[level].ChildListProp.Count >= i+1) continue;
                else list[level].ChildListProp.Add(null);
            }
            WriteNode(root.Right, list);
        }     //заполнение листов узлами из дерева


        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
#endregion
    }
}
