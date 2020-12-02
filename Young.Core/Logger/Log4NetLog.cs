﻿//为项目注册Log4Net.config配置文件
[assembly: log4net.Config.DOMConfigurator(ConfigFile = @"Configurations\Log4Net.config", Watch = true)]

namespace Young.Core.Logger
{
    public class Log4NetLog : Young.Core.Logger.ILog
    {
        private static readonly log4net.ILog _error = log4net.LogManager.GetLogger("ExceptionLogger");

        #region Implementation of ILog

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="obj"></param>
        public void Info(object obj)
        {
            _error.Info(obj);
        }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="obj"></param>
        public void Exception(object obj)
        {
            Error(obj);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="obj"></param>
        public void Debug(object obj)
        {
            _error.Debug(obj);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="obj"></param>
        public void Warn(object obj)
        {
            _error.Warn(obj);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="obj"></param>
        public void Error(object obj)
        {
            _error.Error(obj);
        }

        #endregion
    }
}
