namespace Worktile.WindowsUI.Models.Results
{
    public class DataResult<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
    }
}
