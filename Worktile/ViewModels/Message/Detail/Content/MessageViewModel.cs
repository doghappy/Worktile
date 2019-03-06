using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common.WtRequestClient;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    abstract class MessageViewModel<S> : FileMessageViewModel<S> where S : ISession
    {
        public MessageViewModel(S session) : base(session) { }

        protected virtual string IdType { get; }

        public async virtual Task<bool> UnPinAsync(Models.Message.Message msg)
        {
            string url = $"/api/messages/{msg.Id}/unpinned?{IdType}={Session.Id}";
            var client = new WtHttpClient();
            var response = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                return true;
            }
            return false;
        }
    }
}
