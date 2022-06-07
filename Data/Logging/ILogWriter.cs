using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallSimulator.Data.Logging;

public interface ILogWriter : IDisposable
{
    void Write(IEnumerable<LogEntry> logEntries);
}
