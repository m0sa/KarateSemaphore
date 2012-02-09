using System.Globalization;

namespace WkfSemaphore.Events
{
    public class AwardEvent : TimestampedEvent
    {
        private readonly CompetitorViewModel _competitor;
        private readonly Award _award;
        private readonly int _initialPoints;

        public AwardEvent(CompetitorViewModel competitor, Award award)
        {
            _competitor = competitor;
            _award = award;
            _initialPoints = competitor.Points;
        }

        public override string Display
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}: {1} {2}", base.Display, _competitor.Belt, _award); }
        }

        public override void Redo()
        {
            _competitor.Points = _initialPoints + (int)_award;
        }

        public override void Undo()
        {
            _competitor.Points = _initialPoints;
        }
    }
}
