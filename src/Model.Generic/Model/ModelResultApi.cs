namespace Model.Generic.Model
{
    public class ModelResultApi
    {
        public bool ProcessOk { get; set; }

        public string MsgErro { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        public object Result { get; set; } = new object();
    }
}
