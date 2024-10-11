using SQLite4Unity3d;
using System;
using UnityEngine.UI;

public class SessionLog
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Summary { get; set; }
    public DateTime Created_at { get; set; }
    public int Counselor_id { get; set; }
    public int? Report_id { get; set; } // Nullable

    // 디버깅용 출력 string 생성하여 반환
    public override string ToString()
    {
        return string.Format("[SessionLog: Id={0}, Summary={1},  Created_at={2}, Counselor_id={3}, Report_id={4}]", Id, Summary, Created_at, Counselor_id, Report_id);
    }
}