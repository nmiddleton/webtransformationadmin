using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace webtransformationadmin.Domain
{
    
    public class TestEvent
    {
        private string _EventType = " EventType=\"PROBLEM\"";
        private string _Severity;

        public string Feedname { get; set; }
        public string Hostname { get; set; }
        public string MESSAGE { get; set; }
        public string SEVERITY
        {
            get
            {
                return _Severity;
            }
            set
            {
                if (value == " Severity=\"CLEAR\"")
                {
                    this.EventType = " EventType=\"RESOLUTION\"";
                }
                _Severity = value;
            }
        }
        public string SEVCOLOR { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public string Environment { get; set; }
        public string Status { get; set; }
        public string INF { get; set; }
        public string CAP { get; set; }
        public string LSG { get; set; }
        public string EventType  {
            // Information, Problem, Resolution
            get 
            {
                return _EventType;
            }
            set
            {
                if (this.SEVERITY == " Severity=\"CLEAR\"")
                {
                    _EventType = " EventType=\"RESOLUTION\"";
                }
                else
                {
                    _EventType = value;
                }
            }
        }  
        public string EventGroup { get; set; }
        public string EventKey { get; set; }
        public string EventClass { get; set; }
        public string EventSource { get; set; }
    }
}
