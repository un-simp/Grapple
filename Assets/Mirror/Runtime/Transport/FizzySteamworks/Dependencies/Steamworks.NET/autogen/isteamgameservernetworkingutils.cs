#define STEAMNETWORKINGSOCKETS_ENABLE_SDR
// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2019 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
	#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
    public static class SteamGameServerNetworkingUtils
    {
	    /// <summary>
	    ///     <para> Efficient message sending</para>
	    ///     <para>/ Allocate and initialize a message object.  Usually the reason</para>
	    ///     <para>/ you call this is to pass it to ISteamNetworkingSockets::SendMessages.</para>
	    ///     <para>/ The returned object will have all of the relevant fields cleared to zero.</para>
	    ///     <para>/</para>
	    ///     <para>/ Optionally you can also request that this system allocate space to</para>
	    ///     <para>/ hold the payload itself.  If cbAllocateBuffer is nonzero, the system</para>
	    ///     <para>/ will allocate memory to hold a payload of at least cbAllocateBuffer bytes.</para>
	    ///     <para>/ m_pData will point to the allocated buffer, m_cbSize will be set to the</para>
	    ///     <para>/ size, and m_pfnFreeData will be set to the proper function to free up</para>
	    ///     <para>/ the buffer.</para>
	    ///     <para>/</para>
	    ///     <para>/ If cbAllocateBuffer=0, then no buffer is allocated.  m_pData will be NULL,</para>
	    ///     <para>/ m_cbSize will be zero, and m_pfnFreeData will be NULL.  You will need to</para>
	    ///     <para>/ set each of these.</para>
	    /// </summary>
	    public static IntPtr AllocateMessage(int cbAllocateBuffer)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_AllocateMessage(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), cbAllocateBuffer);
        }

	    /// <summary>
	    ///     <para> Access to Steam Datagram Relay (SDR) network</para>
	    ///     <para> Initialization and status check</para>
	    ///     <para>/ If you know that you are going to be using the relay network (for example,</para>
	    ///     <para>/ because you anticipate making P2P connections), call this to initialize the</para>
	    ///     <para>/ relay network.  If you do not call this, the initialization will</para>
	    ///     <para>/ be delayed until the first time you use a feature that requires access</para>
	    ///     <para>/ to the relay network, which will delay that first access.</para>
	    ///     <para>/</para>
	    ///     <para>/ You can also call this to force a retry if the previous attempt has failed.</para>
	    ///     <para>/ Performing any action that requires access to the relay network will also</para>
	    ///     <para>/ trigger a retry, and so calling this function is never strictly necessary,</para>
	    ///     <para>/ but it can be useful to call it a program launch time, if access to the</para>
	    ///     <para>/ relay network is anticipated.</para>
	    ///     <para>/</para>
	    ///     <para>/ Use GetRelayNetworkStatus or listen for SteamRelayNetworkStatus_t</para>
	    ///     <para>/ callbacks to know when initialization has completed.</para>
	    ///     <para>/ Typically initialization completes in a few seconds.</para>
	    ///     <para>/</para>
	    ///     <para>/ Note: dedicated servers hosted in known data centers do *not* need</para>
	    ///     <para>/ to call this, since they do not make routing decisions.  However, if</para>
	    ///     <para>/ the dedicated server will be using P2P functionality, it will act as</para>
	    ///     <para>/ a "client" and this should be called.</para>
	    /// </summary>
	    public static void InitRelayNetworkAccess()
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamNetworkingUtils_InitRelayNetworkAccess(CSteamGameServerAPIContext
                .GetSteamNetworkingUtils());
        }

	    /// <summary>
	    ///     <para>/ Fetch current status of the relay network.</para>
	    ///     <para>/</para>
	    ///     <para>/ SteamRelayNetworkStatus_t is also a callback.  It will be triggered on</para>
	    ///     <para>/ both the user and gameserver interfaces any time the status changes, or</para>
	    ///     <para>/ ping measurement starts or stops.</para>
	    ///     <para>/</para>
	    ///     <para>/ SteamRelayNetworkStatus_t::m_eAvail is returned.  If you want</para>
	    ///     <para>/ more details, you can pass a non-NULL value.</para>
	    /// </summary>
	    public static ESteamNetworkingAvailability GetRelayNetworkStatus(out SteamRelayNetworkStatus_t pDetails)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetRelayNetworkStatus(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out pDetails);
        }

	    /// <summary>
	    ///     <para> "Ping location" functions</para>
	    ///     <para> We use the ping times to the valve relays deployed worldwide to</para>
	    ///     <para> generate a "marker" that describes the location of an Internet host.</para>
	    ///     <para> Given two such markers, we can estimate the network latency between</para>
	    ///     <para> two hosts, without sending any packets.  The estimate is based on the</para>
	    ///     <para> optimal route that is found through the Valve network.  If you are</para>
	    ///     <para> using the Valve network to carry the traffic, then this is precisely</para>
	    ///     <para> the ping you want.  If you are not, then the ping time will probably</para>
	    ///     <para> still be a reasonable estimate.</para>
	    ///     <para> This is extremely useful to select peers for matchmaking!</para>
	    ///     <para> The markers can also be converted to a string, so they can be transmitted.</para>
	    ///     <para> We have a separate library you can use on your app's matchmaking/coordinating</para>
	    ///     <para> server to manipulate these objects.  (See steamdatagram_gamecoordinator.h)</para>
	    ///     <para>/ Return location info for the current host.  Returns the approximate</para>
	    ///     <para>/ age of the data, in seconds, or -1 if no data is available.</para>
	    ///     <para>/</para>
	    ///     <para>/ It takes a few seconds to initialize access to the relay network.  If</para>
	    ///     <para>/ you call this very soon after calling InitRelayNetworkAccess,</para>
	    ///     <para>/ the data may not be available yet.</para>
	    ///     <para>/</para>
	    ///     <para>/ This always return the most up-to-date information we have available</para>
	    ///     <para>/ right now, even if we are in the middle of re-calculating ping times.</para>
	    /// </summary>
	    public static float GetLocalPingLocation(out SteamNetworkPingLocation_t result)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetLocalPingLocation(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out result);
        }

	    /// <summary>
	    ///     <para>/ Estimate the round-trip latency between two arbitrary locations, in</para>
	    ///     <para>/ milliseconds.  This is a conservative estimate, based on routing through</para>
	    ///     <para>/ the relay network.  For most basic relayed connections, this ping time</para>
	    ///     <para>/ will be pretty accurate, since it will be based on the route likely to</para>
	    ///     <para>/ be actually used.</para>
	    ///     <para>/</para>
	    ///     <para>/ If a direct IP route is used (perhaps via NAT traversal), then the route</para>
	    ///     <para>/ will be different, and the ping time might be better.  Or it might actually</para>
	    ///     <para>/ be a bit worse!  Standard IP routing is frequently suboptimal!</para>
	    ///     <para>/</para>
	    ///     <para>/ But even in this case, the estimate obtained using this method is a</para>
	    ///     <para>/ reasonable upper bound on the ping time.  (Also it has the advantage</para>
	    ///     <para>/ of returning immediately and not sending any packets.)</para>
	    ///     <para>/</para>
	    ///     <para>/ In a few cases we might not able to estimate the route.  In this case</para>
	    ///     <para>/ a negative value is returned.  k_nSteamNetworkingPing_Failed means</para>
	    ///     <para>/ the reason was because of some networking difficulty.  (Failure to</para>
	    ///     <para>/ ping, etc)  k_nSteamNetworkingPing_Unknown is returned if we cannot</para>
	    ///     <para>/ currently answer the question for some other reason.</para>
	    ///     <para>/</para>
	    ///     <para>/ Do you need to be able to do this from a backend/matchmaking server?</para>
	    ///     <para>/ You are looking for the "ticketgen" library.</para>
	    /// </summary>
	    public static int EstimatePingTimeBetweenTwoLocations(ref SteamNetworkPingLocation_t location1,
            ref SteamNetworkPingLocation_t location2)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_EstimatePingTimeBetweenTwoLocations(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref location1, ref location2);
        }

	    /// <summary>
	    ///     <para>/ Same as EstimatePingTime, but assumes that one location is the local host.</para>
	    ///     <para>/ This is a bit faster, especially if you need to calculate a bunch of</para>
	    ///     <para>/ these in a loop to find the fastest one.</para>
	    ///     <para>/</para>
	    ///     <para>/ In rare cases this might return a slightly different estimate than combining</para>
	    ///     <para>/ GetLocalPingLocation with EstimatePingTimeBetweenTwoLocations.  That's because</para>
	    ///     <para>/ this function uses a slightly more complete set of information about what</para>
	    ///     <para>/ route would be taken.</para>
	    /// </summary>
	    public static int EstimatePingTimeFromLocalHost(ref SteamNetworkPingLocation_t remoteLocation)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_EstimatePingTimeFromLocalHost(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref remoteLocation);
        }

	    /// <summary>
	    ///     <para>/ Convert a ping location into a text format suitable for sending over the wire.</para>
	    ///     <para>/ The format is a compact and human readable.  However, it is subject to change</para>
	    ///     <para>/ so please do not parse it yourself.  Your buffer must be at least</para>
	    ///     <para>/ k_cchMaxSteamNetworkingPingLocationString bytes.</para>
	    /// </summary>
	    public static void ConvertPingLocationToString(ref SteamNetworkPingLocation_t location, out string pszBuf,
            int cchBufSize)
        {
            InteropHelp.TestIfAvailableGameServer();
            var pszBuf2 = Marshal.AllocHGlobal(cchBufSize);
            NativeMethods.ISteamNetworkingUtils_ConvertPingLocationToString(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref location, pszBuf2, cchBufSize);
            pszBuf = InteropHelp.PtrToStringUTF8(pszBuf2);
            Marshal.FreeHGlobal(pszBuf2);
        }

	    /// <summary>
	    ///     <para>/ Parse back SteamNetworkPingLocation_t string.  Returns false if we couldn't understand</para>
	    ///     <para>/ the string.</para>
	    /// </summary>
	    public static bool ParsePingLocationString(string pszString, out SteamNetworkPingLocation_t result)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (var pszString2 = new InteropHelp.UTF8StringHandle(pszString))
            {
                return NativeMethods.ISteamNetworkingUtils_ParsePingLocationString(
                    CSteamGameServerAPIContext.GetSteamNetworkingUtils(), pszString2, out result);
            }
        }

	    /// <summary>
	    ///     <para>/ Check if the ping data of sufficient recency is available, and if</para>
	    ///     <para>/ it's too old, start refreshing it.</para>
	    ///     <para>/</para>
	    ///     <para>/ Please only call this function when you *really* do need to force an</para>
	    ///     <para>/ immediate refresh of the data.  (For example, in response to a specific</para>
	    ///     <para>/ user input to refresh this information.)  Don't call it "just in case",</para>
	    ///     <para>/ before every connection, etc.  That will cause extra traffic to be sent</para>
	    ///     <para>/ for no benefit. The library will automatically refresh the information</para>
	    ///     <para>/ as needed.</para>
	    ///     <para>/</para>
	    ///     <para>/ Returns true if sufficiently recent data is already available.</para>
	    ///     <para>/</para>
	    ///     <para>/ Returns false if sufficiently recent data is not available.  In this</para>
	    ///     <para>/ case, ping measurement is initiated, if it is not already active.</para>
	    ///     <para>/ (You cannot restart a measurement already in progress.)</para>
	    ///     <para>/</para>
	    ///     <para>/ You can use GetRelayNetworkStatus or listen for SteamRelayNetworkStatus_t</para>
	    ///     <para>/ to know when ping measurement completes.</para>
	    /// </summary>
	    public static bool CheckPingDataUpToDate(float flMaxAgeSeconds)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_CheckPingDataUpToDate(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), flMaxAgeSeconds);
        }

	    /// <summary>
	    ///     <para> List of Valve data centers, and ping times to them.  This might</para>
	    ///     <para> be useful to you if you are use our hosting, or just need to measure</para>
	    ///     <para> latency to a cloud data center where we are running relays.</para>
	    ///     <para>/ Fetch ping time of best available relayed route from this host to</para>
	    ///     <para>/ the specified data center.</para>
	    /// </summary>
	    public static int GetPingToDataCenter(SteamNetworkingPOPID popID, out SteamNetworkingPOPID pViaRelayPoP)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetPingToDataCenter(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), popID, out pViaRelayPoP);
        }

	    /// <summary>
	    ///     <para>/ Get *direct* ping time to the relays at the data center.</para>
	    /// </summary>
	    public static int GetDirectPingToPOP(SteamNetworkingPOPID popID)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetDirectPingToPOP(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), popID);
        }

	    /// <summary>
	    ///     <para>/ Get number of network points of presence in the config</para>
	    /// </summary>
	    public static int GetPOPCount()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetPOPCount(CSteamGameServerAPIContext
                .GetSteamNetworkingUtils());
        }

	    /// <summary>
	    ///     <para>/ Get list of all POP IDs.  Returns the number of entries that were filled into</para>
	    ///     <para>/ your list.</para>
	    /// </summary>
	    public static int GetPOPList(out SteamNetworkingPOPID list, int nListSz)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetPOPList(CSteamGameServerAPIContext.GetSteamNetworkingUtils(),
                out list, nListSz);
        }

	    /// <summary>
	    ///     <para> Misc</para>
	    ///     <para>/ Fetch current timestamp.  This timer has the following properties:</para>
	    ///     <para>/</para>
	    ///     <para>/ - Monotonicity is guaranteed.</para>
	    ///     <para>/ - The initial value will be at least 24*3600*30*1e6, i.e. about</para>
	    ///     <para>/   30 days worth of microseconds.  In this way, the timestamp value of</para>
	    ///     <para>/   0 will always be at least "30 days ago".  Also, negative numbers</para>
	    ///     <para>/   will never be returned.</para>
	    ///     <para>/ - Wraparound / overflow is not a practical concern.</para>
	    ///     <para>/</para>
	    ///     <para>/ If you are running under the debugger and stop the process, the clock</para>
	    ///     <para>/ might not advance the full wall clock time that has elapsed between</para>
	    ///     <para>/ calls.  If the process is not blocked from normal operation, the</para>
	    ///     <para>/ timestamp values will track wall clock time, even if you don't call</para>
	    ///     <para>/ the function frequently.</para>
	    ///     <para>/</para>
	    ///     <para>/ The value is only meaningful for this run of the process.  Don't compare</para>
	    ///     <para>/ it to values obtained on another computer, or other runs of the same process.</para>
	    /// </summary>
	    public static SteamNetworkingMicroseconds GetLocalTimestamp()
        {
            InteropHelp.TestIfAvailableGameServer();
            return (SteamNetworkingMicroseconds) NativeMethods.ISteamNetworkingUtils_GetLocalTimestamp(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils());
        }

	    /// <summary>
	    ///     <para>/ Set a function to receive network-related information that is useful for debugging.</para>
	    ///     <para>/ This can be very useful during development, but it can also be useful for troubleshooting</para>
	    ///     <para>/ problems with tech savvy end users.  If you have a console or other log that customers</para>
	    ///     <para>/ can examine, these log messages can often be helpful to troubleshoot network issues.</para>
	    ///     <para>/ (Especially any warning/error messages.)</para>
	    ///     <para>/</para>
	    ///     <para>/ The detail level indicates what message to invoke your callback on.  Lower numeric</para>
	    ///     <para>/ value means more important, and the value you pass is the lowest priority (highest</para>
	    ///     <para>/ numeric value) you wish to receive callbacks for.</para>
	    ///     <para>/</para>
	    ///     <para>/ The value here controls the detail level for most messages.  You can control the</para>
	    ///     <para>/ detail level for various subsystems (perhaps only for certain connections) by</para>
	    ///     <para>/ adjusting the configuration values k_ESteamNetworkingConfig_LogLevel_Xxxxx.</para>
	    ///     <para>/</para>
	    ///     <para>/ Except when debugging, you should only use k_ESteamNetworkingSocketsDebugOutputType_Msg</para>
	    ///     <para>/ or k_ESteamNetworkingSocketsDebugOutputType_Warning.  For best performance, do NOT</para>
	    ///     <para>/ request a high detail level and then filter out messages in your callback.  This incurs</para>
	    ///     <para>/ all of the expense of formatting the messages, which are then discarded.  Setting a high</para>
	    ///     <para>/ priority value (low numeric value) here allows the library to avoid doing this work.</para>
	    ///     <para>/</para>
	    ///     <para>/ IMPORTANT: This may be called from a service thread, while we own a mutex, etc.</para>
	    ///     <para>/ Your output function must be threadsafe and fast!  Do not make any other</para>
	    ///     <para>/ Steamworks calls from within the handler.</para>
	    /// </summary>
	    public static void SetDebugOutputFunction(ESteamNetworkingSocketsDebugOutputType eDetailLevel,
            FSteamNetworkingSocketsDebugOutput pfnFunc)
        {
            InteropHelp.TestIfAvailableGameServer();
            NativeMethods.ISteamNetworkingUtils_SetDebugOutputFunction(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eDetailLevel, pfnFunc);
        }

	    /// <summary>
	    ///     <para> Set and get configuration values, see ESteamNetworkingConfigValue for individual descriptions.</para>
	    ///     <para> Shortcuts for common cases.  (Implemented as inline functions below)</para>
	    ///     <para> Set global callbacks.  If you do not want to use Steam's callback dispatch mechanism and you</para>
	    ///     <para> want to use the same callback on all (or most) listen sockets and connections, then</para>
	    ///     <para> simply install these callbacks first thing, and you are good to go.</para>
	    ///     <para> See ISteamNetworkingSockets::RunCallbacks</para>
	    ///     <para>/ Set a configuration value.</para>
	    ///     <para>/ - eValue: which value is being set</para>
	    ///     <para>/ - eScope: Onto what type of object are you applying the setting?</para>
	    ///     <para>
	    ///         / - scopeArg: Which object you want to change?  (Ignored for global scope).  E.g. connection handle, listen
	    ///         socket handle, interface pointer, etc.
	    ///     </para>
	    ///     <para>
	    ///         / - eDataType: What type of data is in the buffer at pValue?  This must match the type of the variable
	    ///         exactly!
	    ///     </para>
	    ///     <para>/ - pArg: Value to set it to.  You can pass NULL to remove a non-global setting at this scope,</para>
	    ///     <para>/   causing the value for that object to use global defaults.  Or at global scope, passing NULL</para>
	    ///     <para>/   will reset any custom value and restore it to the system default.</para>
	    ///     <para>/   NOTE: When setting pointers (e.g. callback functions), do not pass the function pointer directly.</para>
	    ///     <para>/   Your argument should be a pointer to a function pointer.</para>
	    /// </summary>
	    public static bool SetConfigValue(ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType,
            IntPtr scopeObj, ESteamNetworkingConfigDataType eDataType, IntPtr pArg)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_SetConfigValue(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eValue, eScopeType, scopeObj, eDataType, pArg);
        }

	    /// <summary>
	    ///     <para>/ Set a configuration value, using a struct to pass the value.</para>
	    ///     <para>/ (This is just a convenience shortcut; see below for the implementation and</para>
	    ///     <para>/ a little insight into how SteamNetworkingConfigValue_t is used when</para>
	    ///     <para>/ setting config options during listen socket and connection creation.)</para>
	    ///     <para>/ Get a configuration value.</para>
	    ///     <para>/ - eValue: which value to fetch</para>
	    ///     <para>/ - eScopeType: query setting on what type of object</para>
	    ///     <para>/ - eScopeArg: the object to query the setting for</para>
	    ///     <para>/ - pOutDataType: If non-NULL, the data type of the value is returned.</para>
	    ///     <para>
	    ///         / - pResult: Where to put the result.  Pass NULL to query the required buffer size.
	    ///         (k_ESteamNetworkingGetConfigValue_BufferTooSmall will be returned.)
	    ///     </para>
	    ///     <para>/ - cbResult: IN: the size of your buffer.  OUT: the number of bytes filled in or required.</para>
	    /// </summary>
	    public static ESteamNetworkingGetConfigValueResult GetConfigValue(ESteamNetworkingConfigValue eValue,
            ESteamNetworkingConfigScope eScopeType, IntPtr scopeObj, out ESteamNetworkingConfigDataType pOutDataType,
            IntPtr pResult, out ulong cbResult)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetConfigValue(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eValue, eScopeType, scopeObj, out pOutDataType,
                pResult, out cbResult);
        }

	    /// <summary>
	    ///     <para>/ Returns info about a configuration value.  Returns false if the value does not exist.</para>
	    ///     <para>/ pOutNextValue can be used to iterate through all of the known configuration values.</para>
	    ///     <para>
	    ///         / (Use GetFirstConfigValue() to begin the iteration, will be k_ESteamNetworkingConfig_Invalid on the last
	    ///         value)
	    ///     </para>
	    ///     <para>/ Any of the output parameters can be NULL if you do not need that information.</para>
	    ///     <para>/</para>
	    ///     <para>/ See k_ESteamNetworkingConfig_EnumerateDevVars for some more info about "dev" variables,</para>
	    ///     <para>/ which are usually excluded from the set of variables enumerated using this function.</para>
	    /// </summary>
	    public static bool GetConfigValueInfo(ESteamNetworkingConfigValue eValue, IntPtr pOutName,
            out ESteamNetworkingConfigDataType pOutDataType, out ESteamNetworkingConfigScope pOutScope,
            out ESteamNetworkingConfigValue pOutNextValue)
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetConfigValueInfo(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), eValue, pOutName, out pOutDataType, out pOutScope,
                out pOutNextValue);
        }

	    /// <summary>
	    ///     <para>/ Return the lowest numbered configuration value available in the current environment.</para>
	    /// </summary>
	    public static ESteamNetworkingConfigValue GetFirstConfigValue()
        {
            InteropHelp.TestIfAvailableGameServer();
            return NativeMethods.ISteamNetworkingUtils_GetFirstConfigValue(CSteamGameServerAPIContext
                .GetSteamNetworkingUtils());
        }

	    /// <summary>
	    ///     <para> String conversions.  You'll usually access these using the respective</para>
	    ///     <para> inline methods.</para>
	    /// </summary>
	    public static void SteamNetworkingIPAddr_ToString(ref SteamNetworkingIPAddr addr, out string buf, uint cbBuf,
            bool bWithPort)
        {
            InteropHelp.TestIfAvailableGameServer();
            var buf2 = Marshal.AllocHGlobal((int) cbBuf);
            NativeMethods.ISteamNetworkingUtils_SteamNetworkingIPAddr_ToString(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref addr, buf2, cbBuf, bWithPort);
            buf = InteropHelp.PtrToStringUTF8(buf2);
            Marshal.FreeHGlobal(buf2);
        }

        public static bool SteamNetworkingIPAddr_ParseString(out SteamNetworkingIPAddr pAddr, string pszStr)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (var pszStr2 = new InteropHelp.UTF8StringHandle(pszStr))
            {
                return NativeMethods.ISteamNetworkingUtils_SteamNetworkingIPAddr_ParseString(
                    CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out pAddr, pszStr2);
            }
        }

        public static void SteamNetworkingIdentity_ToString(ref SteamNetworkingIdentity identity, out string buf,
            uint cbBuf)
        {
            InteropHelp.TestIfAvailableGameServer();
            var buf2 = Marshal.AllocHGlobal((int) cbBuf);
            NativeMethods.ISteamNetworkingUtils_SteamNetworkingIdentity_ToString(
                CSteamGameServerAPIContext.GetSteamNetworkingUtils(), ref identity, buf2, cbBuf);
            buf = InteropHelp.PtrToStringUTF8(buf2);
            Marshal.FreeHGlobal(buf2);
        }

        public static bool SteamNetworkingIdentity_ParseString(out SteamNetworkingIdentity pIdentity, string pszStr)
        {
            InteropHelp.TestIfAvailableGameServer();
            using (var pszStr2 = new InteropHelp.UTF8StringHandle(pszStr))
            {
                return NativeMethods.ISteamNetworkingUtils_SteamNetworkingIdentity_ParseString(
                    CSteamGameServerAPIContext.GetSteamNetworkingUtils(), out pIdentity, pszStr2);
            }
        }
    }
}

#endif // !DISABLESTEAMWORKS