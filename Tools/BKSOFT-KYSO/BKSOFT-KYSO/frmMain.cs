using BKSOFT_KYSO.Modal;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

namespace BKSOFT_KYSO
{
    public partial class frmMain : Form
    {
        public Setting Setting { set; get; }

        public Firewall Firewall { set; get; }

        private ServerSocket serverSocket;

        private ContextMenu trayMenu;

        public frmMain()
        {
            InitializeComponent();

            // Remove old version
            RemoveOldVersion();

            // Update new version
            UpdateVersion();

            // Add to startup
            RegisterInStartup(true);

            // Check process exist
            string proName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (Process.GetProcessesByName(proName).Count() > 1)
            {
                Process.GetCurrentProcess().Kill();
            }

            // Remove old log
            string fullPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Utils.ClearFolder($"{fullPath}\\log");

            // Fire wall grant
            FirewallGrantAuthorization();

            // Run server socket
            serverSocket = new ServerSocket();
            serverSocket.Start(Setting);

            // Tray menu
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            notifyIcon.ContextMenu = trayMenu;
            notifyIcon.BalloonTipText = $"{this.Text} {Setting.Version} đã chạy";

            // Delete file version
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}ver.dat";
            if (File.Exists(path) && !Convert.ToBoolean(ConfigurationManager.AppSettings["NO_ALWAYS_UPDATE"]))
            {
                File.Delete(path);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Visible = false;        // Hide form window.
                ShowInTaskbar = false;  // Remove from taskbar.
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                Hide();
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(100);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    Hide();
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(100);
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private void FirewallGrantAuthorization()
        {
            try
            {
                // Get the current process.
                Firewall = new Firewall();
                Process current = Process.GetCurrentProcess();
                Firewall.GrantAuthorization(current.MainModule.FileName, current.MainModule.ModuleName);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private VersionInfo GetVerionUpdate()
        {
            VersionInfo ver = null;
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string url = Utils.Base64Decode(Setting.URL);

                    var json = wc.DownloadString(url);

                    ver = JsonConvert.DeserializeObject<VersionInfo>(json);
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return ver;
        }

        private Setting GetCurrentVer()
        {
            Setting setting = null;

            try
            {
                string path = $"{AppDomain.CurrentDomain.BaseDirectory}ver.dat";

                // Check exit
                if (!File.Exists(path))
                {
                    setting = new Setting
                    {
                        Port = Constants.SETTING_PORT,
                        URL = Constants.SETTING_URL,
                        Date = Constants.SETTING_DATE,
                        Version = Constants.SETTING_VERSION,
                    };
                    string json = JsonConvert.SerializeObject(setting);
                    File.WriteAllText(path, json);
                }
                else
                {
                    string json = File.ReadAllText(path);
                    setting = JsonConvert.DeserializeObject<Setting>(json);
                }
            }
            catch (Exception)
            {
            }

            return setting;
        }

        private void UpdateVersion()
        {
            try
            {
                // Get current version
                Setting = GetCurrentVer();
                if (this.lbVersion.InvokeRequired)
                {
                    lbVersion.Invoke(new MethodInvoker(delegate { lbVersion.Text = Setting.Version; }));
                }
                else
                {
                    lbVersion.Text = Setting.Version;
                }

                if (this.lbVersionDate.InvokeRequired)
                {
                    lbVersionDate.Invoke(new MethodInvoker(delegate { lbVersionDate.Text = Setting.Date; }));
                }
                else
                {
                    lbVersionDate.Text = Setting.Date;
                }

                // Check version update
                VersionInfo info = GetVerionUpdate();
                if (info != null && !Setting.Version.Contains(info.Version))
                {
                    string pathZip = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.BKSOFT_KYSO_ZIP);

                    // Update verion
                    using (WebClient web = new WebClient())
                    {
                        web.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                        web.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");

                        web.DownloadFile(new Uri(info.Link), pathZip);

                        // Check file exist.
                        if (File.Exists(pathZip))
                        {
                            string agr1 = Utils.Base64Encode(Process.GetCurrentProcess().ProcessName);
                            string agr2 = Utils.Base64Encode(AppDomain.CurrentDomain.BaseDirectory);
                            string agr3 = Utils.Base64Encode(Constants.BKSOFT_KYSO_ZIP);
                            string agr4 = Utils.Base64Encode(JsonConvert.SerializeObject(info));

                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.KYSO_UPDATE_EXE);
                            process.StartInfo.Arguments = $"{agr1} {agr2} {agr3} {agr4}";
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            process.Start();
                            process.WaitForExit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private void RemoveOldVersion()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.STARTUP_KEY, true);
                if (key.GetValue(Constants.STARTUP_OLD_VALUE) != null)
                {
                    // Get value registry
                    string s_path = key.GetValue(Constants.STARTUP_OLD_VALUE).ToString();

                    // Get process with name
                    Process[] pros = Process.GetProcessesByName(Constants.STARTUP_OLD_VALUE);
                    foreach (var pro in pros)
                    {
                        pro.Kill();
                    }

                    // Check delete all file in directory
                    string pathDir = Path.GetDirectoryName(s_path);
                    bool res = Utils.RemoveDirectory(pathDir);
                    if (res)
                    {
                        Directory.Delete(pathDir, true);
                    }

                    // Delete key
                    key.DeleteValue(Constants.STARTUP_OLD_VALUE);
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private void RegisterInStartup(bool isChecked)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.STARTUP_KEY, true);
            if (key.GetValue(Constants.STARTUP_VALUE) == null)
            {
                string s_path = Application.ExecutablePath.ToString();

                key.SetValue(Constants.STARTUP_VALUE, s_path);
            }
            else if (isChecked)
            {
                // Get value registry
                string s_value = key.GetValue(Constants.STARTUP_VALUE).ToString();

                // Get path execute
                string s_path = Application.ExecutablePath.ToString();

                // Check to re-set registry
                if (s_value != s_path)
                {
                    key.SetValue(Constants.STARTUP_VALUE, s_path);
                }
            }
            else if (!isChecked)
            {
                key.DeleteValue(Constants.STARTUP_VALUE);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (serverSocket != null)
            {
                serverSocket.Stop();
            }

            Environment.Exit(0);
        }

        private void OnNotifyIconMouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Show();
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                notifyIcon.Visible = false;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private void btXML_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get full path xml
                    string xmlPath = openFileDialog.FileName;

                    // Sign
                    Handler.SignXMLFormPath(xmlPath);  
                }
            }
        }
    }
}