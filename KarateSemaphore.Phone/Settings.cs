using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;

namespace KarateSemaphore.Phone
{
    public class Settings
    {
        // Our isolated storage settings
        private readonly IsolatedStorageSettings _isolatedStore;
        
        ///
        /// Constructor that gets the application settings. /// 


        public Settings()
        {
            // Get the settings for this application.
            _isolatedStore = IsolatedStorageSettings.ApplicationSettings;
        }

        public TimeSpan MatchTime
        {
            get { return GetValueOrDefault(TimeSpan.FromMinutes(3), "MatchTime"); }
            set { AddOrUpdateValue(value, "MatchTime"); }
        }

        public bool FlipOrientation
        {
            get { return GetValueOrDefault(false, "FlipOrientation"); }
            set { AddOrUpdateValue(value, "FlipOrientation"); }
        }


        private void AddOrUpdateValue(object value, string key)
        {
            var hasKey = _isolatedStore.Contains(key);
            if (hasKey && _isolatedStore[key] == value)
                return; // value hasn't changed
            if (hasKey)
                _isolatedStore[key] = value;
            else
                _isolatedStore.Add(key, value);

            Save();
        }

        private TVal GetValueOrDefault<TVal>(TVal defaultValue, string key)
        {
            return _isolatedStore.Contains(key) 
                ? (TVal)_isolatedStore[key] 
                : defaultValue;
        }

        private void Save()
        {
            try
            {
                _isolatedStore.Save();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
            }
        }
    }
}