using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ClassDiagram.Models.Entities;
using ClassDiagram.Models;
using ClassDiagram.Models.Arrows;

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
            this.editAssoc();
        }

        public void UnExecute()  
        {
            this._bases.Add(_base);
            this.editAssoc();
        }

        private void editAssoc()
        {
            // Remove all current arrows
            List<Base> removes = (from b in _bases
                                  where b.GetType() != typeof(Entity) // Gets all arrows
                                  select b).ToList();
            removes.ForEach(x => _bases.Remove(x));
            // Re-add all arrows
            List<Base> bs = (from b in _bases
                             where b.GetType() == typeof(Entity)
                             select b).ToList();
            // We can do this, because all arrows has been removed
            bs.ForEach(x =>
            {
                ((Entity)x).Properties.ToList().ForEach(y =>
                {
                    var z = (from b in _bases
                             where b.Name == y.Type
                             select b).ToList();
                    z.ForEach(i =>
                    {
                        _bases.Add((Base)new Association() { Start = (Entity)x, End = (Entity)i });
                    });
                });
            });
        }

    }
}
