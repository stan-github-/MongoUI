using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBUI.Queries
{

    public class QueryExecutionConfiguration {

        public MongoXMLRepository MongoXMLRepository { get; set; }
        
        public QueryExecutionConfiguration()
        {
            this.MongoXMLRepository = Program.JsEngine.MongoXMLRepository;
        }

        public bool NoWindows { get; set; }

        //overrides server configuration
        //for querying collection names
        public bool NoConfirmation { get; set; }

        public bool NoFeedBack
        {
            get { return NoWindows && NoConfirmation; }
            set { NoWindows = true; NoConfirmation = true; }
        }

        public bool ContinueWithExecutionAfterWarning()
        {
            if (NoConfirmation)
            {
                return true;
            }

            var serverName = this.MongoXMLRepository.CurrentServer.Name;

            if (!MongoXMLRepository.Servers.First(s => s.Name == serverName).WithWarning)
            {
                return true;
            }

            if (MessageBox.Show("Continue query with " + serverName, 
                "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                return true;
            }

            return false;
        }
    }
}
