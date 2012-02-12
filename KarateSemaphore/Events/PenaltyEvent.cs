using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace WkfSemaphore.Events
{

    public class PenaltyEvent : TimestampedEvent
    {
        private readonly CompetitorViewModel _competitor;
        private readonly Penalty _penalty;
        private readonly PropertyInfo _propertyInfo;
        private readonly Penalty _initialPenalty;

        public PenaltyEvent(CompetitorViewModel competitor, Penalty penalty, Expression<Func<Penalty>> categoryExpression)
        {
            _competitor = competitor;
            _penalty = penalty;
            _propertyInfo = ReflectionHelper.GetProperty(categoryExpression);
            _initialPenalty = CategoryPenalty;
        }

        private Penalty CategoryPenalty
        {
            get { return (Penalty) _propertyInfo.GetValue(_competitor, null); }
            set { _propertyInfo.SetValue(_competitor, value, null); }
        }

        public override string Display
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}: {1} {2} {3}", base.Display, _competitor.Belt, _propertyInfo.Name, _penalty); }
        }

        public override void Redo()
        {
            CategoryPenalty = _penalty;
        }

        public override void Undo()
        {
            CategoryPenalty = _initialPenalty;
        }
    }
}
