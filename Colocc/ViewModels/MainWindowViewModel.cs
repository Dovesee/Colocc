using Microsoft.Win32;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace Colocc.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "ColoccWorkTime";
        private uint _workTime = 30;
        private uint _freeTime = 5;
        private bool _isOpenRun = true;
        private bool _freeFlag=false;
        private string _message;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly IDialogService _dialogService;
        private readonly ILog _log;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public uint WorkTime
        {
            get { return _workTime; }
            set { SetProperty(ref _workTime, value); }
        }
        public uint FreeTime
        {
            get { return _freeTime; }
            set 
            { 
                SetProperty(ref _freeTime, value);
            }
        }
        public bool IsOpenRun
        {
            get { return _isOpenRun; }
            set { SetProperty(ref _isOpenRun, value); OpenAotuRun(value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        public MainWindowViewModel(IDialogService dialogService,ILog log)
        {
            _log = log;// new LogTextFileApplication(_title) { InfoIsEnable=true};
            _dialogService = dialogService;
            OpenAotuRun(true);
            Thread thread = new Thread(Show);
            thread.IsBackground = true;
            thread.Start();
            _stopwatch.Restart();
        }
        private void Show()
        {
            while (true)
            {
                if (_stopwatch.ElapsedMilliseconds > _workTime * 60000)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _dialogService.ShowDialog("MessageDialog", new DialogParameters() { { "message", "！！！起立--运动！！！" } }, ShowWorkShutDown);
                    });
                    while (_freeFlag)
                    {
                        if (_stopwatch.ElapsedMilliseconds > _freeTime * 60000)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                _dialogService.ShowDialog("MessageDialog", new DialogParameters() { { "message", " 开始工作 " } }, OpenShowWork);
                            });
                        }
                        Thread.Sleep(2000);
                        Message = "休息了" + Math.Round(_stopwatch.ElapsedMilliseconds / 60000.0,1)+"分钟";
                    }
                }
                Thread.Sleep(2000);
                Message = "工作了" + Math.Round(_stopwatch.ElapsedMilliseconds / 60000.0, 1) + "分钟";
            }
        }
        private void OpenShowWork(IDialogResult obj)
        {
            if (obj.Result == ButtonResult.Yes)
            {
                _log.Info("开始工作，此次休息" + _stopwatch.ElapsedMilliseconds / 60000 + "分钟");
                _stopwatch.Restart(); 
                _freeFlag = false;
            }
        }
        private void ShowWorkShutDown(IDialogResult obj)
        {
            if(obj.Result== ButtonResult.Yes)
            {
                _freeFlag = true;
                _log.Info("开始休息，此次工作"+_stopwatch.ElapsedMilliseconds/60000+"分钟");
                _stopwatch.Restart();
            }
        }
        private void OpenAotuRun(bool auto)
        {
            //设置开机自启动  
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            rk2.SetValue("JcShutdown", auto ? path : "false");
            rk2.Close();
            rk.Close();
        }
    }

    public interface ILog
    {
        EnumLogLevel EnumLogLevel
        {
            get;
            set;
        }
        bool TraceIsEnable
        {
            get;
            set;
        }

        bool DebugLogIsEnable
        {
            get;
            set;
        }

        bool InfoIsEnable
        {
            get;
            set;
        }

        bool WarnIsEnable
        {
            get;
            set;
        }

        bool ErrorIsEnable
        {
            get;
            set;
        }

        bool FatalIsEnable
        {
            get;
            set;
        }

        void Trace(object message);

        void DebugLog(object message);

        void Info(object message);

        void Warn(object message);

        void Error(object message);

        void Fatal(object message);

    }
    public enum EnumLogLevel : byte
    {
        ALL,
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
        OFF,
        Undefine
    }
    public class LogTextFileApplication : ILog
    {
        private string _LogPath;
        private readonly object _lock = new object();
        public bool TraceIsEnable
        {
            get;
            set;
        }
        public bool DebugLogIsEnable
        {
            get;
            set;
        }
        public bool InfoIsEnable
        {
            get;
            set;
        }
        public bool WarnIsEnable
        {
            get;
            set;
        }
        public bool ErrorIsEnable
        {
            get;
            set;
        }
        public bool FatalIsEnable
        {
            get;
            set;
        }
        public EnumLogLevel EnumLogLevel
        {
            get;
            set;
        }

        public LogTextFileApplication(string path)
        {
            InitLoggerSettings(path);
        }
        private void InitLoggerSettings(string path)
        {
            try
            {

                long _maxSize = 1024 * 1024;//1兆
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                _LogPath = Path.Combine(logDir, path + ".log");

                if (File.Exists(_LogPath))
                {
                    FileInfo fi = new FileInfo(_LogPath);
                    if (fi.Length >= _maxSize)
                    {    //文件大小超过1M，则备份后删除
                        string strDate = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString() + "-";
                        string logBakPath = Path.Combine(logDir, strDate + path + ".log");
                        fi.CopyTo(logBakPath, true);
                        fi.Delete();
                    }
                }
            }
            catch
            {
            }
        }

        public void Trace(object message)
        {
            WriteLog(EnumLogLevel.Trace, message);
        }

        public void DebugLog(object message)
        {
            WriteLog(EnumLogLevel.Debug, message);
        }

        public void Info(object message)
        {
            WriteLog(EnumLogLevel.Info, message);
        }

        public void Warn(object message)
        {
            WriteLog(EnumLogLevel.Warn, message);
        }

        public void Error(object message)
        {
            WriteLog(EnumLogLevel.Error, message);
        }

        public void Fatal(object message)
        {
            WriteLog(EnumLogLevel.Fatal, message);
        }

        private void Write(EnumLogLevel enumLogLevel, object message)
        {
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(_LogPath, DateTime.Now.ToString() + ":" + enumLogLevel.ToString() + ":\r\n" + message + "\r\n", System.Text.Encoding.Default);
                }
                catch
                {
                }
            }
        }

        private void WriteLog(EnumLogLevel enumLogLevel, object message)
        {
            if ((int)enumLogLevel >= (int)EnumLogLevel)
            {
                switch (enumLogLevel)
                {
                    case EnumLogLevel.ALL:
                        if (TraceIsEnable)
                        {
                            Write(enumLogLevel, message);
                        }
                        break;
                    case EnumLogLevel.Debug:
                        if (DebugLogIsEnable)
                        {
                            Write(enumLogLevel, message);
                        }
                        break;
                    case EnumLogLevel.Info:
                        if (InfoIsEnable)
                        {
                            Write(enumLogLevel, message);
                        }
                        break;
                    case EnumLogLevel.Warn:
                        if (WarnIsEnable)
                        {
                            Write(enumLogLevel, message);
                        }
                        break;
                    case EnumLogLevel.Error:
                        if (ErrorIsEnable)
                        {
                            Write(enumLogLevel, message);
                        }
                        break;
                    case EnumLogLevel.Fatal:
                        if (FatalIsEnable)
                        {
                            Write(enumLogLevel, message);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("enumLogLevel");
                }
            }
        }
    }
}
