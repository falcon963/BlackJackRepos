using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;
using TestProject.Core.Repositories.Interfaces;
using TestProject.Core.Services.Interfaces;

namespace TestProject.Core.Services
{
    public class TaskService
        : ITaskService
    {
        private ITasksRepository _tasksRepository;

        public TaskService(ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }

        public int SaveTask(UserTask task)
        {
            int id;

            if (task.Id == 0)
            {
                id = _tasksRepository.Save(task);

                return id;
            }

            id = _tasksRepository.Update(task);

            return id;
        }
    }
}
