﻿using System;
using System.Linq;
using System.Reflection;

namespace Scrooge.Task
{
    public class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0 || args[0].Length == 0)
                throw new Exception("Task name is missing!");

            var taskType = Assembly.GetCallingAssembly().GetTypes()
                .FirstOrDefault(type =>
                    typeof(TaskBase).IsAssignableFrom(type)
                    && !type.IsAbstract
                    && type.GetCustomAttribute<ObsoleteAttribute>() == null
                    && type.Name == $"{args[0]}Task");

            var taskArgs = args.ToList();
            taskArgs.RemoveAt(0);

            using var task = (TaskBase)Activator.CreateInstance(taskType);
            task.Execute(taskArgs.ToArray());
        }
    }
}
