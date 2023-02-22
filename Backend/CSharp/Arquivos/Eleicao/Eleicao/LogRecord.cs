using System;
using System.Collections.Generic;
using System.Text;

namespace Eleicao
{
    class LogRecord
    {
        public string Candidate { get; set; }
        public int Votes { get; set; }

        public override int GetHashCode()
        {
            return Candidate.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LogRecord))
            {
                return false;
            }
            LogRecord other = obj as LogRecord;
            return Candidate.Equals(other.Candidate);
        }
    }
}
    