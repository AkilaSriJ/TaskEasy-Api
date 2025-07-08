using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Models;

namespace GenX.TaskEasyTool.Data.Interface
{
    public interface ILabelRepository
    {
        List<Label> GetAll();
        Label GetById(int id);
        Label Add(Label label);
        Label GetByName(string name);

        void Delete(int id);
    }
}
