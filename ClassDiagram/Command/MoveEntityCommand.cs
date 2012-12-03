using ClassDiagram.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram.Command
{
    // Bruges til at flytte punkt.
    public class MoveEntityCommand : IUndoRedoCommand
    {
        private IEntity node;
        private int x;
        private int y;
        private int newX;
        private int newY;

        public MoveEntityCommand(IEntity _node, int _newX, int _newY, int _x, int _y)
        { 
            node = _node; 
            newX = _newX; 
            newY = _newY; 
            x = _x; 
            y = _y; 
        }

        public void Execute()
        {
            node.CanvasCenterX = newX;
            node.CanvasCenterY = newY;
        }

        public void UnExecute()
        {
            node.CanvasCenterX = x;
            node.CanvasCenterY = y;
        }
    }
}