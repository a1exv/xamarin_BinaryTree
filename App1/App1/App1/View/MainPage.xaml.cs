﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using App1.ViewModel;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new AlternativeBinaryTreeViewModel();
            //ListView.ItemsSource = ((AlternativeBinaryTreeViewModel) BindingContext).ListOfList;

        }


       
    }
}
