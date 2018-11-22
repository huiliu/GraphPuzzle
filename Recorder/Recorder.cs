using System;

namespace GraphGame.Logic
{
    public class Recorder
    {
        public RecordData Data { get; private set; }
        public Recorder(Version version, int levelID, int levelSeed)
        {
            this.Data = new RecordData();
            this.Data.Version = version;
            this.Data.LevelID = levelID;
            this.Data.Seed = levelSeed;
        }

        public void EqueueStep(MoveStep step)
        {
            this.Data.Steps.Add(step);
        }

        public void Save()
        {
            this.Data.Save(this.GetArchiveFileName());
        }

        protected virtual string GetArchiveFileName()
        {
            return "Video_" + DateTime.Now.ToString("yyMMhh_HHmmss") + ".xml";
        }
    }
}
