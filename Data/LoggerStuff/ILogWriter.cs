using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSimulator.Data.LoggerStuff;

public interface ILogWriter
{
    void Write(IEnumerable<LogEntry> logEntries);
}
