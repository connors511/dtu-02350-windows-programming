using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ClassDiagram.Models.Entities;
using ClassDiagram.Models;
using ClassDiagram.ViewModel;

namespace ClassDiagram.Command
{
    class AddBaseCommand : IUndoRedoCommand
    {
        private ObservableCollection<Base> _bases;
        private Base _base;

        public AddBaseCommand(ObservableCollection<Base> bases, Base o)
        {
            this._bases = bases;
            this._base = o;
        }

        public void Execute()
        {
            //_base.Edit = true;
            this._bases.Add(_base);
            _bases = MainViewModel.buildAssocs(_bases);
        }

        public void UnExecute()
        {
            //_base.Edit = false;
            this._bases.Remove(_base);
            _bases = MainViewModel.buildAssocs(_bases);
        }
    }
}
