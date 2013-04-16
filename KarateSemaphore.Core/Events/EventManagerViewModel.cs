using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KarateSemaphore.Core.Events
{
    public class EventManagerViewModel : ViewModelBase, IEventManager
    {
        private readonly ObservableCollection<IEvent> _events;
        private readonly ReadOnlyObservableCollection<IEvent> _eventsPublic;
        private readonly object _syncRoot;
        private readonly RelayCommand<int> _step;
        private int _current;

        public ICollection<IEvent> Events { get { return _eventsPublic; } }

        public int Position { get { return _current - 1; } }

        public EventManagerViewModel()
        {
            _syncRoot = new object();
            _events = new ObservableCollection<IEvent>();
            _eventsPublic = new ReadOnlyObservableCollection<IEvent>(_events);
            _current = 0;
            _step = new RelayCommand<int>(DoStep);
        }

        public void AddAndExecute(IEvent @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException("event");
            }
            lock (_syncRoot)
            {
                while (_events.Count != _current)
                {
                    _events.RemoveAt(_current);
                }
                _events.Insert(_current++, @event);
                @event.Redo();
            }
            OnPropertyChanged(() => Position);
        }

        public void Redo(int steps)
        {
            lock (_syncRoot) {
                var count = 0;
                foreach (var item in _events.Skip(_current).Take(steps)) {
                    item.Redo();
                    count++;
                }
                _current += count;
            }
            OnPropertyChanged(() => Position);
        }


        public void Undo(int steps)
        {
            lock (_syncRoot)
            {
                var skip = _events.Count - _current;
                var count = 0;
                foreach (var item in _events.Reverse().Skip(skip).Take(steps)) {
                    item.Undo();
                    count++;
                }
                _current -= count;
            }
            OnPropertyChanged(() => Position);
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _events.Clear();
                _current = 0;
            }
            OnPropertyChanged(() => Position);
        }

        public RelayCommand<int> Step { get { return _step; } }

        private void DoStep(int obj)
        {
            if (obj > 0)
            {
                Redo(obj);
            }
            else
            {
                Undo(obj * -1);
            }
        }
    }
}