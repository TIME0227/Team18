using SQLite4Unity3d;
using UnityEngine;
using System.Linq;
using System;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

/*
 * 함수 사용 시
 * var ds = new DataService("database.db"); // 데이터베이스 연결
 * 
 * ---수정---
 * DataService ds; 로 전역변수 선언 후
 * Awake()나 Start()에서 ds = new DataService("database.db"); 로 초기화
 * ----------
 * 
 * 이후 함수 사용 
 * counselor_id 파라미터는 GetCounselorIdByName("KindNPC") 로 가져올 수 있음
 */

public class DataService
{

    private SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }

    // 상담사의 ID를 반환하는 함수
    public int GetCounselorIdByName(string npcName)
    {
        switch (npcName)
        {
            case "CognitiveNPC":
                return 1;
            case "StrengthNPC":
                return 2;
            case "KindNPC":
                return 3;
            case "CynicalNPC":
                return 4;
            default:
                return 1;
        }
    }

    // 한 상담사의 SessionLog를 모두 반환하는 함수
    // 1:Cognitive, 2:Strength, 3:Kind, 4:Cynical
    public IEnumerable<SessionLog> GetSessionLog(int counselor_id = 0)
    {
        if (counselor_id == 0) return _connection.Table<SessionLog>(); // 파라미터 없이 사용할 경우 모든 데이터 반환
        return _connection.Table<SessionLog>().Where(x => x.Counselor_id == counselor_id);
    }
    // 예시
    /*
       var SessionLog = ds.GetSessionLog(1); // 인지치료 상담사

       private void ToConsole(IEnumerable<SessionLog> session_logs)
        {
         foreach (var log in session_logs)
             {
                Debug.Log(log.ToString());
                // 각 속성에 접근
                int id = log.Id;
                string summary = log.Summary;
                DateTime created_at = log.Created_at;
             }
         }
     */

    // 리포트 생성에 쓰이지 않은 SessionLog만을 반환
    // 1:Cognitive, 2:Strength, 3:Kind, 4:Cynical
    public IEnumerable<SessionLog> GetNotReportedSessionLog(int counselor_id)
    {
        return _connection.Table<SessionLog>().Where(x => x.Counselor_id == counselor_id && x.Report_id == null);
    }

    // 한 상담사의 리포트를 모두 반환하는 함수
    public IEnumerable<ReportLog> GetReportLog(int counselor_id = 0)
    {
        if (counselor_id == 0) return _connection.Table<ReportLog>(); // 파라미터 사용하지 않으면 모든 데이터 반환
        return _connection.Table<ReportLog>().Where(x => x.Counselor_id == counselor_id);
    }
    // 예시
    /*
     * foreach (var log in ds.GetReportLog(1))
     *    {
     *      Debug.Log(log.ToString());
     *      int id = log.Id;
     *      string report_content = log.Content
     *      string summary = log.Summary;
     *      DateTime created_at = log.Created_at;
     */

    // 대화시작 시 이전대화 기억용 프롬프트에 넣을 요약본을 생성하여 반환하는 함수
    // 1:Cognitive, 2:Strength, 3:Kind, 4:Cynical, 리포트 생성을 위해 쓸 때는 is_sessionlog = false로 전달
    public string GetConversationHistory(int counselor_id, bool is_sessionlog = true)
    {
        if(counselor_id == 2 && is_sessionlog == true) // Strength상담사는 일회성이라 연속된 대화 제공x
        {
            return "지금부터 진행할 상담에 대해 설명을 해줘.";
        }

        var summaries = GetNotReportedSessionLog(counselor_id);
        string result = "리포트 작성과 메타요약본 생성을 부탁할게. 형식-REPORT:(리포트내용)METASUMMARY:(요약한 내용)/n";

        int summaries_count = 0;

        // 리포트 요약본 가져오기, 리포트 생성할 때에는 필요 없음
        if (is_sessionlog)
        {
            result = "이전에 나눈 대화인데 지금 새로 대화할 때 기억하는 것 처럼 참고하고, 안부 묻는 첫인사를 해줘./n";

            var metasummaries = GetReportLog(counselor_id);

            foreach (var log in metasummaries)
            {
                // "1~5번째 대화: 요약본\n" 형식으로 저장
                result += (summaries_count * 5 + 1) + "~" + (summaries_count * 5 + 5) + "번째 대화: " + log.Summary + "\n";
                summaries_count++;
            }

        }

        // 세션기록 가져오기
        int conversation_count = summaries_count * 5 + 1;
        foreach (var log in summaries)
        {
            // "6번째 대화: 요약본\n" 형식으로 저장
            result += conversation_count + "번째 대화: " + log.Summary + "\n";
            conversation_count++;
        }
        return result;
    }

    // SessionLog Table에 Insert하는 함수
    // 1:Cognitive, 2:Strength, 3:Kind, 4:Cynical
    public SessionLog CreateSessionLog(string summary, int counselor_id)
    {
        var p = new SessionLog
        {
            Summary = summary,
            Counselor_id = counselor_id,
            Created_at = DateTime.Now
        };
        _connection.Insert(p);
        return p;
    }
    // 예시
    /* 
     * ds.CreateSessionLog( gpt에게서 받은 요약본 string, 상담사 id );
     * Debug.Log("New SessionLog has been created");
     */

    // ReportLog Table에 insert하는 함수
    // 1:Cognitive, 3:Kind, 4:Cynical (장점찾기 상담사는 리포트를 만들지 않음)
    public ReportLog CreateReportLog(string content, string summary, int counselor_id)
    {
        var p = new ReportLog
        {
            Content = content,
            Summary = summary,
            Counselor_id = counselor_id,
            Created_at = DateTime.Now
        };
        _connection.Insert(p);
        return p;
    }

    // SessionLog 5개가 모였는지 체크하는 함수
    // 대화종료할 때마다 체크 후, 충족 시 리포트를 생성해야 함
    public bool HasFiveNotReportedSessionLogs(int counselor_id)
    {
        var notReportedLogs = GetNotReportedSessionLog(counselor_id).ToList(); // Count를 쓰기 위해 리스트로 변환
        return notReportedLogs.Count == 5;
    }

    // 새로 리포트를 생성한 SessionLog의 Report_id를 업데이트하는 함수
    public void UpdateReportIdForSessionLogs(int counselor_id, int newReportId)
    {
        var notReportedLogs = GetNotReportedSessionLog(counselor_id);

        foreach (var log in notReportedLogs)
        {
            log.Report_id = newReportId;
            _connection.Update(log);
        }
    }

    // SessionLog Table의 특정 레코드 삭제 함수
    // 1:Cognitive, 2:Strength, 3:Kind, 4:Cynical
    public void DeleteOneSessionLog(int id)
    {
        var sessionLog = _connection.Table<SessionLog>().FirstOrDefault(x => x.Id == id);
        if (sessionLog != null)
        {
            _connection.Delete(sessionLog);
        }
    }
    // 예시: DeleteOneSessionLog(log.Id);

    // ReportLog Table의 특정 레코드 삭제 함수
    public void DeleteOneReportLog(int id)
    {
        var reportnLog = _connection.Table<ReportLog>().FirstOrDefault(x => x.Id == id);
        if (reportnLog != null)
        {
            _connection.Delete(reportnLog);
        }
    }

    // SessionLog와 ReportLog Table의 모든 레코드 삭제 함수
    // 탈퇴(?) 기능 넣는다면 사용
    public void DeleteAllLogs()
    {
        _connection.DeleteAll<SessionLog>();
        _connection.DeleteAll<ReportLog>();
    }
}