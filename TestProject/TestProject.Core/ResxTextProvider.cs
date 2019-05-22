using MvvmCross.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;

namespace TestProject.Core
{
    public class ResxTextProvider
        : IMvxTextProvider
    {
        private readonly ResourceManager _resourceManager;

        public ResxTextProvider(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            CurrentLanguage = Thread.CurrentThread.CurrentUICulture;
        }

        public CultureInfo CurrentLanguage { get; set; }

        public string GetText(string namespaceKey, string typeKey, string name)
        {
            string resolvedKey = name;

            if (!string.IsNullOrEmpty(typeKey))
            {
                resolvedKey = string.Format("{0}.{1}", typeKey, resolvedKey);
            }

            if (!string.IsNullOrEmpty(namespaceKey))
            {
                resolvedKey = string.Format("{0}.{1}", namespaceKey, resolvedKey);
            }

            return _resourceManager.GetString(resolvedKey, CurrentLanguage);
        }

        public string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            string baseText = GetText(namespaceKey, typeKey, name);

            if (string.IsNullOrEmpty(baseText))
            {
                return baseText;
            }

            return string.Format(baseText, formatArgs);
        }

        public bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name)
        {
            throw new NotImplementedException();
        }

        public bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            throw new NotImplementedException();
        }
    }
}
