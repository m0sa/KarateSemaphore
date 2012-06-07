using System.Windows.Input;

namespace KarateSemaphore
{
    public interface ICompetitor
    {
        /// <summary>
        /// Gets the <see cref="Belt"/> color.
        /// </summary>
        Belt Belt { get; }

        /// <summary>
        /// Gets or sets the total points.
        /// </summary>
        int Points { get; set; }

        /// <summary>
        /// Gets the penalties in the category C1.
        /// </summary>
        Penalty C1 { get; set; }

        /// <summary>
        /// Gets the penalties in the category C2.
        /// </summary>
        Penalty C2 { get; set; }

        /// <summary>
        /// Gets the command that can be used to <see cref="Award"/> the competitor with points.
        /// <para />
        /// An <see cref="Award"/> argument is required when executing.
        /// </summary>
        ICommand ChangePoints { get; }

        /// <summary>
        /// Gets the command that can be used to issue an <see cref="Penalty"/> in the category C1. 
        /// <para />
        /// An <see cref="Penalty"/> argument is required when executing.
        /// </summary>
        ICommand ChangeC1 { get; }

        /// <summary>
        /// Gets the command that can be used to issue an <see cref="Penalty"/> in the category C2. 
        /// <para />
        /// An <see cref="Penalty"/> argument is required when executing.
        /// </summary>
        ICommand ChangeC2 { get; }

        /// <summary>
        /// Gets or sets the name to be displayed for this competitor.
        /// </summary>
        string DisplayName { get; set; }
    }
}