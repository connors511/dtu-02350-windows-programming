using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ClassDiagram.Models.Entities;
using ClassDiagram.Models;
using ClassDiagram.Models.Arrows;
using ClassDiagram.ViewModel;

namespace ClassDiagram.Command
{
    class RemoveBaseCommand : IUndoRedoCommand
    {
        private ObservableCollection<Base> _bases;
        private Base _base;

        public RemoveBaseCommand(ObservableCollection<Base> bases, Base o)
        {
            this._bases = bases;
            this._base = o;
        }

        public void Execute()
        {
            this._bases.Remove(_base);
            _bases = MainViewModel.buildAssocs(_bases);
        }

        public void UnExecute()  
        {
            this._bases.Add(_base);
            _bases = MainViewModel.buildAssocs(_bases);
        }

    }
}
