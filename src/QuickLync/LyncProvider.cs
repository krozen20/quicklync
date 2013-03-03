using Microsoft.Lync.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickLync
{
    public class LyncProvider : IPluginProvider
    {

        const string PLUGIN_NAME = "QuickLync";

        List<LyncContact> _contacts = null;

        public List<LyncContact> Contacts
        {
            get
            {
                if (_contacts == null)
                {
                    _contacts = OnGetContacts();
                }

                return _contacts;
            }
            set { _contacts = value; }
        }

        protected virtual List<LyncContact> OnGetContacts()
        {
            var contacts = new List<LyncContact>();

            var client = LyncClient.GetClient();

            foreach (var group in client.ContactManager.Groups)
            {
                foreach (var contact in group)
                {
                    if (!contacts.Exists(c => c.Uri == contact.Uri))
                    {
                        var name = contact.GetContactInformation(ContactInformationType.DisplayName).ToString();
                        contacts.Add(new LyncContact() { Name = name, Uri = contact.Uri });
                    }
                }
            }

            return contacts;
        }

        public string Name
        {
            get { return PLUGIN_NAME; }
        }

        public List<PluginItem> Search(string query)
        {
            List<PluginItem> results = new List<PluginItem>();

            var filteredContacts = Contacts.Where(c => c.Name.ToLower().Contains(query.ToLower())).ToList();

            foreach (var result in filteredContacts)
            {
                results.Add(new PluginItem() { Uri = result.Uri, Name = result.Name, Icon = OnGetIcon(result.Uri) });
            }

            return results;
        }

        protected virtual string OnGetIcon(string p)
        {
            return null;
        }

        public void Refresh()
        {
            _contacts = null;
        }
    }
}
