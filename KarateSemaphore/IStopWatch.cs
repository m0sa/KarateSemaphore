using System;
using System.Windows.Input;

namespace KarateSemaphore
{
    public interface IStopWatch
    {
        /// <summary>
        ///   Gets the command for resetting the stopwatch to an interval given with the command argument.
        /// </summary>
        ICommand Reset { get; }

        /// <summary>
        ///   Gets the command for toggling the start and paused state of the stopwatch.
        /// </summary>
        ICommand StartStop { get; }
        
        /// <summary>
        ///   Gets the command for applying a delta timespan to the <see cref="Remaining"/> timespan.
        /// </summary>
        ICommand Delta { get; }

        /// <summary>
        ///   Gets the raining time of the current match.
        /// </summary>
        TimeSpan Remaining { get; }

        /// <summary>
        ///   Event that happen 10 seconds before the <see cref="StopWatchViewModel.MatchEnd" /> event.
        /// </summary>
        event EventHandler Atoshibaraku;

        /// <summary>
        ///   Event that signals the end of a match.
        /// </summary>
        event EventHandler MatchEnd;
    }
}