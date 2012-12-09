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
		private string _status;
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
			intervalTime = 5000; // 5 seconds
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

			var Props = new ObservableCollection<Models.Property>();
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

            bases.Add(new Entity() { Type = eType.Class, Name = "Hej", X = 30, Y = 40, Width = 200, Height = 100, Properties = Props, Color = Brushes.LightBlue });
			bases.Add(new Entity() { Type = eType.AbstractClass, Name = "Hello", X = 480, Y = 280, Width = 400, Height = 100, Functions = new ObservableCollection<Function>() { t } });

			popupOpen = false;
		}

		// Tilføjer punkt med kommando.
		public void AddBase(string parameter)
		{
			Base obj = new Entity();
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
		}*/

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
			SaveLoad.Load(out b, out currentFile);
			setStatus("Loading..");
			bases.Clear();
			b.ForEach(x => bases.Add(x));
			setStatus("Loaded.");
			resetStatus();
		}

		public void Save()
		{
			var s = getStatus();
			setStatus("Saving...");
			currentFile = SaveLoad.Save(bases.ToList(), currentFile);
			setStatus("Saved.");
			resetStatus(1, s);
		}

		public void SaveAs()
		{
			setStatus("Saving...");
			currentFile = SaveLoad.Save(bases.ToList(), "");
			setStatus("Saved.");
			resetStatus();
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
			if (e.MouseDevice.RightButton == MouseButtonState.Pressed)
			{
				// Ellipsen skaffes.
				FrameworkElement movingEllipse = (FrameworkElement)e.MouseDevice.Target;
				// Ellipsens node skaffes.
				Base movingEntity = (Base)movingEllipse.DataContext;
				setStatus("Removing " + movingEntity.GetType().Name);

				undoRedoController.AddAndExecute(new RemoveBaseCommand(bases, movingEntity));
				resetStatus();
			}
			else
			{
				
				// Lock target and get current element
				e.MouseDevice.Target.CaptureMouse();
				FrameworkElement movingEllipse = (FrameworkElement)e.MouseDevice.Target;
				Base movingNode = (Base)movingEllipse.DataContext;
				if(movingNode.Edit) {

                    ((Entity)movingNode).Width = (int)movingEllipse.ActualWidth;
                    ((Entity)movingNode).Height = (int)movingEllipse.ActualHeight;

                    

					movingNode.Edit = false;
					editingElem = null;
					setStatus("Saved");
					e.MouseDevice.Target.ReleaseMouseCapture();
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


					e.MouseDevice.Target.ReleaseMouseCapture();
					setStatus("Editing " + movingNode.GetType().Name);
					movingNode.Edit = true;
				}
				}   

				}

		}

		// Bruges til at flytter punkter.
		public void MouseMoveNode(MouseEventArgs e)
		{
			// Tjek at musen er fanget og at der ikke er ved at blive tilføjet en kant.
			if (Mouse.Captured != null)
			{

				// Musen er nu fanget af ellipserne og den ellipse som musen befinder sig over skaffes her.
				FrameworkElement movingEllipse = (FrameworkElement)e.MouseDevice.Target;
				// Fra ellipsen skaffes punktet som den er bundet til.
				movingElem = (IEntity)movingEllipse.DataContext;
				setStatus("Moving " + movingElem.GetType().Name);
				// Canvaset findes her udfra ellipsen.
				Canvas canvas = FindParentOfType<Canvas>(movingEllipse);
				// Musens position i forhold til canvas skaffes her.
				Point mousePosition = Mouse.GetPosition(canvas);
				// Når man flytter noget med musen vil denne metode blive kaldt mange gange for hvert lille ryk, 
				// derfor gemmes her positionen før det første ryk så den sammen med den sidste position kan benyttes til at flytte punktet med en kommando.
				if (moveElementPoint == default(Point))
                {
                    if ((mousePosition.Y - ((Entity)movingElem).Height / 2) <= 0)
                    {
                        moveElementPoint.Y = (((Entity)movingElem).Height / 2);
                    }
                    else
                    {
                        moveElementPoint.Y = mousePosition.Y;
                    }
                    if ((mousePosition.X - ((Entity)movingElem).Width / 2) <= 0)
                    {
                        moveElementPoint.X = (((Entity)movingElem).Width / 2);
                    }
                    else
                    {
                        moveElementPoint.X = mousePosition.X;
                    }
                }
                    
                    

                if ((mousePosition.Y - ((Entity)movingElem).Height / 2) <= 0)
                {
                    movingElem.CanvasCenterY = (((Entity)movingElem).Height / 2);
                }
                //else if ((mousePosition.Y + ((Entity)movingNode).Height / 2) >= canvas.ActualHeight)
                //{
                //    movingNode.CanvasCenterY = (int)canvas.ActualHeight - (((Entity)movingNode).Height / 2);
                //}
                else
                {
                    movingElem.CanvasCenterY = (int)mousePosition.Y;
                }

                if ((mousePosition.X - ((Entity)movingElem).Width / 2) <= 0)
                {
                    movingElem.CanvasCenterX = (((Entity)movingElem).Width / 2);
                }
                //else if ((mousePosition.Y + ((Entity)movingNode).Height / 2) >= canvas.ActualHeight)
                //{
                //    movingNode.CanvasCenterY = (int)canvas.ActualHeight - (((Entity)movingNode).Height / 2);
                //}
                else
                {
                    movingElem.CanvasCenterX = (int)mousePosition.X;
                }

			}
		}

		// Benyttes til at flytte punkter og tilføje kanter.
		public void MouseUpNode(MouseButtonEventArgs e)
		{
			if (Mouse.Captured != null && movingElem != null)
			{

				// Save coordinates finally
                undoRedoController.AddAndExecute(new MoveEntityCommand(movingElem, movingElem.CanvasCenterX, movingElem.CanvasCenterY, (int)moveElementPoint.X, (int)moveElementPoint.Y));
				
				// Musen frigøres.
				e.MouseDevice.Target.ReleaseMouseCapture();
				if (!((Base)movingElem).Edit)
				{
					// Only reset if youre we did not enter edit mode
					resetStatus(0.5);
				}
                
                // Nulstil værdier.
				moveElementPoint = new Point();
                movingElem = null;

			}
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