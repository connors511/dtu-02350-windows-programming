﻿using ClassDiagram.Command;
using ClassDiagram.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ClassDiagram.Models.Arrows;
using ClassDiagram.Models.Entities;
using System;

namespace ClassDiagram.ViewModel
{
    /// <summary>
    /// Denne ViewModel er bundet til MainWindow.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // Holder styr på undo/redo.
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        // Bruges til at ændre tilstand når en kant er ved at blive tilføjet.
        private bool isAddingEntity;
        // Gemmer det første punkt som punktet har under en flytning.
        private Point moveElementPoint;
        public double ModeOpacity { get { return isAddingEntity ? 0.4 : 1.0; } }

        // Formålet med at benytte en ObservableCollection er at den implementere INotifyCollectionChanged, der er forskellige fra INotifyPropertyChanged.
        // INotifyCollectionChanged smider en event når mængden af elementer i en kollektion ændres (altså når et element fjernes eller tilføjes).
        // Denne event giver GUI'en besked om ændringen.
        // Dette er en generisk kollektion. Det betyder at den kan defineres til at indeholde alle slags klasser, 
        // men den holder kun klasser af en type når den benyttes.
        public ObservableCollection<Base> bases { get; set; }

        // Kommandoer som UI bindes til.
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand AddBaseCommand { get; private set; }
        public ICommand RemoveNodeCommand { get; private set; }
        public ICommand AddEdgeCommand { get; private set; }
        public ICommand RemoveEdgesCommand { get; private set; }

        // Kommandoer som UI bindes til.
        public ICommand MouseDownNodeCommand { get; private set; }
        public ICommand MouseMoveNodeCommand { get; private set; }
        public ICommand MouseUpNodeCommand { get; private set; }

        public MainViewModel()
        {
            // Her fyldes listen af noder med to noder. Her benyttes et alternativ til konstruktorer med syntaksen 'new Type(){ Attribut = Værdi }'
            // Det der sker er at der først laves et nyt object og så sættes objektets attributer til de givne værdier.
            bases = new ObservableCollection<Base>() { 
                new Class() { X = 30, Y = 40, Width = 80, Height = 80 }, 
                new Class() { X = 140, Y = 230, Width = 100, Height = 100 } 
            };

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes. Her vidersendes metode kaldne til UndoRedoControlleren.
            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            AddBaseCommand = new RelayCommand<string>((s) => AddClass(s));
            //RemoveNodeCommand = new RelayCommand<IList>(RemoveNode, CanRemoveNode);
            //AddEdgeCommand = new RelayCommand(AddEdge);
            //RemoveEdgesCommand = new RelayCommand<IList>(RemoveEdges, CanRemoveEdges);

            // Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
        }

        // Tilføjer punkt med kommando.
        public void AddClass(string parameter)
        {

            Base obj;
            switch (parameter)
            {
                case "Class" :
                    obj = new Class();
                    break;
                default :
                    obj = new Class();
                    break;
            }

            undoRedoController.AddAndExecute(new AddBaseCommand(bases, obj));
        }

        // Tjekker om valgte punkt/er kan fjernes. Det kan de hvis der er nogle der er valgt.
        /*public bool CanRemoveNode(IList _nodes)
        {
            return _nodes.Count == 1;
        }

        // Fjerner valgte punkter med kommando.
        public void RemoveNode(IList _nodes)
        {
            undoRedoController.AddAndExecute(new RemoveNodesCommand(Nodes, Edges, _nodes.Cast<Node>().First()));
        }

        // Starter proceduren der tilføjer en kant.
        public void AddEdge()
        {
            isAddingElement = true;
        }*/

        // Hvis der ikke er ved at blive tilføjet en kant så fanges musen når en musetast trykkes ned. Dette bruges til at flytte punkter.
        public void MouseDownNode(MouseButtonEventArgs e)
        {
            if (!isAddingEntity) e.MouseDevice.Target.CaptureMouse();
        }

        // Bruges til at flytter punkter.
        public void MouseMoveNode(MouseEventArgs e)
        {
            // Tjek at musen er fanget og at der ikke er ved at blive tilføjet en kant.
            if (Mouse.Captured != null && !isAddingEntity)
            {
                // Musen er nu fanget af ellipserne og den ellipse som musen befinder sig over skaffes her.
                FrameworkElement movingEllipse = (FrameworkElement)e.MouseDevice.Target;
                // Fra ellipsen skaffes punktet som den er bundet til.
                IEntity movingNode = (IEntity)movingEllipse.DataContext;
                // Canvaset findes her udfra ellipsen.
                Canvas canvas = FindParentOfType<Canvas>(movingEllipse);
                // Musens position i forhold til canvas skaffes her.
                Point mousePosition = Mouse.GetPosition(canvas);
                // Når man flytter noget med musen vil denne metode blive kaldt mange gange for hvert lille ryk, 
                // derfor gemmes her positionen før det første ryk så den sammen med den sidste position kan benyttes til at flytte punktet med en kommando.
                if (moveElementPoint == default(Point)) moveElementPoint = mousePosition;
                // Punktets position ændres og beskeden bliver så sendt til UI med INotifyPropertyChanged mønsteret.
                movingNode.CanvasCenterX = (int)mousePosition.X;
                movingNode.CanvasCenterY = (int)mousePosition.Y;
            }
        }

        // Benyttes til at flytte punkter og tilføje kanter.
        public void MouseUpNode(MouseButtonEventArgs e)
        {

            // Ellipsen skaffes.
            FrameworkElement movingEllipse = (FrameworkElement)e.MouseDevice.Target;
            // Ellipsens node skaffes.
            IEntity movingEntity = (IEntity)movingEllipse.DataContext;
            // Canvaset skaffes.
            Canvas canvas = FindParentOfType<Canvas>(movingEllipse);
            // Musens position på canvas skaffes.
            Point mousePosition = Mouse.GetPosition(canvas);
            // Punktet flyttes med kommando. Den flyttes egentlig bare det sidste stykke i en række af mange men da de originale punkt gemmes er der ikke noget problem med undo/redo.
            undoRedoController.AddAndExecute(new MoveEntityCommand(movingEntity, (int)mousePosition.X, (int)mousePosition.Y, (int)moveElementPoint.X, (int)moveElementPoint.Y));
            // Nulstil værdier.
            moveElementPoint = new Point();
            // Musen frigøres.
            e.MouseDevice.Target.ReleaseMouseCapture();

        }

        // Rekursiv metode der benyttes til at finde et af et grafisk elements forfædre ved hjælp af typen, der ledes højere og højere op indtil en af typen findes.
        // Syntaksen "() ? () : ()" betyder hvis den første del bliver sand så skal værdien være den anden del, ellers skal den være den tredje del.
        private static T FindParentOfType<T>(DependencyObject o) where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(o);
            if (parent == null) return null;
            return parent is T ? parent as T : FindParentOfType<T>(parent);
        }
    }
}