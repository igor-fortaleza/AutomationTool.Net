using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Model.Generic.Model
{
    public class ModelRecordResult<T>
    {
        public ModelRecordResult()
        {
            this._Watch = new Stopwatch();
            this._Watch.Start();
        }

        public string Content { get; set; } = string.Empty;

        public StatusAutomation StatusAutomation { get; set; }

        public T Return { get; set; } = (T)Activator.CreateInstance(typeof(T));

        public List<ModelDataAdditional> AdditionalData { get; set; } = new List<ModelDataAdditional>();

        public object Session { get; set; }

        public long TimeExecution { get; set; }

        internal Stopwatch _Watch { get; set; }
    }
}
