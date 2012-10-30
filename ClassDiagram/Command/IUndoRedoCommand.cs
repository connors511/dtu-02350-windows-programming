using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassDiagram.Command
{
    public interface IUndoRedoCommand
    {
        void Execute();
        void UnExecute();
    }
}
