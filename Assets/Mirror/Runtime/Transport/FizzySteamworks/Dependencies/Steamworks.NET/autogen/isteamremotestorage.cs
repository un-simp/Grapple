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
using System.Collections.Generic;

namespace Steamworks
{
    public static class SteamRemoteStorage
    {
	    /// <summary>
	    ///     <para> NOTE</para>
	    ///     <para> Filenames are case-insensitive, and will be converted to lowercase automatically.</para>
	    ///     <para> So "foo.bar" and "Foo.bar" are the same file, and if you write "Foo.bar" then</para>
	    ///     <para> iterate the files, the filename returned will be "foo.bar".</para>
	    ///     <para> file operations</para>
	    /// </summary>
	    public static bool FileWrite(string pchFile, byte[] pvData, int cubData)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_FileWrite(CSteamAPIContext.GetSteamRemoteStorage(), pchFile2,
                    pvData, cubData);
            }
        }

        public static int FileRead(string pchFile, byte[] pvData, int cubDataToRead)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_FileRead(CSteamAPIContext.GetSteamRemoteStorage(), pchFile2,
                    pvData, cubDataToRead);
            }
        }

        public static SteamAPICall_t FileWriteAsync(string pchFile, byte[] pvData, uint cubData)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_FileWriteAsync(
                    CSteamAPIContext.GetSteamRemoteStorage(), pchFile2, pvData, cubData);
            }
        }

        public static SteamAPICall_t FileReadAsync(string pchFile, uint nOffset, uint cubToRead)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_FileReadAsync(
                    CSteamAPIContext.GetSteamRemoteStorage(), pchFile2, nOffset, cubToRead);
            }
        }

        public static bool FileReadAsyncComplete(SteamAPICall_t hReadCall, byte[] pvBuffer, uint cubToRead)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_FileReadAsyncComplete(CSteamAPIContext.GetSteamRemoteStorage(),
                hReadCall, pvBuffer, cubToRead);
        }

        public static bool FileForget(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_FileForget(CSteamAPIContext.GetSteamRemoteStorage(), pchFile2);
            }
        }

        public static bool FileDelete(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_FileDelete(CSteamAPIContext.GetSteamRemoteStorage(), pchFile2);
            }
        }

        public static SteamAPICall_t FileShare(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_FileShare(
                    CSteamAPIContext.GetSteamRemoteStorage(), pchFile2);
            }
        }

        public static bool SetSyncPlatforms(string pchFile, ERemoteStoragePlatform eRemoteStoragePlatform)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_SetSyncPlatforms(CSteamAPIContext.GetSteamRemoteStorage(),
                    pchFile2, eRemoteStoragePlatform);
            }
        }

        /// <summary>
        ///     <para> file operations that cause network IO</para>
        /// </summary>
        public static UGCFileWriteStreamHandle_t FileWriteStreamOpen(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return (UGCFileWriteStreamHandle_t) NativeMethods.ISteamRemoteStorage_FileWriteStreamOpen(
                    CSteamAPIContext.GetSteamRemoteStorage(), pchFile2);
            }
        }

        public static bool FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t writeHandle, byte[] pvData, int cubData)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_FileWriteStreamWriteChunk(CSteamAPIContext.GetSteamRemoteStorage(),
                writeHandle, pvData, cubData);
        }

        public static bool FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_FileWriteStreamClose(CSteamAPIContext.GetSteamRemoteStorage(),
                writeHandle);
        }

        public static bool FileWriteStreamCancel(UGCFileWriteStreamHandle_t writeHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_FileWriteStreamCancel(CSteamAPIContext.GetSteamRemoteStorage(),
                writeHandle);
        }

        /// <summary>
        ///     <para> file information</para>
        /// </summary>
        public static bool FileExists(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_FileExists(CSteamAPIContext.GetSteamRemoteStorage(), pchFile2);
            }
        }

        public static bool FilePersisted(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_FilePersisted(CSteamAPIContext.GetSteamRemoteStorage(),
                    pchFile2);
            }
        }

        public static int GetFileSize(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_GetFileSize(CSteamAPIContext.GetSteamRemoteStorage(),
                    pchFile2);
            }
        }

        public static long GetFileTimestamp(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_GetFileTimestamp(CSteamAPIContext.GetSteamRemoteStorage(),
                    pchFile2);
            }
        }

        public static ERemoteStoragePlatform GetSyncPlatforms(string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_GetSyncPlatforms(CSteamAPIContext.GetSteamRemoteStorage(),
                    pchFile2);
            }
        }

        /// <summary>
        ///     <para> iteration</para>
        /// </summary>
        public static int GetFileCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_GetFileCount(CSteamAPIContext.GetSteamRemoteStorage());
        }

        public static string GetFileNameAndSize(int iFile, out int pnFileSizeInBytes)
        {
            InteropHelp.TestIfAvailableClient();
            return InteropHelp.PtrToStringUTF8(
                NativeMethods.ISteamRemoteStorage_GetFileNameAndSize(CSteamAPIContext.GetSteamRemoteStorage(), iFile,
                    out pnFileSizeInBytes));
        }

        /// <summary>
        ///     <para> configuration management</para>
        /// </summary>
        public static bool GetQuota(out ulong pnTotalBytes, out ulong puAvailableBytes)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_GetQuota(CSteamAPIContext.GetSteamRemoteStorage(),
                out pnTotalBytes, out puAvailableBytes);
        }

        public static bool IsCloudEnabledForAccount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForAccount(CSteamAPIContext.GetSteamRemoteStorage());
        }

        public static bool IsCloudEnabledForApp()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_IsCloudEnabledForApp(CSteamAPIContext.GetSteamRemoteStorage());
        }

        public static void SetCloudEnabledForApp(bool bEnabled)
        {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamRemoteStorage_SetCloudEnabledForApp(CSteamAPIContext.GetSteamRemoteStorage(), bEnabled);
        }

        /// <summary>
        ///     <para> user generated content</para>
        ///     <para> Downloads a UGC file.  A priority value of 0 will download the file immediately,</para>
        ///     <para> otherwise it will wait to download the file until all downloads with a lower priority</para>
        ///     <para> value are completed.  Downloads with equal priority will occur simultaneously.</para>
        /// </summary>
        public static SteamAPICall_t UGCDownload(UGCHandle_t hContent, uint unPriority)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_UGCDownload(
                CSteamAPIContext.GetSteamRemoteStorage(), hContent, unPriority);
        }

        /// <summary>
        ///     <para>
        ///         Gets the amount of data downloaded so far for a piece of content. pnBytesExpected can be 0 if function
        ///         returns false
        ///     </para>
        ///     <para> or if the transfer hasn't started yet, so be careful to check for that before dividing to get a percentage</para>
        /// </summary>
        public static bool GetUGCDownloadProgress(UGCHandle_t hContent, out int pnBytesDownloaded,
            out int pnBytesExpected)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_GetUGCDownloadProgress(CSteamAPIContext.GetSteamRemoteStorage(),
                hContent, out pnBytesDownloaded, out pnBytesExpected);
        }

        /// <summary>
        ///     <para>
        ///         Gets metadata for a file after it has been downloaded. This is the same metadata given in the
        ///         RemoteStorageDownloadUGCResult_t call result
        ///     </para>
        /// </summary>
        public static bool GetUGCDetails(UGCHandle_t hContent, out AppId_t pnAppID, out string ppchName,
            out int pnFileSizeInBytes, out CSteamID pSteamIDOwner)
        {
            InteropHelp.TestIfAvailableClient();
            IntPtr ppchName2;
            var ret = NativeMethods.ISteamRemoteStorage_GetUGCDetails(CSteamAPIContext.GetSteamRemoteStorage(),
                hContent, out pnAppID, out ppchName2, out pnFileSizeInBytes, out pSteamIDOwner);
            ppchName = ret ? InteropHelp.PtrToStringUTF8(ppchName2) : null;
            return ret;
        }

        /// <summary>
        ///     <para> After download, gets the content of the file.</para>
        ///     <para>
        ///         Small files can be read all at once by calling this function with an offset of 0 and cubDataToRead equal to
        ///         the size of the file.
        ///     </para>
        ///     <para>
        ///         Larger files can be read in chunks to reduce memory usage (since both sides of the IPC client and the game
        ///         itself must allocate
        ///     </para>
        ///     <para>
        ///         enough memory for each chunk).  Once the last byte is read, the file is implicitly closed and further calls
        ///         to UGCRead will fail
        ///     </para>
        ///     <para> unless UGCDownload is called again.</para>
        ///     <para> For especially large files (anything over 100MB) it is a requirement that the file is read in chunks.</para>
        /// </summary>
        public static int UGCRead(UGCHandle_t hContent, byte[] pvData, int cubDataToRead, uint cOffset,
            EUGCReadAction eAction)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_UGCRead(CSteamAPIContext.GetSteamRemoteStorage(), hContent, pvData,
                cubDataToRead, cOffset, eAction);
        }

        /// <summary>
        ///     <para> Functions to iterate through UGC that has finished downloading but has not yet been read via UGCRead()</para>
        /// </summary>
        public static int GetCachedUGCCount()
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_GetCachedUGCCount(CSteamAPIContext.GetSteamRemoteStorage());
        }

        public static UGCHandle_t GetCachedUGCHandle(int iCachedContent)
        {
            InteropHelp.TestIfAvailableClient();
            return (UGCHandle_t) NativeMethods.ISteamRemoteStorage_GetCachedUGCHandle(
                CSteamAPIContext.GetSteamRemoteStorage(), iCachedContent);
        }
#if _SERVER
		/// <summary>
		/// <para> The following functions are only necessary on the Playstation 3. On PC &amp; Mac, the Steam client will handle these operations for you</para>
		/// <para> On Playstation 3, the game controls which files are stored in the cloud, via FilePersist, FileFetch, and FileForget.</para>
		/// <para> Connect to Steam and get a list of files in the Cloud - results in a RemoteStorageAppSyncStatusCheck_t callback</para>
		/// </summary>
		public static void GetFileListFromServer() {
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamRemoteStorage_GetFileListFromServer(CSteamAPIContext.GetSteamRemoteStorage());
		}

		/// <summary>
		/// <para> Indicate this file should be downloaded in the next sync</para>
		/// </summary>
		public static bool FileFetch(string pchFile) {
			InteropHelp.TestIfAvailableClient();
			using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile)) {
				return NativeMethods.ISteamRemoteStorage_FileFetch(CSteamAPIContext.GetSteamRemoteStorage(), pchFile2);
			}
		}

		/// <summary>
		/// <para> Indicate this file should be persisted in the next sync</para>
		/// </summary>
		public static bool FilePersist(string pchFile) {
			InteropHelp.TestIfAvailableClient();
			using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile)) {
				return NativeMethods.ISteamRemoteStorage_FilePersist(CSteamAPIContext.GetSteamRemoteStorage(), pchFile2);
			}
		}

		/// <summary>
		/// <para> Pull any requested files down from the Cloud - results in a RemoteStorageAppSyncedClient_t callback</para>
		/// </summary>
		public static bool SynchronizeToClient() {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_SynchronizeToClient(CSteamAPIContext.GetSteamRemoteStorage());
		}

		/// <summary>
		/// <para> Upload any requested files to the Cloud - results in a RemoteStorageAppSyncedServer_t callback</para>
		/// </summary>
		public static bool SynchronizeToServer() {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_SynchronizeToServer(CSteamAPIContext.GetSteamRemoteStorage());
		}

		/// <summary>
		/// <para> Reset any fetch/persist/etc requests</para>
		/// </summary>
		public static bool ResetFileRequestState() {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamRemoteStorage_ResetFileRequestState(CSteamAPIContext.GetSteamRemoteStorage());
		}
#endif
	    /// <summary>
	    ///     <para> publishing UGC</para>
	    /// </summary>
	    public static SteamAPICall_t PublishWorkshopFile(string pchFile, string pchPreviewFile, AppId_t nConsumerAppId,
            string pchTitle, string pchDescription, ERemoteStoragePublishedFileVisibility eVisibility,
            IList<string> pTags, EWorkshopFileType eWorkshopFileType)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            using (var pchPreviewFile2 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
            using (var pchTitle2 = new InteropHelp.UTF8StringHandle(pchTitle))
            using (var pchDescription2 = new InteropHelp.UTF8StringHandle(pchDescription))
            {
                return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_PublishWorkshopFile(
                    CSteamAPIContext.GetSteamRemoteStorage(), pchFile2, pchPreviewFile2, nConsumerAppId, pchTitle2,
                    pchDescription2, eVisibility, new InteropHelp.SteamParamStringArray(pTags), eWorkshopFileType);
            }
        }

        public static PublishedFileUpdateHandle_t CreatePublishedFileUpdateRequest(PublishedFileId_t unPublishedFileId)
        {
            InteropHelp.TestIfAvailableClient();
            return (PublishedFileUpdateHandle_t) NativeMethods.ISteamRemoteStorage_CreatePublishedFileUpdateRequest(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
        }

        public static bool UpdatePublishedFileFile(PublishedFileUpdateHandle_t updateHandle, string pchFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchFile2 = new InteropHelp.UTF8StringHandle(pchFile))
            {
                return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileFile(
                    CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, pchFile2);
            }
        }

        public static bool UpdatePublishedFilePreviewFile(PublishedFileUpdateHandle_t updateHandle,
            string pchPreviewFile)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchPreviewFile2 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
            {
                return NativeMethods.ISteamRemoteStorage_UpdatePublishedFilePreviewFile(
                    CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, pchPreviewFile2);
            }
        }

        public static bool UpdatePublishedFileTitle(PublishedFileUpdateHandle_t updateHandle, string pchTitle)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchTitle2 = new InteropHelp.UTF8StringHandle(pchTitle))
            {
                return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTitle(
                    CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, pchTitle2);
            }
        }

        public static bool UpdatePublishedFileDescription(PublishedFileUpdateHandle_t updateHandle,
            string pchDescription)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchDescription2 = new InteropHelp.UTF8StringHandle(pchDescription))
            {
                return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileDescription(
                    CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, pchDescription2);
            }
        }

        public static bool UpdatePublishedFileVisibility(PublishedFileUpdateHandle_t updateHandle,
            ERemoteStoragePublishedFileVisibility eVisibility)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileVisibility(
                CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, eVisibility);
        }

        public static bool UpdatePublishedFileTags(PublishedFileUpdateHandle_t updateHandle, IList<string> pTags)
        {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileTags(CSteamAPIContext.GetSteamRemoteStorage(),
                updateHandle, new InteropHelp.SteamParamStringArray(pTags));
        }

        public static SteamAPICall_t CommitPublishedFileUpdate(PublishedFileUpdateHandle_t updateHandle)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_CommitPublishedFileUpdate(
                CSteamAPIContext.GetSteamRemoteStorage(), updateHandle);
        }

        /// <summary>
        ///     <para> Gets published file details for the given publishedfileid.  If unMaxSecondsOld is greater than 0,</para>
        ///     <para> cached data may be returned, depending on how long ago it was cached.  A value of 0 will force a refresh.</para>
        ///     <para>
        ///         A value of k_WorkshopForceLoadPublishedFileDetailsFromCache will use cached data if it exists, no matter how
        ///         old it is.
        ///     </para>
        /// </summary>
        public static SteamAPICall_t GetPublishedFileDetails(PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_GetPublishedFileDetails(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId, unMaxSecondsOld);
        }

        public static SteamAPICall_t DeletePublishedFile(PublishedFileId_t unPublishedFileId)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_DeletePublishedFile(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
        }

        /// <summary>
        ///     <para> enumerate the files that the current user published with this app</para>
        /// </summary>
        public static SteamAPICall_t EnumerateUserPublishedFiles(uint unStartIndex)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumerateUserPublishedFiles(
                CSteamAPIContext.GetSteamRemoteStorage(), unStartIndex);
        }

        public static SteamAPICall_t SubscribePublishedFile(PublishedFileId_t unPublishedFileId)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_SubscribePublishedFile(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
        }

        public static SteamAPICall_t EnumerateUserSubscribedFiles(uint unStartIndex)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumerateUserSubscribedFiles(
                CSteamAPIContext.GetSteamRemoteStorage(), unStartIndex);
        }

        public static SteamAPICall_t UnsubscribePublishedFile(PublishedFileId_t unPublishedFileId)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_UnsubscribePublishedFile(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
        }

        public static bool UpdatePublishedFileSetChangeDescription(PublishedFileUpdateHandle_t updateHandle,
            string pchChangeDescription)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchChangeDescription2 = new InteropHelp.UTF8StringHandle(pchChangeDescription))
            {
                return NativeMethods.ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(
                    CSteamAPIContext.GetSteamRemoteStorage(), updateHandle, pchChangeDescription2);
            }
        }

        public static SteamAPICall_t GetPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_GetPublishedItemVoteDetails(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
        }

        public static SteamAPICall_t UpdateUserPublishedItemVote(PublishedFileId_t unPublishedFileId, bool bVoteUp)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_UpdateUserPublishedItemVote(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId, bVoteUp);
        }

        public static SteamAPICall_t GetUserPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_GetUserPublishedItemVoteDetails(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId);
        }

        public static SteamAPICall_t EnumerateUserSharedWorkshopFiles(CSteamID steamId, uint unStartIndex,
            IList<string> pRequiredTags, IList<string> pExcludedTags)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(
                CSteamAPIContext.GetSteamRemoteStorage(), steamId, unStartIndex,
                new InteropHelp.SteamParamStringArray(pRequiredTags),
                new InteropHelp.SteamParamStringArray(pExcludedTags));
        }

        public static SteamAPICall_t PublishVideo(EWorkshopVideoProvider eVideoProvider, string pchVideoAccount,
            string pchVideoIdentifier, string pchPreviewFile, AppId_t nConsumerAppId, string pchTitle,
            string pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IList<string> pTags)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchVideoAccount2 = new InteropHelp.UTF8StringHandle(pchVideoAccount))
            using (var pchVideoIdentifier2 = new InteropHelp.UTF8StringHandle(pchVideoIdentifier))
            using (var pchPreviewFile2 = new InteropHelp.UTF8StringHandle(pchPreviewFile))
            using (var pchTitle2 = new InteropHelp.UTF8StringHandle(pchTitle))
            using (var pchDescription2 = new InteropHelp.UTF8StringHandle(pchDescription))
            {
                return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_PublishVideo(
                    CSteamAPIContext.GetSteamRemoteStorage(), eVideoProvider, pchVideoAccount2, pchVideoIdentifier2,
                    pchPreviewFile2, nConsumerAppId, pchTitle2, pchDescription2, eVisibility,
                    new InteropHelp.SteamParamStringArray(pTags));
            }
        }

        public static SteamAPICall_t SetUserPublishedFileAction(PublishedFileId_t unPublishedFileId,
            EWorkshopFileAction eAction)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_SetUserPublishedFileAction(
                CSteamAPIContext.GetSteamRemoteStorage(), unPublishedFileId, eAction);
        }

        public static SteamAPICall_t EnumeratePublishedFilesByUserAction(EWorkshopFileAction eAction, uint unStartIndex)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(
                CSteamAPIContext.GetSteamRemoteStorage(), eAction, unStartIndex);
        }

        /// <summary>
        ///     <para> this method enumerates the public view of workshop files</para>
        /// </summary>
        public static SteamAPICall_t EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType eEnumerationType,
            uint unStartIndex, uint unCount, uint unDays, IList<string> pTags, IList<string> pUserTags)
        {
            InteropHelp.TestIfAvailableClient();
            return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(
                CSteamAPIContext.GetSteamRemoteStorage(), eEnumerationType, unStartIndex, unCount, unDays,
                new InteropHelp.SteamParamStringArray(pTags), new InteropHelp.SteamParamStringArray(pUserTags));
        }

        public static SteamAPICall_t UGCDownloadToLocation(UGCHandle_t hContent, string pchLocation, uint unPriority)
        {
            InteropHelp.TestIfAvailableClient();
            using (var pchLocation2 = new InteropHelp.UTF8StringHandle(pchLocation))
            {
                return (SteamAPICall_t) NativeMethods.ISteamRemoteStorage_UGCDownloadToLocation(
                    CSteamAPIContext.GetSteamRemoteStorage(), hContent, pchLocation2, unPriority);
            }
        }
    }
}

#endif // !DISABLESTEAMWORKS