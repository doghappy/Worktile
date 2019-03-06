namespace Worktile.Enums.Privileges
{
    enum AdminPrivilege
    {
        /// <summary>
        /// admin_members
        /// </summary>
        Member,

        /// <summary>
        /// admin_role
        /// </summary>
        Role = 2 ^ 1,

        /// <summary>
        /// admin_applications
        /// </summary>
        Application = 2 ^ 2,

        /// <summary>
        /// admin_services
        /// </summary>
        Service = 2 ^ 3,

        /// <summary>
        /// admin_data
        /// </summary>
        Data = 2 ^ 4,

        /// <summary>
        /// admin_security
        /// </summary>
        Security = 2 ^ 5,

        /// <summary>
        /// admin_billing
        /// </summary>
        Billing = 2 ^ 6,

        /// <summary>
        /// admin_data_stats
        /// </summary>
        DataState = 2 ^ 7,

        /// <summary>
        /// admin_basic_setting
        /// </summary>
        BasicSetting = 2 ^ 8,

        /// <summary>
        /// admin_logo_setting
        /// </summary>
        LogoSetting = 2 ^ 9,

        /// <summary>
        /// admin_loadings_setting
        /// </summary>
        LoadingSetting = 2 ^ 10
    }
}
