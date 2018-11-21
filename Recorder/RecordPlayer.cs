using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphGame.Logic
{
    public class RecordPlayer
    {
        public RecordPlayer()
        {
        }

        private RecordData data = new RecordData();
        public void Load(string name)
        {
            this.data.Load(name);
        }

        public void Play()
        {

        }
    }
}
