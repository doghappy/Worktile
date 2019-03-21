using System.Threading.Tasks;
using Worktile.Common.Communication;
using Worktile.Models.Message.Session;
using Worktile.ApiModels.ApiPinnedMessages;

namespace Worktile.Services
{
    class ChannelMessageService : MessageService
    {
        public override async Task<ApiPinnedMessages> GetPinnedMessagesAsync(ISession session, string anchor)
        {
            string url = $"api/pinneds?channel_id={session.Id}&anchor={anchor}&size=10";
            return await WtHttpClient.GetAsync<ApiPinnedMessages>(url);
        }
    }
}
