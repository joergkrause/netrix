using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetrixHtmlEditorDemo {
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
      Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      try {
        Application.Run(new MainForm());
      }
      catch (Exception) {
      }
    }

    static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {

    }
  }
}
