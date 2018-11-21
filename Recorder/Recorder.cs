using System;

namespace GraphGame.Logic
{
    public class Recorder
    {
        private RecordData data = new RecordData();
        public Recorder(Version version, int levelID, int levelSeed)
        {
            this.data.Version = version;
            this.data.LevelID = levelID;
            this.data.Seed = levelSeed;
        }

        public void EqueueStep(int i)
        {
            this.data.Steps.Add(i);
        }

        public void Save(string name)
        {
            this.data.Save(name);
        }
    }
}
