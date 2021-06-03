// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2019 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
	#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS

using System.Runtime.InteropServices;

namespace Steamworks
{
    public static class SteamApps
    {
        public static bool BIsSubscribed()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsSubscribed(CSteamAPIContext.GetSteamApps());
        }

        public static bool BIsLowViolence()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsLowViolence(CSteamAPIContext.GetSteamApps());
        }

        public static bool BIsCybercafe()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsCybercafe(CSteamAPIContext.GetSteamApps());
        }

        public static bool BIsVACBanned()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsVACBanned(CSteamAPIContext.GetSteamApps());
        }

        public static string GetCurrentGameLanguage()
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(
                NativeMethods.ISteamApps_GetCurrentGameLanguage(CSteamAPIContext.GetSteamApps()));
        }

        public static string GetAvailableGameLanguages()
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(
                NativeMethods.ISteamApps_GetAvailableGameLanguages(CSteamAPIContext.GetSteamApps()));
        }

        /// <summary>
        ///     <para> only use this member if you need to check ownership of another game related to yours, a demo for example</para>
        /// </summary>
        public static bool BIsSubscribedApp(AppId_t appID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsSubscribedApp(CSteamAPIContext.GetSteamApps(), appID);
        }

        /// <summary>
        ///     <para> Takes AppID of DLC and checks if the user owns the DLC &amp; if the DLC is installed</para>
        /// </summary>
        public static bool BIsDlcInstalled(AppId_t appID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsDlcInstalled(CSteamAPIContext.GetSteamApps(), appID);
        }

        /// <summary>
        ///     <para> returns the Unix time of the purchase of the app</para>
        /// </summary>
        public static uint GetEarliestPurchaseUnixTime(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(CSteamAPIContext.GetSteamApps(), nAppID);
        }

        /// <summary>
        ///     <para> Checks if the user is subscribed to the current app through a free weekend</para>
        ///     <para> This function will return false for users who have a retail or other type of license</para>
        ///     <para> Before using, please ask your Valve technical contact how to package and secure your free weekened</para>
        /// </summary>
        public static bool BIsSubscribedFromFreeWeekend()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend(CSteamAPIContext.GetSteamApps());
        }

        /// <summary>
        ///     <para> Returns the number of DLC pieces for the running app</para>
        /// </summary>
        public static int GetDLCCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetDLCCount(CSteamAPIContext.GetSteamApps());
        }

        /// <summary>
        ///     <para> Returns metadata for DLC by index, of range [0, GetDLCCount()]</para>
        /// </summary>
        public static bool BGetDLCDataByIndex(int iDLC, out AppId_t pAppID, out bool pbAvailable, out string pchName,
            int cchNameBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            var pchName2 = Marshal.AllocHGlobal(cchNameBufferSize);
            var ret = NativeMethods.ISteamApps_BGetDLCDataByIndex(CSteamAPIContext.GetSteamApps(), iDLC, out pAppID,
                out pbAvailable, pchName2, cchNameBufferSize);
            pchName = ret ? InteropHelp.PtrToStringUTF8(pchName2) : null;
            Marshal.FreeHGlobal(pchName2);
            return ret;
        }

        /// <summary>
        ///     <para> Install/Uninstall control for optional DLC</para>
        /// </summary>
        public static void InstallDLC(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamApps_InstallDLC(CSteamAPIContext.GetSteamApps(), nAppID);
        }

        public static void UninstallDLC(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamApps_UninstallDLC(CSteamAPIContext.GetSteamApps(), nAppID);
        }

        /// <summary>
        ///     <para> Request legacy cd-key for yourself or owned DLC. If you are interested in this</para>
        ///     <para> data then make sure you provide us with a list of valid keys to be distributed</para>
        ///     <para> to users when they purchase the game, before the game ships.</para>
        ///     <para> You'll receive an AppProofOfPurchaseKeyResponse_t callback when</para>
        ///     <para> the key is available (which may be immediately).</para>
        /// </summary>
        public static void RequestAppProofOfPurchaseKey(AppId_t nAppID)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(CSteamAPIContext.GetSteamApps(), nAppID);
        }

        /// <summary>
        ///     <para> returns current beta branch name, 'public' is the default branch</para>
        /// </summary>
        public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            var pchName2 = Marshal.AllocHGlobal(cchNameBufferSize);
            var ret = NativeMethods.ISteamApps_GetCurrentBetaName(CSteamAPIContext.GetSteamApps(), pchName2,
                cchNameBufferSize);
            pchName = ret ? InteropHelp.PtrToStringUTF8(pchName2) : null;
            Marshal.FreeHGlobal(pchName2);
            return ret;
        }

        /// <summary>
        ///     <para> signal Steam that game files seems corrupt or missing</para>
        /// </summary>
        public static bool MarkContentCorrupt(bool bMissingFilesOnly)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_MarkContentCorrupt(CSteamAPIContext.GetSteamApps(), bMissingFilesOnly);
        }

        /// <summary>
        ///     <para> return installed depots in mount order</para>
        /// </summary>
        public static uint GetInstalledDepots(AppId_t appID, DepotId_t[] pvecDepots, uint cMaxDepots)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetInstalledDepots(CSteamAPIContext.GetSteamApps(), appID, pvecDepots,
                cMaxDepots);
        }

        /// <summary>
        ///     <para> returns current app install folder for AppID, returns folder name length</para>
        /// </summary>
        public static uint GetAppInstallDir(AppId_t appID, out string pchFolder, uint cchFolderBufferSize)
        {
            InteropHelp.TestIfAvailableClient();
            var pchFolder2 = Marshal.AllocHGlobal((int) cchFolderBufferSize);
            var ret = NativeMethods.ISteamApps_GetAppInstallDir(CSteamAPIContext.GetSteamApps(), appID, pchFolder2,
                cchFolderBufferSize);
            pchFolder = ret != 0 ? InteropHelp.PtrToStringUTF8(pchFolder2) : null;
            Marshal.FreeHGlobal(pchFolder2);
            return ret;
        }

        /// <summary>
        ///     <para> returns true if that app is installed (not necessarily owned)</para>
        /// </summary>
        public static bool BIsAppInstalled(AppId_t appID)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsAppInstalled(CSteamAPIContext.GetSteamApps(), appID);
        }

        /// <summary>
        ///     <para> returns the SteamID of the original owner. If this CSteamID is different from ISteamUser::GetSteamID(),</para>
        ///     <para> the user has a temporary license borrowed via Family Sharing</para>
        /// </summary>
        public static CSteamID GetAppOwner()
        {
            InteropHelp.TestIfAvailableClient();
            return (CSteamID) NativeMethods.ISteamApps_GetAppOwner(CSteamAPIContext.GetSteamApps());
        }

        /// <summary>
        ///     <para>
        ///         Returns the associated launch param if the game is run via steam://run/&lt;appid&gt;//?param1=value1&amp;
        ///         param2=value2&amp;param3=value3 etc.
        ///     </para>
        ///     <para>
        ///         Parameter names starting with the character '@' are reserved for internal use and will always return and
        ///         empty string.
        ///     </para>
        ///     <para>
        ///         Parameter names starting with an underscore '_' are reserved for steam features -- they can be queried by
        ///         the game,
        ///     </para>
        ///     <para> but it is advised that you not param names beginning with an underscore for your own features.</para>
        ///     <para> Check for new launch parameters on callback NewUrlLaunchParameters_t</para>
        /// </summary>
        public static string GetLaunchQueryParam(string pchKey)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchKey2 = new InteropHelp.UTF8StringHandle(pchKey))
            {
                return InteropHelp.PtrToStringUTF8(
                    NativeMethods.ISteamApps_GetLaunchQueryParam(CSteamAPIContext.GetSteamApps(), pchKey2));
            }
        }

        /// <summary>
        ///     <para> get download progress for optional DLC</para>
        /// </summary>
        public static bool GetDlcDownloadProgress(AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetDlcDownloadProgress(CSteamAPIContext.GetSteamApps(), nAppID,
                out punBytesDownloaded, out punBytesTotal);
        }

        /// <summary>
        ///     <para> return the buildid of this app, may change at any time based on backend updates to the game</para>
        /// </summary>
        public static int GetAppBuildId()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_GetAppBuildId(CSteamAPIContext.GetSteamApps());
        }

        /// <summary>
        ///     <para> Request all proof of purchase keys for the calling appid and asociated DLC.</para>
        ///     <para> A series of AppProofOfPurchaseKeyResponse_t callbacks will be sent with</para>
        ///     <para> appropriate appid values, ending with a final callback where the m_nAppId</para>
        ///     <para> member is k_uAppIdInvalid (zero).</para>
        /// </summary>
        public static void RequestAllProofOfPurchaseKeys()
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamApps_RequestAllProofOfPurchaseKeys(CSteamAPIContext.GetSteamApps());
        }

        public static SteamAPICall_t GetFileDetails(string pszFileName)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pszFileName2 = new InteropHelp.UTF8StringHandle(pszFileName))
            {
                return (SteamAPICall_t) NativeMethods.ISteamApps_GetFileDetails(CSteamAPIContext.GetSteamApps(),
                    pszFileName2);
            }
        }

        /// <summary>
        ///     <para> Get command line if game was launched via Steam URL, e.g. steam://run/&lt;appid&gt;//&lt;command line&gt;/.</para>
        ///     <para> This method of passing a connect string (used when joining via rich presence, accepting an</para>
        ///     <para> invite, etc) is preferable to passing the connect string on the operating system command</para>
        ///     <para> line, which is a security risk.  In order for rich presence joins to go through this</para>
        ///     <para> path and not be placed on the OS command line, you must set a value in your app's</para>
        ///     <para> configuration on Steam.  Ask Valve for help with this.</para>
        ///     <para> If game was already running and launched again, the NewUrlLaunchParameters_t will be fired.</para>
        /// </summary>
        public static int GetLaunchCommandLine(out string pszCommandLine, int cubCommandLine)
        {
            InteropHelp.TestIfAvailableClient();
            var pszCommandLine2 = Marshal.AllocHGlobal(cubCommandLine);
            var ret = NativeMethods.ISteamApps_GetLaunchCommandLine(CSteamAPIContext.GetSteamApps(), pszCommandLine2,
                cubCommandLine);
            pszCommandLine = ret != -1 ? InteropHelp.PtrToStringUTF8(pszCommandLine2) : null;
            Marshal.FreeHGlobal(pszCommandLine2);
            return ret;
        }

        /// <summary>
        ///     <para> Check if user borrowed this game via Family Sharing, If true, call GetAppOwner() to get the lender SteamID</para>
        /// </summary>
        public static bool BIsSubscribedFromFamilySharing()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsSubscribedFromFamilySharing(CSteamAPIContext.GetSteamApps());
        }

        /// <summary>
        ///     <para> check if game is a timed trial with limited playtime</para>
        /// </summary>
        public static bool BIsTimedTrial(out uint punSecondsAllowed, out uint punSecondsPlayed)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamApps_BIsTimedTrial(CSteamAPIContext.GetSteamApps(), out punSecondsAllowed,
                out punSecondsPlayed);
        }
    }
}

#endif // !DISABLESTEAMWORKS