namespace Worktile.Enums.Privileges
{
    enum AdminPrivilege
    {
        /// <summary>
        /// admin_members
        /// </summary>
        Member = 1,

        /// <summary>
        /// admin_role
        /// </summary>
        Role = 2,

        /// <summary>
        /// admin_applications
        /// </summary>
        Application = 4,

        /// <summary>
        /// admin_services
        /// </summary>
        Service = 8,

        /// <summary>
        /// admin_data
        /// </summary>
        Data = 16,

        /// <summary>
        /// admin_security
        /// </summary>
        Security = 32,

        /// <summary>
        /// admin_billing
        /// </summary>
        Billing = 64,

        /// <summary>
        /// admin_data_stats
        /// </summary>
        DataState = 128,

        /// <summary>
        /// admin_basic_setting
        /// </summary>
        BasicSetting = 256,

        /// <summary>
        /// admin_logo_setting
        /// </summary>
        LogoSetting = 512,

        /// <summary>
        /// admin_loadings_setting
        /// </summary>
        LoadingSetting = 1024
    }
}
