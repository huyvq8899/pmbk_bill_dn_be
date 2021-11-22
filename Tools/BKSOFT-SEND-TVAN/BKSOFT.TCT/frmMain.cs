using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace BKSOFT.TCT
{
    public partial class frmMain : Form
    {
        private ServerSetting settings = new ServerSetting();

        private static Server _server;

        private ContextMenu trayMenu;

        private uint m_total_elapse_time = 0;

        private uint m_sec;

        private uint m_min;

        private uint m_hour;

        private uint m_day;

        private int pre_day;

        public frmMain()
        {
            InitializeComponent();

            //// Add to startup
            //RegisterInStartup(true);

            //// Check process exist
            //string proName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);
            //if (Process.GetProcessesByName(proName).Count() > 1)
            //{
            //    Process.GetCurrentProcess().Kill();
            //}

            //// Start socket
            //_server = new Server(this.GetSettings());
            //_server.Start();

            // Tray menu
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            notifyIcon.ContextMenu = trayMenu;
            notifyIcon.BalloonTipText = $"{this.Text} 1.0 ĐANG CHẠY";

            // Thread queue Out
            ThreadQueueTCT pthread = new ThreadQueueTCT();
            pthread.Start();
        }

        #region Notify 
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
                GPSFileLog.WriteLog(string.Empty, ex);
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
                GPSFileLog.WriteLog(string.Empty, ex);
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
                GPSFileLog.WriteLog(string.Empty, ex);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (_server != null)
            {
                _server.Dispose();
            }

            // Stop timer
            timer.Stop();

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
                GPSFileLog.WriteLog(string.Empty, ex);
            }
        }

        #endregion

        private void OnTimerTick(object sender, EventArgs e)
        {
            try
            {
                //////////////////////////////////////////////////////////////////////////////////////////
                m_total_elapse_time += 1;
                m_sec = m_total_elapse_time % 60;
                m_min = ((m_total_elapse_time / 60) % 60);
                m_hour = ((m_total_elapse_time / 3600) % 24);
                m_day = m_total_elapse_time / 86400;
                string sClock = string.Empty;
                if (m_day == 0)
                {
                    sClock = string.Format("{0:00}:{1:00}:{2:00}", m_hour, m_min, m_sec);
                }
                else
                {
                    sClock = string.Format("{0:00} days {1:00}:{2:00}:{3:00}", m_day, m_hour, m_min, m_sec);
                }
                UIInvokeUtil.InvokeMessageLableText(lbClock, sClock);

                //// Get password
                //if(pre_day != DateTime.Now.Day)
                //{
                //    pre_day = DateTime.Now.Day;             // Re-day

                //    // Reset counter convertion
                //    _server.NumConvertSuccess = 0;
                //    _server.NumConvertError = 0;

                //    //// Get invoice use
                //    //ThreadStatistic pthread = new ThreadStatistic();
                //    //pthread.DateTime = DateTime.Now;
                //    //pthread.Start();

                //    // Write log success & error
                //    GPSFileLog.WriteLog($"NumConvertSuccess = {_server.NumConvertSuccess}, NumConvertError = {_server.NumConvertError}");
                //}    

                //// Get number client
                //int iClients = _server.GetNumberGPSConnected();
                //UIInvokeUtil.InvokeMessageLableText(lblGPSConnecting, $"{iClients}");

                //// Get number convertion
                //UIInvokeUtil.InvokeMessageLableText(lbResult, $"{_server.NumConvertSuccess}  -  {_server.NumConvertError}");
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }
        }

        private ServerSetting GetSettings()
        {
            try
            {
                // Get server information
                settings.ServerListenerIP = ConfigurationManager.AppSettings[Constants.SERVER_LISTENER_IP];
                settings.ServerListenerPort = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SERVER_LISTENER_PORT]);

                // Queue 
                (settings.RabbitQueueCfg).UserName = ConfigurationManager.AppSettings[Constants.TCT_USER_NAME];
                (settings.RabbitQueueCfg).Password = ConfigurationManager.AppSettings[Constants.TCT_PASS_WORD];
                (settings.RabbitQueueCfg).HostName = ConfigurationManager.AppSettings[Constants.TCT_HOST_NAME];
                (settings.RabbitQueueCfg).Port = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.TCT_PORT]);
                (settings.RabbitQueueCfg).Exchange = ConfigurationManager.AppSettings[Constants.TCT_EXCHANGE];
                (settings.RabbitQueueCfg).RoutingKeyIn = ConfigurationManager.AppSettings[Constants.TCT_ROUTING_KEY_IN];
                (settings.RabbitQueueCfg).QueueNameIn = ConfigurationManager.AppSettings[Constants.TCT_QUEUE_IN];
                (settings.RabbitQueueCfg).RoutingKeyOut = ConfigurationManager.AppSettings[Constants.TCT_ROUTING_KEY_OUT];
                (settings.RabbitQueueCfg).QueueNameOut = ConfigurationManager.AppSettings[Constants.TCT_QUEUE_OUT];

                // Set Name App.
                this.Text = string.Format("TCT TRANFER {0}", settings.ServerListenerPort);
                txtMainServer.Text = settings.ServerListenerIP;
                txtMainPort.Text = settings.ServerListenerPort.ToString();
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return settings;
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

        #region Thread Queue In & Out
        private void ReadDataTCTQueueOut()
        {
            throw new NotImplementedException();
        }

        private void ReadDataTCTQueueIn()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
