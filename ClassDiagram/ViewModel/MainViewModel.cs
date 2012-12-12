using ClassDiagram.Command;
using ClassDiagram.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows;
using System;
using ClassDiagram.Models.Arrows;

namespace ClassDiagram.ViewModel
{
    /// <summary>
    /// Denne ViewModel er bundet til MainWindow.
    /// </summary>
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // Holder styr på undo/redo.
        private UndoRedoController undoRedoController = UndoRedoController.GetInstance();

        // Bruges til at ændre tilstand når en kant er ved at blive tilføjet.
        private bool isAddingEntity;
        // Gemmer det første punkt som punktet har under en flytning.
        private Point moveElementPoint;
        public double ModeOpacity { get { return isAddingEntity ? 0.4 : 1.0; } }

        private Base editingElem = null;
        private IEntity movingElem = null;

		private string currentFile = "";
		private string _status = "";
		public string status
		{
			get
			{
				return _status;
			}
			set
			{
				if (_status != value)
				{
					_status = value;
					RaisePropertyChanged("status");
				}
			}
		}

		private int _intervalTime;
		public int intervalTime
		{
			get
			{
				return _intervalTime;
			}
			set
			{
				if (_intervalTime != value)
				{
					_intervalTime = value;
					RaisePropertyChanged("intervalTime");
				}
			}
		}
		private System.Windows.Forms.Timer autoSaver;

		// Formålet med at benytte en ObservableCollection er at den implementere INotifyCollectionChanged, der er forskellige fra INotifyPropertyChanged.
		// INotifyCollectionChanged smider en event når mængden af elementer i en kollektion ændres (altså når et element fjernes eller tilføjes).
		// Denne event giver GUI'en besked om ændringen.
		// Dette er en generisk kollektion. Det betyder at den kan defineres til at indeholde alle slags klasser, 
		// men den holder kun klasser af en type når den benyttes.
		public ObservableCollection<Base> bases { get; set; }
        // Helper list for layering elements correctly
        public List<Base> basesList
        {
            get
            {
                return bases.ToList().OrderBy(x => x.GetType().Name).ToList();
            }
        }

		private bool _popupOpen = false;
		public bool popupOpen
		{
			get
			{
				return _popupOpen;
			}
			set
			{
				if (_popupOpen != value)
				{
					_popupOpen = value;
				}
			}
		}

		public ICommand AddBaseCommand { get; private set; }

		public ICommand UndoCommand { get; private set; }
		public ICommand RedoCommand { get; private set; }

		// Kommandoer som UI bindes til.
		public ICommand DoneEditCommand { get; private set; }
		public ICommand NewCommand { get; private set; }
		public ICommand OpenCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand SaveAsCommand { get; private set; }
		public ICommand ExitCommand { get; private set; }
		public ICommand AboutCommand { get; private set; }

		// Kommandoer som UI bindes til.
		public ICommand MouseDoubleClickCommand { get; private set; }
		public ICommand MouseDownNodeCommand { get; private set; }
		public ICommand MouseMoveNodeCommand { get; private set; }
		public ICommand MouseUpNodeCommand { get; private set; }

		public MainViewModel()
		{
			Load();
			// Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes. Her vidersendes metode kaldne til UndoRedoControlleren.
			UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
			RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

			// Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
			AddBaseCommand = new RelayCommand<string>((s) => AddBase(s));
			DoneEditCommand = new RelayCommand(DoneEdit);
			NewCommand = new RelayCommand(New);
			SaveCommand = new RelayCommand(Save);
			SaveAsCommand = new RelayCommand(SaveAs);
			OpenCommand = new RelayCommand(Open);
			ExitCommand = new RelayCommand(Exit);
			AboutCommand = new RelayCommand(About);
			//RemoveNodeCommand = new RelayCommand<IList>(RemoveNode, CanRemoveNode);
			//AddEdgeCommand = new RelayCommand(AddEdge);
			//RemoveEdgesCommand = new RelayCommand<IList>(RemoveEdges, CanRemoveEdges);

			// Kommandoerne som UI kan kaldes bindes til de metoder der skal kaldes.
			MouseDoubleClickCommand = new RelayCommand<MouseButtonEventArgs>(MouseDoubleClick);
			MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
			MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
			MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);


			PropertyChanged += new PropertyChangedEventHandler(autoSaver_tick);
			intervalTime = 10000; // 10 seconds

		}

        private void baseChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            RaisePropertyChanged("basesList");   
            //You get notified here two times.
            if (args.OldItems != null)
                foreach (Base oldItem in args.OldItems)
                    oldItem.PropertyChanged -= new PropertyChangedEventHandler(Youritem_PropertyChanged);

            if (args.NewItems != null)
                foreach (Base newItem in args.NewItems)
                    newItem.PropertyChanged += new PropertyChangedEventHandler(Youritem_PropertyChanged);

        }

        private void Youritem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Properties")
            {
                // Remove all current arrows
                List<Base> removes = (from b in bases
                                      where b.GetType() != typeof(Entity) // Gets all arrows
                                      select b).ToList();
                removes.ForEach(x => bases.Remove(x));
                // Re-add all arrows
                List<Base> bs = (from b in bases
                                 where b.GetType() == typeof(Entity)
                                 select b).ToList();
                // We can do this, because all arrows has been removed
                bs.ForEach(x =>
                {
                    ((Entity)x).Properties.ToList().ForEach(y =>
                    {
                        var z = (from b in bases
                                 where b.Name == y.Type
                                 select b).ToList();
                        z.ForEach(i => {
                            bases.Add((Base)new Association() { Start = (Entity)x, End = (Entity)i });
                        });
                    });
                });

                RaisePropertyChanged("bases");
            }
        }

        private void autoSaver_tick(object sender, EventArgs e)
        {
            if (e.GetType() == typeof(PropertyChangedEventArgs))
            {
                if (((PropertyChangedEventArgs)e).PropertyName != "intervalTime")
                {
                    // Only modify the timer if intervalTime changed
                    return;
                }
                else
                {
                    if (autoSaver != null)
                    {
                        autoSaver.Stop();
                    }
                    autoSaver = new System.Windows.Forms.Timer();
                    autoSaver.Tick += new EventHandler(autoSaver_tick);
                    autoSaver.Interval = intervalTime;
                    autoSaver.Start();
                }
            }
            if (autoSaver == null)
            {
                autoSaver = new System.Windows.Forms.Timer();
                autoSaver.Tick += new EventHandler(autoSaver_tick);
                autoSaver.Interval = intervalTime;
                autoSaver.Start();
            }

            if (currentFile != "")
            {
                Save();
            }
            else
            {
                Console.WriteLine("Ignoring autosave");
            }
        }

        private void Load()
        {
            undoRedoController.Reset();

            bases = new ObservableCollection<Base>();
            // Needed to refresh gui
            bases.Clear();
            bases.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(baseChanged);

			/*var Props = new List<Models.Property>();
			Props.Add(new Models.Property() { Name = "PublicMethod", Visibility = Models.Visibility.Public, Type = "string" });
			Props.Add(new Models.Property() { Name = "_privateMethod", Visibility = Models.Visibility.Private, Type = "int" });
			Props.Add(new Models.Property() { Name = "protectedMethod", Visibility = Models.Visibility.Protected, Type = "Entity" });
			Props.Add(new Models.Property() { Name = "PublicMethod2", Visibility = Models.Visibility.Public });
			Props.Add(new Models.Property() { Name = "_privateMethod2", Visibility = Models.Visibility.Private });
			Props.Add(new Models.Property() { Name = "protectedMethod2", Visibility = Models.Visibility.Protected });
			var args = new List<Argument>();
			args.Add(new Argument() { Name = "pattern", Type = "string" });
			args.Add(new Argument() { Name = "subject", Type = "string", Value = "" });
			args.Add(new Argument() { Name = "matches", Type = "list<string>", Value = "null" });
			var t = new Models.Function() { Name = "test", Type = "string", Visibility = Models.Visibility.Public, Arguments = args };
            */
            //bases.Add(new Entity() { Type = eType.Class, Name = "Hej", X = 30, Y = 40, Width = 200, Height = 100, Properties = Props, Color = Brushes.LightBlue });
            //bases.Add(new Association() { Start = (Entity)bases.ElementAt(0), End = (Entity)bases.ElementAt(1) });
			//bases.Add(new Entity() { Type = eType.AbstractClass, Name = "Hello", X = 80, Y = 80, Width = 400, Height = 100, Functions = new List<Function>() { t } });

			popupOpen = false;
		}

		// Tilføjer punkt med kommando.
		public void AddBase(string parameter)
		{
			Base obj = new Entity();
            obj.Name = "New_"+parameter;
			switch (parameter)
			{
				case "AbstractClass":
					obj.Type = eType.AbstractClass;
					break;
				case "Enum":
					obj.Type = eType.Enum;
					break;
				case "Struct":
					obj.Type = eType.Struct;
					break;
				case "Class":
				default:
					obj.Type = eType.Class;
					break;
			}

            if (editingElem != null)
                editingElem.Edit = false;

            editingElem = obj;

            undoRedoController.AddAndExecute(new AddBaseCommand(bases, obj));
        }

        public void resetStatus(double time = 2.0, string s = "")
        {
            At.Do(() => status = s, DateTime.Now.AddSeconds(time));
        }

        public void setStatus(string status = "")
        {
            this.status = status;
        }

        public string getStatus()
        {
            return this.status;
        }

        #region Commands

        public void DoneEdit()
        {
            Console.WriteLine("eheh");
        }

        public void New()
        {
            // Do you want to save first?
            Console.WriteLine("Hi!");
            intervalTime = 10000;
            // Reset!
            Load();
        }

        public void Open()
        {
            setStatus("Opening file...");
            var b = new List<Base>();
            var t = currentFile;
            try
            {
                SaveLoad.Load(out b, out currentFile);
                autoSaver.Stop();
                setStatus("Loading..");
                bases.Clear();
                b.ForEach(x => bases.Add(x));
                setStatus("Loaded.");
                resetStatus(1);
                autoSaver.Start();
            }
            catch (Exception e)
            {
                currentFile = t;
                setStatus("Cancelled");
                resetStatus(1, "");
            }
        }

        public void Save()
        {
            var s = getStatus();
            setStatus("Saving...");
            var t = currentFile;
            try
            {
                currentFile = SaveLoad.Save(bases.ToList(), currentFile);
                setStatus("Saved.");
                resetStatus(1, s);
            }
            catch (Exception e)
            {
                setStatus("Cancelled");
                resetStatus(1, s);
                currentFile = t;
            }
        }

        public void SaveAs()
        {
            var s = getStatus();
            setStatus("Saving...");
            var t = currentFile;
            try
            {
                currentFile = SaveLoad.Save(bases.ToList(), "");
                setStatus("Saved.");
                resetStatus(1, s);
            }
            catch (Exception e)
            {
                setStatus("Cancelled");
                resetStatus(1, s);
                currentFile = t;
            }
        }

        public void Export()
        {

        }

        public void Exit()
        {
            // Check if there has been any changes since last save
            var m = System.Windows.Forms.MessageBox.Show("Do you wish to save first?", "Save your work?", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
            if (m == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            else if (m == System.Windows.Forms.DialogResult.Yes)
            {
                Save();
            }

            // shutdown
            System.Windows.Application.Current.Shutdown();
        }

        public void Undo()
        {

        }

        public void Redo()
        {

        }

        public void About()
        {
            // Display about box
        }
        #endregion

        public void MouseDoubleClick(MouseButtonEventArgs e)
        {
        }

        // Hvis der ikke er ved at blive tilføjet en kant så fanges musen når en musetast trykkes ned. Dette bruges til at flytte punkter.
        public void MouseDownNode(MouseButtonEventArgs e)
        {
            FrameworkElement movingEllipse = (FrameworkElement)e.MouseDevice.Target;
            Base movingNode = (Base)movingEllipse.DataContext;

            if (e.MouseDevice.RightButton == MouseButtonState.Pressed)
            {
                setStatus("Removing " + movingNode.GetType().Name);
                undoRedoController.AddAndExecute(new RemoveBaseCommand(bases, movingNode));
                resetStatus();
            }
            else
            {
                if (e.ClickCount == 2 && editingElem != movingNode)
                {
                    if (editingElem != null)
                    {
                        editingElem.Edit = false;
                    }
                    editingElem = movingNode;

                    setStatus("Editing " + movingNode.GetType().Name);
                    movingNode.Edit = true;
                }
                else if (movingNode.Edit)
                {
                    ((Entity)movingNode).Width = (int)movingEllipse.ActualWidth;
                    ((Entity)movingNode).Height = (int)movingEllipse.ActualHeight;

                    movingNode.Edit = false;
                    editingElem = null;
                    setStatus("Saved" + movingNode.GetType().Name);
                }
                else
                {
                    e.MouseDevice.Target.CaptureMouse();
                    movingElem = (IEntity)movingEllipse.DataContext;
                }
            }
        }

        // Bruges til at flytter punkter.
        public void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null)
            {

                FrameworkElement movingEllipse = (FrameworkElement)e.MouseDevice.Target;    
                setStatus("Moving " + movingElem.GetType().Name);
                Window window = FindParentOfType<Window>(movingEllipse);
                Canvas itemcanvas = (Canvas)window.FindName("ClassCanvas");
                Point mousePosition = Mouse.GetPosition(itemcanvas);
                
                if (moveElementPoint == default(Point))
                {
                    if ((mousePosition.Y - ((Entity)movingElem).CenterY) <= 0)
                    {
                        moveElementPoint.Y = (((Entity)movingElem).CenterY);
                    }
                    else if ((mousePosition.Y + ((Entity)movingElem).CenterY) >= itemcanvas.ActualHeight)
                    {
                        moveElementPoint.Y = (int)(itemcanvas.ActualHeight - (((Entity)movingElem).CenterY));
                    }
                    else
                    {
                        moveElementPoint.Y = mousePosition.Y;
                    }
                    if ((mousePosition.X - ((Entity)movingElem).CenterX) <= 0)
                    {
                        moveElementPoint.X = (((Entity)movingElem).CenterX);
                    }
                    else if ((mousePosition.X + ((Entity)movingElem).CenterX) >= itemcanvas.ActualWidth)
                    {
                        moveElementPoint.X = (int)(itemcanvas.ActualWidth - (((Entity)movingElem).CenterX));
                    }
                    else
                    {
                        moveElementPoint.X = mousePosition.X;
                    }
                }

                if ((mousePosition.Y - ((Entity)movingElem).CenterY) <= 0)
                {
                    movingElem.CanvasCenterY = (((Entity)movingElem).CenterY);
                }
                else if ((mousePosition.Y + ((Entity)movingElem).CenterY) >= itemcanvas.ActualHeight)
                {
                    movingElem.CanvasCenterY = (int)(itemcanvas.ActualHeight - (((Entity)movingElem).CenterY));
                }
                else
                {
                    movingElem.CanvasCenterY = (int)mousePosition.Y;
                }

                if ((mousePosition.X - ((Entity)movingElem).CenterX) <= 0)
                {
                    movingElem.CanvasCenterX = (((Entity)movingElem).CenterX);
                }
                else if ((mousePosition.X + ((Entity)movingElem).CenterX) >= itemcanvas.ActualWidth)
                {
                    movingElem.CanvasCenterX = (int)(itemcanvas.ActualWidth - (((Entity)movingElem).CenterX));
                }
                else
                {
                    movingElem.CanvasCenterX = (int)mousePosition.X;
                }

            }
        }

        // Benyttes til at flytte punkter og tilføje kanter.
        public void MouseUpNode(MouseButtonEventArgs e)
        {
            if (Mouse.Captured != null)
            {
                if (movingElem != null)
                {

                    // Save coordinates finally
                    undoRedoController.AddAndExecute(new MoveEntityCommand(movingElem, movingElem.CanvasCenterX, movingElem.CanvasCenterY, (int)moveElementPoint.X, (int)moveElementPoint.Y));

                    // Musen frigøres.
                    e.MouseDevice.Target.ReleaseMouseCapture();

                    // Nulstil værdier.
                    moveElementPoint = new Point();
                    movingElem = null;
                    

                }
                
                    
            }
            if(editingElem == null)
                resetStatus(1);
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