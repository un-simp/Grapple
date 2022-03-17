using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.Threading;
using Barji.Services;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private string s_leaderboardName = "default";
    private const ELeaderboardUploadScoreMethod s_leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;
    private SteamLeaderboard_t s_currentLeaderboard;
    private bool s_initialized = false;
    private CallResult<LeaderboardFindResult_t> m_findResult = new CallResult<LeaderboardFindResult_t>();
    private CallResult<LeaderboardScoreUploaded_t> m_uploadResult = new CallResult<LeaderboardScoreUploaded_t>();
    private CallResult<LeaderboardScoresDownloaded_t> m_downloadResult = new CallResult<LeaderboardScoresDownloaded_t>();
    private CallResult<LeaderboardScoresDownloaded_t> m_localDownloadResult = new CallResult<LeaderboardScoresDownloaded_t>();
    

    void Start()
    {
        Init();
    }

    public void Init()
    {
        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(s_leaderboardName);
        m_findResult.Set(hSteamAPICall, OnLeaderboardFindResult);
        InitTimer();
    }

    public void UpdateScore(int score)
    {
        if (!s_initialized)
        {
            UnityEngine.Debug.Log("Can't upload to the leaderboard because isn't loaded yet");
            return;
        }
        UnityEngine.Debug.Log("uploading score(" + score + ") to steam leaderboard(" + s_leaderboardName + ")");
        SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(s_currentLeaderboard, s_leaderboardMethod, score, null, 0);
        m_uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResult);
    }

    public void DownloadScores()
    {
        if (!s_initialized)
        {
            UnityEngine.Debug.Log("Can't download from the leaderboard because isn't loaded yet");
            return;
        }
        
        //This gets the top 5 players scores
        SteamAPICall_t hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 5);
        m_downloadResult.Set(hSteamAPICall, OnLeaderboardDownloadScores);

        //This gets the local players score
        CSteamID[] Users = { SteamUser.GetSteamID() }; // Local user steam id
        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntriesForUsers(s_currentLeaderboard, Users, Users.Length);
        m_localDownloadResult.Set(handle, OnLeaderboardDownloadEntriesForUsers);
    }

    private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        if(failure)
            UnityEngine.Debug.Log("Failed To find leaderboard");

        UnityEngine.Debug.Log("STEAM LEADERBOARDS: Found - " + pCallback.m_bLeaderboardFound + " leaderboardID - " + pCallback.m_hSteamLeaderboard.m_SteamLeaderboard);
        s_currentLeaderboard = pCallback.m_hSteamLeaderboard;
        s_initialized = true;
    }

    private void OnLeaderboardDownloadScores(LeaderboardScoresDownloaded_t pCallback, bool failure)
    {
        if(failure)
            UnityEngine.Debug.Log("Failed To download scores");
        GetComponent<GameplayManager>().scores = new Dictionary<string, float>();
        for(int i = 0; i < pCallback.m_cEntryCount; i++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out LeaderboardEntry_t leaderboardEntry, null, 0);
            GetComponent<GameplayManager>().scores.Add(SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser),leaderboardEntry.m_nScore);
        }
    }

    private void OnLeaderboardDownloadEntriesForUsers(LeaderboardScoresDownloaded_t pCallback, bool failure)
    {
        if(failure)
            UnityEngine.Debug.Log("Failed To download local scores");
        GetComponent<GameplayManager>().localScore = new Dictionary<int, float>();
        for(int i = 0; i < pCallback.m_cEntryCount; i++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out LeaderboardEntry_t leaderboardEntry, null, 0);
            GetComponent<GameplayManager>().localScore.Add(leaderboardEntry.m_nGlobalRank, leaderboardEntry.m_nScore);
        }
    }

    private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_bSuccess + " NewScore: " + pCallback.m_nGlobalRankNew + " Score " + pCallback.m_nScore + " HasChanged - " + pCallback.m_bScoreChanged);
    }

    private static Timer timer1; 
    public static void InitTimer()
    {
        timer1 = new Timer(timer1_Tick, null, 0, 1000);
    }

    private static void timer1_Tick(object state)
    {
        SteamAPI.RunCallbacks(); 
    }
}
