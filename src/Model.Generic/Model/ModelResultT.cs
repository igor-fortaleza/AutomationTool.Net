using System;
using System.Diagnostics;

namespace Model.Generic.Model
{
    public class ModelResult<T>
    {
        public ModelResult()
        {
            this._Watch = new Stopwatch();
            this._Watch.Start();
        }

        public long TimeExecution { get; set; }

        public bool ProcessOk { get; set; }

        public string MsgError { get; set; }

        public Exception Exception { get; set; }

        public T Return { get; set; }

        internal Stopwatch _Watch { get; set; }
    }
}
