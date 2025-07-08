using GenX.TaskEasyTool.Data.Interface;
using GenX.TaskEasyTool.Data.Models;
using GenX.TaskEasyTool.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Model.DTO_S;

namespace GenX.TaskEasyTool.Service.Service
{
    public class LabelService:ILabelService
    {
        private readonly ILabelRepository _labelRepository;

        public LabelService(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public List<Label> GetAllLabels()
        {
            return _labelRepository.GetAll();
        }

        public Label GetLabelById(int id)
        {
            return _labelRepository.GetById(id);
        }

        public LabelResponseDto CreateLabel(Label label)
        {
            var existingLabel = _labelRepository.GetByName(label.Name);

            if (existingLabel == null)
            {
                var created = _labelRepository.Add(label);
                return new LabelResponseDto
                {
                    Id = created.Id,
                    Name = created.Name
                };
            }
            else
            {
                return new LabelResponseDto
                {
                    Id = existingLabel.Id,
                    Name = existingLabel.Name
                };
            }
        }

        public void DeleteLabel(int id)
        {
            _labelRepository.Delete(id);
        }
        public Label GetOrCreateLabel(string labelName)
        {
            var existing = _labelRepository.GetByName(labelName);

            if (existing != null)
                return existing;

            try
            {
                return _labelRepository.Add(new Label { Name = labelName });
            }
            catch (DbUpdateException)
            {
                return _labelRepository.GetByName(labelName);
            }
        }
    }
}
