using System;
using System.Xml.Linq;

namespace Orchard.ContentManagement.Handlers {
    public class ImportContentContext : ContentContextBase {
        public XElement Data { get; set; }
        private ImportContentSession Session { get; set; }


        public ImportContentContext(ContentItem contentItem, XElement data, ImportContentSession importContentSession)
            : base(contentItem) {
            Data = data;
            Session = importContentSession;
        }

        public string Attribute(string elementName, string attributeName) {
            var element = Data.Element(elementName);
            if (element != null) {
                var attribute = element.Attribute(attributeName);
                if (attribute != null)
                    return attribute.Value;
            }
            return null;
        }

        public void ImportAttribute(string elementName, string attributeName, Action<string> value) {
            ImportAttribute(elementName, attributeName, value, () => { });
        }

        public void ImportAttribute(string elementName, string attributeName, Action<string> value, Action empty) {
            var importedText = Attribute(elementName, attributeName);
            if (importedText != null) {
                try {
                    value(importedText);
                }
                catch {
                    empty();
                }
            }
            else {
                empty();
            }
        }

        public ContentItem GetItemFromSession(string id) {
            return Session.Get(id);
        }

    }
}