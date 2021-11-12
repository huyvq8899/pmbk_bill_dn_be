using BKSOFT_KYSO.Modal;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BKSOFT_KYSO
{
    public partial class frmMain : Form
    {
        // private static readonly string KEY_SPIRE_PDF = "CtOzJs2BlzPokWgBAKMfmNxjRwLa3eqzrAvKtn54UDB/dWjIyGokcs+UQuYuvMY03wX56Ox75KV+U1r5H0PR++c1zc6i8e0QIOVuhMp9Qbg5A9bJJA7e7KvC4KMINTr4jnJy/yTGFwT1aEusw144kml/6oAttwEUoXBkDPLWGOsvNgH1iTYkTGWMXEV8Or4p4t4doNsl0Z7V5qWDKwB6sD/ZiH7l/Jum27FWevOlKIa2VG1rEKjtURYukbWXeSH54IKtmn7nmr0wKwnRgdu3q60aC/PdkxC0zX75EnbU5M6fa3pplU40f3LGOWcgZ2f+8oI7qpPXJ8/s7LrsxBqpQ2YGKfKuqx5ex9ALrXgjnwjcslmXPYun7flHGIkbvBsCjCpo4Ed+M658sZTGATak6gLmftEqhJ1ZZJJKFgXE5qa/TyCY7wIq1ll+z1VNhnSBZUc1RA4TwSBcFKvrZEHlj9o1WFZ1+QqNAcnzh/n+tG48B0wHLCl6D4hroCfWMoaw/23DRxx1WuWqfkazuz2H8ga1RC2XPs83nB7CHPFNs0sT5lsKbfA3P9jgtza5CEhfjAN/3TiwEP/tvnTZY+VABK97veB77h4LEiVMfQXzKfhm9cNW4ft/ofVU2OfqZ8GjtntoZdPxp1bIwTvI98SnQi/H81w19aHwUqNECTeJBjqqHMxdVKVSBAKJL0TM7RyzoOPKS19OfURAxlEgRUqJF/BM8eU0R+UicIM2h36sTuBKO4g3H6woDMlnx0QG0nqthauTB7oK6QFTwk44UQ1kTAu8LeOJwM2xNu5MLsPmoWwDvmIaTuZIW6VUX8C285c9KkrYAf79YKA3e3yxx6SSQdN/jLbtR7MaeGpxRzX0iEbqL9sG1m5USuYVByvVKQ4ntvfCMlLmUN9UCvJ/m63K27Z2dm6fTXIe/g0smYmnvEQ3JQVnldWOi1TKOMK8RbuU5un5mQZ96pLq0Q7g0NLQZh50UMT+OjAzXHPxmXfV6/deHeE8Gbb3ZYJSg7UXW2sty86uXwkj89x5yJTaMNtm6Kh2QQugn/Vd9n8C8QReNewYxjF827FBpMp9yf+vLf2FSyA50wiA9o9luoXYgRmGuUh+g9+KMWgMK5fxQ2h3cHqADzPcwsDhVfG6HuAgt81vH/M5hFLdQztXdvRKVuYOyyTOnQz9K93LZ2EvbeWz0YByRkGxnve+K8UNo3pyNgaPGRQWr5RbeURNJ4PhmM3dB2oMkwE//+s39ccgADdEJS8s35cjRrVEGs8JicRu6mDNqJfdHUNfLmiySMjG/ePwhYkiB2WhJ9AqpY9N7eQ3TBsAMkr34olS6eSNpaE1BjgJsljB27GDnmMAXNZeifyIYpBcqu6H9SLN5pGBF9WHcPVivjdNpMUrKQ==";
        // Spire.License.LicenseProvider.SetLicenseKey(KEY_SPIRE_PDF);

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
    }
}