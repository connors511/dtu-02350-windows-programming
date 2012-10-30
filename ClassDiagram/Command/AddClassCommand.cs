using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ClassDiagram.Models.Entities;
using ClassDiagram.Models;

namespace ClassDiagram.Command
{
    class AddClassCommand : IUndoRedoCommand
    {
        private ObservableCollection<Class> _classes;
        private Class _class = new Class();

        public AddClassCommand(ObservableCollection<Class> classes)
        {
            this._classes = classes;
        }

        public void Execute()
        {
            this._classes.Add(_class);
        }

        public void UnExecute()
        {
            this._classes.Remove(_class);
        }
    }
}
