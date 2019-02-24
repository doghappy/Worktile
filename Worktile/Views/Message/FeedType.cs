﻿namespace Worktile.Views.Message
{
    public enum FeedType
    {
        NewTeamMember = 1,
        RemoveTeamMember = 2,
        UpdateTeamInfo = 3,
        RemoveTeam = 4,
        AddService = 5,
        UpdateService = 6,
        NewChannelMember = 7,
        RemoveChannelMember = 8,
        RemoveChannel = 9,
        UpdateChannelInfo = 10,
        ArchiveChannel = 11,
        ActiveChannel = 12,
        AddFile = 13,
        ChangeFile = 14,
        RemoveFile = 15,
        // addComment= 16,
        // removeComment= 17,
        ChangeUserName = 18,
        ChangeUserDisplayName = 19,
        ChangeUserProfile = 20,
        ChangeUserEmail = 21,
        ChangeUserAvatar = 22,
        ChangeUserPassword = 23,
        ChangeUserPreference = 24,
        EnableTeamMember = 25,
        AppendMessageAttachment = 26,
        SignoutInOtherSite = 27,
        NewChannel = 28,
        ShareFile = 29,
        DisableService = 30,
        EnableService = 31,
        RemoveService = 32,
        AddCommand = 33,
        UpdateCommand = 34,
        DisableCommand = 35,
        EnableCommand = 36,
        RemoveCommand = 37,
        AddRss = 38,
        RemoveRss = 39,
        UpdateServiceAvatar = 40,
        ConvertChannel = 41,
        UpdateServiceBinding = 42,
        ChangeUserMobile = 43,
        UpdateMessage = 44,
        RemoveMessage = 45,
        AddPinned = 46,
        RemovePinned = 47,
        PhoneCallStart = 48,
        PhoneCallEnd = 49,
        GroupCallStart = 50,
        GroupCallMemberJoined = 51,
        GroupCallMemberLeave = 52,
        GroupCallEnd = 53,
        UnPickCall = 54,
        StarChannelOrSession = 55,
        UnstartChannelOrSession = 56,
        UpdateChannelPreference = 57,
        ClearUnRead = 58,
        AutoMessageSent = 59,
        ForceInitPassword = 60,


        //101-200 project
        CreateProject = 101,
        ChangeProjectProperties = 102,
        ChangeProjectMembers = 103,
        ArchiveProject = 104,
        UnarchiveProject = 105,
        DeleteProject = 106,
        RestoreProject = 107,
        CreateEntry = 131,
        UpdateEntry = 132,
        RemoveEntry = 133,
        MoveEntry = 134,
        CreateTask = 135,
        ChangeTaskProperties = 136,
        AddTaskWatchers = 137,
        RemoveTaskWatchers = 138,
        ChangeTaskAssignee = 139,
        MoveTask = 140,
        ChangeTaskDueDate = 141,
        SwitchTaskProject = 142,
        ArchiveTask = 143,
        UnarchiveTask = 144,
        DeleteTask = 145,
        RestoreTask = 146,
        CommentTask = 147,
        LikeTask = 148,
        AddTaskAttachment = 149,
        RemoveCommentTask = 150,
        RemoveLikeTask = 151,
        RemoveTaskAttachment = 152,
        AddTags = 153,
        RemoveTags = 154,
        ChangeTaskCompletion = 155,
        ArchiveTasksInEntry = 156,
        SetTags = 157,
        ChangeTaskBeginDate = 158,
        SetTaskEstimatedWorkload = 159,
        CreateTaskWorkloadEntry = 160,
        UpdateTaskWorkloadEntry = 161,
        RemoveTaskWorkloadEntry = 162,
        SetTaskBeginAndDueDate = 163,

        //201-300 calendar
        CreateCalendar = 201,
        UpdateCalendar = 0,
        RemoveCalendar = 0,
        CreateEvent = 251,
        UpdateEvent = 252,
        RemoveEvent = 253,
        UpdateEventInstance = 254,
        RemoveEventInstance = 255,
        CommentInstance = 256,
        LikeInstance = 257,
        AddInstanceAttachment = 258,
        RemoveCommentInstance = 259,
        RemoveLikeInstance = 260,
        RemoveInstanceAttachment = 261,

        //301-400 drive
        CreateFolder = 301,
        CreateFile = 302,
        RenameFolderOrFile = 303,
        UpdateFolderColor = 304,
        RemoveFolderOrFile = 305,
        UpdateFolderVisibilityOrPermission = 306,
        MoveFolder = 307,
        MoveFile = 308,
        CopyFolder = 309,
        CopyFile = 310,
        AddCommentToDrive = 311,
        RemoveCommentFromDrive = 312,

        //401-450 report
        AddCommentToReport = 401,
        RemoveCommentFromReport = 402,

        //451-500 approval
        AddCommentToApproval = 451,
        RemoveCommentFromApproval = 452,

        //501-550 meeting
        MeetingCall = 501,
        JoinMeeting = 502,

        //551-600
        AddCommentToContract = 551,
        RemoveCommentFromContract = 552,

        Hubot = 600,

        AddComment = 1000,
        RemoveComment = 1001
    }
}
