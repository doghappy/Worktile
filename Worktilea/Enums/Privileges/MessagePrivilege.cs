namespace Worktile.Enums.Privileges
{
    enum MessagePrivilege
    {
        CreatePublicChannel = 1,
        CreatePrivateChannel = 2,
        AdminPublicChannel = 4,
        SendMessageInTeamChannel = 8,
        RemoveMessageInChannel = 16,
        UpdateModuleSetting = 32,
    }
}
