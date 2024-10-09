using SQLite4Unity3d;
using System;
using UnityEngine.UI;

public class ReportLog
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Content { get; set; }

    public string Summary { get; set; }
    public DateTime Created_at { get; set; }
    public int Counselor_id { get; set; }

    // 디버깅용 출력 string 생성하여 반환
    public override string ToString()
    {
        return string.Format("[SessionLog: Id={0}, Content={1}, Summary={4}, Created_at={2}, Counselor_id={3}]", Id, Content, Created_at, Counselor_id, Summary);
    }
}