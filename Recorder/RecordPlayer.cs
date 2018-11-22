using System;
using System.IO;
using System.Xml.Serialization;

namespace GraphGame.Logic
{
    public abstract class RecordPlayer
    {
        protected RecordData Data;
        public abstract void Load(string name);
    }
}
