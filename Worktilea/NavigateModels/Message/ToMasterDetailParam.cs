namespace Worktile.NavigateModels.Message
{
    class ToMasterDetailParam
    {
        public Action Action { get; set; }
        public object Parameter { get; set; }
    }

    enum Action
    {
        NewSession
    }
}
