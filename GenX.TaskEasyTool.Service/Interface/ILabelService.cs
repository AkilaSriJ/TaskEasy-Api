using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Model.DTO_S;

namespace GenX.TaskEasyTool.Service.Interface
{
    public interface ILabelService
    {
        List<Label> GetAllLabels();
        Label GetLabelById(int id);
        LabelResponseDto CreateLabel(Label label);
        void DeleteLabel(int id);
        Label GetOrCreateLabel(string labelName);
    }
}
