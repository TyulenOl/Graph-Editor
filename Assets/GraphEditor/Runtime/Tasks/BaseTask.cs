﻿using System.Linq;
using System.Reflection;
using GraphEditor.Attributes;

namespace GraphEditor.Runtime
{
    public abstract class BaseTask<T> : BaseTask
        where T : BaseTask<T>
    {
        private static readonly PropertyInfo[] properties;

        static BaseTask()
        {
            properties = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(property => property.GetCustomAttribute<DisplayNameAttribute>() != null)
                .ToArray();
        }

        protected BaseTask(TaskInfo taskInfo) : base(taskInfo)
        {
        }
        
        public override string GetDescription()
        {
            var stringProperties = properties.Select(property =>
                $"{property.GetCustomAttribute<DisplayNameAttribute>().DisplayName} - {property.GetValue(this)}");
            return string.Join("\n", stringProperties);
        }
    }

    public abstract class BaseTask : ITask
    {
        public TaskInfo TaskInfo { get; }

        protected BaseTask(TaskInfo taskInfo)
        {
            TaskInfo = taskInfo;
        }
        
        public abstract string GetDescription();

        public abstract bool CheckTask(MonoGraph monoGraph);
    }
}