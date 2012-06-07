using System;
using System.Windows.Input;
using KarateSemaphore.Events;

namespace KarateSemaphore
{
    public interface ISemaphore
    {
        /// <summary>
        ///   Gets or sets the <see cref="TimeSpan" /> to be used as the argument when the <see cref="Reset" /> command is executed. 
        ///   The given value is the forwarded to the <see cref="Reset" /> command.
        /// </summary>
        TimeSpan ResetTime { get; set; }

        /// <summary>
        ///   Gets the view model of the event manager.
        /// </summary>
        IEventManager EventManager { get; }

        /// <summary>
        ///   Gets the command for reseting of the match.
        /// </summary>
        ICommand Reset { get; }

        /// <summary>
        ///   Gets the view model of the stopwatch.
        /// </summary>
        IStopWatch Time { get; }

        /// <summary>
        ///   Gets the view model for the competitor with the <see cref="Belt.Aka" /> <see cref="Belt" /> .
        /// </summary>
        ICompetitor Aka { get; }

        /// <summary>
        ///   Gets the view model for the competitor with the <see cref="Belt.Ao" /> <see cref="Belt" /> .
        /// </summary>
        ICompetitor Ao { get; }

        /// <summary>
        ///   Gets a value indicating whether the semaphore is in the knockdown mode.
        /// </summary>
        bool IsKnockdown { get;  }

        /// <summary>
        ///   Gets the command that toggles the knockdown mode of the semaphore.
        /// </summary>
        ICommand ToggleKnockdownMode { get; }

        /// <summary>
        ///   Gets the command that requests a display text change.
        /// </summary>
        ICommand RequestDisplayTextChange { get; }
    }
}