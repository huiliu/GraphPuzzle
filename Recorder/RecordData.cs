using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace GraphGame.Logic
{
    public class RecordData
    {
        public Version Version;
        public int LevelID;
        public int Seed;
        public List<MoveStep> Steps = new List<MoveStep>();
    }

    public enum StepType
    {
        Normal,     // 正常落子
        Rollback,   // 悔子
    }

    public struct MoveStep
    {
        public string   UID;
        public StepType Type;
        public int      Row;
        public int      Col;
    }

    public static class RecordDataHelper
    {
        public static bool Save(this RecordData recordData, string name)
        {
            try
            {
                using (var sw = new StreamWriter(name))
                {
                    var serializer = new XmlSerializer(typeof(RecordData));
                    serializer.Serialize(sw, recordData);
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return false;
            }

            return true;
        }

        public static void Load(this RecordData recordData, string name)
        {
            try
            {
                using (var sr = new StreamReader(name))
                {
                    var serializer = new XmlSerializer(typeof(RecordData));
                    recordData = serializer.Deserialize(sr) as RecordData;
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
            }
        }
    }
}
