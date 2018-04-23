using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.Module.BusinessObjects {
    [DefaultClassOptions]
    public class ExampleObject : BaseObject {
        private string name;
        private ReferencedObject lookupReferencedObject;

        public ExampleObject(Session session)
            : base(session) {
        }

        public string Name {
            get { return name; }
            set { SetPropertyValue("Name", ref name, value); }
        }

        public ReferencedObject LookupReferencedObject {
            get { return lookupReferencedObject; }
            set { SetPropertyValue("LookupReferencedObject", ref lookupReferencedObject, value); }
        }
    }
    [DefaultClassOptions]
    public class ReferencedObject : XPObject {
        private string name;
        public ReferencedObject(Session session) : base(session) { }
        public string Name {
            get { return name; }
            set { SetPropertyValue("Name", ref name, value); }
        }
    }
}
