using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ClassDiagram.Models.Entities;
using ClassDiagram.Models;
using ClassDiagram.Command;

namespace ClassDiagram.Models.Entities

{
    class AddEnumCommand : IUndoRedoCommand
    {
        private ObservableCollection<Enum> _enums;
        private Enum _enum = new Enum();

        public AddEnumCommand(ObservableCollection<Enum> enums)
        {
            this._enums = enums;
        }

        public void Execute()
        {
            this._enums.Add(_enum);
        }

        public void UnExecute()
        {
            this._enums.Remove(_enum);
        }
    }
}
