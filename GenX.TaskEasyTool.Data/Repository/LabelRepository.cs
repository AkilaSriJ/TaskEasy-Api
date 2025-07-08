using GenX.TaskEasyTool.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Data.Interface;

namespace GenX.TaskEasyTool.Data.Repository
{
    public class LabelRepository:ILabelRepository
    {
        private readonly AppDbContext _context;

        public LabelRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Label> GetAll()
        {
            return _context.Labels.ToList();
        }

        public Label GetById(int id)
        {
            return _context.Labels.Find(id);
        }

        public Label Add(Label label)
        {
            _context.Labels.Add(label);
            _context.SaveChanges();
            return label;
        }
        public Label GetByName(string name)
        {
            return _context.Labels.FirstOrDefault(l => l.Name == name);
        }

        public void Delete(int id)
        {
            var label = _context.Labels.Find(id);
            if (label != null)
            {
                _context.Labels.Remove(label);
                _context.SaveChanges();
            }
        }
    }
}
